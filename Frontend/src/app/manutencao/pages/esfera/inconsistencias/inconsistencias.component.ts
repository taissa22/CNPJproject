import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { HttpErrorResult } from '@core/http';
import { DialogService } from '@shared/services/dialog.service';
import { Page } from '@shared/types/page';
import { Sort, SortOf } from '@shared/types/sort';
import { EMPTY, merge } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { DatePipe } from '@angular/common';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { ProcessoInconsistente } from '@manutencao/models/processo-Inconsistente.model';
import { StaticInjector } from '@manutencao/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ProcessoTributarioInconsistenteService } from '@manutencao/services/processo-tributario-inconsistente.service';

@Component({
  selector: 'inconsistencias',
  templateUrl: './inconsistencias.component.html',
  styleUrls: ['./inconsistencias.component.scss']
})
export class InconsistenciaComponent {

  dataSource: Array<ProcessoInconsistente> = new Array<ProcessoInconsistente>();
  total: number = 0;
  sort: Sort;
  cargaInicial : boolean = true;

  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;
  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
 
  constructor(
    private modal: NgbActiveModal,
    private service: ProcessoTributarioInconsistenteService,
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
    if (!this.cargaInicial){
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
    this.cargaInicial = false;    
  }  

  exportar() {
    let view = this.iniciaValoresDaView();

    this.service.exportar(view.sortColumn, view.sortDirection)
  }
  
  static exibeModal(total : number, lista: Array<ProcessoInconsistente>): Promise<boolean> {    
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(InconsistenciaComponent, {windowClass: 'inconsistencia-modal', centered: true, size: 'sm', backdrop: 'static' });   
      modalRef.componentInstance.dataSource = lista;
      modalRef.componentInstance.total = total;      
    return modalRef.result;
  }

  close(): void {
    this.service.excluir();
    this.modal.close(false);    
  }


 
}
