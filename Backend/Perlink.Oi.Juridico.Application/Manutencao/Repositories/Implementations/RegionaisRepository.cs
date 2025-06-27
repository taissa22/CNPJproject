using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
   public class RegionaisRepository : IRegionaisRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IRegionaisRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public RegionaisRepository(IDatabaseContext databaseContext, ILogger<IRegionaisRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult<IReadOnlyCollection<Regional>> Obter()
        {
            string logName = "Regionais";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_EMPRESA_DO_GRUPO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_EMPRESA_DO_GRUPO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<Regional>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var result = DatabaseContext.Regionais
                                  .AsNoTracking()
                                  .ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Regional>>.Valid(result);
        }
    }
}