using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Shared.Domain.Impl.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class EscritorioService : BaseCrudService<Profissional, long>, IEscritorioService
    {

        private readonly IEscritorioRepository EscritorioRepository;

        public EscritorioService(IEscritorioRepository EscritorioRepository) : base(EscritorioRepository)
        {
            this.EscritorioRepository = EscritorioRepository;
        }

        public async Task<ICollection<EscritorioDTO>> RecuperarAreaCivelEstrategico()
        {
            return await EscritorioRepository.RecuperarAreaCivelEstrategico();
        }

        public async Task<ICollection<EscritorioDTO>> RecuperarAreaCivilConsumidor()
        {
            return await EscritorioRepository.RecuperarAreaCivilConsumidor();
        }

        public async Task<ICollection<EscritorioDTO>> RecuperarProcon()
        {
            return await EscritorioRepository.RecuperarAreaProcon();
        }
        public async Task<ICollection<EscritorioDTO>> RecuperarEscritorioFiltro(long codigoTipoProcesso)
        {
            return await EscritorioRepository.RecuperarEscritorioFiltro(codigoTipoProcesso);
        }

        public async Task<ICollection<EscritorioDTO>> RecuperarEscritorioTrabalhistaFiltro(bool ehEscritorio, string codUsuario)
        {
            if (ehEscritorio)
                return await EscritorioRepository.RecuperarEscritorioTrabalhistaFiltroUsuarioEscritorio(codUsuario);
            else
                return await EscritorioRepository.RecuperarEscritorioTrabalhistaFiltro();
        }

        public async Task<ICollection<EscritorioDTO>> RecuperarEscritorioTrabalhista()
        {
            return await EscritorioRepository.RecuperarEscritorioTrabalhista();
        }
    }
}
