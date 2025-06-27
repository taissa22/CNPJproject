using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Handlers;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.SAP.Services.Implementations
{
    internal class AgendamentosMigracaoPedidosSapService : IAgendamentosMigracaoPedidosSapService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IAgendamentosMigracaoPedidosSapService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }
        private IFileHandler FileHandler { get; }
        private IParametroJuridicoProvider ParametroJuridico { get; }
        private ICsvHandler CsvHandler { get; }

        public AgendamentosMigracaoPedidosSapService(IDatabaseContext databaseContext,
            ILogger<IAgendamentosMigracaoPedidosSapService> logger, IUsuarioAtualProvider usuarioAtual,
            IFileHandler fileHandler, IParametroJuridicoProvider parametroJuridico,
            ICsvHandler csvHandler)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
            FileHandler = fileHandler;
            ParametroJuridico = parametroJuridico;
            CsvHandler = csvHandler;
        }

        private CommandResult SalvaArquivosNAS(IFormFile Migracao, AgendamentoMigracaoPedidosSap agendamentoMigracaoPedidosSap)
        {
            void DeletaArquivosTemporarios(string caminhoArquivos)
            {
                if (Directory.Exists(caminhoArquivos))
                {
                    Directory.Delete(caminhoArquivos, true);
                }
            }

            string operacao = "Salvar arquivos no NAS";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(operacao));

            string caminhoArquivoTemporarioMigracao;
            string caminhoTemporario;
            string caminhoTemporarioRaiz = string.Empty;
            try
            {
                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Parametros Jurídicos"));
                double.TryParse(ParametroJuridico.Obter(ParametrosJuridicos.TAM_MAX_MIGRAR_PEDIDOS).Dados.Conteudo, out double parametroTamanhoArquivo);
                var parametroCaminhoNasMigracaoPedidosSap = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_MIGRACAO_PED_SAP).Dequeue();

                if (string.IsNullOrEmpty(parametroCaminhoNasMigracaoPedidosSap))
                {
                    string message = $"Parametro Jurídico {ParametrosJuridicos.DIR_NAS_MIGRACAO_PED_SAP} inválido.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                if (FileHandler.FileExtensionInvalid(Migracao.FileName, FileExtensions.CSV_EXTENSION).Dados)
                {
                    string message = "O arquivo importado não é .CSV.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Caminho diretório temporário"));
                caminhoTemporario = FileHandler.GetTempPath(Path.Combine("MigracaoPedidosSap", "Arquivos")).Dados;
                caminhoTemporarioRaiz = FileHandler.GetTempPath("MigracaoPedidosSap").Dados;

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Nomes únicos para os arquivos"));
                var nomeArquivoMigracao = FileHandler.GetUniqueFilename(Migracao).Dados;

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Caminhos temporários para os arquivos"));
                caminhoArquivoTemporarioMigracao = Path.Combine(caminhoTemporario, nomeArquivoMigracao);

                Logger.LogInformation("Copiando arquivos para diretório temporário");
                FileHandler.CopyFileToTempPath(Migracao, caminhoArquivoTemporarioMigracao);

                Logger.LogInformation("Verificando tamanho dos arquivos");
                var tamanhoArquivos = FileHandler.FileSize(caminhoArquivoTemporarioMigracao).Dados;


                if (CsvHandler.ArquivoVazio(caminhoArquivoTemporarioMigracao).Dados)
                {
                    DeletaArquivosTemporarios(caminhoTemporarioRaiz);
                    return CommandResult<bool>.Invalid("O arquivo CSV importado não possui registros para carga.");
                }

                int colummNumber = 2;

                if (!CsvHandler.LayoutValido(caminhoArquivoTemporarioMigracao, colummNumber).Dados)
                {
                    DeletaArquivosTemporarios(caminhoTemporarioRaiz);
                    return CommandResult<bool>.Invalid("O arquivo importado não possui a quantidade de colunas esperadas pelo sistema.");
                }

                if (FileHandler.ExceedFileMaxSize(tamanhoArquivos, parametroTamanhoArquivo).Dados)
                {
                    string message = $"O arquivo CSV importado ultrapassa o limite permitido de {parametroTamanhoArquivo}MB.";
                    Logger.LogInformation("Deletendo arquivos temporários");

                    DeletaArquivosTemporarios(caminhoTemporarioRaiz);

                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                Logger.LogInformation("Salvando arquivos no NAS");
                var retorno = FileHandler.SaveFileToNAS(caminhoArquivoTemporarioMigracao, parametroCaminhoNasMigracaoPedidosSap);

                if (!retorno.IsValid)
                {
                    Logger.LogError(string.Join(',',retorno.Mensagens));
                    return CommandResult.Invalid(string.Join(',', retorno.Mensagens));
                }

                Logger.LogError("Deletendo arquivos temporários");
                DeletaArquivosTemporarios(caminhoTemporarioRaiz);

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Atualizando entidade com arquivos salvos"));
                agendamentoMigracaoPedidosSap.AtualizarNomesDocumento(nomeArquivoMigracao);

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(operacao));
                return CommandResult.Valid();
            }
            catch (Exception ex)
            {
                Logger.LogError("Deletendo arquivos temporários");
                DeletaArquivosTemporarios(caminhoTemporarioRaiz);

                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(operacao));
                return CommandResult.Invalid(ex.Message);
            }
        }

        private CommandResult RemoveArquivoNAS(AgendamentoMigracaoPedidosSap agendamentoMigracaoPedidosSap)
        {
            string operacao = "Excluir arquivos do NAS";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(operacao));
            try
            {
                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Parametros Jurídicos"));
                var parametroCaminhosNasMigracaoPedidosSap = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_MIGRACAO_PED_SAP);

                var caminhoArquivoMigracao = string.Empty;
                var caminhoArquivoResultado = string.Empty;

                if (parametroCaminhosNasMigracaoPedidosSap.Count <= 0)
                {
                    Logger.LogInformation("Diretório Nas não encontrado");
                    return CommandResult<string>.Invalid("Diretório NAS não encontrado");
                }

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Arquivos para exclusão"));
                foreach (var caminho in parametroCaminhosNasMigracaoPedidosSap)
                {
                    if (!Directory.Exists(caminho))
                        continue;

                    if (File.Exists(Path.Combine(caminho, agendamentoMigracaoPedidosSap.NomeArquivoDeParaMigracao)))
                    {
                        caminhoArquivoMigracao = Path.Combine(caminho, agendamentoMigracaoPedidosSap.NomeArquivoDeParaMigracao);
                        break;
                    }
                }


                if (!string.IsNullOrEmpty(caminhoArquivoMigracao))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.Obtendo($"Deletando o arquivo {agendamentoMigracaoPedidosSap.NomeArquivoDeParaMigracao}"));
                    FileHandler.DeleteFile(caminhoArquivoMigracao);
                    Logger.LogInformation($"Arquivo {caminhoArquivoMigracao} excluído.");
                }

                foreach (var caminho in parametroCaminhosNasMigracaoPedidosSap)
                {
                    if (!Directory.Exists(caminho))
                        continue;

                    if (File.Exists(Path.Combine(caminho, agendamentoMigracaoPedidosSap.NomeArquivoGerado)))
                    {
                        caminhoArquivoResultado = Path.Combine(caminho, agendamentoMigracaoPedidosSap.NomeArquivoGerado);
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(caminhoArquivoResultado))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.Obtendo($"Deletando o arquivo {agendamentoMigracaoPedidosSap.NomeArquivoGerado}"));
                    FileHandler.DeleteFile(caminhoArquivoResultado);
                    Logger.LogInformation($"Arquivo {caminhoArquivoResultado} excluído.");
                }

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(operacao));
                return CommandResult.Valid();
            }
            catch (Exception ex)
            {
                Logger.LogError(Infra.Extensions.Logs.OperacaoComErro(operacao));
                return CommandResult.Invalid(ex.Message);
            }
        }

        /// <summary>
        /// Rotina que remove o arquivo tanto do BD quando localmente salvo.
        /// </summary>
        /// <param name="agendamentoMigracaoPedidosSapId">Id do registro a ser excluído</param>
        /// <returns></returns>
        public CommandResult Remover(int agendamentoMigracaoPedidosSapId)
        {
            string entityName = "Agendamento de Migração Pedidos SAP";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_MIGRACAO_PEDIDOS_SAP))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_MIGRACAO_PEDIDOS_SAP, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, agendamentoMigracaoPedidosSapId));
                AgendamentoMigracaoPedidosSap agendamentoMigracaoPedidosSap = DatabaseContext.AgendamentosMigracaoPedidosSap
                                                                    .FirstOrDefault(x => x.Id == agendamentoMigracaoPedidosSapId);


                if (agendamentoMigracaoPedidosSap is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, agendamentoMigracaoPedidosSapId));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, agendamentoMigracaoPedidosSapId));
                }


                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Validar remoção {entityName}"));

                //Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Parâmetro Jurídico"));
                //int paramDias = Convert.ToInt32(ParametroJuridico.Obter(ParametrosJuridicos.DIAS_EXPURGO_CARGA_DOC).Dados.Conteudo);
                //DateTime dataExpurgo = DateTime.Today.AddDays(-paramDias);

                if (agendamentoMigracaoPedidosSap.StatusAgendamentoId != StatusAgendamento.AGENDADO.Id
                     && agendamentoMigracaoPedidosSap.StatusAgendamentoId != StatusAgendamento.ERRO.Id)
                {
                    string message = $"Somente agendamentos com status 'Agendado' e 'Erro' poderão ser excluídos.";
                    Logger.LogInformation(Infra.Extensions.Logs.ValidandoRegras());
                    return CommandResult.Invalid(message);
                }
                else
                {
                    Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, agendamentoMigracaoPedidosSapId));
                    DatabaseContext.Remove(agendamentoMigracaoPedidosSap);
                }

                RemoveArquivoNAS(agendamentoMigracaoPedidosSap);

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));
                DatabaseContext.SaveChanges();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                return CommandResult.Valid();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        public CommandResult Criar(IFormFile documentos)
        {
            string entityName = "Migração Pedidos SAP";
            string commandName = $"Criar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));
            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_MIGRACAO_PEDIDOS_SAP))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_MIGRACAO_PEDIDOS_SAP, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                AgendamentoMigracaoPedidosSap agendamentoMigracaoPedidosSap = AgendamentoMigracaoPedidosSap.Criar(UsuarioAtual.Login);

                if (agendamentoMigracaoPedidosSap.HasNotifications)
                {
                    return CommandResult.Invalid(agendamentoMigracaoPedidosSap.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(agendamentoMigracaoPedidosSap);

                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao("Salva Arquivos no diretório NAS"));
                var salvaArquivoNAS = SalvaArquivosNAS(documentos, agendamentoMigracaoPedidosSap);

                if (!salvaArquivoNAS.IsValid)
                {
                    Logger.LogInformation("Erros ao salvar no NAS: ", salvaArquivoNAS.Mensagens);
                    return CommandResult.Invalid(string.Join(',',salvaArquivoNAS.Mensagens));
                }

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));
                DatabaseContext.SaveChanges();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));

                return CommandResult.Valid();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        
        
    }
}
