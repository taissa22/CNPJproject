import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { DeducoesDepJudModalService } from '@esocial/services/formulario/subgrupos/imposto-subgrupos-services/deducoes-dep-jud-modal.service';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { DialogService } from '@shared/services/dialog.service';
import { DeducoesModal_v1_2Component } from './deducoes-modal_v1_2/deducoes-modal_v1_2.component';
import { ErrorLib } from '@esocial/libs/error-lib';
import { TipoDeducoesEnum } from '@esocial/enum/tipo-deducoes';
import { DialogServiceCssClass } from '@shared/layout/dialog-service-css-class';

@Component({
  selector: 'app-detalhamento-deducoes-modal_v1_2',
  templateUrl: './detalhe-deducoes-modal_v1_2.component.html',
  styleUrls: ['./detalhe-deducoes-modal_v1_2.component.scss']
})
export class DetalhamentoDeducoesModal_v1_2Component implements AfterViewInit {

  breadcrumb: string;
  constructor(
    private service: DeducoesDepJudModalService, 
    private dialog: DialogService) { }

  @Input() formularioId: number;
  @Input() codigoInfoValores: number;
  @Input() temPermissaoBlocoCeDeE: boolean;
  @Input() podeIncluir: boolean;

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
      this.service.obterListaDeducoes(this.codigoInfoValores, view.pageIndex).then(x => {
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
    if (!this.podeIncluir) {
      await this.dialog.infoCustom(
        'Incluir Detalhamento das Deduções',
        `Para inclusão de detalhamento de deduções o <b>"Valor Rendimento Exigibilidade Suspensa"</b> do grupo <b>"Valores"</b> deve ser informado.`,
        DialogServiceCssClass.cssDialogo35,
        true
      );
      return;
    }

    this.aoAbrirPopup.emit();
    const teveAlteracao: boolean = await DeducoesModal_v1_2Component.exibeModalIncluir(this.formularioId, this.codigoInfoValores);

    if (teveAlteracao) {
      this.buscarTabela();
    }
  }

  async alterar(codigoDedSusp: number): Promise<void> {
    this.aoAbrirPopup.emit();
    const teveAlteracao: boolean = await DeducoesModal_v1_2Component.exibeModalAlterar(this.formularioId, this.codigoInfoValores, codigoDedSusp, true);

    if (teveAlteracao) {
      this.buscarTabela();
    }
  }

  async consultar(codigoDedSusp: number): Promise<void> {
    await DeducoesModal_v1_2Component.exibeModalAlterar(this.formularioId, this.codigoInfoValores, codigoDedSusp, false);
  }

  async excluir(codigoDedSusp: number): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Detalhamento das Deduções',
      `Deseja excluir o Detalhamento das Deduções<br><b>${codigoDedSusp}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(this.formularioId, this.codigoInfoValores, codigoDedSusp);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Detalhamento das Deduções excluído!'
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

  converteTipoDeducoes(item: number){
    return TipoDeducoesEnum[item];
  }
}
