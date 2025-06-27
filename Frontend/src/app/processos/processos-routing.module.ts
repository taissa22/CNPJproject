import { RoleGuardService } from '@core/services/Roles.guard.ts.service';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LogProcessoResolver } from './guards/log-processo.resolver';
import { LogCivelConsumidorComponent } from './civel-consumidor/log-civel-consumidor/log-civel-consumidor.component';
import { LogCivelEstrategicoComponent } from './civel-estrategico/log-civel-estrategico/log-civel-estrategico.component';
import { LogJuizadoEspecialComponent } from './juizado-especial/log-juizado-especial/log-juizado-especial.component';
import { LogTrabalhistaComponent } from './trabalhista/log-trabalhista/log-trabalhista.component';
import { LogAdministrativoComponent } from './administrativo/log-administrativo/log-administrativo.component';
import { LogDescumprimentosComponent } from './descumprimentos/log-descumprimentos/log-descumprimentos.component';
import { LogTributarioJudicialComponent } from './tributario-judicial/log-tributario-judicial/log-tributario-judicial.component';
import { LogPexComponent } from './pex/log-pex/log-pex.component';
import { AdministrativoComponent } from './criminal/administrativo/administrativo.component';
import { ListarProcessoCriminalComponent } from './criminal/listar-processo-criminal/listar-processo-criminal.component';
import { JudicialComponent } from './criminal/judicial/judicial.component';
import { ParametrizarDistribuicaoProcessosEscritoriosComponent } from './parametrizar-distribuicao-processos-escritorios/parametrizar-distribuicao-processos-escritorios.component';


const routes: Routes = [
    {
        path: 'civel-consumidor/log/:idProcesso/:codInterno',
        component: LogCivelConsumidorComponent,
        resolve: { ultOperacao: LogProcessoResolver },
    },
    {
        path: 'civel-estrategico/log/:idProcesso/:codInterno',
        component: LogCivelEstrategicoComponent,
        resolve: { ultOperacao: LogProcessoResolver },
    },
    {
        path: 'juizado-especial/log/:idProcesso/:codInterno',
        component: LogJuizadoEspecialComponent,
        resolve: { ultOperacao: LogProcessoResolver },
    },
    {
        path: 'trabalhista/log/:idProcesso/:codInterno',
        component: LogTrabalhistaComponent,
        resolve: { ultOperacao: LogProcessoResolver },
    },
    {
        path: 'tributario-judicial/log/:idProcesso/:codInterno',
        component: LogTributarioJudicialComponent,
        resolve: { ultOperacao: LogProcessoResolver },
    },
    {
        path: 'administrativo/log/:idProcesso/:codInterno',
        component: LogAdministrativoComponent,
        resolve: { ultOperacao: LogProcessoResolver },
    },
    {
        path: 'descumprimento/log/:idProcesso/:codInterno',
        component: LogDescumprimentosComponent,
        resolve: { ultOperacao: LogProcessoResolver },
    },
    {
        path: 'pex/log/:idProcesso/:codInterno',
        component: LogPexComponent,
        resolve: { ultOperacao: LogProcessoResolver },
    },
    {
        path: 'processos/criminal/administrativo',
        component: AdministrativoComponent,
    },
    {
        path: 'processos/criminal/judicial',
        component: JudicialComponent,
    },
    {
        path: 'processos/criminal/listar',
        component: ListarProcessoCriminalComponent,
    },
    {
        path: 'processos/parametrizar-distribuicao-processos-escritorios',
        component: ParametrizarDistribuicaoProcessosEscritoriosComponent,
        data: {
            role: 'm_ParamDistribuEscritorio'
        },
        canActivate: [RoleGuardService]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ProcessosRoutingModule { }
