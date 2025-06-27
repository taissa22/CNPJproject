using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.LoteCriacao;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Shared.Domain.Impl.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Service {
    public class LoteLancamentoService : BaseCrudService<LoteLancamento,long>, ILoteLancamentoService {
        private readonly ILoteLancamentoRepository LoteLancamentoRepository;
        private readonly ILoteRepository loteRepository;
        public LoteLancamentoService(ILoteLancamentoRepository LoteLancamentoRepository, ILoteRepository loteRepository) :base(LoteLancamentoRepository) {
            this.LoteLancamentoRepository = LoteLancamentoRepository;
            this.loteRepository = loteRepository;
        }

        

        public async Task VinculoLancamento(Lote lote, IList<DadosLoteCriacaoLancamentoDTO> dadosLancamentoDTOs)
        {
            await LoteLancamentoRepository.VinculoLancamento(lote, dadosLancamentoDTOs);
        }
    }
}
