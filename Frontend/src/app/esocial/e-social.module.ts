import { CommonModule } from '@angular/common';
import { Injector, LOCALE_ID, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SisjurPaginatorModule } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.module';
import { SisjurTableModule } from '@libs/sisjur/sisjur-table/sisjur-table.module';
import { NgbModalModule, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedModule } from '@shared/shared.module';
import { FormControlCustomValidators } from '@shared/validators/form-control-custom-validators';
import { TextMaskModule } from 'angular2-text-mask';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { AlertModule, BsDatepickerModule, ModalModule, TabsModule, defineLocale, ptBrLocale } from 'ngx-bootstrap';
import { IConfig, NgxMaskModule } from 'ngx-mask';
import { INgxSelectOptions, NgxSelectModule } from 'ngx-select-ex';
import { ComponentesModule } from '../componentes/componentes.module';
import { ESOCIALRoutingModule } from './esocial-routing.module';
import { StaticInjector } from './static-injector';

import { ESocialAcompanhamentoComponent } from '@esocial/pages/aplicacao/acompanhamento/e-social-acompanhamento.component';

import { HistoricoFormularioComponent } from '@esocial/pages/aplicacao/parte-processo/historico-formulario/historico-formulario.component';
import { ParteProcessoComponent } from '@esocial/pages/aplicacao/parte-processo/parte-processo.component';

import { ContratoSubgrupos_v1_1_Component } from '@esocial/pages/formulario_v1_1/cadastro-formulario/contrato-subgrupos/contrato-subgrupos.component';
import { Formulario2500_v1_1_Component } from '@esocial/pages/formulario_v1_1/cadastro-formulario/formulario2500/formulario2500.component';
import { Formulario2501_v1_1_Component } from '@esocial/pages/formulario_v1_1/cadastro-formulario/formulario2501/formulario2501.component';

import { EsocialCategoriaSubgrupoComponent } from '@esocial/pages/formulario_v1_1/cadastro-formulario/subgrupos/categoria/esocial-categoria-subgrupo.component';
import { EsocialContratoSubgrupoComponent } from '@esocial/pages/formulario_v1_1/cadastro-formulario/subgrupos/contrato/esocial-contrato-subgrupo.component';
import { EsocialDependenteSubgrupoComponent } from '@esocial/pages/formulario_v1_1/cadastro-formulario/subgrupos/dependente/esocial-dependente-subgrupo.component';
import { DependenteModalComponent } from '@esocial/pages/formulario_v1_1/cadastro-formulario/subgrupos-modals/dependente/dependente-modal.component';

import { EsocialContribuicaoSubgrupoComponent } from '@esocial/pages/formulario_v1_1/cadastro-formulario/subgrupos/contribuicao/esocial-contribuicao-subgrupo.component';
import { EsocialImpostoSubgrupoComponent } from '@esocial/pages/formulario_v1_1/cadastro-formulario/subgrupos/imposto/esocial-imposto-subgrupo.component';
import { EsocialObservacaoSubgrupoComponent } from '@esocial/pages/formulario_v1_1/cadastro-formulario/subgrupos/observacao/esocial-observacao-subgrupo.component';
import { EsocialPeriodoBaseSubgrupoComponent } from '@esocial/pages/formulario_v1_1/cadastro-formulario/subgrupos/periodo-base/esocial-periodo-base-subgrupo.component';
import { EsocialPeriodoSubgrupoComponent } from '@esocial/pages/formulario_v1_1/cadastro-formulario/subgrupos/periodo/esocial-periodo-subgrupo.component';
import { EsocialRemuneracaoSubgrupoComponent } from '@esocial/pages/formulario_v1_1/cadastro-formulario/subgrupos/remuneracao/esocial-remuneracao-subgrupo.component';
import { EsocialUnicidadeSubgrupoComponent } from '@esocial/pages/formulario_v1_1/cadastro-formulario/subgrupos/unicidade/esocial-unicidade-subgrupo.component';

import { CarregarBlocoComponent } from '@esocial/pages/formulario_v1_1/cadastro-formulario/subgrupos-modals/carregar-bloco/carregar-bloco.component';

