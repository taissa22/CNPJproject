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
    public class OrientacaoJuridicaRepository : IOrientacaoJuridicaRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IOrientacaoJuridicaRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public OrientacaoJuridicaRepository(IDatabaseContext databaseContext, ILogger<IOrientacaoJuridicaRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<OrientacaoJuridica> ObterBase(bool obterTrabalhista, OrientacaoJuridicaSort sort, bool ascending, string pesquisa = null)
        {
            string logName = "Orientacao Juridica";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<OrientacaoJuridica> query = DatabaseContext.OrientacoesJuridicas.AsNoTracking();           

            switch (sort)
            {
                case OrientacaoJuridicaSort.Codigo:
                    query = query.SortBy(x => x.CodOrientacaoJuridica, ascending);
                    break;

                case OrientacaoJuridicaSort.Ativo:
                    query = query.SortBy(x => x.Ativo, ascending);
                    break;

                case OrientacaoJuridicaSort.Nome:
                    query = query.SortBy(x => x.Nome, ascending);
                    break;

                case OrientacaoJuridicaSort.TipoOrientacaoJuridica:
                    query = query.SortBy(x => x.TipoOrientacaoJuridica.Descricao, ascending);
                    break;

                default:
                    query = query.SortBy(x => x.CodOrientacaoJuridica, ascending).ThenSortBy(x => x.CodOrientacaoJuridica, ascending);
                    break;
            }

            query = query.WhereIfNotNull(x => x.Descricao.ToUpper().Contains(pesquisa.ToString().ToUpper()), pesquisa);

            return query;
        }

        public CommandResult<IReadOnlyCollection<OrientacaoJuridica>> Obter(bool obterTrabalhista, OrientacaoJuridicaSort sort, bool ascending, string pesquisa = null)
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ORIENTACAO_JURIDICA))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ORIENTACAO_JURIDICA, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<OrientacaoJuridica>>.Forbidden();
            }

            string logName = "Orientacao Juridica";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(obterTrabalhista, sort, ascending, pesquisa).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<OrientacaoJuridica>>.Valid(resultado);
        }

        public CommandResult<PaginatedQueryResult<OrientacaoJuridica>> ObterPaginado(bool obterTrabalhista, OrientacaoJuridicaSort sort, bool ascending, int pagina, int quantidade, string pesquisa = null)
        {
            string logName = "Orientacao Juridica";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ORIENTACAO_JURIDICA))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ORIENTACAO_JURIDICA, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<OrientacaoJuridica>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(obterTrabalhista, sort, ascending, pesquisa);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<OrientacaoJuridica>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<OrientacaoJuridica>>.Valid(resultado);
        }
    }
}