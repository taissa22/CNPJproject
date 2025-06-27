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
    public class EstadoRepository : IEstadoRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IEstadoRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public EstadoRepository(IDatabaseContext databaseContext, ILogger<IEstadoRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<Estado> ObterBase(EstadoSort sort, bool ascending, string estadoId )
        {
            string logName = nameof(Estado);
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<Estado> query = DatabaseContext.Estados.AsNoTracking();

            switch (sort)
            {
                case EstadoSort.Nome:
                    query = query.SortBy(a => a.Nome, ascending);
                    break;

                case EstadoSort.Sigla:
                    query = query.SortBy(a => a.Id, ascending);
                    break;

                case EstadoSort.TaxaJuros:
                    query = query.SortBy(a => a.ValorJuros, ascending);
                    break;

                default:
                    query = query.SortBy(a => a.Nome, ascending);
                    break;
            }


            return query.WhereIfNotNull(x => x.Id == estadoId , estadoId);
        }


        public CommandResult<PaginatedQueryResult<Estado>> ObterPaginado(int pagina, int quantidade, string estadoId, EstadoSort sort, bool ascending)
        {
            string logName = nameof(Estado);
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

           
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESTADO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESTADO, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<Estado>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(sort, ascending, estadoId);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<Estado>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<Estado>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<Estado>> Obter(string estadoId, EstadoSort estado, bool direcao)
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESTADO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESTADO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<Estado>>.Forbidden();
            }


            string logName = nameof(Estado);
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(estado,direcao,estadoId).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Estado>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<Estado>> ObterTodos()
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESTADO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESTADO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<Estado>>.Forbidden();
            }


            string logName = nameof(Estado);
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(EstadoSort.Sigla, true, null).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Estado>>.Valid(resultado);
        }
    }
}
