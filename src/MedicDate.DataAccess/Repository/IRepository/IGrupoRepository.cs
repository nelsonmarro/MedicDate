using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.Shared.Models.Grupo;

namespace MedicDate.DataAccess.Repository.IRepository;

public interface IGrupoRepository : IRepository<Grupo>
{
  public Task<OperationResult> UpdateGrupoAsync(string id,
    GrupoRequestDto grupoRequestDto);
}