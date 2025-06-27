using Perlink.Oi.Juridico.Domain.Processos.DTO.AgendaAudiencia;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Processos;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.Processos
{
    public interface IAdvogadoEscritorioRepository : IBaseCrudRepository<AdvogadoEscritorio, long>
    {
        Task<IEnumerable<AdvogadoEscritorioDTO>> RecuperarAdvogadoEscritorio();

        Task<IEnumerable<AdvogadoEscritorioDTO>> RecuperarAdvogadoEscritorioPorCodigoProfissional(long codigoEscritorio);

        Task<IEnumerable<AdvogadoEscritorioDTO>> RecuperarAdvogadoEscritorioUsuarioEscritorio(string codUsuario);
    }
}
