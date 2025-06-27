using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.External;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    internal class NaturezaAcaoBBRepository : INaturezaAcaoBBRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<INaturezaAcaoBBRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public NaturezaAcaoBBRepository(IDatabaseContext databaseContext, ILogger<INaturezaAcaoBBRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult<IReadOnlyCollection<NaturezaAcaoBB>> ObterNaturezasAcoesBB(int? naturezaId)
        {
            string logName = "Natureza Ações BB";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = DatabaseContext.NaturezasDasAcoesBB.AsNoTracking().WhereIfNotNull(x => x.Id == naturezaId, naturezaId).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<NaturezaAcaoBB>>.Valid(resultado);
        }
    }
}
