using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Shared.Domain.Impl.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class EmpresaDoGrupoService : BaseCrudService<Parte, long>, IEmpresaDoGrupoService
    {
        private readonly IEmpresaDoGrupoRepository EmpresaDoGrupoRepository;

        public EmpresaDoGrupoService(IEmpresaDoGrupoRepository EmpresaDoGrupoRepository) : base(EmpresaDoGrupoRepository)
        {
            this.EmpresaDoGrupoRepository = EmpresaDoGrupoRepository;
        }

        public async Task<bool> ExisteEmpresaDoGrupoComFornecedor(long codigoFornecedor) {
            return await EmpresaDoGrupoRepository.ExisteEmpresaDoGrupoComFornecedor(codigoFornecedor);
        }

        public async Task<ICollection<EmpresaDoGrupoDTO>> RecuperarEmpresaDoGrupo()
        {
            return await EmpresaDoGrupoRepository.RecuperarEmpresaDoGrupo();
        }
    }
}