import { CategoriaModalComponent } from '@esocial/pages/formulario_v1_1/cadastro-formulario/subgrupos-modals/categoria/categoria-modal.component';
import { ContratoModalComponent } from '@esocial/pages/formulario_v1_1/cadastro-formulario/subgrupos-modals/contrato/contrato-modal.component';
import { ContribuicaoModalComponent } from '@esocial/pages/formulario_v1_1/cadastro-formulario/subgrupos-modals/contribuicao/contribuicao-modal.component';
import { ImpostoModalComponent } from '@esocial/pages/formulario_v1_1/cadastro-formulario/subgrupos-modals/imposto/imposto-modal.component';
import { ObservacaoModalComponent } from '@esocial/pages/formulario_v1_1/cadastro-formulario/subgrupos-modals/observacao/observacao-modal.component';
import { PeriodoBaseModalComponent } from '@esocial/pages/formulario_v1_1/cadastro-formulario/subgrupos-modals/periodo-base-modal/periodo-base-modal.component';
import { PeriodoModalComponent } from '@esocial/pages/formulario_v1_1/cadastro-formulario/subgrupos-modals/periodo/periodo-modal.component';
import { RemuneracaoModalComponent } from '@esocial/pages/formulario_v1_1/cadastro-formulario/subgrupos-modals/remuneracao/remuneracao-modal.component';
import { UnicidadeModalComponent } from '@esocial/pages/formulario_v1_1/cadastro-formulario/subgrupos-modals/unicidade/unicidade-modal.component';

import { ContratoSubgrupos_v1_2_Component } from './pages/formulario/cadastro-formulario/contrato-subgrupos/contrato-subgrupos_v1_2.component';
import { Formulario2500_v1_2_Component } from './pages/formulario/cadastro-formulario/formulario2500/formulario2500_v1_2.component';
import { Formulario2501_v1_2_Component } from './pages/formulario/cadastro-formulario/formulario2501/formulario2501_v1_2.component';

import { EsocialCategoriaSubgrupo_v1_2_Component } from './pages/formulario/cadastro-formulario/subgrupos/categoria/esocial-categoria-subgrupo_v1_2.component';
import { EsocialContratoSubgrupo_v1_2_Component } from './pages/formulario/cadastro-formulario/subgrupos/contrato/esocial-contrato-subgrupo_v1_2.component';
import { EsocialContribuicaoSubgrupo_v1_2_Component } from './pages/formulario/cadastro-formulario/subgrupos/contribuicao/esocial-contribuicao-subgrupo_v1_2.component';
import { EsocialImpostoSubgrupo_v1_2_Component } from './pages/formulario/cadastro-formulario/subgrupos/imposto/esocial-imposto-subgrupo_v1_2.component';
import { EsocialObservacaoSubgrupo_v1_2_Component } from './pages/formulario/cadastro-formulario/subgrupos/observacao/esocial-observacao-subgrupo_v1_2.component';
import { EsocialPeriodoBaseSubgrupo_v1_2_Component } from './pages/formulario/cadastro-formulario/subgrupos/periodo-base/esocial-periodo-base-subgrupo_v1_2.component';
import { EsocialPeriodoSubgrupo_v1_2_Component } from './pages/formulario/cadastro-formulario/subgrupos/periodo/esocial-periodo-subgrupo_v1_2.component';
import { EsocialRemuneracaoSubgrupo_v1_2_Component } from './pages/formulario/cadastro-formulario/subgrupos/remuneracao/esocial-remuneracao-subgrupo_v1_2.component';
import { EsocialUnicidadeSubgrupo_v1_2_Component } from './pages/formulario/cadastro-formulario/subgrupos/unicidade/esocial-unicidade-subgrupo_v1_2.component';

import { CarregarBloco_v1_2_Component } from './pages/formulario/cadastro-formulario/subgrupos-modals/carregar-bloco/carregar-bloco_v1_2.component';

import { CategoriaModal_v1_2_Component } from './pages/formulario/cadastro-formulario/subgrupos-modals/categoria/categoria-modal_v1_2.component';
import { ContratoModal_v1_2_Component } from './pages/formulario/cadastro-formulario/subgrupos-modals/contrato/contrato-modal_v1_2.component';
import { ContribuicaoModal_v1_2_Component } from './pages/formulario/cadastro-formulario/subgrupos-modals/contribuicao/contribuicao-modal_v1_2.component';
import { ImpostoModal_v1_2_Component } from './pages/formulario/cadastro-formulario/subgrupos-modals/imposto/imposto-modal_v1_2.component';
import { ObservacaoModal_v1_2_Component } from './pages/formulario/cadastro-formulario/subgrupos-modals/observacao/observacao-modal_v1_2.component';
import { PeriodoBaseModal_v1_2_Component } from './pages/formulario/cadastro-formulario/subgrupos-modals/periodo-base-modal/periodo-base-modal_v1_2.component';
import { PeriodoModal_v1_2_Component } from './pages/formulario/cadastro-formulario/subgrupos-modals/periodo/periodo-modal_v1_2.component';
import { RemuneracaoModal_v1_2_Component } from './pages/formulario/cadastro-formulario/subgrupos-modals/remuneracao/remuneracao-modal_v1_2.component';
import { UnicidadeModal_v1_2_Component } from './pages/formulario/cadastro-formulario/subgrupos-modals/unicidade/unicidade-modal_v1_2.component';


