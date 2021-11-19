using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.Shared.Models.Archivo;

namespace MedicDate.DataAccess.Repository.IRepository;

public interface IArchivoRepository : IRepository<Archivo>
{
  Task<OperationResult> UpdateArchivoAsync(string archivoId,
    ArchivoDbUpdateDto archivoDbUpdate);
}