import { RoleGuardService } from './../core/services/Roles.guard.ts.service';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AlteracaoProcessoBlocoWebComponent } from './alteracao-processo-bloco-web/alteracao-processo-bloco-web.component';
import { ExtracaoBasePrePosRjComponent } from './D-1/extracao-base-pre-pos-rj/extracao-base-pre-pos-rj.component';
import { CCPorMediaComponent } from './civel-consumidor/cc-por-media/cc-por-media.component';
import { PexPorMediaComponent } from './pex/pex-por-media/pex-por-media.component';
import { UsuarioPerfilPermissaoComponent } from './usuario-perfil-permissao/usuario-perfil-permissao-juizado-especial.component';
import { RelatorioDeSolicitacoesComponent } from './pex/relatorio-de-solicitacoes/relatorio-de-solicitacoes.component';
import { Permissoes } from '../permissoes/permissoes';
import { NegociacoesComponent } from './negociacoes/negociacoes.component';
import { RelatorioATMCCComponent } from './contingencia/atm/cc/relatorio-atm-cc.component';

const routes: Routes = [
  {
    path: 'alteracao-processo-bloco-web',
    component: AlteracaoProcessoBlocoWebComponent,
    data: {
      role: 'm_AlteracaoProcBlocoWeb'
    },
    canActivate: [RoleGuardService]
  },
  {
    path: 'extracao-base-pre-pos-rj',
    component: ExtracaoBasePrePosRjComponent,
    data: {
      role: 'm_ExtracaoBasePrePos'
    },
    canActivate: [RoleGuardService]
  },
  {
    path: 'contingencia',
    loadChildren: () =>
      import('@relatorios/contingencia/contingencia.module').then(
        m => m.ContingenciaModule
      )
  },
  {
    path: 'cc-por-media',
    component: CCPorMediaComponent,
    data: {
      role: 'm_RelContCCMedia'
    },
    canActivate: [RoleGuardService]
  },
  {
    path: 'pex-por-media',
    component: PexPorMediaComponent,
    data: {
      role: 'm_RelContPEXMedia'
    },
    canActivate: [RoleGuardService]
  },
  {
    path: 'usuario-perfil-permissao',
    component: UsuarioPerfilPermissaoComponent,
    data: {
       role: 'm_RelatorioUsuarioPerfil'
    }
  },
  {
    path: 'relatorio-de-solicitacoes',
    loadChildren: () =>
      import('@relatorios/pex/relatorio-de-solicitacoes/relatorio-de-solicitacoes.module').then(
        m => m.RelatorioDeSolicitacoesModule
      ),
    data: {
      role: 'm_RelatorioSolicitacoesPEX'
    },
    canActivate: [RoleGuardService]
  },
  {
    path: 'log-de-processos',
    loadChildren: () =>
      import('@relatorios/log-de-processos/log-de-processos.module').then(
        m => m.LogDeProcessosModule
      )
  },
  {
    path: 'relatorio-negociacao',
    component: NegociacoesComponent,
    canActivate: [RoleGuardService],
    data: {
       role: 'm_RelatorioNegociacoes'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RelatoriosRoutingModule {}
