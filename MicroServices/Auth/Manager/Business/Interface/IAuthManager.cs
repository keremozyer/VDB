using VDB.MicroServices.Auth.Model.DTO;

namespace VDB.MicroServices.Auth.Manager.Business.Interface
{
    public interface IAuthManager
    {
        UserModel AuthenticateUser(string userName, string password);
    }
}
