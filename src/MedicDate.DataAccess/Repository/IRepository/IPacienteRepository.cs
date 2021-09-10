using System.Threading.Tasks;
using MedicDate.API.DTOs.Paciente;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.DataAccess.Interfaces;

namespace MedicDate.DataAccess.Repository.IRepository
{
    public interface IPacienteRepository : IRepository<Paciente>, ICedulaValidator, IRelatedEntityValidator
    {
        Task<OperationResult> UpdatePacienteAsync(string id, PacienteRequestDto pacienteRequestDto);
        Task<bool> CheckNumHistoriaExistsAsync(string numHistoria, bool isEdit = false, string id = null);
    }
}