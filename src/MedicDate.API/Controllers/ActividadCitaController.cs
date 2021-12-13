using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Actividad;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicDate.API.Controllers;

[Authorize]
[Route("api/Cita/{citaId}")]
[ApiController]
public class ActividadCitaController : ControllerBase
{
   private readonly IActividadCitaRepository _actividadCitaRepo;

   public ActividadCitaController(IActividadCitaRepository actividadCitaRepo)
   {
      _actividadCitaRepo = actividadCitaRepo;
   }

   [HttpPut("updateActividad/{actividadId}")]
   public async Task<ActionResult> UpdateActividadCitaAsync(string citaId,
      string? actividadId, ActividadCitaRequestDto actividadReq)
   {
      actividadReq.ActividadId = actividadId;
      var result =
        await _actividadCitaRepo.UpdateActividadCitaAsync(citaId, actividadReq);

      return result.Succeeded
        ? result.SuccessResult
        : result.ErrorResult;
   }
}