using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Enum;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
using System;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.Service {
    public class PermissaoAttribute : Attribute {
        private readonly IPermissaoService permissaoService;
        private readonly PermissaoEnum civelEstrategicoParametros;

        public PermissaoAttribute(IPermissaoService permissaoService, PermissaoEnum civelEstrategicoParametros) {
            this.permissaoService = permissaoService;
            this.civelEstrategicoParametros = civelEstrategicoParametros;
        }

        public PermissaoAttribute(PermissaoEnum civelEstrategicoParametros) {
            this.civelEstrategicoParametros = civelEstrategicoParametros;
        }

        public bool TemPermissao(PermissaoEnum permissao) {
            return permissaoService.TemPermissao(permissao);

        }
    }
}