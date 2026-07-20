# CLAUDE.md

## Overview

Console app one-way syncs iTunes library metadata (playlists and ratings) **into** Plex. iTunes is the source of truth; Plex is written to. Reads the iTunes library XML (never modified) and drives changes through the Plex HTTP API.

## Build / Test / Run

```powershell
dotnet build                                              # build the solution
dotnet test                                               # run all tests
dotnet test --filter FullyQualifiedName~HelpersTest       # single test class
dotnet test --filter Name=RoundTo20Tests                  # single test method
dotnet run --project SyncItunesToPlexConsole              # run the console app
dotnet publish -c Release -r win-x64                      # produce a Windows self-contained publish
```

- Targets **net10.0**. `ImplicitUsings` and `Nullable` are enabled in all projects.
- Three projects: `Core` (all logic), `SyncItunesToPlexConsole` (thin entry point), `Test` (MSTest).

## Test structure

- **UnitTests** (`Test/UnitTests/`) run without external dependencies — safe to run anywhere.
- **IntegrationTests** (`Test/IntegrationTests/`) hit a live Plex server and a real iTunes XML file. They read config from **.NET user secrets** (keys: `plexApiBaseUrl`, `plexApiToken`, `plexMusicSectionId`, plus iTunes path keys). Without secrets configured these tests fail/throw — expect that when running the full suite in a bare environment. Set secrets with `dotnet user-secrets` in the `Test` project.

## Runtime configuration (the app, not tests)

- On first run the app writes `config.json` next to the executable and exits with a message telling the user what to fill in. It is designed to be run repeatedly, each run auto-populating the next section of config (iTunes playlists, then Plex library sections) for the user to edit.
- The config-bootstrap flow lives entirely in `Program.cs` (`VerifyAppConfig` → `SetupItunesConfig` / `SetupPlexConfig`). `ExitApp` prints a message and calls `Environment.Exit(0)` — it does not throw, so control-flow after an `ExitApp` call is not reached.
- `example config.json` documents the shape; the README documents each field.

## Architecture

Flow (all orchestrated in `SyncItunesToPlexConsole/Program.cs`):

1. **`Itunes`** (`Core/Services/Itunes.cs`) wraps the `iTunesLibraryParser` NuGet package and reads tracks/playlists from the XML. Only tracks whose `Kind` contains "audio" are included.
2. **`Plex`** (`Core/Services/Plex.cs`) is the Plex API client, organized into Get/Put/Post/Delete regions. It builds `RestApiRequest`s and calls `IRestApiService`.
3. **`Matcher`** (`Core/Services/Matcher.cs`) pairs each iTunes track to exactly one Plex track. Ambiguous (>1) or missing (0) matches are skipped (currently `Console.WriteLine`, with `// TODO log it`). Everything downstream operates on the `List<(ItunesTrack, PlexTrack)>` it returns.
4. `Program` then syncs playlists and/or ratings depending on `MustSyncPlaylists` / `MustSyncRatings`.

### Track matching (the core subtlety)

Tracks are matched **by file path components, not by title/artist tags** — see `ItunesTrack.EqualsPlexTrack`, which compares `ShortFileName` + `ShortAlbumName` + `ShortArtistName` (the last three path segments: artist/album/file), case-insensitive.

`Core/Services/Extensions.cs` derives these "short" names by splitting the file path on `/`. Because iTunes and Plex encode/escape filesystem characters differently, `MakePlexMatchItunesNaming` normalizes Plex-side strings to match iTunes' encoding conventions (specific char substitutions + `Uri.EscapeDataString` + selectively un-escaping certain codes). **If you change this normalization, the corresponding assertions in `Test/UnitTests/HelpersTest.cs` (`ItunesCharacterEncodingTests`) must be kept in sync** — they pin exact expected encodings for a large set of characters.

### Ratings

- iTunes ratings are 0–100; Plex ratings are 0–10. Track ratings are converted by dividing by 10.
- Album and artist ratings are computed by **averaging** the matched track ratings within each Plex `parentRatingKey` / `grandparentRatingKey` group, snapped to the nearest 20 via `Helpers.RoundTo20`, then `/10`. `RoundTo20` behavior is pinned by `HelpersTest.RoundTo20Tests`.
- `Ratings` enum (`Core/Ratings.cs`) enumerates valid Plex 0–10 values (even numbers only); `UpdateItemRating` validates against it.

### Data layer

- `IRestApiService` / `RestApiService` (`Core/Data/`) wrap RestSharp and centralize error handling: any non-successful HTTP response throws `InvalidOperationException`. Deserialization uses `System.Text.Json`.
- DTOs in `Core/Dto/` mirror Plex's `MediaContainer` JSON envelope. String constants for endpoints/keys/values live in `Core/Data/ApiEndpoints.cs`, `ApiKeys.cs`, `ApiValues.cs` and `Core/Constants.cs`.
- Note property naming in DTOs/`PlexTrack` intentionally uses Plex's lowercase JSON names (`ratingKey`, `parentRatingKey`, etc.) to match the wire format.

## Gotchas

- `Plex.GetTracks` writes a debug dump to `C:\Temp\PlexTracks.txt` **only in DEBUG builds** (`#if DEBUG` + `Extensions.SaveToTempFile`). The `C:\Temp` directory must exist or this throws in Debug.
- `AppConfig.SaveConfigASync` deletes then recreates the file each save; several `Program` config-bootstrap methods are `async void`.
- Playlist IDs from iTunes can change between runs, so `SyncCurrentItunesPlaylistsWithConfig` re-matches config playlists to current ones **by Name** and carries the `MustSync` flag forward.
