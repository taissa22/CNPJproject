import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { PensaoDepJudModalService } from '@esocial/services/formulario/subgrupos/imposto-subgrupos-services/pensao-dep-jud-modal.service';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { DialogService } from '@shared/services/dialog.service';
import { PensaoModal_v1_2Component } from './pensao-modal_v1_2/pensao-modal_v1_2.component';
import { ErrorLib } from '@esocial/libs/error-lib';

@Component({
  selector: 'app-pensao-alimenticia-modal_v1_2',
  templateUrl: './pensao-alimenticia-modal_v1_2.component.html',
  styleUrls: ['./pensao-alimenticia-modal_v1_2.component.scss']
})
export class PensaoAlimenticiaModal_v1_2Component implements AfterViewInit {

  breadcrumb: string;
  constructor(
    private service: PensaoDepJudModalService, 
    private dialog: DialogService) { }

  @Input() formularioId: number;
  @Input() codigoDedSusp: number;
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
      this.service.obterListaPensoes(this.codigoDedSusp, view.pageIndex).then(x => {
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

  async incluir(): Promise<void> {
    this.aoAbrirPopup.emit();
    const teveAlteracao: boolean = await PensaoModal_v1_2Component.exibeModalIncluir(this.formularioId, this.codigoDedSusp);

    if (teveAlteracao) {
      this.buscarTabela();
    }
  }

  async alterar(codigoBenefPen: number): Promise<void> {
    this.aoAbrirPopup.emit();
    const teveAlteracao: boolean = await PensaoModal_v1_2Component.exibeModalAlterar(this.formularioId, this.codigoDedSusp, codigoBenefPen, true);

    if (teveAlteracao) {
      this.buscarTabela();
    }
  }

  async consultar(codigoBenefPen: number): Promise<void> {
    await PensaoModal_v1_2Component.exibeModalAlterar(this.formularioId, this.codigoDedSusp, codigoBenefPen, false);
  }

  async excluir(codigoBenefPen: number): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Dependentes/Beneficiários da Pensão Alimentícia',
      `Deseja excluir o Dependentes/Beneficiários da Pensão Alimentícia<br><b>${codigoBenefPen}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(this.formularioId, this.codigoDedSusp, codigoBenefPen);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Dependentes/Beneficiários da Pensão Alimentícia excluído!'
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
