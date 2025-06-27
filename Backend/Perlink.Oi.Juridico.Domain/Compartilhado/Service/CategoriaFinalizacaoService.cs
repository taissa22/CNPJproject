using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Shared.Domain.Impl.Service;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class CategoriaFinalizacaoService : BaseCrudService<CategoriaFinalizacao, long>, ICategoriaFinalizacaoService
    {
        private readonly ICategoriaFinalizacaoRepository repository;

        public CategoriaFinalizacaoService(ICategoriaFinalizacaoRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task<bool> ExisteCategoriaFinalizacao(long codCategoriaPagamento)
        {
            return await repository.ExisteCategoriaFinalizacao(codCategoriaPagamento);
        }
    }
}