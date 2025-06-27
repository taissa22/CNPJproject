using AutoMapper;
using Perlink.Oi.Juridico.Application.ControleDeAcesso.Interface;
using Perlink.Oi.Juridico.Application.ControleDeAcesso.ViewModel;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Enum;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
using Shared.Application.Impl;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.ControleDeAcesso.Impl {
    public class PermissaoAppService : BaseAppService<PermissaoViewModel, Permissao, string>, IPermissaoAppService {
        private readonly IPermissaoService service;
        private readonly IMapper mapper;

        public PermissaoAppService(IPermissaoService service,
            IMapper mapper) : base(service, mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        public bool TemPermissao(PermissaoEnum permissao) {
            return service.TemPermissao(permissao);
        }

        public async Task<IList<string>> PermissoesModulo(List<PermissaoEnum> permissoes) {
            return await service.PermissoesModulo(permissoes);
        }
    }
}