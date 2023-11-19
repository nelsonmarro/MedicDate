using MedicDate.Domain.Entities;
using MedicDate.Domain.Results;
using MedicDate.Shared.Models.Archivo;

namespace MedicDate.Domain.Interfaces.DataAccess;

public interface IArchivoRepository : IRepository<Archivo>
{
  Task<OperationResult> UpdateArchivoAsync(string archivoId,
    ArchivoDbUpdateDto archivoDbUpdate);
}