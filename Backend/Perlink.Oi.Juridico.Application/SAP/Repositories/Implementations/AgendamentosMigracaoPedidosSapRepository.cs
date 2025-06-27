using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.SAP.Enuns;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.IO;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.SAP.Repositories.Implementations
{
    internal class AgendamentosMigracaoPedidosSapRepository : IAgendamentosMigracaoPedidosSapRepository
    {
        private IDatabaseContext DatabaseContext { get; }

        private ILogger<IAgendamentosMigracaoPedidosSapRepository> Logger { get; }

        private IUsuarioAtualProvider UsuarioAtual { get; }

        private IParametroJuridicoProvider ParametroJuridico { get; }

        public AgendamentosMigracaoPedidosSapRepository(IDatabaseContext databaseContext, ILogger<IAgendamentosMigracaoPedidosSapRepository> logger,
            IUsuarioAtualProvider usuarioAtual, IParametroJuridicoProvider parametroJuridico)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
            ParametroJuridico = parametroJuridico;
        }

        public CommandResult<PaginatedQueryResult<AgendamentoMigracaoPedidosSap>> ObterPaginado(int pagina)
        {
            string logName = "Agendamento de Migração de Pedidos SAP";

            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_MIGRACAO_PEDIDOS_SAP))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_MIGRACAO_PEDIDOS_SAP, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<AgendamentoMigracaoPedidosSap>>.Forbidden();
            }

            IQueryable<AgendamentoMigracaoPedidosSap> query = DatabaseContext.AgendamentosMigracaoPedidosSap
                                                                           .OrderByDescending(x => x.DataAgendamento)
                                                                           .AsNoTracking();
            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = query.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            int skip = Pagination.PagesToSkip(5, total, pagina);

            var resultado = new PaginatedQueryResult<AgendamentoMigracaoPedidosSap>()
            {
                Total = total,
                Data = query.Skip(skip).Take(5).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando("a lista das Migrações de Pedidos SAP"));
            return CommandResult<PaginatedQueryResult<AgendamentoMigracaoPedidosSap>>.Valid(resultado);
        }

        public CommandResult<string> ObterArquivos(TipoArquivo tipoArquivo, int? agendamentoId = null)
        {
            string entity = "Agendamento Migração de Pedidos SAP";
            string command = $"Obter Arquivos {entity}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(command));

            switch (tipoArquivo)
            {
                case TipoArquivo.ArquivosCarregados:
                    return ObterArquivosCarregados((int)agendamentoId);

                case TipoArquivo.ResultadoMigracao:
                    return ObterResultadoMigracao((int)agendamentoId);

                case TipoArquivo.ArquivosPadrao:
                    return DownloadPlanilhaPadrao(); 

                case TipoArquivo.NaoInformado:
                default:
                    return CommandResult<string>.Invalid("Tipo de arquivo não informado.");
            }
        }

        private CommandResult<string> DownloadPlanilhaPadrao()
        {
            string nomeArquivo = "modelo_migracao_pedidos.csv";

            var parametroCaminhosNasMigracaoPedidosSap = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_MIGRACAO_PED_SAP);

            if (parametroCaminhosNasMigracaoPedidosSap.Count <= 0)
            {
                Logger.LogInformation("Diretório Nas não encontrado");
                return CommandResult<string>.Invalid("Diretório NAS não encontrado");
            }

            var caminhoPlanilhaPadrao = string.Empty;
            foreach (var caminho in parametroCaminhosNasMigracaoPedidosSap)
            {
                if (!Directory.Exists(caminho))
                {
                    continue;
                }

                if (File.Exists(Path.Combine(caminho, nomeArquivo)))
                {
                    caminhoPlanilhaPadrao = Path.Combine(caminho, nomeArquivo);
                    break;
                }
            }

            if (string.IsNullOrEmpty(caminhoPlanilhaPadrao))
            {
                return CommandResult<string>.Invalid($"Arquivo não encontrado no NAS: {caminhoPlanilhaPadrao}");
            }
            return CommandResult<string>.Valid(caminhoPlanilhaPadrao);
        }

        private CommandResult<string> ObterArquivosCarregados(int agendamentoId)
        {
            string entity = "Agendamento Migração Pedidos SAP";
            string command = $"Obter Arquivos Carregados {entity}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(command));

            try
            {
                Logger.LogInformation(Infra.Extensions.Logs.Obtendo(entity));
                var dadoArquivo = DatabaseContext.AgendamentosMigracaoPedidosSap.FirstOrDefault(a => a.Id == agendamentoId);

                if (dadoArquivo is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entity, agendamentoId));
                    return CommandResult<string>.Invalid("Agendamento não encontrado");
                }

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Parâmetro Jurídico"));
                var parametroCaminhosNasMigracaoPedidosSap = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_MIGRACAO_PED_SAP);

                if (parametroCaminhosNasMigracaoPedidosSap.Count <= 0)
                {
                    Logger.LogInformation("Diretório Nas não encontrado");
                    return CommandResult<string>.Invalid("Diretório NAS não encontrado");
                }

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Arquivo de Documentos"));
                var caminhoArquivoDeParaMigracao = string.Empty;
                foreach (var caminho in parametroCaminhosNasMigracaoPedidosSap)
                {
                    if (!Directory.Exists(caminho))
                    {
                        continue;
                    }

                    if (File.Exists(Path.Combine(caminho, dadoArquivo.NomeArquivoDeParaMigracao)))
                    {
                        caminhoArquivoDeParaMigracao = Path.Combine(caminho, dadoArquivo.NomeArquivoDeParaMigracao);
                        break;
                    }
                }

                Logger.LogInformation("Retornando Arquivo");
                return CommandResult<string>.Valid(caminhoArquivoDeParaMigracao);
            }
            catch (Exception ex)
            {
                Logger.LogError(Infra.Extensions.Logs.OperacaoComErro(command));
                return CommandResult<string>.Invalid(ex.Message);
            }
        }

        private CommandResult<string> ObterResultadoMigracao(int agendamentoId)
        {
            string entity = "Agendamento Migração Pedidos SAP";
            string command = $"Obter Resultado Migração Pedidos SAP {entity}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(command));

            try
            {
                Logger.LogInformation(Infra.Extensions.Logs.Obtendo(entity));
                var dadoArquivo = DatabaseContext.AgendamentosMigracaoPedidosSap.FirstOrDefault(a => a.Id == agendamentoId);

                if (dadoArquivo is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entity, agendamentoId));
                    return CommandResult<string>.Invalid("Agendamento não encontrado");
                }

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Parâmetro Jurídico"));
                var parametroCaminhosMigracaopedidosSap = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_MIGRACAO_PED_SAP);

                if (parametroCaminhosMigracaopedidosSap.Count <= 0)
                {
                    Logger.LogInformation("Diretório Nas não encontrado");
                    return CommandResult<string>.Invalid("Diretório NAS não encontrado");
                }

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Arquivo de Documentos"));
                var caminhoArquivoGerado = string.Empty;
                foreach (var caminho in parametroCaminhosMigracaopedidosSap)
                {
                    if (!Directory.Exists(caminho))
                        continue;

                    if (!string.IsNullOrEmpty(dadoArquivo.NomeArquivoGerado))
                    {
                        if (File.Exists(Path.Combine(caminho, dadoArquivo.NomeArquivoGerado)))
                        {
                            caminhoArquivoGerado = Path.Combine(caminho, dadoArquivo.NomeArquivoGerado);
                            break;
                        }
                    }
                }
                return CommandResult<string>.Valid(caminhoArquivoGerado);
            }
            catch (Exception ex)
            {
                Logger.LogError(Infra.Extensions.Logs.OperacaoComErro(command));
                return CommandResult<string>.Invalid(ex.Message);
            }
        }
    }
}