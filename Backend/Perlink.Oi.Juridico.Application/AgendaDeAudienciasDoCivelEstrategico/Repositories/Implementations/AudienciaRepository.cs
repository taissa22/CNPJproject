using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Filters;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.External;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Repositories.Implementations
{
    internal class AudienciaRepository : IAudienciaRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IAudienciaRepository> Logger { get; }
        public IUsuarioAtualProvider UsuarioAtual { get; }
        public const string logName = "Audiencia Processo";

        public AudienciaRepository(IDatabaseContext databaseContext, IUsuarioAtualProvider user, ILogger<IAudienciaRepository> logger)
        {
            DatabaseContext = databaseContext;
            UsuarioAtual = user;
            Logger = logger;
        }

        public CommandResult<IReadOnlyCollection<AudienciaDoProcesso>> ObterAudienciasPorProcesso(AgendaDeAudienciaDoCivelEstrategicoFilter filtros)
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_AGENDA_CE))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_AGENDA_CE, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<AudienciaDoProcesso>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            IQueryable<AudienciaDoProcesso> consulta = ObterBase(filtros);
            IOrderedQueryable<AudienciaDoProcesso> consultaOrdenarda = ObterOrdenacao(consulta, filtros);

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<AudienciaDoProcesso>>.Valid(consultaOrdenarda.ToArray());
        }

        public CommandResult<PaginatedQueryResult<AudienciaDoProcesso>> ObterAudienciasPorProcessoPaginado(int processoId, int pagina, int quantidade)
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_AGENDA_CE))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_AGENDA_CE, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<AudienciaDoProcesso>>.Forbidden();
            }

            IQueryable<AudienciaDoProcesso> query = DatabaseContext.AudienciasDosProcessos
                               .Where(x => x.Processo.Id == processoId);

            var total = query.Count();
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            PaginatedQueryResult<AudienciaDoProcesso> resultado = null;

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            if (total > quantidade)
            {
                resultado = new PaginatedQueryResult<AudienciaDoProcesso>()
                {
                    Total = total,
                    Data = query.Skip(skip)
                                .Take(quantidade)
                                .OrderByDescending(p => p.DataAudiencia)
                                .AsNoTracking()
                                .ToArray()
                };
            }
            else
            {
                resultado = new PaginatedQueryResult<AudienciaDoProcesso>()
                {
                    Total = total,
                    Data = query.OrderByDescending(p => p.DataAudiencia)
                                .AsNoTracking()
                                .ToArray()
                };
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<AudienciaDoProcesso>>.Valid(resultado);
        }

        public CommandResult<PaginatedQueryResult<AudienciaDoProcesso>> ObterAudienciasPorUsuarioLogado(AgendaDeAudienciaDoCivelEstrategicoFilter filtros)
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_AGENDA_CE))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_AGENDA_CE, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<AudienciaDoProcesso>>.Forbidden();
            }

            IQueryable<AudienciaDoProcesso> consulta = ObterBase(filtros);
            IOrderedQueryable<AudienciaDoProcesso> consultaOrdenarda = ObterOrdenacao(consulta, filtros);

            var total = consultaOrdenarda.Count();
            var skip = Pagination.PagesToSkip(filtros.Quantidade, total, filtros.Pagina);

            var resultado = new PaginatedQueryResult<AudienciaDoProcesso>()
            {
                Total = total,
                Data = consultaOrdenarda.Skip(skip).Take(filtros.Quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<AudienciaDoProcesso>>.Valid(resultado);
        }

        #region Métodos auxiliares

        private IQueryable<AudienciaDoProcesso> ObterBase(AgendaDeAudienciaDoCivelEstrategicoFilter filtros)
        {
            IQueryable<AudienciaDoProcesso> query = DatabaseContext.AudienciasDosProcessos
                                                           .Where(x => x.Processo.TipoProcessoId == TipoProcesso.CIVEL_ESTRATEGICO.Id).AsNoTracking();

            if (filtros.DataInicial != null && filtros.DataInicial != DateTime.MinValue && filtros.DataFinal != null && filtros.DataFinal != DateTime.MinValue)
            {
                var dataInicial = filtros.DataInicial.Date;
                var dataFinal = filtros.DataFinal.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                query = query.Where(x => x.DataAudiencia >= dataInicial && x.DataAudiencia <= dataFinal);
            }

            if (filtros.EscritorioId > 0)
            {
                query = query.Where(x => filtros.EscritorioId == x.Escritorio.Id);
            }
            else
            {
                IReadOnlyCollection<int> escritoriosIds = ObterEscritoriosDoUsuarioLogado().Select(x => x.Id).ToArray();
                if (escritoriosIds.Count > 0)
                    query = query.Where(x => escritoriosIds.Contains(x.Escritorio.Id) /*|| escritoriosIds.Contains((int)x.Processo.EscritorioAcompanhanteId)*/);
            }

            return query.WhereIf(x => x.Processo.Vara.Comarca.Estado.Id == filtros.EstadoId, !string.IsNullOrEmpty(filtros.EstadoId))
                        .WhereIf(x => x.Processo.Comarca.Id == filtros.ComarcaId, filtros.ComarcaId > 0)
                        .WhereIf(x => x.Processo.EmpresaDoGrupo.Id == filtros.EmpresaGrupoId, filtros.EmpresaGrupoId > 0)
                        .WhereIf(x => x.Preposto.Id == filtros.PrepostoId, filtros.PrepostoId > 0)
                        .WhereIf(x => x.ProcessoId == filtros.ProcessoId, filtros.ProcessoId > 0)
                        .WhereIf(x => x.Processo.Assunto.Id == filtros.AssuntoId, filtros.AssuntoId > 0)
                        .WhereIf(x => Convert.ToInt32(x.Processo.Closing) == Convert.ToInt32(filtros.Closing), !string.IsNullOrEmpty(filtros.Closing))
                        .WhereIf(x => x.Processo.ClassificacaoProcessoId == filtros.ClassificacaoProcessoId, !string.IsNullOrEmpty(filtros.ClassificacaoProcessoId))
                        .WhereIf(x => Convert.ToInt32(x.Processo.ClosingClientCo) == Convert.ToInt32(filtros.ClientCo), !string.IsNullOrEmpty(filtros.ClientCo));
        }

        private IQueryable<Escritorio> ObterEscritoriosDoUsuarioLogado()
        {
            return DatabaseContext.EscritoriosDosUsuarios
                          .Where(x => x.UsuarioId == UsuarioAtual.Login)
                          .Select(x => x.Escritorio);
        }

        private IOrderedQueryable<AudienciaDoProcesso> ObterOrdenacao(IQueryable<AudienciaDoProcesso> consulta, AgendaDeAudienciaDoCivelEstrategicoFilter filtros)
        {
            var listaParaOrdenacao = new List<Tuple<int, string, string>>() {
                new Tuple<int, string, string>(filtros.Estado.Ordem, "estado", filtros.Estado.Direction),
                new Tuple<int, string, string>(filtros.Comarca.Ordem, "comarca", filtros.Comarca.Direction),
                new Tuple<int, string, string>(filtros.DataAudiencia.Ordem, "dataAudiencia", filtros.DataAudiencia.Direction),
                new Tuple<int, string, string>(filtros.HoraAudiencia.Ordem, "horaAudiencia", filtros.HoraAudiencia.Direction),
                new Tuple<int, string, string>(filtros.Vara.Ordem, "vara", filtros.Vara.Direction),
                new Tuple<int, string, string>(filtros.TipoVara.Ordem, "tipoVara", filtros.TipoVara.Direction)
            }.OrderBy(p => p.Item1);

            IOrderedQueryable<AudienciaDoProcesso> query = consulta as IOrderedQueryable<AudienciaDoProcesso>;

            foreach (Tuple<int, string, string> item in listaParaOrdenacao)
            {
                var ordem = item.Item1;
                var coluna = item.Item2;
                var asc = item.Item3;

                switch (coluna)
                {
                    case "estado":
                        if (ordem == 0)
                        {
                            query = asc == "asc" ? query.OrderBy(x => x.Processo.Comarca.Estado.Id)
                                : query.OrderByDescending(x => x.Processo.Comarca.Estado.Id);
                            continue;
                        }
                        query = asc == "asc" ? query.ThenBy(x => x.Processo.Comarca.Estado.Id)
                            : query.ThenByDescending(x => x.Processo.Comarca.Estado.Id);
                        break;

                    case "comarca":
                        if (ordem == 0)
                        {
                            query = asc == "asc" ? query.OrderBy(x => x.Processo.Comarca.Nome)
                                : query.OrderByDescending(x => x.Processo.Comarca.Nome);
                            continue;
                        }
                        query = asc == "asc" ? query.ThenBy(x => x.Processo.Comarca.Nome)
                            : query.ThenByDescending(x => x.Processo.Comarca.Nome);
                        break;

                    case "dataAudiencia":
                        if (ordem == 0)
                        {
                            query = asc == "asc" ? query.OrderBy(x => x.DataAudiencia)
                                : query.OrderByDescending(x => x.DataAudiencia);
                            continue;
                        }
                        query = asc == "asc" ? query.ThenBy(x => x.DataAudiencia)
                            : query.ThenByDescending(x => x.DataAudiencia);
                        break;

                    case "horaAudiencia":
                        if (ordem == 0)
                        {
                            query = asc == "asc" ? query.OrderBy(x => x.HoraAudiencia)
                                : query.OrderByDescending(x => x.HoraAudiencia);
                            continue;
                        }
                        query = asc == "asc" ? query.ThenBy(x => x.HoraAudiencia)
                            : query.ThenByDescending(x => x.HoraAudiencia);
                        break;

                    case "vara":
                        if (ordem == 0)
                        {
                            query = asc == "asc" ? query.OrderBy(x => x.Processo.Vara.VaraId)
                                : query.OrderByDescending(x => x.Processo.Vara.VaraId);
                            continue;
                        }
                        query = asc == "asc" ? query.ThenBy(x => x.Processo.Vara.VaraId)
                            : query.ThenByDescending(x => x.Processo.Vara.VaraId);
                        break;

                    case "tipoVara":
                        if (ordem == 0)
                        {
                            query = asc == "asc" ? query.OrderBy(x => x.Processo.TipoVara.Nome)
                                : query.OrderByDescending(x => x.Processo.TipoVara.Nome);
                            continue;
                        }
                        query = asc == "asc" ? query.ThenBy(x => x.Processo.TipoVara.Nome)
                            : query.ThenByDescending(x => x.Processo.TipoVara.Nome);
                        break;

                    default:
                        if (ordem == 0)
                        {
                            query = asc == "asc" ? query.OrderBy(x => x.Sequencial)
                                : query.OrderByDescending(x => x.Sequencial);
                            continue;
                        }
                        query = asc == "asc" ? query.ThenBy(x => x.Sequencial)
                            : query.ThenByDescending(x => x.Sequencial);
                        break;
                }
            }

            return query;
        }

        #endregion Métodos auxiliares
    }
}