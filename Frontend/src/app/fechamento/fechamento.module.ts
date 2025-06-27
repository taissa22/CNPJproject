import { LOCALE_ID, NgModule } from '@angular/core';
import { ComponentesModule } from '../componentes/componentes.module';
import { FechamentoRoutingRoutes } from './../fechamento/fechamento-routing.routing';
import { InputMaskModule } from '@libs/racoon-mask/input-mask.module';
import { GrupoDeEstadosComponent } from './parametrizacao-contingencia-pex-grupo-estados/parametrizacao-contingencia-pex-grupo-estados.component';

import { SharedModule } from '@shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { TooltipModule } from 'ngx-bootstrap';
import { RoleGuardService } from '@core/services/Roles.guard.ts.service';

import { ApurarValorCorteOutliersJuizadoEspecialComponent } from './contigencia/apurar-valor-corte-outliers-juizado-especial/apurar-valor-corte-outliers-juizado-especial.component';
import { AgendarSolicitarFechamentoContingenciaComponent } from './contigencia/agendar-solicitar-fechamento-contingencia/agendar-solicitar-fechamento-contingencia.component';
import { AgendarSolicitarFechamentoContingenciaModalComponent } from './contigencia/modals/agendar-solicitar-fechamento-contingencia-modal/agendar-solicitar-fechamento-contingencia-modal.component';
import { FechamentoCcComponent } from './contigencia/modals/fechamento-cc/fechamento-cc.component';
import { FechamentoCcMediaComponent } from './contigencia/modals/fechamento-cc-media/fechamento-cc-media.component';
import { FechamentoCivelEstrategicoComponent } from './contigencia/modals/fechamento-civel-estrategico/fechamento-civel-estrategico.component';
import { FechamentoJecComponent } from './contigencia/modals/fechamento-jec/fechamento-jec.component';
import { FechamentoPexMediaComponent } from './contigencia/modals/fechamento-pex-media/fechamento-pex-media.component';
import { FechamentoTrabMediaComponent } from './contigencia/modals/fechamento-trab-media/fechamento-trab-media.component';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { LogProcessamentoContingenciaComponent } from './contigencia/log-processamento-contingencia/log-processamento-contingencia.component';
import { SisjurTableModule } from '@libs/sisjur/sisjur-table/sisjur-table.module';
import { SisjurPaginatorModule } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.module';
@NgModule({
  declarations: [
    ApurarValorCorteOutliersJuizadoEspecialComponent,
    GrupoDeEstadosComponent,
    AgendarSolicitarFechamentoContingenciaComponent,
    AgendarSolicitarFechamentoContingenciaModalComponent,
    FechamentoCcComponent,
    FechamentoCcMediaComponent,
    FechamentoCivelEstrategicoComponent,
    FechamentoJecComponent,
    FechamentoPexMediaComponent,
    FechamentoTrabMediaComponent,
    LogProcessamentoContingenciaComponent
  ],
  entryComponents: [
    AgendarSolicitarFechamentoContingenciaModalComponent
  ],
  imports: [
    SharedModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule,
    TooltipModule.forRoot(),
    ComponentesModule,
    FechamentoRoutingRoutes,
    SisjurTableModule,
    SisjurPaginatorModule,
    InputMaskModule,
    TooltipModule,
    CurrencyMaskModule
  ],
  providers: [
    RoleGuardService,
    { provide: LOCALE_ID, useValue: 'pt' }
  ]
})
export class FechamentoModule { }
