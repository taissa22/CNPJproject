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
    internal class IndiceCorrecaoEsferaRepository : IIndiceCorrecaoEsferaRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IIndiceCorrecaoEsferaRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public IndiceCorrecaoEsferaRepository(IDatabaseContext databaseContext, ILogger<IIndiceCorrecaoEsferaRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<IndiceCorrecaoEsfera> ObterBase(int esferaId, IndiceCorrecaoEsferaSort sort, bool ascending)
        {
            string logName = "Indice Correção Esfera";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<IndiceCorrecaoEsfera> query = DatabaseContext.IndiceCorrecaoEsferas.Where(x => x.EsferaId == esferaId).AsNoTracking();

            switch (sort)
            {
                case IndiceCorrecaoEsferaSort.EsferaId:
                    query = query.SortBy(a => a.EsferaId, ascending);
                    break;

                case IndiceCorrecaoEsferaSort.DataVigencia:
                    query = query.SortBy(a => a.DataVigencia, ascending);
                    break;
            }
           
            return query;
        }

        public CommandResult<PaginatedQueryResult<IndiceCorrecaoEsfera>> ObterPaginado(int esferaId, int pagina, int quantidade, IndiceCorrecaoEsferaSort sort, bool ascending)
        {
            string logName = "Esfera";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESFERA))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESFERA, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<IndiceCorrecaoEsfera>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(esferaId, sort, ascending);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<IndiceCorrecaoEsfera>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<IndiceCorrecaoEsfera>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<IndiceCorrecaoEsfera>> Obter(int esferaId, IndiceCorrecaoEsferaSort sort, bool ascending)
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESFERA))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESFERA, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<IndiceCorrecaoEsfera>>.Forbidden();
            }

            string logName = "Esera";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(esferaId,sort, ascending).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<IndiceCorrecaoEsfera>>.Valid(resultado);
        }
    }
}
