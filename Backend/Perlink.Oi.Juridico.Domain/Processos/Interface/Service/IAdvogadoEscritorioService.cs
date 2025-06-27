using Perlink.Oi.Juridico.Domain.Processos.DTO.AgendaAudiencia;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Processos;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Service
{
    public interface IAdvogadoEscritorioService : IBaseCrudService<AdvogadoEscritorio, long>
    {
        Task<IEnumerable<AdvogadoEscritorioDTO>> ConsultarAdvogadoEscritorio(bool ehEscritorio, string codUsuario);

        Task<IEnumerable<AdvogadoEscritorioDTO>> RecuperarAdvogadoEscritorioPorCodigoProfissional(long codigoEscritorio);
    }
}
