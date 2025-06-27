import {  Component, ViewChild } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { DialogService } from '@shared/services/dialog.service';
import { Sort } from '@shared/types/sort';
import { EMPTY } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { MotivoProvavelZero } from '@manutencao/models/motivo-provavel-zero';
import { MotivoProvavelZeroModalComponent } from '@manutencao/modals/motivo-provavel-zero-modal/motivo-provavel-zero-modal.component';
import { MotivoProvavelZeroService } from '@manutencao/services/motivo-provavel-zero.service';
import { HttpErrorResult } from '@core/http';

@Component({
  selector: 'app-motivo-provavel-zero',
  templateUrl: './motivo-provavel-zero.component.html',
  styleUrls: ['./motivo-provavel-zero.component.scss']
})
export class MotivoProvavelZeroComponent {

  dataSource: Array<MotivoProvavelZero> = new Array<MotivoProvavelZero>();
  total: number = 0;
  sort: Sort;

  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;
  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
 
  
  descricaoFormControl: FormControl = new FormControl('', [ Validators.required, Validators.maxLength(100)  ]);
 

  constructor(
    private service: MotivoProvavelZeroService,
    private dialog: DialogService,    
  ) { }

  iniciaValoresDaView() {
    let sort: any = this.table === undefined || !this.table.sortColumn ? "id" : this.table.sortColumn;
    return {
      sortColumn: sort,
      sortDirection: this.table === undefined || !this.table.sortDirection ? "asc" : this.table.sortDirection,
      pageIndex: this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize,
    }
  }

  buscar() {
    let view = this.iniciaValoresDaView();     
    this.service
      .obterPaginado(
        view.pageIndex,
        view.pageSize,
        view.sortColumn,
        view.sortDirection
      )
      .pipe(
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

  exportar() {
    let view = this.iniciaValoresDaView();

    this.service.exportar(view.sortColumn, view.sortDirection)
  }
  
  async incluir(): Promise<void> {
    const teveAlteracao: boolean = await MotivoProvavelZeroModalComponent.exibeModalDeIncluir();
    this.descricaoFormControl.setValue('');
    if (teveAlteracao) { this.buscar()}
  }

  async alterar(item: MotivoProvavelZero): Promise<void> {
    const teveAlteracao: boolean = await MotivoProvavelZeroModalComponent.exibeModalDeAlterar(item);
    this.descricaoFormControl.setValue('');
    if (teveAlteracao) { this.buscar()}
  }
 
  
  async excluir(item: MotivoProvavelZero): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Motivo Provável Zero',
      `Deseja excluir o Motivo Provável Zero <br><b>${item.descricao}</b>?`
    );

    if (!excluir) { return; }

    try {
      await this.service.excluir(item.id);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Motivo Provável Zero excluído!'
      );
      this.descricaoFormControl.setValue('');
      this.buscar();
    } catch (error) {
      if ((error as HttpErrorResult).messages.join().includes('registrado')) {
        await this.dialog.info(`Exclusão não permitida`, (error as HttpErrorResult).messages.join('\n'));
        return;
      }
      await this.dialog.err(`Exclusão não realizada`, (error as HttpErrorResult).messages.join('\n'));
    }
  }

}
