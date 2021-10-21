using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using MedicDate.DataAccess.Entities;
using Microsoft.IdentityModel.Tokens;

namespace MedicDate.Bussines.ApplicationServices.IApplicationServices
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