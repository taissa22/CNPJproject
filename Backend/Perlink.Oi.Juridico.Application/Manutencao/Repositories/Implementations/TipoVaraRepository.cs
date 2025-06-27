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
    internal class TipoVaraRepository : ITipoVaraRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<ITipoVaraRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public TipoVaraRepository(IDatabaseContext databaseContext, ILogger<TipoVaraRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<TipoVara> ObterBase(TipoVaraSort sort, bool ascending, string pesquisa = null)
        {
            string logName = "Tipo de Vara";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<TipoVara> query = DatabaseContext.TiposDeVara.AsNoTracking();

            switch (sort)
            {
                case TipoVaraSort.Codigo:
                    query = query.SortBy(a => a.Id, ascending);
                    break;

                default:
                    query = query.SortBy(a => a.Nome, ascending);
                    break;
            }

            return query.WhereIfNotNull(x => x.Nome.ToUpper().Contains(pesquisa.ToString().ToUpper()), pesquisa);
        }

        public CommandResult<PaginatedQueryResult<TipoVara>> ObterPaginado(int pagina, int quantidade,
            TipoVaraSort sort, bool ascending, string pesquisa = null)
        {
            string logName = "Tipo de Vara";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_VARA))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_VARA, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<TipoVara>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(sort, ascending, pesquisa);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<TipoVara>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<TipoVara>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<TipoVara>> Obter(TipoVaraSort sort, bool ascending, string pesquisa = null)
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_VARA))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_VARA, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<TipoVara>>.Forbidden();
            }

            string logName = "Tipo de Vara";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(sort, ascending, pesquisa).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<TipoVara>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<TipoVara>> ObterTodos()
        {
            string logName = "Tipo de Vara";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var result = DatabaseContext.TiposDeVara.SortBy(t => t.Nome.Trim(),true)
                                        .AsNoTracking()
                                        .ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<TipoVara>>.Valid(result);
        }

        public CommandResult<bool> UtilizadoEmProcesso(int codTipoVara, int codTipoProcesso)
        {
            string logName = "Tipo de Vara";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Verificando se {logName} está sendo utilizado em Processo"));

            var result = DatabaseContext.Processos.Any(x => x.TipoVara.Id == codTipoVara && x.TipoProcessoId == codTipoProcesso);

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<bool>.Valid(result);
        }
    }
}