import { AfterViewInit, Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { ErrorLib } from '@esocial/libs/error-lib';
import { Imposto } from '@esocial/models/subgrupos/imposto';
import { ImpostoService } from '@esocial/services/formulario_v1_1/subgrupos/imposto.service';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { DialogService } from '@shared/services/dialog.service';
import { CarregarBlocoComponent } from '../../subgrupos-modals/carregar-bloco/carregar-bloco.component';
import { ImpostoModalComponent } from '../../subgrupos-modals/imposto/imposto-modal.component';

@Component({
  selector: 'app-esocial-imposto-subgrupo',
  templateUrl: './esocial-imposto-subgrupo.component.html',
  styleUrls: ['./esocial-imposto-subgrupo.component.scss']
})
export class EsocialImpostoSubgrupoComponent implements AfterViewInit {

  @Input() codF2501: number;
  @Input() temPermissaoBlocoCeDeE: boolean;

  @Output() carregado = new EventEmitter();
  @Output() aoAbrirPopup = new EventEmitter();

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  total: number = 0;
  dataSource: Array<Imposto>;
  temDados: boolean = false;

  constructor(
    private service: ImpostoService,
    private dialog: DialogService
  ) { }

  async ngAfterViewInit() {
    await this.buscarTabela();
    this.carregado.emit(true);;
  }

  async buscarTabela() {
    let view = this.iniciaValoresDaView();
    try {
      const data = await this.service
        .obterPaginado(
          this.codF2501,
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
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean = await CarregarBlocoComponent.exibeModal(this.codF2501, "info-cr-irrf");

    if (teveAlteracao) {
      await this.buscarTabela();
    }
  }

  async incluir(): Promise<void> {
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean = await ImpostoModalComponent.exibeModalIncluir(this.codF2501);

    if (teveAlteracao) {
      await this.buscarTabela();
      this.carregado.emit(true);;
    }
  }

  async alterar(codIrrf: number): Promise<void> {
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean = await ImpostoModalComponent.exibeModalAlterar(this.codF2501, codIrrf, this.temPermissaoBlocoCeDeE);
    if (teveAlteracao) {
      await this.buscarTabela();
      this.carregado.emit(true);
    }
  }

  private emitAoAbrirPopup() {
    if (this.temPermissaoBlocoCeDeE) {
      this.aoAbrirPopup.emit();
    }
  }

  async excluir(codIrrf: number, desc: string): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir IRRF',
      `Deseja excluir o IRRF<br><b>${desc.substring(0, 100)}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(this.codF2501, codIrrf);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'IRRF excluído!'
      );
      await this.buscarTabela();
      this.carregado.emit(true);;
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

  async exportar() {
    try {
      let view = this.iniciaValoresDaView();
      return await this.service.exportarListaAsync(this.codF2501, view.sortDirection == 'asc');
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      return await this.dialog.err("Erro ao exportar lista", mensagem);
    }
  }
}
