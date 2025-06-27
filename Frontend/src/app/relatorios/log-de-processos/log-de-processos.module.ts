import { Injector, NgModule } from '@angular/core'; 
import { LogDeProcessosComponent } from './log-de-processos.component';
import { SharedModule } from '@shared/shared.module';
import { ComponentesModule } from 'src/app/componentes/componentes.module';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { LogDeProcessosRoutingModule } from './log-de-processos-routing.module';
import { AgendarRelatorioComponent } from './agendar-relatorio/agendar-relatorio.component';
import { IntrucoesParaCargaModalComponent } from './modals/intrucoes-para-carga-modal/intrucoes-para-carga-modal.component';
import { StaticInjector } from './static-injector'; 
import { SisjurPaginatorModule } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.module';



@NgModule({
  declarations: [
    LogDeProcessosComponent,
    AgendarRelatorioComponent,
    IntrucoesParaCargaModalComponent
  ],
  imports: [ 
    SharedModule,
    NgbModule,
    FormsModule,
    ComponentesModule,
    LogDeProcessosRoutingModule,
    SisjurPaginatorModule
  ],
  entryComponents:[
    IntrucoesParaCargaModalComponent
  ]
})
export class LogDeProcessosModule {
  constructor(private injector: Injector) {
    StaticInjector.setInjectorInstance(this.injector);
  } 
}

