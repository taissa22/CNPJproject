using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
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

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations {

    public class OrgaoRepository : IOrgaoRepository {
        private IDatabaseContext DatabaseContext { get; }

        private ILogger<IOrgaoRepository> Logger { get; }

        private IUsuarioAtualProvider UsuarioAtual { get; }

        public OrgaoRepository(IDatabaseContext databaseContext, ILogger<IOrgaoRepository> logger, IUsuarioAtualProvider usuarioAtual) {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<Orgao> ObterBase(TipoOrgao tipoOrgao, OrgaoSort sort, bool ascending) {
            IQueryable<Orgao> query = DatabaseContext.Orgaos
                                         .Where(x => EF.Property<string>(x, "TipoParteValor") == tipoOrgao.Valor)
                                         .AsNoTracking();

            switch (sort) {
                case OrgaoSort.Telefone:
                    if (ascending) {
                        query = query.OrderBy(x => x.TelefoneDDD == null ? 0 : 1).ThenBy(x => x.TelefoneDDD);
                    } else {
                        query = query.OrderByDescending(x => x.TelefoneDDD == null ? 0 : 1).ThenByDescending(x => x.TelefoneDDD);
                    }
                    break;

                case OrgaoSort.Nome:
                default:
                    query = query.SortBy(x => x.Nome, ascending).ThenSortBy(x => x.Nome, ascending);
                    break;
            }

            return query;
        }

        public CommandResult<IReadOnlyCollection<Orgao>> Obter(TipoOrgao tipoOrgao, OrgaoSort sort, bool ascending) {

            if (!UsuarioAtual.TemPermissaoPara(PermissaoPorTipoOrgao(tipoOrgao))) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(PermissaoPorTipoOrgao(tipoOrgao), UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<Orgao>>.Forbidden();
            }

            string logName = "Órgãos";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(tipoOrgao, sort, ascending).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Orgao>>.Valid(resultado);
        }

        public CommandResult<PaginatedQueryResult<Orgao>> ObterPaginado(TipoOrgao tipoOrgao, OrgaoSort sort, bool ascending, int pagina, int quantidade) {
            string logName = "Órgãos";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(PermissaoPorTipoOrgao(tipoOrgao))) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(PermissaoPorTipoOrgao(tipoOrgao), UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<Orgao>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(tipoOrgao, sort, ascending);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);


            var resultado = new PaginatedQueryResult<Orgao>() {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<Orgao>>.Valid(resultado);
        }

        private string PermissaoPorTipoOrgao(TipoOrgao tipoOrgao) {
            if (tipoOrgao == TipoOrgao.CRIMINAL_ADMINISTRATIVO) {
                return Permissoes.ACESSAR_ORGAOS_CRIMINAL_ADMINISTRATIVO;
            }

            if (tipoOrgao == TipoOrgao.CIVEL_ADMINISTRATIVO) {
                return Permissoes.ACESSAR_ORGAOS_CIVEL_ADMINISTRATIVO;
            }

            if (tipoOrgao == TipoOrgao.DEMAIS_TIPOS) {
                return Permissoes.ACESSAR_ORGAOS_DEMAIS_TIPOS;
            }

            return Permissoes.PERMISSAO_NEGADA;
        }
    }
}