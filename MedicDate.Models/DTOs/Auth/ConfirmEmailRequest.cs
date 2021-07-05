namespace MedicDate.Models.DTOs.Auth
{
    public class ConfirmEmailRequest
    {
        public string UserId { get; set; }
        public string Code { get; set; }
    }
}