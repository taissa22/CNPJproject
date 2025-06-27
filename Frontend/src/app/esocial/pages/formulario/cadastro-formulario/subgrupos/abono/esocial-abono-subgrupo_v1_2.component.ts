import { AfterViewInit, Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { DialogService } from '@shared/services/dialog.service';
import { AbonoModal_v1_2Component } from '../../subgrupos-modals/abono-modal_v1_2/abono-modal_v1_2.component';
import { AbonoService } from '@esocial/services/formulario/subgrupos/abono.service';
import { ErrorLib } from '@esocial/libs/error-lib';
import { Abono } from '@esocial/models/subgrupos/v1_2/abono';

@Component({
  selector: 'app-esocial-abono-subgrupo_v1_2',
  templateUrl: './esocial-abono-subgrupo_v1_2.component.html',
  styleUrls: ['./esocial-abono-subgrupo_v1_2.component.scss']
})
export class EsocialAbonoSubgrupo_v1_2Component implements AfterViewInit {
  breadcrumb: string;
  constructor(
    private service: AbonoService, 
    private dialog: DialogService) { }

  @Input() formularioId: number;
  @Input() contratoId: number;
  @Input() temPermissaoEsocialBlocoJValores: boolean;

  @Output() aoAbrirPopup = new EventEmitter();

  total: number = 0;
  dataSource: Array<any>;
  temDados: boolean = false;

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
        const data = await this.service.obterPaginado(
          this.contratoId,
          view.pageIndex,
          view.sortColumn,
          view.sortDirection == 'asc'
        );

        this.total = data.total;
        this.temDados = this.total > 0;
        this.dataSource = data.lista;

      } catch (error) {
        console.log(error)
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
    const teveAlteracao: boolean = await AbonoModal_v1_2Component.exibeModalIncluir(this.formularioId, this.contratoId);

    if (teveAlteracao) {
      this.buscarTabela();
    }
  }

async excluir(item: Abono){
  const excluir: boolean = await this.dialog.confirm(
    'Excluir ',
    `Deseja excluir o Abono<br><br><b>${item.abonoAnobase}</b>?`
  );

  if (!excluir) {
    return;
  }

  try {
    await this.service.excluir(this.formularioId, item.idEsF2500Infocontrato,item.idEsF2500Abono);
    await this.dialog.alert(
      'Exclusão realizada com sucesso',
      'Abono excluído!'
    );
    await this.buscarTabela();
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

  async alterar(item: Abono): Promise<void> {
    this.aoAbrirPopup.emit();
    const teveAlteracao: boolean = await AbonoModal_v1_2Component.exibeModalAlterar(this.formularioId, this.contratoId, item.idEsF2500Abono, item.abonoAnobase, true);

    if (teveAlteracao) {
      this.buscarTabela();
    }
  }

  async consultar(item: Abono): Promise<void> {
    await AbonoModal_v1_2Component.exibeModalConsultar(this.contratoId, item.idEsF2500Abono, false);
  }
  

}
