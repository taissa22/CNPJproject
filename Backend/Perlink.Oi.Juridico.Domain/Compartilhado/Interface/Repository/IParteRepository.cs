using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Processos.DTO.AgendaAudiencia;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository
{
    public interface IParteRepository : IBaseCrudRepository<Parte, long>
    {
        Task<bool> ExisteParteComEmpresaSap(long codigoEmpresaSap);

        Task<ICollection<Reclamantes_ReclamadasDTO>> RecuperarReclamanteReclamadas(LancamentoEstornoFiltroDTO lancamentoEstornoFiltroDTO, bool reclamadas, long Id);

        Task<bool> ExisteCentroCustoAssociado(long codigoCentroCusto);

        Task<ICollection<PartesDTO>> RecuperarPartesTrabalhistaPorCodigoProcesso(long codigoProcesso, bool autores);

        Task<IList<Parte>> ListarEmpresasSapNaoAssociadas();
    }
}