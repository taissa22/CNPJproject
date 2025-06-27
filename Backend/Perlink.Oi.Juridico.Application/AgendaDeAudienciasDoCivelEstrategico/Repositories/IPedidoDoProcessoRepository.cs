using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Repositories {

    public interface IPedidoDoProcessoRepository {

        CommandResult<IReadOnlyCollection<PedidoProcesso>> ObterPorProcesso(int processoId);
    }
}