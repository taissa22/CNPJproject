using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.DocumentoCredor.Enums;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.IO;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.DocumentoCredor.Repositories.Implementations {

    internal class AgendamentoCargaDocumentoRepository : IAgendamentoCargaDocumentoRepository {
        private IDatabaseContext DatabaseContext { get; }

        private ILogger<IAgendamentoCargaDocumentoRepository> Logger { get; }

        private IUsuarioAtualProvider UsuarioAtual { get; }

        private IParametroJuridicoProvider ParametroJuridico { get; }

        public AgendamentoCargaDocumentoRepository(IDatabaseContext databaseContext, ILogger<IAgendamentoCargaDocumentoRepository> logger,
            IUsuarioAtualProvider usuarioAtual, IParametroJuridicoProvider parametroJuridico) {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
            ParametroJuridico = parametroJuridico;
        }

        public CommandResult<PaginatedQueryResult<AgendamentoCargaDocumento>> ObterPaginado(int pagina) {
            string logName = "Agendamento de Carga de Documentos";

            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_CARGA_DOCUMENTOS)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_CARGA_DOCUMENTOS, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<AgendamentoCargaDocumento>>.Forbidden();
            }

            IQueryable<AgendamentoCargaDocumento> query = DatabaseContext.AgendamentosCargasDocumentos
                                                                           .OrderByDescending(x => x.DataAgendamento)
                                                                           .AsNoTracking();
            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = query.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            int skip = Pagination.PagesToSkip(5, total, pagina);

            var resultado = new PaginatedQueryResult<AgendamentoCargaDocumento>() {
                Total = total,
                Data = query.Skip(skip).Take(5).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando("a lista das cargas de documentos"));
            return CommandResult<PaginatedQueryResult<AgendamentoCargaDocumento>>.Valid(resultado);
        }

        public CommandResult<string> ObterArquivos(TipoArquivo tipoArquivo, int? agendamentoId) {
            string entity = "Agendamento Carga Documentos";
            string command = $"Obter Arquivos {entity}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(command));

            switch (tipoArquivo) {
                case TipoArquivo.ArquivosCarregados:
                    return ObterArquivosCarregados((int)agendamentoId);

                case TipoArquivo.ResultadoCarga:
                    return ObterResultadoCarga((int)agendamentoId);

                case TipoArquivo.ArquivosPadrao:
                    return DownloadPlanilhaPadrao();

                case TipoArquivo.NaoInformado:
                default:
                    return CommandResult<string>.Invalid("Tipo de arquivo não informado.");
            }
        }

        private CommandResult<string> DownloadPlanilhaPadrao() {
            string nomeArquivo = "Modelo_carga_documentos.csv";

            var parametroCaminhosNasCargaDocumentos = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_CARGA_DOCUMENTOS);


            if (parametroCaminhosNasCargaDocumentos.Count <= 0) {
                Logger.LogInformation("Diretório Nas não encontrado");
                return CommandResult<string>.Invalid("Diretório NAS não encontrado");
            }

            var caminhoPlanilhaPadrao = string.Empty;
            foreach (var caminho in parametroCaminhosNasCargaDocumentos) {
                if (!Directory.Exists(caminho)) {
                    continue;
                }
   
                if (File.Exists(Path.Combine(caminho, nomeArquivo))) {
                    caminhoPlanilhaPadrao = Path.Combine(caminho, nomeArquivo);
                    break;
                }
            }

            if (string.IsNullOrEmpty(caminhoPlanilhaPadrao)) {
                return CommandResult<string>.Invalid($"Arquivo não encontrado no NAS: {caminhoPlanilhaPadrao}");
            }
            return CommandResult<string>.Valid(caminhoPlanilhaPadrao);
        }

        private CommandResult<string> ObterArquivosCarregados(int agendamentoId) {
            string entity = "Agendamento Carga Documentos";
            string command = $"Obter Arquivos Carregados {entity}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(command));

            try {
                Logger.LogInformation(Infra.Extensions.Logs.Obtendo(entity));
                var dadoArquivo = DatabaseContext.AgendamentosCargasDocumentos.FirstOrDefault(a => a.Id == agendamentoId);

                if (dadoArquivo is null) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entity, agendamentoId));
                    return CommandResult<string>.Invalid("Agendamento não encontrado");
                }

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Parâmetro Jurídico"));
                var parametroCaminhosNasCargaDocumentos = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_CARGA_DOCUMENTOS);

                if (parametroCaminhosNasCargaDocumentos.Count <= 0) {
                    Logger.LogInformation("Diretório Nas não encontrado");
                    return CommandResult<string>.Invalid("Diretório NAS não encontrado");
                }

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Arquivo de Documentos"));
                var caminhoArquivoBaseFGV = string.Empty;
                foreach (var caminho in parametroCaminhosNasCargaDocumentos) {
                    if (!Directory.Exists(caminho)) {
                        continue;
                    }                   
                    
                    if (File.Exists(Path.Combine(caminho, dadoArquivo.NomeArquivoBaseFGV))) {
                        caminhoArquivoBaseFGV = Path.Combine(caminho, dadoArquivo.NomeArquivoBaseFGV);
                        break;
                    }
                  
                }

                Logger.LogInformation("Retornando Arquivo");
                return CommandResult<string>.Valid(caminhoArquivoBaseFGV);
            } catch (Exception ex) {
                Logger.LogError(Infra.Extensions.Logs.OperacaoComErro(command));
                return CommandResult<string>.Invalid(ex.Message);
            }
        }

        private CommandResult<string> ObterResultadoCarga(int agendamentoId) {
            string entity = "Agendamento Carga Documentos";
            string command = $"Obter Resultado Carga {entity}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(command));

            try {
                Logger.LogInformation(Infra.Extensions.Logs.Obtendo(entity));
                var dadoArquivo = DatabaseContext.AgendamentosCargasDocumentos.FirstOrDefault(a => a.Id == agendamentoId);

                if (dadoArquivo is null) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entity, agendamentoId));
                    return CommandResult<string>.Invalid("Agendamento não encontrado");
                }

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Parâmetro Jurídico"));
                var parametroCaminhosNasCargaDocumentos = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_CARGA_DOC_RESULT);

                if (parametroCaminhosNasCargaDocumentos.Count <= 0) {
                    Logger.LogInformation("Diretório Nas não encontrado");
                    return CommandResult<string>.Invalid("Diretório NAS não encontrado");
                }

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Arquivo de Documentos"));
                var caminhoArquivoGerado = string.Empty;
                foreach (var caminho in parametroCaminhosNasCargaDocumentos) {
                    if (!Directory.Exists(caminho))
                        continue;

                    if (!string.IsNullOrEmpty(dadoArquivo.NomeArquivoGerado)) {
                        if (File.Exists(Path.Combine(caminho, dadoArquivo.NomeArquivoGerado))) {
                            caminhoArquivoGerado = Path.Combine(caminho, dadoArquivo.NomeArquivoGerado);
                            break;
                        }
                    }
                }
                return CommandResult<string>.Valid(caminhoArquivoGerado);
            } catch (Exception ex) {
                Logger.LogError(Infra.Extensions.Logs.OperacaoComErro(command));
                return CommandResult<string>.Invalid(ex.Message);
            }
        }
    }
}