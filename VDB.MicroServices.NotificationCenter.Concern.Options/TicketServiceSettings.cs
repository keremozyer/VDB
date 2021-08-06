namespace VDB.MicroServices.NotificationCenter.Concern.Options
{
    public class TicketServiceSettings
    {
        public string SearchWithCVEEndpoint { get; set; }
        public string CreateIssueEndpoint { get; set; }
        public CustomFieldIDData FieldIDs { get; set; }
        public DefaultFieldValueData DefaultFieldValues { get; set; }
    }

    public class CustomFieldIDData
    {
        public string Project { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string IssueType { get; set; }
        public string CVEID { get; set; }
        public string Vendor { get; set; }
        public string Product { get; set; }
        public string Version { get; set; }
        public string Fields { get; set; }
    }

    public class DefaultFieldValueData
    {
        public string ProjectKey { get; set; }
        public string IssueTypeName { get; set; }
    }
}
