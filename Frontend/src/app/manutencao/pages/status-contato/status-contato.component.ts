import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { HttpErrorResult } from '@core/http';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { StatusContatoModalComponent } from '@manutencao/modals/status-contato-modal/status-contato-modal.component';
import { StatusContatoResponse } from '@manutencao/models/status-contato-response';
import { StatusContatoService } from '@manutencao/services/status-contato.service';
import { Permissoes } from '@permissoes';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { DialogService } from '@shared/services/dialog.service';
import { Page } from '@shared/types/page';
import { SortOf } from '@shared/types/sort';

@Component({
  selector: 'app-status-contato',
  templateUrl: './status-contato.component.html',
  styleUrls: ['./status-contato.component.scss']
})
export class StatusContatoComponent implements OnInit {


  constructor(
    private breadcrumbsService: BreadcrumbsService,
    private dialog: DialogService,
    private service: StatusContatoService
  ) { }

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  buscarDescricaoFormControl: FormControl = new FormControl('');
  breadcrumb: string;

  listaStatusContato: Array<StatusContatoResponse> = [];
  total = 0;

  async ngOnInit() {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_STATUS_CONTATO);
    await this.buscarTabela();
  }

  async buscarTabela(){
    try {
      let sort: SortOf<any> = { column: this.iniciaValoresDaView().sortColumn, direction: this.iniciaValoresDaView().sortDirection, };
      const page: Page = { index: this.iniciaValoresDaView().pageIndex, size: this.iniciaValoresDaView().pageSize,};

      await this.service
        .obterListaStatusContatoAsync(
          page.index,
          page.size,
          sort.direction,
          sort.column,
          this.buscarDescricaoFormControl.value
        )
        .then((x) => {
          this.listaStatusContato = x.lista;
          this.total = x.total;
        });
    } catch (error) {
      await this.dialog.err(
        "Erro ao buscar.",
        "Não foi possível realizar a busca dos Status de Contato."
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
    const teveAlteracao: boolean = await StatusContatoModalComponent.exibeModalDeIncluir();
    this.buscarDescricaoFormControl.setValue('');
    if (teveAlteracao) { this.buscarTabela() }
  }

  async alterar(codStatusContato: number): Promise<void> {
    const teveAlteracao: boolean = await StatusContatoModalComponent.exibeModalDeAlterar(codStatusContato);
    this.buscarDescricaoFormControl.setValue('');
    if (teveAlteracao) { this.buscarTabela() }
  }

  async excluir(codStatusContato: number): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      "Excluir Status de Contato",
      `Deseja excluir o Status de Contato selecionado?`
    );

    if (excluir) {
      try {
        await this.service.excluirStatusContatoAsync(codStatusContato);
        this.buscarTabela();
        await this.dialog.alert(
          "Exclusão realizada com sucesso",
          "Status de Contato excluído!"
        );
      } catch (error) {
        await this.dialog.err(
          `O status de contato não poderá ser excluído`,
          error.error
          // (error as HttpErrorResult).messages.join("\n")
        );
      }
    }
  }

  exportarListaStatusContatoAsync() {
    let sort: SortOf<any> = { column: this.iniciaValoresDaView().sortColumn, direction: this.iniciaValoresDaView().sortDirection, };
    this.service.downloadListaStatusContatoAsync(sort.direction, sort.column, this.buscarDescricaoFormControl.value);
  }

  exportarLogStatusContatoAsync() {
    this.service.downloadLogStatusContatoAsync();
  }

  onClearInputPesquisar() {
    if (!this.buscarDescricaoFormControl.value) { this.buscarTabela() }
  }

}
