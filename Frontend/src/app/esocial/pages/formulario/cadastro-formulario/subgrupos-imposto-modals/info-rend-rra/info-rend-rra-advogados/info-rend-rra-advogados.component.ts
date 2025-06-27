import { AfterViewInit, Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { ErrorLib } from '@esocial/libs/error-lib';
import { ideAdvogado } from '@esocial/models/subgrupos/v1_2/ideAdvogado';
import { InfoRendRraAdvogadosService } from '@esocial/services/formulario/subgrupos/imposto-subgrupos-services/info-rend-rra-advogados.service';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { DialogService } from '@shared/services/dialog.service';
import { InfoRendRraAdvModalComponent } from './info-rend-rra-adv-modal/info-rend-rra-adv-modal.component';

@Component({
  selector: 'app-info-rend-rra-advogados',
  templateUrl: './info-rend-rra-advogados.component.html',
  styleUrls: ['./info-rend-rra-advogados.component.scss']
})
export class InfoRendRraAdvogadosComponent implements AfterViewInit {

  @Input() codF2501: number;
  @Input() codIrrf: number;
  @Input() temPermissaoBlocoCeDeE: boolean;

  @Output() carregado = new EventEmitter();
  @Output() aoAbrirPopup = new EventEmitter();

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  total: number = 0;
  dataSource: Array<ideAdvogado>;
  temDados: boolean = false;
  impostoSelecionado = -1;

  constructor(
    private service: InfoRendRraAdvogadosService,
    private dialog: DialogService
  ) { }

  async ngAfterViewInit() {
    await this.buscarTabela();
  }

  async buscarTabela() {
    let view = this.iniciaValoresDaView();
    try {
      this.service.obterListaAdvogados(this.codIrrf, view.pageIndex).then(x => {
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
    // this.emitAoAbrirPopup();
    // const teveAlteracao: boolean = await CarregarBloco_v1_2_Component.exibeModal(this.codF2501, "info-cr-irrf");

    // if (teveAlteracao) {
    //   await this.buscarTabela();
    // }
  }

  async incluir(): Promise<void> {
    const teveAlteracao: boolean = await InfoRendRraAdvModalComponent.exibeModalIncluir(this.codF2501, this.codIrrf);

    if (teveAlteracao) {
      await this.buscarTabela();
      this.carregado.emit(true);;
    }
  }

  async alterar(codigoIdeAdv: number): Promise<void> {
    const teveAlteracao: boolean = await InfoRendRraAdvModalComponent.exibeModalAlterar(this.codF2501, this.codIrrf, codigoIdeAdv, this.temPermissaoBlocoCeDeE);
    if (teveAlteracao) {
      await this.buscarTabela();
      this.carregado.emit(true);
    }
  }

  async excluir(codigoIdeAdv: number, cpf: string): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Advogado',
      `Deseja excluir o Advogado<br><b>${cpf}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(this.codF2501, this.codIrrf, codigoIdeAdv);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Advogado excluído!'
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
      pageSize: 10
    };
  }

}
