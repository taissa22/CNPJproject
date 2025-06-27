using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Application.Manutencao.ViewModel;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.External;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    public class ObjetoRepository : IObjetoRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IAssuntoRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public ObjetoRepository(IDatabaseContext databaseContext, ILogger<IAssuntoRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<Pedido> ObterBase(int tipoProcessoId, ObjetoSort sort, bool ascending, string descricao)
        {
            string logName = "Objeto";
            IQueryable<Pedido> query = DatabaseContext.Pedidos.AsNoTracking();

            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            switch (sort)
            {
                case ObjetoSort.Id:
                    query = query.SortBy(a => a.Id, ascending);
                    break;

                case ObjetoSort.Descricao:
                    query = query.SortBy(a => a.Descricao, ascending).ThenSortBy(a => a.Descricao, ascending);
                    break;

                case ObjetoSort.AtivoTributarioAdminstrativo:
                    query = query.SortBy(a => a.AtivoTributarioAdministrativo, ascending).ThenSortBy(a => a.AtivoTributarioAdministrativo, ascending);
                    break;

                case ObjetoSort.AtivoTributarioJudicial:
                    query = query.SortBy(a => a.AtivoTributarioJudicial, ascending).ThenSortBy(a => a.AtivoTributarioJudicial, ascending);
                    break;

                case ObjetoSort.TrabalhistaAdministrativo:
                    query = query.SortBy(a => a.EhTrabalhistaAdministrativo, ascending).ThenSortBy(a => a.EhTrabalhistaAdministrativo, ascending);
                    break;

                case ObjetoSort.TributarioAdministrativo:
                    query = query.SortBy(a => a.EhTributarioAdministrativo, ascending).ThenSortBy(a => a.EhTributarioAdministrativo, ascending);
                    break;

                case ObjetoSort.TributarioJudicial:
                    query = query.SortBy(a => a.EhTributarioJudicial, ascending).ThenSortBy(a => a.EhTributarioJudicial, ascending);
                    break;

                case ObjetoSort.Grupo:
                    query = query.SortBy(a => a.GrupoPedido, ascending).ThenSortBy(a => a.GrupoPedido, ascending);
                    break;

                default:
                    query = query.SortBy(a => a.Descricao, ascending).ThenSortBy(a => a.Descricao, ascending);
                    break;
            }

            bool temPermissao = false;

            switch (tipoProcessoId)
            {
                case
                2: //TRABALHISTA
                    temPermissao = UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_OBJETO_TRABALHISTA_ADMINISTRATIVO);
                    break;

                case
                4: //TRIBUTARIO
                    temPermissao = UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_OBJETO_TRIBUTARIO);
                    break;

                default:
                    break;
            }

            // TODO DOUGLAS: retirar a sobreescrita de permissões
            temPermissao = true;

            query = query.Where(x => temPermissao && x.TipoProcesso.Id == tipoProcessoId);

            return query.WhereIfNotNull(x => x.Descricao.ToUpper().Contains(descricao.ToString().ToUpper()), descricao);
        }

        public CommandResult<PaginatedQueryResult<ObjetoViewModel>> ObterPaginado(int tipoProcesso, ObjetoSort sort, bool ascending, int pagina, int quantidade, string descricao)
        {
            string logName = "Pedido do Trabalhista Adminstrativo";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (tipoProcesso == TipoProcesso.TRABALHISTA_ADMINISTRATIVO.Id && !UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_OBJETO_TRABALHISTA_ADMINISTRATIVO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_OBJETO_TRABALHISTA_ADMINISTRATIVO, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<ObjetoViewModel>>.Forbidden();
            }

            if ((tipoProcesso == TipoProcesso.TRIBUTARIO_ADMINISTRATIVO.Id || tipoProcesso == TipoProcesso.TRIBUTARIO_JUDICIAL.Id) && !UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_OBJETO_TRIBUTARIO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_OBJETO_TRIBUTARIO, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<ObjetoViewModel>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(tipoProcesso, sort, ascending, descricao);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<ObjetoViewModel>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade)
                                .Select(x => new ObjetoViewModel
                                {
                                    Id = x.Id,
                                    Descricao = x.Descricao,
                                    AtivoTributarioAdministrativo = x.AtivoTributarioAdministrativo,
                                    AtivoTributarioJudicial = x.AtivoTributarioJudicial,
                                    EhTrabalhistaAdministrativo = x.EhTrabalhistaAdministrativo,
                                    EhTributarioAdministrativo = x.EhTributarioAdministrativo,
                                    EhTributarioJudicial = x.EhTributarioJudicial,
                                    TipoProcesso = x.TipoProcesso,
                                    GrupoPedidoId = x.GrupoPedidoId,
                                    GrupoPedidoDescricao = DatabaseContext.GruposPedidos.FirstOrDefault(g => g.Id == x.GrupoPedidoId.GetValueOrDefault()).Descricao
                                }).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<ObjetoViewModel>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<ObjetoViewModel>> Obter(int tipoProcesso, ObjetoSort sort, bool ascending, string descricao)
        {
            string logName = "Pedido do Trabalhista Adminstrativo";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (tipoProcesso == TipoProcesso.TRABALHISTA_ADMINISTRATIVO.Id && !UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_OBJETO_TRABALHISTA_ADMINISTRATIVO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_OBJETO_TRABALHISTA_ADMINISTRATIVO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<ObjetoViewModel>>.Forbidden();
            }

            if ((tipoProcesso == TipoProcesso.TRIBUTARIO_ADMINISTRATIVO.Id || tipoProcesso == TipoProcesso.TRIBUTARIO_JUDICIAL.Id) && !UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_OBJETO_TRIBUTARIO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_OBJETO_TRIBUTARIO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<ObjetoViewModel>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(tipoProcesso, sort, ascending, descricao)
                                    .Select(x => new ObjetoViewModel
                                    {
                                        Id = x.Id,
                                        Descricao = x.Descricao,
                                        AtivoTributarioAdministrativo = x.AtivoTributarioAdministrativo,
                                        AtivoTributarioJudicial = x.AtivoTributarioJudicial,
                                        EhTrabalhistaAdministrativo = x.EhTrabalhistaAdministrativo,
                                        EhTributarioAdministrativo = x.EhTributarioAdministrativo,
                                        EhTributarioJudicial = x.EhTributarioJudicial,
                                        TipoProcesso = x.TipoProcesso,
                                        GrupoPedidoId = x.GrupoPedidoId,
                                        GrupoPedidoDescricao = DatabaseContext.GruposPedidos.FirstOrDefault(g => g.Id == x.GrupoPedidoId.GetValueOrDefault()).Descricao
                                    }
                                    ).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<ObjetoViewModel>>.Valid(resultado);
        }
    }
}