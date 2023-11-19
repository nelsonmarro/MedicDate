using MedicDate.Domain.Services.IDomainServices;
using MedicDate.Shared.Models.Archivo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicDate.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ArchivoController : ControllerBase
{
   private readonly IArchivoService _archivoService;

   public ArchivoController(IArchivoService archivoService)
   {
      _archivoService = archivoService;
   }

   [HttpGet("listAllByCita/{citaId}")]
   public async Task<ActionResult<List<ArchivoResponseDto>>>
     GetAllArchivosByCitaAsync(string citaId)
   {
      return await _archivoService.GetAllArchivosByCitaIdAsync(citaId);
   }

   [HttpPost("subirListado")]
   public async Task<ActionResult> UploadArchivosListAsync(
     List<CreateArchivoRequestDto> archivosCreate)
   {
      var result = await _archivoService.AddArchivosListAsync(archivosCreate);
      return result.Succeeded
        ? result.SuccessResult
        : result.ErrorResult;
   }

   [HttpPut("edit/{archivoId}")]
   public async Task<ActionResult> UpdateArchivoAsync(string archivoId,
     UpdateArchivoRequestDto updateArchivoRequest)
   {
      var result = await _archivoService
        .UpdateArchivoAsync(archivoId, updateArchivoRequest);

      return result.Succeeded
        ? result.SuccessResult
        : result.ErrorResult;
   }

   [HttpPost("delete/{archivoId}")]
   public async Task<ActionResult> DeleteArchivoAsync(string archivoId,
     DeleteArchivoRequestDto deleteArchivoRequest)
   {
      var result =
        await _archivoService.RemoveArchivoAsync(archivoId, deleteArchivoRequest);
      return result.Succeeded
        ? result.SuccessResult
        : result.ErrorResult;
   }
}