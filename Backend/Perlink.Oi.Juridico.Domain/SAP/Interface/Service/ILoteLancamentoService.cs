using Perlink.Oi.Juridico.Domain.SAP.DTO.LoteCriacao;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Interface.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Service {
    public interface ILoteLancamentoService : IBaseCrudService<LoteLancamento, long>
    {
        Task VinculoLancamento(Lote lote, IList<DadosLoteCriacaoLancamentoDTO> dadosLancamentoDTOs);
    }
}
