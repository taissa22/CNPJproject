using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository
{
    public interface IProfissionalRepository : IBaseCrudRepository<Profissional, long> {

        Task<IEnumerable<Profissional>> RecuperarTodosProfissionais();
        Task<IEnumerable<Profissional>> RecuperarTodosEscritorios();
        Task<bool> ExisteGrupoLoteJuizadoComEscritorio(long codigoGrupoLoteJuizado);
    }
}