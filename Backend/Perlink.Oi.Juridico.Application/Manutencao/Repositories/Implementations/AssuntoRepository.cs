using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Results.Assunto;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.External;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations {

    public class AssuntoRepository : IAssuntoRepository {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IAssuntoRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public AssuntoRepository(IDatabaseContext databaseContext, ILogger<IAssuntoRepository> logger, IUsuarioAtualProvider usuarioAtual) {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        #region Civel Consumidor

        private IQueryable<AssuntoCommandResult> ObterBaseDoCivelConsumidor(AssuntoCivelConsumidorSort sort, bool ascending, string descricao) {
            var listaEstrategicoMigracao = DatabaseContext.Assuntos.Where(x => x.EhCivelEstrategico).Select(y => new { y.Id, y.Descricao, y.Ativo }).AsNoTracking().ToArray();
            IQueryable<AssuntoCommandResult> query = from a in DatabaseContext.Assuntos.AsNoTracking()
                                                     join ma in DatabaseContext.MigracaoAssunto on a.Id equals ma.CodAssuntoCivel into LeftJoinMa
                                                     from ma in LeftJoinMa.DefaultIfEmpty()
                                                     where a.EhCivelConsumidor
                                                     select new AssuntoCommandResult(                                                     
                                                         a.Id,
                                                         a.Descricao,
                                                         a.Ativo,
                                                         (int?)ma.CodAssuntoCivelEstrategico == null ? false : listaEstrategicoMigracao.FirstOrDefault(z => z.Id == ma.CodAssuntoCivelEstrategico) != null ? listaEstrategicoMigracao.FirstOrDefault(z => z.Id == ma.CodAssuntoCivelEstrategico).Ativo : false,
                                                         (int?)ma.CodAssuntoCivelEstrategico,
                                                         (int?)ma.CodAssuntoCivelEstrategico == null ? null : listaEstrategicoMigracao.FirstOrDefault(z => z.Id == ma.CodAssuntoCivelEstrategico).Descricao,
                                                         a.Proposta,
                                                         a.Negociacao,
                                                         a.CodTipoContingencia
                                                     );

            string logName = "Assuntos do Civel Consumidor";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            switch (sort) {
                case AssuntoCivelConsumidorSort.Negociacao:
                    if (ascending) {
                        query = query.OrderBy(a => a.Negociacao == null ? 0 : 1).ThenBy(a => a.Negociacao);
                    } else {
                        query = query.OrderByDescending(a => a.Negociacao == null ? 0 : 1).ThenByDescending(a => a.Negociacao);
                    }

                    break;

                case AssuntoCivelConsumidorSort.Proposta:
                    if (ascending) {
                        query = query.OrderBy(a => a.Proposta == null ? 0 : 1).ThenBy(a => a.Proposta);
                    } else {
                        query = query.OrderByDescending(a => a.Proposta == null ? 0 : 1).ThenByDescending(a => a.Proposta);
                    }

                    break;

                case AssuntoCivelConsumidorSort.Ativo:
                    query = query.SortBy(a => a.Ativo, ascending).ThenSortBy(a => a.Ativo, ascending);
                    break;

                case AssuntoCivelConsumidorSort.Id:
                    query = query.SortBy(a => a.Id, ascending).ThenSortBy(a => a.Id, ascending);
                    break;

                case AssuntoCivelConsumidorSort.CodTipoContingencia:
                case AssuntoCivelConsumidorSort.CalculoContingenciaTratada:
                        query = query.SortBy(a => a.CodTipoContingencia, ascending).ThenSortBy(a => a.Id, ascending);
                        break;

                case AssuntoCivelConsumidorSort.Descricao:
                default:
                    query = query.SortBy(x => x.Descricao, ascending).ThenSortBy(x => x.Descricao, ascending);
                    break;
            }

            return query.WhereIfNotNull(x => x.Descricao.ToString().ToUpper().Contains(descricao.ToString().ToUpper()), descricao);
        }

        public CommandResult<PaginatedQueryResult<AssuntoCommandResult>> ObterPaginadoDoCivelConsumidor(int pagina, int quantidade,
            AssuntoCivelConsumidorSort sort, bool ascending, string pesquisa) {
            string logName = "Assuntos do Civel Consumidor";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ASSUNTO_CIVEL_CONSUMIDOR)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ASSUNTO_CIVEL_CONSUMIDOR, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<AssuntoCommandResult>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));            
            var listaBase = ObterBaseDoCivelConsumidor(sort, ascending, pesquisa).ToArray();          

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<AssuntoCommandResult>() {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<AssuntoCommandResult>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<AssuntoCommandResult>> ObterDoCivelConsumidor(AssuntoCivelConsumidorSort sort,
            bool ascending, string descricao) {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ASSUNTO_CIVEL_CONSUMIDOR)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ASSUNTO_CIVEL_CONSUMIDOR, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<AssuntoCommandResult>>.Forbidden();
            }

            string logName = "Assuntos do Civel Consumidor";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBaseDoCivelConsumidor(sort, ascending, descricao).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<AssuntoCommandResult>>.Valid(resultado);

        }

        public CommandResult<IReadOnlyCollection<Assunto>> ObterDescricaoDeParaCivelConsumidor()
        {

            IQueryable<Assunto> query = DatabaseContext.Assuntos.AsNoTracking().Where(x => x.EhCivelConsumidor);


            return CommandResult<IReadOnlyCollection<Assunto>>.Valid(query.ToArray());
        }

        #endregion Civel Consumidor

        #region Civel Estrategico

        private IQueryable<AssuntoEstrategicoCommandResult> ObterBaseDoCivelEstrategico(AssuntoCivelEstrategicoSort sort, bool ascending, string descricao) {
            var listaConsumidorMigracao = DatabaseContext.Assuntos.Where(x => x.EhCivelConsumidor).Select(y => new { y.Id, y.Descricao, y.Ativo }).AsNoTracking().ToArray();
            IQueryable<AssuntoEstrategicoCommandResult> query = from a in DatabaseContext.Assuntos.AsNoTracking()
                                                     join ma in DatabaseContext.MigracaoAssuntoEstrategico on a.Id equals ma.CodAssuntoCivelEstrat into LeftJoinMa
                                                     from ma in LeftJoinMa.DefaultIfEmpty()
                                                     where a.EhCivelEstrategico
                                                     select new AssuntoEstrategicoCommandResult(
                                                         a.Id,
                                                         a.Descricao,
                                                        a.Ativo,
                                                         (int?)ma.CodAssuntoCivelCons == null ? false : listaConsumidorMigracao.FirstOrDefault(z => z.Id == ma.CodAssuntoCivelCons) != null ? listaConsumidorMigracao.FirstOrDefault(z => z.Id == ma.CodAssuntoCivelCons).Ativo : false,
                                                         (int?)ma.CodAssuntoCivelCons,
                                                         (int?)ma.CodAssuntoCivelCons == null ? null : listaConsumidorMigracao.FirstOrDefault(z => z.Id == ma.CodAssuntoCivelCons).Descricao
                                                         
                                                     );

            string logName = "Assuntos do Civel Estratégico";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            switch (sort) {
                case AssuntoCivelEstrategicoSort.Ativo:
                    query = query.SortBy(a => a.Ativo, ascending).ThenSortBy(a => a.Ativo, ascending);
                    break;

                case AssuntoCivelEstrategicoSort.Id:
                    query = query.SortBy(a => a.Id, ascending).ThenSortBy(a => a.Id, ascending);
                    break;

                case AssuntoCivelEstrategicoSort.Descricao:
                default:
                    query = query.SortBy(x => x.Descricao, ascending).ThenSortBy(x => x.Descricao, ascending);
                    break;
            }

            return query.WhereIfNotNull(x => x.Descricao.ToString().ToUpper().Contains(descricao.ToString().ToUpper()), descricao);
        }

        public CommandResult<PaginatedQueryResult<AssuntoEstrategicoCommandResult>> ObterPaginadoDoCivelEstrategico(int pagina, int quantidade,
            AssuntoCivelEstrategicoSort sort, bool ascending, string descricao) {
            string logName = "Assuntos do Civel Estratégico";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ASSUNTO_CIVEL_ESTRATEGICO)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ASSUNTO_CIVEL_ESTRATEGICO, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<AssuntoEstrategicoCommandResult>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBaseDoCivelEstrategico(sort, ascending, descricao);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<AssuntoEstrategicoCommandResult>() {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<AssuntoEstrategicoCommandResult>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<AssuntoEstrategicoCommandResult>> ObterDoCivelEstrategico(AssuntoCivelEstrategicoSort sort,
            bool ascending, string descricao) {

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ASSUNTO_CIVEL_ESTRATEGICO)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ASSUNTO_CIVEL_ESTRATEGICO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<AssuntoEstrategicoCommandResult>>.Forbidden();
            }

            string logName = "Assuntos do Civel Estratégico";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBaseDoCivelEstrategico(sort, ascending, descricao).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<AssuntoEstrategicoCommandResult>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<Assunto>> ObterDescricaoDeParaCivelEstrategico()
        {

            IQueryable<Assunto> query = DatabaseContext.Assuntos.AsNoTracking().Where(x => x.EhCivelEstrategico);


            return CommandResult<IReadOnlyCollection<Assunto>>.Valid(query.ToArray());
        }

        #endregion Civel Estrategico
    }
}