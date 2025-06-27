using AutoMapper;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.AgendaAudiencia;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Domain.Impl;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Impl {
    public class ComarcaAppService : BaseCrudAppService<ComarcaViewModel, Comarca, long>, IComarcaAppService
    {
        private readonly IComarcaService service;
        private readonly IMapper mapper;
        public ComarcaAppService(IComarcaService service, IMapper mapper) : base(service, mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        public async Task<IResultadoApplication<ComarcaViewModel>> RecuperarComarca(long id)
        {
            var result = new ResultadoApplication<ComarcaViewModel>();

            try
            {
                var model = await service.RecuperarComarca(id);
                result.DefinirData(mapper.Map<ComarcaViewModel>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (System.Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication<ICollection<ComarcaComboViewModel>>> RecuperarComarcaPorEstado(string estado)
        {
            var result = new ResultadoApplication<ICollection<ComarcaComboViewModel>>();

            try
            {
                var model = await service.RecuperarComarcaPorEstado(estado);
                result.DefinirData(mapper.Map<ICollection<ComarcaComboViewModel>>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (System.Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;

        }
    }

}
