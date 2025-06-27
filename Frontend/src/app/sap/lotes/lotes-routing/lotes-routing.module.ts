import { FiltroNumeroDoLoteComponent } from './../../consultaLote/consultaLote-filtros/filtro-numero-do-lote/filtro-numero-do-lote.component';
import { Routes } from '@angular/router';
import { role, tela } from '../../sap.constants';
import { RoleGuardService } from '../../../core/services/Roles.guard.ts.service';
import { ConsultaPageComponent } from '../../consultaLote/consulta-page/consulta-page.component';
import { TipoProcessoResolverGuard } from '../../guards/tipo-processo-resolver.guard';
import { filtrodataComponent } from '../../consultaLote/consultaLote-filtros/filtro-data/filtro-data.component';
import { FiltroSapComponent } from '../../consultaLote/consultaLote-filtros/filtro-sap/filtro-sap.component';
import { FiltroGuiaComponent } from '../../consultaLote/consultaLote-filtros/filtro-guia/filtro-guia.component';
import { FiltroProcessosComponent } from '../../consultaLote/consultaLote-filtros/filtro-processos/filtro-processos.component';
import { FiltroContaJudicialsComponent } from '../../consultaLote/consultaLote-filtros/filtro-numero-conta-judicial/filtro-numero-conta-judicial.component';
import { FiltroEmpresaGrupoComponent } from '../../consultaLote/consultaLote-filtros/filtro-empresa-grupo/filtro-empresa-grupo.component';
import { FiltroEscritorioComponent } from '../../consultaLote/consultaLote-filtros/filtro-escritorio/filtro-escritorio.component';
import { FiltroFornecedorComponent } from '../../consultaLote/consultaLote-filtros/filtro-fornecedor/filtro-fornecedor.component';
import { FiltroCentroCustoComponent } from '../../consultaLote/consultaLote-filtros/filtro-centro-custo/filtro-centro-custo.component';
import { FiltroTipoLancamentoComponent } from '../../consultaLote/consultaLote-filtros/filtro-tipo-lancamento/filtro-tipo-lancamento.component';
import { FiltroStatusLancamentoComponent } from '../../consultaLote/consultaLote-filtros/filtro-status-lancamento/filtro-status-lancamento.component';
import { FiltroCategoriaPagamentoComponent } from '../../consultaLote/consultaLote-filtros/filtro-categoria-pagamento/filtro-categoria-pagamento.component';
import { ResultadoSapComponent } from '../../consultaLote/resultado-sap/resultado-sap.component';
import { SapGuard } from '../../guards/sap.guard';
import { FiltrocriacaoLotesComponent } from '../../criacaoLote/filtro-criacao-lotes/filtro-criacao-lotes.component';
import { DeactivateGuard } from '../../guards/DeactivateGuard.guard';
import { ResultadoCriacaoComponent } from '../../criacaoLote/resultado-criacao/resultado-criacao.component';
import { EstornoLancamentosPagosComponent } from '../estorno-lancamentos-pagos/estorno-lancamentos-pagos.component';
import { EstornoResultadoComponent } from '../estorno-lancamentos-pagos/estorno-resultado/estorno-resultado.component';
import { EstornoResultadoGuard } from '../../guards/estorno-resultado.guard';
import { CriacaoLoteGuard } from '../../guards/criacao-lote.guard';



export const lotesRouting: Routes = [
  {
    path: 'lote',
    children: [
      {
        path: 'consulta',
        component: ConsultaPageComponent,
        resolve: { tipoProcesso: TipoProcessoResolverGuard },
        //o data é para passar para o resolver qual a tela atual estou e adicionar as permissões
        //necessárias, para não precisar criar vários tipos de endpoint.
        data: {
          tela: tela.consultaControleAcompanhamentoLote,
          role: role.menuConsultaControleAcompanhamentoLote
        },
        canActivate: [RoleGuardService],
        children: [
          {
            path: 'criteriosGeraisGuia', component: filtrodataComponent
          },
          {
            path: 'pedidoSapGuia', component: FiltroSapComponent,

          },
          {
            path: 'numeroGuiaSapGuia', component: FiltroGuiaComponent
          },
          {
            path: 'processosGuia', component: FiltroProcessosComponent
          },
          {
            path: 'numeroContaJudicialGuia', component: FiltroContaJudicialsComponent
          },
          {
            path: 'empresaGrupoGuia', component: FiltroEmpresaGrupoComponent
          },
          {
            path: 'escritorioGuia', component: FiltroEscritorioComponent
          },
          {
            path: 'fornecedorGuia', component: FiltroFornecedorComponent
          }, {
            path: 'centroCustoGuia', component: FiltroCentroCustoComponent
          }, {
            path: 'tipoLancamentoGuia', component: FiltroTipoLancamentoComponent
          },
          {
            path: 'statusPagamentoGuia', component: FiltroStatusLancamentoComponent
          },
          {
            path: 'categoriaPagamentoGuia', component: FiltroCategoriaPagamentoComponent
          },
          {
            path: 'filtroNumeroDoLote', component: FiltroNumeroDoLoteComponent
          },
          {
            path: '', redirectTo: 'criteriosGeraisGuia', pathMatch: 'full'
          },

        ]

      },

       {
        path: 'consulta/resultado',
        component: ResultadoSapComponent,
        canActivate: [SapGuard]
      },
      {
        path: 'criar',
        component: FiltrocriacaoLotesComponent,
        resolve: { tipoProcesso: TipoProcessoResolverGuard },
        data: { tela: tela.criacaoLote, role: role.menuCriacaoLote },
        canActivate: [RoleGuardService],
        canDeactivate: [DeactivateGuard],
      },
      {
        path: 'criar/resultado',
        component: ResultadoCriacaoComponent,
        canActivate : [CriacaoLoteGuard]
      },

      {
        path: 'lancamentos/estorno',
        component: EstornoLancamentosPagosComponent,
        data: { role: role.menuEstornoLancamento },
        canActivate : [RoleGuardService]
      }, {
        path: 'lancamentos/estorno/resultado',
        component: EstornoResultadoComponent,
        canActivate: [EstornoResultadoGuard]
      },
    ]
  }
]
