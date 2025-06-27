using Perlink.Oi.Juridico.Domain.SAP.Entity.Processos;
using Shared.Domain.Interface.Repository;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Processos.Interface.Repository
{
    public interface IAudienciaProcessoRepository : IBaseCrudRepository<AudienciaProcesso, long>
    {
        Task<AudienciaProcesso> ObterPorChavesCompostas(long codProcesso, long seqAudiencia);
    }
}
