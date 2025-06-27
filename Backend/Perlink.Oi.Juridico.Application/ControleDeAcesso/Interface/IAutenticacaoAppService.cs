using Perlink.Oi.Juridico.Application.ControleDeAcesso.ViewModel;
using Shared.Application.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.ControleDeAcesso.Interface {
    public interface IAutenticacaoAppService : INakedBaseAppService {
        Task<IResultadoApplication<object>> Autenticar(LoginViewModel viewModel);
        Task<IResultadoApplication<object>> ObterDadosUsuarioLogado();
        Task<IResultadoApplication<string>> ObterNomeDeUsuarioLogado();
    }
}
