using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.ControleDeAcesso.Repository
{
    public class JanelaRepository : BaseCrudRepository<Janela, string>, IJanelaRepository
    {
        public JanelaRepository(JuridicoContext context, IAuthenticatedUser user) : base(context, user)
        { }

        public List<string> FindJanelas(string codMenu)
        {
            try
            {
                var query = context.Set<Janela>() as IQueryable<Janela>;
                return query.Where(janela => janela.CodMenu == codMenu).Select(j => j.CodJanela).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }
    }
}
