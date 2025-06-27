import { TipoDeVaraService } from './../../services/tipo-de-vara.service';
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
import { Sort } from '@shared/types/sort';

// local imports
import { TipoDeVara } from '@manutencao/models/tipo-de-vara';
import { TipoDeVaraServiceMock } from '@manutencao/services/tipo-de-vara.service.mock';
import { TipoDeVaraModalComponent } from '@manutencao/modals/tipo-de-vara-modal/tipo-de-vara-modal.component';
import { HttpErrorResult } from '@core/http';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';

@Component({
  selector: 'app-tipo-de-vara',
  templateUrl: './tipo-de-vara.component.html',
  styleUrls: ['./tipo-de-vara.component.scss'],
  // providers: [
  //   {
  //     provide: TipoDeVaraService,
  //     useClass: TipoDeVaraServiceMock
  //   }
  // ]
})



export class TipoDeVaraComponent implements AfterViewInit {
  constructor(
    private service: TipoDeVaraService,
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService
  ) {}

  sort: Sort;
  breadcrumb: string;
  dataSource: Array<TipoDeVara> = [];
  total: number = 0;

  @ViewChild(JurTable, { static: true }) table: JurTable<TipoDeVara>;
  @ViewChild(JurPaginator, { static: true }) paginator: JurPaginator;

  private search$: BehaviorSubject<string> = new BehaviorSubject('');
  set search(v: string) {
    this.search$.next(v);
  }
  get search(): string {
    return this.search$.value;
  }

  async ngOnInit(){
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_TIPO_VARA);
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
            column: 'nome',
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
    const teveAlteracao: boolean = await TipoDeVaraModalComponent.exibeModalDeIncluir();
    if (teveAlteracao) {
      this.search$.next(this.search);
    }
  }

  async alterar(item: TipoDeVara): Promise<void> {
    // prettier-ignore
    const teveAlteracao: boolean = await TipoDeVaraModalComponent.exibeModalDeAlterar(item);
    if (teveAlteracao) {
      this.search$.next(this.search);
    }
  }

  async excluir(item: TipoDeVara): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Tipo de Vara',
      `Deseja excluir o tipo de vara<br><b>${item.nome}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(item.codigo);
      await this.dialog.alert(
        'Exclusão relizada com sucesso',
        'Tipo de vara excluído!'
      );
      this.search$.next(this.search);
    } catch (error) {
      if ((error as HttpErrorResult).messages.join().includes('relacionado')){
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
