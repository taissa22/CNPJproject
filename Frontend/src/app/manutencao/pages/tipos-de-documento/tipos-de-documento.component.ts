// angular
import { OnInit, Component, ViewChild, AfterViewInit } from '@angular/core';

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
import { TipoDeDocumento } from '@manutencao/models/tipos-de-documento.model';
import { TipoDeDocumentoService } from '@manutencao/services/tipos-de-documento.service';
import { TiposDeDocumentoModalComponent } from '@manutencao/modals/tipos-de-documento-modal/tipos-de-documento-modal.component';
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';
import { HttpErrorResult } from '@core/http';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';


@Component({
  selector: 'app-tipos-de-documento',
  templateUrl: './tipos-de-documento.component.html',
  styleUrls: ['./tipos-de-documento.component.scss'],
  // providers: [
  //   { provide: TipoDeDocumentoService, useClass: TipoDeDocumentoService }
  // ]
})
export class TiposDeDocumentoComponent implements AfterViewInit, OnInit {
  breadcrumb: string;

  constructor(
    private service: TipoDeDocumentoService,
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService
  ) { }


  buscarDescricao = ''
  exibirTabela = false

  dataSource: Array<TipoDeDocumento> = [];
  total: number = 0;

  esteProcesso = false;

  estadoInicial = false;
  sort: Sort;

  tiposProcesso: TiposProcesso[] = TiposProcesso.todos();
  tiposDePrazo: Array<{id: number, descricao: string}>;

  @ViewChild(JurTable, { static: false }) table: JurTable<TipoDeDocumento>;
  @ViewChild(JurPaginator, { static: false }) paginator: JurPaginator;

  private tipoProcessoSelecao$: BehaviorSubject<number> = new BehaviorSubject(-1);
  set tipoProcessoSelecao(v: number) {
    this.tipoProcessoSelecao$.next(v);
  }
  get tipoProcessoSelecao(): number {
    return this.tipoProcessoSelecao$.value;
  }


  private search$: BehaviorSubject<string> = new BehaviorSubject('');
  set search(v: string) {
    this.search$.next(v);
  }
  get search(): string {
    return this.search$.value;
  }

  ngOnInit() {
    this.estadoInicial = false;
  }

  async ngAfterViewInit(): Promise<void>  {
    this.tiposProcesso = await this.service.getTiposDeProcesso();
    this.tiposProcesso.sort((a,b) => {
      return a.descricao.localeCompare(b.descricao);

    });
    this.tiposDePrazo = await this.service.getTiposDePrazo();
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_TIPO_DOCUMENTO);
  }

  buscarTabela(): void {
    merge(this.table && this.table.sort, this.paginator && this.paginator.page, this.search$, this.tipoProcessoSelecao$)
    .pipe(
      switchMap(() => {
        const page: Page = {
          index: this.paginator.pageIndex,
          size: this.paginator.pageSize
        };

        let sort: SortOf<any>;

        sort = {
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

        return this.service.obter(sort, page, this.tipoProcessoSelecao, this.search).pipe(
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

  selecionarTipoProcesso(proceso: any) {
    this.tipoProcessoSelecao = proceso.id;
    this.exibirTabela = true;

    setTimeout(() => {
      this.buscarTabela();
      this.tipoProcessoSelecao$.next(this.tipoProcessoSelecao);
    }, 300 );
  }

  buscar() {
    this.search = this.buscarDescricao
    this.search$.next(this.search)
  }

  exportar() {​​​​​
    this.service.exportar(this.sort, this.tipoProcessoSelecao, this.search)
  }​​​​​

  async incluir(): Promise<void> {
    const teveAlteracao: boolean = await TiposDeDocumentoModalComponent.exibeModalDeIncluir(this.tipoProcessoSelecao, this.tiposDePrazo);
    if (teveAlteracao) {
      this.search$.next(this.search);
    }
  }

  async alterar(item: TipoDeDocumento): Promise<void> {
    const teveAlteracao: boolean = await TiposDeDocumentoModalComponent.exibeModalDeAlterar(item, this.tiposDePrazo);
    if (teveAlteracao) {
      this.search$.next(this.search);
    }
  }

  async excluir(item: TipoDeDocumento) {
    const excluirTipoDocumento: boolean = await this.dialog.confirm(
      'Excluir Tipo de Documento',
      `Deseja excluir o tipo de documento<br><b>${item.descricao}</b>?`
    );

    if (!excluirTipoDocumento) {
      return;
    }

    try {
      await this.service.excluir(item.codigo);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Tipo de Documento excluído!'
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


}
