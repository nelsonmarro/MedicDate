using System.Threading.Tasks;
using MedicDate.API.DTOs.Grupo;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;

namespace MedicDate.DataAccess.Repository.IRepository
{
    public interface IGrupoRepository : IRepository<Grupo>
    {
        public Task<OperationResult> UpdateGrupoAsync(string id, GrupoRequestDto grupoRequestDto);
    }
}