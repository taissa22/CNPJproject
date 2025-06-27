using AutoMapper;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Domain.Impl;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Impl
{
    public class Log_LoteAppService : BaseCrudAppService<Log_LoteHistoricoViewModel, Log_Lote, long>, ILog_LoteAppService
    {
        private readonly ILog_LoteService service;
        private readonly IMapper mapper;

        public Log_LoteAppService(ILog_LoteService service, IMapper mapper) : base(service, mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        public async Task<IResultadoApplication<ICollection<Log_LoteHistoricoViewModel>>> ObterHistorico(long codigoLote)
        {
            var result = new ResultadoApplication<ICollection<Log_LoteHistoricoViewModel>>();

            try
            {
                var model = await service.ObterHistorico(codigoLote);
                result.DefinirData(mapper.Map<ICollection<Log_LoteHistoricoViewModel>>(model)); ;
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