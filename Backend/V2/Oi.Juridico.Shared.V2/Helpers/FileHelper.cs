using Humanizer.Bytes;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;

namespace Oi.Juridico.Shared.V2.Helpers
{
    public class FileHelper
    {
        public static double TamanhoArquivo(string caminhoArquivo)
        {
            try
            {
                var fileInfo = new FileInfo(caminhoArquivo);

                return ByteSize.FromBytes(fileInfo.Length).Megabytes;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static bool TamanhoMaximoExcedido(double tamanhoArquivo, double tamanhoMaximoArquivo)
        {
            try
            {
                return tamanhoArquivo > tamanhoMaximoArquivo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool ExtensaoArquivoInvalida(string nomeArquivo, string extensaoAceita)
        {
            try
            {
                var extensaoArquivo = Path.GetExtension(nomeArquivo);
                return extensaoArquivo != extensaoAceita;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool ExtensaoArquivoInvalida(string nomeArquivo, string[] extensoesAceitas)
        {
            try
            {
                if (extensoesAceitas is not null)
                {
                    var extensaoArquivo = Path.GetExtension(nomeArquivo);

                    return !(extensoesAceitas.Any(x => x == extensaoArquivo));
                }
                throw new Exception("Extensões aceitas não informadas.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string CopiaArquivoPastaTemporaria(IFormFile arquivo, string nomePasta, string novoNomeArquivo)
        {
            try
            {
                var caminhoTemporario = Path.Combine(Directory.GetCurrentDirectory(), nomePasta);
                //var caminhoTemp = Path.GetTempPath();
                //var caminhoTemporario = Path.Combine(caminhoTemp, nomePasta);

                if (!Directory.Exists(caminhoTemporario))
                {
                    Directory.CreateDirectory(caminhoTemporario);
                }

                using (var arquivoMemoria = new FileStream(caminhoTemporario, FileMode.Create))
                {
                    arquivo.CopyTo(arquivoMemoria);
                }

                if (File.Exists(caminhoTemporario))
                {
                    if (!string.IsNullOrEmpty(novoNomeArquivo))
                    {
                        string novoCaminhoArquivo = Path.Combine(Path.GetDirectoryName(caminhoTemporario), novoNomeArquivo);
                        File.Move(caminhoTemporario, novoCaminhoArquivo);
                        return novoCaminhoArquivo;
                    }
                }

                throw new Exception("Não foi possivel mover o arquivo para a pasta temporaria");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
