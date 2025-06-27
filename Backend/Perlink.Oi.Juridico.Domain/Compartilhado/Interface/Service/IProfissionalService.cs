using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service
{
    public interface IProfissionalService : IBaseCrudService<Profissional, long> {
        Task<IEnumerable<Profissional>> RecuperarTodosProfissionais();

        Task<IEnumerable<Profissional>> RecuperarTodosEscritorios();
        Task<bool> ExisteGrupoLoteJuizadoComEscritorio(long codigoGrupoLoteJuizado);
    }
}
