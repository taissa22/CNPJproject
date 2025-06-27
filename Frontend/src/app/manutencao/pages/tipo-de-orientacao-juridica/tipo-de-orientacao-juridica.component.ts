import { TipoDeOrientacaoJuridicaModalComponent } from './../../modals/tipo-de-orientacao-juridica-modal/tipo-de-orientacao-juridica-modal.component';
import { TipoDeOrientacaoJuridica } from './../../models/tipo-de-orientacao-juridica';
import { Component, OnInit, ViewChild } from '@angular/core';
import { TipoDeOrientacaoJuridicaServiceMock } from '@manutencao/data/tipo-de-orientacao-juridica.service.mock';
import { JurPaginator } from '@shared/components/jur-paginator/jur-paginator.component';
import { JurTable } from '@shared/components/jur-table/jur-table.component';
import { DialogService } from '@shared/services/dialog.service';
import { BehaviorSubject, EMPTY, merge } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { Page } from '@shared/types/page';
import { Sort } from '@shared/types/sort';
import { TipoDeOrientacaoJuridicaService } from '@manutencao/services/tipo-de-orientacao-juridica.service';
import { HttpErrorResult } from '@core/http/http-error-result';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';

@Component({
  selector: 'app-tipo-de-orientacao-juridica',
  templateUrl: './tipo-de-orientacao-juridica.component.html',
  styleUrls: ['./tipo-de-orientacao-juridica.component.scss'],
  providers: [{provide: TipoDeOrientacaoJuridicaServiceMock, useClass: TipoDeOrientacaoJuridicaServiceMock }] 
})
export class TipoDeOrientacaoJuridicaComponent {
  
  dataSource: Array<TipoDeOrientacaoJuridica> = [];
  total: number = 0;
  breadcrumb: string;
  sort: Sort;

  constructor(
    private service: TipoDeOrientacaoJuridicaService,
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

  @ViewChild(JurTable, { static: true }) table: JurTable<TipoDeOrientacaoJuridica>;
  @ViewChild(JurPaginator, { static: true }) paginator: JurPaginator;

  async ngOnInit() {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_TIPO_ORIENTACAO_JURIDICA);
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
    //const teveAlteracao: boolean = await TipoDePendenciaModalComponent.exibeModalDeIncluir(this.tiposProcesso);
    const teveAlteracao: boolean = await TipoDeOrientacaoJuridicaModalComponent.exibeModalDeIncluir();
    if (teveAlteracao) {
      this.search$.next(this.search);
    }
  }

  async alterar(item): Promise<void> {
    //const teveAlteracao: boolean = await TipoDePendenciaModalComponent.exibeModalDeIncluir(this.tiposProcesso);
    const teveAlteracao: boolean = await TipoDeOrientacaoJuridicaModalComponent.exibeModalDeAlterar(item);
    if (teveAlteracao) {
      this.search$.next(this.search);
    }
  }


  async excluir(item: TipoDeOrientacaoJuridica): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Tipo de Orientação Jurídica',
      `Deseja excluir o tipo de orientação jurídica<br><b>${item.descricao}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(item.codigo);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Tipo de orientação jurídica excluída!'
      );
      this.search$.next(this.search);
    } catch (error) {
      console.log(error);
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
