using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
using System;
using System.Text.RegularExpressions;
using VDB.MicroServices.Auth.Concern.Options;
using VDB.MicroServices.Auth.Manager.Business.Interface;
using VDB.MicroServices.Auth.Model.DTO;

namespace VDB.MicroServices.Auth.Manager.Business.Implementation
{
    public class LDAPManager : IAuthManager
    {
        private readonly LDAPSettings LDAPSettings;
        private readonly LDAPSecrets LDAPSecrets;

        public LDAPManager(IOptions<LDAPSettings> ldapSettings, IOptions<LDAPSecrets> ldapSecrets)
        {
            this.LDAPSettings = ldapSettings.Value;
            this.LDAPSecrets = ldapSecrets.Value;
        }

        public UserModel AuthenticateUser(string userName, string password)
        {
            using LdapConnection ldapConnection = new() { SecureSocketLayer = false };
            
            ldapConnection.Connect(this.LDAPSettings.Host, this.LDAPSettings.Port);
            ldapConnection.Bind(this.LDAPSecrets.AdminDN, this.LDAPSecrets.AdminPassword);

            var searchResults = ldapConnection.Search(this.LDAPSettings.DC, this.LDAPSettings.UIDSearchScope, String.Format(this.LDAPSettings.UIDSearchTemplate, userName), null, false);
            LdapEntry result = null;
            try
            {
                result = searchResults.Next();
            }
            catch (Exception)
            {
                return null;
            }

            UserModel userModel = new(new Regex($"{this.LDAPSettings.OuRegexLeftPart}{this.LDAPSettings.OuRegexExtractor}{this.LDAPSettings.OuRegexRightPart}").Match(result.Dn).Value.Replace(this.LDAPSettings.OuRegexLeftPart, String.Empty).Replace(this.LDAPSettings.OuRegexRightPart, String.Empty));

            try
            {
                ldapConnection.Bind(String.Format(this.LDAPSettings.UserDNTemplate, userName, userModel.OrganizationalUnit, this.LDAPSettings.DC), password);
            }
            catch (Exception)
            {
                return null;
            }
            

            return userModel;
        }
    }
}
