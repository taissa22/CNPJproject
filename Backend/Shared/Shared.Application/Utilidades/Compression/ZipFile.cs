using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using Shared.Domain.Impl;

namespace Shared.Application.Utilidades.Compression
{
    /// <summary>
    /// Creates a zip file.
    /// </summary>
    public class ZipFile
    {
        #region Fields
        List<String> files;
        Dictionary<String, Stream> streams;
        #endregion Fields

        #region Properties
        /// <summary>
        /// Files to zip.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<String> Files
        {
            get
            {
                if (files == null)
                {
                    files = new List<String>();
                }
                return files;
            }
        }
        /// <summary>
        /// Streams to zip. The key must be the name of the file in the Zip.
        /// </summary>
        public Dictionary<String, Stream> Streams
        {
            get
            {
                if (streams == null)
                {
                    streams = new Dictionary<String, Stream>();
                }
                return streams;
            }
        }
        #endregion Properties

        #region Save
        /// <summary>
        /// Generate the bytes of the stream
        /// </summary>
        /// <returns>Return bytes[]</returns>
        public byte[] GetBytes(ZipContentType contentType)
        {
            byte[] result = new byte[0];
            byte[] buffer = new byte[4096];
            var memory = new MemoryStream();
            using (ZipOutputStream s = new ZipOutputStream(memory))
            {
                s.SetLevel(9); // 0 - store only to 9 - means best compression                
                switch (contentType)
                {
                    case ZipContentType.Files:
                        foreach (String file in Files)
                        {
                            if (!File.Exists(file))
                            {
                                throw new ArgumentException(Textos.ZipFileToZippedNotExistsMessage);
                            }
                            ZipEntry entry = new ZipEntry(Path.GetFileName(file));
                            s.PutNextEntry(entry);
                            using (FileStream fs = File.OpenRead(file))
                            {
                                StreamUtils.Copy(fs, s, buffer);
                            }
                        }
                        break;
                    case ZipContentType.Stream:
                        foreach (String key in Streams.Keys)
                        {
                            if (String.IsNullOrEmpty(key))
                            {
                                throw new ArgumentException(Textos.ZipFileToZippedNotExistsMessage);
                            }
                            ZipEntry entry = new ZipEntry(key);
                            s.PutNextEntry(entry);
                            StreamUtils.Copy(Streams[key], s, buffer);
                        }
                        break;
                }
                s.Close();
                memory.Close();
                result = memory.ToArray();
            }
            return result;
        }
        /// <summary>
        /// Saves a new zip file with the included files.
        /// </summary>
        /// <param name="path">Path to save to.</param>
        public void Save(String path)
        {
            Save(path, ZipContentType.Files);
        }
        /// <summary>
        /// Saves a new zip file with the included files.
        /// </summary>
        /// <param name="path">Path to save to.</param>
        /// <param name="contentType">Indicate where to look for the content to be compressed.</param>
        public void Save(String path, ZipContentType contentType)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                throw new ArgumentException(Textos.ZipFolderNotExistsMessage);
            }
            byte[] buffer = new byte[4096];
            using (ZipOutputStream s = new ZipOutputStream(File.Create(path)))
            {
                s.SetLevel(9); // 0 - store only to 9 - means best compression                
                switch (contentType)
                {
                    case ZipContentType.Files:
                        foreach (String file in Files)
                        {
                            if (!File.Exists(file))
                            {
                                throw new ArgumentException(Textos.ZipFileToZippedNotExistsMessage);
                            }
                            ZipEntry entry = new ZipEntry(Path.GetFileName(file));
                            entry.Size = new FileInfo(file).Length;
                            s.PutNextEntry(entry);
                            using (FileStream fs = File.OpenRead(file))
                            {
                                StreamUtils.Copy(fs, s, buffer);
                            }
                        }
                        break;
                    case ZipContentType.Stream:
                        foreach (String key in Streams.Keys)
                        {
                            if (String.IsNullOrEmpty(key))
                            {
                                throw new ArgumentException(Textos.ZipFileToZippedNotExistsMessage);
                            }
                            ZipEntry entry = new ZipEntry(key);
                            s.PutNextEntry(entry);
                            StreamUtils.Copy(Streams[key], s, buffer);
                        }
                        break;
                }
                s.Close();
            }
        }
        public void SaveStreamAndFile(String path)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                throw new ArgumentException(Textos.ZipFolderNotExistsMessage);
            }
            byte[] buffer = new byte[4096];
            using (ZipOutputStream s = new ZipOutputStream(File.Create(path)))
            {
                s.SetLevel(9); // 0 - store only to 9 - means best compression                
                foreach (String file in Files)
                {
                    if (!File.Exists(file))
                    {
                        throw new ArgumentException(Textos.ZipFileToZippedNotExistsMessage);
                    }
                    ZipEntry entry = new ZipEntry(Path.GetFileName(file));
                    entry.Size = new FileInfo(file).Length;
                    s.PutNextEntry(entry);
                    using (FileStream fs = File.OpenRead(file))
                    {
                        StreamUtils.Copy(fs, s, buffer);
                    }
                }
                foreach (String key in Streams.Keys)
                {
                    if (String.IsNullOrEmpty(key))
                    {
                        throw new ArgumentException(Textos.ZipFileToZippedNotExistsMessage);
                    }
                    ZipEntry entry = new ZipEntry(key);
                    s.PutNextEntry(entry);
                    StreamUtils.Copy(Streams[key], s, buffer);
                }
                s.Close();
            }
        }
        #endregion Save
    }
}
