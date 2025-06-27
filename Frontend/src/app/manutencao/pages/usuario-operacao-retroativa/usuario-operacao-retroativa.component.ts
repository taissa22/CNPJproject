import { AfterViewInit, Component, ViewChild, OnDestroy, OnInit, ValueProvider } from '@angular/core';
import { DialogService } from '@shared/services/dialog.service';
import { Page } from '@shared/types/page';
import { SortOf } from '@shared/types/sort';
import { FormControl } from '@angular/forms';
import { HttpErrorResult } from '@core/http';
import { UsuarioOperacaoRetroativa } from '@manutencao/models/usuario-operacao-retroativa';
import { UsuarioOperacaoRetroativaService } from '@manutencao/services/usuario-operacao-retroativa';
import { catchError } from 'rxjs/operators';
import { EMPTY } from 'rxjs';
import { UsuarioOperacaoRetroativaModalComponent } from '@manutencao/modals/usuario-operacao-retroativa-modal/usuario-operacao-retroativa-modal.component';
import { Usuario } from '@manutencao/models/usuario';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';

@Component({
  selector: 'app-usuario-operacao-retroativa',
  templateUrl: './usuario-operacao-retroativa.component.html',
  styleUrls: ['./usuario-operacao-retroativa.component.scss']
})

export class UsuarioOperacaoRetroativaComponent implements AfterViewInit {
  constructor(
    private service: UsuarioOperacaoRetroativaService,
    private dialog: DialogService
  ) {}

  datasource : Array<UsuarioOperacaoRetroativa> = [];
  total: number = 0;

  UsuariosLista: Array<Usuario> = [];

  buscarDescricaoFormControl : FormControl = new FormControl;

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  ngAfterViewInit(): void {         
    this.obterUsuarios();
    this.buscarTabela(); 
  }

  buscarTabela(): void {
    let sort: SortOf<any> = { column: this.iniciaValoresDaView().sortColumn , direction: this.iniciaValoresDaView().sortDirection};
    const page: Page = { index: this.iniciaValoresDaView().pageIndex, size: this.iniciaValoresDaView().pageSize};

    this.service.obterPaginado(page, sort, this.buscarDescricaoFormControl.value).pipe(
      catchError(err => {
        console.log(err);
        this.dialog.err(
          'Informações não carregadas',
          'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte'
        );
        return EMPTY;
      })
    ).subscribe(data => {
          this.datasource = data.lista;
          this.total = data.total;
        });      

  }

  iniciaValoresDaView() {
    let sort: any = this.table === undefined || !this.table.sortColumn ? "id" : this.table.sortColumn;
    return {
      sortColumn: sort,
      sortDirection: this.table === undefined || !this.table.sortDirection ? "asc" : this.table.sortDirection,
      pageIndex: this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize,
    }
  }  

  async obterUsuarios(): Promise<void> {
    try{
      const result = await this.service.obterUsuarios();
      this.UsuariosLista = result;
    }
    catch (error) {
      await this.dialog.err(`Informações não carregadas`, (error as HttpErrorResult).messages.join('\n'));
    }
  } 


  async incluir(): Promise<void> {
    const teveAlteracao: boolean = await UsuarioOperacaoRetroativaModalComponent.exibeModalDeIncluir(this.UsuariosLista);
    this.buscarDescricaoFormControl.setValue('');

    if (teveAlteracao) { 
      this.buscarTabela()
    }
  }

  async alterar(item: UsuarioOperacaoRetroativa): Promise<void> {
    const teveAlteracao: boolean = await UsuarioOperacaoRetroativaModalComponent.exibeModalDeAlterar(item);
    this.buscarDescricaoFormControl.setValue('');
    if (teveAlteracao) { 
      this.buscarTabela()
    }
  }

  async excluir(item: UsuarioOperacaoRetroativa): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Usuário',
      `Deseja excluir o Usuário <br><b>${item.usuario.nome}</b>?`
    );

    if (!excluir) { return; }

    try {
      await this.service.excluir(item.codUsuario);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Usuário excluído!'
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
    this.service.exportar(sort,this.buscarDescricaoFormControl.value);
  }

  onClearInputPesquisar(){ if(!this.buscarDescricaoFormControl.value){ this.buscarTabela()  } }
}
