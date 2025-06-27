using Shared.Domain.Commands;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Results.TiposParticipacao
{
    public class PesquisarTipoParticipacaoCommandResult : ICommandResult
    {
        public PesquisarTipoParticipacaoCommandResult(IEnumerable<FiltroTipoParticipacaoCommandResult> tiposParticipacao, int totalElementos)
        {
            Lista = tiposParticipacao;
            Total = totalElementos;
        }

        public IEnumerable<FiltroTipoParticipacaoCommandResult> Lista { get; private set; }

        public int Total { get; private set; }
    }
}
