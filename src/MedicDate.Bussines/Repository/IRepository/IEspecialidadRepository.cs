using System.Threading.Tasks;
using MedicDate.API.DTOs.Especialidad;
using MedicDate.Bussines.Helpers;
using MedicDate.DataAccess.Entities;

namespace MedicDate.Bussines.Repository.IRepository
{
    public interface IEspecialidadRepository : IRepository<Especialidad>
    {
        public Task<ApiOperationResult> UpdateEspecialidadAsync(string id, EspecialidadRequestDto especialidadDto);
    }
}