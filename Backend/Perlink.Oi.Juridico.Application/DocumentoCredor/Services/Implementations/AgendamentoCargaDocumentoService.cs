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

namespace Perlink.Oi.Juridico.Application.DocumentoCredor.Services.Implementations {
    internal class AgendamentoCargaDocumentoService : IAgendamentoCargaDocumentoService {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IAgendamentoCargaDocumentoService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }
        private IFileHandler FileHandler { get; }
        private IParametroJuridicoProvider ParametroJuridico { get; }
        private ICsvHandler CsvHandler { get; }

        public AgendamentoCargaDocumentoService(IDatabaseContext databaseContext,
            ILogger<IAgendamentoCargaDocumentoService> logger, IUsuarioAtualProvider usuarioAtual,
            IFileHandler fileHandler, IParametroJuridicoProvider parametroJuridico,
            ICsvHandler csvHandler) {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
            FileHandler = fileHandler;
            ParametroJuridico = parametroJuridico;
            CsvHandler = csvHandler;
        }

        private CommandResult SalvaArquivosNAS(IFormFile documentos, AgendamentoCargaDocumento agendamentoCargaDocumento) {
            void DeletaArquivosTemporarios(string caminhoArquivos) {
                if (Directory.Exists(caminhoArquivos)) {
                    Directory.Delete(caminhoArquivos, true);
                }
            }

            string operacao = "Salvar arquivos no NAS";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(operacao));

            string caminhoArquivoTemporarioDocumentos = string.Empty;
            string caminhoTemporario = string.Empty;
            string caminhoTemporarioRaiz = string.Empty;
            try {
                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Parametros Jurídicos"));
                double.TryParse(ParametroJuridico.Obter(ParametrosJuridicos.TAM_MAX_CARGA_DOCUMENTOS).Dados.Conteudo, out double parametroTamanhoArquivo);
                var parametroCaminhoNasCargaDocumentos = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_CARGA_DOCUMENTOS).Dequeue();

