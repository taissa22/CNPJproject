using Perlink.Oi.Juridico.Domain.External.Interface;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.External.Service
{
    public class NasService : INasService
    {

        private readonly IParametroService parametroService;
        public NasService(IParametroService parametroService)
        {
            this.parametroService = parametroService;
        }

        /// <summary>
        /// Responsável por obter um arquivo no Storage do NAS
        /// 
        /// Exemplo de chamada: nasService.GetFileFromNAs('planilha_padrao_carga_01102019145132.xlsm', 'HON_DIR_SERV_CARGA_PAG');
        /// </summary>
        /// <param name="fileName">Nome do arquivo armazenado no NAS</param>
        /// <param name="parameterName">Código do parâmetro jurídico com o caminho onde o arquivo é armazenado</param>
        /// <returns>Retorna o arquivo em bytes</returns>
        public async Task<byte[]> GetFileFromNAs(string fileName, string parameterName)
        {
            var parameter = this.parametroService.RecuperarPorNome(parameterName).Conteudo;

            var pathsDir = TratarCaminhoDinamicoNoParametroJuridico(parameter);
            var finalFilePath = String.Empty;
            var filePath = String.Empty;
            foreach (var pathDir in pathsDir)
            {
                if (!Directory.Exists(pathDir))
                    continue;

                filePath = Path.Combine(pathDir, fileName);

                if (File.Exists(filePath))
                {
                    finalFilePath = filePath;
                    break;
                }
            }

            if (String.IsNullOrEmpty(finalFilePath))
            {
                throw new DirectoryNotFoundException($"Arquivo não encontrado no NAS: {filePath}");
            }

            return await File.ReadAllBytesAsync(finalFilePath);
        }

        /// <summary>
        /// Responsável por obter o path de um arquivo no Storage do NAS
        /// 
        /// Exemplo de chamada: nasService.GetFilePathFromNAs('planilha_padrao_carga_01102019145132.xlsm', 'HON_DIR_SERV_CARGA_PAG');
        /// </summary>
        /// <param name="fileName">Nome do arquivo armazenado no NAS</param>
        /// <param name="parameterName">Código do parâmetro jurídico com o caminho onde o arquivo é armazenado</param>
        /// <returns>Retorna o caminho do arquivo</returns>
        public string GetFilePathFromNAs(string fileName, string parameterName) {
            var parameter = this.parametroService.RecuperarPorNome(parameterName).Conteudo;

            var pathsDir = TratarCaminhoDinamicoNoParametroJuridico(parameter);
            var finalFilePath = String.Empty;
            var filePath = String.Empty;
            foreach (var pathDir in pathsDir) {
                if (!Directory.Exists(pathDir))
                    continue;

                filePath = Path.Combine(pathDir, fileName);

                if (File.Exists(filePath)) {
                    finalFilePath = filePath;
                    break;
                }
            }

            if (String.IsNullOrEmpty(finalFilePath)) {
                throw new DirectoryNotFoundException($"Arquivo não encontrado no NAS: {filePath}");
            }

            return finalFilePath;
        }

        public void SaveFileFromTempDir(string fromFile, string toPath)
        {
            string pathDir = TratarCaminhoDinamicoNoParametroJuridico(toPath).Dequeue();

            if (!Directory.Exists(pathDir))
            {
                Directory.CreateDirectory(pathDir);
            }

            var fileName = Path.GetFileName(fromFile);

            var toFile = Path.Combine(pathDir, fileName);

            File.Copy(fromFile, toFile);

            File.Delete(fromFile);
        }

        /*
       * STI 84291 - Múltiplos storages para armazenamento de arquivos
       * Método resposável por retornar uma fila de diretórios a serem utilizados na 
       * busca de arquivos físicos em vários storages.
       * 
       * A regra de formação é:
       * 
       * {STORAGE1|STORAGE2}\PASTA1\PASTA2
       * 
       * A parte dinâmica é a string entre chaves que vai ser quebrada em vários pontos
       * para localização de arquivos.
       * 
       * É utilizada queue para garantir first in first out (FIFO). Para o upload de
       * arquivos será utilizado o primeiro storage identificado.
       * 
       * Caso não tenha valores dinâmicos (sem as chaves), retorna o próprio path.
       * 
       */
        public Queue<string> TratarCaminhoDinamicoNoParametroJuridico(string path)
        {
            var queueDeStorages = new Queue<string>();

            // Expressão regular que recupera a parte dinâmica do diretório.
            Match match = Regex.Match(path, @"{(.*?)}");

            if (match.Groups.Count > 1)
            {
                string valorDinamico = match.Groups[1].Value;
                string[] matchs = valorDinamico.Split('|');

                // Para cada token recuperado no split, cria o endereço completo com as duas ou mais opções.
                foreach (string m in matchs)
                {
                    var pathNas = this.parametroService.RecuperarPorNome(m.ToUpper()).Conteudo;
                    queueDeStorages.Enqueue(path.Replace(string.Format("{{{0}}}", valorDinamico), pathNas));
                }
            }
            else
            {
                queueDeStorages.Enqueue(path);
            }

            return queueDeStorages;
        }
    }
}