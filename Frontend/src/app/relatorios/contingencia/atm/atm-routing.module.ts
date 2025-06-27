import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RoleGuardService } from '@core/services/Roles.guard.ts.service';
import { Permissoes } from '@permissoes';

import { RelatorioAtmPexComponent } from './pex/relatorio-atm-pex/relatorio-atm-pex.component';
import { AgendamentosPageComponent } from './jec/agendamentos-page/agendamentos-page.component';
import { RelatorioATMCCComponent } from '@relatorios/contingencia/atm/cc/relatorio-atm-cc.component';

const routes: Routes = [
  {
    path: 'pex/relatorio-atm-pex',
    component: RelatorioAtmPexComponent,
    data: {
        role: Permissoes.ACESSAR_ATM_PEX
    },
    canActivate: [RoleGuardService]
  },
  {
    path: 'jec',
    component: AgendamentosPageComponent,
    data: {
        role: Permissoes.ACESSAR_ATM_JEC
    },
    canActivate: [RoleGuardService]
  },
  {

    path: 'cc/relatorio-atm-cc',    
    component: RelatorioATMCCComponent,
    canActivate: [RoleGuardService],
    data: {
       role: 'm_RelatorioAtmCC'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AtmRoutingModule {}
