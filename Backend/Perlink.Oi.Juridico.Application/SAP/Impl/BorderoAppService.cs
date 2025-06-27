using AutoMapper;
using CsvHelper;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Enum;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
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

namespace Perlink.Oi.Juridico.Application.SAP.Impl {
    public class BorderoAppService : BaseCrudAppService<BorderoViewModel, Bordero, long>, IBorderoAppService
    {
        private readonly IBorderoService service;
        private readonly IMapper mapper;
        private readonly IPermissaoService permissaoService;
        public BorderoAppService(IBorderoService service, IMapper mapper, IPermissaoService permissaoService) : base(service, mapper)
        {
            this.service = service;
            this.mapper = mapper;
            this.permissaoService = permissaoService;
        }

        public async Task<IResultadoApplication<byte[]>> ExportarBorderoDoLote(long codigoLote, long codigoTipoProcesso) {
            var result = new ResultadoApplication<byte[]>();
            try {

                if ((!permissaoService.TemPermissao(PermissaoEnum.f_ExportarLotesCivelCons) &&
                    codigoTipoProcesso == TipoProcessoEnum.CivelConsumidor.GetHashCode()) ||
                    (!permissaoService.TemPermissao(PermissaoEnum.f_ExportarLotesEstrat) &&
                    codigoTipoProcesso == TipoProcessoEnum.CivelEstrategico.GetHashCode()) ||
                    (!permissaoService.TemPermissao(PermissaoEnum.f_ExportarLotesJuizado) &&
                    codigoTipoProcesso == TipoProcessoEnum.JuizadoEspecial.GetHashCode()) ||
                    (!permissaoService.TemPermissao(PermissaoEnum.f_ExportarLotesPex) &&
                    codigoTipoProcesso == TipoProcessoEnum.Pex.GetHashCode()) ||
                    (!permissaoService.TemPermissao(PermissaoEnum.f_ExportarLotesTrabalhista) &&
                    codigoTipoProcesso == TipoProcessoEnum.Trabalhista.GetHashCode()))
                    throw new Exception("O usuário não possui permissão para exportar Borderôs lote.");

                var lista = await service.GetBordero(codigoLote);
                byte[] dados;
                var csvConfiguration = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.GetCultureInfo("pt-BR")) {
                    Delimiter = ";",
                    TrimOptions = CsvHelper.Configuration.TrimOptions.Trim,
                    HasHeaderRecord = true,
                    IgnoreBlankLines = true
                };
                using (var memoryStream = new MemoryStream())
                using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                using (var csv = new CsvWriter(streamWriter, csvConfiguration)) {

                    csv.WriteRecords(mapper.Map<ICollection<BorderoExportarViewModel>>(lista));

                    streamWriter.Flush();
                    dados = memoryStream.ToArray();
                }
                result.DefinirData(dados);
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            } catch (Exception ex) {

                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IResultadoApplication<ICollection<BorderoViewModel>>> GetBordero(long CodigoLote)
        {
            var result = new ResultadoApplication<ICollection<BorderoViewModel>>();

            try
            {
                var model = await service.GetBordero(CodigoLote);
                result.DefinirData(mapper.Map<ICollection<BorderoViewModel>>(model)); ;
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
