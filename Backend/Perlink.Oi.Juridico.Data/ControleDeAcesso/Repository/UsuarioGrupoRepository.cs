using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Data.ControleDeAcesso.Repository
{
    public class UsuarioGrupoRepository : BaseRepository<UsuarioGrupo, string>, IUsuarioGrupoRepository
    {
        public UsuarioGrupoRepository(JuridicoContext context, IAuthenticatedUser user) : base(context, user)
        {

        }

        public List<string> ObterGruposUsuario(string usuario)
        {
            var query = context.Set<UsuarioGrupo>() as IQueryable<UsuarioGrupo>;
            var retorno = query.Where(x => x.UsuarioId == usuario).Select(p => p.GrupoUsuario.ToUpper());
            return retorno.ToList();
        }

        public UsuarioGrupo ObterUsuarioAssociadoPerfil(string codigoUsuario, string codigoUsuarioDelegacao)
        {

            var retorno = (from p in context.Set<UsuarioGrupo>()
                               where p.GrupoUsuario == codigoUsuarioDelegacao
                               && p.UsuarioId == codigoUsuario
                               select new UsuarioGrupo()
                               {
                                   Aplicacao = p.Aplicacao,
                                   GrupoAplicacao = p.GrupoAplicacao,
                                   GrupoUsuario = p.GrupoUsuario,
                                   UsuarioId = p.UsuarioId
                               }).FirstOrDefault();

            return retorno;
        }

        public void CriarUsuarioAssociadoPerfil(UsuarioGrupo usuarioGrupo)
        {
            context.Add(usuarioGrupo);         
        }

        public void RemoverUsuarioAssociadoPerfil(UsuarioGrupo usuarioGrupo)
        {
            context.Entry(usuarioGrupo).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            SaveChanges();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
