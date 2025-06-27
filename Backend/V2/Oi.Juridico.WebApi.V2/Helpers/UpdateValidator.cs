using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oi.Juridico.WebApi.V2.Helpers
{
    public class UploadValidator
    {
        private const long MaxFileSize = 10 * 1024 * 1024;
        private const int ExpectedColumnCount = 31;

        public async Task<string> ValidarArquivoCSV(IFormFileCollection arquivos)
        {
            // Verifica se há arquivos na coleção
            if (arquivos == null || arquivos.Count == 0)
            {
                return "Nenhum arquivo foi enviado.";
            }

            // Acessa o primeiro arquivo da coleção (ajustar se necessário para múltiplos arquivos)
            var arquivo = arquivos[0];

            // Verifica o tipo de arquivo pelo nome e extensão
            if (Path.GetExtension(arquivo.FileName).ToLower() != ".csv")
            {
                return "O arquivo deve estar no formato CSV.";
            }

            // Verifica o tamanho do arquivo
            if (arquivo.Length > MaxFileSize)
            {
                return "O tamanho do arquivo não pode exceder 10MB.";
            }

            // Lê o arquivo e valida o número de colunas
            using (var stream = new StreamReader(arquivo.OpenReadStream(), Encoding.UTF8))
            {
                // Lê a primeira linha do arquivo
                string linha = await stream.ReadLineAsync();

                // Verifica se a linha não é nula
                if (linha == null)
                {
                    return "O arquivo está vazio ou corrompido.";
                }

                // Divide a linha usando a vírgula como separador
                var colunas = linha.Split(';');

                // Verifica se há o número esperado de colunas
                if (colunas.Length != ExpectedColumnCount)
                {
                    return $"A planilha importada não possui o número de colunas esperadas.";
                }
            }

            // Se passar em todas as validações, retorna sucesso
            return "";
        }
    }
}