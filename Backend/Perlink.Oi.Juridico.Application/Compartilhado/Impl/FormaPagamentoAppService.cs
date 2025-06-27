using AutoMapper;
using CsvHelper;
using Perlink.Oi.Juridico.Application.Compartilhado.Interface;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.ManutencaoFormaPagamento;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoFormaPagamento;
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

namespace Perlink.Oi.Juridico.Application.Compartilhado.Impl
{
    public class FormaPagamentoAppService : BaseCrudAppService<FormaPagamentoViewModel, FormaPagamento, long>, IFormaPagamentoAppService
    {
        private readonly IFormaPagamentoService service;
        private readonly ILoteService loteService;
        private readonly ILancamentoProcessoService lancamentoProcessoService;
        private readonly IMapper mapper;

        public FormaPagamentoAppService(IFormaPagamentoService service,
                                        ILoteService loteService,
                                        ILancamentoProcessoService lancamentoProcessoService,
                                        IMapper mapper) : base(service, mapper)
        {
            this.service = service;
            this.loteService = loteService;
            this.lancamentoProcessoService = lancamentoProcessoService;
            this.mapper = mapper;
        }

        public async Task<IPagingResultadoApplication<IEnumerable<FormaPagamentoGridViewModel>>> GetFormaPagamentoGridManutencao(FormaPagamentoFiltroDTO filtros)
        {
            var result = new PagingResultadoApplication<IEnumerable<FormaPagamentoGridViewModel>>();

            try
            {
                var model = await service.GetFormaPagamentoGridManutencao(filtros);
                result.DefinirData(mapper.Map<IEnumerable<FormaPagamentoGridViewModel>>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<int> GetTotalFormaPagamentoGridManutencao(FormaPagamentoFiltroDTO filtros)
        {
            return await service.GetTotalFormaPagamentoGridManutencao(filtros);
        }

        public async Task<IResultadoApplication> ExcluirFormaPagamentoComAssociacao(long id)
        {
            var result = new ResultadoApplication();
            try
            {
                bool hasAssociacaoLote = await loteService.ExisteLoteComFormaPagamento(id);
                bool hasAssociacaoLancamentoProcesso = await lancamentoProcessoService.ExisteLancamentoProcessoComFormaPagamento(id);
                if (!hasAssociacaoLote && !hasAssociacaoLancamentoProcesso)
                    return await this.RemoverPorId(id);
                else if (hasAssociacaoLote)
                {
                    result.ExibeNotificacao = true;
                    result.ExibirMensagem("Não será possível excluir a Forma de Pagamento selecionada, pois se encontra relacionado com Lote");
                    return result;
                }
                else
                {
                    result.ExibeNotificacao = true;
                    result.ExibirMensagem("Não será possível excluir a Forma de Pagamento selecionada, pois se encontra relacionado com Lançamento de Processo");
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }
            return result;
        }

        public async Task<IResultadoApplication<byte[]>> ExportarFormasPagamento(FormaPagamentoFiltroDTO filtroDTO)
        {
            var result = new ResultadoApplication<byte[]>();
            try
            {
                var lista = await service.GetFormaPagamentoGridExportarManutencao(filtroDTO);
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
                    csv.WriteRecords(mapper.Map<IEnumerable<FormaPagamentoGridExportarViewModel>>(lista));
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

        public async Task<IResultadoApplication> SalvarFormaPagamento(FormaPagamentoInclusaoEdicaoDTO inclusaoEdicaoDTO)
        {
            var result = new ResultadoApplication();
            var entidade = mapper.Map<FormaPagamento>(inclusaoEdicaoDTO);

            try
            {
                if (inclusaoEdicaoDTO.Codigo == 0)
                    await service.CadastrarFormaPagamento(entidade);
                else
                    await service.Atualizar(entidade);

                service.Commit();
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception e)
            {
                result.ExecutadoComErro(e);
            }
            return result;
        }
    }
}