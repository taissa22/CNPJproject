using AutoMapper;
using CsvHelper;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
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
    public class GruposLotesJuizadosAppService : BaseCrudAppService<GruposLotesJuizadosViewModel, GruposLotesJuizados, long>, IGruposLotesJuizadosAppService
    {
        private readonly IGruposLotesJuizadosService service;
        private readonly IProfissionalService profissionalService;
        private readonly IMapper mapper;

        public GruposLotesJuizadosAppService(IGruposLotesJuizadosService service, 
                                             IProfissionalService profissionalService, 
                                             IMapper mapper) : base(service, mapper)
        {
            this.mapper = mapper;
            this.service = service;
            this.profissionalService = profissionalService;
        }

        public async Task<IPagingResultadoApplication<ICollection<GruposLotesJuizadosViewModel>>> ConsultarGruposLotesJuizadosPorFiltroPaginado(FiltrosDTO filtros)
        {
            var result = new PagingResultadoApplication<ICollection<GruposLotesJuizadosViewModel>>();

            try
            {
                var model = await service.RecuperarGrupoLoteJuizadoPorFiltro(filtros);

                if (filtros.Total <= 0) {
                    result.Total = await service.ObterQuantidadeTotalPorFiltro(filtros);
                } else {
                    if (Math.Floor(Convert.ToDecimal(filtros.Total / filtros.Quantidade)) == filtros.Pagina) {
                        result.Total = await service.ObterQuantidadeTotalPorFiltro(filtros);
                    } else
                        result.Total = filtros.Total;
                }


                result.DefinirData(mapper.Map<ICollection<GruposLotesJuizadosViewModel>>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (System.Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<long> ObterUltimoId()
        {
            return await service.ObterUltimoId();
        }

        public async Task<IResultadoApplication> ExcluirGruposLotesJuizados(long codigo)
        {
            var resultado = new ResultadoApplication();
            try
            {
                if (await profissionalService.ExisteGrupoLoteJuizadoComEscritorio(codigo))
                {
                    resultado.ExibeNotificacao = true;
                    throw new Exception("Não será possível excluir o grupo de lote de juizado selecionado, pois se encontra relacionado com um escritório");
                }
               
                await service.RemoverPorId(codigo);
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

        public async Task<IResultadoApplication<byte[]>> ExportarGruposLotesJuizado(FiltrosDTO filtros)
        {
            var result = new ResultadoApplication<byte[]>();
            try
            {
                
                filtros.Pagina = 0;
                filtros.Total = 0;
                filtros.Quantidade = 0;

                var lista = await service.RecuperarGrupoLoteJuizadoPorFiltro(filtros);
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
                    csv.WriteRecords(mapper.Map<ICollection<GruposLotesJuizadosExportarViewModel>>(lista));
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
