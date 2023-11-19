using MedicDate.Domain.Entities;
using MedicDate.Domain.Results;
using MedicDate.Shared.Models.Paciente;

namespace MedicDate.Domain.Interfaces.DataAccess;

public interface IPacienteRepository : IRepository<Paciente>
{
  Task<OperationResult> UpdatePacienteAsync(string id,
    PacienteRequestDto pacienteRequestDto);
}