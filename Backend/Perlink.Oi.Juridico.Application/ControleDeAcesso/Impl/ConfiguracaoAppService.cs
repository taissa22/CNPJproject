using AutoMapper;
using Perlink.Oi.Juridico.Application.ControleDeAcesso.Interface;
using Perlink.Oi.Juridico.Application.ControleDeAcesso.ViewModel;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Domain.Impl;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.ControleDeAcesso.Impl
{
    public class ConfiguracaoAppService : BaseCrudAppService<ConfiguracaoViewModel, Parametro, string>, IConfiguracaoAppService
    {
        private readonly IParametroService parametroService;
        private readonly IPermissaoService permissaoService;
        private readonly IMapper mapper;

        public ConfiguracaoAppService(IParametroService service,
            IMapper mapper, IPermissaoService permissaoService) : base(service, mapper)
        {
            this.parametroService = service;
            this.mapper = mapper;
            this.permissaoService = permissaoService;
        }

        public IResultadoApplication<ConfiguracaoViewModel> CarregarConfiguracao()
        {
            var result = new ResultadoApplication<ConfiguracaoViewModel>();
            ConfiguracaoViewModel configuracao = null;

            try
            {
                IList<Parametro> parametros = parametroService.CarregarConfiguracao();

                if (parametros.Any())
                {
                    configuracao = new ConfiguracaoViewModel
                    {
                        Parametros = mapper.Map<IList<ParametroViewModel>>(parametros)
                    };

                    result.DefinirData(configuracao);
                    result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                    result.ExecutadoComSuccesso();
                    return result;
                }

                result.ExecutadoComSuccesso();

            }
            catch (System.Exception excecao)
            {
                result.ExecutadoComErro(excecao);
            }

            return result;
        }
    }
}
