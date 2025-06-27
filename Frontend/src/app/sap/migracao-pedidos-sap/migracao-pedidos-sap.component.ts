import { DialogService } from '@shared/services/dialog.service';
import { HttpErrorResult } from '@core/http/http-error-result';
import { Component, OnInit } from '@angular/core';
import { MigracaoPedidosSapServiceService } from './services/migracao-pedidos-sap-service.service';
import { MigracaoPedidos } from './models/migracao-pedidos-sap';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { role } from '../sap.constants';


@Component({
  // templateUrl: './carga-de-documentos.component.html',
  // styleUrls: ['./carga-de-documentos.component.scss']

  selector: 'app-migracao-pedidos-sap',
  templateUrl: './migracao-pedidos-sap.component.html',
  styleUrls: ['./migracao-pedidos-sap.component.scss']
})
export class MigracaoPedidosSapComponent implements OnInit {

  titulo: string = 'Migração de Pedidos SAP';
  breadcrumb: string = '';

  migracaoPedidos: Array<MigracaoPedidos> = [];
  totalPedidos: number = 0;
  pagina: number = 1;

  constructor(
    private service: MigracaoPedidosSapServiceService,
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService
  ) { }

  async ngOnInit(): Promise<void> {
    await this.obterPaginado();
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(role.menuMigracaoPedidoSAP);
  }

  async obterPaginado(): Promise<void> {

    try {
      const listaPedidos = await this.service.obterPaginado(this.pagina);
      listaPedidos.data.forEach((d: MigracaoPedidos) => this.migracaoPedidos.push(MigracaoPedidos.fromJson(d)));     
      this.totalPedidos = listaPedidos.total;

    } catch (error) {
      console.error(error);
      await this.dialog.err('Operação não realizada', (error as HttpErrorResult).messages.join('\n'));
    }
  }

  async obterMaisComprovantes(): Promise<void> {
    this.pagina += 1;
    await this.obterPaginado();
  }

  async redefinirListagem(): Promise<void> {
    this.migracaoPedidos = [];
    this.pagina = 1;

    await this.obterPaginado();
  }

  async excluir(id: number | string): Promise<void> {
    try {
      await this.service.excluir(id);
    } catch (error) {
      console.error(error);
      await this.dialog.err('Operação não realizada', (error as HttpErrorResult).messages.join('\n'));
      return;
    }
    await this.redefinirListagem();
  }

}
