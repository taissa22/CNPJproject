using System;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository
{
    [Obsolete]
    public interface ISequencialRepository
    {
        Task AtualizarSequence(string codTabela);

        Task<long> RecuperarSequencialAsync(string codTabela);
    }
}
