import { Injector, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AtmRoutingModule } from './atm-routing.module';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';
import { ComponentesModule } from 'src/app/componentes/componentes.module';
import { SharedModule } from '@shared/shared.module';

import { RelatorioAtmPexComponent } from './pex/relatorio-atm-pex/relatorio-atm-pex.component';
import { AgendamentoPexModalComponent } from './pex/agendamento-pex-modal/agendamento-pex-modal.component';
import { AgendamentosPageComponent } from './jec/agendamentos-page/agendamentos-page.component';
import { AgendamentoModalComponent } from './jec/agendamento-modal/agendamento-modal.component';

import { AgendamentoCCModalComponent } from '@relatorios/contingencia/atm/cc/agendamento-cc-modal/agendamento-cc-modal.component';
import { RelatorioATMCCComponent } from '@relatorios/contingencia/atm/cc/relatorio-atm-cc.component';

import { SisjurModule } from '@libs/sisjur/sisjur.module';
import { StaticInjector } from './static-injector';

@NgModule({
  imports: [
    CommonModule,
    AtmRoutingModule,
    NgbModule,
    FormsModule,
    ComponentesModule,
    SharedModule,
    SisjurModule
  ],
  declarations: [
    RelatorioAtmPexComponent,
    AgendamentoPexModalComponent,
    AgendamentosPageComponent,
    AgendamentoModalComponent,
    RelatorioATMCCComponent,
    AgendamentoCCModalComponent
  ],
  entryComponents: [
    AgendamentoPexModalComponent,
    AgendamentoModalComponent,
    AgendamentoCCModalComponent
    
  ]
})
export class AtmModule {  
  constructor(private injector: Injector) {    
    StaticInjector.setInjectorInstance(this.injector);
  }
}
