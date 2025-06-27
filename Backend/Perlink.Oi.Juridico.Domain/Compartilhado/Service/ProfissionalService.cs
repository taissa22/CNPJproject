using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Shared.Domain.Impl.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class ProfissionalService : BaseCrudService<Profissional, long>, IProfissionalService
    {
        private readonly IProfissionalRepository repository;

        public ProfissionalService(IProfissionalRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public Task<IEnumerable<Profissional>> RecuperarTodosEscritorios()
        {
            return repository.RecuperarTodosEscritorios();
        }

        public Task<IEnumerable<Profissional>> RecuperarTodosProfissionais()
        {
            return repository.RecuperarTodosProfissionais();
        }

        public async Task<bool> ExisteGrupoLoteJuizadoComEscritorio(long codigoGrupoLoteJuizado)
        {
            return await repository.ExisteGrupoLoteJuizadoComEscritorio(codigoGrupoLoteJuizado);
        }
    }
}