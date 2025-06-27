using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.ManutencaoFactory.Eventos;
using Perlink.Oi.Juridico.Application.Manutencao.Results.Evento;
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

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    internal class EventoRepository : IEventoRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IEventoRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public EventoRepository(IDatabaseContext databaseContext, ILogger<IEventoRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private string PermissaoPorTipoProcesso(TipoProcessoManutencao tipoProcesso)
        {
            switch (tipoProcesso.Id)
            {
                case 1: return Permissoes.ACESSAR_EVENTO_CIVEL_CONSUMIDOR;
                case 2: return Permissoes.ACESSAR_EVENTO_TRABALHISTA;
                case 3: return Permissoes.ACESSAR_EVENTO_ADMINSTRATIVO;
                case 4: return Permissoes.ACESSAR_EVENTO_TRIBUTARIO_ADMINISTRATIVO;
                case 5: return Permissoes.ACESSAR_EVENTO_TRIBUTARIO_JUDICIAL;
                case 6: return Permissoes.ACESSAR_EVENTO_TRABALHISTA_ADMINISTRATIVO;
                case 9: return Permissoes.ACESSAR_EVENTO_CIVEL_ESTRATEGICO;

                default: return "";
            }
        }

        private IQueryable<Evento> ObterBase(TipoProcessoManutencao tipoProcesso, EventoSort sort, bool ascending, string pesquisa)
        {
            string logName = "Evento";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<Evento> query = EventoFactory.EventoResult(tipoProcesso)
                .CreateQuery(DatabaseContext).GerarQuery(tipoProcesso.Id, sort, UsuarioAtual, ascending, pesquisa);

            
            return query;
        }

        public CommandResult<PaginatedQueryResult<Evento>> ObterPaginado(TipoProcessoManutencao tipoProcesso, int pagina, int quantidade, EventoSort sort, bool ascending, string pesquisa)
        {
            string logName = "Evento";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(PermissaoPorTipoProcesso(tipoProcesso)))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(PermissaoPorTipoProcesso(tipoProcesso), UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<Evento>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(tipoProcesso, sort, ascending, pesquisa);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<Evento>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<Evento>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<Evento>> Obter(TipoProcessoManutencao tipoProcesso, EventoSort sort, bool ascending, string pesquisa)
        {
            if (!UsuarioAtual.TemPermissaoPara(PermissaoPorTipoProcesso(tipoProcesso)))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(tipoProcesso.Descricao, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<Evento>>.Forbidden();
            }

            string logName = "Evento";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(tipoProcesso, sort, ascending, pesquisa).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Evento>>.Valid(resultado);
        }
        public CommandResult<IReadOnlyCollection<Evento>> ObterDependente(int eventoId, bool ascending)
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_EVENTO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_EVENTO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<Evento>>.Forbidden();
            }

            string logName = "Evento Dependente";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBaseDependente(ascending, eventoId).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Evento>>.Valid(resultado);
        }


        public CommandResult<PaginatedQueryResult<Evento>> ObterDependentePaginado(int pagina, int quantidade, bool ascending, int eventoId)
        {
            string logName = "Evento";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_EVENTO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_EVENTO, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<Evento>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBaseDependente(ascending, eventoId);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<Evento>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<Evento>>.Valid(resultado);
        }

        private IQueryable<Evento> ObterBaseDependente(bool ascending, int eventoId)
        {
            string logName = "Evento";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<Evento> query = from a in DatabaseContext.EventosDependentes.Where(x => x.EventoDependenteId == eventoId).AsNoTracking()
                                       join d in DatabaseContext.Eventos on a.EventoId equals d.Id
                                       where 1 == 1
                                       select d;

            query = query.SortBy(a => a.Nome, ascending);
            return query;
        }

        public IEnumerable<EventoDisponivelCommandResult> ObterDisponiveis(int eventoId)
        {
            string logName = "Evento";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));

            IQueryable<EventoDisponivelCommandResult> query = from a in DatabaseContext.Eventos.Where(x => x.EhTrabalhista.GetValueOrDefault()).AsNoTracking()
                                                              let qtd = (int)DatabaseContext.EventosDependentes.Where(x => x.EventoDependenteId == eventoId && x.EventoId == a.Id).Count()
                                                              select new EventoDisponivelCommandResult { Id = a.Id, Label = a.Nome, Selecionado = qtd > 0 };

            query = query.SortBy(a => a.Label, true);

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return query.ToList();
        }

        public IEnumerable<Evento> ObterDescricaoEstrategico()
        {
            return DatabaseContext.Eventos.Where(x => x.EhCivelEstrategico).AsNoTracking();
        }

        public IEnumerable<Evento> ObterDescricaoConsumidor()
        {
            return DatabaseContext.Eventos.Where(x => x.EhCivel.Value).AsNoTracking();
        }
    }
}