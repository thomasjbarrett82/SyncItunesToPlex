namespace Core.Dto
{
    public class PlexServerResponse
    {
        public required ServerMediaContainer MediaContainer { get; set; }
    }

    public class ServerMediaContainer
    {
        public int Size { get; set; }
        public bool AllowSync { get; set; }
        public required string CountryCode { get; set; }
        public required string FriendlyName { get; set; }
        public bool HubSearch { get; set; }
        public bool Multiuser { get; set; }
        public required string MachineIdentifier { get; set; }
        public bool MyPlex { get; set; }
        public required string Platform { get; set; }
        public required string PlatformVersion { get; set; }
        public required string Version { get; set; }
    }
}

