using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.DTO;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Enum;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.ControleDeAcesso.Repository
{
    public class PermissaoRepository : BaseCrudRepository<Permissao, string>, IPermissaoRepository
    {
        private UsuarioGrupoRepository _usuarioGrupoRepository;
        private UsuarioRepository _usuarioRepository;
        private IAuthenticatedUser _user;
        public PermissaoRepository(JuridicoContext context, IAuthenticatedUser user) : base(context, user)
        {
            _usuarioGrupoRepository = new UsuarioGrupoRepository(context, user);
            _usuarioRepository = new UsuarioRepository(context, user);
            _user = user;
        }

        public bool TemPermissao(string usuario, PermissaoEnum permissao)
        {
            var query = context.Set<Permissao>() as IQueryable<Permissao>;

            var GruposUsuario = _usuarioGrupoRepository.ObterGruposUsuario(usuario);

            query = query.Where(x => x.Menu.ToUpper() == permissao.ToString().ToUpper() && GruposUsuario.Contains(x.GrupoUsuario.ToUpper())).Distinct();
            return query.Count() > 0;
        }

        public bool TemPermissao(string usuario, string permissao)
        {
            var query = context.Set<Permissao>() as IQueryable<Permissao>;

            var GruposUsuario = _usuarioGrupoRepository.ObterGruposUsuario(usuario);

            query = query.Where(x => x.Menu.ToUpper() == permissao.ToUpper() && GruposUsuario.Contains(x.GrupoUsuario.ToUpper())).Distinct();
            return query.Count() > 0;
        }

        public async Task<IList<string>> PermissoesModulo(string usuario)
        {
            var query = context.Set<Permissao>() as IQueryable<Permissao>;

            var GruposUsuario = await _usuarioRepository.ValidarPerfilWeb(_usuarioGrupoRepository.ObterGruposUsuario(usuario));

            var retorno = query.Where(x => GruposUsuario.Contains(x.GrupoUsuario.ToUpper())).Select(p => p.Menu).Distinct();

            return retorno.ToList();
        }
        public async Task<IList<string>> PermissoesUsuarioLogado()
        {
            base.context.Database.ExecuteSqlCommand(string.Format("call jur.SP_ACA_INSERE_LOG_USUARIO('{0}', 'S')", _user.Login));

            var retorno = base.context.Query<PermissaoDTO>().FromSql(@"select 
                                                                                Cod_Aplicacao as Aplicacao,
                                                                                Cod_Janela as Janela,
                                                                                Cod_Menu as Menu,
                                                                                Cod_usuario as CodigoUsuario
                                                                            from JUR.v_permissoes_acesso_web;").ToList();

            return retorno.Select(x => x.Menu).ToList();
        }

        public void RemoverPermissoes(IList<Permissao> permissoes)
        {
            context.Set<Permissao>().RemoveRange(permissoes);
            SaveChanges();
        }

        public IList<Permissao> ObterPermissoes(string codGrupo, string codJanela, string codMenu)
        {

            var permissao = context.Set<Permissao>().Where(x => x.Aplicacao == "JUR"
                            && x.GrupoUsuario == codGrupo
                            //&& x.Janela == codJanela
                            && x.Menu == codMenu).ToList();

            return permissao;
        }

        public void CriarPermissoes(IList<Permissao> permissao)
        {
            context.Set<Permissao>().AddRange(permissao);
            SaveChanges();
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new System.NotImplementedException();
        }
        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}