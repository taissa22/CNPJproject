using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class SequencialService //: BaseCrudService<Sequencial, string>, ISequencialService
    {
        private readonly ISequencialRepository repository;

        public SequencialService(ISequencialRepository repository) //: base(repository)
        {
            this.repository = repository;
        }
    }
}
