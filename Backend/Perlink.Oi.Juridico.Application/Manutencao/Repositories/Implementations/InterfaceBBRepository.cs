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
    public class InterfaceBBRepository : IInterfaceBBRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<ICentroDeCustoRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public InterfaceBBRepository(IDatabaseContext databaseContext, ILogger<ICentroDeCustoRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult<IReadOnlyCollection<InterfaceBB>> Obter()
        {
            string logName = "Interfaces BB";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_EMPRESA_DO_GRUPO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_EMPRESA_DO_GRUPO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<InterfaceBB>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var result = DatabaseContext.InterfacesBB
                                        .AsNoTracking()
                                        .ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<InterfaceBB>>.Valid(result);
        }
    }
}