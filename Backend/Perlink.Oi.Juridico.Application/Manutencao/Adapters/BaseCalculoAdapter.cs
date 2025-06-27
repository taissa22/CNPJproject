using Perlink.Oi.Juridico.Application.Manutencao.Result.BaseCalculos;
using Perlink.Oi.Juridico.Domain.Manutencao.DTO;

namespace Perlink.Oi.Juridico.Application.Manutencao.Adapters
{
    public class BaseCalculoAdapter
    {
        public static FiltroBaseCalculoCommandResult ToCommandResult(FiltroBaseCalculoResultadoDTO dto)
        {
            return new FiltroBaseCalculoCommandResult
            {
                CodBaseCalculo = dto.CodBaseCalculo,
                Descricao = dto.Descricao,
                EhCalculoInicial = dto.EhCalculoInicial ? "Sim" : "Não"
            };
        }
    }
}
