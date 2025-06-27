// angular
import { AfterViewInit, Component, ViewChild } from '@angular/core';

// 3rd party
import { BehaviorSubject, EMPTY, merge } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';

// core & shared imports
import { JurTable } from '@shared/components/jur-table/jur-table.component';
import { JurPaginator } from '@shared/components/jur-paginator/jur-paginator.component';
import { DialogService } from '@shared/services/dialog.service';
import { Page } from '@shared/types/page';
import { Sort, SortOf } from '@shared/types/sort';

// local imports
import { BaseDeCalculo } from '@manutencao/models/base-de-calculo';
import { IndiceModalComponent } from '@manutencao/modals/indice-modal/indice-modal.component';
import { IndiceServiceMock } from '@manutencao/services/indice.service.mock';
import { IndiceService } from '@manutencao/services/indice.service';
import { Indice } from '@manutencao/models/indice';
import { HttpErrorResult } from '@core/http';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';

@Component({
  selector: 'app-indice',
  templateUrl: './indice.component.html',
  styleUrls: ['./indice.component.scss'],
  // providers: [
  //   { provide: IndiceService, useClass:  IndiceServiceMock }
  // ]
})
export class IndiceComponent implements AfterViewInit {
  constructor(
    private service: IndiceService,
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService
  ) { }

  dataSource: Array<Indice> = [];
  total: number = 0;
  breadcrumb: string;
  sort: Sort;

  @ViewChild(JurTable, { static: true }) table: JurTable<BaseDeCalculo>;
  @ViewChild(JurPaginator, { static: true }) paginator: JurPaginator;

  private search$: BehaviorSubject<string> = new BehaviorSubject('');
  set search(v: string) {
    this.search$.next(v);
  }
  get search(): string {
    return this.search$.value;
  }

  async ngOnInit(){
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_CADASTRO_INDICES);
  }

  ngAfterViewInit(): void {
    merge(this.table.sort, this.paginator.page, this.search$)
      .pipe(
        switchMap(() => {
          const page: Page = {
            index: this.paginator.pageIndex,
            size: this.paginator.pageSize
          };
          let sort: Sort = {
            column: 'descricao',
            direction: 'asc'
          };
          if (this.table.sortDirection !== null) {
            sort = {
              column: this.table.sortColumn,
              direction: this.table.sortDirection
            };
          }

          this.sort = sort;
          return this.service.obterPaginado(page, sort, this.search).pipe(
            catchError(err => {
              
              this.dialog.err(
                'Informações não carregadas',
                'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte'
              );
              return EMPTY;
            })
          );
        })
      )
      .subscribe(data => {
        this.dataSource = data.lista;
        this.total = data.total;
      });
  }

  async incluir(): Promise<void> {
    const teveAlteracao: boolean = await IndiceModalComponent.exibeModalDeIncluir();
    if (teveAlteracao) {
      this.search$.next(this.search);
    }
  }

  async alterar(item: Indice): Promise<void> {
    const teveAlteracao: boolean = await IndiceModalComponent.exibeModalDeAlterar(item);
    if (teveAlteracao) {
      this.search$.next(this.search);
    }
  }

  async excluir(item: Indice): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Índice',
      `Deseja excluir o Índice <br><b>${item.descricao}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(item.id);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Índice excluído!'
      );
      this.search$.next(this.search);
    } catch (error) {

      await this.dialog.err(`Exclusão não realizada`, (error as HttpErrorResult).messages.join('\n'));
    }
  }

  exportar() {
    this.service.exportar(this.sort, this.search)
  }
}
