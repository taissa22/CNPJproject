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
import { TipoDeParticipacao } from '@manutencao/models/tipo-de-participacao';
import { TipoDeParticipacaoService } from '@manutencao/services/tipo-de-participacao.service';
import { TipoDeParticipacaoServiceMock } from '@manutencao/services/tipo-de-participacao.service.mock';
import { TipoDeParticipacaoModalComponent } from '@manutencao/modals/tipo-de-participacao-modal/tipo-de-participacao-modal.component';
import { HttpErrorResult } from '@core/http/http-error-result';
import { Permissoes } from '@permissoes';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';

@Component({
  selector: 'app-tipos-de-participacao',
  templateUrl: './tipos-de-participacao.component.html',
  styleUrls: ['./tipos-de-participacao.component.scss'],
  // providers: [
  //   {
  //     provide: TipoDeParticipacaoService,
  //     useClass: TipoDeParticipacaoService
  //   }
  // ]
})
export class TiposDeParticipacaoComponent implements AfterViewInit {
  constructor(
    private service: TipoDeParticipacaoService,
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService
  ) {}

  dataSource: Array<TipoDeParticipacao> = [];
  total: number = 0;
  breadcrumb: string;
  sort: Sort;

  @ViewChild(JurTable, { static: true }) table: JurTable<TipoDeParticipacao>;
  @ViewChild(JurPaginator, { static: true }) paginator: JurPaginator;

  private search$: BehaviorSubject<string> = new BehaviorSubject('');
  set search(v: string) {
    this.search$.next(v);
  }
  get search(): string {
    return this.search$.value;
  }

  async ngOnInit(){
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_TIPO_PARTICIPACAO);
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

          return this.service.obterPaginado( page, sort, this.search).pipe(
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
    const teveAlteracao: boolean = await TipoDeParticipacaoModalComponent.exibeModalDeIncluir();
    if (teveAlteracao) {
      this.search$.next(this.search);
    }
  }

  async alterar(item: TipoDeParticipacao): Promise<void> {
    // prettier-ignore
    const teveAlteracao: boolean = await TipoDeParticipacaoModalComponent.exibeModalDeAlterar(item);
    if (teveAlteracao) {
      this.search$.next(this.search);
    }
  }

  async excluir(item: TipoDeParticipacao): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Tipo de Participação',
      `Deseja excluir o tipo de participação<br><b>${item.descricao}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(item.codigo);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Tipo de participação excluída!'
      );
      this.search$.next(this.search);
    } catch (error) {
      console.log(error);
      if ((error as HttpErrorResult).messages.join().includes('relacionado')){
       await this.dialog.info(`Exclusão não permitida`, (error as HttpErrorResult).messages.join('\n'));
       return;
      }

      await this.dialog.err( `Exclusão não realizada`, 
      (error as HttpErrorResult).messages.join('\n'));
    }
  }

  exportar() {
    this.service.exportar(this.sort, this.search)
  }
}
