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
    public class EmpresaDoGrupoAppService : BaseCrudAppService<EmpresaDoGrupoViewModel, Parte, long>, IEmpresaDoGrupoAppService
    {
        private readonly IEmpresaDoGrupoService service;
        private readonly IMapper mapper;

        public EmpresaDoGrupoAppService(IEmpresaDoGrupoService service, IMapper mapper) : base(service, mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        public async Task<IResultadoApplication<IEnumerable<EmpresaDoGrupoListaViewModel>>> RecuperarEmpresaDoGrupo()
        {
            var application = new ResultadoApplication<IEnumerable<EmpresaDoGrupoListaViewModel>>();

            try
            {
                var retorno = await service.RecuperarEmpresaDoGrupo();

                application.DefinirData(mapper.Map<IEnumerable<EmpresaDoGrupoListaViewModel>>(retorno));
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
