import { Routes } from '@angular/router';
import { ManutencaoEmpresasSapComponent } from '../manutencao-empresas-sap/manutencao-empresas-sap.component';
import { role } from '../../sap.constants';
import { RoleGuardService } from '../../../core/services/Roles.guard.ts.service';
import { ManutencaoFornecedoresComponent } from '../manutencao-fornecedores/manutencao-fornecedores.component';
import { ManutencaoFornecedorBancoResolver } from '../../guards/manutencao-fornecedores-banco-resolver.guard';
import { ManutencaoFornecedorEscritorioResolver } from '../../guards/manutencao-fornecedores-escritorio-resolver.guard';
import { ManutencaoFornecedorProfissionaisResolver } from '../../guards/manutencao-fornecedores-profissionais-resolver.guard';
import { ManutencaoCategoriaPagamentoComponent } from '../manutencao-categoria-pagamento/manutencao-categoria-pagamento.component';
import { ManutencaoCentroCustoComponent } from '../manutencao-centro-custo/manutencao-centro-custo.component';
import { ManutencaoFormasPagamentoComponent } from '../manutencao-formas-pagamento/manutencao-formas-pagamento.component';
import { ManutencaoGrupoLoteJuizadoComponent } from '../manutencao-grupo-lote-juizado/manutencao-grupo-lote-juizado.component';
import { ManutencaoFornecedoresContingenciaSapComponent } from '../manutencao-fornecedores-contingencia-sap/manutencao-fornecedores-contingencia-sap.component';



export const manutencaoSapRouting: Routes = [
  {
    path: 'manutencao',

    children: [
      {
        path: 'empresasSap',
        component: ManutencaoEmpresasSapComponent,
        data: { role: role.menuManutencaoEmpresaSap },
        canActivate : [RoleGuardService]
      },
      {
        path: 'fornecedores',
        component: ManutencaoFornecedoresComponent,
        data: { role: role.menuManutencaoFornecedores },
        canActivate : [RoleGuardService],
        resolve: {
          banco: ManutencaoFornecedorBancoResolver,
          escritorio: ManutencaoFornecedorEscritorioResolver,
          profissionais: ManutencaoFornecedorProfissionaisResolver},
      },
      {
        path: 'categoriapagamento',
        component: ManutencaoCategoriaPagamentoComponent,
        data: { role: role.menuCategoriaPagamento },
        canActivate : [RoleGuardService]
      },
      {
        path: 'centroCusto',
        component: ManutencaoCentroCustoComponent,
        data: { role: role.menuManutencaoCentroCusto },
        canActivate : [RoleGuardService]
      },
      {
        path: 'formaspagamento',
        component: ManutencaoFormasPagamentoComponent,
        data: { role: role.menuManutencaoFormaPagamento },
        canActivate : [RoleGuardService]
      },
      {
        path: 'grupolotejuizado',
        data: { role: role.menuManutencaoGrupoLoteJuizado },
        canActivate : [RoleGuardService],
        component: ManutencaoGrupoLoteJuizadoComponent,
      },
      {
        path: 'fornecedoresContingencia',
        data: { role: role.menuManutencaoFornecedoresContingencia },
        canActivate: [RoleGuardService],
        component: ManutencaoFornecedoresContingenciaSapComponent
      }
    ]
  }
]
