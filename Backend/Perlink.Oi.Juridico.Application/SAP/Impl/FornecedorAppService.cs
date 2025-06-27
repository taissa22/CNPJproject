using AutoMapper;
using CsvHelper;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
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
    public class FornecedorAppService : BaseCrudAppService<FornecedorViewModel, Fornecedor, long>, IFornecedorAppService
    {
        private readonly IFornecedorService service;
        private readonly IMapper mapper;
        private readonly ILancamentoProcessoService lancamentoProcessoService;
        private readonly ILoteService loteService;
        private readonly IEmpresaDoGrupoService empresaDoGrupoService;
        private readonly IPedidoSAPService pedidoSAPService;
        private readonly IFornecedorFormaPagamentoService fornecedorFormaPagamentoService;
        public FornecedorAppService(IFornecedorService service, IMapper mapper, ILancamentoProcessoService lancamentoProcessoService, ILoteService loteService, IEmpresaDoGrupoService empresaDoGrupoService, IPedidoSAPService pedidoSAPService, IFornecedorFormaPagamentoService fornecedorFormaPagamentoService) : base(service, mapper)
        {
            this.service = service;
            this.mapper = mapper;
            this.lancamentoProcessoService = lancamentoProcessoService;
            this.loteService = loteService;
            this.empresaDoGrupoService = empresaDoGrupoService;
            this.pedidoSAPService = pedidoSAPService;
            this.fornecedorFormaPagamentoService = fornecedorFormaPagamentoService;
        }

        public async Task<IResultadoApplication> ExcluirFornecedor(long codigoFornecedor)
        {
            var resultado = new ResultadoApplication<FornecedorResultadoViewModel>();
            try
            {
                if (await lancamentoProcessoService.ExisteLancamentoProcessoComFornecedor(codigoFornecedor))
                {
                    throw new Exception("Não será possível excluir o Fornecedor selecionado, pois se encontra relacionado com Lançamento de Processo.");
                }
                if (await loteService.ExisteLoteComFornecedor(codigoFornecedor))
                {
                    throw new Exception("Não será possível excluir o Fornecedor selecionado, pois se encontra relacionado com Lote.");
                }
                if (await empresaDoGrupoService.ExisteEmpresaDoGrupoComFornecedor(codigoFornecedor))
                {
                    throw new Exception("Não será possível excluir o Fornecedor selecionado, pois se encontra relacionado com Empresa do Grupo.");
                }
                if (await pedidoSAPService.ExistePedidoSAPComFornecedor(codigoFornecedor))
                {
                    throw new Exception("Não será possível excluir o Fornecedor selecionado, pois se encontra relacionado com Pedido SAP.");
                }
                if(await fornecedorFormaPagamentoService.ExisteFormaPagamentoComFornecedor(codigoFornecedor)) {
                    throw new Exception("Não será possível excluir o Fornecedor selecionado, pois se encontra relacionado com Forma Pagamento.");
                }


                await service.ExcluirFornecedor(codigoFornecedor);
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

        public async Task<IResultadoApplication> CadastrarFornecedor(FornecedorCriacaoViewModel fornecedorCriacaoViewModel)
        {
            var resultado = new ResultadoApplication<FornecedorCriacaoViewModel>();

            try
            {
                var fornecedorJaCadastrado = await service.FornecedorComCodigoSAPJaCadastrado(fornecedorCriacaoViewModel.CodigoFornecedorSAP);
                if (fornecedorJaCadastrado != null && !fornecedorCriacaoViewModel.CriarCodigoFornecedorSAP )
                {
                    fornecedorCriacaoViewModel.ConfirmacaoEnvio = true;
                    resultado.DefinirData(fornecedorCriacaoViewModel);
                  

                    throw new Exception("Este código de fornecedor SAP já está cadastrado em outro registro. Confirma a operação?");
                }

                var fornecedor = await this.service.CadastrarFornecedor(fornecedorCriacaoViewModel.MapearParaEntidadeFornecedor());
                fornecedorCriacaoViewModel.Id = fornecedor.Id;
                resultado.DefinirData(fornecedorCriacaoViewModel);
                resultado.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Inclusao);
                resultado.ExecutadoComSuccesso();

                this.service.Commit();
            }
            catch (Exception excecao)
            {
                resultado.ExibirMensagem(excecao.Message);
                resultado.ExecutadoComErro(excecao);
            }

            return resultado;
        }

        public async Task<IResultadoApplication> AtualizarFornecedor(FornecedorAtualizarViewModel fornecedorAtualizarViewModel)
        {
            var resultado = new ResultadoApplication<FornecedorAtualizarViewModel>(); ;

            try
            {
                var verificacao = await service.VerificarCodigoSap(fornecedorAtualizarViewModel.CodigoFornecedorSAP, fornecedorAtualizarViewModel.Id);

                if (verificacao == null)
                {
                    var fornecedorJaCadastrado = await service.FornecedorComCodigoSAPJaCadastrado(fornecedorAtualizarViewModel.CodigoFornecedorSAP);
                    if (fornecedorJaCadastrado != null && !fornecedorAtualizarViewModel.CriarCodigoFornecedorSAP)
                    {
                        fornecedorAtualizarViewModel.ConfirmacaoEnvio = true;
                        resultado.DefinirData(fornecedorAtualizarViewModel);
                        throw new Exception("Este código de fornecedor SAP já está cadastrado em outro registro. Confirma a operação?");
                      
                    }
                }
                await service.AtualizarFornecedor(fornecedorAtualizarViewModel.MapearParaEntidadeFornecedorEdicao());

                resultado.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Alteracao);
                resultado.ExecutadoComSuccesso();
                this.service.Commit();
            }
            catch (Exception execao)
            {
                resultado.ExecutadoComErro(execao);
            }

            return resultado;
        }

        public async Task<IResultadoApplication<byte[]>> ExportarFornecedores(FornecedorFiltroDTO fornecedorFiltroDTO)
        {
            var result = new ResultadoApplication<byte[]>();
            try
            {
                fornecedorFiltroDTO.Quantidade = 0;
                fornecedorFiltroDTO.Pagina = 0;
                var lista = await service.ExportarFornecedores(fornecedorFiltroDTO);
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
                    csv.WriteRecords(mapper.Map<ICollection<FornecedorExportarViewModel>>(lista));
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

        public async Task<IPagingResultadoApplication<ICollection<FornecedorResultadoViewModel>>> RecuperarFornecedorPorFiltro(FornecedorFiltroDTO fornecedorFiltroDTO)
        {
            var result = new PagingResultadoApplication<ICollection<FornecedorResultadoViewModel>>();

            try
            {
                var model = await service.RecuperarFornecedorPorFiltro(fornecedorFiltroDTO);

                result.DefinirData(mapper.Map<ICollection<FornecedorResultadoViewModel>>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (System.Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<int> ObterQuantidadeTotalPorFiltro(FornecedorFiltroDTO loteFiltroDTO)
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
    }
}