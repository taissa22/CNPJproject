using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service.InterfaceBB;
using Shared.Domain.Impl.Service;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Service.InterfaceBB
{
    public class BBComarcaService : BaseCrudService<BBComarca, long>, IBBComarcaService
    {
        private readonly IBBComarcaRepository repository;
        public BBComarcaService(IBBComarcaRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task<bool> CodigoBBJaExiste(BBComarca bbComarca)
        {
            return await repository.CodigoBBJaExiste(bbComarca);
        }

        public async Task<bool> UFIsValid(string uf)
        {
            return await repository.UFIsValid(uf);
        }

        public async Task<ICollection<BBComarca>> RecuperarTodosEmOrdemAlfabetica() {
            var lista = await base.RecuperarTodos();
            return lista.OrderBy(c => c.CodigoEstado).ThenBy(c => c.Descricao).ToList();
        }

        public async Task<ICollection<BBComarca>> ConsultarBBComarca(FiltrosDTO filtroDTO)
        {
            return await repository.ConsultarBBComarca(filtroDTO);
        }

        public async Task<ICollection<BBComarca>> ExportarBBComarca(FiltrosDTO filtroDTO)
        {
            return await repository.ExportarBBComarca(filtroDTO);
        }
       public async Task<int> TotalBBComarca(FiltrosDTO filtroDTO)
        {
            return await repository.TotalBBComarca(filtroDTO);
        }
    }
}
