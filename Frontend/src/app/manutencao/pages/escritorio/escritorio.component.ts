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
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Escritorio } from '@manutencao/models/escritorio.model';
import { EscritorioModalComponent } from '@manutencao/modals/escritorio-modal/escritorio-modal.component';
import { EscritorioService } from '@manutencao/services/escritorio.service';
import { Estado, Estados } from '@core/models';
import { NgbTooltip } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-escritorio',
  templateUrl: './escritorio.component.html',
  styleUrls: ['./escritorio.component.scss']
})
export class EscritorioComponent implements OnInit {
  breadcrumb: string;
  constructor(
    private service: EscritorioService,
    private dialog: DialogService,
    private permisoesService: PermissoesService,
    private breadcrumbsService: BreadcrumbsService
  ) {}

  estados: Array<Estados> = Estados.obterUfs();
  mostrartooltip : boolean = false;

  tiposProcesso: TiposProcesso[] = [];
  escritorios : Array<Escritorio> = [];
  total: number = 0;

  tipoProcessoSelecionado: boolean = false;
  areaAtuacaoFormControl : FormControl = new FormControl(null);
  estadoFormControl : FormControl = new FormControl(null);
  buscarDescricaoFormControl : FormControl = new FormControl(null);

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_ESCRITORIO);
  }

  ngOnInit(): void {
    const items = [ { tipoProcesso: TiposProcesso.ADMINISTRATIVO, descricao: 'Administrativo'},
                    { tipoProcesso: TiposProcesso.CIVEL_CONSUMIDOR, descricao: 'Civel Consumidor'},
                    { tipoProcesso: TiposProcesso.CIVEL_ESTRATEGICO, descricao: 'Cível Estratégico'},
                    {tipoProcesso: TiposProcesso.CRIMINAL_ADMINISTRATIVO, descricao: 'Críminal Administrativo'},
                    {tipoProcesso: TiposProcesso.CRIMINAL_JUDICIAL, descricao: 'Críminal Judicial'},
                    {tipoProcesso: TiposProcesso.JEC, descricao: 'Juizado Especial'},
                    {tipoProcesso: TiposProcesso.PEX, descricao: 'Pex'},
                    { tipoProcesso: TiposProcesso.PROCON, descricao: 'Procon'},
                    {tipoProcesso: TiposProcesso.TRABALHISTA, descricao: 'Trabalhista'},
                    {tipoProcesso: TiposProcesso.TRIBUTARIO_ADMINISTRATIVO, descricao: 'Tributário'}]


       items.forEach((item) => {
          this.tiposProcesso.push({...item.tipoProcesso, descricao: item.descricao })
      })

}

  buscarTabela(): void {
    let sort: SortOf<any> = { column: this.iniciaValoresDaView().sortColumn , direction: this.iniciaValoresDaView().sortDirection};
    const page: Page = { index: this.iniciaValoresDaView().pageIndex, size: this.iniciaValoresDaView().pageSize};

    this.service.obter(sort, page,this.estadoFormControl.value, this.areaAtuacaoFormControl.value, this.buscarDescricaoFormControl.value).pipe(
              catchError(err => {
                console.log(err);
                this.dialog.err(
                  'Informações não carregadas',
                  'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte'
                );
                return EMPTY;
              })
            ).subscribe(data => {
                 
              this.escritorios = data.data;
                  this.total = data.total;
                });;
  }

  iniciaValoresDaView() {
    return {
      sortColumn: this.table === undefined || !this.table.sortColumn ? "Nome" : this.table.sortColumn,
      sortDirection: this.table === undefined || !this.table.sortDirection ? "asc" : this.table.sortDirection,
      pageIndex: this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize,
    }
  }

  selecionarTipoProcesso() {
    this.buscarDescricaoFormControl.setValue('') ;

    this.buscarTabela() ;
  }

  async incluir(): Promise<void> {
    const teveAlteracao: boolean = await EscritorioModalComponent.exibeModalDeIncluir();

    if (teveAlteracao) {
      this.buscarDescricaoFormControl.setValue('');
      this.buscarTabela()
    }
  }

 async alterar(item: Escritorio): Promise<void> {
    
  
  const teveAlteracao: boolean = await EscritorioModalComponent.exibeModalDeAlterar(item);

    if (teveAlteracao) {
      this.buscarDescricaoFormControl.setValue('');
      this.buscarTabela()
    }
  }



  async excluir(item: Escritorio): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Escritório',
      `Deseja excluir o Escritório<br><b>${item.nome}</b>?`
    );

    if (!excluir) { return; }

    try {
      await this.service.excluir(item.id);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Escritório excluído!'
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
    this.service.exportar(sort,page,this.estadoFormControl.value ,this.areaAtuacaoFormControl.value, this.buscarDescricaoFormControl.value);
  }



  exportaradvogado( ){
    let sort: SortOf<any>;
    sort = { column: 'descricao', direction: 'asc' };
    sort = { column: this.iniciaValoresDaView().sortColumn, direction: this.iniciaValoresDaView().sortDirection };
    const page: Page = { index: this.paginator.pageIndex, size: this.paginator.pageSize};
    this.service.exportaradvogado(sort,page,this.estadoFormControl.value ,this.areaAtuacaoFormControl.value, this.buscarDescricaoFormControl.value);
  }

  exportaratuacao( ){
    let sort: SortOf<any>;
    sort = { column: 'descricao', direction: 'asc' };
    sort = { column: this.iniciaValoresDaView().sortColumn, direction: this.iniciaValoresDaView().sortDirection };
    const page: Page = { index: this.paginator.pageIndex, size: this.paginator.pageSize};
    this.service.exportaratuacao(sort,page,this.estadoFormControl.value ,this.areaAtuacaoFormControl.value, this.buscarDescricaoFormControl.value);
  }

  onClearInputPesquisar(){ if(!this.buscarDescricaoFormControl.value){ this.buscarTabela()  } }
}
