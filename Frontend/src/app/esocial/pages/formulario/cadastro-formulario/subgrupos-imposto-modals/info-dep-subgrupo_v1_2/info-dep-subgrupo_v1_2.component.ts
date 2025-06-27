import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { DialogService } from '@shared/services/dialog.service';
import { InfoDepService } from '@esocial/services/formulario/subgrupos/info-dep.service';
import { InfoDepModal_v1_2Component } from './info-dep-modal_v1_2/info-dep-modal_v1_2.component';
import { ErrorLib } from '@esocial/libs/error-lib';
import { InfoDep } from '@esocial/models/subgrupos/v1_2/info-dep';

@Component({
  selector: 'app-info-dep-subgrupo-v1-2',
  templateUrl: './info-dep-subgrupo_v1_2.component.html',
  styleUrls: ['./info-dep-subgrupo_v1_2.component.scss']
})
export class InfoDepSubgrupo_v1_2Component implements AfterViewInit {

  breadcrumb: string;
  constructor(
    private service: InfoDepService, 
    private dialog: DialogService) { }

  @Input() formularioId: number;
  @Input() temPermissaoBlocoG: boolean;

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
    if (this.formularioId > 0) {
      let view = this.iniciaValoresDaView();
      try {
        const data = await this.service.obterPaginado(
          this.formularioId,
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
    const teveAlteracao: boolean = await InfoDepModal_v1_2Component.exibeModalIncluir(this.formularioId, this.temPermissaoBlocoG);

    if (teveAlteracao) {
      this.buscarTabela();
    }
  }


  async alterar(idInfoDep: number): Promise<void> {
    this.aoAbrirPopup.emit();
    const teveAlteracao: boolean = await InfoDepModal_v1_2Component.exibeModalAlterar(this.formularioId, idInfoDep, this.temPermissaoBlocoG);

    if (teveAlteracao) {
      this.buscarTabela();
    }
  }

  async consultar(idInfoDep: number): Promise<void> {
    await InfoDepModal_v1_2Component.exibeModalConsultar(this.formularioId, idInfoDep);
  }

  async excluir(item: InfoDep): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Dependente',
      `Deseja excluir as informações do dependentes cpf:<br><b>${item.infodepCpfdep}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(this.formularioId, item.idEsF2501Infodep);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Dependente excluído!'
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

}
