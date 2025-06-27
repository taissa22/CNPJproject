using Shared.Domain.Commands;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Results.TiposAudiencias
{
    public class ExportarTipoAudienciaCommandResult : ICommandResult
    {
        public ExportarTipoAudienciaCommandResult(IEnumerable<FiltroTipoAudienciaCommandResult> tiposAudiencias)
        {
            TiposAudiencias = tiposAudiencias;
        }

        public IEnumerable<FiltroTipoAudienciaCommandResult> TiposAudiencias { get; private set; }
    }
}
