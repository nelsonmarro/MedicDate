using System.Threading.Tasks;
using MedicDate.API.DTOs.Cita;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;

namespace MedicDate.DataAccess.Repository.IRepository
{
    public interface ICitaRepository : IRepository<Cita>
    {
        Task<OperationResult> UpdateCitaAsync(string citaId, CitaRequestDto citaRequestDto);
    }
}