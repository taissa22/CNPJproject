using AutoMapper;
using Perlink.Oi.Juridico.Application.Compartilhado.Interface;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Domain.Impl;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.Compartilhado.Impl
{
    public class EstadoAppService : IEstadoAppService
    {
        private readonly IEstadoService service;
        private readonly IMapper mapper;

        public EstadoAppService(IEstadoService service,
                                IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        public  async Task<IResultadoApplication<IEnumerable<EstadoDTO>>> RecuperarEstados()
        {
            var result = new ResultadoApplication<IEnumerable<EstadoDTO>>();

            try
            {
                var model = await service.RecuperarListaEstados();
                result.DefinirData(model);
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
