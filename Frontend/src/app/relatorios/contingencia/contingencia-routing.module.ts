import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  {
    path: 'trabalhista',
    loadChildren: () =>
      import('@relatorios/contingencia/trabalhista/trabalhista.module').then(
        m => m.TrabalhistaModule
      )
  },
  {
    path: 'atm',
    loadChildren: () =>
      import('@relatorios/contingencia/atm/atm.module').then(m => m.AtmModule)
  },
  {
    path: 'movimentacao',
    loadChildren: () =>
      import('@relatorios/contingencia/movimentacoes/movimentacoes.module').then(m => m.MovimentacoesModule)
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ContingenciaRoutingModule {}
