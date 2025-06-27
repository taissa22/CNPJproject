using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Processos;
using Shared.Domain.Interface.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Service.Processos
{
    public interface IPrepostoService : IBaseCrudService<Preposto, long>
    {
        Task<IEnumerable<PrepostoDTO>> ConsultarPreposto();
        Task<IEnumerable<PrepostoDTO>> ListarPreposto(long? tipoProcesso);
    }
}
