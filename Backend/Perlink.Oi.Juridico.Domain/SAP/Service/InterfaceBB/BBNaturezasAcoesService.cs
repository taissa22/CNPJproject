using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service.InterfaceBB;
using Shared.Domain.Impl.Service;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Service.InterfaceBB
{
    public class BBNaturezasAcoesService : BaseCrudService<BBNaturezasAcoes, long>, IBBNaturezasAcoesService
    {
        private readonly IBBNaturezasAcoesRepository repository;

        public BBNaturezasAcoesService(IBBNaturezasAcoesRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task<bool> CodigoBBJaExiste(BBNaturezasAcoes obj)
        {
            return await repository.CodigoBBJaExiste(obj);
        }
    }
}