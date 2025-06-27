import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RoleGuardService } from '@core/services/Roles.guard.ts.service';
import { Permissoes } from '@permissoes';
import { RelatorioMovimentacaoCCComponent } from './relatorio-movimentacao-cc/relatorio-movimentacao-cc.component';
import { RelatorioMovimentacaoJecComponent } from './relatorio-movimentacao-jec/relatorio-movimentacao-jec.component';

import { RelatorioMovimentacaoPexComponent } from './relatorio-movimentacao-pex/relatorio-movimentacao-pex.component';

const routes: Routes = [
  {
    path: 'relatorio-movimentacao-pex',
    component: RelatorioMovimentacaoPexComponent,
    data: {
      role: Permissoes.ACESSAR_MOVIMENTACAO_PEX
    },
    canActivate: [RoleGuardService]
  },
  {
    path: 'relatorio-movimentacao-cc',
    component: RelatorioMovimentacaoCCComponent,
    data: {
      role: Permissoes.ACESSAR_MOVIMENTACAO_CC
    },
    canActivate: [RoleGuardService]
  },
  {
    path: 'relatorio-movimentacao-jec',
    component: RelatorioMovimentacaoJecComponent,
    data: {
      role: Permissoes.ACESSAR_MOVIMENTACAO_JEC
     },
    canActivate: [RoleGuardService]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MovimentacoesRoutingModule {}
