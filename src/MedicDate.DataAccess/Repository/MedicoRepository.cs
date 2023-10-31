using AutoMapper;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Medico;
using Microsoft.EntityFrameworkCore;
using static System.Net.HttpStatusCode;

namespace MedicDate.DataAccess.Repository;

public class MedicoRepository(ApplicationDbContext context, IMapper mapper)
  : Repository<Medico>(context),
    IMedicoRepository
{
  public async Task<OperationResult> UpdateMedicoAsync(string id, MedicoRequestDto medicoRequestDto)
  {
    var medicoDb = await context.Medico
      .Include(x => x.MedicosEspecialidades)
      .FirstOrDefaultAsync(x => x.Id == id);

    if (medicoDb is null)
      return OperationResult.Error(NotFound, "No se encontró el doctor ha actualizar");

    mapper.Map(medicoRequestDto, medicoDb);
    await context.SaveChangesAsync();

    return OperationResult.Success(OK, "Doctor actualizado con éxito");
  }
}
