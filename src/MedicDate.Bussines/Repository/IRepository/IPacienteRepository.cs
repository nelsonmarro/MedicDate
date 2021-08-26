using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Interfaces;
using System.Threading.Tasks;
using MedicDate.API.DTOs.Paciente;
using MedicDate.DataAccess.Entities;

namespace MedicDate.Bussines.Repository.IRepository
{
    public interface IPacienteRepository : IRepository<Paciente>, ICedulaValidator, IRelatedEntityValidator
    {
        Task<ApiOperationResult> UpdatePacienteAsync(string id, PacienteRequestDto pacienteRequestDto);
        Task<bool> CheckNumHistoriaExistsAsync(string numHistoria, bool isEdit = false, string id = null);
    }
}