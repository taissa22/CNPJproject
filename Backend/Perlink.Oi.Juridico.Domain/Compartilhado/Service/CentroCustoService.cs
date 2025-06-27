using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoCentroCusto;
using Shared.Domain.Impl.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class CentroCustoService : BaseCrudService<CentroCusto,long>, ICentroCustoService {
        private readonly ICentroCustoRepository CentroCustoRepository;

        public CentroCustoService(ICentroCustoRepository CentroCustoRepository): base(CentroCustoRepository) {
            this.CentroCustoRepository = CentroCustoRepository;
        }


        public async Task<IEnumerable<CentroCusto>> RecuperarCentroCustoParaFiltroLote()
        {
            return await CentroCustoRepository.RecuperarCentroCustoParaFiltroLote();
        }

        public async Task<IEnumerable<CentroCustoResultadoDTO>> ConsultarCentrosCustos(CentroCustoFiltroDTO filtros)
        {
            return await CentroCustoRepository.ConsultarCentrosCustos(filtros);
        }

        public async Task<int> ObterQuantidadeTotalPorFiltro(CentroCustoFiltroDTO centroCustoFiltroDTO)
        {
            return await CentroCustoRepository.ObterQuantidadeTotalPorFiltro(centroCustoFiltroDTO);
        }

        public async Task<ICollection<CentroCustoResultadoDTO>> ExportarCentrosCustos(CentroCustoFiltroDTO centroCustoFiltroDTO)
        {
            return await CentroCustoRepository.ExportarCentrosCustos(centroCustoFiltroDTO);
        }

        public async Task CadastrarCentroCusto(CentroCusto entidade) {
            await CentroCustoRepository.CadastrarCentroCusto(entidade);
        }

        public async Task<bool> VerificarDuplicidadeDescricaoCentroCusto(CentroCusto centroCusto) {
            return await CentroCustoRepository.VerificarDuplicidadeDescricaoCentroCusto(centroCusto);
        }

        public async Task<bool> VerificarDuplicidadeCentroCustoSAP(CentroCusto centroCusto) {
            return await CentroCustoRepository.VerificarDuplicidadeCentroCustoSAP(centroCusto);
        }
    }
}
