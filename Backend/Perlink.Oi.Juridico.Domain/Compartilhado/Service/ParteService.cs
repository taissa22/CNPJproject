using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.Processos.DTO.AgendaAudiencia;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Shared.Domain.Impl.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class ParteService : BaseCrudService<Parte, long>, IParteService
    {
        private readonly IParteRepository parteRepository;
        public ParteService(IParteRepository parteRepository) : base(parteRepository)
        {
            this.parteRepository = parteRepository;
        }

        public async Task<bool> ExisteParteComEmpresaSap(long codigoEmpresaSap)
        {
            return await parteRepository.ExisteParteComEmpresaSap(codigoEmpresaSap);
        }

        public async Task<ICollection<Reclamantes_ReclamadasDTO>> RecuperarReclamanteReclamadas(LancamentoEstornoFiltroDTO lancamentoEstornoFiltroDTO, bool reclamadas, long Id)
        {
            return await parteRepository.RecuperarReclamanteReclamadas(lancamentoEstornoFiltroDTO, reclamadas, Id);
        }
        public async Task<bool> ExisteCentroCustoAssociado(long codigoCentroCusto)
        {
            return await parteRepository.ExisteCentroCustoAssociado(codigoCentroCusto);
        }

        public async Task<ICollection<PartesDTO>> RecuperarPartesTrabalhistaPorCodigoProcesso(long codigoProcesso, bool autores)
        {
            return await parteRepository.RecuperarPartesTrabalhistaPorCodigoProcesso(codigoProcesso, autores);
        }

    }
}
