using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Shared.Domain.Impl.Service;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Service
{
    public class FornecedorasFaturasService : BaseCrudService<FornecedorasFaturas, long>, IFornecedorasFaturasService
    {
        private readonly IFornecedorasFaturasRepository faturasRepository;
        public FornecedorasFaturasService(IFornecedorasFaturasRepository faturasRepository) : base(faturasRepository)
        {
            this.faturasRepository = faturasRepository;
        }

        public async Task<bool> ExisteFornecedorasFaturasComEmpresaSap(long codigoEmpresaSap)
        {
            return await faturasRepository.ExisteFornecedorasFaturasComEmpresaSap(codigoEmpresaSap);
        }
    }
}
