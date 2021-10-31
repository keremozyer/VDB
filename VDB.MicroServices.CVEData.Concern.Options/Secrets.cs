namespace VDB.MicroServices.CVEData.Concern.Options
{
    public class Secrets
    {
        public NVDSecrets NVD { get; set; }
    }

    public class NVDSecrets
    {
        public string APIKey { get; set; }
    }
}
