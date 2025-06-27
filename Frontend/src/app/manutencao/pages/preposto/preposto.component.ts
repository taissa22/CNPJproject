import { AfterViewInit } from '@angular/core';
// angular
import { Component, ViewChild,  OnInit } from '@angular/core';

// 3rd party
import { EMPTY} from 'rxjs';
import { catchError} from 'rxjs/operators';

// core & shared imports
import { DialogService } from '@shared/services/dialog.service';
import { Page } from '@shared/types/page';
import { SortOf } from '@shared/types/sort';

// local imports
import { FormControl } from '@angular/forms';
import { Permissoes, PermissoesService } from '@permissoes';
import { HttpErrorResult } from '@core/http';
import { PrepostoService } from '@manutencao/services/preposto.service';
import { PrepostoModalComponent } from '@manutencao/modals/preposto-modal/preposto-modal.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { Preposto } from '@manutencao/models/preposto.model';
import { Usuario } from '@manutencao/models/usuario';
import { AlocacaoModalComponent } from '@manutencao/modals/preposto-modal/alocacao-preposto/alocacao-preposto.component';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';

@Component({
  selector: 'app-preposto',
  templateUrl: './preposto.component.html',
  styleUrls: ['./preposto.component.scss']
})
export class PrepostoComponent implements OnInit, AfterViewInit{
  constructor(
    private service: PrepostoService,
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService
  ) {}
  
  breadcrumb: string;
  preposto: Array<Preposto> = [];
  usuarioLista : Array<Usuario>;
  total: number = 0;
  
  buscarDescricaoFormControl : FormControl = new FormControl(null);
  
  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;  
  
  ngOnInit(): void {
    this.buscarTabela();
    this.obterUsuarios();
  }
  
  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_PREPOSTO);
  }

   async obterUsuarios(): Promise<void> {
    try{
      const result = await this.service.obterTodosPrepostos();
      this.usuarioLista = result;
    }
    catch (error) {
      await this.dialog.err(`Informações não carregadas`, (error as HttpErrorResult).messages.join('\n'));
    }
  }

  buscarTabela(): void {
    let sort: SortOf<any> = { column: this.iniciaValoresDaView().sortColumn , direction: this.iniciaValoresDaView().sortDirection};
    const page: Page = { index: this.iniciaValoresDaView().pageIndex, size: this.iniciaValoresDaView().pageSize};

    this.service.obter(sort, page, this.buscarDescricaoFormControl.value ).pipe(
              catchError(err => {
                console.log(err);
                this.dialog.err(
                  'Informações não carregadas',
                  'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte'
                );
                return EMPTY;
              })
            ).subscribe(data => {
                  this.preposto = data.data;
                  this.total = data.total;
                });;
  }

  iniciaValoresDaView() {
    return {
      sortColumn: this.table === undefined || !this.table.sortColumn ? "nome" : this.table.sortColumn,
      sortDirection: this.table === undefined || !this.table.sortDirection ? "asc" : this.table.sortDirection,
      pageIndex: this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize,
    }
  }

  async incluir(): Promise<void> {
    const teveAlteracao: boolean = await PrepostoModalComponent.exibeModalDeIncluir(this.usuarioLista);
    this.buscarDescricaoFormControl.setValue('');
    if (teveAlteracao) { this.buscarTabela()}
  }

  async alterar(item: Preposto): Promise<void> {
    const teveAlteracao: boolean = await PrepostoModalComponent.exibeModalDeAlterar(item,this.usuarioLista);
    this.buscarDescricaoFormControl.setValue('');
    if (teveAlteracao) { this.buscarTabela()}
  }

  async excluir(item: Preposto): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir preposto',
      `Deseja excluir o preposto<br><b>${item.nome}</b>?`
    );

    if (!excluir) { return; }

    try {
      await this.service.excluir(item.id);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'preposto excluído!'
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
    this.service.exportar(sort,page, this.buscarDescricaoFormControl.value);
  }

  onClearInputPesquisar(){ if(!this.buscarDescricaoFormControl.value){ this.buscarTabela()  } }

  nenhumTipoProcesso(preposto: Preposto) {
    return (!preposto.ehCivel
            && !preposto.ehCivelEstrategico
            && !preposto.ehJuizado
            && !preposto.ehPex
            && !preposto.ehProcon
            && !preposto.ehTrabalhista);
  }


TratarCampoPesquisa(){
  let texto = this.buscarDescricaoFormControl.value;
  const a = 'àáäâãèéëêìíïîõòóöôùúüûñçßÿœæŕśńṕẃǵǹḿǘẍźḧ'
  const b = 'aaaaaeeeeiiiiooooouuuuncsyoarsnpwgnmuxzh'
  const p = new RegExp(a.split('').join('|'), 'g')

  texto = texto.toString().toLowerCase().trim()
    .replace(p, c => b.charAt(a.indexOf(c))) // Replace special chars

  texto = texto.replace(/[^a-z0-9\-s ]/gi, "");

 this.buscarDescricaoFormControl.setValue(texto.toString().toUpperCase());
}

}
