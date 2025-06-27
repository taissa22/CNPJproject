using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Perlink.Oi.Juridico.Infra.Constants;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    public class ComarcaBBRepository : IComarcaBBRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IComarcaBBRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public ComarcaBBRepository(IDatabaseContext databaseContext, ILogger<IComarcaBBRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult<IReadOnlyCollection<ComarcaBB>> ObterPorEstado(string estadoId)
        {
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_COMARCA))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_COMARCA, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<ComarcaBB>>.Forbidden();
            }

            string logName = "ComarcaBB";

            var comarcasBB = DatabaseContext.ComarcaBBs.Where(c => c.EstadoId == estadoId).AsNoTracking().ToList();

            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));

            return CommandResult<IReadOnlyCollection<ComarcaBB>>.Valid(comarcasBB);

        }
    }
}
