using AutoMapper;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.General;
using Perlink.Oi.Juridico.Application.SAP.Interface.InterfaceBB;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB;
using Perlink.Oi.Juridico.Data;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
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
using static Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB.BBNaturezasAcoesViewModel;

namespace Perlink.Oi.Juridico.Application.SAP.Impl.InterfaceBB
{
    public class BBNaturezasAcoesAppService : BaseCrudAppService<BBNaturezasAcoesViewModel, BBNaturezasAcoes, long>, IBBNaturezasAcoesAppService
    {
        private readonly IBBNaturezasAcoesService service;
        private readonly IMapper mapper;
        private readonly IAcaoService acaoService;
        public BBNaturezasAcoesAppService(IBBNaturezasAcoesService service, IMapper mapper,
                                    IAcaoService acaoService) : base(service, mapper)
        {
            this.service = service;
            this.mapper = mapper;
            this.acaoService = acaoService;
        }

        public async Task<IPagingResultadoApplication<ICollection<BBNaturezasAcoesViewModel>>> ConsultarBBNaturezasAcoes(DescriptionFilterViewModel filtroDTO)
        {
            var result = new PagingResultadoApplication<ICollection<BBNaturezasAcoesViewModel>>();

            try
            {
                var listaOrdenada = await service.Pesquisar(x => string.IsNullOrEmpty(filtroDTO.Descricao) || 
                                                            (x.Descricao.ToLower().Trim().Contains(filtroDTO.Descricao.ToLower().Trim())))
                                                 .OrdenarPorPropriedade(filtroDTO.Ascendente, filtroDTO.Ordenacao, "DESCRICAO")
                                                 .Paginar(filtroDTO.Pagina, filtroDTO.Quantidade)
                                                 .ToListAsync();
                                                    
                result.DefinirData(mapper.Map<ICollection<BBNaturezasAcoesViewModel>>(listaOrdenada));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
                result.Total = service.getTotalFromSearch(x => string.IsNullOrEmpty(filtroDTO.Descricao) || x.Descricao.ToLower().Trim().Contains(filtroDTO.Descricao.ToLower().Trim()));
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }
        public async Task<IResultadoApplication> AlterarBBNaturezasAcoes(BBNaturezasAcoesViewModel objeto)
        {
            var result = new ResultadoApplication();
            var entidade = mapper.Map<BBNaturezasAcoes>(objeto);
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

        public async Task<IResultadoApplication> CadastrarBBNaturezasAcoes(BBNaturezasAcoesViewModel objeto)
        {
            var result = new ResultadoApplication();
            var entidade = mapper.Map<BBNaturezasAcoes>(objeto);
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
        public async Task<IResultadoApplication> ExcluirBBNaturezasAcoes(long id)
        {
            var resultado = new ResultadoApplication();
            try
            {
                if (await acaoService.ExisteBBNaturezaAcaoAssociadoAcao(id))
                {
                    throw new Exception("Não será possível excluir a Natureza da Ação BB \nselecionada, pois se encontra relacionada com\n Ação.");
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
        public async Task<IResultadoApplication<byte[]>> ExportarBBNaturezasAcoes(DescriptionFilterViewModel filtroDTO)
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
                    csv.WriteRecords(mapper.Map<ICollection<BBNaturezasAcoesExportarViewModel>>(lista));
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
        private async Task ValidarAsync(BBNaturezasAcoes obj)
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
                throw new Exception("O Código Natureza da Ação BB já está cadastrado em outro registro.");
        }
    }

}
