namespace VDB.MicroServices.Auth.Concern.Options
{
    public class LDAPSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string DC { get; set; }
        public string UserDNTemplate { get; set; }
        public string OuRegexExtractor { get; set; }
        public string OuRegexLeftPart { get; set; }
        public string OuRegexRightPart { get; set; }
        public string UIDSearchTemplate { get; set; }
        public int UIDSearchScope { get; set; }
    }
}
