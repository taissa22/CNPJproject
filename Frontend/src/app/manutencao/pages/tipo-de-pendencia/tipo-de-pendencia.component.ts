import { TipoDePendenciaServiceMock } from './../../data/tipo-de-pendencia.service.mock';
import { BehaviorSubject, EMPTY, merge } from 'rxjs';
import { Component, ViewChild } from '@angular/core';
import { TipoDePendencia } from '@manutencao/models/tipo-de-pendencia';
import { JurPaginator } from '@shared/components/jur-paginator/jur-paginator.component';
import { JurTable } from '@shared/components/jur-table/jur-table.component';
import { Page } from '@shared/types/page';
import { Sort } from '@shared/types/sort';
import { catchError, switchMap } from 'rxjs/operators';
import { DialogService } from '@shared/services/dialog.service';
import { TipoDePendenciaModalComponent } from '@manutencao/modals/tipo-de-pendencia-modal/tipo-de-pendencia-modal.component';
import { TipoDePendenciaService } from '@manutencao/services/tipo-de-pendencia.service';
import { HttpErrorResult } from '@core/http';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';

@Component({
  selector: 'app-tipo-de-pendencia',
  templateUrl: './tipo-de-pendencia.component.html',
  styleUrls: ['./tipo-de-pendencia.component.scss'],
  providers: [{provide: TipoDePendenciaServiceMock, useClass: TipoDePendenciaServiceMock }] 
})
export class TipoDePendenciaComponent {

  dataSource: Array<TipoDePendencia> = [];
  total: number = 0;

  sort: Sort;  
  breadcrumb: string;

  constructor(
    private service: TipoDePendenciaService,
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService
  ) { }


  private search$: BehaviorSubject<string> = new BehaviorSubject('');
  set search(v: string) {
    this.search$.next(v);
  }
  get search(): string {
    return this.search$.value;
  }

  @ViewChild(JurTable, { static: true }) table: JurTable<TipoDePendencia>;
  @ViewChild(JurPaginator, { static: true }) paginator: JurPaginator;

  async ngOnInit(){
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_TIPO_PENDENCIA);
  }

  ngAfterViewInit(): void {
    merge(this.table.sort, this.paginator.page, this.search$)
      .pipe(
        switchMap(() => {
          
          const page: Page = {
            index: this.paginator.pageIndex + 1,
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
            catchError(() => {
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
    const teveAlteracao: boolean = await TipoDePendenciaModalComponent.exibeModalDeIncluir();
    if (teveAlteracao) {
      this.search$.next(this.search);
    }
  }

  async alterar(item): Promise<void> {
    const teveAlteracao: boolean = await TipoDePendenciaModalComponent.exibeModalDeAlterar(item);
    if (teveAlteracao) {
      this.search$.next(this.search);
    }
  }


  async excluir(item: TipoDePendencia): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Tipo de Pendência',
      `Deseja excluir o tipo de pendência<br><b>${item.descricao}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(item.id);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Tipo de pendência excluída!'
      );
      this.search$.next(this.search);
    } catch (error) {
      if ((error as HttpErrorResult).messages.join('\n').includes('Pendências do Processo')) {
        this.dialog.info('Exclusão não realizada', (error as HttpErrorResult).messages.join('\n'));
      } else {
        this.dialog.err('Exclusão não realizada', (error as HttpErrorResult).messages.join('\n'));
        throw error;
      }
    }
  }

  exportar() {
    this.service.exportar(this.sort, this.search)
  }

}
