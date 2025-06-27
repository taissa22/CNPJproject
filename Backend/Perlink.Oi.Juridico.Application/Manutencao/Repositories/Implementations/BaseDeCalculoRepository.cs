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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    internal class BaseDeCalculoRepository: IBaseDeCalculoRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IBaseDeCalculoRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public BaseDeCalculoRepository(IDatabaseContext databaseContext, ILogger<IBaseDeCalculoRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<BaseDeCalculo> ObterBase(BaseDeCalculoSort sort, bool ascending, string pesquisa)
        {
            string logName = "Ações - Cível Estratégico";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<BaseDeCalculo> query = DatabaseContext.BasesDeCalculo.AsNoTracking();

            switch (sort)
            {
                case BaseDeCalculoSort.Codigo:
                    query = query.SortBy(a => a.IndBaseInicial == true, false).ThenSortBy(a => a.Codigo, ascending); 
                    break;

                case BaseDeCalculoSort.Descricao:
                default:
                    query = query.SortBy(a => a.IndBaseInicial == true, false).ThenSortBy(a => a.Descricao, ascending);
                    break;
            }

            return query.WhereIfNotNull(x => x.Descricao.ToUpper().Contains(pesquisa.ToUpper()), pesquisa);
        }

        public CommandResult<PaginatedQueryResult<BaseDeCalculo>> ObterPaginado(int pagina, int quantidade, BaseDeCalculoSort sort, bool ascending, string pesquisa)
        {

            string logName = "Base de Cálculo";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_BASE_DE_CALCULO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_BASE_DE_CALCULO, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<BaseDeCalculo>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(sort, ascending, pesquisa);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<BaseDeCalculo>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<BaseDeCalculo>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<BaseDeCalculo>> Obter(BaseDeCalculoSort sort, bool ascending, string pesquisa)
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_BASE_DE_CALCULO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_BASE_DE_CALCULO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<BaseDeCalculo>>.Forbidden();
            }

            string logName = "Base de Cálculo";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(sort, ascending, pesquisa).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<BaseDeCalculo>>.Valid(resultado);
        }
    }
}
