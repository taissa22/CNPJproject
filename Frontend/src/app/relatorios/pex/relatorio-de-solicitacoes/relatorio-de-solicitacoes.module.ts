import { Injector, NgModule } from '@angular/core'; 
import { RelatorioDeSolicitacoesComponent } from './relatorio-de-solicitacoes.component';
import { SalvarComoModeloModalComponent } from './modals/salvar-como-modelo-modal/salvar-como-modelo-modal.component';  
import { SharedModule } from '@shared/shared.module';
import { RelatorioDeSolicitacoesRoutingModule } from './relatorio-de-solicitacoes-routing.module'; 
import { ComponentesModule } from 'src/app/componentes/componentes.module';
import { NgbModalModule, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ModalModule } from 'ngx-bootstrap';
import { TextMaskModule } from 'angular2-text-mask';
import { ModelosSalvosModalComponent } from './modals/modelos-salvos-modal/modelos-salvos-modal.component';
import { EditarAgendamentoComponent } from './modals/buscar-baixar-agendamentos-modal/editar-agendamento/editar-agendamento.component';
import { MostrarApenasNoPeriodoComponent } from './modals/buscar-baixar-agendamentos-modal/mostrar-apenas-no-periodo/mostrar-apenas-no-periodo.component';
import { DetalhesAgendamentoComponent } from './modals/buscar-baixar-agendamentos-modal/detalhes-agendamento/detalhes-agendamento.component';
import { HistoricoAgendamentoComponent } from './modals/buscar-baixar-agendamentos-modal/historico-agendamento/historico-agendamento.component';
import { AgendamentoRelatorioSolicitacoesModalComponent } from './modals/agendamento-relatorio-solicitacoes-modal/agendamento-relatorio-solicitacoes-modal.component';
import { StaticInjector } from './static-injector';
import { BuscarBaixarAgendamentosModalComponent } from './modals/buscar-baixar-agendamentos-modal/buscar-baixar-agendamentos-modal.component';
import { JurMultiselectListModule } from '@shared/components/jur-multiselect-list/jur-multiselect-list.module';
import { JurRadioViewModule } from '@shared/components/jur-radio-view/jur-radio-view.module';



@NgModule({
  declarations: [
    RelatorioDeSolicitacoesComponent,
    AgendamentoRelatorioSolicitacoesModalComponent,
    SalvarComoModeloModalComponent,
    ModelosSalvosModalComponent,
    BuscarBaixarAgendamentosModalComponent, 
    EditarAgendamentoComponent,
    MostrarApenasNoPeriodoComponent,
    DetalhesAgendamentoComponent,
    HistoricoAgendamentoComponent 
  ],
  imports: [  
    NgbModule,
    ComponentesModule,
    SharedModule,
    JurRadioViewModule,
    JurMultiselectListModule,
    RelatorioDeSolicitacoesRoutingModule,
    NgbModalModule,
    ModalModule.forRoot(), 
    TextMaskModule,
  ],
  entryComponents:[ 
    AgendamentoRelatorioSolicitacoesModalComponent,
    ModelosSalvosModalComponent,
    SalvarComoModeloModalComponent,
    BuscarBaixarAgendamentosModalComponent,
    MostrarApenasNoPeriodoComponent
  ] 
})
export class RelatorioDeSolicitacoesModule { 
  constructor(private injector: Injector) {
    StaticInjector.setInjectorInstance(this.injector);
  }
}
