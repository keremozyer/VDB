using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using VDB.Architecture.Concern.Options;
using VDB.MicroServices.Auth.Model.Exchange.Token.Create;
using Microsoft.IdentityModel.Tokens;
using VDB.MicroServices.Auth.Manager.Business.Interface;
using Microsoft.Extensions.Options;
using VDB.MicroServices.Auth.Model.DTO;
using VDB.Architecture.Concern.GenericValidator;
using VDB.Architecture.AppException.Model.Derived.Validation;
using VDB.Architecture.Concern.Resources.ResourceKeys;

namespace VDB.MicroServices.Auth.Manager.Business.Implementation
{
    public class TokenManager : ITokenManager
    {
        private readonly Validator Validator;
        private readonly TokenSettings TokenSettings;
        private readonly IAuthManager AuthManager;

        public TokenManager(Validator validator, IOptions<TokenSettings> tokenSettings, IAuthManager authManager)
        {
            this.Validator = validator;
            this.TokenSettings = tokenSettings.Value;
            this.AuthManager = authManager;
        }

        public CreateTokenResponseModel CreateToken(CreateTokenRequestModel request)
        {
            this.Validator.Validate<CreateTokenRequestModel>(request);

            UserModel user = this.AuthManager.AuthenticateUser(request.Username, request.Password);

            if (user == null)
            {
                throw new ValidationException(ErrorResourceKeys.InvalidUsernameOrPassword);                
            }

            DateTime utcNow = DateTime.UtcNow;

            SecurityTokenDescriptor descriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, request.Username),
                    new Claim("Role", user.OrganizationalUnit)
                }),
                NotBefore = utcNow,
                Expires = utcNow.Add(TimeSpan.FromSeconds(this.TokenSettings.ExpiresIn)),
                Issuer = this.TokenSettings.Issuer,
                Audience = this.TokenSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.TokenSettings.SecurityKey)), SecurityAlgorithms.HmacSha256),
                //EncryptingCredentials = new X509EncryptingCredentials(new X509Certificate2(this.TokenSettings.EncryptionCertificate.PublicCertificatePath))
            };
            JwtSecurityTokenHandler handler = new();

            return new CreateTokenResponseModel(handler.WriteToken(handler.CreateToken(descriptor)), descriptor.Expires.Value, descriptor.Expires.Value.Subtract(utcNow).TotalSeconds, user.OrganizationalUnit);
        }
    }
}
