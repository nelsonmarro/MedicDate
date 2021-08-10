using System.Threading.Tasks;
using MedicDate.Bussines.Helpers;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs.Grupo;

namespace MedicDate.Bussines.Repository.IRepository
{
    public interface IGrupoRepository : IRepository<Grupo>
    {
        public Task<DataResponse<string>> UpdateGrupoAsync(string id, GrupoRequest grupoRequest);
    }
}