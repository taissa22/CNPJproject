import { AnexarNovoDocumentoModalComponent } from './modals/anexar-novo-documento-modal/anexar-novo-documento-modal.component';
import { NgModule, LOCALE_ID, Injector } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { SharedModule } from '@shared/shared.module';

import { LogCivelConsumidorComponent } from './civel-consumidor/log-civel-consumidor/log-civel-consumidor.component';
import { LogCivelEstrategicoComponent } from './civel-estrategico/log-civel-estrategico/log-civel-estrategico.component';
import { ProcessosRoutingModule } from './processos-routing.module';
import { LogProcessoResolver } from './guards/log-processo.resolver';
import { LogJuizadoEspecialComponent } from './juizado-especial/log-juizado-especial/log-juizado-especial.component';
import { LogProcessoService } from '../core/services/processos/log-processo/log-processo.service';
import { CoreModule } from '../core';
import { LogTrabalhistaComponent } from './trabalhista/log-trabalhista/log-trabalhista.component';
import { LogAdministrativoComponent } from './administrativo/log-administrativo/log-administrativo.component';
import { LogDescumprimentosComponent } from './descumprimentos/log-descumprimentos/log-descumprimentos.component';
import { LogTributarioJudicialComponent } from './tributario-judicial/log-tributario-judicial/log-tributario-judicial.component';
import { LogPexComponent } from './pex/log-pex/log-pex.component';
import { AdministrativoComponent } from './criminal/administrativo/administrativo.component';
import { ListarProcessoCriminalComponent } from './criminal/listar-processo-criminal/listar-processo-criminal.component';
import { JudicialComponent } from './criminal/judicial/judicial.component';
import { SisjurTableModule } from '@libs/sisjur/sisjur-table/sisjur-table.module';
import { SisjurPaginatorModule } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.module';
import { ParametrizarDistribuicaoProcessosEscritoriosComponent } from './parametrizar-distribuicao-processos-escritorios/parametrizar-distribuicao-processos-escritorios.component';
import { ParametrizarEscritorioComponent } from './parametrizar-distribuicao-processos-escritorios/parametrizar-escritorio/parametrizar-escritorio.component';
import { EscritorioModalComponent } from './modals/escritorio-modal/escritorio-modal.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { ComponentesModule } from '../componentes/componentes.module';
import { NovaParametrizacaoModalComponent } from './modals/nova-parametrizacao-modal/nova-parametrizacao-modal.component';
import { NgbTabsetModule, NgbTooltipModule } from '@ng-bootstrap/ng-bootstrap';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { AnexoModalComponent } from './modals/anexo-modal/anexo-modal.component';
import { RoleGuardService } from '@core/services/Roles.guard.ts.service';
import { StaticInjector } from './static-injector';

@NgModule({
  declarations: [
    LogCivelConsumidorComponent,
    LogCivelEstrategicoComponent,
    LogJuizadoEspecialComponent,
    LogTributarioJudicialComponent,
    LogTrabalhistaComponent,
    LogAdministrativoComponent,
    LogDescumprimentosComponent,
    LogPexComponent,
    AdministrativoComponent,
    JudicialComponent,
    ListarProcessoCriminalComponent,
    ParametrizarDistribuicaoProcessosEscritoriosComponent,
    ParametrizarEscritorioComponent,
    EscritorioModalComponent,
    NovaParametrizacaoModalComponent,
    AnexoModalComponent,
    AnexarNovoDocumentoModalComponent
  ],
  imports: [
    ProcessosRoutingModule,
    SharedModule,   
    FormsModule,
    SisjurTableModule,
    SisjurPaginatorModule,
    NgSelectModule,
    ComponentesModule,
    NgbTabsetModule,
    DragDropModule,
    NgbTooltipModule
  ],
  exports: [
    LogCivelConsumidorComponent,
    LogCivelEstrategicoComponent,
    LogJuizadoEspecialComponent,
    LogTributarioJudicialComponent,
    LogTrabalhistaComponent,
    LogAdministrativoComponent,
    LogDescumprimentosComponent,
    LogPexComponent,
    AdministrativoComponent,
    JudicialComponent,
    ListarProcessoCriminalComponent
  ],
  providers: [
    RoleGuardService,
    LogProcessoService,
    LogProcessoResolver,
    { provide: LOCALE_ID, useValue: 'pt' }
  ],
  entryComponents: [
    EscritorioModalComponent,
    NovaParametrizacaoModalComponent,
    AnexoModalComponent,
    AnexarNovoDocumentoModalComponent
  ]
})
export class ProcessosModule { 
  constructor(private injector: Injector) {
    StaticInjector.setInjectorInstance(this.injector);
  }
}
