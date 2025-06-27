using AutoMapper;
using Perlink.Oi.Juridico.Application.Compartilhado.Interface;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.Filtros;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Domain.Impl;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.Compartilhado.Impl
{
    public class ProfissionalAppService : BaseCrudAppService<ProfissionalDropDownViewModel, Profissional, long>, IProfissionalAppService
    {
        private readonly IProfissionalService service;
        private readonly IMapper mapper;

        public ProfissionalAppService(IProfissionalService service, IMapper mapper) : base(service, mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        public async Task<IResultadoApplication<ICollection<ProfissionalDropDownViewModel>>> RecuperarTodosProfissionais()
        {
            var result = new ResultadoApplication<ICollection<ProfissionalDropDownViewModel>>();
            try
            {
                var model = await service.RecuperarTodosProfissionais();
                result.DefinirData(mapper.Map<ICollection<ProfissionalDropDownViewModel>>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();

            }
            catch (System.Exception ex)
            {
                result.ExecutadoComErro(ex);

            }
            return result;
        }

        public async Task<IResultadoApplication<ICollection<ProfissionalDropDownViewModel>>> RecuperarTodosEscritorios()
        {
            var result = new ResultadoApplication<ICollection<ProfissionalDropDownViewModel>>();
            try
            {
                var model = await service.RecuperarTodosEscritorios();
                result.DefinirData(mapper.Map<ICollection<ProfissionalDropDownViewModel>>(model));
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