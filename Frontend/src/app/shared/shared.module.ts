import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ListErrorsComponent } from './list-errors.component';
import { ShowAuthedDirective } from './show-authed.directive';
import { FormDebugComponent } from './formulario/form-debug/form-debug.component';
import { TextOverflowDirective } from './diretive/text-overflow.directive';
import { ColLancamentoAliasPipe } from './pipes/col-lancamento-alias.pipe';
import { ToLocalDateIfDatePipe } from './pipes/to-local-date-if-date.pipe';
import { BoolToPTPipe } from './pipes/bool-to-pt.pipe';
import { CpfCnpjPipe } from './pipes/cpf-cnpj.pipe';
import { PisPipe } from './pipes/pis.pipe';
import { StatusAtivoInativoAmbosPipe } from './pipes/statusAtivoInativoAmbos.pipe';
import { NumberPontoHifenDirective } from './numbersPontoHifen.directive';
import { BooleanNullToFalsePipe } from './pipes/booleanNullToFalse.pipe';
import { FiltroAliasPipe } from './pipes/sap/filtro-alias.pipe';
import { FornecedoresManutencaoAliasPipe } from './pipes/sap/fornecedores-manutencao-alias.pipe';
import { EmpresasSAPManutencaoAliasPipe } from './pipes/sap/empresas-sapmanutencao-alias.pipe';
import { EstornoLancamentoTabelaAliasPipe } from './pipes/sap/estorno-lancamento-tabela-alias.pipe';
import { EstornoLancamentoTabelaGarantiasAliasPipe } from './pipes/sap/estorno-lancamento-tabela-garantias-alias.pipe';
import { CategoriaPagamentoPipe } from './pipes/sap/categoriaPagamento.pipe';
import { ManutencaoFormaPagamentoAliasPipe } from './pipes/sap/manutencao-forma-pagamento-alias.pipe';
import { CentrosCustoManutencaoAliaPipe } from './pipes/sap/centrosCustoManutencaoAlia.pipe';
import { ComarcabbAliasPipe } from './pipes/sap/comarcabb-alias.pipe';
import { InterfacebbOrgaosBbAliasPipe } from './pipes/sap/interfacebb-orgaos-bb-alias.pipe';
import { TribunalBBAliasPipe } from './pipes/sap/tribunalBB-alias.pipe';
import { IndicaInstanciaPipe } from './pipes/sap/indicaInstancia.pipe';
import { NaturezaBBAliasPipe } from './pipes/sap/naturezaBB-alias.pipe';
import { ModalidadeProdutoBbAliasPipe } from './pipes/sap/modalidade-produto-bb-alias.pipe';
import { StatusParcelaBBAliasPipe } from './pipes/sap/status-parcela-bbalias.pipe';
import { NoStartWithSpaceDirective } from './noStartWithSpace.directive';
import { FornecedoresContingenciaSapAliasPipe } from './pipes/sap/fornecedores-contingencia-sap-alias.pipe';
import { GrupoLoteJuizadoAliasPipe } from './pipes/sap/grupo-lote-juizado-alias.pipe';
import { NumberDirective } from './numbers-only.directive';
import { GuiaComProblemasPipe } from './pipes/sap/guia-com-problemas.pipe';
import { CriterioPesquisaPipe } from './pipes/criterio-pesquisa.pipe';
import { SaldoGarantiaResultadoAliasPipe } from './pipes/sap/saldo-garantia-resultado-alias.pipe';
import { PageHeaderComponent } from './layout/page-header/page-header.component';
import { NgbdSortableHeader } from './directive/sortable.directive';
import { NumberDataDirective } from './diretive/numberData.directive';
import { CadastrosCompartilhadosJurosAliasPipe } from './pipes/manutencao/cadastros-compartilhados-juros-alias.pipe';
import { GuiasOkAliasPipe } from './pipes/sap/importacao-arquivo-bb-alias.pipe';
import { FiltroBuscaGrupoPorEmpresa } from './pipes/relatorios/filtro-busca-grupo-por-empresa.pipe';
import { FiltroBuscaEmListaDinamico } from './pipes/relatorios/filtro-busca-em-lista-dinamico.pipe';

// angular
import { FlexLayoutModule } from '@angular/flex-layout';

// 3rd party
import { NgSelectModule } from '@ng-select/ng-select';

// components
import { JurPaginator } from './components/jur-paginator/jur-paginator.component';
import { JurTable } from './components/jur-table/jur-table.component';
import { JurTableHeader } from './components/jur-table/jur-table-header/jur-table-header.component';
import { JurColumnTemplate } from './components/jur-table/jur-column-template/jur-column-template.directive';
import { JurRowData } from './components/jur-table/jur-row-data/jur-row-data.directive';
import { CadastrosCompartilhadosTipoAudienciaAliasPipe } from './pipes/sap/cadastrosCompartilhadosTipoAudienciaAlias.pipe';
import { FiltroBuscaGrupoPorEstado } from "./pipes/fechamento/filtro-busca-grupo-por-estados.pipe";

import { JurSelectablePanel } from './components/jur-selectable-panel/jur-selectable-panel.component';

import { PercentualAtmAliasPipe } from "./pipes/manutencao/percentual-atm-alias.pipe";
import { PadStartPipe } from './pipes/pad-start.pipe';
import { TwoDigitDecimaNumberDirectiveDirective } from './two-digit-decima-number-directive.directive';
import { PesquisaHeaderComponent } from '../componentes/pesquisa-header/pesquisa-header.component';
import { ComponentesModule } from '../componentes/componentes.module';
import { SafeHtmlPipe } from './pipes/safe-html.pipe';

