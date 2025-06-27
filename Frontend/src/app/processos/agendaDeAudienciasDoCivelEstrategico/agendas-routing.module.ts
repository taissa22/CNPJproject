import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AgendaAudienciasComponent } from './pages/agenda-audiencias/agenda-audiencias.component';

const routes: Routes = [{ path: 'civel-estrategico', component: AgendaAudienciasComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AgendasRoutingModule { }
