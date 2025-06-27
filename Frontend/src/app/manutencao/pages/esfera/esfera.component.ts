// angular
import { OnInit, Component, ViewChild, AfterViewInit, Output, EventEmitter } from '@angular/core';

// 3rd party
import { catchError, switchMap } from 'rxjs/operators';

// core & shared imports
import { DialogService } from '@shared/services/dialog.service';

// local imports
import { Esfera } from '@manutencao/models/esfera.model';
import { EsferaService } from '@manutencao/services/esfera.service';
import { HttpErrorResult } from '@core/http';
import { EsferaModalComponent } from '@manutencao/modals/esfera-modal/esfera-modal.component';
import { EMPTY } from 'rxjs';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';
import { InconsistenciaComponent } from './inconsistencias/inconsistencias.component';



@Component({
  selector: 'app-esfera',
  templateUrl: './esfera.component.html',
  styleUrls: ['./esfera.component.scss'],  
})
export class EsferaComponent implements AfterViewInit {

  dataSource: Array<Esfera> = [];
  total: number = 0;
  linhaSelecionada: number = -1;

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;  
  breadcrumb: any;

  constructor(
    private service: EsferaService,
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService
  ) { }

  async ngAfterViewInit(): Promise<void>  {    
    this.buscarTabela(); 
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_ESFERA);
  }

  buscarTabela() {
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

  iniciaValoresDaView() {
    let sort: any = this.table === undefined || !this.table.sortColumn ? "id" : this.table.sortColumn;
    return {
      sortColumn: sort,
      sortDirection: this.table === undefined || !this.table.sortDirection ? "asc" : this.table.sortDirection,
      pageIndex: this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize,
    }
  }

  abreIndiceCorrecaoEsferas(i) {
    if (this.linhaSelecionada == i) {
      this.linhaSelecionada = -1;
      return;
    }
    this.linhaSelecionada = i;
  }

  async incluir(){
    const teveAlteracao: boolean = await EsferaModalComponent.exibeModalDeIncluir();
    if (teveAlteracao) {
      this.buscarTabela();
    }

  }
 
  async alterar(item: Esfera): Promise<void> {
    const teveAlteracao: boolean = await EsferaModalComponent.exibeModalDeAlterar(item);
    if (teveAlteracao) {
      this.buscarTabela();
    }
  }

  async excluir(item: Esfera): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Esfera',
      `Deseja excluir a Esfera <br><b>${item.nome}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(item.id);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Esfera excluída!'
      );
      this.buscarTabela();
    } catch (error) {

      await this.dialog.err(`Exclusão não realizada`, (error as HttpErrorResult).messages.join('\n'));
    }
  }

  exportar() {
    let view = this.iniciaValoresDaView();

    this.service.exportar(view.sortColumn, view.sortDirection)
  }


}


