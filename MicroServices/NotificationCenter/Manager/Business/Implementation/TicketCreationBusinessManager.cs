using Dasync.Collections;
using System;
using System.Threading.Tasks;
using VDB.Architecture.Concern.ExtensionMethods;
using VDB.MicroServices.NotificationCenter.ExternalData.Manager.Contract;
using VDB.MicroServices.NotificationCenter.Manager.Business.Interface;
using VDB.MicroServices.NotificationCenter.Model.Exchange.CreateTicket;

namespace VDB.MicroServices.NotificationCenter.Manager.Business.Implementation
{
    public class TicketCreationBusinessManager : ITicketCreationBusinessManager
    {
        private readonly ITicketServiceManager TicketServiceManager;

        public TicketCreationBusinessManager(ITicketServiceManager ticketServiceManager)
        {
            this.TicketServiceManager = ticketServiceManager;
        }

        public async Task CreateTicket(CreateTicketRequestModel request)
        {
            if ((request?.Data.IsNullOrEmpty()).GetValueOrDefault()) throw new ArgumentNullException(nameof(CreateTicketRequestModel.Data));

            await request.Data.ParallelForEachAsync(async ticketData => 
            {
                await CreateTicket(ticketData);
            });
        }

        private async Task CreateTicket(TicketData ticketData)
        {
            bool isTicketAlreadyCreated = await this.TicketServiceManager.HasTicketWithCVE(ticketData);
            if (isTicketAlreadyCreated)
            {
                return;
            }

            await this.TicketServiceManager.CreateTicket(ticketData);
        }
    }
}
