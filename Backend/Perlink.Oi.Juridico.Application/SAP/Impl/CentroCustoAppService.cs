using AutoMapper;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.ManutencaoCentroCusto;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoCentroCusto;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Domain.Impl;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Impl
{
    public class CentroCustoAppService : BaseCrudAppService<CentroCustoViewModel, CentroCusto, long>, ICentroCustoAppService
    {
        private readonly ICentroCustoService service;
        private readonly IMapper mapper;
        private readonly ILancamentoProcessoService lancamentoService;
        private readonly IParteService empresaService;
        private readonly ILoteService loteService;

        public CentroCustoAppService(ICentroCustoService service, IMapper mapper, ILancamentoProcessoService lancamentoService,
                IParteService empresaService, ILoteService loteService
            ) : base(service, mapper)
        {
            this.service = service;
            this.mapper = mapper;
            this.empresaService = empresaService;
            this.lancamentoService = lancamentoService;
            this.loteService = loteService;
        }

        public async Task<IPagingResultadoApplication<ICollection<CentroCustoViewModel>>> ConsultarCentrosCustos(CentroCustoFiltroDTO CentroCustoFiltroDTO)
        {
            var result = new PagingResultadoApplication<ICollection<CentroCustoViewModel>>();

            try
            {
                var model = await service.ConsultarCentrosCustos(CentroCustoFiltroDTO);
                result.Total = await service.ObterQuantidadeTotalPorFiltro(CentroCustoFiltroDTO);

                result.DefinirData(mapper.Map<ICollection<CentroCustoViewModel>>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (System.Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication<byte[]>> ExportarCentroCusto(CentroCustoFiltroDTO CentroCustoFiltroDTO)
        {
            var result = new ResultadoApplication<byte[]>();
            try
            {
                var lista = await service.ExportarCentrosCustos(CentroCustoFiltroDTO);
                byte[] dados;
                var csvConfiguration = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.GetCultureInfo("pt-BR"))
                {
                    Delimiter = ";",
                    TrimOptions = CsvHelper.Configuration.TrimOptions.Trim,
                    HasHeaderRecord = true,
                    IgnoreBlankLines = true
                };

                using (var memoryStream = new MemoryStream())
                using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                using (var csv = new CsvWriter(streamWriter, csvConfiguration))
                {
                    csv.WriteRecords(mapper.Map<ICollection<CentroCustoExportarViewModel>>(lista));
                    streamWriter.Flush();
                    dados = memoryStream.ToArray();
                }

                result.DefinirData(dados);
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }
            return result;
        }

        public async Task<int> ObterQuantidadeTotalPorFiltro(CentroCustoFiltroDTO loteFiltroDTO)
        {
            var result = new ResultadoApplication<int>();
            try
            {
                //Zerando pra pegar a query sem quebra de página
                loteFiltroDTO.Quantidade = 0;
                loteFiltroDTO.Pagina = 0;
                var model = await service.ObterQuantidadeTotalPorFiltro(loteFiltroDTO);
                result.DefinirData(model);
            }
            catch (System.Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result.Data;
        }

        public async Task<IResultadoApplication> CriarAlterarCentroCusto(CentroCustoViewModel centroCustoViewModel)
        {
            var result = new ResultadoApplication();

            try
            {
                var entidade = mapper.Map<CentroCusto>(centroCustoViewModel);

                if (await service.VerificarDuplicidadeDescricaoCentroCusto(entidade))
                    throw new Exception("O valor informado para Descrição do Centro de Custo já está cadastrado em outro registro.");
                
                //PKE12892 - remoção da validação, porque no MI não validava isso.
                //if (await service.VerificarDuplicidadeCentroCustoSAP(entidade))
                //    throw new Exception("O valor informado para Centro de Custo SAP já está cadastrado em outro registro.");

                if (entidade.Id == 0)
                    await service.CadastrarCentroCusto(entidade);
                else
                    await service.Atualizar(entidade);

                service.Commit();
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication> ExcluirCentroCusto(long codigoCentroCusto)
        {
            var resultado = new ResultadoApplication();
            try
            {
                if (await loteService.ExisteCentroCustoAssociado(codigoCentroCusto))
                {
                    resultado.ExibeNotificacao = true;
                    resultado.ExibirMensagem("Não será possível excluir o Centro de Custo selecionado, pois se encontra relacionado com Lote.");
                    return resultado;
                }
                if (await lancamentoService.ExisteCentroCustoAssociado(codigoCentroCusto))
                {
                    resultado.ExibeNotificacao = true;
                    resultado.ExibirMensagem("Não será possível excluir o Centro de Custo selecionado, pois se encontra relacionado com Lançamento de Processo.");
                    return resultado;
                }
                if (await empresaService.ExisteCentroCustoAssociado(codigoCentroCusto))
                {
                    resultado.ExibeNotificacao = true;
                    resultado.ExibirMensagem("Não será possível excluir o Centro de Custo selecionado, pois se encontra relacionado com Empresa do Grupo.");
                    return resultado;
                }

                await service.RemoverPorId(codigoCentroCusto);
                service.Commit();

                resultado.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Exclusao);
                resultado.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                resultado.ExibirMensagem(ex.Message);
                resultado.ExecutadoComErro(ex);
            }
            return resultado;
        }
    }
}