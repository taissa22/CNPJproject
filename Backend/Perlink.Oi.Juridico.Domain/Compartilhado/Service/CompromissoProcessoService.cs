using Shared.Domain.Impl.Service;
using System.Threading.Tasks;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class CompromissoProcessoService : BaseCrudService<CompromissoProcesso, long>, ICompromissoProcessoService
    {
        private readonly ICompromissoProcessoRepository repository;

        public CompromissoProcessoService(ICompromissoProcessoRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task AtualizarValorCompromisso(long codigoProcesso, long codigoCompromisso) {
            await repository.AtualizarCompromisso(codigoProcesso, codigoCompromisso);
        }
    }
}
