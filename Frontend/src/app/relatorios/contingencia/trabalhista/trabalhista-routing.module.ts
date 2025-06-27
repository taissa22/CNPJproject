import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RoleGuardService } from '@core/services/Roles.guard.ts.service';
import { Permissoes } from '@permissoes';

import { ProvisaoTrabalhistaPorMediaPageComponent } from './provisao-trabalhista-por-media/provisao-trabalhista-por-media-page/provisao-trabalhista-por-media-page.component';
import { EstoqueDePedidosPageComponent } from './estoque-de-pedidos/estoque-de-pedidos-page/estoque-de-pedidos-page.component';
import { FechamentoComponent } from './fechamento/fechamento.component';
import { DetalheProvisaoTrabalhistaPedidoPRComponent } from './DetalheProvisãoTrabalhistaPedido/DetalheProvisaoTrabalhistaPedidoPR/DetalheProvisaoTrabalhistaPedidoPR.component';
import { DetalheProvisaoTrabalhistaPedidoPOComponent } from './DetalheProvisãoTrabalhistaPedido/DetalheProvisaoTrabalhistaPedidoPO/DetalheProvisaoTrabalhistaPedidoPO.component';
import { RelatorioMovimentacaoTrabalhistaComponent } from './relatorio-movimentacao-trabalhista/relatorio-movimentacao-trabalhista.component';

const routes: Routes = [
  {
    path: 'provisao-trabalhista-por-media',
    component: ProvisaoTrabalhistaPorMediaPageComponent,
    data: {
      role: Permissoes.ACESSAR_PROVISAO_TRABALHISTA
    },
    canActivate: [RoleGuardService]
  },
  {
    path: 'estoque-de-pedidos',
    component: EstoqueDePedidosPageComponent,
    data: {
      role: Permissoes.ACESSAR_ESTOQUE_DE_PEDIDOS
    },
    canActivate: [RoleGuardService]
  },
  {
    path: 'fechamento-trabalhista',
    component: FechamentoComponent,
  },
  {
    path: 'detalhe-provisao-trabalhista-pedidoPR',
    component: DetalheProvisaoTrabalhistaPedidoPRComponent,
  },
  {
    path: 'detalhe-provisao-trabalhista-pedidoPO',
    component: DetalheProvisaoTrabalhistaPedidoPOComponent
  },
  {
    path: 'relatorio-movimentacao-trabalhista',
    component: RelatorioMovimentacaoTrabalhistaComponent,
    data: {
      role: Permissoes.ACESSAR_MOVIMENTACAO_TRABALHISTA
    },
    //canActivate: [RoleGuardService]
  }
];
 
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TrabalhistaRoutingModule {}
