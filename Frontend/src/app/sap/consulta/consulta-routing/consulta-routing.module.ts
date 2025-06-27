import { Routes } from '@angular/router';
import { ConsultaSaldoGarantiaComponent } from '../consulta-saldo-garantia/consulta-saldo-garantia.component';
import { tela, role } from '../../sap.constants';
import { TipoProcessoResolverGuard } from '../../guards/tipo-processo-resolver.guard';
import { SaldoGarantiaCriteriosGeraisComponent } from '../consulta-saldo-garantia/filtros/saldo-garantia-criterios-gerais/saldo-garantia-criterios-gerais.component';
import { SaldoGarantiaBancoComponent } from '../consulta-saldo-garantia/filtros/saldo-garantia-banco/saldo-garantia-banco.component';
import { SaldoGarantiaEmpresaGrupoComponent } from '../consulta-saldo-garantia/filtros/saldo-garantia-empresa-grupo/saldo-garantia-empresa-grupo.component';
import { SaldoGarantiaEstadoComponent } from '../consulta-saldo-garantia/filtros/saldo-garantia-estado/saldo-garantia-estado.component';
import { SaldoGarantiaProcessosComponent } from '../consulta-saldo-garantia/filtros/saldo-garantia-processos/saldo-garantia-processos.component';
import { BuscarAgendamentosComponent } from '../consulta-saldo-garantia/buscar-agendamentos/buscar-agendamentos.component';
import {  AgendamentosSaldoGarantiaResolver } from '../consulta-saldo-garantia/resolvers/agendamentos.resolver';
import { RoleGuardService } from '../../../core/services/Roles.guard.ts.service';

export const consultaRouting: Routes = [
  {
    path: 'consulta',

    children: [
      {
        path: 'consultaSaldoGarantias',
        component: ConsultaSaldoGarantiaComponent,
        data: { tela: tela.consultaSaldoGarantia,
          role: role.menuConsultaSaldoGarantia},
        resolve: { tipoProcesso: TipoProcessoResolverGuard },
        canActivate : [RoleGuardService],
        children: [
          {
            path: 'criteriosGeraisGuia', component: SaldoGarantiaCriteriosGeraisComponent
          },
          {
            path: 'bancoGuia', component: SaldoGarantiaBancoComponent
          },
          {
            path: 'empresaGrupoGuia', component: SaldoGarantiaEmpresaGrupoComponent
          },
          {
            path: 'estadoGuia', component: SaldoGarantiaEstadoComponent
          },
          {
            path: 'processosGuia', component: SaldoGarantiaProcessosComponent
          },
          {
            path: '', redirectTo: 'criteriosGeraisGuia', pathMatch: 'full'
          }

        ]
      },
      {
        path: 'consultaSaldoGarantias/agendamentos',
        data:{ role: role.menuConsultaSaldoGarantia},
        canActivate : [RoleGuardService],
        component: BuscarAgendamentosComponent,
        resolve: {
          agendamentos: AgendamentosSaldoGarantiaResolver
        }
      },
    ]


  }
]
