using AutoMapper;
using MedicDate.Bussines.ApplicationServices.IApplicationServices;
using MedicDate.Bussines.DomainServices.IDomainServices;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Archivo;
using MedicDate.Shared.Models.Common;
using MedicDate.Utility;
using static System.Net.HttpStatusCode;

namespace MedicDate.Bussines.DomainServices;

public class ArchivoService : IArchivoService
{
  private readonly IArchivoRepository _archivoRepo;
  private readonly IFileUpload _fileUpload;
  private readonly IMapper _mapper;

  public ArchivoService(IFileUpload fileUpload,
    IArchivoRepository archivoRepo, IMapper mapper)
  {
    _fileUpload = fileUpload;
    _archivoRepo = archivoRepo;
    _mapper = mapper;
  }

  public async Task<OperationResult> AddArchivosListAsync(
    List<CreateArchivoRequestDto> archivosReq)
  {
    var filesForAzStorage = archivosReq.Select(x =>
      new CreateFileForAzStorageDto
      {
        Container = Sd.AZ_STORAGE_CONTAINER_PACIENTE,
        Content = Convert.FromBase64String(x.ImageBase64 ?? ""),
        ContentType = x.ContentType,
        Extension = x.ExtensionImage
      }).ToList();

    var resultRouteList =
      await _fileUpload.SaveFileListAsync(filesForAzStorage);
    var archivosToSave = archivosReq.Select(x => new ArchivoDbSaveDto
    {
      CitaId = x.CitaId ?? "",
      Descripcion = x.Descripcion
    }).ToList();

    for (var i = 0; i < resultRouteList.Count; i++)
      archivosToSave[i].RutaArchivo = resultRouteList[i];

    var archivosDb = _mapper.Map<List<Archivo>>(archivosToSave);
    await _archivoRepo.AddRangeAsync(archivosDb);
    await _archivoRepo.SaveAsync();

    return OperationResult.Success(Created);
  }

  public async Task<List<ArchivoResponseDto>> GetAllArchivosByCitaIdAsync(
    string citaId)
  {
    var archivosDb =
      await _archivoRepo.GetAllAsync(filter: x => x.CitaId == citaId);
    return _mapper.Map<List<ArchivoResponseDto>>(archivosDb);
  }

  public async Task<OperationResult> RemoveArchivoAsync
    (string archivoId, DeleteArchivoRequestDto deleteArchivoRequest)
  {
    await _fileUpload.DeleteFileAsync(deleteArchivoRequest.RutaCreated,
      Sd.AZ_STORAGE_CONTAINER_PACIENTE);

    await _archivoRepo.RemoveAsync(archivoId);
    await _archivoRepo.SaveAsync();

    return OperationResult.Success(OK);
  }

  public async Task<OperationResult> UpdateArchivoAsync(string archivoId,
    UpdateArchivoRequestDto? archivoRequestDto)
  {
    if (archivoRequestDto is null) return OperationResult.Error(NotFound);

    var archivoUpdateDb = new ArchivoDbUpdateDto
    {
      Descripcion = archivoRequestDto.Description
    };

    if (!string.IsNullOrEmpty(archivoRequestDto.ImageBase64))
    {
      var updateFileFromAzStorage = new UpdateFileForAzStorageDto
      {
        Container = Sd.AZ_STORAGE_CONTAINER_PACIENTE,
        Content = Convert.FromBase64String(archivoRequestDto.ImageBase64),
        ContentType = archivoRequestDto.ContentType,
        CreatedRoute = archivoRequestDto.CreatedRoute,
        Extension = archivoRequestDto.ExtensionImage
      };

      archivoUpdateDb.RutaArchivo = await _fileUpload
        .UpdateFileAsync(updateFileFromAzStorage);
    }

    return await _archivoRepo.UpdateArchivoAsync(archivoId,
      archivoUpdateDb);
  }
}