using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using MedicDate.DataAccess.Entities;

namespace MedicDate.Bussines.Services.IServices
{
    public interface ITokenService
    {
        SigningCredentials GetSigningCredentials(string signInKey);
        Task<List<Claim>> GetClaims(ApplicationUser user);

        JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims,
            string validAudience, string validIssuer, string expiryInMinutes);

        string GenerateRefreshToken();

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string singInKey, string validAudience,
            string validIssuer);
    }
}