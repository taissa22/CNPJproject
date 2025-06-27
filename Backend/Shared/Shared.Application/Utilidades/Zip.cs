using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Shared.Application.Utilidades
{
    public static class Zip
    {
        public static void CompressGZIP(string path, string fileName)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo fileToCompress in di.GetFiles($"{fileName}*"))
            {
                using (FileStream originalFileStream = fileToCompress.OpenRead())
                {
                    if ((File.GetAttributes(fileToCompress.FullName) &
                       FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".zip")
                    {
                        using (FileStream compressedFileStream = File.Create(Path.Combine(di.FullName, fileName) + ".zip"))
                        {
                            using (GZipStream compressionStream = new GZipStream(compressedFileStream,
                               CompressionMode.Compress))
                            {
                                originalFileStream.CopyTo(compressionStream);
                            }
                        }
                    }
                }
            }
        }
    }
}
