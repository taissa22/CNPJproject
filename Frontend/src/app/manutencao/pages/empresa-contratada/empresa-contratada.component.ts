import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { HttpErrorResult } from '@core/http';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { EmpresaContratadaModalComponent } from '@manutencao/modals/empresa-contratada-modal/empresa-contratada-modal.component';
import { ListaEmpresasContratadasResponse } from '@manutencao/models/lista-empresas-contratadas-response';
import { EmpresaContratadaService } from '@manutencao/services/empresa-contratada.service';
import { Permissoes } from '@permissoes';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { DialogService } from '@shared/services/dialog.service';
import { Page } from '@shared/types/page';
import { SortOf } from '@shared/types/sort';

@Component({
  selector: 'app-empresa-contratada',
  templateUrl: './empresa-contratada.component.html',
  styleUrls: ['./empresa-contratada.component.css']
})
export class EmpresaContratadaComponent implements OnInit {

  constructor(
    private breadcrumbsService: BreadcrumbsService,
    private dialog: DialogService,
    private service: EmpresaContratadaService
  ) { }

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  buscarDescricaoFormControl: FormControl = new FormControl('');
  breadcrumb: string;

  listaEmpresasContratadas: Array<ListaEmpresasContratadasResponse> = [];
  total = 0;

  async ngOnInit() {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_EMPRESA_CONTRATADA);
    await this.buscarTabela();
  }

  async buscarTabela(){
    try {
      let sort: SortOf<any> = { column: this.iniciaValoresDaView().sortColumn, direction: this.iniciaValoresDaView().sortDirection, };
      const page: Page = { index: this.iniciaValoresDaView().pageIndex, size: this.iniciaValoresDaView().pageSize,};

      await this.service
        .obterListaEmpresaContratadaAsync(
          page.index,
          page.size,
          sort.direction,
          sort.column,
          this.buscarDescricaoFormControl.value
        )
        .then((x) => {
          this.listaEmpresasContratadas = x.lista;
          this.total = x.total;
        });
    } catch (error) {
      await this.dialog.err(
        "Erro ao buscar.",
        "Não foi possível realizar a busca dos contratos."
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
    const teveAlteracao: boolean = await EmpresaContratadaModalComponent.exibeModalDeIncluir();
    this.buscarDescricaoFormControl.setValue('');
    if (teveAlteracao) { this.buscarTabela() }
  }

  async alterar(codEmpresaContratada: number, nomEmpresaContratada: string): Promise<void> {
    const teveAlteracao: boolean = await EmpresaContratadaModalComponent.exibeModalDeAlterar(codEmpresaContratada, nomEmpresaContratada);
    this.buscarDescricaoFormControl.setValue('');
    if (teveAlteracao) { this.buscarTabela() }
  }

  async excluir(codEmpresa: number): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      "Excluir Contrato",
      `Deseja excluir o contrato selecionado?`
    );

    if (excluir) {
      try {
        await this.service.excluirEmpresaContratadaAsync(codEmpresa);
        this.buscarTabela();
        await this.dialog.alert(
          "Exclusão realizada com sucesso",
          "Empresa contratada excluída!"
        );
      } catch (error) {
        await this.dialog.err(
          `Exclusão não realizada`,
          (error as HttpErrorResult).messages.join("\n")
        );
      }
    }
  }

  exportarListaEmpresaContratadaAsync() {
    let sort: SortOf<any> = { column: this.iniciaValoresDaView().sortColumn, direction: this.iniciaValoresDaView().sortDirection, };
    this.service.downloadListaEmpresaContratadaAsync(sort.direction, sort.column, this.buscarDescricaoFormControl.value);
  }

  exportarLogEmpresaContratadaAsync() {
    this.service.downloadLogEmpresaContratadaAsync();
  }

  onClearInputPesquisar() {
    if (!this.buscarDescricaoFormControl.value) { this.buscarTabela() }
  }


}
