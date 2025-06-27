using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.LoteCriacao;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Repository
{
    public interface ILoteRepository : IBaseCrudRepository<Lote, long>
    {
        Task<string> RecuperarPorCodigoSAP(long cod);

        Task<string> RecuperarPorNumeroSAPCodigoTipoProcesso(long NumeroPedidoSAP, long CodigoTipoProcesso);

        Task<IEnumerable<Lote>> RecuperarPorDataCriacaoLote(DateTime DataCriacaoMin, DateTime DataCriacaoMax);

        Task<IEnumerable<Lote>> RecuperarPorDataCriacaoPedidoLote(DateTime DataCriacaoPedidoMin, DateTime DataCriacaoPedidoMax);

        Task<ICollection<Lote>> RecuperarPorParteCodigoSAP(long CodigoSapParte);

        Task<LoteDetalhesDTO> RecuperarDetalhes(long codigoLote);
        Task<long?> AtualizarNumeroLoteBBAsync(long codigoLote);
        Task<Lote> CriacaoLote(Lote lote);
        Task<bool> ExisteLoteComFornecedor(long codigoFornecedor);
        Task<bool> ExisteCentroCustoAssociado(long codigoCentroCusto);
        Task<bool> ExisteLoteComFormaPagamento(long codigoFormaPagamento);
        Task<long> ValidarNumeroLote(long numeroLote, long codigoTipoProcesso);
    }
}