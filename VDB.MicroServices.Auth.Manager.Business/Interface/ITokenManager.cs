using VDB.MicroServices.Auth.Model.Exchange.Token.Create;

namespace VDB.MicroServices.Auth.Manager.Business.Interface
{
    public interface ITokenManager
    {
        CreateTokenResponseModel CreateToken(CreateTokenRequestModel request);
    }
}
