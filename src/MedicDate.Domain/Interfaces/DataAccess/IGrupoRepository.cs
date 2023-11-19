using MedicDate.Domain.Entities;
using MedicDate.Domain.Results;
using MedicDate.Shared.Models.Grupo;

namespace MedicDate.Domain.Interfaces.DataAccess;

public interface IGrupoRepository : IRepository<Grupo>
{
  public Task<OperationResult> UpdateGrupoAsync(string id,
    GrupoRequestDto grupoRequestDto);
}