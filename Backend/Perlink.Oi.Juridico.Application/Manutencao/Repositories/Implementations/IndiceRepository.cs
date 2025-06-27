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
    internal class IndiceRepository : IIndiceRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IIndiceRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public IndiceRepository(IDatabaseContext databaseContext, ILogger<IIndiceRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<Indice> ObterBase(IndicesSort sort, bool ascending, string pesquisa = null)
        {
            string logName = "Indice";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<Indice> query = DatabaseContext.Indices.AsNoTracking();

            switch (sort)
            {
                case IndicesSort.Id:
                    query = query.SortBy(a => a.Id, ascending);
                    break;

                case IndicesSort.CodigoTipoIndice:
                    query = query.SortBy(a => a.CodigoTipoIndice, ascending);
                    break;

                case IndicesSort.CodigoValorIndice:
                    query = query.SortBy(a => a.CodigoValorIndice, ascending);
                    break; 
                
                case IndicesSort.Acumulado:
                    query = query.SortBy(a => a.Acumulado, ascending);
                    break;

                case IndicesSort.AcumuladoAutomatico:
                    query = query.SortBy(a => a.AcumuladoAutomatico, ascending);
                    break;

                default:
                    query = query.SortBy(a => a.Descricao, ascending);
                    break;
            }

            return query.WhereIfNotNull(x => x.Descricao.ToUpper().Contains(pesquisa.ToString().ToUpper()), pesquisa);
        }

        public CommandResult<PaginatedQueryResult<Indice>> ObterPaginado(int pagina, int quantidade,
            IndicesSort sort, bool ascending, string pesquisa = null)
        {
            string logName = "Indice";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_INDICE))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_INDICE, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<Indice>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(sort, ascending, pesquisa);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<Indice>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<Indice>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<Indice>> Obter(IndicesSort sort, bool ascending, string pesquisa = null)
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_INDICE))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_INDICE, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<Indice>>.Forbidden();
            }

            string logName = "Indice";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(sort, ascending, pesquisa).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Indice>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<Indice>> ObterTodos()
        {
            string logName = "Indices";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var result = DatabaseContext.Indices
                                        .AsNoTracking()
                                        .ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Indice>>.Valid(result);
        }

        public CommandResult<bool> UtilizadoEmCotacao(int codIndice)
        {
            try
            {
                Logger.LogInformation("Verificando se o Índice encontra-se relacionado com Cotação Índice");
                bool utilizado = DatabaseContext.Cotacoes.Any(x => x.Indice.Id == codIndice);

                return CommandResult<bool>.Valid(utilizado);
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                return CommandResult<bool>.Invalid("Erro ao consultar relacionamento de Índice");
            }
        }
    }
}