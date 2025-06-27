using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO.EstornoLancamentoPago;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository
{
    public interface ICompromissoProcessoParcelaRepository : IBaseCrudRepository<CompromissoProcessoParcela, long>
    {
        Task<DadosCompromissoEstornoDTO> ObterDadosCompromissoParaEstorno(long codigoProcesso, long codigoLancamento);
        Task<CompromissoProcessoParcela> ObterCompromissoParcela(long codigoProcesso, long codigoCompromisso, long codigoParcela);
        Task<ICollection<CompromissoProcessoParcela>> ObterParcelasCompromissoProcesso(long codigoProcesso, long codigoCompromisso);
        List<CompromissoProcessoParcela> ObterListaParcelasCompromissoProcesso(long codigoProcesso, long codigoCompromisso);
        List<CompromissoProcessoParcela> ObterListaParcelasCompromissoProcessoReordenacao(long codigoProcesso, long codigoCompromisso);
        long ObterProximoCodigoParcela(long codigoProcesso, long codigoCompromisso);


    }
}
