using AutoMapper;
using Perlink.Oi.Juridico.Application.ControleDeAcesso.Interface;
using Perlink.Oi.Juridico.Application.ControleDeAcesso.ViewModel;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
using Shared.Application.Impl;
using Shared.Application.Interface;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.ControleDeAcesso.Impl {
    public class UsuarioAppService : BaseCrudAppService<UsuarioViewModel, Usuario, string>, IUsuarioAppService {
        private readonly IUsuarioService service;
        private readonly IMapper mapper;

        public UsuarioAppService(IUsuarioService service, IMapper mapper) : base(service, mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public bool ExisteUsuario(string login) {
            return service.ExisteUsuario(login);
        }

        public async Task<IResultadoApplication<object>> ObterTokenDoUsuarioComChave(LoginViewModel viewModel) {
            var result = new ResultadoApplication<object>();

            try {
                //var data2 = await service.ObterTokenDoUsuarioChave(viewModel.Username, viewModel.RefreshToken);
                var data = await service.RecuperarPorLogin(viewModel.Username);
                result.DefinirData(mapper.Map<object>(data));
                result.ExecutadoComSuccesso();
            } catch (System.Exception ex) {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public string ObterLoginDoUsuarioLogado() {
            return service.ObterLoginDoUsuarioLogado();
        }
    }
}