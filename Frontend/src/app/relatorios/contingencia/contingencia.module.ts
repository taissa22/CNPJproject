import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ContingenciaRoutingModule } from './contingencia-routing.module';
import { RelatorioMovimentacaoPexComponent } from './movimentacoes/relatorio-movimentacao-pex/relatorio-movimentacao-pex.component';
import { AgendamentoRelatorioMovimentacaoPexModalComponent } from './movimentacoes/agendamento-relatorio-movimentacao-pex-modal/agendamento-relatorio-movimentacao-pex-modal.component';

@NgModule({
  imports: [CommonModule, ContingenciaRoutingModule]
})
export class ContingenciaModule {}
