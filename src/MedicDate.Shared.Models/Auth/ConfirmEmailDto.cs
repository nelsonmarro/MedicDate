namespace MedicDate.API.DTOs.Auth
{
    public class ConfirmEmailDto
    {
        public string UserId { get; set; }
        public string Code { get; set; }
    }
}