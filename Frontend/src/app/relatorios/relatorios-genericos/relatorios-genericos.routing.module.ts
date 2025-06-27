import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RelatoriosGenericosComponent } from './relatorios-genericos.component';
import { CriteriosComponent } from './criterios/criterios.component';
import { EscritorioComponent } from './escritorio/escritorio.component';
import { EmpresasGrupoComponent } from './empresas-grupo/empresas-grupo.component';
import { EscritoriosAuthResolver } from './escritorio/escritorio-auth-resolver.service';

const routes: Routes = [
  {
    path: 'relatorios/:tipo',
    component: RelatoriosGenericosComponent,
    children: [
      {
        path: 'criterios',
        component: CriteriosComponent
      },
      {
        path: 'escritorios',
        component: EscritorioComponent,
        resolve: {
          listaEscritorio: EscritoriosAuthResolver
        }
      },
      {
        path: 'empresa',
        component: EmpresasGrupoComponent
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RelatoriosGenericosRoutingModule { }
