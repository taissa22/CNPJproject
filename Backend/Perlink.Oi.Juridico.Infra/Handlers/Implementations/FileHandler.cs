using Humanizer.Bytes;
using Microsoft.AspNetCore.Http;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Perlink.Oi.Juridico.Infra.Handlers.Implementations {
    public class FileHandler : IFileHandler {        

        public CommandResult<double> FileSize(string filePath) {
            try {
                var fileInfo = new FileInfo(filePath);

                var fileSize = ByteSize.FromBytes(fileInfo.Length).Megabytes;

                return CommandResult<double>.Valid(fileSize);
            } catch (Exception ex) {
                return CommandResult<double>.Invalid(ex.Message);
            }
            
        }

        public CommandResult<bool> ExceedFileMaxSize(double fileSize, double maxFileSize) {
            try {
                var exceeded = fileSize > maxFileSize;

                return CommandResult<bool>.Valid(exceeded);        
                
            } catch (Exception ex) {
               return CommandResult<bool>.Invalid(ex.Message);
            }
        }

        public CommandResult<string> GetUniqueFilename(IFormFile file) {
            try {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName) + "_" + DateTime.Now.ToString("ddMMyyyyHmmss");

                var fileExtension = Path.GetExtension(file.FileName);

                return CommandResult<string>.Valid(Path.Combine(fileNameWithoutExtension + fileExtension));

            } catch (Exception ex) {
                return CommandResult<string>.Invalid(ex.Message);
            }            
        }

        public CommandResult<string> GetTempPath(string folderName) {
            try {
                var tempPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (!Directory.Exists(tempPath)) {
                    Directory.CreateDirectory(tempPath);
                }

                return CommandResult<string>.Valid(tempPath);

            } catch (Exception ex) {
                return CommandResult<string>.Invalid(ex.Message);
            }
        }

        public CommandResult<bool> FileExtensionInvalid(string fileName, string extensionToCompare) {
            try {
                var fileExtension = Path.GetExtension(fileName);

                var extensionInvalid = fileExtension != extensionToCompare;             

                return CommandResult<bool>.Valid(extensionInvalid);

            } catch (Exception ex) {
                return CommandResult<bool>.Invalid(ex.Message);
            }
        }

        public CommandResult CopyFileToTempPath(IFormFile file, string filePath, string newFilename = "") {
            try {
                using (var fileStream = new FileStream(filePath, FileMode.Create)) {
                    file.CopyTo(fileStream);
                }

                if (File.Exists(filePath)) {
                    if (!string.IsNullOrEmpty(newFilename)) {
                        string newFilePath = Path.Combine(Path.GetDirectoryName(filePath), newFilename);
                        File.Move(filePath, newFilePath);
                    }
                }                  

                return CommandResult.Valid();

            } catch (Exception ex) {
                return CommandResult.Invalid(ex.Message);
            }
        }

        public CommandResult SaveFileToNAS(string originFile, string destinyPath, bool deleteOriginFile = true) {
            try {
                if (!Directory.Exists(destinyPath)) {
                    Directory.CreateDirectory(destinyPath);
                }

                var filename = Path.GetFileName(originFile);
                var destinyFileName = Path.Combine(destinyPath, filename);

                File.Copy(originFile, destinyFileName);    
                
                if (deleteOriginFile) {
                    DeleteFile(originFile);
                }
  
                return CommandResult.Valid();

            } catch (Exception ex) {
                return CommandResult.Invalid($"Não foi possível salvar o arquivo no NAS - Erro: {ex.Message}");
            }
        }

        public CommandResult DeleteFile(string filePath) {
            try {
                if (File.Exists(filePath)) {
                    File.Delete(filePath);
                }

                return CommandResult.Valid();

            } catch (Exception ex) {
                return CommandResult.Invalid(ex.Message);
            }
        }

        public CommandResult<MemoryStream> GetZippedFiles(string file) {
            List<string> fileList = new List<string>();
            fileList.Add(file);            

            return GetZippedFiles(fileList);
        }


        public CommandResult<MemoryStream> GetZippedFiles(List<string> fileList) {
            using (MemoryStream finalZipFile = new MemoryStream()) {
                using (ZipArchive zip = new ZipArchive(finalZipFile, ZipArchiveMode.Create, true)) {
                    foreach (string file in fileList) {                        
                        string filename = Path.GetFileName(file);
                        ZipArchiveEntry readFile = zip.CreateEntry(filename);                        
                        using (MemoryStream memoryReadData = new MemoryStream(File.ReadAllBytes(file))) {
                            using (Stream readData = readFile.Open()) {
                                memoryReadData.CopyTo(readData);
                            }
                        }
                    }
                }
                return CommandResult<MemoryStream>.Valid(finalZipFile);
            }
        }

    }
}
