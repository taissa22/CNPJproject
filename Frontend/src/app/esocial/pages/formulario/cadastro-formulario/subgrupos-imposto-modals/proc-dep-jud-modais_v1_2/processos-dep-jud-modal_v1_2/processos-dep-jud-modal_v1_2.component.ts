import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { ProcessosDepJudModalService } from '@esocial/services/formulario/subgrupos/imposto-subgrupos-services/processos-dep-jud-modal.service';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { DialogService } from '@shared/services/dialog.service';
import { ProcessosModal_v1_2Component } from './processos-modal_v1_2/processos-modal_v1_2.component';
import { ErrorLib } from '@esocial/libs/error-lib';

@Component({
  selector: 'app-processos-dep-jud-modal_v1_2',
  templateUrl: './processos-dep-jud-modal_v1_2.component.html',
  styleUrls: ['./processos-dep-jud-modal_v1_2.component.scss']
})
export class ProcessosDepJudModal_v1_2Component implements AfterViewInit {

  breadcrumb: string;
  constructor(
    private service: ProcessosDepJudModalService, 
    private dialog: DialogService) { }

  @Input() formularioId: number;
  @Input() codIrrf: number;
  @Input() temPermissaoBlocoCeDeE: boolean;

  @Output() aoAbrirPopup = new EventEmitter();

  total: number = 0;
  dataSource: Array<any>;
  temDados: boolean = false;
  processoSelecionado = -1;

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  async ngAfterViewInit(): Promise<void> {
    this.buscarTabela();
  }

  iniciaValoresDaView() {
    let sort: any =
      this.table === undefined || !this.table.sortColumn
        ? 'id'
        : this.table.sortColumn;
    return {
      sortColumn: sort,
      sortDirection:
        this.table === undefined || !this.table.sortDirection
          ? 'asc'
          : this.table.sortDirection,
      pageIndex:
        this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize
    };
  }
  async buscarTabela() {
      let view = this.iniciaValoresDaView();
      try {
        this.service.obterListaProcessos(this.codIrrf, view.pageIndex).then(x => {
          this.dataSource = x.lista;
          this.total = x.total
        });
      } catch (error) {
        const mensagem = ErrorLib.ConverteMensagemErro(error);
        this.dialog.err(
          'Informações não carregadas',
          mensagem
        );
        this.dataSource = []; // ou atribua o valor desejado em caso de erro
      }
  }

  async carregarBloco(): Promise<void> {
    // this.aoAbrirPopup.emit();
    // const teveAlteracao: boolean = await CarregarBloco_v1_2_Component.exibeModal(this.formularioId, "periodo", this.contratoId);

    // if (teveAlteracao) {
    //   await this.buscarTabela();
    // }
  }

  async incluir(): Promise<void> {
    this.aoAbrirPopup.emit();
    const teveAlteracao: boolean = await ProcessosModal_v1_2Component.exibeModalIncluir(this.formularioId, this.codIrrf);
    if (teveAlteracao) {
      await this.buscarTabela(); 
    }
  }

// async excluir(){

// }

  async alterar(idEsF2501Infoprocret: number): Promise<void> {
    this.aoAbrirPopup.emit();
    const teveAlteracao: boolean = await ProcessosModal_v1_2Component.exibeModalAlterar(this.formularioId, this.codIrrf, idEsF2501Infoprocret, true);

    if (teveAlteracao) {
      await this.buscarTabela();
    }
  }

  async consultar(idEsF2501Infoprocret: number): Promise<void> {
    await ProcessosModal_v1_2Component.exibeModalAlterar(this.formularioId, this.codIrrf, idEsF2501Infoprocret, false);
  }

  async excluir(idEsF2501Infoprocret: number): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Processo',
      `Deseja excluir o Processo<br><b>${idEsF2501Infoprocret}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(this.formularioId, this.codIrrf, idEsF2501Infoprocret);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Processo excluído!'
      );
      this.buscarTabela();
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      if (mensagem.includes('registrado')) {
        await this.dialog.info(
          `Exclusão não permitida`,
          mensagem
        );
        return;
      }
      await this.dialog.err(
        `Exclusão não realizada`,
        mensagem
      );
    }
  }

  converterTipoProcesso(tipo: number): string{
    return tipo == 1 ? 'Administrativo' : 'Judicial';
  }

  selecionarProcesso(index: number): void {
    if (this.processoSelecionado == index) {
      this.processoSelecionado = -1;
      return;
    }
    this.processoSelecionado = index;
  }
}
