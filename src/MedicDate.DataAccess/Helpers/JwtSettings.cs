namespace MedicDate.DataAccess.Helpers
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string ExpiryInMinutes { get; set; }
    }
}