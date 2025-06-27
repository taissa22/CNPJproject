import { Injector, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MovimentacoesRoutingModule } from './movimentacoes-routing.module';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';
import { ComponentesModule } from 'src/app/componentes/componentes.module';
import { SharedModule } from '@shared/shared.module';

import { RelatorioMovimentacaoPexComponent } from './relatorio-movimentacao-pex/relatorio-movimentacao-pex.component';
import { AgendamentoRelatorioMovimentacaoPexModalComponent } from './agendamento-relatorio-movimentacao-pex-modal/agendamento-relatorio-movimentacao-pex-modal.component';
import { RelatorioMovimentacaoJecComponent } from './relatorio-movimentacao-jec/relatorio-movimentacao-jec.component';

import { StaticInjector } from './static-injector';
import { AgendamentoRelatorioMovimentacaoCCModalComponent } from './agendamento-relatorio-movimentacao-cc-modal/agendamento-relatorio-movimentacao-cc-modal.component';
import { RelatorioMovimentacaoCCComponent } from './relatorio-movimentacao-cc/relatorio-movimentacao-cc.component';
import { AgendamentoRelatorioMovimentacaoJecModalComponent } from './agendamento-relatorio-movimentacao-jec-modal/agendamento-relatorio-movimentacao-jec-modal.component';

@NgModule({
  imports: [
    CommonModule,
    MovimentacoesRoutingModule,
    NgbModule,
    FormsModule,
    ComponentesModule,
    SharedModule,
  ],
  declarations: [
    RelatorioMovimentacaoPexComponent,
    AgendamentoRelatorioMovimentacaoPexModalComponent,RelatorioMovimentacaoCCComponent, AgendamentoRelatorioMovimentacaoCCModalComponent,
    RelatorioMovimentacaoJecComponent,
    AgendamentoRelatorioMovimentacaoJecModalComponent
  ],
  entryComponents: [
    AgendamentoRelatorioMovimentacaoPexModalComponent, AgendamentoRelatorioMovimentacaoCCModalComponent,
    AgendamentoRelatorioMovimentacaoJecModalComponent
  ]
})
export class MovimentacoesModule {
  constructor(private injector: Injector) {
    StaticInjector.setInjectorInstance(this.injector);
  }
}
