using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

    internal class EmpresaCentralizadoraRepository : IEmpresaCentralizadoraRepository {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IEmpresaCentralizadoraRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public EmpresaCentralizadoraRepository(IDatabaseContext databaseContext, ILogger<IEmpresaCentralizadoraRepository> logger, IUsuarioAtualProvider usuarioAtual) {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<EmpresaCentralizadora> ObterBase(EmpresaCentralizadoraSort sort, bool ascending, DataString? nome, int? ordem, int? codigo) {
            IQueryable<EmpresaCentralizadora> query = DatabaseContext.EmpresasCentralizadoras;

            switch (sort) {
                case EmpresaCentralizadoraSort.Codigo:
                    query = query.SortBy(x => x.Codigo, ascending).ThenSortBy(x => x.Codigo, ascending);
                    break;

                case EmpresaCentralizadoraSort.Ordem:
                    query = query.SortBy(x => x.Ordem, ascending).ThenSortBy(x => x.Ordem, ascending);
                    break;

                case EmpresaCentralizadoraSort.Nome:
                default:
                    query = query.SortBy(x => x.Nome, ascending).ThenSortBy(x => x.Nome, ascending);
                    break;
            }

            return query
                .WhereIfNotNull(x => x.Nome.ToUpper().ToString().Contains(nome.ToString().ToUpper()), nome)
                .WhereIfNotNull(x => x.Ordem == ordem, ordem)
                .WhereIfNotNull(x => x.Codigo == codigo, codigo)
                .AsNoTracking();
        }

        /// <summary>
        /// Método usado para exportação sem paginação
        /// </summary>
        public CommandResult<IReadOnlyCollection<EmpresaCentralizadora>> Obter(EmpresaCentralizadoraSort sort, bool ascending, DataString? nome, int? ordem, int? codigo) {
            string logName = "Empresas Centralizadoras";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_EMPRESA_CENTRALIZADORA)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_EMPRESA_CENTRALIZADORA, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<EmpresaCentralizadora>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(sort, ascending, nome, ordem, codigo).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<EmpresaCentralizadora>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<EmpresaCentralizadora>> Obter() {
            string logName = "Empresas Centralizadoras";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_EMPRESA_CENTRALIZADORA)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_EMPRESA_CENTRALIZADORA, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<EmpresaCentralizadora>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = DatabaseContext.EmpresasCentralizadoras;

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<EmpresaCentralizadora>>.Valid(resultado.ToArray());
        }

        public CommandResult<PaginatedQueryResult<EmpresaCentralizadora>> ObterPaginado(EmpresaCentralizadoraSort sort, bool ascending, int pagina, int quantidade,
            DataString? nome, int? ordem, int? codigo) {
            string logName = "Empresas Centralizadoras";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_EMPRESA_CENTRALIZADORA)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_EMPRESA_CENTRALIZADORA, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<EmpresaCentralizadora>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(sort, ascending, nome, ordem, codigo);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<EmpresaCentralizadora>() {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<EmpresaCentralizadora>>.Valid(resultado);
        }

        public CommandResult<bool> Existe(DataString nome, int? codigo) {
            string logName = "Empresas Centralizadoras";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Verificar {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_EMPRESA_CENTRALIZADORA)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_EMPRESA_CENTRALIZADORA, UsuarioAtual.Login));
                return CommandResult<bool>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));

            bool resultado = DatabaseContext.EmpresasCentralizadoras
                .WhereIfNotNull(x => x.Codigo != codigo, codigo)
                .Any(x => x.Nome == nome.ToString());

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<bool>.Valid(resultado);
        }
    }
}