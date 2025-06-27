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
    class MotivoProvavelZeroRepository : IMotivoProvavelZeroRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IMotivoProvavelZeroRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public MotivoProvavelZeroRepository(IDatabaseContext databaseContext, ILogger<IMotivoProvavelZeroRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<MotivoProvavelZero> ObterBase(MotivoProvavelZeroSort sort, bool ascending,string pesquisa = null)
        {
            string logName = "Motivo Provavel Zero";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<MotivoProvavelZero> query = DatabaseContext.MotivoProvavelZeros.AsNoTracking();
            
            switch (sort)
            {
                case MotivoProvavelZeroSort.Id:
                    query = query.SortBy(a => a.Id, ascending);
                    break;
                case MotivoProvavelZeroSort.Descricao:
                    query = query.SortBy(a => a.Descricao, ascending);
                    break;
                default:
                    query = query.SortBy(a => a.Id, ascending);
                    break;
            }

            return query.WhereIfNotNull(x => x.Descricao.ToLower().Contains(pesquisa.ToLower()) , pesquisa);
        }

        public CommandResult<PaginatedQueryResult<MotivoProvavelZero>> ObterPaginado(int pagina, int quantidade, MotivoProvavelZeroSort sort, bool ascending, string pesquisa = null)
        {
            string logName = "MotivoProvavelZero";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_MOTIVO_PROVAVEL_ZERO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_MOTIVO_PROVAVEL_ZERO, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<MotivoProvavelZero>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(sort, ascending,  pesquisa);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<MotivoProvavelZero>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<MotivoProvavelZero>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<MotivoProvavelZero>> Obter(MotivoProvavelZeroSort sort, bool ascending, string pesquisa = null)
        {

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_MOTIVO_PROVAVEL_ZERO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_MOTIVO_PROVAVEL_ZERO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<MotivoProvavelZero>>.Forbidden();
            }

            string logName = "MotivoProvavelZero";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(sort, ascending, pesquisa).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<MotivoProvavelZero>>.Valid(resultado);
        }

    }
}
