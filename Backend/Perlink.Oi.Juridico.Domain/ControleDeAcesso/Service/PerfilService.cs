using Perlink.Oi.Juridico.Domain.ControleDeAcesso.DTO;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Repository;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
using Shared.Domain.Impl.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.Service
{
    public class PerfilService : BaseService<Usuario, string>, IPerfilService
    {
        private readonly IUsuarioGrupoRepository usuarioGrupoRepository;
        private readonly IPerfilRepository repository;
        private readonly IPermissaoRepository permissaoRepository;
        private readonly IJanelaRepository janelaRepository;
        private readonly IUsuarioRepository usuarioRepository;
        public PerfilService(IPerfilRepository repository, IUsuarioGrupoRepository usuarioGrupoRepository, IPermissaoRepository permissaoRepository, IJanelaRepository janelaRepository, IUsuarioRepository usuarioRepository) : base(repository)
        {
            this.repository = repository;
            this.usuarioGrupoRepository = usuarioGrupoRepository;
            this.permissaoRepository = permissaoRepository;
            this.janelaRepository = janelaRepository;
            this.usuarioRepository = usuarioRepository;
        }

        public async Task<PerfilDTO> ObterDetalhePerfil(string codigoPerfil)
        {
            return await repository.ObterDetalhePerfil(codigoPerfil);
        }

        public async Task<IList<PerfilDTO>> ObterGestoresAprovadoresAtivos()
        {
            return await repository.ObterGestoresAprovadoresAtivos();
        }

        public async Task<IList<PerfilPermissaoDTO>> ObterPermissoesAssociadasAoPerfil(string codigoPerfil)
        {
            return await repository.ObterPermissoesAssociadasAoPerfil(codigoPerfil);
        }

        public async Task<IList<UsuariosPerfilDTO>> ObterUsuariosAssociadasAoPerfil(string codigoPerfil)
        {
            return await repository.ObterUsuariosAssociadosAoPerfil(codigoPerfil);
        }

        public async Task<PerfilDTO> CriarNovoUsuarioPerfil()
        {
            return await repository.CriarNovoUsuarioPerfil();
        }

        public Task AtualizarPerfil(PerfilDTO perfilDTO)
        {
            perfilDTO.NomeId = perfilDTO.NomeId.ToUpper();
            try
            {
                if (perfilDTO.mudancasPerfilUsuario)
                {
                    if (perfilDTO.tipoQuery)
                        AtualizarUsuarioPerfil(perfilDTO);
                    
                    else
                        SalvarUsuarioPerfil(perfilDTO);
                }


                if (perfilDTO.mudancasUsuario)
                {
                    foreach (var usuarioPerfil in perfilDTO.UsuariosPerfil)
                    {
                        if (!usuarioPerfil.Associado)
                        {
                            var usuarioGrupoRemover = usuarioGrupoRepository.ObterUsuarioAssociadoPerfil(usuarioPerfil.Codigo, perfilDTO.NomeId.ToUpper());
                            if (usuarioGrupoRemover != null)
                            {
                                usuarioGrupoRepository.RemoverUsuarioAssociadoPerfil(usuarioGrupoRemover);
                            }
                        }
                        else
                        {
                            var _usuarioPerfil = new UsuarioGrupo()
                            {
                                GrupoUsuario = perfilDTO.NomeId,
                                Aplicacao = "JUR",
                                UsuarioId = usuarioPerfil.Codigo,
                                GrupoAplicacao = "JUR"
                            };

                            usuarioGrupoRepository.CriarUsuarioAssociadoPerfil(_usuarioPerfil);
                            usuarioGrupoRepository.SaveChanges();
                        }
                    }
                }
                if (perfilDTO.mudancasPermissoes)
                {
                    foreach (var permissao in perfilDTO.Permissoes)
                    {
                        if (!permissao.Associado)
                        {
                            IList<Permissao> permissoesDoPerfil = permissaoRepository.ObterPermissoes(permissao.Codigo.ToUpper(), permissao.Janela, permissao.CodigoMenu);
                            permissaoRepository.RemoverPermissoes(permissoesDoPerfil);
                        }
                        else
                        {
                            IList<string> janelas = janelaRepository.FindJanelas(permissao.CodigoMenu);
                            IList<Permissao> permissoes = new List<Permissao>();
                            foreach (var janela in janelas)
                            {
                                Permissao perfilPermissao = new Permissao()
                                {
                                    Aplicacao = "JUR",
                                    Menu = permissao.CodigoMenu,
                                    Janela = janela,
                                    Id = !perfilDTO.tipoQuery ? perfilDTO.NomeId : permissao.Codigo.ToUpper()
                            };
                                Permissao _permissao = Permissao.Criar(perfilPermissao);
                                permissoes.Add(_permissao);
                            }
                            this.permissaoRepository.CriarPermissoes(permissoes);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Task.CompletedTask;
        }

        private void AtualizarUsuarioPerfil(PerfilDTO perfilDTO)
        {
            try
            {
                Usuario usuario = new Usuario()
                {
                    Id = perfilDTO.NomeId,
                    Ativo = perfilDTO.Ativo,
                    Nome = perfilDTO.Descricao,
                    GestorDefaultId = perfilDTO.GestorDefaultId,
                    TipoPerfil = perfilDTO.TipoUsuario,
                    Restrito = perfilDTO.Restrito,
                    EhPerfilWeb = perfilDTO.PerfilWeb,
                    EhPerfil = true
                };
                usuarioRepository.AtualizarUsuarioPerfil(usuario);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private void SalvarUsuarioPerfil(PerfilDTO perfilDTO)
        {
            try
            {
                Usuario usuario = new Usuario()
                {
                    Id = perfilDTO.NomeId.ToUpper(),
                    Ativo = perfilDTO.Ativo,
                    Nome = perfilDTO.Descricao,
                    GestorDefaultId = perfilDTO.GestorDefaultId,
                    TipoPerfil = perfilDTO.TipoUsuario,
                    Restrito = perfilDTO.Restrito,
                    EhPerfilWeb = perfilDTO.PerfilWeb,
                    EhPerfil = true
                };
                usuarioRepository.SalvarUsuarioPerfil(usuario);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        Task IPerfilService.SalvarUsuarioPerfil(PerfilDTO perfilDTO)
        {
            throw new NotImplementedException();
        }

        public bool ExistePerfil(string NomePerfil)
        {
            return repository.ExistePerfil(NomePerfil);
        }
    }
}
