import { HttpErrorResult } from './../../core/http/http-error-result';
import { DialogService } from './../../shared/services/dialog.service';

import { isNullOrUndefined } from 'util';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Component, OnInit, DoCheck, ViewChild } from '@angular/core';
import { Comprovante } from '../models/comprovante.model';
import { CargaComprovantesService } from '../services/carga-comprovantes.service';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';

@Component({
  templateUrl: './carga-de-comprovantes.component.html',
  styleUrls: ['./carga-de-comprovantes.component.scss']
})
export class CargaDeComprovantesComponent implements OnInit {

  comprovantes: Array<Comprovante> = [];
  totalAgendamentos: number;
  pagina: number = 1;

  titulo: string = 'Carga de Comprovante de Pagamento';
  breadcrumb: string = '';

  constructor(private service: CargaComprovantesService, 
              private dialog: DialogService,
              private breadcrumbsService: BreadcrumbsService) { }

  async ngOnInit() {
    await this.obterPaginado();
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_CARGA_COMPROVANTE_PAGAMENTO_RJ);
  }

  async novo(): Promise<void> {

  }

  async redefinirListagem(): Promise<void> {
    this.pagina = 1;
    this.comprovantes = [];

    await this.obterPaginado();
  }

  async obterMaisComprovantes(): Promise<void> {
    this.pagina += 1;
    await this.obterPaginado();
  }

  async obterPaginado(): Promise<void> {
    try {
      const listaComprovantes = await this.service.obterPaginado(this.pagina);
      listaComprovantes.data.forEach((c: Comprovante) => this.comprovantes.push(Comprovante.fromJson(c)));
      this.totalAgendamentos = listaComprovantes.total;
    } catch (error) {
      console.error(error);
      await this.dialog.showErr('Operação não realizada', (error as HttpErrorResult).messages.join('\n'));
    }
  }

  async excluir(id: number | string): Promise<void> {
    try {
      await this.service.excluir(id);
    } catch (error) {
      console.error(error);
      await this.dialog.showErr('Operação não realizada', (error as HttpErrorResult).messages.join('\n'));
      return;
    }
    await this.redefinirListagem();
  }

}
