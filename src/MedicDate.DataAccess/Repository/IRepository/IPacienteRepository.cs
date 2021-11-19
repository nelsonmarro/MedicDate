using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.Shared.Models.Paciente;

namespace MedicDate.DataAccess.Repository.IRepository;

public interface IPacienteRepository : IRepository<Paciente>
{
  Task<OperationResult> UpdatePacienteAsync(string id,
    PacienteRequestDto pacienteRequestDto);
}