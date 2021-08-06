using System;

namespace VDB.MicroServices.Auth.Model.Exchange.Token.Create
{
    public class CreateTokenResponseModel
    {
        /// <summary>
        /// JWE Bearer token
        /// </summary>
        public string Token { get; set; } 
        /// <summary>
        /// Token will expire in this timestamp. (Timezone = UTC)
        /// </summary>
        public DateTime ExpiresAt { get; set; } 
        /// <summary>
        /// Token will expire in this much seconds.
        /// </summary>
        public double ExpiresIn { get; set; }
        /// <summary>
        /// Users organizational unit.
        /// </summary>
        public string Unit { get; set; }

        public CreateTokenResponseModel(string token, DateTime expiresAt, double expiresIn, string unit)
        {
            Token = token;
            ExpiresAt = expiresAt;
            ExpiresIn = expiresIn;
            Unit = unit;
        }
    }
}
