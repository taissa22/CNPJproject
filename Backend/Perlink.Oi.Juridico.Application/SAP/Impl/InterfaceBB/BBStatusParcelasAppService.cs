using AutoMapper;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.General;
using Perlink.Oi.Juridico.Application.SAP.Interface.InterfaceBB;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB;
using Perlink.Oi.Juridico.Data;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service.InterfaceBB;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Domain.Impl;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB.BBStatusParcelasViewModel;

namespace Perlink.Oi.Juridico.Application.SAP.Impl.InterfaceBB
{
    public class BBStatusParcelasAppService : BaseCrudAppService<BBStatusParcelasViewModel, BBStatusParcelas, long>, IBBStatusParcelasAppService
    {
        private readonly IBBStatusParcelasService service;
        private readonly IMapper mapper;
        private readonly ILancamentoProcessoService lancamentoProcessoService;

        public BBStatusParcelasAppService(IBBStatusParcelasService service, ILancamentoProcessoService lancamentoProcessoService, IMapper mapper) : base(service, mapper)
        {
            this.service = service;
            this.lancamentoProcessoService = lancamentoProcessoService;
            this.mapper = mapper;
        }

        public async Task<IPagingResultadoApplication<ICollection<BBStatusParcelasViewModel>>> ConsultarBBStatusParcelas(DescriptionFilterViewModel filtroDTO)
        {
            var result = new PagingResultadoApplication<ICollection<BBStatusParcelasViewModel>>();
            try
            {
                var listaOrdenada = await service.Pesquisar(x => string.IsNullOrEmpty(filtroDTO.Descricao) || 
                                                            (x.Descricao.ToLower().Trim().Contains(filtroDTO.Descricao.ToLower().Trim())))
                                                 .OrdenarPorPropriedade(filtroDTO.Ascendente, filtroDTO.Ordenacao, "DESCRICAO")
                                                 .Paginar(filtroDTO.Pagina, filtroDTO.Quantidade).ToListAsync();

                result.DefinirData(mapper.Map<ICollection<BBStatusParcelasViewModel>>(listaOrdenada));
                result.Total = service.getTotalFromSearch(x => string.IsNullOrEmpty(filtroDTO.Descricao) || x.Descricao.ToLower().Trim().Contains(filtroDTO.Descricao.ToLower().Trim()));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();                
            }
            catch (Exception ex)
            { 
                result.ExecutadoComErro(ex); 
            }

            return result;
        }

        public async Task<IResultadoApplication> ExcluirBBStatusParcelas(long id)
        {
            var resultado = new ResultadoApplication();
            try
            {
                if (await lancamentoProcessoService.ExisteLamentoProcessoAssociadoComStatusParcela(id))
                {
                    throw new Exception("Não será possível excluir o Status da Parcela BB selecionado, pois se encontra relacionado com Lançamento Processo.");
                }

                await service.RemoverPorId(id);
                service.Commit();

                resultado.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Exclusao);
                resultado.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                resultado.ExibirMensagem(ex.Message);
                resultado.ExecutadoComErro(ex);
                resultado.ExibeNotificacao = true;
            }
            return resultado;
        }

        public async Task<IResultadoApplication<byte[]>> ExportarBBStatusParcelas(DescriptionFilterViewModel filtroDTO)
        {
            var result = new ResultadoApplication<byte[]>();
            try
            {
                var lista = await service.Pesquisar(x => string.IsNullOrEmpty(filtroDTO.Descricao) || x.Descricao.ToLower().Trim().Contains(filtroDTO.Descricao.ToLower().Trim())).ToListAsync();
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
                    csv.WriteRecords(mapper.Map<ICollection<BBStatusParcelasExportarViewModel>>(lista));
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

        public async Task<IResultadoApplication> SalvarBBStatusParcelas(BBStatusParcelaInclusaoEdicaoDTO inclusaoEdicao)
        {
            var result = new ResultadoApplication();
            var entidade = mapper.Map<BBStatusParcelas>(inclusaoEdicao);
            try
            {
                Validar(entidade);

                if (await service.VerificarDuplicidadeCodigoBBStatusParcela(entidade))
                    throw new Exception("O Código Status da Parcela BB já está cadastrado em outro registro.");

                if (entidade.Id == 0)
                    await service.Inserir(entidade);
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

        private async Task Validar(BBStatusParcelas obj)
        {

            if (!obj.Validar().IsValid)
            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach (var error in obj.Validar().Errors)
                {
                    stringBuilder.AppendLine(error.ErrorMessage);
                }

                throw new Exception(stringBuilder.ToString());
            }
        }
    }
}