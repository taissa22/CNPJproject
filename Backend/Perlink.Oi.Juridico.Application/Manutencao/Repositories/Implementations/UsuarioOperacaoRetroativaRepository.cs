using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations {

    public class UsuarioOperacaoRetroativaRepository : IUsuarioOperacaoRetroativaRepository
    {
        private IDatabaseContext DatabaseContext { get; }

        private ILogger<IUsuarioOperacaoRetroativaRepository> Logger { get; }

        private IUsuarioAtualProvider UsuarioAtual { get; }

        public UsuarioOperacaoRetroativaRepository(IDatabaseContext databaseContext, ILogger<IUsuarioOperacaoRetroativaRepository> logger, IUsuarioAtualProvider usuarioAtual) {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<UsuarioOperacaoRetroativa> ObterBase(UsuarioOperacaoRetroativaSort sort, bool ascending, string pesquisa = null) {
            IQueryable<UsuarioOperacaoRetroativa> query = DatabaseContext.UsuarioOperacaoRetroativas.AsNoTracking();

            switch (sort) {
                case UsuarioOperacaoRetroativaSort.Usuario:
                    query = query.SortBy(x => x.Usuario.Nome, ascending);
                    break;

                case UsuarioOperacaoRetroativaSort.LimiteAlteracao:
                    query = query.SortBy(x => x.LimiteAlteracao, ascending);
                    break;

                default:
                    query = query.SortBy(x => x.Usuario.Nome, ascending);
                    break;
            }
            query = query.WhereIfNotNull(x => x.Usuario.Nome.ToUpper().Contains(pesquisa.ToString().ToUpper()), pesquisa);
            return query;
        }

        public CommandResult<IReadOnlyCollection<UsuarioOperacaoRetroativa>> Obter(UsuarioOperacaoRetroativaSort sort, bool ascending, string  pesquisa = null) {
            
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_OPERACOES_RETROATIVAS)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_OPERACOES_RETROATIVAS, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<UsuarioOperacaoRetroativa>>.Forbidden();
            }

            string logName = "Usuario Configuracao Retroativa";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(sort, ascending, pesquisa).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<UsuarioOperacaoRetroativa>>.Valid(resultado);
        }

        public CommandResult<PaginatedQueryResult<UsuarioOperacaoRetroativa>> ObterPaginado(UsuarioOperacaoRetroativaSort sort, bool ascending, int pagina, int quantidade, string pesquisa = null ) {
            string logName = "Usuario Configuracao Retroativa";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_OPERACOES_RETROATIVAS)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_OPERACOES_RETROATIVAS, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<UsuarioOperacaoRetroativa>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase( sort, ascending, pesquisa);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);


            var resultado = new PaginatedQueryResult<UsuarioOperacaoRetroativa>() {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<UsuarioOperacaoRetroativa>>.Valid(resultado);
        }
      
    }
}