using Perlink.Oi.Juridico.Domain.AlteracaoBloco.DTO;
using Perlink.Oi.Juridico.Domain.AlteracaoBloco.Entity;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.AlteracaoBloco.Interface.Repository
{
    public interface IAlteracaoEmBlocoRepository : IBaseCrudRepository<AlteracaoEmBloco, long>
    {
        Task<IEnumerable<AlteracaoEmBlocoRetornoDTO>> ListarAgendamentos(int index, int count);
        Task<IEnumerable<AlteracaoEmBlocoRetornoDTO>> ListarAgendamentosPorUsuario(int index, int count, string usuario);
        Task<int> QuantidadeTotal();
        Task<int> QuantidadeTotalPorUsuario(string usuario);
        Task<IEnumerable<AlteracaoEmBloco>> ExpurgoAlteracaoEmBloco(int parametro);
        Task<IEnumerable<AlteracaoEmBloco>> ListarAgendamentosComStatusAgendado();
    }
}
