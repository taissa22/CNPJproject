import { IConfig, NgxMaskModule } from 'ngx-mask';
import { NgxSelectModule, INgxSelectOptions } from 'ngx-select-ex';
import { Injector, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StaticInjector } from './static-injector';
import { WorkflowRoutingRoutes } from './workflow.routing';
import { AgendamentoVepComponent } from './pages/agendamento-vep/agendamento-vep.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AlertModule, BsDatepickerModule, ModalModule, TabsModule } from 'ngx-bootstrap';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { TextMaskModule } from 'angular2-text-mask';
import { NgSelectModule } from '@ng-select/ng-select';
import { ComponentesModule } from '../componentes/componentes.module';
import { SisjurTableModule } from '@libs/sisjur/sisjur-table/sisjur-table.module';
import { SisjurPaginatorModule } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.module';
import { SharedModule } from '@shared/shared.module';
import { NgbModalModule, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AgendamentoVepModalComponent } from './modal/agendamento-vep-modal/agendamento-vep-modal.component';
import { ManutencaoContratoEscritorioComponent } from './pages/manutencao-contrato-escritorio/manutencao-contrato-escritorio.component';
import { ManutencaoContratoEscritorioModalComponent } from './modal/manutencao-contrato-escritorio-modal/manutencao-contrato-escritorio-modal.component';
import { RelatorioPagamentoEscritorioModalComponent } from './modal/relatorio-pagamento-escritorio-modal/relatorio-pagamento-escritorio-modal.component';
import { RelatorioPagamentoEscritorioComponent } from './pages/relatorio-pagamento-escritorio/relatorio-pagamento-escritorio.component';

const opcoesSelect: INgxSelectOptions = {
  optionValueField: 'id',
  optionTextField: 'nome',
  keepSelectedItems: true,
  autoSelectSingleOption: true
};

const maskConfig: Partial<IConfig> = {
  validation: false,
};

@NgModule({
  imports: [
    CommonModule,
    WorkflowRoutingRoutes,
    ReactiveFormsModule,
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
  ],
  declarations: [
    AgendamentoVepComponent,
    AgendamentoVepModalComponent,
    ManutencaoContratoEscritorioComponent,
    ManutencaoContratoEscritorioModalComponent,
    RelatorioPagamentoEscritorioModalComponent,
    RelatorioPagamentoEscritorioComponent
  ],
  entryComponents: [
    AgendamentoVepModalComponent,
    ManutencaoContratoEscritorioModalComponent,
    RelatorioPagamentoEscritorioModalComponent
  ],
})
export class WorkflowModule { 
  constructor(private injector: Injector) {
    StaticInjector.setInjectorInstance(this.injector);
  }
}
