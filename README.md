# SyncItunesToPlex

## Description

This is a basic console app that will synchronize an iTunes library's metadata with a Plex library. Specifically, it syncs the playlists and ratings according to the configuration.

## Usage

Run the app to create the config.json file at the executable's location. It will stop with the message:

> iTunes library path not set

Open the config file and set the iTunes library XML location, Plex base API URL, and Plex API token, then save the file. 

Run the app again to retrieve and populate the config list of iTunes playlists. Each playlist will sync by default, so set **MustSync = false** to stop this behavior.

The app will also populate the Plex library sections config. Find the music library to sync iTunes against and set **IsSelected = true**.

## Configuration

* ItunesLibraryPath
  * File location of the iTunes library XML file.
  * Windows default location: C:\Users\\%user%\\Music\iTunes\iTunes Music Library.xml
* ItunesPlaylists
  * List of iTunes playlists. Each item has the attribute **MustSync**, which is a flag for synchronizing the playlist. True will sync, false will not. 
* PlexApiBaseUrl
  * Base URL for the Plex server, in the format **http://serverIP:port/**
* PlexApiToken
  * Instructions for finding the Plex API token can be found [here](https://support.plex.tv/articles/204059436-finding-an-authentication-token-x-plex-token/).
* PlexLibrarySections
  * List of Plex music libraries. Set **IsSelected = true** on the library to sync.
* MustSyncRatings
  * Flag for synchronizing ratings. True will sync, false will not.
* MustSyncPlaylists
  * Flag for synchronizing playlists. True will sync, false will not.

## Publish

Use the following command to create the publish file:
` dotnet publish -c Release -r win-x64 `
