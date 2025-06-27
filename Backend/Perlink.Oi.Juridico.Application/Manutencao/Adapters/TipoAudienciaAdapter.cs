using Perlink.Oi.Juridico.Application.Manutencao.Results.TiposAudiencias;
using Perlink.Oi.Juridico.Domain.Manutencao.DTO;

namespace Perlink.Oi.Juridico.Application.Manutencao.Adapters
{
    public class TipoAudienciaAdapter
    {
        public static FiltroTipoAudienciaCommandResult ToCommandResult(FiltroTipoAudienciaResultadoDTO dto)
        {
            return new FiltroTipoAudienciaCommandResult
            {
                CodTipoAudiencia = dto.CodTipoAudiencia,
                Sigla = dto.Sigla,
                Descricao = dto.Descricao,
                TipoDeProcesso = dto.TipoProcesso,
                Ativo = dto.EstaAtivo ? "Sim" : "Não"
            };
        }
    }
}
