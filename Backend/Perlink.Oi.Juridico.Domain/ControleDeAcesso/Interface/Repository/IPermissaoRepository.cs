using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Enum;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Repository
{
    public interface IPermissaoRepository : IBaseCrudRepository<Permissao, string>
    {
        bool TemPermissao(string usuario, PermissaoEnum permissao);
        bool TemPermissao(string usuario, string permissao);
        Task<IList<string>> PermissoesModulo(string usuario); 
        Task<IList<string>> PermissoesUsuarioLogado();

        void RemoverPermissoes(IList<Permissao> permissao);
        IList<Permissao> ObterPermissoes(string codGrupo, string codJanela, string codMenu);
        void CriarPermissoes(IList<Permissao> permissao);
        void SaveChanges();


    }
}
