using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    public class CentroDeCustoRepository : ICentroDeCustoRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<ICentroDeCustoRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public CentroDeCustoRepository(IDatabaseContext databaseContext, ILogger<ICentroDeCustoRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }


        public CommandResult<IReadOnlyCollection<CentroCusto>> Obter()
        {
            string logName = "Centros De Custo";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_EMPRESA_DO_GRUPO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_EMPRESA_DO_GRUPO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<CentroCusto>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var result = DatabaseContext.CentrosCustos
                                  .AsNoTracking()
                                  .ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<CentroCusto>>.Valid(result);
        }
    }
}