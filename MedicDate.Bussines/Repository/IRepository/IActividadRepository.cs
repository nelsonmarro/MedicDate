using System.Threading.Tasks;
using MedicDate.Bussines.Helpers;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs.Actividad;

namespace MedicDate.Bussines.Repository.IRepository
{
    public interface IActividadRepository : IRepository<Actividad>
    {
        Task<DataResponse<string>> UpdateActividadAsync(string actId, ActividadRequest actRequest);
    }
}