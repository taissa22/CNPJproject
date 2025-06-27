import { TrabalhistaResultadoComponent } from '../agendaDeAudiencias/trabalhista-filtros/trabalhista-resultado/trabalhista-resultado.component';
import { TrabalhistaVaraGuiaComponent } from '../agendaDeAudiencias/trabalhista-filtros/filtros/trabalhistaVaraGuia/trabalhistaVaraGuia.component';
import { TrabalhistaFiltrosComponent } from '../agendaDeAudiencias/trabalhista-filtros/trabalhista-filtros.component';
import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { ProcessosTrabalhistaComponent } from '../agendaDeAudiencias/trabalhista-filtros/filtros/processosTrabalhista/processosTrabalhista.component';
import { AdvogadosComponent } from '../agendaDeAudiencias/trabalhista-filtros/filtros/advogados/advogados.component';
import { AdvogadoAcompanhanteComponent } from '../agendaDeAudiencias/trabalhista-filtros/filtros/advogadoAcompanhante/advogadoAcompanhante.component';
import { ComarcaComponent } from '../agendaDeAudiencias/trabalhista-filtros/filtros/comarca/comarca.component';
import { EmpresaGrupoComponent } from '../agendaDeAudiencias/trabalhista-filtros/filtros/empresaGrupo/empresaGrupo.component';
import { EscritorioComponent } from '../agendaDeAudiencias/trabalhista-filtros/filtros/escritorio/escritorio.component';
import { EscritorioAcompanhanteComponent } from '../agendaDeAudiencias/trabalhista-filtros/filtros/escritorioAcompanhante/escritorioAcompanhante.component';
import { EstadoComponent } from '../agendaDeAudiencias/trabalhista-filtros/filtros/estado/estado.component';
import { PrepostoComponent } from '../agendaDeAudiencias/trabalhista-filtros/filtros/preposto/preposto.component';
import { PrepostoAcompanhanteComponent } from '../agendaDeAudiencias/trabalhista-filtros/filtros/prepostoAcompanhante/prepostoAcompanhante.component';
import { CriterioGeraisComponent } from '../agendaDeAudiencias/trabalhista-filtros/filtros/criterioGerais/criterioGerais.component';
import { AgendaAudienciaFiltrosTrabalhistaResolver } from '../guards/AgendaAudienciaFiltrosResolver';
import { CanActiveAgendaAudienciaResultado } from '../guards/CanActiveAgendaAudienciaResultado.guard';
import { RoleGuardService } from '@core/services/Roles.guard.ts.service';
import { role } from 'src/app/sap/sap.constants';
import { PautaJuizadoPesquisaComponent } from './../pauta-juizado/pesquisa/pauta-juizado-pesquisa.component';
import { PautaJuizadoComposicaoComponent } from '../pauta-juizado/composicao/pauta-juizado-composicao.component';
import { PautaCivelConsumidorPesquisaComponent } from '../pauta-civel-consumidor/pesquisa/pauta-civel-consumidor-pesquisa.component';
import { PautaCivelConsumidorComposicaoComponent } from '../pauta-civel-consumidor/composicao/pauta-civel-consumidor-composicao.component';
import { PautaProconPesquisaComponent } from '../pauta-procon/pesquisa/pauta-procon-pesquisa.component';
import { PautaProconComposicaoComponent } from '../pauta-procon/composicao/pauta-procon-composicao.component';
import { AgendaAudienciaTrabalhistaComponent } from '../agenda-audiencia/trabalhista/agenda-audiencia-trabalhista/agenda-audiencia-trabalhista.component';


