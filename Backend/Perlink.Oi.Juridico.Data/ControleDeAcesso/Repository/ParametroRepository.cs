using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.ControleDeAcesso.Repository
{
    public class ParametroRepository : BaseCrudRepository<Parametro, string>, IParametroRepository
    {
        public ParametroRepository(JuridicoContext context, IAuthenticatedUser user) : base(context, user)
        {
        }

        public override async Task<IDictionary<string, string>> RecuperarDropDown()
        {
            return await context.Set<Parametro>()
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .ToDictionaryAsync(x => x.Id, x => x.Id);
        }

        public Parametro RecuperarPorNome(string codigoDoParametro)
        {
            return context.Set<Parametro>()
                .SingleOrDefault(x => x.Id == codigoDoParametro);
        }

        public string RecuperarConteudoParametroJuridico(string nomeparamentro)
        {         
                 return context.Set<Parametro>()
                .Where(x => x.Id == nomeparamentro)
                .Select(p => p.Conteudo).FirstOrDefault();
        }
        public async Task<List<string>> validarparametro()
        {
           
            return await context.Set<Parametro>()
               .AsNoTracking()
               .Select(l => l.Conteudo)
               .ToListAsync();
        }
      
    }
}