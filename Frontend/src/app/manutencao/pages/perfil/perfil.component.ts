import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { PermissaoModalComponent } from '@manutencao/modals/permissao-modal/permissao-modal.component';
import { PermissaoModulos } from '@manutencao/models/permissao-modulos.model';
import { Permissao } from '@manutencao/models/permissao.model';
import { TelaPerfil } from '@manutencao/models/tela-perfil.model';
import { TelaPerfilService } from '@manutencao/services/tela-perfil.service';
import { Permissoes } from '@permissoes';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { DialogService } from '@shared/services/dialog.service';
import { Page } from '@shared/types/page';
import { SortOf } from '@shared/types/sort';
import { EMPTY } from 'rxjs';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  constructor(
    private service: TelaPerfilService,
    private dialog: DialogService,
    private router: Router,
    private breadcrumbService: BreadcrumbsService
  ) { }

  ngOnInit() {
    this.obterModulos();
    this.breadcrumbService.nomeBreadcrumb(Permissoes.ACESSAR_PERFIL).then((x) => { this.breadcrump = x });
  }

  breadcrump: string;
  pagina: number = 1;
  quantidadePorPagina: number = null;
  filtro: FormControl = new FormControl(null);
  statusSelecionado = { name: "Todos" }
  status = [
    { name: "Todos" },
    { name: "Ativo" },
    { name: "Inativo" }
  ]
  moduloSelecionado = { name: "Todos" };
  modulos = [
    { name: "Todos" },
    { name: "Web" },
    { name: "Interno" }
  ];

  coluna: string = null;
  direcao: string = null;

  totalDeRegistros: number;

  perfil: Array<TelaPerfil> = [];

  iniciaValoresDaView() {
    let sort: any = this.table === undefined || !this.table.sortColumn ? "padrao" : this.table.sortColumn;
    return {
      sortColumn: sort,
      sortDirection: this.table === undefined || !this.table.sortDirection ? "asc" : this.table.sortDirection,
      pageIndex: this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize,
    }
  }

  obterModulos() {
    try {
      let sort: SortOf<any> = { column: this.iniciaValoresDaView().sortColumn, direction: this.iniciaValoresDaView().sortDirection };
      const page: Page = { index: this.iniciaValoresDaView().pageIndex, size: this.iniciaValoresDaView().pageSize };
      this.service
        .obter(page.index, page.size, this.filtro.value, this.statusSelecionado.name, this.moduloSelecionado.name, sort.column, sort.direction)
        .subscribe(data => {
          this.perfil = data.data;
          this.totalDeRegistros = data.quantidade
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

  alterar(codigo) {
    this.router.navigateByUrl(`/manutencao/perfil/${btoa(codigo)}`)
  }

  Novo() {
    this.router.navigateByUrl('/manutencao/perfil/Y3JlYXRlUGVyZmlsUGF0aA==')
  }

  excluir(codigo) {
    this.dialog.confirm(`Deseja excluir o perfil ${codigo}?`).then((result) => {
      if (result) {
        this.service.excluir(codigo);
        this.obterModulos();
      }
    });
  }

  exportar() {
    let sort: SortOf<any> = { column: this.iniciaValoresDaView().sortColumn, direction: this.iniciaValoresDaView().sortDirection };
    this.service.exportar(this.filtro.value, this.statusSelecionado.name, this.moduloSelecionado.name, sort.column, sort.direction);
  }

}
