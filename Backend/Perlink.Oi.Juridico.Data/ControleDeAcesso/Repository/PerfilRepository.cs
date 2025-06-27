using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.DTO;
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
    public class PerfilRepository : BaseCrudRepository<Usuario, string>, IPerfilRepository
    {
        private UsuarioRepository usuarioRepository;
        public PerfilRepository(JuridicoContext dbcontext, IAuthenticatedUser user) : base(dbcontext, user)
        {
            usuarioRepository = new UsuarioRepository(dbcontext, user);
        }

        public async Task<PerfilDTO> ObterDetalhePerfil(string codigoPerfil)
        {
            var perfil = await (from p in context.Set<Usuario>()
                                where p.Id == codigoPerfil
                                && p.EhPerfil
                                select new PerfilDTO()
                                {
                                    NomeId = p.Id,
                                    Descricao = p.Nome,
                                    GestorDefaultId = (p.GestorDefault == null) ? string.Empty : p.GestorDefault.Id,
                                    GestorDefault = (p.GestorDefault == null) ? string.Empty : p.GestorDefault.Nome,
                                    TipoUsuario = p.TipoPerfil,
                                    Restrito = p.Restrito,
                                    Ativo = p.Ativo,
                                    PerfilWeb = p.EhPerfilWeb
                                }).FirstOrDefaultAsync();

            ObterPermissoes(perfil, codigoPerfil);
            perfil.UsuariosPerfil = new List<UsuariosPerfilDTO>();
            //ObterUsuarios(perfil, codigoPerfil);
            ObterCount(perfil);

            return perfil;
        }

        public async Task<PerfilDTO> CriarNovoUsuarioPerfil()
        {
            var permissoes = this.ObterTodasPermissoesDisponiveis();
            var usuarios = this.ObterTodosUsuariosDisponiveis();
            var perfilDTO = new PerfilDTO()
            {
                Ativo = true,
                TipoUsuario = "1",
                PerfilWeb = true,
                Restrito = false,
                UsuariosPerfil = usuarios,
                Permissoes = permissoes,
                Descricao = "",
                NomeId = "",
                GestorDefault = "",
                GestorDefaultId = ""
            };

            ObterCount(perfilDTO);

            return perfilDTO;
        }

        public async Task<List<PerfilDTO>> ObterGestoresAprovadoresAtivos()
        {
            var perfil = await (from p in context.Set<Usuario>()
                                where p.Ativo && p.EhGestorAprovador
                                orderby p.Nome ascending
                                select new PerfilDTO()
                                {
                                    NomeId = p.Id,
                                    Descricao = p.Nome
                                }).ToListAsync();

            return perfil;
        }

        public async Task<IList<UsuariosPerfilDTO>> ObterUsuariosAssociadosAoPerfil(string id)
        {
            var usuariosDoGrupo = (from r in context.Set<UsuarioGrupo>()
                                   where r.GrupoUsuario.Equals(id)
                                   select new UsuariosPerfilDTO() { Codigo = r.UsuarioId })
                                   .ToList();


            return await context.Set<Usuario>()
                          .Where(x => usuariosDoGrupo.Select(y => y.Codigo)
                          .Contains(x.Id)
                          && x.EhPerfil == false )
                          .Select(u => new UsuariosPerfilDTO()
                          {
                              Ativo = u.Ativo,
                              Codigo = u.Id,
                              Nome = u.Nome,
                              Email = u.Email,
                              DataUltimoAcesso = u.DataUltimoAcesso
                          })
                          .ToListAsync();
        }

        public async Task<IList<PerfilPermissaoDTO>> ObterPermissoesAssociadasAoPerfil(string id)
        {
            return await (from p in context.Set<Permissao>()
                          join m in context.Set<Menu>() on p.Menu equals m.Id
                          where p.GrupoUsuario.Equals(id)
                          select new PerfilPermissaoDTO()
                          {
                              Codigo = p.GrupoUsuario,
                              Descricao = m.DescricaoMenu,
                              Janela = p.Janela,
                              CodigoMenu = m.Id,
                              Caminho = m.CaminhoMenu
                          }).OrderBy(x => x.Descricao)
                   .ToListAsync();
        }


        private void ObterPermissoes(PerfilDTO perfil, string codPerfil)
        {
            if (perfil.PerfilWeb)
            {
                perfil.Permissoes = (from m in context.Set<Menu>()
                                     join p in context.Set<Permissao>()
                                     .Where(x => x.GrupoUsuario == codPerfil).GroupBy(x => x.Menu).Select(x => x.FirstOrDefault()) on m.Id
                                     equals p.Menu into leftJoinPermissao
                                     from p in leftJoinPermissao.DefaultIfEmpty()
                                     where (m.TipoMenu == "MWEB" || m.TipoMenu == "FWEB")
                                     select new PerfilPermissaoDTO()
                                     {
                                         Codigo = p.Id,
                                         Descricao = m.DescricaoMenu,
                                         Janela = p.Janela,
                                         CodigoMenu = m.Id,
                                         Caminho = m.CaminhoMenu,
                                         Associado = p.GrupoUsuario == codPerfil
                                     }).OrderBy(x => x.Descricao).ToList();
            }
            else
            {
                perfil.Permissoes = (from m in context.Set<Menu>()
                                     join p in context.Set<Permissao>()
                                     .Where(x => x.GrupoUsuario == codPerfil).GroupBy(x => x.Menu).Select(x => x.FirstOrDefault()) on m.Id
                                     equals p.Menu into leftJoinPermissao
                                     from p in leftJoinPermissao.DefaultIfEmpty()
                                     where (m.TipoMenu == "MDI" || m.TipoMenu == "MAIN" || m.TipoMenu == "ANC")
                                     select new PerfilPermissaoDTO()
                                     {
                                         Codigo = p.GrupoUsuario,
                                         Descricao = m.DescricaoMenu,
                                         Janela = p.Janela,
                                         CodigoMenu = m.Id,
                                         Caminho = m.CaminhoMenu,
                                         Associado = p.GrupoUsuario == codPerfil
                                     }).OrderBy(x => x.Descricao).ToList();
            }
        }


        private void ObterUsuarios(PerfilDTO perfil, string codPerfil)
        {
            var usuariosPerfil = (from r in context.Set<Usuario>()
                                  where r.Ativo && !r.EhPerfil && !r.EhPerfilWeb
                                  select new UsuariosPerfilDTO() { Codigo = r.Id, Nome = $"{r.Nome} - {r.Id}", Email = r.Email })
                                  .OrderBy(x => x.Nome).ToList();

            var grupo = (from r in context.Set<UsuarioGrupo>()
                         where r.GrupoUsuario.Equals(codPerfil)
                         select new UsuariosPerfilDTO() { Codigo = r.UsuarioId }).ToList();

            var usuarioAssociado = usuariosPerfil.Where(x => grupo.Select(y => y.Codigo).Contains(x.Codigo)).ToList();

            usuariosPerfil = usuariosPerfil.Select(x => new UsuariosPerfilDTO()
            {
                Codigo = x.Codigo,
                Nome = x.Nome,
                Email = x.Email,
                Associado = usuarioAssociado.Any(y => y.Codigo == x.Codigo),
                Ativo = x.Ativo
            }).OrderBy(x => x.Nome).ToList();

            perfil.UsuariosPerfil = usuariosPerfil;
        }

        private void ObterCount(PerfilDTO perfilDTO)
        {
            perfilDTO.QuantidadeAssociadaPermissao = perfilDTO.Permissoes.Where(x => x.Associado).Count();
            perfilDTO.QuantidadeDisponivelPermissao = perfilDTO.Permissoes.Where(x => !x.Associado).Count();

            perfilDTO.QuantidadeAssociadaUsuario = 0; // perfilDTO.UsuariosPerfil.Where(x => x.Associado).Count();
            perfilDTO.QuantidadeDisponivelUsuario = 0; // perfilDTO.UsuariosPerfil.Where(x => !x.Associado).Count();
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new System.NotImplementedException();
        }

        public IList<UsuariosPerfilDTO> ObterTodosUsuariosDisponiveis()
        {
            try
            {
                return (from u in context.Set<Usuario>()
                        where u.Ativo && !u.EhPerfil
                        select new UsuariosPerfilDTO()
                        {
                            Codigo = u.Id,
                            Nome = $"{u.Nome} - {u.Id}"
                        }).OrderBy(user => user.Nome).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public IList<PerfilPermissaoDTO> ObterTodasPermissoesDisponiveis()
        {
            try
            {
                return (from m in context.Set<Menu>()
                        join p in context.Set<Permissao>()
                        .GroupBy(x => x.Menu).Select(x => x.FirstOrDefault()) on m.Id
                        equals p.Menu into leftJoinPermissao
                        from p in leftJoinPermissao.DefaultIfEmpty()
                        select new PerfilPermissaoDTO()
                        {
                            Codigo = p.GrupoUsuario,
                            Descricao = m.DescricaoMenu,
                            Janela = p.Janela,
                            CodigoMenu = m.Id,
                            Caminho = m.CaminhoMenu,
                            Associado = false,
                        }).OrderBy(x => x.Descricao).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool ExistePerfil(string NomePerfil)
        {
            return usuarioRepository.ExisteUsuario(NomePerfil);
        }
    }
}
