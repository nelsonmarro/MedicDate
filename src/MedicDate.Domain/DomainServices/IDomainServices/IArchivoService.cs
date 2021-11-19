using MedicDate.DataAccess.Helpers;
using MedicDate.Shared.Models.Archivo;

namespace MedicDate.Bussines.DomainServices.IDomainServices;

public interface IArchivoService
{
  Task<List<ArchivoResponseDto>> GetAllArchivosByCitaIdAsync(string citaId);

  Task<OperationResult> AddArchivosListAsync(
    List<CreateArchivoRequestDto> archivos);

  Task<OperationResult> RemoveArchivoAsync(string archivoId,
    DeleteArchivoRequestDto deleteArchivoRequest);

  Task<OperationResult> UpdateArchivoAsync(string archivoId,
    UpdateArchivoRequestDto? archivoDbUpdate);
}