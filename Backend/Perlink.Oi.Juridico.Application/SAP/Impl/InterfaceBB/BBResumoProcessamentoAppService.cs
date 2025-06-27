using AutoMapper;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel;
using Perlink.Oi.Juridico.Application.SAP.Interface.InterfaceBB;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service.InterfaceBB;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Domain.Impl;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Impl.InterfaceBB
{
    public class BBResumoProcessamentoAppService : BaseCrudAppService<BBResumoProcessamentoViewModel, BBResumoProcessamento, long>, IBBResumoProcessamentoAppService
    {
        private readonly IBBResumoProcessamentoService service;
        private readonly IMapper mapper;


        public BBResumoProcessamentoAppService(IBBResumoProcessamentoService service, IMapper mapper) : base(service, mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        public async Task<IResultadoApplication<ICollection<BBResumoProcessamentoResultadoViewModel>>> ConsultarArquivoRetorno(BBResumoProcessamentoFiltroDTO filtroDTO)
        {
            var result = new PagingResultadoApplication<ICollection<BBResumoProcessamentoResultadoViewModel>>();

            try
            {
                var model = await service.ConsultarArquivoRetorno(filtroDTO);
                result.DefinirData(mapper.Map<ICollection<BBResumoProcessamentoResultadoViewModel>>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
                if (filtroDTO.Total <= 0)
                {
                    filtroDTO.Pagina = 0;
                    filtroDTO.Quantidade = 0;
                    result.Total = await service.TotaisArquivoRetorno(filtroDTO);
                }
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication<ICollection<BBResumoProcessamentoGuiaViewModel>>> BuscarGuiasOK(long numeroLoteBB)
        {
            var result = new ResultadoApplication<ICollection<BBResumoProcessamentoGuiaViewModel>>();
            try
            {
                var model = await service.BuscarGuiasOK(numeroLoteBB);
                result.DefinirData(mapper.Map<ICollection<BBResumoProcessamentoGuiaViewModel>>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication<byte[]>> ExportarArquivoRetorno(BBResumoProcessamentoFiltroDTO filtroDTO)
        {
            var result = new ResultadoApplication<byte[]>();
            try
            {
                var lista = await service.ExportarArquivoRetorno(filtroDTO);
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
                    csv.WriteRecords(mapper.Map<ICollection<BBResumoProcessamentoResultadoExportarViewModel>>(lista));
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

        public async Task<IResultadoApplication<byte[]>> ExportarGuias(long numeroLoteBB)
        {
            var result = new ResultadoApplication<byte[]>();
            try
            {
                var guias = await service.BuscarGuiasOK(numeroLoteBB);
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
                    csv.WriteRecords(mapper.Map<ICollection<BBResumoProcessamentoGuiaExportarViewModel>>(guias));
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

        public async Task<IResultadoApplication<BBResumoProcessamentoGuiaExibidaViewModel>> BuscarGuiaExibicao(long codigoProcesso, long codigoLancamento)
        {
            var result = new ResultadoApplication<BBResumoProcessamentoGuiaExibidaViewModel>();
            try
            {
                var model = await service.BuscarGuiaExibicao(codigoProcesso, codigoLancamento);
                result.DefinirData(mapper.Map<BBResumoProcessamentoGuiaExibidaViewModel>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();

            }
            catch (Exception ex)
            {

                result.ExecutadoComErro(ex);
            }

            return result;
        }


        private BBResumoProcessamentoImportacaoViewModel BuscarDadosImportacaoUpload(BBResumoProcessamentoImportacaoDTO dadosArquivo,
                                                                                     IFormFile file)
        {
            var resultadoVM = mapper.Map<BBResumoProcessamentoImportacaoViewModel>(dadosArquivo);
            resultadoVM.ArquivoGuiasOk = RetornarDadosExportacaoGuiasOK(dadosArquivo);
            resultadoVM.ArquivoGuiasNaoOk = RetornarDadosExportacaoGuiasNaoOK(dadosArquivo);
            resultadoVM.nomeArquivo = file.FileName;
            return resultadoVM;
        }
        public async Task<IResultadoApplication<List<BBResumoProcessamentoImportacaoViewModel>>> Upload(List<IFormFile> files)
        {
            var result = new ResultadoApplication<List<BBResumoProcessamentoImportacaoViewModel>>();
            try
            {
                // Verificando limite máximo no Parametro Juridico
                int limiteQtdArquivosUpload = await service.ConsultarParametroMaxArquivosUpload();
                if (files.Count() > limiteQtdArquivosUpload)
                {
                    throw new TaskCanceledException($"Não podem ser importados mais de {limiteQtdArquivosUpload} arquivos.");
                }

                // Processando arquivos e add na lista para resultado final.
                var listaImportacaoDTO = new List<BBResumoProcessamentoImportacaoViewModel>();
                BBResumoProcessamentoImportacaoDTO importacaoDTO;
                var listaErros = new List<string>();

                foreach (var file in files)
                {
                    if (file.ContentType.ToLower() == "text/plain")
                    {
                        importacaoDTO = await service.RecuperarDadosImportacao(file);
                        if (string.IsNullOrEmpty(importacaoDTO.MsgErro))
                        {
                            var resultadoVM = BuscarDadosImportacaoUpload(importacaoDTO, file);
                            listaImportacaoDTO.Add(resultadoVM);
                        }
                        else
                        {
                            result.ExibeNotificacao = true;
                            listaErros.Add(string.Format(importacaoDTO.MsgErro, file.FileName));
                        }
                    }
                }

                if (listaErros.Count() > 0)
                {
                    result.ExecutadoComErro(string.Join("<br>", listaErros));
                }
                else
                {
                    result.DefinirData(listaImportacaoDTO);
                    result.ExecutadoComSuccesso();
                }
            }
            catch (Exception e)
            {
                result.ExecutadoComErro(e);
                result.ExibeNotificacao = false;
            }
            return result;
        }


        private Task ImportarArquivo(BBResumoProcessamentoImportacaoDTO arquivo)
        {
            if (arquivo.GuiasComProblema != null && arquivo.GuiasComProblema.Count() > 0)
            {
                throw new TaskCanceledException("Não é permitido salvar a importação quando houver guias com problemas.");
            }
            return service.SalvarImportacao(arquivo);
        }
        public async Task<IResultadoApplication> SalvarImportacao(List<BBResumoProcessamentoImportacaoDTO> listaImportacao)
        {
            var result = new ResultadoApplication<BBResumoProcessamentoSalvarImportacaoViewModel>();
            var data = new BBResumoProcessamentoSalvarImportacaoViewModel()
            {
                arquivosSucesso = new List<string>()
            };
            var listaErros = new List<string>();

            try
            {
                foreach (BBResumoProcessamentoImportacaoDTO arquivo in listaImportacao)
                {
                    try
                    {
                        await ImportarArquivo(arquivo);
                        data.arquivosSucesso.Add(arquivo.NomeArquivo); // Utilizado por UX
                    }
                    catch (Exception e)
                    {
                        listaErros.Add($"{arquivo.NomeArquivo} - {e.Message}"); // - {e.Message}
                    }
                }

                if (listaErros.Count() > 0)
                {
                    string msgErro = string.Join("<br>", listaErros);
                    result.ExibirMensagem(msgErro);
                }
                else
                {
                    result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Inclusao);
                }

                result.DefinirData(data);
                result.ExecutadoComSuccesso();
            }
            catch (Exception e)
            {
                result.ExecutadoComErro(e);
            }
            return result;
        }

        private byte[] RetornarDadosExportacaoGuiasNaoOK(BBResumoProcessamentoImportacaoDTO dto)
        {
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
                csv.WriteRecords(mapper.Map<ICollection<BBResumoProcessamentoGuiasComProblemaExportarViewModel>>(dto.GuiasComProblema));
                streamWriter.Flush();
                dados = memoryStream.ToArray();
            }
            return dados;
        }

        private byte[] RetornarDadosExportacaoGuiasOK(BBResumoProcessamentoImportacaoDTO dto)
        {
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
                csv.WriteRecords(mapper.Map<ICollection<BBResumoProcessamentoGuiaExportarViewModel>>(dto.GuiasOk));
                streamWriter.Flush();
                dados = memoryStream.ToArray();
            }
            return dados;
        }

        public async Task<IResultadoApplication<ImportacaoParametroJuridicoUploadViewModel>> ConsultarParametrosUpload()
        {
            var result = new ResultadoApplication<ImportacaoParametroJuridicoUploadViewModel>();
            try
            {
                var data = new ImportacaoParametroJuridicoUploadViewModel
                {
                    QuantidadeMaxArquivosUpload = await service.ConsultarParametroMaxArquivosUpload(),
                    TamanhoMaxArquivosUpload = await service.ConsultarParametroTamanhoArquivosUpload()
                };
                result.DefinirData(data);
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