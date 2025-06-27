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
    internal class EsferaRepository : IEsferaRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IEsferaRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public EsferaRepository(IDatabaseContext databaseContext, ILogger<IEsferaRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<Esfera> ObterBase(EsferasSort sort, bool ascending)
        {
            string logName = "Esfera";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<Esfera> query = DatabaseContext.Esferas.AsNoTracking();

            switch (sort)
            {
                case EsferasSort.Id:
                    query = query.SortBy(a => a.Id, ascending);
                    break;

                case EsferasSort.Nome:
                    query = query.SortBy(a => a.Nome, ascending);
                    break;

                case EsferasSort.CorrigePrincipal:
                    query = query.SortBy(a => a.CorrigePrincipal, ascending);
                    break;

                case EsferasSort.CorrigeMultas:
                    query = query.SortBy(a => a.CorrigeMultas, ascending);
                    break;

                case EsferasSort.CorrigeJuros:
                    query = query.SortBy(a => a.CorrigeJuros, ascending);
                    break;
            }
           
            return query;
        }

        public CommandResult<PaginatedQueryResult<Esfera>> ObterPaginado(int pagina, int quantidade, EsferasSort sort, bool ascending)
        {
            string logName = "Esfera";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESFERA))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESFERA, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<Esfera>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(sort, ascending);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<Esfera>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<Esfera>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<Esfera>> Obter(EsferasSort sort, bool ascending)
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESFERA))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESFERA, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<Esfera>>.Forbidden();
            }

            string logName = "Esera";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(sort, ascending).ToArray();


            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Esfera>>.Valid(resultado);
        }
    }
}
