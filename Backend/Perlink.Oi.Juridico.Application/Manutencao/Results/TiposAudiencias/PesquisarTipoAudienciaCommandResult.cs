using Shared.Domain.Commands;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Results.TiposAudiencias
{
    public class PesquisarTipoAudienciaCommandResult : ICommandResult
    {
        public PesquisarTipoAudienciaCommandResult(IEnumerable<FiltroTipoAudienciaCommandResult> tiposAudiencias, int totalElementos)
        {
            TiposAudiencias = tiposAudiencias;
            TotalElementos = totalElementos;
        }

        public IEnumerable<FiltroTipoAudienciaCommandResult> TiposAudiencias { get; private set; }

        public int TotalElementos { get; private set; }
    }
}
