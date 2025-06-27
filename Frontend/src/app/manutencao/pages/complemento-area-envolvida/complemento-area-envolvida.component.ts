import { async } from '@angular/core/testing';
import { text } from '@fortawesome/fontawesome-svg-core';
import { TipoProcesso } from 'src/app/core/models/tipo-processo';
import { Subscription } from 'rxjs';
import { Permissoes } from 'src/app/permissoes/permissoes';
import { TiposProcesso } from '@manutencao/services/tipos-de-processos';
// angular
import { AfterViewInit, Component, ViewChild, OnDestroy, OnInit, ValueProvider } from '@angular/core';

// 3rd party
import { BehaviorSubject, EMPTY, merge, Observable } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';

// core & shared imports
import { JurTable } from '@shared/components/jur-table/jur-table.component';
import { JurPaginator } from '@shared/components/jur-paginator/jur-paginator.component';
import { DialogService } from '@shared/services/dialog.service';
import { Page } from '@shared/types/page';
import { Sort, SortOf } from '@shared/types/sort';

// local imports
import { ComplementoAreaEnvolvida } from '@manutencao/models/complemento-area-envolvida.model';
import { ComplementoAreaEnvolvidaService } from '@manutencao/services/complemento-area-envolvida.service';
import { FormControl } from '@angular/forms';
import { PermissoesService } from '@permissoes';
import { ComplementoAreaEnvolvidaModalComponent } from '@manutencao/modals/complemento-area-envolvida-modal/complemento-area-envolvida-modal.component';
import { HttpErrorResult } from '@core/http';
import { faWindowRestore } from '@fortawesome/free-solid-svg-icons';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';

@Component({
  selector: 'app-complemento-area-envolvida',
  templateUrl: './complemento-area-envolvida.component.html',
  styleUrls: ['./complemento-area-envolvida.component.scss']
})
export class ComplementoAreaEnvolvidaComponent implements OnInit {
  breadcrumb: any;
  constructor(
    private service: ComplementoAreaEnvolvidaService,
    private dialog: DialogService,
    private permisoesService: PermissoesService,
    private breadcrumbsService: BreadcrumbsService
  ) {}

  exibirTabela:boolean = false;
  tiposProcesso: TiposProcesso[] = [];
  dataSource: Array<ComplementoAreaEnvolvida> = [];
  total: number = 0;

  tipoProcessoSelecionado: boolean = false;
  tipoProcessoFormControl : FormControl = new FormControl(null);
  buscarDescricaoFormControl : FormControl = new FormControl(null);

  @ViewChild(JurTable, { static: false }) table: JurTable<ComplementoAreaEnvolvida>;
  @ViewChild(JurPaginator, { static: false }) paginator: JurPaginator;

  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_COMPLEMENTO_AREA_ENVOLVIDA);
  }

  ngOnInit(): void {
      const permissesTela = [ {permissao: Permissoes.ACESSAR_COMPLEMENTO_AREA_ENVOLVIDA_ADMINISTRATIVO, tipoProcesso: TiposProcesso.ADMINISTRATIVO, menu: 'Administrativo'},
                              {permissao: Permissoes.ACESSAR_COMPLEMENTO_AREA_ENVOLVIDA_CIVEL_ESTRATATEGICO, tipoProcesso: TiposProcesso.CIVEL_ESTRATEGICO, menu: 'Cível Estratégico'},
                              {permissao: Permissoes.ACESSAR_COMPLEMENTO_AREA_ENVOLVIDA_CRIMINAL, tipoProcesso: TiposProcesso.CRIMINAL, menu: 'Criminal'},
                              {permissao: Permissoes.ACESSAR_COMPLEMENTO_AREA_ENVOLVIDA_TRABALHISTA, tipoProcesso: TiposProcesso.TRABALHISTA, menu: 'Trabalhista'},
                              {permissao: Permissoes.ACESSAR_COMPLEMENTO_AREA_ENVOLVIDA_TRIBUTARIO, tipoProcesso: TiposProcesso.TRIBUTARIO_JUDICIAL, menu: 'Tributário'}]


        permissesTela.forEach((item) => {
           if (this.permisoesService.temPermissaoPara(item.permissao)) {
             this.tiposProcesso.push({...item.tipoProcesso, descricao: item.menu })
           }
        })
  }

  buscarTabela(): void {
    let sort: SortOf<any> = { column: this.iniciaValoresDaView().sortColumn, direction: this.iniciaValoresDaView().sortDirection};
    const page: Page = { index: this.iniciaValoresDaView().pageIndex, size: this.iniciaValoresDaView().pageSize};

    this.service.obter(sort, page, this.tipoProcessoFormControl.value, this.buscarDescricaoFormControl.value).pipe(
              catchError(err => {
                console.log(err);
                this.dialog.err(
                  'Informações não carregadas',
                  'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte'
                );
                return EMPTY;
              })
            ).subscribe(data => {
                  this.dataSource = data.data;
                  this.total = data.total;
                });;

  }

  iniciaValoresDaView() {
    return {
      sortColumn: this.table === undefined || !this.table.sortColumn ? "nome" : this.table.sortColumn,
      sortDirection: this.table === undefined || !this.table.sortDirection ? "asc" : this.table.sortDirection,
      pageIndex: this.paginator === undefined ? 1 : this.paginator.pageIndex,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize,
    }
  }

  selecionarTipoProcesso() {
    this.buscarDescricaoFormControl.setValue('') ;

    if (this.exibirTabela) this.buscarTabela() ;
    this.exibirTabela = true;
  }

  async incluir(): Promise<void> {
    const teveAlteracao: boolean = await ComplementoAreaEnvolvidaModalComponent.exibeModalDeIncluir(this.tipoProcessoFormControl.value);
    this.buscarDescricaoFormControl.setValue('');
    if (teveAlteracao) { this.buscarTabela()}
  }

  async alterar(item: ComplementoAreaEnvolvida): Promise<void> {
    const teveAlteracao: boolean = await ComplementoAreaEnvolvidaModalComponent.exibeModalDeAlterar(item);
    this.buscarDescricaoFormControl.setValue('');
    if (teveAlteracao) { this.buscarTabela()}
  }

  async excluir(item: ComplementoAreaEnvolvida): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Complemento Área Envolvida',
      `Deseja excluir o complemento<br><b>${item.nome}</b>?`
    );

    if (!excluir) { return; }

    try {
      await this.service.excluir(item.id);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Complemento excluído!'
      );
      this.buscarDescricaoFormControl.setValue('');
      this.buscarTabela();
    } catch (error) {
      if ((error as HttpErrorResult).messages.join().includes('registrado')) {
        await this.dialog.info(`Exclusão não permitida`, (error as HttpErrorResult).messages.join('\n'));
        return;
      }
      await this.dialog.err(`Exclusão não realizada`, (error as HttpErrorResult).messages.join('\n'));
    }
  }

  exportar( ){
    let sort: SortOf<any>;
    sort = { column: 'descricao', direction: 'asc' };

    if (this.table.sortDirection !== null) {
      sort = { column: this.table.sortColumn, direction: this.table.sortDirection };
    }

    const page: Page = { index: this.paginator.pageIndex, size: this.paginator.pageSize};
    this.service.exportar(sort,page, this.tipoProcessoFormControl.value, this.buscarDescricaoFormControl.value);
  }

  onClearInputPesquisar(){ if(!this.buscarDescricaoFormControl.value){ this.buscarTabela()  } }
}
