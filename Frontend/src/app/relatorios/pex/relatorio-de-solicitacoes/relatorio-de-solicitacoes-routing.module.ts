import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RelatorioDeSolicitacoesComponent } from './relatorio-de-solicitacoes.component';
 
const routes: Routes = [
    {
        path: '',
        component: RelatorioDeSolicitacoesComponent
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RelatorioDeSolicitacoesRoutingModule {}