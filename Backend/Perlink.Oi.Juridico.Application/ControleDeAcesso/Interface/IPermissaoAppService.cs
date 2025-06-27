using Perlink.Oi.Juridico.Application.ControleDeAcesso.ViewModel;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Enum;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.ControleDeAcesso.Interface {
    public interface IPermissaoAppService : IBaseAppService<PermissaoViewModel, Permissao, string> {
        bool TemPermissao(PermissaoEnum permissao);
        Task<IList<string>> PermissoesModulo(List<PermissaoEnum> permissoes);
    }
}