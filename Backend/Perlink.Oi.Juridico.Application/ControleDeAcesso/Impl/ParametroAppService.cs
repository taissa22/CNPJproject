using System.Collections.Generic;
using AutoMapper;
using Perlink.Oi.Juridico.Application.ControleDeAcesso.Interface;
using Perlink.Oi.Juridico.Application.ControleDeAcesso.ViewModel;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
using Shared.Application.Impl;
using Shared.Application.Interface;

namespace Perlink.Oi.Juridico.Application.ControleDeAcesso.Impl {
    public class ParametroAppService : BaseCrudAppService<ParametroViewModel, Parametro, string>, IParametroAppService {
        private readonly IParametroService service;
        private readonly IMapper mapper;

        public ParametroAppService(IParametroService service,
            IMapper mapper) : base(service, mapper) {
            this.service = service;
            this.mapper = mapper;
        }

        IResultadoApplication<ParametroViewModel> IParametroAppService.RecuperarPorNome(string codigoDoParametro) {
            var result = new ResultadoApplication<ParametroViewModel>();

            try {
                result.DefinirData(mapper.Map<ParametroViewModel>(service.RecuperarPorNome(codigoDoParametro)));
                result.ExecutadoComSuccesso();
            } catch (System.Exception ex) {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public IResultadoApplication<IList<ParametroViewModel>> CarregarConfiguracao() {
            var result = new ResultadoApplication<IList<ParametroViewModel>>();

            try {
                result.DefinirData(mapper.Map<IList<ParametroViewModel>>(service.CarregarConfiguracao()));
                result.ExecutadoComSuccesso();
            } catch (System.Exception excecao) {
                result.ExecutadoComErro(excecao);
            }

            return result;
        }
    }
}