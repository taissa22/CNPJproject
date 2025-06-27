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
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';

// local imports
import { BaseDeCalculo } from '@manutencao/models/base-de-calculo';
import { BaseDeCalculoService } from '@manutencao/services/base-de-calculo.service';
import { BaseDeCalculoServiceMock } from '@manutencao/services/base-de-calculo.service.mock';
import { BaseDeCalculoModalComponent } from '@manutencao/modals/base-de-calculo-modal/base-de-calculo-modal.component';
import { HttpErrorResult } from '@core/http/http-error-result';

@Component({
  selector: 'app-bases-de-calculo',
  templateUrl: './bases-de-calculo.component.html',
  styleUrls: ['./bases-de-calculo.component.scss'],
  // providers: [
  //   { provide: BaseDeCalculoService, useClass: BaseDeCalculoService }
  // ]
})
export class BasesDeCalculoComponent implements AfterViewInit {
  constructor(
    private service: BaseDeCalculoService,
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService
  ) { }

  dataSource: Array<BaseDeCalculo> = [];
  total: number = 0;
  sort: Sort;
  breadcrumb: string;

  @ViewChild(JurTable, { static: true }) table: JurTable<BaseDeCalculo>;
  @ViewChild(JurPaginator, { static: true }) paginator: JurPaginator;

  private search$: BehaviorSubject<string> = new BehaviorSubject('');
  set search(v: string) {
    this.search$.next(v);
  }
  get search(): string {
    return this.search$.value;
  }

  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_BASE_CALCULO);
    merge(this.table.sort, this.paginator.page, this.search$)
      .pipe(
        switchMap(() => {
          const page: Page = {
            index: this.paginator.pageIndex,
            size: this.paginator.pageSize
          };
          let sort: SortOf<any>;
          sort = {
            column: 'codigo',
            direction: 'desc'
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
              console.log(err);
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
    const teveAlteracao: boolean = await BaseDeCalculoModalComponent.exibeModalDeIncluir();
    if (teveAlteracao) {
      this.search$.next(this.search);
    }
  }

  async alterar(item: BaseDeCalculo): Promise<void> {
    // prettier-ignore
    const teveAlteracao: boolean = await BaseDeCalculoModalComponent.exibeModalDeAlterar(item);
    if (teveAlteracao) {
      this.search$.next(this.search);
    }
  }

  async excluir(item: BaseDeCalculo): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Base de Cálculo',
      `Deseja excluir a base <br><b>${item.descricao}</b>?`
    );

    if (!excluir) {
      return;
    }

    if (item.ehCalculoInicial) {
      this.dialog.info(
        'Desculpe, não é possível a exclusão',
        // tslint:disable-next-line: max-line-length
        'O registro selecionado possui a indicação de Base de Cálculo Inicial, transfira esta indicação para outro registro antes de excluí-lo.'
      );
      return;
    }

    try {
      await this.service.excluir(item.codigo);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Vigência da base de cálculo excluída!'
      );
      this.search$.next(this.search);
    } catch (error) {
      console.log(error);
       if ((error as HttpErrorResult).messages.join().includes('relacionada')){
        await this.dialog.info(`Exclusão não permitida`, (error as HttpErrorResult).messages.join('\n'));
        return;
       }
      await this.dialog.err(`Exclusão não realizada`, (error as HttpErrorResult).messages.join('\n'));
    }

  }

  exportar() {
    this.service.exportar(this.sort, this.search)
  }
}
