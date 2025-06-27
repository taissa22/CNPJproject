using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Enum;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Repository;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
using Shared.Domain.Impl.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.Service {
    public class PermissaoService : BaseCrudService<Permissao, string>, IPermissaoService {
        private readonly IPermissaoRepository repository;
        private readonly IUsuarioService usuarioService;
        public PermissaoService(IPermissaoRepository repository, IUsuarioService usuarioService) : base(repository) {
            this.repository = repository;
            this.usuarioService = usuarioService;
        }

        public bool TemPermissao(PermissaoEnum permissao) {
            var usuario = usuarioService.ObterLoginDoUsuarioLogado();
            return repository.TemPermissao(usuario, permissao);
        }


        //Esse método foi criado para poder funcionar sem necessidade de adicionar
        //coisas no enum. O método que funciona por enum deve parar de ser utilizado
        //    com o tempo
        public bool TemPermissao(string permissao)
        {
            var usuario = usuarioService.ObterLoginDoUsuarioLogado();
            return repository.TemPermissao(usuario, permissao);
        }

        public async Task<IList<string>> PermissoesModulo(List<PermissaoEnum> permissoes) {
            //var usuario = usuarioService.ObterLoginDoUsuarioLogado();
            //return await repository.PermissoesModulo(usuario);

            return await repository.PermissoesUsuarioLogado();
        }
    }
}