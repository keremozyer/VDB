using System.Threading.Tasks;
using VDB.MicroServices.NotificationCenter.ExternalData.Model.Email;

namespace VDB.MicroServices.NotificationCenter.ExternalData.Manager.Contract
{
    public interface IEmailServiceManager
    {
        public Task SendMail(EmailMessageModel message);
    }
}
