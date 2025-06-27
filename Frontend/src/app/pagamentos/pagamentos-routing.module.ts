import { AuthGuard } from './../core/services/auth-guard.service';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CargaDeComprovantesComponent } from './carga-de-comprovantes/carga-de-comprovantes.component';
import { CargaDeDocumentosComponent } from './carga-de-documentos/carga-de-documentos.component';
import { CargaDeCompromissosComponent } from './carga-de-compromissos/carga-de-compromissos.component';
import { AcompanhamentoComponent } from './acompanhamento/acompanhamento.component';

const routes: Routes = [
  { path: 'carga-de-comprovantes', component: CargaDeComprovantesComponent, canActivate: [AuthGuard] },
  { path: 'carga-de-documentos', component: CargaDeDocumentosComponent, canActivate: [AuthGuard] },
  { path: 'carga-de-compromissos', component: CargaDeCompromissosComponent, canActivate: [AuthGuard] },
  { path: 'acompanhamento-de-compromissos', component: AcompanhamentoComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PagamentosRoutingModule { }
