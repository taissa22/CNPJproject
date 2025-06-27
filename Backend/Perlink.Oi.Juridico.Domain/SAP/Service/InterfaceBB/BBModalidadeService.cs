using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service.InterfaceBB;
using Shared.Domain.Impl.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Service.InterfaceBB
{
    public class BBModalidadeService : BaseCrudService<BBModalidade, long>, IBBModalidadeService
    {
        private readonly IBBModalidadeRepository repository;
        public BBModalidadeService(IBBModalidadeRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task<bool> CodigoBBJaExiste(BBModalidade obj)
        {
            return await repository.CodigoBBJaExiste(obj);
        }

        public async Task<IEnumerable<BBModalidade>> recuperarModalidadeBB(BBModalidadeFiltroDTO filtro)
        {
            return await repository.recuperarModalidadeBB(filtro);
        }

       public async Task<ICollection<BBModalidade>> exportarModalidadeBB(BBModalidadeFiltroDTO filtro)
        {
            return await repository.exportarModalidadeBB(filtro);
        }
        public async Task<int> ObterQuantidadeTotalPorFiltro(BBModalidadeFiltroDTO filtroDTO)
        {
            return await repository.ObterQuantidadeTotalPorFiltro(filtroDTO);
        }
    }
}
