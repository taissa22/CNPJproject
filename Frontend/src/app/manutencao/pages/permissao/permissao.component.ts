import { Component, OnInit, ViewChild } from '@angular/core';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { PermissaoModalComponent } from '@manutencao/modals/permissao-modal/permissao-modal.component';
import { PermissaoService } from '@manutencao/services/permissao.service';
import { Page } from '@shared/types/page';
import { SortOf } from '@shared/types/sort';
import { Permissao } from '@manutencao/models/permissao.model';
import { DialogService } from '@shared/services/dialog.service';
import { FormControl } from '@angular/forms';
import { PermissaoModulos } from '@manutencao/models/permissao-modulos.model';
import { EMPTY } from 'rxjs';

@Component({
  selector: 'app-permissao',
  templateUrl: './permissao.component.html',
  styleUrls: ['./permissao.component.scss']
})
export class PermissaoComponent implements OnInit {

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  constructor(
    private service: PermissaoService,
    private dialog: DialogService
    ) { }

    ngOnInit() {
    this.obterModulos();
  }

  //#region Grid
  pagina: number = 1;
  quantidadePorPagina: number = null;
  buscarDescricaoFormControl: FormControl = new FormControl(null);

  coluna: string = null;
  direcao: string = null;

  totalDeRegistros: number;

  datasource: Array<Permissao> = [];

  modulos: Array<PermissaoModulos> = [];
  permissao: string
  modulosTootip = null;
  moduloSelecionado = null;

  buscarTabela() {
    let sort: SortOf<any> = { column: this.iniciaValoresDaView().sortColumn, direction: this.iniciaValoresDaView().sortDirection };
    const page: Page = { index: this.iniciaValoresDaView().pageIndex, size: this.iniciaValoresDaView().pageSize };
    let buscarModulo = this.moduloSelecionado != null ? this.moduloSelecionado.descricao : null;
    this.service
      .obterPaginado(page.index, page.size, this.buscarDescricaoFormControl.value, buscarModulo, sort.column, sort.direction)
      .subscribe(data => {
        this.totalDeRegistros = data.quantidade;
        this.datasource = data.data;
      })
  }

  exportar() {
    let sort: SortOf<any> = { column: this.iniciaValoresDaView().sortColumn, direction: this.iniciaValoresDaView().sortDirection };
    let buscarModulo = this.moduloSelecionado != null ? this.moduloSelecionado.descricao : null;
    return this.service.exportar(this.buscarDescricaoFormControl.value, buscarModulo, sort.column, sort.direction)
  }

  iniciaValoresDaView() {
    let sort: any = this.table === undefined || !this.table.sortColumn ? "nome" : this.table.sortColumn;
    return {
      sortColumn: sort,
      sortDirection: this.table === undefined || !this.table.sortDirection ? "asc" : this.table.sortDirection,
      pageIndex: this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize,
    }
  }

  obterModulos() {
    try {
      this.service
        .obterModulos()
        .subscribe(data => {
          this.modulos = data;
        });
    }
    catch (error) {
      this.dialog.err(
        'Informações não carregadas',
        'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte'
      );
      return EMPTY;
    }
  }

  async alterar(item: any, listaModulos: any) {
    //var listaModulos = [];
    // if (modulos) {
    //   for (let i = 0; i < modulos.length; i++) {
    //     listaModulos.push(modulos)
    //   }
    // }
    const teveAlteracao: boolean = await PermissaoModalComponent.exibeModalDeAlterar(item, listaModulos == null ? '' : listaModulos, this.modulos);

    if (teveAlteracao) {
      this.buscarTabela()
    }
  }

  openTootltip(modulos, permissao){
    this.modulosTootip = modulos == null ? null : modulos.split(';');
    this.permissao = permissao;
  }

}
