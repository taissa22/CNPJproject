using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Enum;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service
{
    public interface IPermissaoService : IBaseCrudService<Permissao, string>
    {
        bool TemPermissao(PermissaoEnum permissao);
        bool TemPermissao(string permissao);
        Task<IList<string>> PermissoesModulo(List<PermissaoEnum> permissoes);
    }
}
