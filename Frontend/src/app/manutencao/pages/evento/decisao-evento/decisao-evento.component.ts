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
import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { DecisaoEventoModalComponent } from '@manutencao/modals/decisao-evento-modal/decisao-evento-modal.component';
import { TrabalhistaJudicialComponent } from '@manutencao/manutencao-parametrizacao-closing/trabalhista-judicial/trabalhista-judicial.component';

@Component({
  selector: 'decisao-evento',
  templateUrl: './decisao-evento.component.html',
  styleUrls: ['./decisao-evento.component.scss']
})
export class DecisaoEventoComponent {
  
  datasource: Array<DecisaoEvento> = new Array<DecisaoEvento>();
  total: number = 0;
  sort: Sort;

  totalAtual: number = -1;

  @Input() evento : Evento;
  @Input() tipoProcesso : number;
  
  @Output() retornoChange = new EventEmitter<boolean>();
  
  constructor(
    private dialog: DialogService, 
    private service: DecisaoEventoService, 
    private datepipe: DatePipe) { }

  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;
  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
 

  buscar() {      
    this.service.obter(this.iniciaValoresDaView().sortColumn,this.iniciaValoresDaView().sortDirection,this.iniciaValoresDaView().pageIndex,this.iniciaValoresDaView().pageSize , this.evento.id)
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
        
        
        if (this.totalAtual > -1 && this.totalAtual != this.total) {
          if (this.totalAtual === 0 && this.total > 0 ) {
            this.retornoChange.emit(true);  
          }
          else if (this.totalAtual > 0 && this.total === 0 ) {
            this.retornoChange.emit(true);  
          }
        }
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
    const teveAlteracao: boolean = await DecisaoEventoModalComponent.exibeModalDeIncluir(this.evento,this.datasource);
    if (teveAlteracao) {   
      this.totalAtual = this.total; 
      this.buscar();      
    }    
  }

  async alterar(item: DecisaoEvento): Promise<void> {
    const teveAlteracao: boolean = await DecisaoEventoModalComponent.exibeModalDeAlterar(item, this.evento,this.datasource);
    if (teveAlteracao) {
      this.totalAtual = this.total; 
      this.buscar();      
    }
  }

  async excluir(item: DecisaoEvento) {
    //Trabalhista
    if(this.tipoProcesso === 2){
      if (item.decisaoDefault){
        await this.dialog.err(
          'Não é possivel excluir a Decisão Default',
          'Marque outra decisão como default antes de excluir esta decisão.'
        );
        return;
      }
    }
    
    const excluirTipoProcedimento: boolean = await this.dialog.confirm(
      'Excluir a Decisão do Evento',
      `Deseja excluir a Decisão do Evento<br><b>${item.descricao}</b>?`
    );

    if (!excluirTipoProcedimento) {
      return;
    }

    try {
      await this.service.excluir(item);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Decisão do Evento excluída!'
      );

      this.totalAtual = this.total;   
      this.buscar();

    } catch (error) {
      if ((error as HttpErrorResult).messages.join().includes('relacionado')){
        await this.dialog.info(`Exclusão não permitida`, (error as HttpErrorResult).messages.join('\n'));
        return;
       }
      await this.dialog.err(`Exclusão não realizada`, (error as HttpErrorResult).messages.join('\n'));
    }

  }








}
