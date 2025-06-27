import { ModalAlteracaoAudienciaComponent } from './agendaDeAudiencias/trabalhista-filtros/trabalhista-resultado/modal/modalAlteracaoAudiencia/modalAlteracaoAudiencia.component';
import { BsDatepickerModule, ModalModule } from 'ngx-bootstrap';
import { TrabalhistaResultadoComponent } from './agendaDeAudiencias/trabalhista-filtros/trabalhista-resultado/trabalhista-resultado.component';
import { TrabalhistaVaraGuiaComponent } from './agendaDeAudiencias/trabalhista-filtros/filtros/trabalhistaVaraGuia/trabalhistaVaraGuia.component';
import { TrabalhistaFiltrosComponent } from './agendaDeAudiencias/trabalhista-filtros/trabalhista-filtros.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AgendaRoutingModule } from './agenda-routing/agenda-routing.module';
import { ComponentesModule } from '../componentes/componentes.module';

import { ProcessosTrabalhistaComponent } from './agendaDeAudiencias/trabalhista-filtros/filtros/processosTrabalhista/processosTrabalhista.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
import { AdvogadosComponent } from './agendaDeAudiencias/trabalhista-filtros/filtros/advogados/advogados.component';
import { AdvogadoAcompanhanteComponent } from './agendaDeAudiencias/trabalhista-filtros/filtros/advogadoAcompanhante/advogadoAcompanhante.component';
import { ComarcaComponent } from './agendaDeAudiencias/trabalhista-filtros/filtros/comarca/comarca.component';
import { EmpresaGrupoComponent } from './agendaDeAudiencias/trabalhista-filtros/filtros/empresaGrupo/empresaGrupo.component';
import { EscritorioComponent } from './agendaDeAudiencias/trabalhista-filtros/filtros/escritorio/escritorio.component';
import { EscritorioAcompanhanteComponent } from './agendaDeAudiencias/trabalhista-filtros/filtros/escritorioAcompanhante/escritorioAcompanhante.component';
import { EstadoComponent } from './agendaDeAudiencias/trabalhista-filtros/filtros/estado/estado.component';
import { PrepostoComponent } from './agendaDeAudiencias/trabalhista-filtros/filtros/preposto/preposto.component';
import { PrepostoAcompanhanteComponent } from './agendaDeAudiencias/trabalhista-filtros/filtros/prepostoAcompanhante/prepostoAcompanhante.component';
import { CriterioGeraisComponent } from './agendaDeAudiencias/trabalhista-filtros/filtros/criterioGerais/criterioGerais.component';
import { AgendaAudienciaFiltrosTrabalhistaResolver } from './guards/AgendaAudienciaFiltrosResolver';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ResultadoAgendaAudienciaAliasPipe } from './agendaDeAudiencias/trabalhista-filtros/pipe/ResultadoAgendaAudienciaAlias.pipe';
import { CanActiveAgendaAudienciaResultado } from './guards/CanActiveAgendaAudienciaResultado.guard';
import { PautaJuizadoPesquisaComponent } from './pauta-juizado/pesquisa/pauta-juizado-pesquisa.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { PautaJuizadoComposicaoComponent } from './pauta-juizado/composicao/pauta-juizado-composicao.component';
import { SisjurTableModule } from '@libs/sisjur/sisjur-table/sisjur-table.module';
import { SisjurPaginatorModule } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.module';
import { JurPicklistModule } from '@shared/components/jur-picklist/jur-picklist.module';
import { PautaCivelConsumidorPesquisaComponent } from './pauta-civel-consumidor/pesquisa/pauta-civel-consumidor-pesquisa.component';
import { PautaCivelConsumidorComposicaoComponent } from './pauta-civel-consumidor/composicao/pauta-civel-consumidor-composicao.component';
import { PautaProconPesquisaComponent } from './pauta-procon/pesquisa/pauta-procon-pesquisa.component';
import { PautaProconComposicaoComponent } from './pauta-procon/composicao/pauta-procon-composicao.component';
import { AgendaAudienciaTrabalhistaComponent } from './agenda-audiencia/trabalhista/agenda-audiencia-trabalhista/agenda-audiencia-trabalhista.component';

@NgModule({
  declarations: [
    TrabalhistaFiltrosComponent,
    TrabalhistaVaraGuiaComponent,
    ProcessosTrabalhistaComponent,
    AdvogadosComponent,
    AdvogadoAcompanhanteComponent,
    ComarcaComponent,
    EmpresaGrupoComponent,
    EscritorioComponent,
    EscritorioAcompanhanteComponent,
    EstadoComponent,
    PrepostoComponent,
    PrepostoAcompanhanteComponent,
    CriterioGeraisComponent,
    TrabalhistaResultadoComponent,
    ModalAlteracaoAudienciaComponent,
    ResultadoAgendaAudienciaAliasPipe,
    PautaJuizadoPesquisaComponent,
    PautaJuizadoComposicaoComponent,
    PautaCivelConsumidorPesquisaComponent,
    PautaCivelConsumidorComposicaoComponent,
    PautaProconPesquisaComponent,
    PautaProconComposicaoComponent,

    AgendaAudienciaTrabalhistaComponent
  ],
  imports: [
    CommonModule,
    AgendaRoutingModule,
    ComponentesModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    ModalModule.forRoot(),
    NgbModule,
    BsDatepickerModule.forRoot(),
    NgSelectModule,
    SisjurTableModule,
    SisjurPaginatorModule,
    JurPicklistModule
  ],
  providers: [
    AgendaAudienciaFiltrosTrabalhistaResolver,
    CanActiveAgendaAudienciaResultado
  ],
  entryComponents: [ModalAlteracaoAudienciaComponent]
})
export class AgendaModule { }
