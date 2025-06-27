using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Repository
{
    public interface ITokenSegurancaRepository : IBaseCrudRepository<TokenSeguranca, decimal>
    {
        Task<TokenSeguranca> ObterTokenDoUsuarioComChave(string codigoDoUsuario, string token);
        Task<IList<TokenSeguranca>> ObterTokensDoUsuario(string codigoDoUsuario);
        void Excluir(TokenSeguranca tokenDeSeguranca);
    }
}
