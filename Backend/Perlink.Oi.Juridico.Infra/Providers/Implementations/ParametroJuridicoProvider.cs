using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Perlink.Oi.Juridico.Infra.Providers.Implementations {

    public class ParametroJuridicoProvider : IParametroJuridicoProvider {
        private Lazy<IDatabaseContext> LazyContext { get; }
        private IDatabaseContext DatabaseContext => LazyContext.Value;

        public ParametroJuridicoProvider(Lazy<IDatabaseContext> lazyContext) {
            LazyContext = lazyContext;
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

        public Queue<string> TratarCaminhoDinamico(string parametroId) {
            var queueDeStorages = new Queue<string>();

            var path = Obter(parametroId).Dados.Conteudo;

            // Expressão regular que recupera a parte dinâmica do diretório.
            Match match = Regex.Match(path, @"{(.*?)}");

            if (match.Groups.Count > 1) {
                string valorDinamico = match.Groups[1].Value;
                string[] matchs = valorDinamico.Split('|');

                // Para cada token recuperado no split, cria o endereço completo com as duas ou mais opções.
                foreach (string m in matchs) {
                    var pathNas = Obter(m).Dados.Conteudo;
                    queueDeStorages.Enqueue(path.Replace(string.Format("{{{0}}}", valorDinamico), pathNas));
                }
            } else {
                queueDeStorages.Enqueue(path);
            }
            return queueDeStorages;
        }

        public CommandResult<ParametroJuridico> Obter(string parametroId) {
            try {
                return CommandResult<ParametroJuridico>.Valid(DatabaseContext.ParametrosJuridicos
                    .FirstOrDefault(x => x.Parametro == parametroId.ToUpper()));
            } catch (Exception ex) {
                return CommandResult<ParametroJuridico>.Invalid(ex.Message);
            }
        }
    }
}