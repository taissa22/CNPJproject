import { AfterViewInit, Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { ErrorLib } from '@esocial/libs/error-lib';
import { PeriodoBase } from '@esocial/models/subgrupos/periodo-base';
import { PeriodoBaseService } from '@esocial/services/formulario_v1_1/subgrupos/periodo-base.service';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { DialogService } from '@shared/services/dialog.service';
import { CarregarBlocoComponent } from '../../subgrupos-modals/carregar-bloco/carregar-bloco.component';
import { PeriodoBaseModalComponent } from '../../subgrupos-modals/periodo-base-modal/periodo-base-modal.component';

@Component({
  selector: 'app-esocial-periodo-base-subgrupo',
  templateUrl: './esocial-periodo-base-subgrupo.component.html',
  styleUrls: ['./esocial-periodo-base-subgrupo.component.scss']
})
export class EsocialPeriodoBaseSubgrupoComponent implements AfterViewInit {

  @Input() formularioId: number;
  @Input() temPermissaoBlocoCeDeE: boolean;

  @Output() carregado = new EventEmitter();
  @Output() aoAbrirPopup = new EventEmitter();

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  total: number = 0;
  dataSource: Array<PeriodoBase>;
  periodoSelecionado = -1;
  temDados: boolean = false;

  constructor(
    private service: PeriodoBaseService,
    private dialog: DialogService
  ) { }

  async ngAfterViewInit() {
    await this.buscarTabela();
    this.carregado.emit(true);
  }

  async buscarTabela() {
    try {
      let view = this.iniciaValoresDaView();
      const resposta = await this.service.obterPaginado(
          this.formularioId,
          view.pageIndex,
          view.sortColumn,
          view.sortDirection == 'asc'
        );

      if (resposta) {
        this.total = resposta.total;
        this.temDados = this.total > 0;
        this.dataSource = resposta.lista;
      } else {
        await this.dialog.err(
          'Informações não carregadas',
          'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte'
        );
      }
    } catch(error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialog.err(
        'Informações não carregadas',
        mensagem
      );
    }
  }

  async carregarBlocoPeriodo(): Promise<void> {
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean = await CarregarBlocoComponent.exibeModal(this.formularioId, "periodo-base");

    if (teveAlteracao) {
      await this.buscarTabela();
    }
  }

  async carregarBlocoContribuicao(): Promise<void> {
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean = await CarregarBlocoComponent.exibeModal(this.formularioId, "contribuicao");

    if (teveAlteracao) {
      await this.buscarTabela();
    }
  }

  async incluir(): Promise<void> {
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean = await PeriodoBaseModalComponent.exibeModalIncluir(this.formularioId);

    if (teveAlteracao) {
      await this.buscarTabela();
      this.carregado.emit(true);
    }
  }

  async alterar(codF2501: number, codCalTrib: number): Promise<void> {
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean = await PeriodoBaseModalComponent.exibeModalAlterar(codF2501, codCalTrib, this.temPermissaoBlocoCeDeE);
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

  async excluir(codF2501: number, codCalTrib: number, PerRef: Date): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Período',
      `Deseja excluir o Período<br><br><b>${PerRef}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(codF2501, codCalTrib);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Período excluído!'
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

  selecionarPeriodo(index: number): void {
    if (this.periodoSelecionado == index) {
      this.periodoSelecionado = -1;
      return;
    }
    this.periodoSelecionado = index;
  }

  async exportar() {
    try {
      let view = this.iniciaValoresDaView();
      return await this.service.exportarListaAsync(this.formularioId, view.sortDirection == 'asc');
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      return await this.dialog.err("Erro ao exportar lista", mensagem);
    }
  }

  async exportarContribuicoes() {
    try {
      let view = this.iniciaValoresDaView();
      return await this.service.exportarContribListaAsync(this.formularioId, view.sortDirection == 'asc');
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      return await this.dialog.err("Erro ao exportar lista", mensagem);
    }
  }
}
