using AutoMapper;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Application.SAP.Interface.InterfaceBB;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB;
using Perlink.Oi.Juridico.Data;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
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
using static Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB.BBModalidadeViewModel;

namespace Perlink.Oi.Juridico.Application.SAP.Impl.InterfaceBB {
    public class BBModalidadeAppService : BaseCrudAppService<BBModalidadeViewModel, BBModalidade, long>, IBBModalidadeAppService
    {
        private readonly IBBModalidadeService service;
        private readonly IMapper mapper;
        private readonly ILancamentoProcessoService lancamentoService;

        public BBModalidadeAppService(IBBModalidadeService service, IMapper mapper,
                                        ILancamentoProcessoService lancamentoService) : base(service, mapper)
        {
            this.service = service;
            this.mapper = mapper;
            this.lancamentoService = lancamentoService;
        }
        public async Task<IPagingResultadoApplication<ICollection<BBModalidadeViewModel>>> ConsultarBBModalidade(BBModalidadeFiltroDTO filtroDTO)
        {
            var result = new PagingResultadoApplication<ICollection<BBModalidadeViewModel>>();

            try
            {

                var model = await service.recuperarModalidadeBB(filtroDTO);
                result.Total = await service.ObterQuantidadeTotalPorFiltro(filtroDTO);
               
                

                result.DefinirData(mapper.Map<ICollection<BBModalidadeViewModel>>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();             
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }
        public async Task<IResultadoApplication> AlterarBBModalidade(BBModalidadeViewModel objeto)
        {
            var result = new ResultadoApplication();
            var entidade = mapper.Map<BBModalidade>(objeto);
            try
            {
                await ValidarAsync(entidade);
                await service.Atualizar(entidade);
                service.Commit();
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Alteracao);
                result.ExecutadoComSuccesso();
            }
            catch (Exception e)
            {
                result.ExecutadoComErro(e);
            }
            return result;
        }

        public async Task<IResultadoApplication> CadastrarBBModalidade(BBModalidadeViewModel objeto)
        {
            var result = new ResultadoApplication();
            var entidade = mapper.Map<BBModalidade>(objeto);
            try
            {
                await ValidarAsync(entidade);
                await service.Inserir(entidade);
                service.Commit();
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Inclusao);
                result.ExecutadoComSuccesso();
            }
            catch (Exception e)
            {
                result.ExecutadoComErro(e);
            }
            return result;
        }
        public async Task<IResultadoApplication> ExcluirBBModalidade(long id)
        {
            var resultado = new ResultadoApplication();
            try
            {
                if (await lancamentoService.ExisteBBModalidadeAssociadoLancamento(id))
                {
                    throw new Exception("Não será possível excluir a Modalidade do Produto BB\n selecionada, pois se encontra relacionada com\n Lançamento de Processo.");
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
        public async Task<IResultadoApplication<byte[]>> ExportarBBModalidade(BBModalidadeFiltroDTO filtroDTO)
        {
            var result = new ResultadoApplication<byte[]>();

            try
            {
                var lista = await service.exportarModalidadeBB(filtroDTO);
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
                    csv.WriteRecords(mapper.Map<ICollection<BBModalidadeExportarViewModel>>(lista));
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
        private async Task ValidarAsync(BBModalidade obj)
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
            if (await service.CodigoBBJaExiste(obj))
                throw new Exception("O Código Modalidade BB \njá está cadastrado em outro registro.");
        }

    }

}
