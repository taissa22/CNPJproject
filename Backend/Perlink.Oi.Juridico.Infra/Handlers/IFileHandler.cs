using Microsoft.AspNetCore.Http;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;
using System.IO;

namespace Perlink.Oi.Juridico.Infra.Handlers {
    public interface IFileHandler {
        CommandResult<double> FileSize(string filePath);
        CommandResult<bool> ExceedFileMaxSize(double fileSize, double maxFileSize);
        CommandResult<string> GetUniqueFilename(IFormFile fileForms);
        CommandResult<string> GetTempPath(string folderName);
        CommandResult<bool> FileExtensionInvalid(string fileName, string extensionToCompare);
        CommandResult DeleteFile(string filePath);
        CommandResult CopyFileToTempPath(IFormFile file, string filePath, string newFilename = "");
        CommandResult SaveFileToNAS(string originFile, string destinyPath, bool deleteOriginFile = true);
        CommandResult<MemoryStream> GetZippedFiles(string filename);
        CommandResult<MemoryStream> GetZippedFiles(List<string> fileList);
    }
}
