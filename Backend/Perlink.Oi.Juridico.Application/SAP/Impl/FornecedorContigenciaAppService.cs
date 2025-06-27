using AutoMapper;
using CsvHelper;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.ManutençãoFornecedorContigencia;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoFornecedores;
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
    public class FornecedorContigenciaAppService : BaseCrudAppService<FornecedorViewModel, Fornecedor, long>, IFornecedorContigenciaAppService
    {
        private readonly IFornecedorContigenciaService service;
        private readonly IMapper mapper;

        public FornecedorContigenciaAppService(IFornecedorContigenciaService service, IMapper mapper) : base(service, mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        public async Task<IResultadoApplication> AtualizarFornecedorContigencia(FornecedorContigenciaAtualizaViewModel fornecedorAtualizar)
        {
            var result = new ResultadoApplication();
            try
            {
                Fornecedor fornecedor = await service.RecuperarPorId(fornecedorAtualizar.id);
                fornecedor.ValorCartaFianca = fornecedorAtualizar.valorCartaFianca;
                fornecedor.DataCartaFianca = fornecedorAtualizar.dataVencimentoCartaFianca;
                fornecedor.IndicaAtivoSAP = fornecedorAtualizar.statusFornecedor == 1 ? true : false;

                

                await service.Atualizar(fornecedor);
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Alteracao);

                service.Commit();

                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }
            return result;
        }

        public async Task<IPagingResultadoApplication<ICollection<FornecedorContigenciaResultadoViewModel>>> ConsultarFornecedorContigencia(FornecedorContigenciaConsultaDTO fornecedorContigenciaConsultaDTO)
        {
            var result = new PagingResultadoApplication<ICollection<FornecedorContigenciaResultadoViewModel>>();
            try
            {
                var model = await service.ConsultarFornecedorContigencia(fornecedorContigenciaConsultaDTO);

                if (fornecedorContigenciaConsultaDTO.Total <= 0)
                {
                    result.Total = await service.RecuperarTotalRegistros(fornecedorContigenciaConsultaDTO);
                }
                else
                {
                    if (Math.Floor(Convert.ToDecimal(fornecedorContigenciaConsultaDTO.Total / fornecedorContigenciaConsultaDTO.Quantidade)) == fornecedorContigenciaConsultaDTO.Pagina)
                    {
                        result.Total = await service.RecuperarTotalRegistros(fornecedorContigenciaConsultaDTO);
                    }
                    else
                    {
                        result.Total = fornecedorContigenciaConsultaDTO.Total;
                    }
                }

                result.DefinirData(mapper.Map<ICollection<FornecedorContigenciaResultadoViewModel>>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }
            return result;
        }

        public async Task<IResultadoApplication<byte[]>> ExportarFornecedorContingencia(FornecedorContigenciaConsultaDTO fornecedorContigenciaConsultaDTO)
        {
            var result = new ResultadoApplication<byte[]>();
            try
            {
                var lista = await service.ExportarFornecedorContigencia(fornecedorContigenciaConsultaDTO);
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
                    csv.WriteRecords(mapper.Map<ICollection<FornecedorContigenciaExportarViewModel>>(lista));
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
    }
}