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
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    internal class AdvogadodoEscritorioRepository : IAdvogadoDoEscritorioRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IAdvogadoDoEscritorioRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public AdvogadodoEscritorioRepository(IDatabaseContext databaseContext, ILogger<IAdvogadoDoEscritorioRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<AdvogadoDoEscritorio> ObterTodosBase(AdvogadodoEscritorioSort sort, bool ascending, string pesquisa = null)
        {
            string logName = "Advogado do Escritório";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));

            IQueryable<AdvogadoDoEscritorio> query = (from a in DatabaseContext.AdvogadosDosEscritorios                                                      
                                                      select a).AsNoTracking();
            switch (sort)
            {
                case AdvogadodoEscritorioSort.Estado:
                    query = query.SortBy(a => a.EstadoId, ascending);
                    break;

                case AdvogadodoEscritorioSort.Nome:
                    query = query.SortBy(a => a.Nome, ascending);
                    break;

                case AdvogadodoEscritorioSort.Telefone:
                    query = query.SortBy(a => a.Celular, ascending);
                    break;

                case AdvogadodoEscritorioSort.OAB:
                    query = query.SortBy(a => a.NumeroOAB, ascending);
                    break;

                default:
                    query = query.SortBy(a => a.Nome, ascending);
                    break;
            }

            query = query.WhereIfNotNull(x => x.Nome.ToString().ToUpper().Contains(pesquisa.ToString().ToUpper()), pesquisa);

            return query;
        }

        private IQueryable<AdvogadoDoEscritorio> ObterBase(int escritorioId, string estado, AdvogadodoEscritorioSort sort, bool ascending, string pesquisa = null)
        {
            string logName = "Advogado do Escritório";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));

            IQueryable<AdvogadoDoEscritorio> query = (from a in DatabaseContext.AdvogadosDosEscritorios
                                                      where ( a.EscritorioId == escritorioId )
                                                      select a).AsNoTracking();
            switch (sort)
            {
                case AdvogadodoEscritorioSort.Estado:
                    query = query.SortBy(a => a.EstadoId, ascending);
                    break;

                case AdvogadodoEscritorioSort.Nome:
                    query = query.SortBy(a => a.Nome, ascending);
                    break;

                case AdvogadodoEscritorioSort.Telefone:
                    query = query.SortBy(a => a.Celular, ascending);
                    break;

                case AdvogadodoEscritorioSort.OAB:
                    query = query.SortBy(a => a.NumeroOAB, ascending);
                    break;

                default:
                    query = query.SortBy(a => a.Nome, ascending);
                    break;
            }

            query = query.WhereIfNotNull(x => x.Nome.ToString().ToUpper().Contains(pesquisa.ToString().ToUpper()), pesquisa);
            query = query.WhereIfNotNull(x => x.EstadoId == estado, estado);

            return query;
        }

        public CommandResult<PaginatedQueryResult<AdvogadoDoEscritorio>> ObterPaginado(int escritorioId, string estado, int pagina, int quantidade, AdvogadodoEscritorioSort sort, bool ascending, string pesquisa = null)
        {
            string logName = "Advogado do Escritório";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));            

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(escritorioId, estado, sort, ascending, pesquisa);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<AdvogadoDoEscritorio>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<AdvogadoDoEscritorio>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<AdvogadoDoEscritorio>> Obter(int escritorioId, string estado, AdvogadodoEscritorioSort sort, bool ascending, string pesquisa = null)
        {
            string logName = "Advogado do Escritório";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESCRITORIO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESCRITORIO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<AdvogadoDoEscritorio>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(escritorioId, estado, sort, ascending, pesquisa).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<AdvogadoDoEscritorio>>.Valid(resultado);
        }

        public CommandResult<IEnumerable<AdvogadoDoEscritorio>> ObterTodos(AdvogadodoEscritorioSort sort, bool ascending, string pesquisa = null)
        {
            string logName = "Advogado do Escritório";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter Todos {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESCRITORIO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESCRITORIO, UsuarioAtual.Login));
                return CommandResult<IEnumerable<AdvogadoDoEscritorio>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterTodosBase(sort, ascending, pesquisa).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IEnumerable<AdvogadoDoEscritorio>>.Valid(resultado);
        }
    }
}