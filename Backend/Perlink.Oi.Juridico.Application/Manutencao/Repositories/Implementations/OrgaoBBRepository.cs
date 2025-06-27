using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.External;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    public class OrgaoBBRepository: IOrgaoBBRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IOrgaoBBRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public OrgaoBBRepository(IDatabaseContext databaseContext, ILogger<IOrgaoBBRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult<IReadOnlyCollection<OrgaoBB>> ObterPorTribunalEComarcaBB(int TribunalBBId,int ComarcaBBId)
        {

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_COMARCA))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_COMARCA, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<OrgaoBB>>.Forbidden();
            }

            string logName = "ComarcaBB";

            var reultado = DatabaseContext.OrgaoBBs.Where(o => o.TribunalBBId == TribunalBBId && o.ComarcaBBId == ComarcaBBId)
                .SortBy(t => t.Nome.Trim(), true).AsNoTracking().ToList();

            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));

            return CommandResult<IReadOnlyCollection<OrgaoBB>>.Valid(reultado);
        }


    }
}