import { CurrencyPipe } from '@angular/common';
import { CpfPipe } from './libs/cpf.pipe';

import { EsocialAbonoSubgrupo_v1_2Component } from './pages/formulario/cadastro-formulario/subgrupos/abono/esocial-abono-subgrupo_v1_2.component';
import { AbonoModal_v1_2Component } from './pages/formulario/cadastro-formulario/subgrupos-modals/abono-modal_v1_2/abono-modal_v1_2.component';

import { InfoDeducoesIsencoesModal_v1_2Component } from './pages/formulario/cadastro-formulario/subgrupos-imposto-modals/info-deducoes-isencoes-modal_v1_2/info-deducoes-isencoes-modal_v1_2.component';
import { EsocialDedDepenModal_v1_2_Component } from './pages/formulario/cadastro-formulario/subgrupos-imposto-modals/info-deducoes-dependente-modal/esocial-deducoes-dependente-modal.component';
import { DedDepenModal_v1_2_Component } from './pages/formulario/cadastro-formulario/subgrupos-imposto-modals/info-deducoes-dependente-edit-modal/deducoes-dependente-edit-modal.component';
import { EsocialPenAlimModal_v1_2_Component } from './pages/formulario/cadastro-formulario/subgrupos-imposto-modals/info-deducoes-pen-alim-modal/esocial-deducoes-pen-alim-modal.component';
import { PenAlimModal_v1_2_Component } from './pages/formulario/cadastro-formulario/subgrupos-imposto-modals/info-deducoes-pen-alim-edit-modal/deducoes-pen-alim-edit-modal.component';
import { InfoDepSubgrupo_v1_2Component } from './pages/formulario/cadastro-formulario/subgrupos-imposto-modals/info-dep-subgrupo_v1_2/info-dep-subgrupo_v1_2.component';
import { InfoDepModal_v1_2Component } from './pages/formulario/cadastro-formulario/subgrupos-imposto-modals/info-dep-subgrupo_v1_2/info-dep-modal_v1_2/info-dep-modal_v1_2.component';


import { ProcDepositosJudiciaisModal_v1_2Component } from './pages/formulario/cadastro-formulario/subgrupos-imposto-modals/proc-depositos-judiciais-modal_v1_2/proc-depositos-judiciais-modal_v1_2.component';
import { ProcessosDepJudModal_v1_2Component } from './pages/formulario/cadastro-formulario/subgrupos-imposto-modals/proc-dep-jud-modais_v1_2/processos-dep-jud-modal_v1_2/processos-dep-jud-modal_v1_2.component';
import { ValoresDepJudModal_v1_2Component } from './pages/formulario/cadastro-formulario/subgrupos-imposto-modals/proc-dep-jud-modais_v1_2/valores-dep-jud-modal_v1_2/valores-dep-jud-modal_v1_2.component';
import { DetalhamentoDeducoesModal_v1_2Component } from './pages/formulario/cadastro-formulario/subgrupos-imposto-modals/proc-dep-jud-modais_v1_2/detalhamento-deducoes-modal_v1_2/detalhe-deducoes-modal_v1_2.component';
import { PensaoAlimenticiaModal_v1_2Component } from './pages/formulario/cadastro-formulario/subgrupos-imposto-modals/proc-dep-jud-modais_v1_2/pensao-alimenticia-modal_v1_2/pensao-alimenticia-modal_v1_2.component';
import { ProcessosModal_v1_2Component } from './pages/formulario/cadastro-formulario/subgrupos-imposto-modals/proc-dep-jud-modais_v1_2/processos-dep-jud-modal_v1_2/processos-modal_v1_2/processos-modal_v1_2.component';
import { ValoresModal_v1_2Component } from './pages/formulario/cadastro-formulario/subgrupos-imposto-modals/proc-dep-jud-modais_v1_2/valores-dep-jud-modal_v1_2/valores-modal_v1_2/valores-modal_v1_2.component';
import { DeducoesModal_v1_2Component } from './pages/formulario/cadastro-formulario/subgrupos-imposto-modals/proc-dep-jud-modais_v1_2/detalhamento-deducoes-modal_v1_2/deducoes-modal_v1_2/deducoes-modal_v1_2.component';
import { PensaoModal_v1_2Component } from './pages/formulario/cadastro-formulario/subgrupos-imposto-modals/proc-dep-jud-modais_v1_2/pensao-alimenticia-modal_v1_2/pensao-modal_v1_2/pensao-modal_v1_2.component';
import { InfoRendRraComponent } from './pages/formulario/cadastro-formulario/subgrupos-imposto-modals/info-rend-rra/info-rend-rra.component';
import { InfoRendRraAdvogadosComponent } from './pages/formulario/cadastro-formulario/subgrupos-imposto-modals/info-rend-rra/info-rend-rra-advogados/info-rend-rra-advogados.component';
import { InfoRendRraAdvModalComponent } from './pages/formulario/cadastro-formulario/subgrupos-imposto-modals/info-rend-rra/info-rend-rra-advogados/info-rend-rra-adv-modal/info-rend-rra-adv-modal.component';
import { CnpjPipe } from './libs/cnpj.pipe';
import { CpfCnpjPipe } from '@shared/pipes/cpf-cnpj.pipe';
import { SisjurProgressBarModule } from '@libs/sisjur/sisjur-progress-bar/sisjur-progress-bar.module';
import { DashboardEsocialComponent } from './pages/aplicacao/dashboard-esocial/dashboard-esocial.component';
import { AlertaDownloadComponent } from './pages/aplicacao/dashboard-esocial/alerta-download/alerta-download.component';
import { ParteProcessoBuscaModalComponent } from './pages/aplicacao/parte-processo/parte-processo-busca-modal/parte-processo-busca-modal.component';
import { InfoIntermComponent } from './pages/formulario/cadastro-formulario/subgrupos-modals/periodo/info-interm/info-interm.component';
import { InfoIntermModalComponent } from './pages/formulario/cadastro-formulario/subgrupos-modals/periodo/info-interm/info-interm-modal/info-interm-modal.component';


