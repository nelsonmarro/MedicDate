using System.Collections.Generic;
using System.Threading.Tasks;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs.Medico;
using MedicDate.Utility;

namespace MedicDate.Bussines.Repository.IRepository
{
    public interface IMedicoRepository : IRepository<Medico>
    {
        public Task<DataResponse<string>> UpdateMedicoAsync(int id, MedicoRequest medicoRequest);

        public Task<bool> ExisteEspecialidadIdParaCrearMedico(List<int> especialidadesIds);

        public Task<TResponse> GetMedicoConEspecialidades<TResponse>(int medicoId);

        public Task<TRequest> GetMedicoParaEdicion<TRequest>(int medicoId);
    }
}