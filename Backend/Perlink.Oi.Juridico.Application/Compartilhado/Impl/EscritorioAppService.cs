using AutoMapper;
using Perlink.Oi.Juridico.Application.Compartilhado.Interface;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Shared.Application.Impl;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.Compartilhado.Impl
{

    public class EscritorioAppService : BaseCrudAppService<EscritorioViewModel, Profissional, long>, IEscritorioAppService

	{
		private readonly IEscritorioService service;
		private readonly IMapper mapper;

        public EscritorioAppService(IEscritorioService service, IMapper mapper) : base(service, mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }
        public async Task<IResultadoApplication<IEnumerable<EscritorioListaViewModel>>> RecuperarAreaCivelConsumidor()
        {
            
            var application = new ResultadoApplication<IEnumerable<EscritorioListaViewModel>>();

            try
            {
                var retorno = await service.RecuperarAreaCivilConsumidor();

                application.DefinirData(mapper.Map<IEnumerable<EscritorioListaViewModel>>(retorno));
                application.ExecutadoComSuccesso();
            }
            catch (System.Exception ex)
            {
                application.ExecutadoComErro(ex);
            }

            return application;
        }

        public async Task<IResultadoApplication<IEnumerable<EscritorioListaViewModel>>> RecuperarAreaCivelEstrategico()
        {
            var application = new ResultadoApplication<IEnumerable<EscritorioListaViewModel>>();

            try
            {
                var retorno = await service.RecuperarAreaCivelEstrategico();

                application.DefinirData(mapper.Map<IEnumerable<EscritorioListaViewModel>>(retorno));
                application.ExecutadoComSuccesso();
            }
            catch (System.Exception ex)
            {
                application.ExecutadoComErro(ex);
            }

            return application;
        }

        public async Task<IResultadoApplication<IEnumerable<EscritorioListaViewModel>>> RecuperarAreaProcon()
        {
            var application = new ResultadoApplication<IEnumerable<EscritorioListaViewModel>>();

            try
            {
                var retorno = await service.RecuperarProcon();

                application.DefinirData(mapper.Map<IEnumerable<EscritorioListaViewModel>>(retorno));
                application.ExecutadoComSuccesso();
            }
            catch (System.Exception ex)
            {
                application.ExecutadoComErro(ex);
            }

            return application;
        }
    }
    
}
