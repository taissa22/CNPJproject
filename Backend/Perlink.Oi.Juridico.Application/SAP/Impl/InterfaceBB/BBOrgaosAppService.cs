using AutoMapper;
using CsvHelper;
using Perlink.Oi.Juridico.Application.SAP.Interface.InterfaceBB;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB;
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

namespace Perlink.Oi.Juridico.Application.SAP.Impl.InterfaceBB {
    public class BBOrgaosAppService : BaseCrudAppService<BBOrgaosViewModel, BBOrgaos, long>, IBBOrgaosAppService {

        private readonly IBBOrgaosService service;
        private readonly IMapper mapper;
        private readonly IBBComarcaService bBComarcaService;
        private readonly IBBTribunaisService bBTribunaisService;
        private readonly IVaraService varaService;
        public BBOrgaosAppService(IBBOrgaosService service, IMapper mapper, IBBComarcaService bBComarcaService, IBBTribunaisService bBTribunaisService, IVaraService varaService) : base(service, mapper) {
            this.service = service;
            this.mapper = mapper;
            this.bBComarcaService = bBComarcaService;
            this.bBTribunaisService = bBTribunaisService;
            this.varaService = varaService;
        }

        public async Task<IResultadoApplication<BBOrgaosFiltrosViewModel>> CarregarFiltros() {
            var result = new ResultadoApplication<BBOrgaosFiltrosViewModel>();
            try {
                var BBComarcas = await bBComarcaService.RecuperarTodosEmOrdemAlfabetica();
                var BBTribunais = await bBTribunaisService.RecuperarTodosEmOrdemAlfabetica();
                BBOrgaosFiltrosViewModel model = new BBOrgaosFiltrosViewModel() {
                    BBTribunais = mapper.Map<IEnumerable<BBTribunaisComboBoxViewModel>>(BBTribunais),
                    BBComarcas = mapper.Map<IEnumerable<BBComarcaComboBoxViewModel>>(BBComarcas)
                };
                result.DefinirData(model);
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComErro();
            } catch (Exception ex) {
                result.ExecutadoComErro(ex);
            }
            return result;
        }

        public async Task<IPagingResultadoApplication<ICollection<BBOrgaosViewModel>>> ConsultarBBOrgaos(ConsultaBBOrgaosDTO consultaBBOrgaosDTO) {
            var result = new PagingResultadoApplication<ICollection<BBOrgaosViewModel>>();
            try {
                var model = await service.RecuperarPorFiltros(consultaBBOrgaosDTO);

                result.Total = await service.RecuperarTotalRegistros(consultaBBOrgaosDTO);

                result.DefinirData(mapper.Map<ICollection<BBOrgaosViewModel>>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();

            } catch (Exception ex) {
                result.ExecutadoComErro(ex);
            }
            return result;
        }

        public async Task<IResultadoApplication> ExcluirBBOrgaos(long id) {
            var result = new ResultadoApplication();
            try {

                if(await varaService.ExisteBBOrgaoVinculado(id))
                    throw new Exception("Não será possível excluir o Órgão BB selecionado, pois se encontra relacionado com Vara");

                await service.RemoverPorId(id);
                service.Commit();

                result.ExecutadoComSuccesso();
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Exclusao);
            } catch (Exception ex) {
                result.ExecutadoComErro(ex);
                result.ExibeNotificacao = true;
            }
            return result;
        }

        public async Task<IResultadoApplication<byte[]>> ExportarBBOrgaos(ConsultaBBOrgaosDTO consultaBBOrgaosDTO) {
            var result = new ResultadoApplication<byte[]>();
            try {
                //necessario para trazer todos os resultados para os filtros selecionados na ordem default da grid
                consultaBBOrgaosDTO.Pagina = 0;
                consultaBBOrgaosDTO.Quantidade = 0;
                consultaBBOrgaosDTO.Ordenacao = string.Empty;
                consultaBBOrgaosDTO.Ascendente = true;

                var model = await service.RecuperarPorFiltros(consultaBBOrgaosDTO);

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
                    csv.WriteRecords(mapper.Map<ICollection<BBOrgaosExportarViewModel>>(model));
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

        public async Task<IResultadoApplication> SalvarBBOrgaos(BBOrgaosViewModel viewModel) {
            var result = new ResultadoApplication();
            try {
                var entidade = mapper.Map<BBOrgaos>(viewModel);
                await ValidarAsync(entidade);
                if (entidade.Id > 0) {
                    var entidadeNoBanco = await service.RecuperarPorId(entidade.Id);

                    if(entidade.CodigoBBComarca != entidadeNoBanco.CodigoBBComarca &&
                        await varaService.ExisteBBOrgaoVinculado(entidade.Id))
                        throw new Exception("A comarca BB não pode ser alterada, pois no cadastro de Comarcas/Varas do SISJUR, existe um órgão apontando para esta dupla Tribunal BB/Comarca BB.");

                    await service.Atualizar(entidade);
                    result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Alteracao);
                } else {

                    await service.Inserir(entidade);
                    result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Inclusao);
                }

                service.Commit();

                
                result.ExecutadoComSuccesso();
            } catch (Exception ex) {
                result.ExecutadoComErro(ex);
            }
            return result;
        }

        private async Task ValidarAsync(BBOrgaos entidade) {
            if (!entidade.Validar().IsValid) {
                StringBuilder stringBuilder = new StringBuilder();

                foreach (var error in entidade.Validar().Errors) {
                    stringBuilder.AppendLine(error.ErrorMessage);
                }

                throw new Exception(stringBuilder.ToString());
            }
            if (await service.CodgioOrgaoBBExiste(entidade))
                throw new Exception("O Código Órgão BB já está cadastrado em outro registro.");
        }
    }
}