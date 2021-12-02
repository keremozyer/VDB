using System.ComponentModel;

namespace VDB.MicroServices.Auth.Model.Exchange.Token.Create
{
    [DisplayName("Token Oluşturma İsteği")]
    public record CreateTokenRequestModel
    {
        [DisplayName("Kullanıcı Adı")]
        public string Username { get; set; }
        [DisplayName("Şifre")]
        public string Password { get; set; }
    }
}
