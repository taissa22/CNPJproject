using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.ControleDeAcesso.Repository
{
    public class TokenSegurancaRepository : BaseCrudRepository<TokenSeguranca, decimal>, ITokenSegurancaRepository
    {

        public TokenSegurancaRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {

        }

        public async Task<TokenSeguranca> ObterTokenDoUsuarioComChave(string codigoDoUsuario, string token)
        {
            var tokenMaisRecente = context.Set<TokenSeguranca>()
                .Where(a => a.CodigoUsuario.Equals(codigoDoUsuario));

        #if !DEBUG
            tokenMaisRecente.Where(a => a.Token.Equals(token));
        #endif

            tokenMaisRecente.OrderByDescending(a => a.DataDeCriacao);

            return await Task.FromResult(tokenMaisRecente.FirstOrDefault());
        }

        public async Task<IList<TokenSeguranca>> ObterTokensDoUsuario(string codigoDoUsuario) {
            var tokens = context.Set<TokenSeguranca>()
                .Where(a => a.CodigoUsuario.Equals(codigoDoUsuario))
                .ToList();

            return await Task.FromResult(tokens);
        }

        public void Excluir(TokenSeguranca tokenDeSeguranca) {
            context.Set<TokenSeguranca>().Remove(tokenDeSeguranca);
        }

        public override async Task<IDictionary<string, string>> RecuperarDropDown()
        {
            return await context.Set<Usuario>()
                    .AsNoTracking()
                    .OrderBy(x => x.Nome)
                    .ToDictionaryAsync(x => x.Id, x => x.Nome);
        }
    }
}