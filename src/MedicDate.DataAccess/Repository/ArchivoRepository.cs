using MedicDate.Domain.Entities;
using MedicDate.Domain.Interfaces.DataAccess;
using MedicDate.Domain.Results;
using MedicDate.Shared.Models.Archivo;
using static System.Net.HttpStatusCode;

namespace MedicDate.DataAccess.Repository;

public class ArchivoRepository : Repository<Archivo>, IArchivoRepository
{
  public ArchivoRepository(ApplicationDbContext context) : base(context)
  {
  }

  public async Task<OperationResult> UpdateArchivoAsync(string archivoId,
    ArchivoDbUpdateDto archivoDbUpdate)
  {
    var archivoDb = await FindAsync(archivoId);
    if (archivoDb == null)
      return OperationResult.Error(NotFound,
        "No se pudo actualizar el archivo");

    archivoDb.Descripcion = archivoDbUpdate.Descripcion;
    if (!string.IsNullOrEmpty(archivoDbUpdate.RutaArchivo))
      archivoDb.RutaArchivo = archivoDbUpdate.RutaArchivo;

    await SaveAsync();

    return OperationResult.Success(OK,
      "Archivo actualizado correctamente");
  }
}