import { NgModule } from '@angular/core';
import { Routes, RouterModule, PreloadAllModules, NoPreloading } from '@angular/router';

import { AuthGuard } from './core/services/auth-guard.service';

const routes: Routes = [
  {
    path: 'home',
    loadChildren: () =>
      import('./home/home.module').then((mod) => mod.HomeModule),
    canActivate: [AuthGuard],
  },
  // {
  //   path: 'relatorios',
  //   loadChildren: () =>
  //     import('./relatorios/relatorios.module').then(
  //       (mod) => mod.RelatoriosModule
  //     ),
  //   canActivate: [AuthGuard],
  // },
  {
    path: 'sap',
    loadChildren: () => import('./sap/sap.module').then((m) => m.SapModule),
    canActivate: [AuthGuard],
  },
  {
    path: 'manutencao',
    loadChildren: () =>
      import('./manutencao/manutencao.module').then((m) => m.ManutencaoModule),
    canActivate: [AuthGuard],
  },
  {
    path: 'pagamentos-rj',
    loadChildren: () =>
      import('./pagamentos/pagamentos.module').then((m) => m.PagamentosModule),
    canActivate: [AuthGuard],
  },
  {
    path: 'agenda',
    loadChildren: () =>
      import('./agenda/agenda.module').then((m) => m.AgendaModule),
    canActivate: [AuthGuard],
  },
  {
    path: 'processos',
    loadChildren: () =>
      import('./processos/processos.module').then((m) => m.ProcessosModule),
    canActivate: [AuthGuard],
  },
  {
    path: 'processos/agenda-audiencia',
    loadChildren: () =>
      import('./processos/agendaDeAudienciasDoCivelEstrategico/agendas.module').then((m) => m.AgendasModule),
      canActivate: [AuthGuard]
  },
  {
    path: 'controle-acesso',
    loadChildren: () => import('./controle-acesso/controle-acesso.module').then(mod => mod.ControleAcessoModule),
    canActivate: [AuthGuard],
  },
  {
    path: 'relatorio',
    loadChildren: () => import('./relatorios/relatorios.module').then(mod => mod.RelatoriosModule),
    canActivate: [AuthGuard],
  },
  {
    path: 'sap',
    loadChildren: () => import('./sap/sap.module').then(m => m.SapModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'fechamento',
    loadChildren: () => import('./fechamento/fechamento.module').then(m => m.FechamentoModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'esocial',
    loadChildren: () => import('./esocial/e-social.module').then(m => m.ESocialModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'workflow',
    loadChildren: () => import('./workflow/workflow.module').then(m => m.WorkflowModule), 
    canActivate: [AuthGuard]
  }
  // {
  //   path: "manutencao",
  //   loadChildren: () =>
  //     import("./manutencao/manutencao-aba/manutencao-aba.module").then(
  //       (m) => m.ManutencaoAbaModule
  //     ),
  //   canActivate: [AuthGuard],
  // },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      useHash: true,
      preloadingStrategy: NoPreloading,
      scrollPositionRestoration: 'enabled'
    }),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