export const AgendaRouting: Routes = [
  // {
  //   path: 'agendaAudienncia',

  //   children: [
  //     {
  //       path: 'trabalhista',
  //       component: TrabalhistaFiltrosComponent,
  //       resolve: { filtros: AgendaAudienciaFiltrosTrabalhistaResolver },
  //       data: { role: role.menuAgendaAudiencia },
  //       canActivate: [RoleGuardService],
  //       children: [
  //         {
  //           path: '', redirectTo: 'criteriosGuia', pathMatch: 'full'
  //         },
  //         {

  //           //criterio
  //           path: 'criteriosGuia', component: CriterioGeraisComponent
  //         },
  //         {
  //           //advogado
  //           path: 'advogadoGuia', component: AdvogadosComponent
  //         },
  //         {
  //           //advogado acompanhante
  //           path: 'advogadoAcompanhanteGuia', component: AdvogadoAcompanhanteComponent
  //         },
  //         {
  //           //comarca
  //           path: 'comarcaGuia', component: ComarcaComponent
  //         },
  //         {
  //           //empresa do grupo
  //           path: 'empresaDoGrupoGuia', component: EmpresaGrupoComponent
  //         },
  //         {
  //           //escritorio
  //           path: 'escritorioGuia', component: EscritorioComponent
  //         },
  //         {
  //           //escritorio acompanhante
  //           path: 'escritorioAcompanhanteGuia', component: EscritorioAcompanhanteComponent
  //         },
  //         {
  //           //estado
  //           path: 'estadoGuia', component: EstadoComponent
  //         },
  //         {
  //           //preposto
  //           path: 'prepostoGuia', component: PrepostoComponent
  //         },
  //         {
  //           //preposto acompanhante
  //           path: 'prepostoAcompanhanteGuia', component: PrepostoAcompanhanteComponent
  //         },
  //         {
  //           //criterio
  //           path: 'processosGuia', component: ProcessosTrabalhistaComponent
  //         },
  //         {
  //           //vara
  //           path: 'varaGuia', component: TrabalhistaVaraGuiaComponent
  //         }

  //       ]
  //     },
  //     {
  //       //vara
  //       path: 'trabalhista-resultado', component: TrabalhistaResultadoComponent,
  //       canActivate: [CanActiveAgendaAudienciaResultado]

  //     }
  //   ]
  // },
  {
    path: 'agendaAudienncia/trabalhista',
    component: AgendaAudienciaTrabalhistaComponent,
    data: { role: role.menuAgendaAudiencia },
    canActivate: [RoleGuardService]
  },{
    path: 'agenda-audiencia/pauta-juizado/pesquisa',
    component: PautaJuizadoPesquisaComponent,
    data: {
      role: 'mComposicao'
    },
    canActivate: [RoleGuardService]
  },
  {
    path: 'agenda-audiencia/pauta-juizado/composicao/:porJui/:dtIni/:dtFim/:tpAud/:empGrp/:UF/:com/:jui/:sitProc/:audSemPrep/:empCent/:grpJui/:stsAud/:prep',
    component: PautaJuizadoComposicaoComponent,
    data: {
      role: 'mComposicao'
    },
    canActivate: [RoleGuardService]
  },
  {
    path: 'agenda-audiencia/pauta-civel-consumidor/pesquisa',
    component: PautaCivelConsumidorPesquisaComponent,
    data: {
      role: 'mAgendaPautaCivel_Composicao'
    },
    canActivate: [RoleGuardService]
  },
  {
    path: 'agenda-audiencia/pauta-civel-consumidor/composicao/:dtIni/:dtFim/:tpAud/:empGrp/:UF/:com/:var/:sitProc/:audSemPrep/:empCent/:reqPre/:stsAud/:prep',
    component: PautaCivelConsumidorComposicaoComponent,
    data: {
      role: 'mAgendaPautaCivel_Composicao'
    },
    canActivate: [RoleGuardService]
  },
  {
    path: 'agenda-audiencia/pauta-procon/pesquisa',
    component: PautaProconPesquisaComponent,
    data: {
      role: 'mComposicaoProcon'
    },
    canActivate: [RoleGuardService]
  },
  { path: 'agenda-audiencia/pauta-procon/composicao/:porJui/:dtIni/:dtFim/:tpAud/:empGrp/:UF/:com/:jui/:sitProc/:audSemPrep/:empCent/:grpJui/:prep', component: PautaProconComposicaoComponent },
]

@NgModule({
  imports: [RouterModule.forChild(AgendaRouting)],
  exports: [RouterModule]
})
export class AgendaRoutingModule { }
