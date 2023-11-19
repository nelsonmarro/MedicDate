using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MedicDate.Domain.Interfaces.Infrastructure;
using MedicDate.Shared.Models.Common;
using Microsoft.Extensions.Configuration;

namespace MedicDate.Infrastructure.Services;

public class AzureStorageFileUpload : IFileUpload
{
  private readonly string? _connectionString;

  public AzureStorageFileUpload(IConfiguration configuration)
  {
    _connectionString = configuration.GetConnectionString("AzureStorage");
  }

  public async Task DeleteFileAsync(string route, string container)
  {
    if (string.IsNullOrEmpty(route)) return;

    var client = new BlobContainerClient(_connectionString, container);
    await client.CreateIfNotExistsAsync();

    var archivo = Path.GetFileName(route);
    var blob = client.GetBlobClient(archivo);
    await blob.DeleteIfExistsAsync();
  }

  public async Task<string> SaveFileAsync(
    CreateFileForAzStorageDto createFileForAzStorage)
  {
    var client = new BlobContainerClient(_connectionString,
      createFileForAzStorage.Container);

    await client.CreateIfNotExistsAsync();
    await client.SetAccessPolicyAsync(PublicAccessType.Blob);

    var fileName = $"{Guid.NewGuid()}.{createFileForAzStorage.Extension}";
    var blob = client.GetBlobClient(fileName);
    var blobUploadOptions = new BlobUploadOptions();
    var blobHttpHeaders = new BlobHttpHeaders();
    blobHttpHeaders.ContentType = createFileForAzStorage.ContentType;
    blobUploadOptions.HttpHeaders = blobHttpHeaders;

    await blob.UploadAsync(new BinaryData(createFileForAzStorage.Content),
      blobUploadOptions);

    return blob.Uri.ToString();
  }

  public async Task<List<string>> SaveFileListAsync(
    List<CreateFileForAzStorageDto> createFilesForAzStorage)
  {
    var resultList = new List<string>();

    foreach (var file in createFilesForAzStorage)
    {
      var result = await SaveFileAsync(file);
      resultList.Add(result);
    }

    return resultList;
  }

  public async Task<string> UpdateFileAsync(
    UpdateFileForAzStorageDto updateFileForAzStorage)
  {
    await DeleteFileAsync(updateFileForAzStorage.CreatedRoute,
      updateFileForAzStorage.Container);

    return await SaveFileAsync(updateFileForAzStorage);
  }

  public async Task<List<string>> UpdateFileListAsync(
    List<UpdateFileForAzStorageDto> updateFilesForAzStorage)
  {
    var resultList = new List<string>();

    foreach (var file in updateFilesForAzStorage)
    {
      var result = await UpdateFileAsync(file);
      resultList.Add(result);
    }

    return resultList;
  }
}