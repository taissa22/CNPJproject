using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.External;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    internal class DecisaoEventoRepository : IDecisaoEventoRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IEventoRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public DecisaoEventoRepository(IDatabaseContext databaseContext, ILogger<IEventoRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<DecisaoEvento> ObterBase(int eventoId, DecisaoEventoSort sort, bool ascending)
        {
            string logName = "Decisão Evento";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<DecisaoEvento> query = DatabaseContext.DecisaoEventos.Where(x =>x.EventoId == eventoId).AsNoTracking();

            switch (sort)
            {
                case DecisaoEventoSort.Id:
                    query = query.SortBy(a => a.Id, ascending);
                    break;

                case DecisaoEventoSort.Descricao:
                    query = query.SortBy(a => a.Descricao, ascending);
                    break;

                case DecisaoEventoSort.PerdaPotencial:
                    query = query.SortBy(a => a.PerdaPotencial, ascending);
                    break;

                case DecisaoEventoSort.RiscoPerda:
                    query = query.SortBy(a => a.RiscoPerda, ascending);
                    break;

                case DecisaoEventoSort.ReverCalculo:
                    query = query.SortBy(a => a.ReverCalculo, ascending);
                    break;

                case DecisaoEventoSort.DecisaoDefault:
                    query = query.SortBy(a => a.DecisaoDefault, ascending);
                    break;
            }

             
          
            return query;
        }

        public CommandResult<PaginatedQueryResult<DecisaoEvento>> ObterPaginado(int eventoId, int pagina, int quantidade, DecisaoEventoSort sort, bool ascending)
        {
            string logName = "Evento";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            //if (!UsuarioAtual.TemPermissaoPara(Permissoes.))
            //{
            //    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes., UsuarioAtual.Login));
            //    return CommandResult<PaginatedQueryResult<Evento>>.Forbidden();
            //}

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(eventoId, sort, ascending);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<DecisaoEvento>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<DecisaoEvento>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<DecisaoEvento>> Obter(int eventoId, DecisaoEventoSort sort, bool ascending)
        {
            //if (!UsuarioAtual.TemPermissaoPara(Permissoes.))
            //{
            //    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes., UsuarioAtual.Login));
            //    return CommandResult<IReadOnlyCollection<Evento>>.Forbidden();
            //}

            string logName = "Evento";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(eventoId, sort, ascending).ToArray();


            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<DecisaoEvento>>.Valid(resultado);
        }
    }
}
