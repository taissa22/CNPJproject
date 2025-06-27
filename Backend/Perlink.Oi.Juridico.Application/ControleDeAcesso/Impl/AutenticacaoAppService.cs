using Microsoft.Extensions.Caching.Distributed;
using Perlink.Oi.Juridico.Application.ControleDeAcesso.Interface;
using Perlink.Oi.Juridico.Application.ControleDeAcesso.ViewModel;
using Perlink.Oi.Juridico.Application.Security;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Enum;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Repository;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.ControleDeAcesso.Impl
{
    public class AutenticacaoAppService : NakedBaseAppService, IAutenticacaoAppService
    {

        private readonly ITokenSegurancaRepository repository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJwtService _jwtService;
        private readonly IAuthenticatedUser _user;
        private readonly IPermissaoRepository permissaoRepository;
        private readonly IParametroRepository parametroRepository;
        private readonly IDistributedCache _cache;

        public AutenticacaoAppService(ITokenSegurancaRepository repository, IUsuarioRepository usuarioRepository, IJwtService jwtService, IAuthenticatedUser user, IPermissaoRepository permissaoRepository, IParametroRepository parametroRepository, IDistributedCache cache)
        {
            this.repository = repository;
            this._usuarioRepository = usuarioRepository;
            this._jwtService = jwtService;
            this._user = user;
            this.permissaoRepository = permissaoRepository;
            this.parametroRepository = parametroRepository;
            this._cache = cache;

        }

        public async Task<IResultadoApplication<string>> ObterNomeDeUsuarioLogado()
        {
            var resultado = new ResultadoApplication<string>();

            try
            {
                var usuario = await this._usuarioRepository.RecuperarPorLogin(this._user.Login);
                resultado.DefinirData(usuario.Nome);
                resultado.ExecutadoComSuccesso();
            }
            catch (Exception exececao)
            {
                resultado.ExecutadoComErro(exececao);
            }

            return resultado;
        }

        public async Task<IResultadoApplication<object>> ObterDadosUsuarioLogado()
        {

            var result = new ResultadoApplication<object>();

            var usuario = await this._usuarioRepository.RecuperarPorLogin(this._user.Login);

            if (usuario == null)
            {
                result.ExecutadoComErro();
            }
            else
            {
                result.DefinirData(new
                {
                    Username = usuario.Id,
                    Nome = usuario.Nome,
                    permissoes = permissaoRepository.PermissoesUsuarioLogado().Result,
                    Ambiente = parametroRepository.RecuperarPorNome("NOME_BANCO").Conteudo,
                    usuario.EhEscritorio
                });
                result.ExecutadoComSuccesso();
            }

            return result;
        }

        public async Task<IResultadoApplication<object>> Autenticar(LoginViewModel viewModel)
        {

            var result = new ResultadoApplication<object>();
            try
            {
                bool credenciaisValidas = false;
                Usuario usuario = null;

                if (viewModel.GrantType.Equals("password"))
                {
                    usuario = await HandleUserAuthentication(viewModel);
                    credenciaisValidas = usuario != null;
                }
                else if (viewModel.GrantType == "refresh_token")
                {
                    if (!String.IsNullOrWhiteSpace(viewModel.RefreshToken))
                    {
                        RefreshToken? refreshTokenBase = null;

                        string strTokenArmazenado =
                           _cache.GetString(viewModel.RefreshToken);
                        if (!String.IsNullOrWhiteSpace(strTokenArmazenado))
                        {
                            refreshTokenBase = JsonSerializer
                                .Deserialize<RefreshToken>(strTokenArmazenado);
                        }

                        credenciaisValidas = (refreshTokenBase != null &&
                            viewModel.Username == refreshTokenBase.Username &&
                            viewModel.RefreshToken == refreshTokenBase.Token);

                        usuario = await HandleUserRefreshAuthentication(viewModel);

                        if (usuario == null)
                        {
                            result.ExecutadoComErro("Acesso negado!");
                            return result;
                        }

                        // Elimina o token de refresh já que um novo será gerado
                        if (credenciaisValidas && usuario != null)
                            _cache.Remove(viewModel.RefreshToken);
                    }
                }

                if (usuario == null)
                {
                    result.ExecutadoComErro("Acesso negado!");
                    return result;
                }

                if (credenciaisValidas)
                {
                    var jwt = this._jwtService.CreateJsonWebToken(new User(usuario.Nome, usuario.Id, String.Empty));
                    //await _refreshTokenRepository.Save(jwt.RefreshToken);

                    result.DefinirData(new
                    {
                        access_token = jwt.AccessToken,
                        refresh_token = jwt.RefreshToken.Token,
                        token_type = jwt.TokenType,
                        expires_in = jwt.ExpiresIn
                    });
                } else {
                    result.ExecutadoComErro("Acesso negado!");
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }
            return result;
        }

        private async Task<Usuario> HandleUserAuthentication(LoginViewModel viewModel)
        {
            Usuario usuario = null;

            Criptografador criptografador = new Criptografador(viewModel.Password);
            var token = await this.repository.ObterTokenDoUsuarioComChave(viewModel.Username, criptografador.GerarToken(viewModel.Username));
            Boolean valido = false;
            #if DEBUG
                        valido = true;
            #else
                        valido = token != null && token.DataDeCriacao <= DateTime.Now.AddMinutes(5);
            #endif

            ExcluirTokensDoUsuario(viewModel.Username);

            if (!valido)
            {
                return null;
            }

            usuario = await this._usuarioRepository.RecuperarPorLogin(viewModel.Username);
            return usuario;
        }

        private async Task<Usuario> HandleUserRefreshAuthentication(LoginViewModel viewModel)
        {        
            Usuario usuario = await this._usuarioRepository.RecuperarPorLogin(viewModel.Username);
            return usuario;
        }

        private async void ExcluirTokensDoUsuario(String codigoDoUsuario) {
            foreach (var tokenDeSeguranca in await this.repository.ObterTokensDoUsuario(codigoDoUsuario)) {
                this.repository.Excluir(tokenDeSeguranca);
            }
        }

        private List<PermissaoEnum> ObterPermissoes()
        {
            //TODO: Solução alternativa, remover comentários acima caso funcione.
            // Pega os valores do Enum "PermissaoEnum" e cria uma lista com seus valores
            return Enum.GetValues(typeof(PermissaoEnum))
                       .Cast<PermissaoEnum>()
                       .ToList();
        }
    }
}
