// angular
import { Component, ViewChild,  OnInit, AfterViewInit } from '@angular/core';

// 3rd party
import { EMPTY} from 'rxjs';
import { catchError} from 'rxjs/operators';

// core & shared imports
import { DialogService } from '@shared/services/dialog.service';
import { Page } from '@shared/types/page';
import { SortOf } from '@shared/types/sort';

// local imports
import { PrepostoService } from '@manutencao/services/preposto.service';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { Preposto } from '@manutencao/models/preposto.model';
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';
import { StaticInjector } from '@manutencao/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AlocacaoFutura } from '@manutencao/models/alocacao-futura.model';

@Component({
  selector: 'app-preposto',
  templateUrl: './alocacao-preposto.component.html',
  styleUrls: ['./alocacao-preposto.component.scss']
})
export class AlocacaoModalComponent implements AfterViewInit{
  constructor(
    private modal: NgbActiveModal,
    private service: PrepostoService,
    private dialog: DialogService    
  ) {}

  preposto : Preposto;   
  processosAlterados : any;

  alocacoes : Array<AlocacaoFutura> = [];
  
  total: number = 0;

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;  

  ngAfterViewInit(): void {
    this.buscarTabela();
  }

  buscarTabela(): void {    
    let sort: SortOf<any> = { column: this.iniciaValoresDaView().sortColumn , direction: this.iniciaValoresDaView().sortDirection};
    const page: Page = { index: this.iniciaValoresDaView().pageIndex, size: this.iniciaValoresDaView().pageSize};

    this.service.obterAlocacao(sort, page, this.preposto.id, this.processosAlterados).pipe(
              catchError(err => {
                console.log(err);
                this.dialog.err(
                  'Informações não carregadas',
                  'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte'
                );
                return EMPTY;
              })
            ).subscribe(data => {
                  this.alocacoes = data.data;
                  this.total = data.total;
                });;
  }

  iniciaValoresDaView() {
    return {
      sortColumn: this.table === undefined || !this.table.sortColumn ? "nome" : this.table.sortColumn,
      sortDirection: this.table === undefined || !this.table.sortDirection ? "asc" : this.table.sortDirection,
      pageIndex: this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize,
    }
  } 

  exportar( ){
    let sort: SortOf<any>;
    sort = { column: 'Data', direction: 'asc' };
    sort = { column: this.iniciaValoresDaView().sortColumn, direction: this.iniciaValoresDaView().sortDirection };   
    const page: Page = { index: this.paginator.pageIndex, size: this.paginator.pageSize};
    this.service.exportarAlcacao(sort,this.preposto.id,this.processosAlterados);
  }

  public static exibeModal(preposto : Preposto , processosAlterados : any): Promise<boolean> {
    // prettier-ignore
    
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(AlocacaoModalComponent, {windowClass: 'modal-alocacao', centered: true, size: 'sm', backdrop: 'static' });    
      modalRef.componentInstance.preposto = preposto;      
      modalRef.componentInstance.processosAlterados = processosAlterados;  
    return modalRef.result;
  }

    close(): void {
    this.modal.close(false);
  }

}
