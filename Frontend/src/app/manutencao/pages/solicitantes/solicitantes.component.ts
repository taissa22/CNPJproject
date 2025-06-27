import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { SolicitantesModalComponent } from '@manutencao/modals/solicitantes/solicitantes.component';
import { Solicitante } from '@manutencao/models/solicitante';
import { SolicitanteService } from '@manutencao/services/solicitante.service';
import { Permissoes } from '@permissoes';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { DialogService } from '@shared/services/dialog.service';
import { Page } from '@shared/types/page';
import { SortOf } from '@shared/types/sort';

@Component({
  selector: 'app-solicitantes',
  templateUrl: './solicitantes.component.html',
  styleUrls: ['./solicitantes.component.scss']
})
export class SolicitantesComponent implements OnInit {

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  constructor(
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService,
    private service: SolicitanteService
  ) { }

  async ngOnInit() {
    this.buscarTabela();
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_SOLICITANTE);
  }

  titulo = "Solicitantes"
  solicitantes: Array<Solicitante>;
  total: number;
  breadcrumb: string;

  buscarDescricaoFormControl: FormControl = new FormControl(null);

  async onClearInputPesquisar() {
    if (!this.buscarDescricaoFormControl.value) {
      await this.buscarTabela()
    }
  }


  async buscarTabela() {
    let sort: SortOf<any> = { column: this.iniciaValoresDaView().sortColumn, direction: this.iniciaValoresDaView().sortDirection };
    const page: Page = { index: this.iniciaValoresDaView().pageIndex, size: this.iniciaValoresDaView().pageSize };

    const result = await this.service.listarGridAsync(this.buscarDescricaoFormControl.value, sort.column, sort.direction, page.index - 1, page.size);
    this.solicitantes = result.lista;
    this.total = result.total;
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
    const teveAlteracao: boolean = await SolicitantesModalComponent.exibeModalDeIncluir();
    this.buscarDescricaoFormControl.setValue('');
    if (teveAlteracao) { return await this.buscarTabela() }
  }

  async alterar(item: Solicitante): Promise<void> {
    const teveAlteracao: boolean = await SolicitantesModalComponent.exibeModalDeAlterar(item);
    this.buscarDescricaoFormControl.setValue('');
    if (teveAlteracao) { return await this.buscarTabela() }
  }

  async excluir(item: Solicitante): Promise<void> {
    const resposta = await this.dialog.confirm('Excluir Solicitante', 'Deseja excluir o Solicitante selecionado?');
    if (resposta) {
      try {
        await this.service.excluir(item.codSolicitanteHash);
        await this.dialog.alert('Excluido com sucesso!', 'Solicitante excluído com sucesso.')
        return await this.buscarTabela();
      } catch (error) {
        await this.dialog.info('Erro ao excluir', error.error.detail);
      }

    }
  }

  async exportar() {
    let sort: SortOf<any>;
    sort = { column: 'nome', direction: 'asc' };
    sort = { column: this.iniciaValoresDaView().sortColumn, direction: this.iniciaValoresDaView().sortDirection };
    await this.service.exportar(this.buscarDescricaoFormControl.value, sort.column, sort.direction);
  }
}
