using System.Threading.Tasks;
using MedicDate.Bussines.Helpers;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs;
using MedicDate.Models.DTOs.Especialidad;
using MedicDate.Utility;

namespace MedicDate.Bussines.Repository.IRepository
{
    public interface IEspecialidadRepository : IRepository<Especialidad>
    {
        public Task<DataResponse<string>> UpdateEspecialidad(int id, EspecialidadRequest especialidadDto);
    }
}