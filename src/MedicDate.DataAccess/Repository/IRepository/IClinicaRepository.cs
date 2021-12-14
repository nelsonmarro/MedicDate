using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.Shared.Models.Clinica;

namespace MedicDate.DataAccess.Repository.IRepository;

public interface IClinicaRepository : IRepository<Clinica>
{
    Task<OperationResult> UpdateClinicaAsync(string id, ClinicaRequestDto clinicaRequest);
}
