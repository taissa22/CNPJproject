using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    internal class TribunalBBRepository: ITribunalBBRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<ITribunalBBRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public TribunalBBRepository(IDatabaseContext databaseContext, ILogger<ITribunalBBRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult<IReadOnlyCollection<TribunalBB>> ObterTodos()
        {
            
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_COMARCA))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_COMARCA, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<TribunalBB>>.Forbidden();
            }

            string logName = "ComarcaBB";

            var resultado = DatabaseContext.TribunalBBs.SortBy(t => t.Nome.Trim(),true).AsNoTracking().ToList();

            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));

            return CommandResult<IReadOnlyCollection<TribunalBB>>.Valid(resultado);
        }
    }
}
