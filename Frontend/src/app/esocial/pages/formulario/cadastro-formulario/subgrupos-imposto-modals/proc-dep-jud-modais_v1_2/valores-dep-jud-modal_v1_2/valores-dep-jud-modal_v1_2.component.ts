import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { ValoresDepJudModalService } from '@esocial/services/formulario/subgrupos/imposto-subgrupos-services/valores-dep-jud-modal.service';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { DialogService } from '@shared/services/dialog.service';
import { ValoresModal_v1_2Component } from './valores-modal_v1_2/valores-modal_v1_2.component';
import { ErrorLib } from '@esocial/libs/error-lib';

@Component({
  selector: 'app-valores-dep-jud-modal_v1_2',
  templateUrl: './valores-dep-jud-modal_v1_2.component.html',
  styleUrls: ['./valores-dep-jud-modal_v1_2.component.scss']
})
export class ValoresDepJudModal_v1_2Component implements AfterViewInit {

  breadcrumb: string;
  constructor(
    private service: ValoresDepJudModalService, 
    private dialog: DialogService) { }

  @Input() formularioId: number;
  @Input() codInfoprocret: number;
  @Input() temPermissaoBlocoCeDeE: boolean;

  @Output() aoAbrirPopup = new EventEmitter();

  total: number = 0;
  dataSource: Array<any>;
  temDados: boolean = false;
  valoresSelecionado = -1;

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
      this.service.obterListaValores(this.codInfoprocret, view.pageIndex).then(x => {
        this.dataSource = x.lista,
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
    const teveAlteracao: boolean = await ValoresModal_v1_2Component.exibeModalIncluir(this.formularioId, this.codInfoprocret);

    if (teveAlteracao) {
      this.buscarTabela();
    }
  }

  async alterar(codInfoValores: number): Promise<void> {
    this.aoAbrirPopup.emit();
    const teveAlteracao: boolean = await ValoresModal_v1_2Component.exibeModalAlterar(this.formularioId, codInfoValores, this.codInfoprocret, true);

    if (teveAlteracao) {
      this.buscarTabela();
    }
  }

  async consultar(codInfoValores: number): Promise<void> {
    await ValoresModal_v1_2Component.exibeModalAlterar(this.formularioId, codInfoValores, this.codInfoprocret, false);
  }

  async excluir(codInfoValores: number): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Valores',
      `Deseja excluir os Valores<br><b>${codInfoValores}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(this.formularioId, this.codInfoprocret, codInfoValores);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Valores excluído!'
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

  selecionarValores(index: number): void {
    if (this.valoresSelecionado == index) {
      this.valoresSelecionado = -1;
      return;
    }
    this.valoresSelecionado = index;
  }
}
