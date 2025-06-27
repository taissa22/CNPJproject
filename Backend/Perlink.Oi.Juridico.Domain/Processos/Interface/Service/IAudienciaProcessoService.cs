using Perlink.Oi.Juridico.Domain.SAP.Entity.Processos;
using Shared.Domain.Interface.Service;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Processos.Interface.Service
{
    public interface IAudienciaProcessoService : IBaseCrudService<AudienciaProcesso, long>
    {
        Task<AudienciaProcesso> ObterPorChavesCompostas(long codProcesso, long seqAudiencia);
    }
}
