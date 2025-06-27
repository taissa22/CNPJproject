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

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    internal class CotacaoRepository : ICotacaoRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<ICotacaoRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public CotacaoRepository(IDatabaseContext databaseContext, ILogger<ICotacaoRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<Cotacao> ObterBase(CotacaoSort sort, bool ascending, int codigoIndice, DateTime dataInicial, DateTime dataFinal, string pesquisa = null)
        {
            string logName = "Cotações";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<Cotacao> query = DatabaseContext.Cotacoes.Where(x => x.Id == codigoIndice && x.DataCotacao.Date >= dataInicial.Date && x.DataCotacao.Date <= dataFinal.Date).AsNoTracking();

            switch (sort)
            {
                case CotacaoSort.Id:
                    query = query.SortBy(a => a.Id, ascending);
                    break;

                case CotacaoSort.Valor:
                    query = query.SortBy(a => a.Valor, ascending);
                    break;

                case CotacaoSort.DataCotacao:
                default:
                    query = query.SortBy(a => a.DataCotacao, ascending);
                    break;
            }

            return query.WhereIfNotNull(x => x.Indice.Descricao.ToUpper().Contains(pesquisa.ToString().ToUpper()), pesquisa);
        }

        public CommandResult<PaginatedQueryResult<Cotacao>> ObterPaginado(int pagina, int quantidade, int codigoIndice,
          CotacaoSort sort, bool ascending, DateTime dataInicial, DateTime dataFinal, string pesquisa = null)
        {
            string logName = "Cotações";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_COTACAO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_COTACAO, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<Cotacao>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(sort, ascending, codigoIndice, dataInicial, dataFinal, pesquisa);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<Cotacao>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<Cotacao>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<Cotacao>> Obter(int codigoIndice, CotacaoSort sort, bool ascending, DateTime dataInicial, DateTime dataFinal, string pesquisa = null)
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_COTACAO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_COTACAO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<Cotacao>>.Forbidden();
            }

            string logName = "Cotações";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(sort, ascending, codigoIndice, dataInicial, dataFinal, pesquisa).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Cotacao>>.Valid(resultado);
        }
    }
}