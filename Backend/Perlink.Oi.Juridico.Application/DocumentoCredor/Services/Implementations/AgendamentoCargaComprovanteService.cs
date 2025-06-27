using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
using System.IO;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.DocumentoCredor.Services.Implementations {

    internal class AgendamentoCargaComprovanteService : IAgendamentoCargaComprovanteService {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IAgendamentoCargaComprovanteService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }
        private IFileHandler FileHandler { get; }
        private IParametroJuridicoProvider ParametroJuridico { get; }

        private IXlsxHandler XlsxHandler { get; }

        public AgendamentoCargaComprovanteService(IDatabaseContext databaseContext,
            ILogger<IAgendamentoCargaComprovanteService> logger, IUsuarioAtualProvider usuarioAtual,
            IFileHandler fileHandler, IParametroJuridicoProvider parametroJuridico,
            IXlsxHandler xlsxHandler) {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
            FileHandler = fileHandler;
            ParametroJuridico = parametroJuridico;
            XlsxHandler = xlsxHandler;
        }

        public CommandResult<bool> ValidarBaseSAP(IFormFile baseSAP) {
            void DeletaArquvosTemporarios(string caminhoArquivos) {
                if (Directory.Exists(caminhoArquivos)) {
                    Directory.Delete(caminhoArquivos, true);
                }
            }
            string caminhoTemporarioRaiz = string.Empty;
            try {
                string caminhoTemporarioBaseSAP = FileHandler.GetTempPath(Path.Combine("DocumentoCredores", "Comprovantes", "Arquivos")).Dados;
                caminhoTemporarioRaiz = FileHandler.GetTempPath("DocumentoCredores").Dados;
                string nomeArquivoTemporarioBaseSAP = Path.Combine(caminhoTemporarioBaseSAP, baseSAP.FileName);
                FileHandler.CopyFileToTempPath(baseSAP, nomeArquivoTemporarioBaseSAP);

                if (!File.Exists(nomeArquivoTemporarioBaseSAP)) {
                    return CommandResult<bool>.Invalid("Não foi possível testar o arquivo");
                }

                int colummNumber = 16;

                bool arquivoVazio = XlsxHandler.ArquivoVazio(nomeArquivoTemporarioBaseSAP).Dados;
                if (arquivoVazio) {
                    DeletaArquvosTemporarios(caminhoTemporarioRaiz);
                    return CommandResult<bool>.Invalid("O arquivo .xlsx importado não possui registros para carga");
                }
                bool layoutInvalido = !XlsxHandler.LayoutValido(nomeArquivoTemporarioBaseSAP, colummNumber).Dados;
                if (layoutInvalido) {
                    DeletaArquvosTemporarios(caminhoTemporarioRaiz);
                    return CommandResult<bool>.Invalid("A base SAP importada não possui a quantidade de colunas esperadas pelo sistema");
                }

                DeletaArquvosTemporarios(caminhoTemporarioRaiz);

                return CommandResult<bool>.Valid(true);
            } catch (Exception ex) {
                DeletaArquvosTemporarios(caminhoTemporarioRaiz);
                return CommandResult<bool>.Invalid(ex.Message);
            }
        }

        private CommandResult SalvaArquivosNAS(IFormFile comprovantes, IFormFile baseSAP, AgendamentoCargaComprovante agendamentoCargaComprovante) {
            void DeletaArquivosTemporarios(string caminhoArquivos) {
                if (Directory.Exists(caminhoArquivos)) {
                    Directory.Delete(caminhoArquivos, true);
                }
            }

            string operacao = "Salvar arquivos no NAS";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(operacao));

            string caminhoArquivoTemporarioComprovantes;
            string caminhoArquivoTemporarioBaseSAP;
            string caminhoTemporario;
            var caminhoTemporarioRaiz = string.Empty;
            try {
                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Parametros Jurídicos"));
                double.TryParse(ParametroJuridico.Obter(ParametrosJuridicos.TAM_MAX_CARGA_COMPROVANTE).Dados.Conteudo, out double parametroTamanhoArquivo);
                var parametroCaminhoNasCargaComprovantes = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_CARGA_COMPROVANTE).Dequeue();

                if (string.IsNullOrEmpty(parametroCaminhoNasCargaComprovantes)) {
                    string message = $"Parametro Jurídico {ParametrosJuridicos.DIR_NAS_CARGA_COMPROVANTE} inválido.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                if (FileHandler.FileExtensionInvalid(comprovantes.FileName, FileExtensions.PDF_EXTENSION).Dados) {
                    string message = "Extensão do arquivo de comprovantes inválida.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                if (FileHandler.FileExtensionInvalid(baseSAP.FileName, FileExtensions.XLSX_EXTENSION).Dados) {
                    string message = "Extensão do arquivo de Base SAP inválida.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Caminho diretório temporário"));
                caminhoTemporario = FileHandler.GetTempPath(Path.Combine("DocumentoCredores", "Comprovantes", "Arquivos")).Dados;
                caminhoTemporarioRaiz = FileHandler.GetTempPath("DocumentoCredores").Dados;

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Nomes únicos para os arquivos"));
                var nomeArquivoComprovantes = FileHandler.GetUniqueFilename(comprovantes).Dados;
                var nomeArquivoBaseSAP = FileHandler.GetUniqueFilename(baseSAP).Dados;

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Caminhos temporários para os arquivos"));
                caminhoArquivoTemporarioComprovantes = Path.Combine(caminhoTemporario, nomeArquivoComprovantes);
                caminhoArquivoTemporarioBaseSAP = Path.Combine(caminhoTemporario, nomeArquivoBaseSAP);

                Logger.LogInformation("Copiando arquivos para diretório temporário");
                FileHandler.CopyFileToTempPath(comprovantes, caminhoArquivoTemporarioComprovantes);
                FileHandler.CopyFileToTempPath(baseSAP, caminhoArquivoTemporarioBaseSAP);

                Logger.LogInformation("Verificando tamanho dos arquivos");
                var tamanhoArquivos = FileHandler.FileSize(caminhoArquivoTemporarioComprovantes).Dados + FileHandler.FileSize(caminhoArquivoTemporarioBaseSAP).Dados;

                if (FileHandler.ExceedFileMaxSize(tamanhoArquivos, parametroTamanhoArquivo).Dados) {
                    string message = $"Tamanho dos arquivos importados ultrapassa o limite permitido de {parametroTamanhoArquivo}mb.";
                    Logger.LogError("Deletendo arquivos temporários");
                    DeletaArquivosTemporarios(caminhoTemporarioRaiz);
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                Logger.LogInformation("Salvando arquivos no NAS");
                FileHandler.SaveFileToNAS(caminhoArquivoTemporarioComprovantes, parametroCaminhoNasCargaComprovantes);
                FileHandler.SaveFileToNAS(caminhoArquivoTemporarioBaseSAP, parametroCaminhoNasCargaComprovantes);

                DeletaArquivosTemporarios(caminhoTemporarioRaiz);

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Atualizando entidade com arquivos salvos"));
                agendamentoCargaComprovante.AtualizarNomesArquivosDeCarga(nomeArquivoComprovantes, nomeArquivoBaseSAP);

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(operacao));
                return CommandResult.Valid();
            } catch (Exception ex) {
                Logger.LogError("Deletendo arquivos temporários");

                DeletaArquivosTemporarios(caminhoTemporarioRaiz);

                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(operacao));
                return CommandResult.Invalid(ex.Message);
            }
        }

        private CommandResult RemoveArquivoNAS(AgendamentoCargaComprovante agendamentoCargaComprovante) {
            string operacao = "Excluir arquivos do NAS";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(operacao));
            try {
                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Parametros Jurídicos"));
                var parametroCaminhosNasCargaComprovantes = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_CARGA_COMPROVANTE);

                if (parametroCaminhosNasCargaComprovantes.Count <= 0) {
                    Logger.LogInformation("Diretório Nas não encontrado");
                    return CommandResult<byte[]>.Invalid("Diretório NAS não encontrado");
                }

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Arquivos para exclusão"));
                var caminhoArquivoComprovantes = string.Empty;
                var caminhoArquivoBaseSAP = string.Empty;

                foreach (var caminho in parametroCaminhosNasCargaComprovantes) {
                    if (!Directory.Exists(caminho)) {
                        continue;
                    }                    
                    if (File.Exists(Path.Combine(caminho, agendamentoCargaComprovante.NomeArquivoComprovante))) {
                        caminhoArquivoComprovantes = Path.Combine(caminho, agendamentoCargaComprovante.NomeArquivoComprovante);
                        break;
                    }
                }

                foreach (var caminho in parametroCaminhosNasCargaComprovantes) {
                    if (!Directory.Exists(caminho)) {
                        continue;
                    }

                    if (File.Exists(Path.Combine(caminho, agendamentoCargaComprovante.NomeArquivoBaseSap))) {
                        caminhoArquivoBaseSAP = Path.Combine(caminho, agendamentoCargaComprovante.NomeArquivoBaseSap);
                        break;
                    }
                }

                Logger.LogInformation("Apagando arquivos");
                FileHandler.DeleteFile(caminhoArquivoComprovantes);
                FileHandler.DeleteFile(caminhoArquivoBaseSAP);

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(operacao));
                return CommandResult.Valid();
            } catch (Exception ex) {
                Logger.LogError(Infra.Extensions.Logs.OperacaoComErro(operacao));
                return CommandResult.Invalid(ex.Message);
            }
        }

        public CommandResult Remover(int agendamentoCargaComprovanteId) {
            string entityName = "Agendamento de Carga de Comprovantes";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_CARGA_COMPROVANTES)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_CARGA_COMPROVANTES, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, agendamentoCargaComprovanteId));
                AgendamentoCargaComprovante agendamentoCargaComprovante = DatabaseContext.AgendamentosCargasComprovantes
                                                                                         .FirstOrDefault(x => x.Id == agendamentoCargaComprovanteId);

                if (agendamentoCargaComprovante is null) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, agendamentoCargaComprovanteId));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, agendamentoCargaComprovanteId));
                }

                string commandRemocao = $"Validar remoção {entityName}";
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandRemocao));

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Parâmetro Jurídico"));
                int paramDias = Convert.ToInt32(ParametroJuridico.Obter(ParametrosJuridicos.DIAS_EXPURGO_CARGA_COMPROVANTE).Dados.Conteudo);
                DateTime dataExpurgo = DateTime.Today.AddDays(-paramDias);

                if ((agendamentoCargaComprovante.StatusAgendamentoId != StatusAgendamento.AGENDADO.Id
                     && agendamentoCargaComprovante.StatusAgendamentoId != StatusAgendamento.ERRO.Id)
                                        && agendamentoCargaComprovante.DataAgendamento > dataExpurgo) {
                    string message = $"Somente agendamentos com status 'Agendado' ou 'Erro' poderão ser excluídos.";
                    Logger.LogInformation(Infra.Extensions.Logs.ValidandoRegras());
                    return CommandResult.Invalid(message);
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, agendamentoCargaComprovanteId));
                DatabaseContext.Remove(agendamentoCargaComprovante);

                RemoveArquivoNAS(agendamentoCargaComprovante);

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));
                DatabaseContext.SaveChanges();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                return CommandResult.Valid();
            } catch (Exception ex) {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        public CommandResult Criar(IFormFile comprovantes, IFormFile baseSAP) {
            string entityName = "Carga de Comprovantes de Pagamento";
            string commandName = $"Criar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));
            try {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_CARGA_COMPROVANTES)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_CARGA_COMPROVANTES, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                AgendamentoCargaComprovante agendamentoCargaComprovante = AgendamentoCargaComprovante.Criar(UsuarioAtual.Login);

                if (agendamentoCargaComprovante.HasNotifications) {
                    return CommandResult.Invalid(agendamentoCargaComprovante.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(agendamentoCargaComprovante);
                
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao("Salva Arquivos no diretório NAS"));
                var salvaArquivoNAS = SalvaArquivosNAS(comprovantes, baseSAP, agendamentoCargaComprovante);

                if (!salvaArquivoNAS.IsValid) {
                    Logger.LogInformation("Erros ao salvar no NAS: ", salvaArquivoNAS.Mensagens);
                    return CommandResult.Invalid(salvaArquivoNAS.Mensagens);
                }

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));
                DatabaseContext.SaveChanges();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));

                return CommandResult.Valid();
            } catch (Exception ex) {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }      
    }
}