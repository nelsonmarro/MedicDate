namespace MedicDate.Client.Services.IServices
{
    public interface IRefreshTokenService
    {
        public Task<string?> TryRefreshToken();
    }
}