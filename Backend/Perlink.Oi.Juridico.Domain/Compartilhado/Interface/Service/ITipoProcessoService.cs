using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service
{
    public interface ITipoProcessoService : IBaseCrudService<TipoProcesso, long> {
        Task<IEnumerable<TipoProcesso>> RecuperarTodosConsultaLote();

        Task<IEnumerable<TipoProcesso>> RecuperarTodosCriaLote();

        Task<IEnumerable<TipoProcesso>> RecuperarTodosEstornaLancamento();

        Task<IEnumerable<TipoProcesso>> RecuperarParaConsultaSaldoDeGarantia();

        Task<IEnumerable<TipoProcesso>> RecuperarTodosManutencaoCategoriaPagamento();

        Task<IEnumerable<TipoProcesso>> RecuperarTodosManutencaoVigenciaCivil();

        Task<IEnumerable<TipoProcesso>> RecuperarTodosManutencaoTipoAudiencia();
    }
}
