﻿using MedicDate.Shared.Models.Common;

namespace MedicDate.Domain.Interfaces.Infrastructure;

public interface IFileUpload
{
  Task<string> SaveFileAsync(CreateFileForAzStorageDto createFileForAzStorage);

  Task<List<string>> SaveFileListAsync(
    List<CreateFileForAzStorageDto> createFilesForAzStorage);

  Task<string>
    UpdateFileAsync(UpdateFileForAzStorageDto updateFileForAzStorage);

  Task<List<string>> UpdateFileListAsync(
    List<UpdateFileForAzStorageDto> updateFilesForAzStorage);

  Task DeleteFileAsync(string route, string container);
}