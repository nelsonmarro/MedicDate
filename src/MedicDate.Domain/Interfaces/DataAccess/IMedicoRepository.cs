using MedicDate.Domain.Entities;
using MedicDate.Domain.Results;
using MedicDate.Shared.Models.Medico;

namespace MedicDate.Domain.Interfaces.DataAccess;

public interface IMedicoRepository : IRepository<Medico>
{
  public Task<OperationResult> UpdateMedicoAsync(string id,
    MedicoRequestDto medicoRequestDto);
}