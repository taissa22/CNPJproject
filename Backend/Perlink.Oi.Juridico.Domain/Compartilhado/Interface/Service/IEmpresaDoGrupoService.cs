using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service
{
    public  interface IEmpresaDoGrupoService : IBaseCrudService<Parte, long>
    {
        Task<ICollection<EmpresaDoGrupoDTO>> RecuperarEmpresaDoGrupo();
        Task<bool> ExisteEmpresaDoGrupoComFornecedor(long codigoFornecedor);
    }
}
