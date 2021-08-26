using System.Threading.Tasks;
using MedicDate.API.DTOs.Actividad;
using MedicDate.Bussines.Helpers;
using MedicDate.DataAccess.Entities;

namespace MedicDate.Bussines.Repository.IRepository
{
    public interface IActividadRepository : IRepository<Actividad>
    {
        Task<ApiOperationResult> UpdateActividadAsync(string actId, ActividadRequestDto actRequestDto);
    }
}