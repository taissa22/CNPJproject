import { HttpErrorResult } from '@core/http';
import { IndicecorrecaoesferaModalComponent } from '@manutencao/modals/indice-correcao-esfera-modal/indice-correcao-esfera-modal.component';
import { DialogService } from '@shared/services/dialog.service';
import { Page } from '@shared/types/page';
import { Sort, SortOf } from '@shared/types/sort';
import { EMPTY, merge } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { DatePipe } from '@angular/common';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { DecisaoEvento } from '@manutencao/models/decisao-evento.model';
import { DecisaoEventoService } from '@manutencao/services/decisao-evento.service';
import { Evento } from '@manutencao/models/evento.model';
import { Component, Input, ViewChild } from '@angular/core';
import { EventoService } from '@manutencao/services/evento.service';
import { EventoDependenteModalComponent } from '@manutencao/modals/evento-dependente-modal/evento-dependente-modal.component';

@Component({
  selector: 'evento-dependente',
  templateUrl: './evento-dependente.component.html',
  styleUrls: ['./evento-dependente.component.scss']
})
export class EventoDependenteComponent {

  datasource: Array<Evento> = new Array<Evento>();
  total: number = 0;
  sort: Sort;

  @Input() evento : Evento; 

  constructor(
    private dialog: DialogService, 
    private service: EventoService, 
    private datepipe: DatePipe) { }

  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;
  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
 

  buscar() {
    this.service.obterDependente(this.iniciaValoresDaView().sortColumn,
                                 this.iniciaValoresDaView().sortDirection,
                                 this.iniciaValoresDaView().pageIndex,
                                 this.iniciaValoresDaView().pageSize,
                                 this.evento.id)
      .pipe(
        catchError(() => {
          this.dialog.err(
            'Informações não carregadas',
            'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte'
          );
          return EMPTY;
        })
      )
      .subscribe(resposta => {
        this.datasource = resposta.data;
        this.total = resposta.total;
      });
  }
 
  iniciaValoresDaView() {
    return {
      sortColumn: this.table === undefined || !this.table.sortColumn ? "Id" : this.table.sortColumn,
      sortDirection: this.table === undefined || !this.table.sortDirection ? "asc" : this.table.sortDirection,
      pageIndex: this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize,
    }
  }

  async incluirAlterar(): Promise<void> {
    const teveAlteracao: boolean = await EventoDependenteModalComponent.exibeModal(this.evento.id);
    if (teveAlteracao) {
      this.buscar();
    }    
  }   
}
