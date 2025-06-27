using AutoMapper;
using Perlink.Oi.Juridico.Application.Compartilhado.Interface;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Domain.Impl;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.Compartilhado.Impl
{
    public class ExportacaoPrePosRJAppService : BaseCrudAppService<ExportacaoPrePosRJViewModel, ExportacaoPrePosRJ, long>, IExportacaoPrePosRJAppService
    {
        private readonly IExportacaoPrePosRJService service;
        private readonly IMapper mapper;

        public ExportacaoPrePosRJAppService(IExportacaoPrePosRJService service, IMapper mapper) : base(service, mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        public async Task<IResultadoApplication<ExportacaoPrePosRJViewModel>> InserirDados(ExportacaoPrePosRJViewModel viewModel)
        {
            var application = new ResultadoApplication<ExportacaoPrePosRJViewModel>();

            try
            {
                var resultado = await service.InserirDados(mapper.Map<ExportacaoPrePosRJ>(viewModel));
                var retorno = mapper.Map<ExportacaoPrePosRJViewModel>(resultado);
                application.DefinirData(retorno);

                //if (application.Sucesso)
                //{
                service.Commit();
                application.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Inclusao);
                //}
            }
            catch (System.Exception ex)
            {
                application.ExecutadoComErro(ex);
            }

            return application;
        }

        public async Task<IResultadoApplication<ICollection<ExportacaoPrePosRJViewModel>>> ListarExportacaoPrePosRj(DateTime? dataExtracao, int pagina, int qtd)
        {
            var resultado = new PagingResultadoApplication<ICollection<ExportacaoPrePosRJViewModel>>();

            try
            {
                var model = await service.ListarExportacaoPrePosRj(dataExtracao, pagina, qtd);
                var data = mapper.Map<ICollection<ExportacaoPrePosRJViewModel>>(model);

                resultado.DefinirData(data);
                resultado.Total = await service.ObterQuantidadeTotal(dataExtracao);
                resultado.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                resultado.ExecutadoComSuccesso();
            }
            catch (System.Exception ex)
            {
                resultado.ExecutadoComErro(ex);
            }

            return resultado;
        }

        public async Task<IResultadoApplication<ExportacaoPrePosRJViewModel>> ExpurgarExportacaoPrePosRj(long idExtracao, bool naoExpurgar)
        {
            var resultado = new ResultadoApplication<ExportacaoPrePosRJViewModel>();

            try
            {
                var model = await service.RecuperarPorId(idExtracao);

                model.NaoExpurgar = naoExpurgar;
                resultado.Resultado(await service.Atualizar(mapper.Map<ExportacaoPrePosRJ>(model)));

                if (resultado.Sucesso)
                {
                    service.Commit();
                    resultado.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                }
            }
            catch (Exception ex)
            {
                resultado.ExecutadoComErro(ex);
            }

            return resultado;
        }

        public async Task<IResultadoApplication<ExportacaoPrePosRJListaViewModel>> DownloadExportacaoPrePosRj(long idExtracao, ICollection<string> tiposProcessos)
        {
            var resultado = new ResultadoApplication<ExportacaoPrePosRJListaViewModel>();

            try
            {
                var viewModel = await service.DownloadExportacaoPrePosRj(idExtracao, tiposProcessos);
                var data = mapper.Map<ExportacaoPrePosRJListaViewModel>(viewModel);

                resultado.DefinirData(data);
                resultado.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                resultado.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                resultado.ExecutadoComErro(ex);
            }
            return resultado;
        }
    }
}
