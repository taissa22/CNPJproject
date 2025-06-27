using Shared.Domain.Commands;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Results.TiposParticipacao
{
    public class ExportarTipoParticipacaoCommandResult : ICommandResult
    {
        public ExportarTipoParticipacaoCommandResult(IEnumerable<FiltroTipoParticipacaoCommandResult> tiposParticipacao)
        {
            TiposParticipacao = tiposParticipacao;
        }

        public IEnumerable<FiltroTipoParticipacaoCommandResult> TiposParticipacao { get; private set; }
    }
}
