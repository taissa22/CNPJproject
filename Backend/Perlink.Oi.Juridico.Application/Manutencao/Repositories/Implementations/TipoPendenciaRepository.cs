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

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    internal class TipoPendenciaRepository: ITipoPendenciaRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<ITipoPendenciaRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public TipoPendenciaRepository(IDatabaseContext databaseContext, ILogger<ITipoPendenciaRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<TipoPendencia> ObterBase(TipoPendenciaSort sort, bool ascending, string pesquisa = null)
        {
            string logName = "Tipo de Pendencia";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<TipoPendencia> query = DatabaseContext.TipoPendencias.AsNoTracking();

            switch (sort)
            {
                case TipoPendenciaSort.Id:
                    query = query.SortBy(a => a.Id, ascending);
                    break;


                case TipoPendenciaSort.Descricao:
                default:
                    query = query.SortBy(a => a.Descricao, ascending);
                    break;
            }

            return query.WhereIfNotNull(x => x.Descricao.ToUpper().Contains(pesquisa.ToString().ToUpper()), pesquisa);
    
        }

        public CommandResult<PaginatedQueryResult<TipoPendencia>> ObterPaginado(int pagina, int quantidade,
          TipoPendenciaSort sort, bool ascending, string pesquisa = null)
        {
            
            string logName = "Tipo de Pendencias";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_PENDENCIA))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_PENDENCIA, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<TipoPendencia>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(sort, ascending, pesquisa);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<TipoPendencia>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<TipoPendencia>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<TipoPendencia>> Obter(TipoPendenciaSort sort, bool ascending, string pesquisa = null)
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_TIPO_PENDENCIA))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_TIPO_PENDENCIA, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<TipoPendencia>>.Forbidden();
            }

            string logName = "Tipo de Pendencias";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(sort, ascending, pesquisa).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<TipoPendencia>>.Valid(resultado);
        }
    }
}
