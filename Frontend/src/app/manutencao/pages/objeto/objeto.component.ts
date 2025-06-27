import { Permissoes } from 'src/app/permissoes/permissoes';
// angular
import { AfterViewInit, Component, ViewChild, OnDestroy, OnInit, ValueProvider } from '@angular/core';

// 3rd party
import { BehaviorSubject, EMPTY, merge, Observable } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';

// core & shared imports
import { DialogService } from '@shared/services/dialog.service';
import { Page } from '@shared/types/page';
import { Sort, SortOf } from '@shared/types/sort';

// local imports
import { FormControl } from '@angular/forms';
import { PermissoesService } from '@permissoes';
import { HttpErrorResult } from '@core/http';
import { faWindowRestore } from '@fortawesome/free-solid-svg-icons';
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';
import { ObjetoService } from '@manutencao/services/objeto.service';
import { Objeto } from '@manutencao/models/objeto.model';
import { ObjetoModalComponent } from '@manutencao/modals/objeto-modal/objeto-modal.component';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';

@Component({
  selector: 'app-objeto',
  templateUrl: './objeto.component.html',
  styleUrls: ['./objeto.component.scss']
})
export class ObjetoComponent implements OnInit {
  breadcrumb: string;
  constructor(
    private service: ObjetoService,
    private dialog: DialogService,
    private permisoesService: PermissoesService,
    private breadcrumbsService: BreadcrumbsService
  ) {}

  exibirTabela:boolean = false;
  tiposProcesso: TiposProcesso[] = [];
  objetos: Array<Objeto> = [];
  total: number = 0;

  tipoProcessoSelecionado: boolean = false;
  tipoProcessoFormControl : FormControl = new FormControl(null);
  buscarDescricaoFormControl : FormControl = new FormControl(null);

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;
  
  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_OBJETO);
  }

  ngOnInit(): void {
    const permissesTela = [ {permissao: Permissoes.ACESSAR_OBJETO_TRABALHISTA_ADMINISTRATIVO, tipoProcesso: TiposProcesso.TRABALHISTA_ADMINISTRATIVO, menu: 'Trabalhista Administrativo'},
                            {permissao: Permissoes.ACESSAR_OBJETO_TRIBUTARIO, tipoProcesso: TiposProcesso.TRIBUTARIO_ADMINISTRATIVO, menu: 'Tributário'}]

      permissesTela.forEach((item) => {
        if (this.permisoesService.temPermissaoPara(item.permissao)) {
          this.tiposProcesso.push({...item.tipoProcesso, descricao: item.menu })
        }
      })
}

  buscarTabela(): void {
    let sort: SortOf<any> = { column: this.iniciaValoresDaView().sortColumn , direction: this.iniciaValoresDaView().sortDirection};
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
                  this.objetos = data.data;
                  this.total = data.total;
                });;
  }

  iniciaValoresDaView() {
    return {
      sortColumn: this.table === undefined || !this.table.sortColumn ? "descricao" : this.table.sortColumn,
      sortDirection: this.table === undefined || !this.table.sortDirection ? "asc" : this.table.sortDirection,
      pageIndex: this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize,
    }
  }

  selecionarTipoProcesso() {
    this.buscarDescricaoFormControl.setValue('') ;  
    

    if (this.exibirTabela) this.buscarTabela() ;

    this.exibirTabela = true;
  }

  async incluir(): Promise<void> {
    const teveAlteracao: boolean = await ObjetoModalComponent.exibeModalDeIncluir(TiposProcesso.porId(this.tipoProcessoFormControl.value));
    this.buscarDescricaoFormControl.setValue('');
    if (teveAlteracao) { this.buscarTabela()}
  }

  async alterar(item: Objeto): Promise<void> {
    const teveAlteracao: boolean = await ObjetoModalComponent.exibeModalDeAlterar(TiposProcesso.porId(this.tipoProcessoFormControl.value),item);
    this.buscarDescricaoFormControl.setValue('');
    if (teveAlteracao) { this.buscarTabela()}
  }

  async excluir(item: Objeto): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Objeto',
      `Deseja excluir o Objeto<br><b>${item.descricao}</b>?`
    );

    if (!excluir) { return; }

    try {
      await this.service.excluir(item.id);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Objeto excluído!'
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
    sort = { column: this.iniciaValoresDaView().sortColumn, direction: this.iniciaValoresDaView().sortDirection };   
    const page: Page = { index: this.paginator.pageIndex, size: this.paginator.pageSize};
    this.service.exportar(sort,page, this.tipoProcessoFormControl.value, this.buscarDescricaoFormControl.value);
  }

  onClearInputPesquisar(){ if(!this.buscarDescricaoFormControl.value){ this.buscarTabela()  } }
}
