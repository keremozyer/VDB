using System;

namespace VDB.MicroServices.Auth.Model.Exchange.Token.Create
{
    public record CreateTokenResponseModel(string Token, DateTime ExpiresAt, double ExpiresIn, string Unit);
}
