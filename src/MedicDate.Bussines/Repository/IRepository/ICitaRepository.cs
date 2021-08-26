using MedicDate.API.DTOs.Cita;
using MedicDate.Bussines.Helpers;
using MedicDate.DataAccess.Entities;
using System.Threading.Tasks;

namespace MedicDate.Bussines.Repository.IRepository
{
    public interface ICitaRepository : IRepository<Cita>
    {
        Task<ApiOperationResult> UpdateCitaAsync(string citaId, CitaRequestDto citaRequestDto);
    }
}