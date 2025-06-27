import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { BehaviorSubject, EMPTY, merge } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { DialogService } from '@shared/services/dialog.service';
import { Sort } from '@shared/types/sort';
import { OrientacaoJuridicaTrabalhista } from '@manutencao/models/orientacao-juridica-trabalhista';

import { OrientacaoJuridicaTrabalhistaModalComponent } from '@manutencao/modals/orientacao-juridica-trabalhista-modal/orientacao-juridica-trabalhista-modal.component';
import { OrientacaoJuridicaService } from '@manutencao/services/orientacaoJuridica.service';
import { FormControl } from '@angular/forms';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { HttpErrorResult } from '@core/http';

@Component({
  selector: 'app-orientacao-juridica-trabalhista',
  templateUrl: './orientacao-juridica-trabalhista.component.html',
  styleUrls: ['./orientacao-juridica-trabalhista.component.scss'],  
})
export class OrientacaoJuridicaTrabalhistaComponent implements AfterViewInit {
  constructor(
    private service: OrientacaoJuridicaService,
    private dialog: DialogService
  ) {}

  
  buscarDescricaoFormControl : FormControl = new FormControl(null);

  sort : Sort;

  dataSource: Array<OrientacaoJuridicaTrabalhista> = [];
  total: number = 0;
  
  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;  


  nome: string;

  ngAfterViewInit(): void {
    this.buscarTabela(); 
  }

  async incluir(): Promise<void> {
    const teveAlteracao: boolean = await OrientacaoJuridicaTrabalhistaModalComponent.exibeModalDeIncluir();
    if (teveAlteracao) {
      this.buscarTabela();
    }
  }

  async alterar(item: OrientacaoJuridicaTrabalhista): Promise<void> {
    // prettier-ignore
    const teveAlteracao: boolean = await OrientacaoJuridicaTrabalhistaModalComponent.exibeModalDeAlterar(item);
    if (teveAlteracao) {
      this.buscarTabela();
    }
  }

  async excluir(item: OrientacaoJuridicaTrabalhista): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Orientação Jurídica Trabalhista',
      `Deseja excluir a orientação<br><b>${item.nome}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(item.codOrientacaoJuridica);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Orientação excluída!'
      );
      this.buscarTabela();
    } catch (error) {
      if ((error as HttpErrorResult).messages.join('\n').includes('relacionada')) {
        this.dialog.info('Exclusão Não Permitida', (error as HttpErrorResult).messages.join('\n'));
      } else {
        this.dialog.err('Exclusão Não Realizada', (error as HttpErrorResult).messages.join('\n'));
        throw error;
      }
    }
  }

  buscarTabela() {
    let view = this.iniciaValoresDaView();
    this.service
      .obterPaginado(
        view.pageIndex,
        view.pageSize,
        true,
        view.sortColumn,
        view.sortDirection,
        this.buscarDescricaoFormControl.value
      ).pipe(
        catchError(() => {
          this.dialog.err(
            'Informações não carregadas',
            'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte'
          );
          return EMPTY;
        })
      )
      .subscribe(data => {
        this.total = data.total;
        this.dataSource = data.data;
      });
  }  

  iniciaValoresDaView() {
    let sort: any = this.table === undefined || !this.table.sortColumn ? "codigo" : this.table.sortColumn;
    return {
      sortColumn: sort,
      sortDirection: this.table === undefined || !this.table.sortDirection ? "asc" : this.table.sortDirection,
      pageIndex: this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize,
    }
  }

  exportar( ){
    this.service.exportar(true, this.iniciaValoresDaView().sortColumn, this.iniciaValoresDaView().sortDirection, this.buscarDescricaoFormControl.value)
  }
}
