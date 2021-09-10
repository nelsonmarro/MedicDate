using System.Threading.Tasks;
using MedicDate.API.DTOs.Medico;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.DataAccess.Interfaces;

namespace MedicDate.DataAccess.Repository.IRepository
{
    public interface IMedicoRepository : IRepository<Medico>, ICedulaValidator, IRelatedEntityValidator
    {
        public Task<OperationResult> UpdateMedicoAsync(string id, MedicoRequestDto medicoRequestDto);
    }
}