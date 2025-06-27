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
    internal class TipoDeParticipacaoRepository : ITipoDeParticipacaoRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<ITipoDeParticipacaoRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public TipoDeParticipacaoRepository(IDatabaseContext databaseContext, ILogger<ITipoDeParticipacaoRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<TipoDeParticipacao> ObterBaseDoCivelEstrategico(TipoDeParticipacaoSort sort, bool ascending, string pesquisa)
        {
            string logName = "Tipo de Participação";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<TipoDeParticipacao> query = DatabaseContext.TiposDeParticipacoes.AsNoTracking();

            switch (sort)
            {
                case TipoDeParticipacaoSort.Codigo:
                    query = query.SortBy(a => a.Codigo, ascending);
                    break;

                case TipoDeParticipacaoSort.Descricao:
                default:
                    query = query.SortBy(a => a.Descricao, ascending);
                    break;
            }

            return query.WhereIfNotNull(x => x.Descricao.ToUpper().Contains(pesquisa.ToUpper()), pesquisa);
        }

        public CommandResult<PaginatedQueryResult<TipoDeParticipacao>> ObterPaginado(int pagina, int quantidade, TipoDeParticipacaoSort sort, bool ascending, string pesquisa)
        {
            string logName = "Tipo de Participação";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_DE_PARTICIPACAO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_DE_PARTICIPACAO, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<TipoDeParticipacao>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBaseDoCivelEstrategico(sort, ascending, pesquisa);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<TipoDeParticipacao>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<TipoDeParticipacao>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<TipoDeParticipacao>> Obter(TipoDeParticipacaoSort sort, bool ascending, string pesquisa)
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_DE_PARTICIPACAO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_DE_PARTICIPACAO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<TipoDeParticipacao>>.Forbidden();
            }

            string logName = "Tipo de Participação";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBaseDoCivelEstrategico(sort, ascending, pesquisa).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<TipoDeParticipacao>>.Valid(resultado);
        }
    }
}