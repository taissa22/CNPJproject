using AutoMapper;
using Perlink.Oi.Juridico.Application.Compartilhado.Interface;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.Filtros;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Domain.Impl;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.Compartilhado.Impl
{
    public class BancoAppService : BaseCrudAppService<BancoViewModel, Banco, long>, IBancoAppService
    {
        private readonly IBancoService service;
        private readonly IMapper mapper;

        public BancoAppService(IBancoService service, IMapper mapper) : base(service, mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        public async Task<IResultadoApplication<ICollection<BancoViewModel>>> RecuperarNomeBanco()
        {
            var result = new ResultadoApplication<ICollection<BancoViewModel>>();
            try
            {
                var model = await service.RecuperarNomeBanco();
                result.DefinirData(mapper.Map<ICollection<BancoViewModel>>(model));
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
