import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { HttpErrorResult } from '@core/http';
import { IndicecorrecaoesferaModalComponent } from '@manutencao/modals/indice-correcao-esfera-modal/indice-correcao-esfera-modal.component';
import { Esfera } from '@manutencao/models/esfera.model';
import { IndiceCorrecaoEsfera } from '@manutencao/models/IndiceCorrecaoEsfera.model';
import { IndiceCorrecaoEsferaService } from '@manutencao/services/IndiceCorrecaoEsfera.service';
import { DialogService } from '@shared/services/dialog.service';
import { Page } from '@shared/types/page';
import { Sort, SortOf } from '@shared/types/sort';
import { EMPTY, merge } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { DatePipe } from '@angular/common';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { InconsistenciaComponent } from '../inconsistencias/inconsistencias.component';
import { ProcessoTributarioInconsistenteService } from '@manutencao/services/processo-tributario-inconsistente.service';

@Component({
  selector: 'indice-correcao-esfera',
  templateUrl: './indice-correcao-esfera.component.html',
  styleUrls: ['./indice-correcao-esfera.component.scss']
})
export class IndiceCorrecaoEsferaComponent {

  datasource: Array<IndiceCorrecaoEsfera> = new Array<IndiceCorrecaoEsfera>();
  total: number = 0;
  sort: Sort;

  @Input() esfera: Esfera;
  @Output() IndiceCorrecaoEsferaCarregadas: EventEmitter<IndiceCorrecaoEsfera[]> = new EventEmitter();
  constructor(
    private dialog: DialogService, 
    private service: IndiceCorrecaoEsferaService, 
    private serviceInconsistencia: ProcessoTributarioInconsistenteService, 
    private datepipe: DatePipe) { }

  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;
  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
 

  buscar() {
    let view = this.iniciaValoresDaView();
    let sortColumn: any = view.sortColumn;
    this.service.obterPaginado(this.esfera.id, view.pageIndex, view.pageSize, sortColumn, view.sortDirection)
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
        this.IndiceCorrecaoEsferaCarregadas.emit(this.datasource);
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

  async incluir(): Promise<void> {
    const teveAlteracao: boolean = await IndicecorrecaoesferaModalComponent.exibeModalDeIncluir(this.esfera.id);
    if (teveAlteracao) {            
      this.buscar();
    }    
  }

  async excluir(item: IndiceCorrecaoEsfera): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Índice de Correção',
      `Deseja excluir o Índice <br><b>${this.datepipe.transform(item.dataVigencia, 'dd/MM/yyyy') + '-' + item.indice.descricao  }</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      this.serviceInconsistencia.excluir();
      let retorno = await this.service.excluir(item.esferaId,item.dataVigencia, item.indiceId);

      if (retorno && (retorno.total > 0)){
        await InconsistenciaComponent.exibeModal(retorno.total, retorno.data); 
      }
      else{          
        await this.dialog.alert(
          'Exclusão realizada com sucesso',
          'Índice de Correção excluído!'
        );
      }
      this.buscar();
    } catch (error) {

      await this.dialog.err(`Exclusão não realizada`, (error as HttpErrorResult).messages.join('\n'));
    }
  }








}
