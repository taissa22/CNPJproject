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
import { TipoDeAudiencia } from '@manutencao/models/tipo-de-audiencia';
import { TipoDeAudienciaService } from '@manutencao/services/tipo-de-audiencia.service';
import { TipoDeAudienciaServiceMock } from '@manutencao/services/tipo-de-audiencia.service.mock';
import { TipoDeAudienciaModalComponent } from '@manutencao/modals/tipo-de-audiencia-modal/tipo-de-audiencia-modal.component';
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';
import { FormControl, FormGroup } from '@angular/forms';
import { HttpErrorResult } from '@core/http/http-error-result';
import { Permissoes } from '@permissoes';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';

@Component({
  selector: 'app-tipos-de-audiencia',
  templateUrl: './tipos-de-audiencia.component.html',
  styleUrls: ['./tipos-de-audiencia.component.scss']
})
export class TiposDeAudienciaComponent implements AfterViewInit {
  breadcrumb: any;
  constructor(
    private service: TipoDeAudienciaService,
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService
  ) {}

  buscarDescricao = '';
  sort: Sort;

  tiposProcesso: TiposProcesso[] = []
  dataSource: Array<TipoDeAudiencia> = [];
  total: number = 0;

  @ViewChild(JurTable, { static: true }) table: JurTable<TipoDeAudiencia>;
  @ViewChild(JurPaginator, { static: true }) paginator: JurPaginator;

  private search$: BehaviorSubject<string> = new BehaviorSubject('');
  set search(v: string) {
    this.search$.next(v);
  }
  get search(): string {
    return this.search$.value;
  }

  tipoProcessoFormControl: FormControl = new FormControl(null);
  exibirBotaoAdicionar: boolean = false;
  exibeEstrategico: boolean = false;
  exibeConsumidor: boolean = false;

  formGroup: FormGroup = new FormGroup({
    tipoProcesso: this.tipoProcessoFormControl
  });

  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_TIPO_AUDIENCIA);
    this.tiposProcesso = await this.service.getTiposDeProcesso();
    
    merge(
      this.table.sort,
      this.paginator.page,
      this.search$
    )
      .pipe(
        switchMap(() => {
          const page: Page = {
            index: this.paginator.pageIndex,
            size: this.paginator.pageSize
          };
          let sort: Sort;

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

          return this.service
            .obterPaginado(sort, page, this.tipoProcessoFormControl.value, this.search )
            .pipe(
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
    const teveAlteracao: boolean = await TipoDeAudienciaModalComponent.exibeModalDeIncluir(this.tiposProcesso);
    if (teveAlteracao) {
      this.search$.next(this.search);
    }
  }

  async alterar(item: TipoDeAudiencia): Promise<void> {
    // prettier-ignore
    const teveAlteracao: boolean = await TipoDeAudienciaModalComponent.exibeModalDeAlterar(item, this.tiposProcesso);
    if (teveAlteracao) {
      this.search$.next(this.search);
    }
  }

  async excluir(item: TipoDeAudiencia): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Tipo de Audiência',
      `Deseja excluir o tipo de audiência<br><b>${item.descricao}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(item.codigoTipoAudiencia);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Tipo de audiência excluída!'
      );
      this.search$.next(this.search);
    } catch (error) {
      if ((error as HttpErrorResult).messages.join('\n').includes('relacionado')) {
        this.dialog.info('Exclusão não permitida', (error as HttpErrorResult).messages.join('\n'));
      } else {
        this.dialog.err('Exclusão não realizada', (error as HttpErrorResult).messages.join('\n'));
      }
    }
  }

  aoSelecionarTipoProcesso() {
    this.exibirBotaoAdicionar = true;
  }

  buscar(){
    this.search = this.buscarDescricao
    this.exibeConsumidor = this.tipoProcessoFormControl.value === 1 ? true : false;
    this.exibeEstrategico = this.tipoProcessoFormControl.value === 9 ? true : false;
  }

  exportar(  ){
    this.service.exportar(this.sort, this.tipoProcessoFormControl.value, this.search)
  }
}
