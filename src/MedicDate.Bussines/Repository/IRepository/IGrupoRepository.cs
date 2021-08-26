using System.Threading.Tasks;
using MedicDate.API.DTOs.Grupo;
using MedicDate.Bussines.Helpers;
using MedicDate.DataAccess.Entities;

namespace MedicDate.Bussines.Repository.IRepository
{
    public interface IGrupoRepository : IRepository<Grupo>
    {
        public Task<ApiOperationResult> UpdateGrupoAsync(string id, GrupoRequestDto grupoRequestDto);
    }
}