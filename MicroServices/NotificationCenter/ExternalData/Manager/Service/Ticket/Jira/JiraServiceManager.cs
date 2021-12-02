using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VDB.Architecture.Concern.ExtensionMethods;
using VDB.MicroServices.NotificationCenter.Concern.Options;
using VDB.MicroServices.NotificationCenter.ExternalData.Manager.Contract;
using VDB.MicroServices.NotificationCenter.ExternalData.Model.Ticket.Jira;
using VDB.MicroServices.NotificationCenter.Model.Exchange.CreateTicket;

namespace VDB.MicroServices.NotificationCenter.ExternalData.Manager.Service.Ticket.Jira
{
    public class JiraServiceManager : ITicketServiceManager
    {
        private readonly HttpClient HttpClient;
        private readonly TicketServiceSettings TicketServiceSettings;

        public JiraServiceManager(HttpClient httpClient, IOptions<TicketServiceSettings> ticketServiceSettings)
        {
            this.HttpClient = httpClient;
            this.TicketServiceSettings = ticketServiceSettings.Value;
        }

        public async Task<bool> HasTicketWithCVE(TicketData ticketData)
        {
            HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(String.Format(this.TicketServiceSettings.SearchWithCVEEndpoint, ticketData.CVEID, ticketData.Vendor, ticketData.Product, ticketData.Version));

            TicketSearchResponseModel jiraResponse = (await httpResponse.Content.ReadAsStringAsync()).DeserializeJSON<TicketSearchResponseModel>();

            return jiraResponse.total > 0;
        }

        public async Task CreateTicket(TicketData ticketData)
        {
            var fields = new ExpandoObject() as IDictionary<string, Object>;
            fields.Add(this.TicketServiceSettings.FieldIDs.Project, new CreateTicketRequestProjectData(this.TicketServiceSettings.DefaultFieldValues.ProjectKey));
            fields.Add(this.TicketServiceSettings.FieldIDs.Summary, ticketData.Summary);
            fields.Add(this.TicketServiceSettings.FieldIDs.Description, ticketData.Description);
            fields.Add(this.TicketServiceSettings.FieldIDs.IssueType, new CreateTicketRequestIssueTypeData(this.TicketServiceSettings.DefaultFieldValues.IssueTypeName));
            fields.Add(this.TicketServiceSettings.FieldIDs.CVEID, ticketData.CVEID);
            fields.Add(this.TicketServiceSettings.FieldIDs.Vendor, ticketData.Vendor);
            fields.Add(this.TicketServiceSettings.FieldIDs.Product, ticketData.Product);
            fields.Add(this.TicketServiceSettings.FieldIDs.Version, ticketData.Version);

            var root = new ExpandoObject() as IDictionary<string, Object>;
            root.Add(this.TicketServiceSettings.FieldIDs.Fields, fields);

            string body = root.SerializeAsJson();

            HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(this.TicketServiceSettings.CreateIssueEndpoint, new StringContent(body, Encoding.UTF8, "application/json"));

            httpResponse.EnsureSuccessStatusCode();
        }
    }
}
