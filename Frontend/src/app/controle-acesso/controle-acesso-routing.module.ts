import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LogOperacoesComponent } from './usuarios/log-operacoes/log-operacoes.component';

const routes: Routes = [
  {
    path: 'usuarios-log-operacoes/:usuario',
    component: LogOperacoesComponent,
    // data: {
    //   role: 'm_AlteracaoProcBlocoWeb'
    // },
    // canActivate: [RoleGuardSAPService]
  },
  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ControleAcessoRoutingModule { }
