namespace Core.Dto
{
    public class PlexServerResponse
    {
        public ServerMediaContainer MediaContainer { get; set; }
    }

    public class ServerMediaContainer
    {
        public int size { get; set; }
        public bool allowSync { get; set; }
        public string countryCode { get; set; }
        public string friendlyName { get; set; }
        public bool hubSearch { get; set; }
        public bool multiuser { get; set; }
        public string machineIdentifier { get; set; }
        public bool myPlex { get; set; }
        public string platform { get; set; }
        public string platformVersion { get; set; }
        public string version { get; set; }
    }
}

