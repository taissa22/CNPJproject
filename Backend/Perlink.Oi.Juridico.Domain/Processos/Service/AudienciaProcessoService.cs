using Perlink.Oi.Juridico.Domain.Processos.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Processos.Interface.Service;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Processos;
using Shared.Domain.Impl.Service;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Processos.Service
{
    public class AudienciaProcessoService : BaseCrudService<AudienciaProcesso, long>, IAudienciaProcessoService
    {
        private readonly IAudienciaProcessoRepository _audienciaProcessoRepository;

        public AudienciaProcessoService(IAudienciaProcessoRepository repository) : base(repository)
        {
            _audienciaProcessoRepository = repository;
        }

        public async Task<AudienciaProcesso> ObterPorChavesCompostas(long codProcesso, long seqAudiencia)
        {
            return await _audienciaProcessoRepository.ObterPorChavesCompostas(codProcesso, seqAudiencia);
        }
    }
}
