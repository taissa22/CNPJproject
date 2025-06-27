import { NgModule, LOCALE_ID } from '@angular/core';
import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';
import { FormsModule } from '@angular/forms';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from '../../shared';
import { ComponentesModule } from '../../componentes/componentes.module';

import { RelatoriosGenericosComponent } from './relatorios-genericos.component';
import { RelatoriosGenericosRoutingModule } from './relatorios-genericos.routing.module';
import { CriteriosComponent } from './criterios/criterios.component';
import { EscritorioComponent } from './escritorio/escritorio.component';
import { EmpresasGrupoComponent } from './empresas-grupo/empresas-grupo.component';
import { RelatorioService } from '../../core/services/relatorio.service';
import { RelatoriosGenericosAuthResolver } from './relatorios-genericos-auth-resolver.service';
import { EscritoriosAuthResolver } from './escritorio/escritorio-auth-resolver.service';
registerLocaleData(localePt);
@NgModule({
  imports: [
    SharedModule,
    RelatoriosGenericosRoutingModule,
    NgbModule,
    FormsModule,
    ComponentesModule

  ],
  declarations: [
    RelatoriosGenericosComponent,
    CriteriosComponent,
    EscritorioComponent,
    EmpresasGrupoComponent
  ],
  providers: [
    EscritoriosAuthResolver,
    RelatorioService,
    RelatoriosGenericosAuthResolver,
    { provide: LOCALE_ID, useValue: 'pt' }
  ]
})
export class RelatoriosGenericosModule { }
