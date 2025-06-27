using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data
{
    public partial class ParametroJuridicoContext : DbContext
    {
        public async Task<string> RecuperaConteudoParametroJuridicoPorId(string parametroId)
        {
            try
            {
                var conteudo = await ParametroJuridico.Where(x => x.CodParametro == parametroId).Select(x => x.DscConteudoParametro).FirstAsync();

                return conteudo;
            }
            catch (Exception)
            {
                throw new Exception("Parâmetro Jurídico não encontado");
            }
            
        }

        public async Task<string> RecuperaConteudoParametroJuridicoPorListaDeId(List<string> lst_parametros)
        {
            try
            {
                // Obtém os conteúdos que correspondem aos parâmetros na lista
                var conteudos = await ParametroJuridico
                    .Where(x => lst_parametros.Contains(x.CodParametro))
                    .Select(x => x.DscConteudoParametro)
                    .ToListAsync();

                return string.Join(",", conteudos);
            }
            catch (Exception ex)
            {
                throw new Exception("Parâmetro Jurídico não encontrado: " + ex.Message);
            }
        }

        public async Task<Queue<string>> TratarCaminhoDinamicoAsync(string parametroId, string nomeArquivo = "")
        {
            var queueDeStorages = new Queue<string>();

            var conteudo = await ParametroJuridico.Where(x => x.CodParametro == parametroId).Select(x => x.DscConteudoParametro).FirstAsync();

            // Expressão regular que recupera a parte dinâmica do diretório.
            Match match = Regex.Match(conteudo, @"{(.*?)}");

            if (match.Groups.Count > 1)
            {
                string valorDinamico = match.Groups[1].Value;
                string[] matchs = valorDinamico.Split('|');

                // Para cada token recuperado no split, cria o endereço completo com as duas ou mais opções.
                foreach (string m in matchs)
                {
                    var pathNas = await ParametroJuridico.Where(x => x.CodParametro == m).Select(x => x.DscConteudoParametro).FirstAsync();
                    var path = nomeArquivo == ""
                        ? conteudo.Replace(string.Format("{{{0}}}", valorDinamico), pathNas)
                        : Path.Combine(conteudo.Replace(string.Format("{{{0}}}", valorDinamico), pathNas), nomeArquivo);
                    queueDeStorages.Enqueue(path);
                }
            }
            else
            {
                queueDeStorages.Enqueue(conteudo);
            }

            return queueDeStorages;
        }

        public async Task<string[]> TratarCaminhoDinamicoArrayAsync(string parametroId, string nomeArquivo = "", CancellationToken ct = default)
        {
            var queueDeStorages = new List<string>();

            var conteudo = await ParametroJuridico.Where(x => x.CodParametro == parametroId).Select(x => x.DscConteudoParametro).FirstAsync(ct);

            // Expressão regular que recupera a parte dinâmica do diretório.
            Match match = Regex.Match(conteudo, @"{(.*?)}");

            if (match.Groups.Count > 1)
            {
                string valorDinamico = match.Groups[1].Value;
                string[] matchs = valorDinamico.Split('|');

                // Para cada token recuperado no split, cria o endereço completo com as duas ou mais opções.
                foreach (string m in matchs)
                {
                    var pathNas = await ParametroJuridico.Where(x => x.CodParametro == m).Select(x => x.DscConteudoParametro).FirstAsync(ct);
                    var path = nomeArquivo == ""
                        ? conteudo.Replace(string.Format("{{{0}}}", valorDinamico), pathNas)
                        : Path.Combine(conteudo.Replace(string.Format("{{{0}}}", valorDinamico), pathNas), nomeArquivo);
                    queueDeStorages.Add(path);
                }
            }
            else
            {
                queueDeStorages.Add(conteudo);
            }

            return queueDeStorages.ToArray();
        }

    }
}
