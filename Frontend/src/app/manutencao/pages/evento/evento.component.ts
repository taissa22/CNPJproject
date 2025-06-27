// angular
import { Component, ViewChild, AfterViewInit, OnInit } from '@angular/core';

// 3rd party
import { catchError, switchMap } from 'rxjs/operators';

// core & shared imports
import { DialogService } from '@shared/services/dialog.service';
import { Page } from '@shared/types/page';
import { Sort, SortOf } from '@shared/types/sort';

// local imports
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';
import { HttpErrorResult } from '@core/http';
import { Evento } from '@manutencao/models/evento.model';
import { EventoService } from '@manutencao/services/evento.service';
import { FormControl } from '@angular/forms';
import { Permissoes, PermissoesService } from '@permissoes';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { EMPTY } from 'rxjs';
import { EventoModalComponent } from '@manutencao/modals/evento-modal/evento-modal.component';


@Component({
  selector: 'app-evento',
  templateUrl: './evento.component.html',
  styleUrls: ['./evento.component.scss'],
})
export class EventoComponent implements  OnInit {

  constructor(
    private service: EventoService,
    private dialog: DialogService,
    private permisoesService: PermissoesService
  ) { }

  exibirTabela = false;
  linhaSelecionada: number = -1;
  recarregar: boolean = false;

  dataSource: Array<Evento> = [];
  total: number = 0;

  sort: Sort;

  tiposProcesso: Array<TiposProcesso> = [];

  tipoProcessoFormControl : FormControl = new FormControl(null);
  buscarDescricaoFormControl : FormControl = new FormControl(null);

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  async ngOnInit(): Promise<void>  {
    this.ObterListaTipoProcesso();
  }

  ObterListaTipoProcesso() {
    const permissesTela = [ {permissao: Permissoes.ACESSAR_EVENTO_ADMINSTRATIVO, tipoProcesso: TiposProcesso.ADMINISTRATIVO, menu: 'Administrativo'},
                            {permissao: Permissoes.ACESSAR_EVENTO_CIVEL_CONSUMIDOR, tipoProcesso: TiposProcesso.CIVEL_CONSUMIDOR, menu: 'CÍVEL CONSUMIDOR'},
                            {permissao: Permissoes.ACESSAR_EVENTO_CIVEL_ESTRATEGICO, tipoProcesso: TiposProcesso.CIVEL_ESTRATEGICO, menu: 'Cível Estratégico'},
                            {permissao: Permissoes.ACESSAR_EVENTO_TRABALHISTA, tipoProcesso: TiposProcesso.TRABALHISTA, menu: 'Trabalhista'},
                            {permissao: Permissoes.ACESSAR_EVENTO_TRABALHISTA_ADMINISTRATIVO, tipoProcesso: TiposProcesso.TRABALHISTA_ADMINISTRATIVO, menu: 'Trabalhista Administrativo'},
                            {permissao: Permissoes.ACESSAR_EVENTO_TRIBUTARIO_ADMINISTRATIVO, tipoProcesso: TiposProcesso.TRIBUTARIO_ADMINISTRATIVO, menu: 'Tributário Administrativo'},
                            {permissao: Permissoes.ACESSAR_EVENTO_TRIBUTARIO_JUDICIAL, tipoProcesso: TiposProcesso.TRIBUTARIO_JUDICIAL, menu: 'Tributário Judicial'}  ]

      permissesTela.forEach((item) => {
        if (this.permisoesService.temPermissaoPara(item.permissao)) {
          this.tiposProcesso.push({...item.tipoProcesso, descricao: item.menu })
        }
      })
  }
  buscarDecisao(i) :void{
    if (this.linhaSelecionada == i) {
      this.linhaSelecionada = -1;
      return;
    }
    this.linhaSelecionada = i;
  }

  buscarTabela(): void {
    let sort: SortOf<any> = { column: this.iniciaValoresDaView().sortColumn , direction: this.iniciaValoresDaView().sortDirection};
    const page: Page = { index: this.iniciaValoresDaView().pageIndex, size: this.iniciaValoresDaView().pageSize};

    this.linhaSelecionada = -1;

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
      sortColumn: this.table === undefined || !this.table.sortColumn ? "id" : this.table.sortColumn,
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

  exportar() {
    
    let sort: SortOf<any> = { column: this.iniciaValoresDaView().sortColumn , direction: this.iniciaValoresDaView().sortDirection};    ​​​
    this.service.exportar(sort,this.tipoProcessoFormControl.value,this.buscarDescricaoFormControl.value );
  }

  exportarDependente()​​​​​{ let sort: SortOf<any> = { column: this.iniciaValoresDaView().sortColumn , direction: this.iniciaValoresDaView().sortDirection};
   ​​​​​
  this.service.exportarDependente(sort,this.buscarDescricaoFormControl.value)
}

  async incluir(): Promise<void> {
    
    const teveAlteracao: boolean = await EventoModalComponent.exibeModalDeIncluir(this.tipoProcessoFormControl.value);
    if (teveAlteracao) {
      this.buscarTabela();
    }
  }

  async alterar(item: Evento): Promise<void> {
    const teveAlteracao: boolean = await EventoModalComponent.exibeModalDeAlterar(this.tipoProcessoFormControl.value, item);
    if (teveAlteracao) {
      this.buscarTabela();
    }
  }

  async excluir(item: Evento) {
    const excluirTipoProcedimento: boolean = await this.dialog.confirm(
      'Excluir Evento',
      `Deseja excluir o evento <br><b>${item.nome}</b>?`
    );

    if (!excluirTipoProcedimento) {
      return;
    }

    try {
      await this.service.excluir(item.id,this.tipoProcessoFormControl.value);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Evento excluído!'
      );

      this.onClearInputPesquisar();
      this.buscarTabela();

    } catch (error) {
      if ((error as HttpErrorResult).messages.join().includes('relacionado')){
        await this.dialog.info(`Exclusão não permitida`, (error as HttpErrorResult).messages.join('\n'));
        return;
       }
      await this.dialog.err(`Exclusão não realizada`, (error as HttpErrorResult).messages.join('\n'));
    }

  }

  async recarregaTela(recarregar: boolean) {
    this.buscarTabela();
  }


  onClearInputPesquisar(){ if(!this.buscarDescricaoFormControl.value){ this.buscarTabela()  } }

}



