using AutoMapper;
using CsvHelper;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
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
    public class Empresas_SapAppService : BaseCrudAppService<Empresas_SapViewModel, Empresas_Sap, long>, IEmpresas_SapAppService
    {
        private readonly IEmpresas_SapService service;
        private readonly IMapper mapper;
        private readonly IFornecedorasContratosService fornecedorasContratosService;
        private readonly IFornecedorasFaturasService fornecedorasFaturasService;
        private readonly IEmpresasSapFornecedorasService empresasSapFornecedorasService;

        private readonly IParteService parteService;

        public Empresas_SapAppService(IEmpresas_SapService service, IMapper mapper, IEmpresasSapFornecedorasService empresasSapFornecedorasService, IFornecedorasFaturasService fornecedorasFaturasService, IFornecedorasContratosService fornecedorasContratosService, IParteService parteService) : base(service, mapper)
        {
            this.service = service;
            this.mapper = mapper;
            this.fornecedorasContratosService = fornecedorasContratosService;
            this.fornecedorasFaturasService = fornecedorasFaturasService;
            this.parteService = parteService;
            this.empresasSapFornecedorasService = empresasSapFornecedorasService;
        }

        public async Task<IResultadoApplication<byte[]>> ExportarEmpresasSap(Empresas_SapFiltroDTO filtroDTO)
        {
            var result = new ResultadoApplication<byte[]>();
            try
            {
                var lista = await service.ExportarEmpresasSap(filtroDTO);
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
                    csv.WriteRecords(mapper.Map<ICollection<Empresas_SapExportarViewModel>>(lista));
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

        public async Task<int> ObterQuantidadeTotalPorFiltro(Empresas_SapFiltroDTO filtroDTO)
        {
            var result = new ResultadoApplication<int>();

            try
            {
                //Zerando pra pegar a query sem quebra de página
                filtroDTO.Quantidade = 0;
                filtroDTO.Pagina = 0;
                var model = await service.ObterQuantidadeTotalPorFiltro(filtroDTO);
                result.DefinirData(model);
            }
            catch (System.Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result.Data;
        }

        public async Task<IPagingResultadoApplication<ICollection<Empresas_SapResultadoViewModel>>> RecuperarEmpresasPorFiltro(Empresas_SapFiltroDTO filtroDTO)
        {
            var result = new PagingResultadoApplication<ICollection<Empresas_SapResultadoViewModel>>();

            try
            {
                var model = await service.RecuperarEmpresasPorFiltro(filtroDTO);

                if (filtroDTO.Total <= 0)
                {
                    result.Total = await service.ObterQuantidadeTotalPorFiltro(filtroDTO);
                }
                else
                {
                    if (Math.Floor(Convert.ToDecimal(filtroDTO.Total / filtroDTO.Quantidade)) == filtroDTO.Pagina)
                    {
                        result.Total = await service.ObterQuantidadeTotalPorFiltro(filtroDTO);
                    }
                    else
                        result.Total = filtroDTO.Total;
                }

                result.DefinirData(mapper.Map<ICollection<Empresas_SapResultadoViewModel>>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (System.Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication> CadastrarEmpresa(Empresas_SapViewModel viewModel)
        {
            var resultado = new ResultadoApplication<object>();
            var entidade = mapper.Map<Empresas_Sap>(viewModel);
            try
            {
                Validar(entidade);
                await CadastrarAtualizar(resultado, entidade);
            }
            catch (Exception excecao)
            {
                resultado.ExibirMensagem(excecao.Message);
                resultado.ExecutadoComErro(excecao);
            }

            return resultado;
        }

        public async Task<IResultadoApplication> AtualizarEmpresa(Empresas_SapViewModel viewModel)
        {
            var resultado = new ResultadoApplication<object>();
            var entidade = mapper.Map<Empresas_Sap>(viewModel);
            try
            {
                if (service.EmpresaAssociadaNaEmpresaDoGrupo(entidade).Result && !viewModel.ConfirmaSiglaRepetidaNaAlteracao)
                {
                    bool confirmacaoEnvio = true;
                    var obj = new { confirmacaoEnvio };
                    resultado.DefinirData(obj);
                    throw new Exception($"Existe(m) Empresa(s) do Grupo associada(s) à sigla. Confirma a alteração?");
                }

                Validar(entidade);
                await CadastrarAtualizar(resultado, entidade);
            }
            catch (Exception excecao)
            {
                resultado.ExibirMensagem(excecao.Message);
                resultado.ExecutadoComErro(excecao);
            }

            return resultado;
        }

        private async Task CadastrarAtualizar(ResultadoApplication<object> resultado, Empresas_Sap entidade)
        {
            if (entidade.Id == 0)
            {
                await service.CadastrarEmpresa(entidade);
                // resultado.DefinirData(id);
                resultado.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Inclusao);
            }
            else
            {
                await service.AtualizarEmpresa(entidade);
                resultado.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Alteracao);
            }
            
            service.Commit();

            resultado.DefinirData(resultado.Data ?? entidade.Id);

            resultado.ExecutadoComSuccesso();
        }

        private void Validar(Empresas_Sap entidade)
        {
            if (service.EmpresaComSiglaJaCadastrada(entidade).Result)
            {
                throw new Exception($"Sigla '{entidade.Sigla.ToUpper()}' já cadastrada");
            }
            if (!entidade.Validar().IsValid)
            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach (var error in entidade.Validar().Errors)
                {
                    stringBuilder.AppendLine(error.ErrorMessage);
                }

                throw new Exception(stringBuilder.ToString());
            }
        }

        public async Task<IResultadoApplication> ExcluirEmpresasSap(long codigoEmpresasSap)
        {
            var resultado = new ResultadoApplication<Empresas_SapViewModel>();
            try
            {
                if (await fornecedorasContratosService.ExisteFornecedorasContratosComEmpresaSap(codigoEmpresasSap))
                {
                    resultado.ExibeNotificacao = true;
                    resultado.ExibirMensagem("Não será possível excluir a Empresa SAP selecionada, pois se encontra relacionada com Contrato.");
                    return resultado;
                }
                if (await parteService.ExisteParteComEmpresaSap(codigoEmpresasSap))
                {
                    resultado.ExibeNotificacao = true;
                    resultado.ExibirMensagem("Não será possível excluir a Empresa SAP selecionada, pois se encontra relacionada com Empresa do Grupo.");
                    return resultado;
                }
                if (await fornecedorasFaturasService.ExisteFornecedorasFaturasComEmpresaSap(codigoEmpresasSap))
                {
                    resultado.ExibeNotificacao = true;
                    resultado.ExibirMensagem("Não será possível excluir a Empresa SAP selecionada, pois se encontra relacionada com Fatura.");
                    return resultado;
                }
                if (await empresasSapFornecedorasService.ExisteEmpresaSapFornecedorasComEmpresaSap(codigoEmpresasSap))
                {
                    resultado.ExibeNotificacao = true;
                    resultado.ExibirMensagem("Não será possível excluir a Empresa SAP selecionada, pois se encontra relacionada com Fornecedor.");
                    return resultado;
                }
                await service.ExcluirEmpresasSap(codigoEmpresasSap);
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