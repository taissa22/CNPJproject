using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.LoteCriacao;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Interface.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Service
{
    public interface ILoteService : IBaseCrudService<Lote, long>
    {
        Task<string> RecuperarPorCodigoSAP(long Cod);
        Task<string> RecuperarPorNumeroSAPCodigoTipoProcesso(long NumeroPedidoSAP, long CodigoTipoProcesso);
        Task<IEnumerable<Lote>> RecuperarPorDataCriacaoLote(DateTime DataCriacaoMin, DateTime DataCriacaoMax);
        Task<IEnumerable<Lote>> RecuperarPorDataCriacaoPedidoLote(DateTime DataCriacaoPedidoMin, DateTime DataCriacaoPedidoMax);
        Task<ICollection<Lote>> RecuperarPorParteCodigoSAP(long CodigoSapParte);
        Task<bool> ExisteLoteComFormaPagamento(long codigoFormaPagamento);
        Task<LoteDetalhesDTO> RecuperaDetalhes(long codigoLote);
        Task<CompromissoLoteDTO> VerificarCompromisso(long codigoLote);
        Task<long?> AtualizarNumeroLoteBBAsync(long codigoLote);
        Task<LoteCanceladoDTO> CancelamentoLote(LoteCancelamentoDTO loteCancelamentoDTO, CargaCompromissoParcelaCancelamentoDTO infos=null);
        Task<Lote> CriacaoLote(Lote lote);
        Task<bool> ExisteLoteComFornecedor(long codigoFornecedor);
        Task<bool> ExisteCentroCustoAssociado(long codigoCentroCusto);
        Task<long> ValidarNumeroLote(long numeroLote, long codigoTipoProcesso);
    }
}
