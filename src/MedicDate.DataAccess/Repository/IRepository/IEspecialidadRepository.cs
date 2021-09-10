using System.Threading.Tasks;
using MedicDate.API.DTOs.Especialidad;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;

namespace MedicDate.DataAccess.Repository.IRepository
{
    public interface IEspecialidadRepository : IRepository<Especialidad>
    {
        public Task<OperationResult> UpdateEspecialidadAsync(string id, EspecialidadRequestDto especialidadDto);
    }
}