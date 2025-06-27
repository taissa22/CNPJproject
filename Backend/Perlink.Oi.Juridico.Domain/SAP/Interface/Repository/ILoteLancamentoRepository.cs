using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.LoteCriacao;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Repository {
    public interface ILoteLancamentoRepository : IBaseCrudRepository<LoteLancamento,long> {
        Task<IEnumerable<LoteResultadoDTO>> FiltroConsultaLote(LoteFiltroDTO filtros);
        Task<TotaisLoteResultadoDTO> TotalFiltroConsultaLote(LoteFiltroDTO filtros);
        Task<LancamentoProcesso> RetornaLancamentoDoLote(long codigoLote);
        Task<List<LancamentoProcesso>> RetornaLancamentosDoLote(long codigoLote);
        Task<IEnumerable<LoteExportarDTO>> ExportarConsultaLote(LoteFiltroDTO loteFiltroDTO);
       
        Task VinculoLancamento(Lote lote, IList<DadosLoteCriacaoLancamentoDTO> dadosLancamentoDTOs);
        double TotalValorLotes { get; set; }
        long QuantidadesLancamentos { get; set; }
    }
}