import { PercentNumberDirectiveDirective } from './percent-number-directive.directive';
import { NumbersLimitHundredDirective } from './numbers-limit-hundred.directive';
@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    RouterModule,

    // angular
    FlexLayoutModule,

    // 3rd party
    NgSelectModule
  ],
  declarations: [
    ListErrorsComponent,
    ShowAuthedDirective,
    FormDebugComponent,
    ColLancamentoAliasPipe,
    TextOverflowDirective,
    FiltroAliasPipe,
    ToLocalDateIfDatePipe,
    FornecedoresManutencaoAliasPipe,
    EmpresasSAPManutencaoAliasPipe,
    BoolToPTPipe,
    EstornoLancamentoTabelaAliasPipe,
    EstornoLancamentoTabelaGarantiasAliasPipe,
    CpfCnpjPipe,
    PisPipe,
    BoolToPTPipe,
    CategoriaPagamentoPipe,
    StatusAtivoInativoAmbosPipe,
    NumberPontoHifenDirective,
    ManutencaoFormaPagamentoAliasPipe,
    CentrosCustoManutencaoAliaPipe,
    BooleanNullToFalsePipe,
    ComarcabbAliasPipe,
    InterfacebbOrgaosBbAliasPipe,
    TribunalBBAliasPipe,
    IndicaInstanciaPipe,
    ModalidadeProdutoBbAliasPipe,
    NaturezaBBAliasPipe,
    FornecedoresContingenciaSapAliasPipe,
    StatusParcelaBBAliasPipe,
    NoStartWithSpaceDirective,
    GrupoLoteJuizadoAliasPipe,
    NumberDirective,
    GuiasOkAliasPipe,
    GuiaComProblemasPipe,
    CriterioPesquisaPipe,
    SaldoGarantiaResultadoAliasPipe,
    PageHeaderComponent,
    NgbdSortableHeader,
    NumberDataDirective,
    CadastrosCompartilhadosJurosAliasPipe,
    PercentualAtmAliasPipe,
    // components
    JurPaginator,
    JurTable,
    JurTableHeader,
    JurColumnTemplate,
    JurRowData,
    CadastrosCompartilhadosTipoAudienciaAliasPipe,
    FiltroBuscaGrupoPorEmpresa,
    FiltroBuscaEmListaDinamico,
    JurSelectablePanel,
    FiltroBuscaGrupoPorEstado,
    PadStartPipe,

    TwoDigitDecimaNumberDirectiveDirective,
    PesquisaHeaderComponent,
    SafeHtmlPipe,
    PercentNumberDirectiveDirective,
    NumbersLimitHundredDirective
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    ListErrorsComponent,
    RouterModule,
    ShowAuthedDirective,
    FormDebugComponent,
    ColLancamentoAliasPipe,
    TextOverflowDirective,
    FiltroAliasPipe,
    ToLocalDateIfDatePipe,
    FornecedoresManutencaoAliasPipe,
    EmpresasSAPManutencaoAliasPipe,
    CpfCnpjPipe,
    BoolToPTPipe,
    EstornoLancamentoTabelaAliasPipe,
    EstornoLancamentoTabelaGarantiasAliasPipe,
    PisPipe,
    BoolToPTPipe,
    CentrosCustoManutencaoAliaPipe,
    CategoriaPagamentoPipe,
    StatusAtivoInativoAmbosPipe,
    NumberPontoHifenDirective,
    ManutencaoFormaPagamentoAliasPipe,
    BooleanNullToFalsePipe,
    ModalidadeProdutoBbAliasPipe,
    ComarcabbAliasPipe,
    InterfacebbOrgaosBbAliasPipe,
    TribunalBBAliasPipe,
    IndicaInstanciaPipe,
    NaturezaBBAliasPipe,
    StatusParcelaBBAliasPipe,
    NoStartWithSpaceDirective,
    FornecedoresContingenciaSapAliasPipe,
    StatusParcelaBBAliasPipe,
    NoStartWithSpaceDirective,
    TwoDigitDecimaNumberDirectiveDirective,
    PercentNumberDirectiveDirective,
    NumbersLimitHundredDirective,
    GrupoLoteJuizadoAliasPipe,
    GuiasOkAliasPipe,
    NumberDirective,
    GuiaComProblemasPipe,
    CriterioPesquisaPipe,
    SaldoGarantiaResultadoAliasPipe,
    FiltroBuscaGrupoPorEstado,
    PageHeaderComponent,
    NgbdSortableHeader,
    NumberDataDirective,
    CadastrosCompartilhadosJurosAliasPipe,
    FiltroBuscaGrupoPorEmpresa,
    FiltroBuscaEmListaDinamico,
    PercentualAtmAliasPipe,
    PesquisaHeaderComponent,
    SafeHtmlPipe,


    // angular
    FlexLayoutModule,

    // 3rd party
    NgSelectModule,

    // components
    JurPaginator,
    JurTable,
    JurColumnTemplate,
    JurRowData,
    JurSelectablePanel,

    CadastrosCompartilhadosTipoAudienciaAliasPipe,
    PadStartPipe
  ]
})
export class SharedModule { }
