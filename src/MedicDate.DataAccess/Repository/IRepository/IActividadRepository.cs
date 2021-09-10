using System.Threading.Tasks;
using MedicDate.API.DTOs.Actividad;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;

namespace MedicDate.DataAccess.Repository.IRepository
{
    public interface IActividadRepository : IRepository<Actividad>
    {
        Task<OperationResult> UpdateActividadAsync(string actId, ActividadRequestDto actRequestDto);
    }
}