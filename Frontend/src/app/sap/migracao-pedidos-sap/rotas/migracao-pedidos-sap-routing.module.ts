import { FiltroNumeroDoLoteComponent } from './../../consultaLote/consultaLote-filtros/filtro-numero-do-lote/filtro-numero-do-lote.component';
import { Routes } from '@angular/router';

import { MigracaoPedidosSapComponent } from '../migracao-pedidos-sap.component';



export const MigracaoPedidosSapRouting: Routes = [
  {
    path: 'migracao-pedidos-sap',
    component: MigracaoPedidosSapComponent,
    //canActivate: [EstornoResultadoGuard]
   
  }
]
