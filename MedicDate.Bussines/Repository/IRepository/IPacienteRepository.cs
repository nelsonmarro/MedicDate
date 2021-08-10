using System.Threading.Tasks;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Interfaces;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs.Paciente;

namespace MedicDate.Bussines.Repository.IRepository
{
    public interface IPacienteRepository : IRepository<Paciente>, IRequestEntityValidator
    {
        Task<DataResponse<string>> UpdatePacienteAsync(string id, PacienteRequest pacienteRequest);
        Task<bool> CheckNumHistoriaExistsAsync(string numHistoria, bool isEdit = false, string id = null);
    }
}