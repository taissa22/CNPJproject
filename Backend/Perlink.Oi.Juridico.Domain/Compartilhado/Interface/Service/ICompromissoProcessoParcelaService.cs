using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO.EstornoLancamentoPago;
using Shared.Domain.Interface.Service;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service
{
    public interface ICompromissoProcessoParcelaService : IBaseCrudService<CompromissoProcessoParcela, long>
    {
        Task<DadosCompromissoEstornoDTO> ObterDadosCompromissoParaEstorno(long codigoProcesso, long codigoLancamento);
        Task<CompromissoProcessoParcela> ObterCompromissoParcela(long codigoProcesso, long codigoCompromisso, long codigoParcela);
        Task<long> ObterNextCodigoParcelaCompromissoProcesso(long codigoProcesso, long codigoCompromisso);
        Task<long> ObterNextNumeroParcelaCompromissoProcesso(long codigoProcesso, long codigoCompromisso);
        Task AtualizarCompromissoProcessoParcela(CompromissoProcessoParcela parcelaCompromisso);
    }
}
