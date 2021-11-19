using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.Shared.Models.Medico;

namespace MedicDate.DataAccess.Repository.IRepository;

public interface IMedicoRepository : IRepository<Medico>
{
  public Task<OperationResult> UpdateMedicoAsync(string id,
    MedicoRequestDto medicoRequestDto);
}