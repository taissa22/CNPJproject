using AutoMapper;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.VariosContextos;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Domain.Impl;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Impl
{
    public class TipoVaraAppService : BaseCrudAppService<ComboboxViewModel<long>, TipoVara, long>, ITipoVaraAppService
    {
        private readonly ITipoVaraService service;
        private readonly IMapper mapper;

        public TipoVaraAppService(ITipoVaraService service, IMapper mapper) : base(service, mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        public async Task<IResultadoApplication<IEnumerable<ComboboxViewModel<long>>>> RecuperarPorVaraEComarca(long codigoComarca, long codigoVara)
        {
            var result = new ResultadoApplication<IEnumerable<ComboboxViewModel<long>>>();

            try
            {
                var model = await service.RecuperarPorVaraEComarca(codigoComarca, codigoVara);
                result.DefinirData(mapper.Map<IEnumerable<ComboboxViewModel<long>>>(model)); 
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }
    }
}
