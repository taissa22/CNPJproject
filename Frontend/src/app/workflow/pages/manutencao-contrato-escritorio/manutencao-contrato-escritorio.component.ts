import { Component, OnInit, ViewChild } from "@angular/core";
import { ManutencaoContratoEscritorioService } from "../../services/manutencao-contrato-escritorio.service";
import { DialogService } from "@shared/services/dialog.service";
import { Permissoes } from "@permissoes";
import { BreadcrumbsService } from "@shared/services/breadcrumbs.service";
import { SortOf } from "@shared/types/sort";
import { Page } from "@shared/types/page";
import { FormControl } from "@angular/forms";
import { SisjurTable } from "@libs/sisjur/sisjur-table/sisjur-table.component";
import { SisjurPaginator } from "@libs/sisjur/sisjur-paginator/sisjur-paginator.component";
import { ContratoEscritorioResponse } from "../../model/contrato-escritorio-response";
import { HttpErrorResult } from "@core/http";
import { ManutencaoContratoEscritorioModalComponent } from "../../modal/manutencao-contrato-escritorio-modal/manutencao-contrato-escritorio-modal.component";

@Component({
  selector: "app-manutencao-contrato-escritorio",
  templateUrl: "./manutencao-contrato-escritorio.component.html",
  styleUrls: ["./manutencao-contrato-escritorio.component.scss"],
})
export class ManutencaoContratoEscritorioComponent implements OnInit {
  breadcrumb: string;
  constructor(
    private service: ManutencaoContratoEscritorioService,
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService
  ) { }

  tipoContratosList: Array<any>;
  escritoriosList: Array<any>;

  contratos: Array<ContratoEscritorioResponse> = [];
  total: number = 0;

  tipoContratoFormControl: FormControl = new FormControl(null);
  // escritorioFormControl: FormControl = new FormControl(null);
  // buscarCnpjFormControl: FormControl = new FormControl(null);
  buscarNomContratoFormControl: FormControl = new FormControl(null);

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(
      Permissoes.ACESSAR_MANUTENCAO_CONTRATO_ESCRITORIO
    );
  }

  async ngOnInit(): Promise<void> {
    await this.iniciarCombosAsync();
  }

  async iniciarCombosAsync() {
    try {
      this.tipoContratosList = await this.service.obterTipoContrato();
      // this.escritoriosList = await this.service.obterEscritorio();
    } catch (erro) {
      await this.dialog.err(`Exclusão não realizada`, erro);
    }
  }

  async buscarTabela() {
    try {
      let sort: SortOf<any> = {
        column: this.iniciaValoresDaView().sortColumn,
        direction: this.iniciaValoresDaView().sortDirection,
      };
      const page: Page = {
        index: this.iniciaValoresDaView().pageIndex,
        size: this.iniciaValoresDaView().pageSize,
      };

      await this.service
        .obterListaContrato(
          page.index,
          page.size,
          sort.direction,
          this.tipoContratoFormControl.value,
          // this.escritorioFormControl.value,
          // this.buscarCnpjFormControl.value,
          this.buscarNomContratoFormControl.value,
          sort.column
        )
        .then((x) => {
          this.contratos = x.lista;
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
      sortColumn:
        this.table === undefined || !this.table.sortColumn
          ? "Nome"
          : this.table.sortColumn,
      sortDirection:
        this.table === undefined || !this.table.sortDirection
          ? "asc"
          : this.table.sortDirection,
      pageIndex:
        this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize,
    };
  }

  selecionarTipoProcesso() {
    // this.buscarCnpjFormControl.reset();
    this.buscarNomContratoFormControl.reset();
    this.buscarTabela();
  }
  
  async incluir(): Promise<void> {
    const teveAlteracao: boolean = await ManutencaoContratoEscritorioModalComponent.exibeModalDeIncluir();
    if (teveAlteracao) {
      this.buscarTabela();
    }
  }

  async alterar(codContrato: number): Promise<void> {
    const teveAlteracao: boolean =
      await ManutencaoContratoEscritorioModalComponent.exibeModalDeAlterar(codContrato);

    if (teveAlteracao)
      this.buscarTabela();
  }

  async excluir(codContrato: number): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      "Excluir Contrato",
      `Deseja excluir o contrato selecionado?`
    );

    if (excluir) {
      try {
        await this.service.excluirContrato(codContrato);
        await this.dialog.alert(
          "Exclusão realizada com sucesso",
          "Contrato excluído!"
        );
        this.buscarTabela();
      } catch (error) {
        await this.dialog.err(
          `Exclusão não realizada`,
          (error as HttpErrorResult).messages.join("\n")
        );
      }
    }
  }

  async exportarListaContrato() {
    try {
      let sort: SortOf<any>;
      sort = { column: "descricao", direction: "asc" };
      sort = {
        column: this.iniciaValoresDaView().sortColumn,
        direction: this.iniciaValoresDaView().sortDirection,
      };
      await this.service.exportarListaContrato(
        sort.direction,
        this.tipoContratoFormControl.value,
        // this.escritorioFormControl.value,
        // this.buscarCnpjFormControl.value,
        this.buscarNomContratoFormControl.value,
        sort.column
      );
    } catch (error) {
      await this.dialog.err(
        'Erro no download',
        'Não foi possível realizar o download da lista de contratos.'
      );
    }
  }

  async exportarLogContrato() {
    try {
      await this.service.exportarLogContrato();
    } catch (error) {
      await this.dialog.err(
        'Erro no download',
        'Não foi possível realizar o download do log de contratos.'
      );
    }
  }

  onClearInputPesquisar() {
    if (!this.buscarNomContratoFormControl.value) {
      this.buscarTabela();
    }
  }

  listarEscritorio(escritorios: string){
    return escritorios.split(', ');
  }

  titleNumeroContrato(numJec: string, numProcon: string): string{
    var pipe = numJec && numProcon ? ' | ' : ''
    var jec = numJec ? numJec : '';
    var procon = numProcon ? numProcon : '';
    return jec+ pipe + procon;
  }

}
