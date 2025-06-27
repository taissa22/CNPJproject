using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.External;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Repositories.Implementations {

    internal class AssuntoRepository : IAssuntoRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IAssuntoRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public AssuntoRepository(IDatabaseContext databaseContext, ILogger<IAssuntoRepository> logger, IUsuarioAtualProvider usuarioAtual) {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }       

        public CommandResult<PaginatedQueryResult<Assunto>> ObterParaDropdown(int pagina, int quantidade, int assuntoId = 0) {
            string logName = "Assunto";
            
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));

            var listaBase = DatabaseContext.Assuntos
                                          .WhereIf(x => x.Id == assuntoId, assuntoId > 0 )
                                          .Where(x => x.EhCivelEstrategico)
                                          .OrderBy(x => x.Descricao );            

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<Assunto>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<Assunto>>.Valid(resultado);
        }
    }
}