using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MedicDate.Bussines.ApplicationServices.IApplicationServices;
using MedicDate.Bussines.DomainServices.IDomainServices;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.Shared.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using static System.Net.HttpStatusCode;

namespace MedicDate.Bussines.DomainServices;

public class TokenService : ITokenService
{
  private readonly JwtSettings _jwtSettings;
  private readonly ITokenBuilderService _tokenBuilderService;
  private readonly UserManager<ApplicationUser> _userManager;

  public TokenService(
    UserManager<ApplicationUser> userManager,
    ITokenBuilderService tokenBuilderService,
    IOptions<JwtSettings> jwtOptions
  )
  {
    _userManager = userManager;
    _tokenBuilderService = tokenBuilderService;
    _jwtSettings = jwtOptions.Value;
  }

  public async Task<OperationResult<LoginResponseDto>> RefreshTokenAsync(
    RefreshTokenDto? refreshTokenDto
  )
  {
    if (refreshTokenDto is null)
    {
      return OperationResult<LoginResponseDto>.Error(
        NotFound,
        "Error al validar la sesión del usuario"
      );
    }

    var principal = _tokenBuilderService.GetPrincipalFromExpiredToken(
      refreshTokenDto.Token,
      _jwtSettings.SecretKey,
      _jwtSettings.ValidAudience,
      _jwtSettings.ValidIssuer
    );

    var emailClaim = principal.FindFirst(ClaimTypes.Email);
    var user = await _userManager.FindByEmailAsync(emailClaim?.Value ?? string.Empty);

    if (string.IsNullOrEmpty(user?.RefreshToken))
    {
      return OperationResult<LoginResponseDto>.Error(
        NotFound,
        "No se encotró un refresh token en la DB"
      );
    }

    if (
      user.RefreshToken != refreshTokenDto.RefreshToken
      || user.RefreshTokenExpiryTime <= DateTime.Now
    )
    {
      return OperationResult<LoginResponseDto>.Error(
        BadRequest,
        new LoginResponseDto
        {
          IsAuthSuccessful = false,
          ErrorMessage = "Petición inválida al servidor"
        }
      );
    }

    var signinCreds = _tokenBuilderService.GetSigningCredentials(_jwtSettings.SecretKey);

    var claims = await _tokenBuilderService.GetClaims(user);

    var tokenOptions = _tokenBuilderService.GenerateTokenOptions(
      signinCreds,
      claims,
      _jwtSettings.ValidAudience,
      _jwtSettings.ValidIssuer,
      _jwtSettings.ExpiryInMinutes
    );

    var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    user.RefreshToken = _tokenBuilderService.GenerateRefreshToken();

    await _userManager.UpdateAsync(user);

    return OperationResult<LoginResponseDto>.Success(
      new LoginResponseDto
      {
        Token = token,
        RefreshToken = user.RefreshToken,
        IsAuthSuccessful = true
      }
    );
  }
}
