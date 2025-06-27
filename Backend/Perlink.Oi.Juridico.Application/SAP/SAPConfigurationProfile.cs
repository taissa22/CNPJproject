using AutoMapper;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.Filtros;
using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.AgendaAudiencia;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.LoteCriacao;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.ManutencaoCategoriaPagamento;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.ManutencaoCentroCusto;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.ManutencaoFormaPagamento;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.ManutençãoFornecedorContigencia;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.SaldoGarantia;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.VariosContextos;
using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoCategoriaPagamento;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoFormaPagamento;
using Shared.Application.Conversores;
using static Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB.BBModalidadeViewModel;
using static Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB.BBNaturezasAcoesViewModel;
using static Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB.BBStatusParcelasViewModel;
using static Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB.BBTribunaisViewModel;

namespace Perlink.Oi.Juridico.Application.SAP
{
    public class SAPConfigurationProfile : Profile
    {
        public SAPConfigurationProfile()
        {
            Configuracao.Registrar(this);
            AllowNullCollections = true;

            //colocar em ordem alfabetica para facilitar a busca
            AgendamentoResultadoViewModel.Mapping(this);
            
            BBStatusParcelasViewModel.Mapping(this);
            BBStatusParcelasExportarViewModel.Mapping(this);
            BBNaturezasAcoesViewModel.Mapping(this);
            BBNaturezasAcoesExportarViewModel.Mapping(this);
            BBTribunaisViewModel.Mapping(this);
            BBTribunaisExportarViewModel.Mapping(this);
            BBTributarioInclusaoEdicaoDTO.Mapping(this);           
            BorderoViewModel.Mapping(this);
            BorderoExportarViewModel.Mapping(this);
            CategoriaPagamentoComboboxViewModel.Mapping(this);
            CategoriaPagamentoPopupComboboxViewModel.Mapping(this);
            CategoriaPagamentoViewModel.Mapping(this);
            ComarcaViewModel.Mapping(this);
            ComarcaComboViewModel.Mapping(this);
            ComboboxViewModel<int>.Mapping(this);
            CriterioSaldoGarantiaResultadoViewModel.Mapping(this);
            BBStatusParcelaInclusaoEdicaoDTO.Mapping(this);
            ImportacaoParametroJuridicoUploadViewModel.Mapping(this);
            LoteViewModel.Mapping(this);
            LoteExportarViewModel.Mapping(this);
            LoteExportarToCEorTrabViewModel.Mapping(this);
            LoteResultadoViewModel.Mapping(this);
            LoteDetalhesViewModel.Mapping(this);
            Log_LoteViewModel.Mapping(this);
            Log_LoteHistoricoViewModel.Mapping(this);
            LoteCanceladoViewModel.Mapping(this);
            LoteCriacaoResultadoEmpresaCentralizadoraViewModel.Mapping(this);
            LoteCriacaoResultadoEmpresaGrupoViewModel.Mapping(this);
            LoteCriacaoResultadoLancamentoViewModel.Mapping(this);
            LoteCriacaoViewModel.Mapping(this);
            FormaPagamentoGridViewModel.Mapping(this);
            FormaPagamentoGridExportarViewModel.Mapping(this);
            FormaPagamentoInclusaoEdicaoDTO.Mapping(this);
            FornecedorViewModel.Mapping(this);
            FornecedorResultadoViewModel.Mapping(this);
            FornecedorExportarViewModel.Mapping(this);
            FornecedorAtualizarViewModel.Mapping(this);
            FornecedorContigenciaResultadoViewModel.Mapping(this);
            FornecedorContigenciaExportarViewModel.Mapping(this);
            FornecedorContigenciaAtualizaViewModel.Mapping(this);
            GruposLotesJuizadosExportarViewModel.Mapping(this);
            GruposLotesJuizadosViewModel.Mapping(this);
            Empresas_SapViewModel.Mapping(this);
            Empresas_SapResultadoViewModel.Mapping(this);
            Empresas_SapExportarViewModel.Mapping(this);
            CategoriaDePagamentoResultadoViewModel.Mapping(this);
            CategoriaPagamentoPadraoViewModel.Mapping(this);
            CategoriaPagamentoJuizadoDespViewModel.Mapping(this);
            CategoriaPagamentoCCGarantiasViewModel.Mapping(this);
            CategoriaPagamentoBasicoFinalzContabilViewModel.Mapping(this);
            CategoriaPagamentoCCPagViewModel.Mapping(this);
            CategoriaPagamentoCEDespesasViewModel.Mapping(this);
            CategoriaPagamentoBasicoViewModel.Mapping(this);
            CategoriaPagamentoBasicoClassGarantiaViewModel.Mapping(this);
            CategoriaPagamentoCE_Pag_Gar_ViewModel.Mapping(this);
            CategoriaPagamentoTrabGarantiasViewModel.Mapping(this);
            CategoriaPagamentoTrabDespesasJudViewModel.Mapping(this);
            CategoriaPagamentoTrabPagViewModel.Mapping(this);
            CategoriaPagamentoJuizadoGarantiaViewModel.Mapping(this);
            CategoriaPagamentoJuizadoPagViewModel.Mapping(this);
            CategoriaPagamentoBasicoAdmViewModel.Mapping(this);
            CategoriaPagamentoProconDespesaViewModel.Mapping(this);
            CategoriaPagamentoPexDespesasJudViewModel.Mapping(this);
            CategoriaPagamentoPexGarantiasViewModel.Mapping(this);
            CategoriaPagamentoPexHonoViewModel.Mapping(this);
            CategoriaPagamentoPexPagamentosViewModel.Mapping(this);
            CategoriaPagamentoInclusaoEdicaoDTO.Mapping(this);
            CentroCustoViewModel.Mapping(this);
            CentroCustoExportarViewModel.Mapping(this);
            LoteCriteriosFiltrosViewModel.Mapping(this);
            SaldoGarantiaFiltrosViewModel.Mapping(this);
            BBComarcaViewModel.Mapping(this);
            BBComarcaComboBoxViewModel.Mapping(this);
            BBTribunaisComboBoxViewModel.Mapping(this);
            BBOrgaosViewModel.Mapping(this);
            BBComarcaExportarViewModel.Mapping(this);
            BBModalidadeViewModel.Mapping(this);
            SaldoGarantiaResultadoViewModel.Mapping(this);
            SaldoGarantiaExportacaoCCViewModel.Mapping(this);
            SaldoGarantiaExportacaoJECViewModel.Mapping(this);
            SaldoGarantiaExportacaoTrabalhistaViewModel.Mapping(this);
            SaldoGarantiaExportacaoCEViewModel.Mapping(this);
            SaldoGarantiaExportacaoTributarioJudicialViewModel.Mapping(this);
            SaldoGarantiaExportacaoTributarioADMViewModel.Mapping(this);
            BBModalidadeExportarViewModel.Mapping(this);
            BBOrgaosExportarViewModel.Mapping(this);
            BBResumoProcessamentoViewModel.Mapping(this);
            BBResumoProcessamentoGuiaViewModel.Mapping(this);
            BBResumoProcessamentoResultadoViewModel.Mapping(this);
            BBResumoProcessamentoGuiaExportarViewModel.Mapping(this);
            BBResumoProcessamentoGuiasComProblemaViewModel.Mapping(this);
            BBResumoProcessamentoImportacaoViewModel.Mapping(this);
            BBResumoProcessamentoGuiasComProblemaExportarViewModel.Mapping(this);
            BBResumoProcessamentoGuiaExibidaViewModel.Mapping(this);
            BBResumoProcessamentoResultadoExportarViewModel.Mapping(this);
            CategoriaPagamentoPadraoConsumidorViewModel.Mapping(this);
            CategoriaPagamentoCEGarantiasConsumidorViewModel.Mapping(this);
            CategoriaPagamentoBasicoFinalzContabilConsumidorViewModel.Mapping(this);
            CategoriaPagamentoCCPagConsumidorViewModel.Mapping(this);
            CompromissoLoteViewModel.Mapping(this);
        }
    }
}