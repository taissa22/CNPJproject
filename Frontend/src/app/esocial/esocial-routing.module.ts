import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { RoleGuardService } from "@core/services/Roles.guard.ts.service";
import { Permissoes } from "@permissoes";
import { Formulario2500_v1_2_Component } from "./pages/formulario/cadastro-formulario/formulario2500/formulario2500_v1_2.component";
import { Formulario2501_v1_2_Component } from "./pages/formulario/cadastro-formulario/formulario2501/formulario2501_v1_2.component";
import { ESocialAcompanhamentoComponent } from "./pages/aplicacao/acompanhamento/e-social-acompanhamento.component";
import { ParteProcessoComponent } from "./pages/aplicacao/parte-processo/parte-processo.component";
import { Formulario2500_v1_1_Component } from "./pages/formulario_v1_1/cadastro-formulario/formulario2500/formulario2500.component";
import { Formulario2501_v1_1_Component } from "./pages/formulario_v1_1/cadastro-formulario/formulario2501/formulario2501.component";
import { DashboardEsocialComponent } from "./pages/aplicacao/dashboard-esocial/dashboard-esocial.component";


const routes: Routes = [
  {
    path: 'parte-processo', component: ParteProcessoComponent, canActivate: [RoleGuardService], data: { role: Permissoes.ACESSAR_CADASTRO_ESOCIAL }
  },
  {
    path: 'acompanhamento', component: ESocialAcompanhamentoComponent, canActivate: [RoleGuardService], data: { role: Permissoes.ACESSAR_ACOMPANHAMENTO_ENVIO_ESOCIAL }
  },
  {
    path: 'parte-processo/formulario_v1_1/formulario-2500', component: Formulario2500_v1_1_Component, canActivate: [RoleGuardService], data: { role: Permissoes.ACESSAR_CADASTRO_ESOCIAL }
  },
  {
    path: 'parte-processo/formulario_v1_1/formulario-2501', component: Formulario2501_v1_1_Component, canActivate: [RoleGuardService], data: { role: Permissoes.ACESSAR_CADASTRO_ESOCIAL }
  },
  {
    path: 'parte-processo/formulario/formulario-2500', component: Formulario2500_v1_2_Component, canActivate: [RoleGuardService], data: { role: Permissoes.ACESSAR_CADASTRO_ESOCIAL }
  },
  {
    path: 'parte-processo/formulario/formulario-2501', component: Formulario2501_v1_2_Component, canActivate: [RoleGuardService], data: { role: Permissoes.ACESSAR_CADASTRO_ESOCIAL }
  },
  {
    path: 'dashboard', component: DashboardEsocialComponent, canActivate: [RoleGuardService], data: { role: Permissoes.ACESSAR_DASHBOARD_ESOCIAL}
  }

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class ESOCIALRoutingModule { } 
