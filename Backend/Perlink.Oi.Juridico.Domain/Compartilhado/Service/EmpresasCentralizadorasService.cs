using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Shared.Domain.Impl.Service;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class EmpresasCentralizadorasService : BaseCrudService<EmpresasCentralizadoras, long>, IEmpresasCentralizadorasService
    {
        private readonly IEmpresasCentralizadorasRepository EmpresasCentralizadorasRepository;
        public EmpresasCentralizadorasService(IEmpresasCentralizadorasRepository EmpresasCentralizadorasRepository) : base(EmpresasCentralizadorasRepository)
        {
            this.EmpresasCentralizadorasRepository = EmpresasCentralizadorasRepository;
        }
    }
}
