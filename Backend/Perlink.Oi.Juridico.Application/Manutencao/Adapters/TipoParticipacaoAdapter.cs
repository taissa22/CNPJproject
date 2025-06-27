using Perlink.Oi.Juridico.Application.Manutencao.Results.TiposParticipacao;
using Perlink.Oi.Juridico.Domain.Manutencao.DTO;

namespace Perlink.Oi.Juridico.Application.Manutencao.Adapters
{
    public class TipoParticipacaoAdapter
    {
        public static FiltroTipoParticipacaoCommandResult ToCommandResult(FiltroTipoParticipacaoResultadoDTO dto)
        {
            return new FiltroTipoParticipacaoCommandResult
            {
                CodTipoParticipacao = dto.CodTipoParticipacao,
                Descricao = dto.Descricao
            };
        }
    }
}
