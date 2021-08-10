using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Interfaces;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs.Medico;
using System.Threading.Tasks;

namespace MedicDate.Bussines.Repository.IRepository
{
    public interface IMedicoRepository : IRepository<Medico>, IRequestEntityValidator
    {
        public Task<DataResponse<string>> UpdateMedicoAsync(string id, MedicoRequest medicoRequest);
    }
}