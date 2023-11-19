using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MedicDate.Domain.Entities;
using MedicDate.Domain.Interfaces.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace MedicDate.Infrastructure.Services;

public class TokenBuilderService : ITokenBuilderService
{
  private readonly UserManager<ApplicationUser> _userManager;

  public TokenBuilderService(UserManager<ApplicationUser> userManager)
  {
    _userManager = userManager;
  }

  public SigningCredentials GetSigningCredentials(string signInKey)
  {
    var key = Encoding.UTF8.GetBytes(signInKey);
    var secret = new SymmetricSecurityKey(key);

    return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
  }

  public async Task<List<Claim>> GetClaims(ApplicationUser user)
  {
    if (user.Email == null)
      return new List<Claim>();

    var claims = new List<Claim>
    {
      new(ClaimTypes.Email, user.Email),
      new(ClaimTypes.Name, user.Nombre),
    };

    var roles = await _userManager.GetRolesAsync(user);

    claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

    return claims;
  }

  public JwtSecurityToken GenerateTokenOptions(
    SigningCredentials signingCredentials,
    List<Claim> claims,
    string validAudience,
    string validIssuer,
    string expiryInMinutes
  )
  {
    var tokenOptions = new JwtSecurityToken(
      validIssuer,
      validAudience,
      claims,
      expires: DateTime.Now.AddMinutes(Convert.ToDouble(expiryInMinutes)),
      signingCredentials: signingCredentials
    );

    return tokenOptions;
  }

  public string GenerateRefreshToken()
  {
    var randomNumber = new byte[32];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomNumber);
    return Convert.ToBase64String(randomNumber);
  }

  public ClaimsPrincipal GetPrincipalFromExpiredToken(
    string token,
    string singInKey,
    string validAudience,
    string validIssuer
  )
  {
    var tokenValidationParameters = new TokenValidationParameters
    {
      ValidateAudience = true,
      ValidateIssuer = true,
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(singInKey)),
      ValidateLifetime = false,
      ValidIssuer = validIssuer,
      ValidAudience = validAudience
    };

    var tokenHandler = new JwtSecurityTokenHandler();

    var principal = tokenHandler.ValidateToken(
      token,
      tokenValidationParameters,
      out var securityToken
    );

    if (
      securityToken is not JwtSecurityToken jwtSecurityToken
      || !jwtSecurityToken.Header.Alg.Equals(
        SecurityAlgorithms.HmacSha256,
        StringComparison.InvariantCultureIgnoreCase
      )
    )
      throw new SecurityTokenException("Invalid token");

    return principal;
  }
}
