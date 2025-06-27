import { Component, OnInit } from '@angular/core';

// angular
import { AfterViewInit, ViewChild } from '@angular/core';

// 3rd party
import { BehaviorSubject, EMPTY, merge } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';

import { DialogService } from '@shared/services/dialog.service';
import { Page } from '@shared/types/page';
import { Sort, SortOf } from '@shared/types/sort';

// local imports
import { TipoDePrazo } from '@manutencao/models/tipo-de-prazo';
import { TipoDePrazoService } from '@manutencao/services/tipo-de-prazo.service';
import { TipoDePrazoModalComponent } from '@manutencao/modals/tipo-de-prazo-modal/tipo-de-prazo-modal.component';
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';
import { HttpErrorResult } from '@core/http';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';
//import { TipoDePrazoServiceMock } from '@manutencao/services/tipo-de-prazo.service.mock';


@Component({

  selector: 'app-tipo-de-prazo',
  templateUrl: './tipo-de-prazo.component.html',
  styleUrls: ['./tipo-de-prazo.component.scss'],
})

export class TipoDePrazoComponent implements AfterViewInit {
  breadcrumb: string;

  constructor(
    private service: TipoDePrazoService,
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService,
  ) { }

  tiposDePrazo: TipoDePrazo[] = [];
  total: number = 0;
  sort: Sort;
  exibirTabela : Boolean = false;

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  tiposProcesso: Array<TiposProcesso> = [];
  tiposProcessoFormControl : FormControl = new FormControl(null, Validators.required);
  descricaoFormControl : FormControl = new FormControl();
  exibeEstrategico: boolean = false;
  exibeConsumidor: boolean = false;

  formGroup: FormGroup = new FormGroup({
    tipoProcesso: this.tiposProcessoFormControl,
    descicao: this.descricaoFormControl
  });

  async ngAfterViewInit() {
    this.tiposProcesso = await this.service.getTipoProcesso();
    
    this.tiposProcesso.sort((a,b) => {
      return a.descricao.localeCompare(b.descricao);
    })
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_TIPO_PRAZO);    
  }

  async obter(): Promise<void> {
      try {
        let sort: SortOf<any> = { column: this.iniciaValoresDaView().sortColumn , direction: this.iniciaValoresDaView().sortDirection};
        const page: Page = { index: this.iniciaValoresDaView().pageIndex, size: this.iniciaValoresDaView().pageSize};

        const result = await this.service.obter(sort, page, this.tiposProcessoFormControl.value, this.descricaoFormControl.value);
        this.tiposDePrazo = result.data;
        this.total = result.total;

    } catch (error) {
        this.dialog.err(
          'Informações não carregadas',
          'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte'
        );
      }
  }

  iniciaValoresDaView() {
    return {
      sortColumn: this.table === undefined || !this.table.sortColumn ? "descricao" : this.table.sortColumn,
      sortDirection: this.table === undefined || !this.table.sortDirection ? "asc" : this.table.sortDirection,
      pageIndex: this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize,
    }
  }

  async incluir(): Promise<void> {
    const teveAlteracao: boolean = await TipoDePrazoModalComponent.exibeModalDeIncluir(this.tiposProcesso,this.tiposProcessoFormControl.value);
    if (teveAlteracao) {
      this.buscar();
    }
  }

  async alterar(item: TipoDePrazo): Promise<void> {
    // prettier-ignore
    const teveAlteracao: boolean = await TipoDePrazoModalComponent.exibeModalDeAlterar(item,this.tiposProcesso,this.tiposProcessoFormControl.value);
    if (teveAlteracao) {
      this.buscar();
    }
  }

  async excluir(item: TipoDePrazo): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Tipo de Prazo',
      `Deseja excluir o tipo de prazo<br><b>${item.descricao}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(item.codigo);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'O registro foi excluído do sistema.'
      );
      this.buscar();
    } catch (error) {
      if ((error as HttpErrorResult).messages.join('\n').includes('prazo do processo')) {
        this.dialog.info('Exclusão não permitida', (error as HttpErrorResult).messages.join('\n'));
      } else {
        this.dialog.err('Exclusão não realizada', (error as HttpErrorResult).messages.join('\n'));
        throw error;
      }
    }
  }


  buscar() {
    this.exibirTabela = true;
    this.exibeEstrategico = this.tiposProcessoFormControl.value === 9 ? true : false;
    this.exibeConsumidor = this.tiposProcessoFormControl.value === 1 ? true : false;
    this.obter();
  }

  exportar() {
    let sort: SortOf<any> = { column: this.iniciaValoresDaView().sortColumn , direction: this.iniciaValoresDaView().sortDirection};
    this.service.exportar(sort, this.tiposProcessoFormControl.value, this.descricaoFormControl.value)
  }

}
