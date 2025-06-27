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
    public class UsuarioRepository : BaseCrudRepository<Usuario, string>, IUsuarioRepository
    {
        //private readonly CivelEstrategicoContext dbContext;
        public UsuarioRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            //this.dbContext = dbContext;
        }

        public bool ExisteUsuario(string login)
        {
            var query = context.Set<Usuario>() as IQueryable<Usuario>;
            query = query.Where(x => x.Id != login);
            return query.Count(x => x.Id == login) > 0;
        }


        public override async Task<IDictionary<string, string>> RecuperarDropDown()
        {
            return await context.Set<Usuario>()
                    .AsNoTracking()
                    .OrderBy(x => x.Nome)
                    .ToDictionaryAsync(x => x.Id, x => x.Nome);
        }

        public async Task<Usuario> RecuperarPorLogin(string login)
        {
            return await context.Set<Usuario>()
                .SingleOrDefaultAsync(x => x.Id == login);
        }

        public async Task<string> ObterUsuarioNome(string CodigoUsuario)
        {
            var query = await context.Set<Usuario>()
            .Where(x => x.Id == CodigoUsuario)
                .Select(x => x.Nome)
                .FirstOrDefaultAsync();

            return query;
        }

        internal async Task<IList<string>> ValidarPerfilWeb(IList<string> list)
        {

            var listaPerfilWeb = await context.Set<Usuario>()
                .Where(x => list.Contains(x.Id) && x.EhPerfilWeb).Select(x => x.Id).ToListAsync();

            var listaValida = new List<string>();
            foreach (var item in list)
            {
                if (listaPerfilWeb.Contains(item))
                    listaValida.Add(item);
            }

            return listaValida;
        }

        public void AtualizarUsuarioPerfil(Usuario usuarioPerfil)
        {
            context.Attach(usuarioPerfil);
            context.Entry(usuarioPerfil).Property("Ativo").IsModified = true;
            context.Entry(usuarioPerfil).Property("Nome").IsModified = true;
            context.Entry(usuarioPerfil).Property("GestorDefaultId").IsModified = true;
            context.Entry(usuarioPerfil).Property("TipoPerfil").IsModified = true;
            context.Entry(usuarioPerfil).Property("Restrito").IsModified = true;
            context.Entry(usuarioPerfil).Property("EhPerfilWeb").IsModified = true;
            context.SaveChanges();
        }

        public void SalvarUsuarioPerfil(Usuario usuarioPerfil)
        {
            context.Set<Usuario>().Add(usuarioPerfil);
            context.SaveChanges();
        }

        public bool ExistePerfil(string NomePerfil)
        {
            return context.Set<Usuario>().Where(p => p.Id == NomePerfil).Count() > 0;
        }
    }
}
