using System.Threading.Tasks;
using VDB.MicroServices.NotificationCenter.ExternalData.Manager.Model.Email;

namespace VDB.MicroServices.NotificationCenter.ExternalData.Manager.Service.Emailing
{
    public interface IEmailServiceManager
    {
        public Task SendMail(EmailMessageModel message);
    }
}
