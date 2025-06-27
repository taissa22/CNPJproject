import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RoleGuardService } from '@core/services/Roles.guard.ts.service';
import { Permissoes } from '@permissoes';
import { AgendamentoVepComponent } from './pages/agendamento-vep/agendamento-vep.component';
import { ManutencaoContratoEscritorioComponent } from './pages/manutencao-contrato-escritorio/manutencao-contrato-escritorio.component';
import { RelatorioPagamentoEscritorioComponent } from './pages/relatorio-pagamento-escritorio/relatorio-pagamento-escritorio.component';

const routes: Routes = [
  {
    path: 'calcular-valor-esperado-processo', component: AgendamentoVepComponent, canActivate: [RoleGuardService], data: { role: Permissoes.ACESSAR_AGEND_VALOR_ESPERADO_PROCESSO}
  },
  {
    path: 'manutencao-contrato', component: ManutencaoContratoEscritorioComponent, canActivate: [RoleGuardService], data: { role: Permissoes.ACESSAR_MANUTENCAO_CONTRATO_ESCRITORIO}
  },
  {
    path: 'calcular-relatorio-pagamentos-escritorios', component: RelatorioPagamentoEscritorioComponent, canActivate: [RoleGuardService], data: { role: Permissoes.ACESSAR_RELATORIO_PAGAMENTO_ESCRITORIO}
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class WorkflowRoutingRoutes { }
