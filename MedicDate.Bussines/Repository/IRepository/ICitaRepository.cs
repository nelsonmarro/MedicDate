using System.Threading.Tasks;
using MedicDate.Bussines.Helpers;
using MedicDate.DataAccess.Models;

namespace MedicDate.Bussines.Repository.IRepository
{
    public interface ICitaRepository : IRepository<Cita>
    {
        Task<DataResponse<string>> UpdateCitaAsync(string citaId);
    }
}