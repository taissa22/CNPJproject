import { FiltroNumeroDoLoteComponent } from './consultaLote/consultaLote-filtros/filtro-numero-do-lote/filtro-numero-do-lote.component';
import { Injector, LOCALE_ID, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbActiveModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from '@shared/shared.module';
import { CurrencyMaskModule } from 'ng2-currency-mask';
// tslint:disable-next-line: max-line-length
import { ModalModule } from "ngx-bootstrap";
import { InfiniteScrollModule } from "ngx-infinite-scroll";
import { InputMaskModule } from '@libs/racoon-mask/input-mask.module';;
import { ComponentesModule } from "../componentes/componentes.module";
import { RoleGuardService } from "../core/services/Roles.guard.ts.service";
import { BuscarAgendamentosComponent } from "./consulta/consulta-saldo-garantia/buscar-agendamentos/buscar-agendamentos.component";
import { ConsultaSaldoGarantiaComponent } from "./consulta/consulta-saldo-garantia/consulta-saldo-garantia.component";
import { SaldoGarantiaBancoComponent } from "./consulta/consulta-saldo-garantia/filtros/saldo-garantia-banco/saldo-garantia-banco.component";
import { SaldoGarantiaCriteriosGeraisComponent } from "./consulta/consulta-saldo-garantia/filtros/saldo-garantia-criterios-gerais/saldo-garantia-criterios-gerais.component";
import { SaldoGarantiaEmpresaGrupoComponent } from "./consulta/consulta-saldo-garantia/filtros/saldo-garantia-empresa-grupo/saldo-garantia-empresa-grupo.component";
import { SaldoGarantiaEstadoComponent } from "./consulta/consulta-saldo-garantia/filtros/saldo-garantia-estado/saldo-garantia-estado.component";
import { SaldoGarantiaProcessosComponent } from "./consulta/consulta-saldo-garantia/filtros/saldo-garantia-processos/saldo-garantia-processos.component";
import { ModalAdicionarAgendamentoComponent } from "./consulta/consulta-saldo-garantia/modal-adicionar-agendamento/modal-adicionar-agendamento.component";
import { ConsultaPageComponent } from "./consultaLote/consulta-page/consulta-page.component";
import { FiltroCategoriaPagamentoComponent } from "./consultaLote/consultaLote-filtros/filtro-categoria-pagamento/filtro-categoria-pagamento.component";
import { FiltroCentroCustoComponent } from "./consultaLote/consultaLote-filtros/filtro-centro-custo/filtro-centro-custo.component";
import { filtrodataComponent } from "./consultaLote/consultaLote-filtros/filtro-data/filtro-data.component";
import { FiltroEmpresaGrupoComponent } from "./consultaLote/consultaLote-filtros/filtro-empresa-grupo/filtro-empresa-grupo.component";
import { FiltroEscritorioComponent } from "./consultaLote/consultaLote-filtros/filtro-escritorio/filtro-escritorio.component";
import { FiltroFornecedorComponent } from "./consultaLote/consultaLote-filtros/filtro-fornecedor/filtro-fornecedor.component";
import { FiltroGuiaComponent } from "./consultaLote/consultaLote-filtros/filtro-guia/filtro-guia.component";
import { FiltroContaJudicialsComponent } from "./consultaLote/consultaLote-filtros/filtro-numero-conta-judicial/filtro-numero-conta-judicial.component";
import { FiltroProcessosComponent } from "./consultaLote/consultaLote-filtros/filtro-processos/filtro-processos.component";
import { FiltroSapComponent } from "./consultaLote/consultaLote-filtros/filtro-sap/filtro-sap.component";
import { FiltroStatusLancamentoComponent } from "./consultaLote/consultaLote-filtros/filtro-status-lancamento/filtro-status-lancamento.component";
import { FiltroTipoLancamentoComponent } from "./consultaLote/consultaLote-filtros/filtro-tipo-lancamento/filtro-tipo-lancamento.component";
import { DetalhamentoResultadoComponent } from "./consultaLote/resultado-sap/detalhamento-resultado/detalhamento-resultado.component";
import { lancamentohistoricoComponent } from "./consultaLote/resultado-sap/detalhamento-resultado/lancamento-historico/lancamento-historico.component";
import { ModalAlteracaoDataGuiaComponent } from "./consultaLote/resultado-sap/modal-alteracao-data-guia/modal-alteracao-data-guia.component";
import { ResultadoSapComponent } from "./consultaLote/resultado-sap/resultado-sap.component";
import { FiltrocriacaoLotesComponent } from "./criacaoLote/filtro-criacao-lotes/filtro-criacao-lotes.component";
// tslint:disable-next-line: max-line-length
import { AbasContentComponent } from "./criacaoLote/resultado-criacao/lotes-container/detalhamento-lancamento/abasContent/abasContent.component";
// tslint:disable-next-line: max-line-length
import { DetalhamentoLancamentoComponent } from "./criacaoLote/resultado-criacao/lotes-container/detalhamento-lancamento/detalhamento-lancamento.component";
// tslint:disable-next-line: max-line-length
import { BorderoCadastroComponent } from "./criacaoLote/resultado-criacao/lotes-container/detalhamento-lancamento/form-bordero/bordero-cadastro.component/bordero-cadastro.component";
// tslint:disable-next-line: max-line-length
import { LabelsLoteComponent } from "./criacaoLote/resultado-criacao/lotes-container/detalhamento-lancamento/labels-lote/labels-lote.component";
import { LotesContainerComponent } from "./criacaoLote/resultado-criacao/lotes-container/lotes-container.component";
import { ResultadoCriacaoComponent } from "./criacaoLote/resultado-criacao/resultado-criacao.component";
// tslint:disable-next-line: max-line-length
import { TabelaComponent } from "./criacaoLote/tabela/tabela.component";
import { TextareaLimitadoComponent } from "./criacaoLote/textarea-limitado/textarea-limitado.component";
import { FiltrosModule } from "./filtros/filtros.module";
import { ComarcaTribunaisResolverGuard } from "./guards/comarcaTribunaisResolverGuard";
import { DeactivateGuard } from "./guards/DeactivateGuard.guard";
import { ManutencaoFornecedorBancoResolver } from "./guards/manutencao-fornecedores-banco-resolver.guard";
import { ManutencaoFornecedorEscritorioResolver } from "./guards/manutencao-fornecedores-escritorio-resolver.guard";
import { ManutencaoFornecedorProfissionaisResolver } from "./guards/manutencao-fornecedores-profissionais-resolver.guard";
import { SapGuard } from "./guards/sap.guard";
import { TipoProcessoResolverGuard } from "./guards/tipo-processo-resolver.guard";
import { InterfacesBBComarcaBBComponent } from "./interfacesBB/comarcaBB/interfacesBB-comarcaBB.component";
import { ModalAdicionarComarcaBBComponent } from "./interfacesBB/comarcaBB/modal-adicionar-comarcaBB/modal-adicionar-comarcaBB.component";
import { ConsultaArquivoRetornoComponent } from "./interfacesBB/importacao-arquivo-retorno/consulta-arquivo-retorno/consulta-arquivo-retorno.component";
import { ConsultaArquivoRetornoResolver } from "./interfacesBB/importacao-arquivo-retorno/guards/resolver/consulta-arquivo-retorno-resolver.guards";
import { ImportarArquivoComponent } from "./interfacesBB/importacao-arquivo-retorno/importar-arquivo/importar-arquivo.component";
import { DetalheGuiaImportacaoArquivoRetornoComponent } from "./interfacesBB/importacao-arquivo-retorno/resultado-arquivo-retorno/detalhe-guia-importacao-arquivo-retorno/detalhe-guia-importacao-arquivo.component";
import { ImagemGuiaComponent } from "./interfacesBB/importacao-arquivo-retorno/resultado-arquivo-retorno/detalhe-guia-importacao-arquivo-retorno/imagem-guia/imagem-guia.component";
import { ResultadoArquivoRetornoComponent } from "./interfacesBB/importacao-arquivo-retorno/resultado-arquivo-retorno/resultado-arquivo-retorno.component";
import { ModalAdicionarModalidadeBbComponent } from "./interfacesBB/modalidade-produto-bb/modal-adicionar-modalidade-bb/modal-adicionar-modalidade-bb.component";
import { ModalidadeProdutoBbComponent } from "./interfacesBB/modalidade-produto-bb/modalidade-produto-bb.component";
import { ModalAdicionarNaturezaBBComponent } from "./interfacesBB/natureza-bb/modal-adicionar-naturezaBB/modal-adicionar-naturezaBB.component";
import { NaturezaBbComponent } from "./interfacesBB/natureza-bb/natureza-bb.component";
import { FiltroOrgaoBbComponent } from "./interfacesBB/orgaoBB/filtro-orgao-bb/filtro-orgao-bb.component";
import { ModalAdicionarOrgaoBBComponent } from "./interfacesBB/orgaoBB/modal-adicionar-orgao-bb/modal-adicionar-orgao-bb.component";
import { OrgaoBBComponent } from "./interfacesBB/orgaoBB/orgaoBB.component";
import { ModalAdicionarStatusParcelaComponent } from "./interfacesBB/status-parcela-bb/modal-adicionar-status-parcela/modal-adicionar-status-parcela.component";
import { StatusParcelaBBComponent } from "./interfacesBB/status-parcela-bb/status-parcela-bb.component";
import { ModalTribunaisBbComponent } from "./interfacesBB/tribunaisBB/modal-tribunais-bb/modal-tribunais-bb.component";
import { TribunaisBBComponent } from "./interfacesBB/tribunaisBB/tribunaisBB.component";
import { EstornoLancamentosPagosComponent } from "./lotes/estorno-lancamentos-pagos/estorno-lancamentos-pagos.component";
import { DespesaTabComponent } from "./lotes/estorno-lancamentos-pagos/estorno-resultado/estorno-abas/despesa-tab/despesa-tab.component";
import { EstornoAbasComponent } from "./lotes/estorno-lancamentos-pagos/estorno-resultado/estorno-abas/estorno-abas.component";
import { GarantiasTabComponent } from "./lotes/estorno-lancamentos-pagos/estorno-resultado/estorno-abas/garantias-tab/garantias-tab.component";
import { PagamentosTabComponent } from "./lotes/estorno-lancamentos-pagos/estorno-resultado/estorno-abas/pagamentos-tab/pagamentos-tab.component";
import { EstornoResultadoComponent } from "./lotes/estorno-lancamentos-pagos/estorno-resultado/estorno-resultado.component";
import { ModalPartesComponent } from "./lotes/estorno-lancamentos-pagos/estorno-resultado/modal-partes/modal-partes.component";
import { ModalPedidosComponent } from "./lotes/estorno-lancamentos-pagos/estorno-resultado/modal-partes/modal-pedidos/modal-pedidos.component";
import { ManutencaoCategoriaPagamentoComponent } from "./manutenção/manutencao-categoria-pagamento/manutencao-categoria-pagamento.component";
import { ModalAdicionarDespesasJudiciaisComponent } from "./manutenção/manutencao-categoria-pagamento/modal-adicionar-despesas-judiciais/modal-adicionar-despesas-judiciais.component";
import { ModalAdicionarGarantiasComponent } from "./manutenção/manutencao-categoria-pagamento/modal-adicionar-garantias/modal-adicionar-garantias.component";
import { ModalAdicionarHonorariosComponent } from "./manutenção/manutencao-categoria-pagamento/modal-adicionar-honorarios/modal-adicionar-honorarios.component";
import { ModalAdicionarPagamentosComponent } from "./manutenção/manutencao-categoria-pagamento/modal-adicionar-pagamentos/modal-adicionar-pagamentos.component";
import { ModalAdicionarRecuperacaoPagamentoComponent } from "./manutenção/manutencao-categoria-pagamento/modal-adicionar-recuperacao-pagamento/modal-adicionar-recuperacao-pagamento.component";
import { ManutencaoCentroCustoComponent } from "./manutenção/manutencao-centro-custo/manutencao-centro-custo.component";
import { ModalAdicionarCentrosCustoComponent } from "./manutenção/manutencao-centro-custo/modal-adicionar-centros-custo/modal-adicionar-centros-custo.component";
import { ManutencaoEmpresasSapComponent } from "./manutenção/manutencao-empresas-sap/manutencao-empresas-sap.component";
import { ModalAdicionarEmpresasSapComponent } from "./manutenção/manutencao-empresas-sap/modal-adicionar-empresas-sap/modal-adicionar-empresas-sap.component";
import { ManutencaoFormasPagamentoComponent } from "./manutenção/manutencao-formas-pagamento/manutencao-formas-pagamento.component";
import { ModalFormasPagamentoComponent } from "./manutenção/manutencao-formas-pagamento/modal-formas-pagamento/modal-formas-pagamento.component";
import { FiltroFornecedoresContingenciaSapComponent } from "./manutenção/manutencao-fornecedores-contingencia-sap/filtro-fornecedores-contingencia-sap/filtro-fornecedores-contingencia-sap.component";
import { ManutencaoFornecedoresContingenciaSapComponent } from "./manutenção/manutencao-fornecedores-contingencia-sap/manutencao-fornecedores-contingencia-sap.component";
import { ModalEditarFornecedoresContingenciaSapComponent } from "./manutenção/manutencao-fornecedores-contingencia-sap/modal-editar-fornecedores-contingencia-sap/modal-editar-fornecedores-contingencia-sap.component";
import { FiltroFornecedoresComponent } from "./manutenção/manutencao-fornecedores/filtro-fornecedores/filtro-fornecedores.component";
import { ManutencaoFornecedoresComponent } from "./manutenção/manutencao-fornecedores/manutencao-fornecedores.component";
import { ModalAdicionarFornecedoresComponent } from "./manutenção/manutencao-fornecedores/modal-adicionar-fornecedores/modal-adicionar-fornecedores.component";
import { ManutencaoGrupoLoteJuizadoComponent } from "./manutenção/manutencao-grupo-lote-juizado/manutencao-grupo-lote-juizado.component";
import { ModalAdicionarGrupoLoteJuizadoComponent } from "./manutenção/manutencao-grupo-lote-juizado/modal-adicionar-grupo-lote-juizado/modal-adicionar-grupo-lote-juizado.component";
import { SapRoutingModule } from "./sap-routing.module";
import { CommonModule } from '@angular/common';
import { OnlyNumberNoStartWith0Directive } from '@shared/pipes/onlyNumberNoStartWith0.directive';
import { MigracaoPedidosSapComponent } from './migracao-pedidos-sap/migracao-pedidos-sap.component';
import { PainelMigracaoPedidosSapComponent } from './migracao-pedidos-sap/painel-migracao-pedidos-sap/painel-migracao-pedidos-sap.component';
import { ListagemMigracaoPedidosSapComponent } from './migracao-pedidos-sap/listagem-migracao-pedidos-sap/listagem-migracao-pedidos-sap.component';
import { InstrucoesMigracaoPedidosSapComponent } from './migracao-pedidos-sap/instrucoes-migracao-pedidos-sap/instrucoes-migracao-pedidos-sap.component';
import { StaticInjector } from './static-injector';


@NgModule({
  imports: [
    SharedModule,
    FormsModule,
    SapRoutingModule,
    NgbModule,
    ModalModule.forRoot(),
    ComponentesModule,
    InputMaskModule,
    CurrencyMaskModule,
    ReactiveFormsModule,
    CommonModule,
    InfiniteScrollModule,
    FiltrosModule,
  ],
  declarations: [
    filtrodataComponent,
    ConsultaPageComponent,
    FiltroGuiaComponent,
    FiltroSapComponent,
    ResultadoSapComponent,
    lancamentohistoricoComponent,
    FiltrocriacaoLotesComponent,
    DetalhamentoResultadoComponent,
    ResultadoCriacaoComponent,
    OnlyNumberNoStartWith0Directive,
    LotesContainerComponent,
    AbasContentComponent,
    DetalhamentoLancamentoComponent,
    LabelsLoteComponent,
    BorderoCadastroComponent,
    TextareaLimitadoComponent,
    ManutencaoEmpresasSapComponent,
    ManutencaoFornecedoresComponent,
    FiltroFornecedoresComponent,
    ModalAdicionarEmpresasSapComponent,
    ModalAdicionarFornecedoresComponent,
    EstornoLancamentosPagosComponent,
    EstornoResultadoComponent,
    EstornoAbasComponent,
    ModalPartesComponent,
    ModalPedidosComponent,
    GarantiasTabComponent,
    DespesaTabComponent,
    PagamentosTabComponent,
    ModalAdicionarEmpresasSapComponent,
    ModalAdicionarFornecedoresComponent,
    FiltroProcessosComponent,
    FiltroContaJudicialsComponent,
    FiltroEmpresaGrupoComponent,
    FiltroEscritorioComponent,
    FiltroFornecedorComponent,
    FiltroCentroCustoComponent,
    FiltroTipoLancamentoComponent,
    FiltroStatusLancamentoComponent,
    FiltroCategoriaPagamentoComponent,
    ModalAdicionarFornecedoresComponent,
    ManutencaoCategoriaPagamentoComponent,
    ModalAdicionarDespesasJudiciaisComponent,
    ModalAdicionarPagamentosComponent,
    ModalAdicionarHonorariosComponent,
    ModalAdicionarGarantiasComponent,
    ModalAdicionarRecuperacaoPagamentoComponent,
    ManutencaoFormasPagamentoComponent,
    ModalFormasPagamentoComponent,
    ManutencaoCentroCustoComponent,
    ModalAdicionarCentrosCustoComponent,
    InterfacesBBComarcaBBComponent,
    ConsultaSaldoGarantiaComponent,
    SaldoGarantiaCriteriosGeraisComponent,
    SaldoGarantiaBancoComponent,
    SaldoGarantiaProcessosComponent,
    SaldoGarantiaEmpresaGrupoComponent,
    SaldoGarantiaEstadoComponent,
    ModalAdicionarComarcaBBComponent,
    OrgaoBBComponent,
    FiltroOrgaoBbComponent,
    ModalAdicionarOrgaoBBComponent,
    TribunaisBBComponent,
    ModalTribunaisBbComponent,
    ModalAdicionarNaturezaBBComponent,
    ModalAdicionarModalidadeBbComponent,
    ModalidadeProdutoBbComponent,
    NaturezaBbComponent,
    StatusParcelaBBComponent,
    ModalAdicionarStatusParcelaComponent,
    FiltroFornecedoresContingenciaSapComponent,
    ManutencaoFornecedoresContingenciaSapComponent,
    ModalEditarFornecedoresContingenciaSapComponent,
    ModalAdicionarStatusParcelaComponent,
    ManutencaoGrupoLoteJuizadoComponent,
    ModalAdicionarGrupoLoteJuizadoComponent,
    ModalAdicionarStatusParcelaComponent,
    ConsultaArquivoRetornoComponent,
    ResultadoArquivoRetornoComponent,
    ModalAlteracaoDataGuiaComponent,
    DetalheGuiaImportacaoArquivoRetornoComponent,
    ImportarArquivoComponent,
    ImagemGuiaComponent,
    ModalAdicionarAgendamentoComponent,
    BuscarAgendamentosComponent,
    ImportarArquivoComponent,
    FiltroNumeroDoLoteComponent,
    MigracaoPedidosSapComponent,
    PainelMigracaoPedidosSapComponent,
    ListagemMigracaoPedidosSapComponent,
    InstrucoesMigracaoPedidosSapComponent
  ],
  providers: [
    TipoProcessoResolverGuard,
    NgbActiveModal,
    DeactivateGuard,
    SapGuard,
    RoleGuardService,
    ManutencaoFornecedorProfissionaisResolver,
    ManutencaoFornecedorBancoResolver,
    ManutencaoFornecedorEscritorioResolver,
    ComarcaTribunaisResolverGuard,
    ConsultaArquivoRetornoResolver,
    ModalTribunaisBbComponent,
    { provide: LOCALE_ID, useValue: "pt" },
  ],
  entryComponents: [
    BorderoCadastroComponent,
    ModalAdicionarFornecedoresComponent,
    ModalAdicionarEmpresasSapComponent,
    ModalAdicionarDespesasJudiciaisComponent,
    ModalAdicionarHonorariosComponent,
    ModalAdicionarPagamentosComponent,
    ModalAdicionarGarantiasComponent,
    ModalAdicionarRecuperacaoPagamentoComponent,
    ModalPartesComponent,
    ModalPedidosComponent,
    ModalFormasPagamentoComponent,
    ModalAdicionarComarcaBBComponent,
    ModalAdicionarNaturezaBBComponent,
    ModalAdicionarCentrosCustoComponent,
    ModalAdicionarOrgaoBBComponent,
    ModalTribunaisBbComponent,
    ModalAdicionarModalidadeBbComponent,
    ModalAdicionarStatusParcelaComponent,
    ModalAdicionarGrupoLoteJuizadoComponent,
    ModalAlteracaoDataGuiaComponent,
    ModalAdicionarStatusParcelaComponent,
    ModalEditarFornecedoresContingenciaSapComponent,
    ModalAdicionarAgendamentoComponent,
    InstrucoesMigracaoPedidosSapComponent

  ],

  bootstrap: [
    ConsultaPageComponent,
    TabelaComponent,
    ManutencaoGrupoLoteJuizadoComponent,
    AbasContentComponent,
    ModalAdicionarFornecedoresComponent,
    ModalAdicionarEmpresasSapComponent,
  ],
})
export class SapModule {
  constructor(private injector: Injector) {
    StaticInjector.setInjectorInstance(this.injector);
  }
}
