using MedicDate.Bussines.DomainServices.IDomainServices;
using MedicDate.Shared.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace MedicDate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
  private readonly ITokenService _tokenService;

  public TokenController(ITokenService tokenService)
  {
    _tokenService = tokenService;
  }

  [HttpPost("refresh")]
  public async Task<ActionResult<LoginResponseDto>> RefreshAsync(
    RefreshTokenDto refreshTokenDto)
  {
    var resp = await _tokenService.RefreshTokenAsync(refreshTokenDto);

    return resp.Succeeded
      ? resp.DataResult ?? new LoginResponseDto()
      : resp.ErrorResult;
  }
}