                if (string.IsNullOrEmpty(parametroCaminhoNasCargaDocumentos)) {
                    string message = $"Parametro Jurídico {ParametrosJuridicos.DIR_NAS_CARGA_DOCUMENTOS} inválido.";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                if (FileHandler.FileExtensionInvalid(documentos.FileName, FileExtensions.CSV_EXTENSION).Dados) {
                    string message = "O arquivo importado não é .csv";
                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Caminho diretório temporário"));
                caminhoTemporario = FileHandler.GetTempPath(Path.Combine("Documentos", "Arquivos")).Dados;
                caminhoTemporarioRaiz = FileHandler.GetTempPath("Documentos").Dados;

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Nomes únicos para os arquivos"));
                var nomeArquivoDocumentos = FileHandler.GetUniqueFilename(documentos).Dados;

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Caminhos temporários para os arquivos"));
                caminhoArquivoTemporarioDocumentos = Path.Combine(caminhoTemporario, nomeArquivoDocumentos);

                Logger.LogInformation("Copiando arquivos para diretório temporário");
                FileHandler.CopyFileToTempPath(documentos, caminhoArquivoTemporarioDocumentos);

                Logger.LogInformation("Verificando tamanho dos arquivos");
                var tamanhoArquivos = FileHandler.FileSize(caminhoArquivoTemporarioDocumentos).Dados;


                if (CsvHandler.ArquivoVazio(caminhoArquivoTemporarioDocumentos).Dados) {
                    DeletaArquivosTemporarios(caminhoTemporarioRaiz);
                    return CommandResult<bool>.Invalid("O arquivo CSV importado não possui registros para carga");
                }

                int colummNumber = 3;

                if (!CsvHandler.LayoutValido(caminhoArquivoTemporarioDocumentos, colummNumber).Dados) {
                    DeletaArquivosTemporarios(caminhoTemporarioRaiz);
                    return CommandResult<bool>.Invalid("O arquivo importado não possui a quantidade de colunas esperadas pelo sistema");
                }

                if (FileHandler.ExceedFileMaxSize(tamanhoArquivos, parametroTamanhoArquivo).Dados) {
                    string message = $"O arquivo CSV importado ultrapassa o limite permitido de {parametroTamanhoArquivo}MB.";
                    Logger.LogError("Deletendo arquivos temporários");

                    DeletaArquivosTemporarios(caminhoTemporarioRaiz);

                    Logger.LogInformation(message);
                    return CommandResult.Invalid(message);
                }

                Logger.LogInformation("Salvando arquivos no NAS");
                FileHandler.SaveFileToNAS(caminhoArquivoTemporarioDocumentos, parametroCaminhoNasCargaDocumentos);

                Logger.LogError("Deletendo arquivos temporários");
                DeletaArquivosTemporarios(caminhoTemporarioRaiz);

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Atualizando entidade com arquivos salvos"));
                agendamentoCargaDocumento.AtualizarNomesDocumento(nomeArquivoDocumentos);

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(operacao));
                return CommandResult.Valid();
            }
            catch (Exception ex) {
                Logger.LogError("Deletendo arquivos temporários");
                DeletaArquivosTemporarios(caminhoTemporarioRaiz);

                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(operacao));
                return CommandResult.Invalid(ex.Message);
            }
        }

        private CommandResult RemoveArquivoNAS(AgendamentoCargaDocumento agendamentoCargaDocumento) {
            string operacao = "Excluir arquivos do NAS";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(operacao));
            try {
                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Parametros Jurídicos"));
                var parametroCaminhosNasCargaDocumentos = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_CARGA_DOCUMENTOS);
                bool deleteArquivosOrigem = ParametroJuridico.Obter(ParametrosJuridicos.DEL_DOC_CREDORES_ORIGEM).Dados.Conteudo == "S";
                var caminhoNasOrigemDocumentos = ParametroJuridico.Obter(ParametrosJuridicos.SERV_CARGA_DOCUMENTOS).Dados.Conteudo;

                var caminhoArquivoBaseFGV = string.Empty;

                if (parametroCaminhosNasCargaDocumentos.Count <= 0) {
                    Logger.LogInformation("Diretório Nas não encontrado");
                    return CommandResult<string>.Invalid("Diretório NAS não encontrado");
                }

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Arquivos para exclusão"));
                foreach (var caminho in parametroCaminhosNasCargaDocumentos) {
                    if (!Directory.Exists(caminho))
                        continue;

                    if (File.Exists(Path.Combine(caminho, agendamentoCargaDocumento.NomeArquivoBaseFGV))) {
                        caminhoArquivoBaseFGV = Path.Combine(caminho, agendamentoCargaDocumento.NomeArquivoBaseFGV);
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(caminhoArquivoBaseFGV)) {
                    if (deleteArquivosOrigem) {
                        Logger.LogInformation(Infra.Extensions.Logs.Obtendo($"Deletando o documentos de origem"));
                        using (var fs = File.OpenRead(caminhoArquivoBaseFGV)) {
                            using (var reader = new StreamReader(fs)) {
                                // ignora o header;
                                reader.ReadLine();

                                string line;
                                while ((line = reader.ReadLine()) != null) {
                                    var cells = line.Replace("\"", "").Split(";");

                                    Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Informações do arquivo DExPARA."));

                                    string codigoInternoProcesso = string.IsNullOrEmpty(cells[0]) ? string.Empty : cells[0];
                                    string nomeArquivoOrigem = string.IsNullOrEmpty(cells[2]) ? string.Empty : Path.Combine(caminhoNasOrigemDocumentos, cells[2]);

                                    if (string.IsNullOrEmpty(nomeArquivoOrigem)) {
                                        continue;
                                    }

                                    FileHandler.DeleteFile(nomeArquivoOrigem);
                                }
                                fs.Dispose();
                            }
                        }
                    }
                    Logger.LogInformation("Apagando arquivos");
                    FileHandler.DeleteFile(caminhoArquivoBaseFGV);
                }


                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(operacao));
                return CommandResult.Valid();
            }
            catch (Exception ex) {
                Logger.LogError(Infra.Extensions.Logs.OperacaoComErro(operacao));
                return CommandResult.Invalid(ex.Message);
            }
        }

        /// <summary>
        /// Rotina que remove o arquivo tanto do BD quando localmente salvo.
        /// </summary>
        /// <param name="agendamentoCargaDocumentoId">Id do registro a ser excluído</param>
        /// <returns></returns>
        public CommandResult Remover(int agendamentoCargaDocumentoId) {
            string entityName = "Agendamento de Carga Documentos";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_CARGA_DOCUMENTOS)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_CARGA_DOCUMENTOS, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, agendamentoCargaDocumentoId));
                AgendamentoCargaDocumento agendamentoCargaDocumento = DatabaseContext.AgendamentosCargasDocumentos
                                                                    .FirstOrDefault(x => x.Id == agendamentoCargaDocumentoId);


                if (agendamentoCargaDocumento is null) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, agendamentoCargaDocumentoId));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, agendamentoCargaDocumentoId));
                }


                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Validar remoção {entityName}"));

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Parâmetro Jurídico"));
                int paramDias = Convert.ToInt32(ParametroJuridico.Obter(ParametrosJuridicos.DIAS_EXPURGO_CARGA_DOC).Dados.Conteudo);
                DateTime dataExpurgo = DateTime.Today.AddDays(-paramDias);

                if ((agendamentoCargaDocumento.StatusAgendamentoId != StatusAgendamento.AGENDADO.Id
                     && agendamentoCargaDocumento.StatusAgendamentoId != StatusAgendamento.ERRO.Id)
                                        && agendamentoCargaDocumento.DataAgendamento > dataExpurgo) {
                    string message = $"Somente agendamentos com status 'Agendado' e 'Erro' poderão ser excluídos.";
                    Logger.LogInformation(Infra.Extensions.Logs.ValidandoRegras());
                    return CommandResult.Invalid(message);
                }
                else {
                    Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, agendamentoCargaDocumentoId));
                    DatabaseContext.Remove(agendamentoCargaDocumento);
                }

                RemoveArquivoNAS(agendamentoCargaDocumento);

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));
                DatabaseContext.SaveChanges();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                return CommandResult.Valid();
            }
            catch (Exception ex) {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        public CommandResult Criar(IFormFile documentos) {
            string entityName = "Carga de Documentos";
            string commandName = $"Criar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));
            try {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_CARGA_DOCUMENTOS)) {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_CARGA_DOCUMENTOS, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));
                AgendamentoCargaDocumento agendamentoDocumento = AgendamentoCargaDocumento.Criar(UsuarioAtual.Login);

                if (agendamentoDocumento.HasNotifications) {
                    return CommandResult.Invalid(agendamentoDocumento.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(agendamentoDocumento);

                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao("Salva Arquivos no diretório NAS"));
                var salvaArquivoNAS = SalvaArquivosNAS(documentos, agendamentoDocumento);

                if (!salvaArquivoNAS.IsValid) {
                    Logger.LogInformation("Erros ao salvar no NAS: ", salvaArquivoNAS.Mensagens);
                    return CommandResult.Invalid(salvaArquivoNAS.Mensagens);
                }

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));
                DatabaseContext.SaveChanges();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));

                return CommandResult.Valid();
            }
            catch (Exception ex) {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        public CommandResult RemoverDocumentosGerados(int agendamentoId) {
            string entity = "Agendamento Carga Documentos";
            string command = $"Obter Resultado Carga {entity}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(command));

            try {
                Logger.LogInformation(Infra.Extensions.Logs.Obtendo(entity));
                var dadoArquivo = DatabaseContext.AgendamentosCargasDocumentos
                    .FirstOrDefault(a => a.Id == agendamentoId);

                if (dadoArquivo is null) {
                    string mensagem = $"Não foi possivel encontrar agendamentos de {agendamentoId} para expurgo dos documentos.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                string caminhoArquivoBaseFGV = string.Empty;
                var caminhoArquivoGerado = string.Empty;

                var parametroCaminhosNasCargaDocumentos = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_CARGA_DOCUMENTOS);
                var parametroCaminhosNasResultadoCargaDocumentos = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_CARGA_DOC_RESULT);

                if (parametroCaminhosNasCargaDocumentos.Count <= 0) {
                    Logger.LogInformation("Diretório Nas não encontrado");
                    return CommandResult.Invalid("Diretório NAS não encontrado");
                }

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Arquivos de Carga de Documentos"));
                foreach (var caminho in parametroCaminhosNasCargaDocumentos) {
                    if (!Directory.Exists(caminho)) {
                        continue;
                    }

                    if (File.Exists(Path.Combine(caminho, dadoArquivo.NomeArquivoBaseFGV))) {
                        caminhoArquivoBaseFGV = Path.Combine(caminho, dadoArquivo.NomeArquivoBaseFGV);
                        Logger.LogInformation(Infra.Extensions.Logs.Obtendo($"Deletando o arquivo {dadoArquivo.NomeArquivoBaseFGV}"));
                        FileHandler.DeleteFile(caminhoArquivoBaseFGV);
                        Logger.LogInformation($"Arquivo {caminhoArquivoBaseFGV} excluído.");
                    }
                }

                foreach (var caminho in parametroCaminhosNasResultadoCargaDocumentos) {
                    if (!Directory.Exists(caminho)) {
                        continue;
                    }

                    if (File.Exists(Path.Combine(caminho, dadoArquivo.NomeArquivoGerado))) {
                        caminhoArquivoGerado = Path.Combine(caminho, dadoArquivo.NomeArquivoGerado);
                        Logger.LogInformation(Infra.Extensions.Logs.Obtendo($"Deletando o arquivo {dadoArquivo.NomeArquivoGerado}"));
                        FileHandler.DeleteFile(caminhoArquivoGerado);
                        Logger.LogInformation($"Arquivo {caminhoArquivoGerado} excluído.");
                    }
                }

                return CommandResult.Valid();
            }
            catch (Exception ex) {
                string mensagem = $"Não foi possivel encontrar o agendamento de id {agendamentoId} para expurgo -  {ex.Message} ";
                Logger.LogInformation(mensagem);
                return CommandResult.Invalid(mensagem);
            }
        }
    }
}
