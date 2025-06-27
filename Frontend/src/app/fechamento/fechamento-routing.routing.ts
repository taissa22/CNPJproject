import { AgendarSolicitarFechamentoContingenciaComponent } from './contigencia/agendar-solicitar-fechamento-contingencia/agendar-solicitar-fechamento-contingencia.component';
import { Permissoes } from 'src/app/permissoes/permissoes';
import { Routes, RouterModule } from '@angular/router';
import { GrupoDeEstadosComponent } from './parametrizacao-contingencia-pex-grupo-estados/parametrizacao-contingencia-pex-grupo-estados.component';
import { AuthGuard } from './../core/services/auth-guard.service';
import { RoleGuardService } from '../core/services/Roles.guard.ts.service';
import { NgModule } from '@angular/core';
import { ApurarValorCorteOutliersJuizadoEspecialComponent } from './contigencia/apurar-valor-corte-outliers-juizado-especial/apurar-valor-corte-outliers-juizado-especial.component';
import { LogProcessamentoContingenciaComponent } from './contigencia/log-processamento-contingencia/log-processamento-contingencia.component';


const routes: Routes = [
  {
    path: 'parametrizacao-contingencia-pex-grupo-estados',
    component: GrupoDeEstadosComponent,
    data: {
      role: Permissoes.ACESSAR_GRUPO_DE_ESTADOS
    },
    canActivate: [RoleGuardService]
  },
  {
    path: 'contingencia/apurar-valor-corte-outliers-juizado-especial',
    component: ApurarValorCorteOutliersJuizadoEspecialComponent,
    data: {
      role: 'm_ApurarValorCorteOutliersJEC'
    },
    canActivate: [RoleGuardService]
  },
  {
    path: 'contingencia/agendar-solicitar-fechamento-contingencia',
    component: AgendarSolicitarFechamentoContingenciaComponent,
    // data: {
    //   role: 'm_ApurarValorCorteOutliersJEC'
    // },
    // canActivate: [RoleGuardService]
  },
  {
    path: 'contingencia/fila-execucao-fechamento-contingencia',
    component: LogProcessamentoContingenciaComponent,
    // data: {
    //   role: 'm_ApurarValorCorteOutliersJEC'
    // },
    // canActivate: [RoleGuardService]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class FechamentoRoutingRoutes { }
