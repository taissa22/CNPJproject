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

namespace Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Repositories.Implementations {

    internal class PedidoDoProcessoRepository : IPedidoDoProcessoRepository {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IPedidoDoProcessoRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public PedidoDoProcessoRepository(IDatabaseContext databaseContext, ILogger<IPedidoDoProcessoRepository> logger, IUsuarioAtualProvider usuarioAtual) {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult<IReadOnlyCollection<PedidoProcesso>> ObterPorProcesso(int processoId) {
            string logName = "Pedido do Processo";

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_AGENDA_CE)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_AGENDA_CE, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<PedidoProcesso>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var result = DatabaseContext.PedidosDoProcesso
                            .Where(x => x.ProcessoId == processoId)
                            .OrderBy(e => e.Pedido.Descricao)
                            .AsNoTracking()
                            .ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<PedidoProcesso>>.Valid(result);
        }
    }
}