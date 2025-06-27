using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.DocumentoCredor.Enums;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Handlers;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.DocumentoCredor.Repositories.Implementations {

    public class AgendamentoCargaComprovanteRepository : IAgendamentoCargaComprovanteRepository {
        private IDatabaseContext DatabaseContext { get; }

        private ILogger<IAgendamentoCargaComprovanteRepository> Logger { get; }

        private IUsuarioAtualProvider UsuarioAtual { get; }

        private IParametroJuridicoProvider ParametroJuridico { get; }

        private IFileHandler FileHandler { get; }

        public AgendamentoCargaComprovanteRepository(IDatabaseContext databaseContext, ILogger<IAgendamentoCargaComprovanteRepository> logger,
            IUsuarioAtualProvider usuarioAtual, IParametroJuridicoProvider parametroJuridico, IFileHandler fileHandler) {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
            ParametroJuridico = parametroJuridico;
            FileHandler = fileHandler;
        }

        public CommandResult<PaginatedQueryResult<AgendamentoCargaComprovante>> ObterPaginado(int pagina) {
            string logName = "Agendamento de Carga de Pagamentos";

            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_CARGA_COMPROVANTES)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_CARGA_COMPROVANTES, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<AgendamentoCargaComprovante>>.Forbidden();
            }

            IQueryable<AgendamentoCargaComprovante> query = DatabaseContext.AgendamentosCargasComprovantes
                                                                           .OrderByDescending(x => x.DataAgendamento)
                                                                           .AsNoTracking();
            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = query.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            int skip = Pagination.PagesToSkip(5, total, pagina);

            var resultado = new PaginatedQueryResult<AgendamentoCargaComprovante>() {
                Total = total,
                Data = query.Skip(skip).Take(5).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando("a lista das cargas de comprovante"));
            return CommandResult<PaginatedQueryResult<AgendamentoCargaComprovante>>.Valid(resultado);
        }

        public CommandResult<byte[]> ObterArquivos(TipoArquivo tipoArquivo, int? agendamentoId) {
            string entity = "Agendamento Carga Comprovante";
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
                    return CommandResult<byte[]>.Invalid("Tipo de arquivo não informado.");
            }
        }

        private CommandResult<byte[]> DownloadPlanilhaPadrao() {
            string nomeArquivo = "Arquivos Padrão.zip";

            var parametroCaminhosNasCargaComprovantes = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_CARGA_COMPROVANTE);

            if (parametroCaminhosNasCargaComprovantes.Count <= 0) {
                Logger.LogInformation("Diretório Nas não encontrado");
                return CommandResult<byte[]>.Invalid("Diretório NAS não encontrado");
            }

            var caminhoArquivoPadrao = string.Empty;
            foreach (var caminho in parametroCaminhosNasCargaComprovantes) {
                if (!Directory.Exists(caminho)) {
                    continue;
                }                  

                if (File.Exists(Path.Combine(caminho, nomeArquivo))) {
                    caminhoArquivoPadrao = Path.Combine(caminho, nomeArquivo);
                    break;
                }
            }

            if (string.IsNullOrEmpty(caminhoArquivoPadrao)) {
                return CommandResult<byte[]>.Invalid($"Arquivo não encontrado no NAS: {nomeArquivo}");
            }
            return CommandResult<byte[]>.Valid(File.ReadAllBytes(caminhoArquivoPadrao));
        }

        private CommandResult<byte[]> ObterArquivosCarregados(int agendamentoId) {
            string entity = "Agendamento Carga Comprovante";
            string command = $"Obter Arquivos Carregados {entity}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(command));

            try {
                Logger.LogInformation("Verificando Id do Agendamento");
                if (agendamentoId <= 0) {
                    return CommandResult<byte[]>.Invalid("Id do agendamento não informado");
                }                

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo(entity));
                var dadoArquivo = DatabaseContext.AgendamentosCargasComprovantes.FirstOrDefault(a => a.Id == agendamentoId);

                if (dadoArquivo is null) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entity, agendamentoId));
                    return CommandResult<byte[]>.Invalid("Agendamento não encontrado");
                }
                
                List<string> ListaArquivos = new List<string>();
                var caminhoArquivoCarregado = string.Empty;

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Parâmetro Jurídico"));
                var parametroCaminhosNasCargaComprovantes = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_CARGA_COMPROVANTE);

                if (parametroCaminhosNasCargaComprovantes.Count <= 0) {
                    Logger.LogInformation("Diretório Nas não encontrado");
                    return CommandResult<byte[]>.Invalid("Diretório NAS não encontrado");
                }

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Arquivo de Comprovantes"));
                foreach (var caminho in parametroCaminhosNasCargaComprovantes) {
                    if (!Directory.Exists(caminho))
                        continue;

                    if (!string.IsNullOrEmpty(dadoArquivo.NomeArquivoComprovante)) {
                        caminhoArquivoCarregado = Path.Combine(caminho, dadoArquivo.NomeArquivoComprovante);
                        if (File.Exists(caminhoArquivoCarregado)) {
                            ListaArquivos.Add(caminhoArquivoCarregado);
                            break;
                        }
                    }
                }

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Arquivo de Base SAP"));
                foreach (var caminho in parametroCaminhosNasCargaComprovantes) {
                    if (!Directory.Exists(caminho))
                        continue;

                    if (!string.IsNullOrEmpty(dadoArquivo.NomeArquivoBaseSap)) {
                        caminhoArquivoCarregado = Path.Combine(caminho, dadoArquivo.NomeArquivoBaseSap);
                        if (File.Exists(caminhoArquivoCarregado)) {
                            ListaArquivos.Add(caminhoArquivoCarregado);
                            break;
                        }
                    }
                }

                Logger.LogInformation("Retornando Arquivos");
                if (ListaArquivos.Count == 2) {
                    return CommandResult<byte[]>.Valid(FileHandler.GetZippedFiles(ListaArquivos).Dados.ToArray());
                }

                Logger.LogInformation("Arquivo não encontrado.");
                return CommandResult<byte[]>.Invalid("Arquivo não encontrado.");
            } catch (Exception ex) {
                Logger.LogError(Infra.Extensions.Logs.OperacaoComErro(command));
                return CommandResult<byte[]>.Invalid(ex.Message);
            }
        }

        private CommandResult<byte[]> ObterResultadoCarga(int agendamentoId) {
            string entity = "Agendamento Carga Comprovante";
            string command = $"Obter Resultado Carga {entity}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(command));

            try {
                Logger.LogInformation("Verificando Id do Agendamento");
                if (agendamentoId <= 0) {
                    return CommandResult<byte[]>.Invalid("Id do agendamento não informado");
                }                

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo(entity));
                var dadoArquivo = DatabaseContext.AgendamentosCargasComprovantes.FirstOrDefault(a => a.Id == agendamentoId);

                if (dadoArquivo is null) {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entity, agendamentoId));
                    return CommandResult<byte[]>.Invalid("Agendamento não encontrado");
                }
                               
                var caminhoResultadoCarga = string.Empty;

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Parâmetro Jurídico"));
                var parametroCaminhosNasResultadoCarga = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_CARGA_COMP_RESULT);

                if (parametroCaminhosNasResultadoCarga.Count <= 0) {
                    Logger.LogInformation("Diretório Nas não encontrado");
                    return CommandResult<byte[]>.Invalid("Diretório NAS não encontrado");
                }

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Arquivo de Comprovantes"));
                foreach (var caminho in parametroCaminhosNasResultadoCarga) {
                    if (!Directory.Exists(caminho))
                        continue;

                    if (!string.IsNullOrEmpty(dadoArquivo.NomeArquivoGerado)) {
                        
                        if (File.Exists(Path.Combine(caminho, dadoArquivo.NomeArquivoGerado))) {
                            caminhoResultadoCarga = Path.Combine(caminho, dadoArquivo.NomeArquivoGerado);
                            break;
                        }
                    }
                }

                Logger.LogInformation("Retornando Arquivos");
                if (caminhoResultadoCarga != string.Empty) {
                    return CommandResult<byte[]>.Valid(File.ReadAllBytes(caminhoResultadoCarga));
                }

                Logger.LogInformation("Arquivo não encontrado");
                return CommandResult<byte[]>.Invalid("Arquivo não encontrado");
            } catch (Exception ex) {
                Logger.LogError(Infra.Extensions.Logs.OperacaoComErro(command));
                return CommandResult<byte[]>.Invalid(ex.Message);
            }
        }
    }
}