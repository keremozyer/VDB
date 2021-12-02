namespace VDB.Architecture.Concern.Options
{
    public record TokenSettings
    {
        public string SecurityKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double ExpiresIn { get; set; }
        public EncryptionCertificate EncryptionCertificate { get; set; }
    }

    public record EncryptionCertificate
    {
        public string PublicCertificatePath { get; set; }
        public string PrivateKeyPath { get; set; }
        public string PrivateKeyPassword { get; set; }
    }
}
