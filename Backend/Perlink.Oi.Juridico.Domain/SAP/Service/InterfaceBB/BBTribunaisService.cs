using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service.InterfaceBB;
using Shared.Domain.Impl.Service;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Service.InterfaceBB
{
    public class BBTribunaisService : BaseCrudService<BBTribunais, long>, IBBTribunaisService
    {
        private readonly IBBTribunaisRepository repository;

        public BBTribunaisService(IBBTribunaisRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task<ICollection<BBTribunais>> ObterPorFiltroPaginado(FiltrosDTO filtros) {
            return await repository.ObterPorFiltroPaginado(filtros);
        }

        public async Task<int> ObterQuantidadeTotalPorFiltro(FiltrosDTO filtros) {
            return await repository.ObterQuantidadeTotalPorFiltro(filtros);
        }

        public async Task<ICollection<BBTribunais>> RecuperarTodosEmOrdemAlfabetica() {
            var lista = await RecuperarTodos();
            return lista.OrderBy(t => t.Descricao).ToList();
        }

        public Task<bool> VerificarDuplicidadeTribunalnalBB(BBTribunais entidade)
        {
            return repository.VerificarDuplicidadeTribunalnalBB(entidade);
        }
    }
}