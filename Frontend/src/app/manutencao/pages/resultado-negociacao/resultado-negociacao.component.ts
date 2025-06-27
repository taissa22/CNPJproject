import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { HttpErrorResult } from '@core/http';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { ResultadoNegociacaoModalComponent } from '@manutencao/modals/resultado-negociacao-modal/resultado-negociacao-modal.component';
import { ListaResultadoNegociacaoResponse } from '@manutencao/models/lista-resultado-negociacao-response';
import { ResultadoNegociacaoService } from '@manutencao/services/resultado-negociacao.service';
import { Permissoes } from '@permissoes';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { DialogService } from '@shared/services/dialog.service';
import { Page } from '@shared/types/page';
import { SortOf } from '@shared/types/sort';

@Component({
  selector: 'app-resultado-negociacao',
  templateUrl: './resultado-negociacao.component.html',
  styleUrls: ['./resultado-negociacao.component.css']
})
export class ResultadoNegociacaoComponent implements OnInit {

  constructor(
    private breadcrumbsService: BreadcrumbsService,
    private dialog: DialogService,
    private service: ResultadoNegociacaoService
  ) { }

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  buscarDescricaoFormControl: FormControl = new FormControl('');
  breadcrumb: string;

  listaResultadoNegociacao: Array<ListaResultadoNegociacaoResponse> = [];
  total = 0;

  async ngOnInit() {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_RESULTADO_NEGOCIACAO);
    await this.buscarTabela();
  }

  async buscarTabela(){
    try {
      let sort: SortOf<any> = { column: this.iniciaValoresDaView().sortColumn, direction: this.iniciaValoresDaView().sortDirection, };
      const page: Page = { index: this.iniciaValoresDaView().pageIndex, size: this.iniciaValoresDaView().pageSize,};

      await this.service
        .obterListaResultadoNegociacaoAsync(
          page.index,
          page.size,
          sort.direction,
          sort.column,
          this.buscarDescricaoFormControl.value
        )
        .then((x) => {
          this.listaResultadoNegociacao = x.lista;
          this.total = x.total;
        });
    } catch (error) {
      await this.dialog.err(
        "Erro ao buscar.",
        "Não foi possível realizar a busca dos resultado de negociação."
      );
    }
  }

  iniciaValoresDaView() {
    return {
      sortColumn: this.table === undefined || !this.table.sortColumn ? "id" : this.table.sortColumn,
      sortDirection: this.table === undefined || !this.table.sortDirection ? "asc" : this.table.sortDirection,
      pageIndex: this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize,
    }
  }

  async incluir(): Promise<void> {
    const teveAlteracao: boolean = await ResultadoNegociacaoModalComponent.exibeModalDeIncluir();
    this.buscarDescricaoFormControl.setValue('');
    if (teveAlteracao) { this.buscarTabela() }
  }

  async alterar(codResultadoContratada: number): Promise<void> {
    const teveAlteracao: boolean = await ResultadoNegociacaoModalComponent.exibeModalDeAlterar(codResultadoContratada);
    this.buscarDescricaoFormControl.setValue('');
    if (teveAlteracao) { this.buscarTabela() }
  }

  async excluir(codResultado: number): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      "Excluir Resultado de Negociação",
      `Deseja excluir o resultado de negociação selecionado?`
    );

    if (excluir) {
      try {
        await this.service.excluirResultadoNegociacaoAsync(codResultado);
        this.buscarTabela();
        await this.dialog.alert(
          "Exclusão realizada com sucesso",
          "Resultado de negociação excluído!"
        );
      } catch (error) {
        await this.dialog.err(
          `A exclusão não poderá ser realizada`,
          error.error
        );
      }
    }
  }

  exportarListaResultadoNegociacaoAsync() {
    let sort: SortOf<any> = { column: this.iniciaValoresDaView().sortColumn, direction: this.iniciaValoresDaView().sortDirection, };
    this.service.downloadListaResultadoNegociacaoAsync(sort.direction, sort.column, this.buscarDescricaoFormControl.value);
  }

  exportarLogResultadoNegociacaoAsync() {
    this.service.downloadLogResultadoNegociacaoAsync();
  }

  onClearInputPesquisar() {
    if (!this.buscarDescricaoFormControl.value) { this.buscarTabela() }
  }

}
