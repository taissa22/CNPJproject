import { manutencaoSapRouting } from './manutenção/manutencao-routing/manutencao-sap-routing.module';
import { consultaRouting } from './consulta/consulta-routing/consulta-routing.module';
import { NgModule, Component } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { interfaceBBRota } from './interfacesBB/rotas/rota-interface-bb';
import { lotesRouting } from './lotes/lotes-routing/lotes-routing.module';
import { role } from './sap.constants';
import { MigracaoPedidosSapRouting } from './migracao-pedidos-sap/rotas/migracao-pedidos-sap-routing.module';
export const routes: Routes = [

    ...interfaceBBRota,
    ...manutencaoSapRouting,
    ...consultaRouting,
    ...lotesRouting,
    ...MigracaoPedidosSapRouting,


];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SapRoutingModule { }
