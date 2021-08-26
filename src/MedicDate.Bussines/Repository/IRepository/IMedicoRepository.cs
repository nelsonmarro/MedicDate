using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Interfaces;
using System.Threading.Tasks;
using MedicDate.API.DTOs.Medico;
using MedicDate.DataAccess.Entities;

namespace MedicDate.Bussines.Repository.IRepository
{
    public interface IMedicoRepository : IRepository<Medico>, ICedulaValidator, IRelatedEntityValidator
    {
        public Task<ApiOperationResult> UpdateMedicoAsync(string id, MedicoRequestDto medicoRequestDto);
    }
}