const opcoesSelect: INgxSelectOptions = {
  optionValueField: 'id',
  optionTextField: 'nome',
  keepSelectedItems: true,
  autoSelectSingleOption: true
};

const maskConfig: Partial<IConfig> = {
  validation: false,
};

ptBrLocale.invalidDate = 'Data Inválida';
defineLocale('pt-br', ptBrLocale);



@NgModule({
  imports: [
    ReactiveFormsModule,
    CommonModule,
    ESOCIALRoutingModule,
    AlertModule.forRoot(),
    NgxSelectModule.forRoot(opcoesSelect),
    BsDatepickerModule.forRoot(),
    ModalModule.forRoot(),
    CurrencyMaskModule,
    TextMaskModule,
    NgSelectModule,
    ComponentesModule,
    FormsModule,
    TabsModule.forRoot(),
    NgxMaskModule.forRoot(maskConfig),
    SisjurTableModule,
    SisjurPaginatorModule,
    SharedModule,
    NgbModule,
    NgbModalModule,
    SisjurProgressBarModule
  ],
  declarations: [
    ParteProcessoComponent,
    HistoricoFormularioComponent,

    Formulario2500_v1_1_Component,
    Formulario2501_v1_1_Component,

    ContratoSubgrupos_v1_1_Component,   

    EsocialCategoriaSubgrupoComponent,
    EsocialContratoSubgrupoComponent,
    EsocialDependenteSubgrupoComponent,
    EsocialObservacaoSubgrupoComponent,
    EsocialPeriodoSubgrupoComponent,
    EsocialRemuneracaoSubgrupoComponent,
    EsocialUnicidadeSubgrupoComponent,
    EsocialPeriodoBaseSubgrupoComponent,
    EsocialImpostoSubgrupoComponent,
    EsocialContribuicaoSubgrupoComponent,

    CategoriaModalComponent,
    ContratoModalComponent,
    DependenteModalComponent,
    ObservacaoModalComponent,
    PeriodoModalComponent,
    RemuneracaoModalComponent,
    UnicidadeModalComponent,
    PeriodoBaseModalComponent,
    ContribuicaoModalComponent,
    ImpostoModalComponent,
    
    CarregarBlocoComponent,
    
    Formulario2500_v1_2_Component,
    Formulario2501_v1_2_Component,

    ContratoSubgrupos_v1_2_Component,

    EsocialCategoriaSubgrupo_v1_2_Component,
    EsocialContratoSubgrupo_v1_2_Component,
    EsocialObservacaoSubgrupo_v1_2_Component,
    EsocialPeriodoSubgrupo_v1_2_Component,
    EsocialRemuneracaoSubgrupo_v1_2_Component,
    EsocialUnicidadeSubgrupo_v1_2_Component,
    EsocialPeriodoBaseSubgrupo_v1_2_Component,
    EsocialImpostoSubgrupo_v1_2_Component,
    EsocialContribuicaoSubgrupo_v1_2_Component,

    CategoriaModal_v1_2_Component,
    ContratoModal_v1_2_Component,
    ObservacaoModal_v1_2_Component,
    PeriodoModal_v1_2_Component,
    RemuneracaoModal_v1_2_Component,
    UnicidadeModal_v1_2_Component,
    PeriodoBaseModal_v1_2_Component,
    ContribuicaoModal_v1_2_Component,
    ImpostoModal_v1_2_Component,
    InfoIntermComponent,
    InfoIntermModalComponent,

    CarregarBloco_v1_2_Component,

    InfoDeducoesIsencoesModal_v1_2Component,
    EsocialDedDepenModal_v1_2_Component,
    DedDepenModal_v1_2_Component,
    EsocialPenAlimModal_v1_2_Component,
    PenAlimModal_v1_2_Component,
    ProcDepositosJudiciaisModal_v1_2Component,
    ProcessosDepJudModal_v1_2Component,
    ValoresDepJudModal_v1_2Component,
    DetalhamentoDeducoesModal_v1_2Component,
    PensaoAlimenticiaModal_v1_2Component,
    ProcessosModal_v1_2Component,
    ValoresModal_v1_2Component,
    DeducoesModal_v1_2Component,
    PensaoModal_v1_2Component,
    InfoRendRraComponent,
    InfoRendRraAdvogadosComponent,
    InfoRendRraAdvModalComponent,

    EsocialAbonoSubgrupo_v1_2Component,
    AbonoModal_v1_2Component,

    InfoDepSubgrupo_v1_2Component,
    InfoDepModal_v1_2Component,

    ESocialAcompanhamentoComponent,
    CpfPipe,
    CnpjPipe,
    DashboardEsocialComponent,
    AlertaDownloadComponent,
    ParteProcessoBuscaModalComponent
  ],
  entryComponents: [
    ParteProcessoComponent,

    ESocialAcompanhamentoComponent,
    
    CarregarBlocoComponent,

    CategoriaModalComponent,
    ContratoModalComponent,
    DependenteModalComponent,
    ObservacaoModalComponent,
    PeriodoModalComponent,
    RemuneracaoModalComponent,
    UnicidadeModalComponent,
    PeriodoBaseModalComponent,
    ContribuicaoModalComponent,
    ImpostoModalComponent,

    CarregarBloco_v1_2_Component,

    CategoriaModal_v1_2_Component,
    ContratoModal_v1_2_Component,
    ObservacaoModal_v1_2_Component,
    PeriodoModal_v1_2_Component,
    RemuneracaoModal_v1_2_Component,
    UnicidadeModal_v1_2_Component,
    PeriodoBaseModal_v1_2_Component,
    ContribuicaoModal_v1_2_Component,
    ImpostoModal_v1_2_Component,
    InfoIntermComponent,
    InfoIntermModalComponent,

    AbonoModal_v1_2Component,
    InfoDeducoesIsencoesModal_v1_2Component,
    EsocialDedDepenModal_v1_2_Component,
    DedDepenModal_v1_2_Component,
    EsocialPenAlimModal_v1_2_Component,
    PenAlimModal_v1_2_Component,
    InfoDepSubgrupo_v1_2Component,
    InfoDepModal_v1_2Component,
    ProcDepositosJudiciaisModal_v1_2Component,
    ProcessosDepJudModal_v1_2Component,
    ValoresDepJudModal_v1_2Component,
    DetalhamentoDeducoesModal_v1_2Component,
    PensaoAlimenticiaModal_v1_2Component,
    ProcessosModal_v1_2Component,
    ValoresModal_v1_2Component,
    DeducoesModal_v1_2Component,
    PensaoModal_v1_2Component,
    InfoRendRraComponent,
    InfoRendRraAdvogadosComponent,
    InfoRendRraAdvModalComponent,
    ParteProcessoBuscaModalComponent

  ],
  providers:[
    { provide: LOCALE_ID, useValue: 'pt-BR' },
    FormControlCustomValidators,
    CurrencyPipe,
    CpfPipe,
    CnpjPipe
  ]
})

export class ESocialModule {
  constructor(private injector: Injector) {
    StaticInjector.setInjectorInstance(this.injector);
  }
}
