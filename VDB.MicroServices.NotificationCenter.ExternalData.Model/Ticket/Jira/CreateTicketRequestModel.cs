using System.Collections.Generic;

namespace VDB.MicroServices.NotificationCenter.ExternalData.Model.Ticket.Jira
{
    public record CreateTicketRequestModel
    {
        public CreateTicketRequestFieldData Fields { get; set; }
    }

    public class CreateTicketRequestFieldData
    {
        public CreateTicketRequestProjectData Project { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public CreateTicketRequestIssueTypeData Issuetype { get; set; }
    }

    public record CreateTicketRequestProjectData(string key);

    public record CreateTicketRequestIssueTypeData(string name);
}
