import { AfterViewInit, Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { ErrorLib } from '@esocial/libs/error-lib';
import { Imposto } from '@esocial/models/subgrupos/v1_2/imposto';
import { ImpostoService } from '@esocial/services/formulario/subgrupos/imposto.service';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { DialogService } from '@shared/services/dialog.service';
import { CarregarBloco_v1_2_Component } from '../../subgrupos-modals/carregar-bloco/carregar-bloco_v1_2.component';
import { ImpostoModal_v1_2_Component } from '../../subgrupos-modals/imposto/imposto-modal_v1_2.component';
import { InfoDeducoesIsencoesModal_v1_2Component } from '../../subgrupos-imposto-modals/info-deducoes-isencoes-modal_v1_2/info-deducoes-isencoes-modal_v1_2.component';
import { EsocialDedDepenModal_v1_2_Component } from '../../subgrupos-imposto-modals/info-deducoes-dependente-modal/esocial-deducoes-dependente-modal.component';
import { EsocialPenAlimModal_v1_2_Component } from '../../subgrupos-imposto-modals/info-deducoes-pen-alim-modal/esocial-deducoes-pen-alim-modal.component';
import { ProcDepositosJudiciaisModal_v1_2Component } from '../../subgrupos-imposto-modals/proc-depositos-judiciais-modal_v1_2/proc-depositos-judiciais-modal_v1_2.component';
import { InfoRendRraComponent } from '../../subgrupos-imposto-modals/info-rend-rra/info-rend-rra.component';

@Component({
  selector: 'app-esocial-imposto-subgrupo-v1-2',
  templateUrl: './esocial-imposto-subgrupo_v1_2.component.html',
  styleUrls: ['./esocial-imposto-subgrupo_v1_2.component.scss']
})
export class EsocialImpostoSubgrupo_v1_2_Component implements AfterViewInit {

  @Input() codF2501: number;
  @Input() temPermissaoBlocoCeDeE: boolean;

  @Output() carregado = new EventEmitter();
  @Output() aoAbrirPopup = new EventEmitter();

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  total: number = 0;
  dataSource: Array<Imposto>;
  temDados: boolean = false;
  impostoSelecionado = -1;

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
    const teveAlteracao: boolean = await CarregarBloco_v1_2_Component.exibeModal(this.codF2501, "info-cr-irrf");

    if (teveAlteracao) {
      await this.buscarTabela();
    }
  }

  async incluir(): Promise<void> {
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean = await ImpostoModal_v1_2_Component.exibeModalIncluir(this.codF2501);

    if (teveAlteracao) {
      await this.buscarTabela();
      await this.expandirImposto();
      this.carregado.emit(true);;
    }
  }

  async alterar(codIrrf: number): Promise<void> {
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean = await ImpostoModal_v1_2_Component.exibeModalAlterar(this.codF2501, codIrrf, this.temPermissaoBlocoCeDeE);
    if (teveAlteracao) {
      await this.buscarTabela();
      this.carregado.emit(true);
    }
  }

  private emitAoAbrirPopup() {   
      this.aoAbrirPopup.emit();   
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

  selecionarImposto(index: number): void {
    if (this.impostoSelecionado == index) {
      this.impostoSelecionado = -1;
      return;
    }
    this.impostoSelecionado = index;
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

  async alterarInfoDeducoesIsencoes(codIrrf: number, codReceita: string, valorIrrf: number, valorIrrf13: number): Promise<void> {
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean = await InfoDeducoesIsencoesModal_v1_2Component.exibeModalAlterar(this.codF2501, codIrrf, codReceita, valorIrrf, valorIrrf13, this.temPermissaoBlocoCeDeE);
    if (teveAlteracao) {
      await this.buscarTabela();
      this.carregado.emit(true);
    }
  }

  async consultarInfoDeducoesIsencoes(codIrrf: number, codReceita: string, valorIrrf: number, valorIrrf13: number): Promise<void> {
    const teveAlteracao: boolean = await InfoDeducoesIsencoesModal_v1_2Component.exibeModalAlterar(this.codF2501, codIrrf, codReceita, valorIrrf, valorIrrf13, false);
    if (teveAlteracao) {
      await this.buscarTabela();
      this.carregado.emit(true);
    }
  }
  async alterarDepositosJuduciais(codIrrf: number, codReceita: string, valorIrrf: number, valorIrrf13: number): Promise<void> {
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean = await ProcDepositosJudiciaisModal_v1_2Component.exibeModalAlterar(this.codF2501, codIrrf, codReceita, valorIrrf, valorIrrf13, this.temPermissaoBlocoCeDeE);
    if (teveAlteracao) {
      await this.buscarTabela();
      this.carregado.emit(true);
    }
  }

  async consultarDepositosJuduciais(codIrrf: number, codReceita: string, valorIrrf: number, valorIrrf13: number): Promise<void> {
    const teveAlteracao: boolean = await ProcDepositosJudiciaisModal_v1_2Component.exibeModalAlterar(this.codF2501, codIrrf, codReceita, valorIrrf, valorIrrf13, this.temPermissaoBlocoCeDeE);
    if (teveAlteracao) {
      await this.buscarTabela();
      this.carregado.emit(true);
    }
  }

  async alterarDedDepen(codIrrf: number, codReceita: string, valorIrrf: number, valorIrrf13: number){
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean = await EsocialDedDepenModal_v1_2_Component.exibeModalAlterar(this.codF2501, codIrrf, codReceita, valorIrrf, valorIrrf13, this.temPermissaoBlocoCeDeE);
    if (teveAlteracao) {
      await this.buscarTabela();
      this.carregado.emit(true);
    }
  }

  async alterarInfoRendRRA(codIrrf: number, codReceita: string, valorIrrf: number, valorIrrf13: number): Promise<void> {
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean = await InfoRendRraComponent.exibeModalAlterar(this.codF2501, codIrrf, codReceita, valorIrrf, valorIrrf13, this.temPermissaoBlocoCeDeE);
    if (teveAlteracao) {
      await this.buscarTabela();
      this.carregado.emit(true);
    }
  }

  async consultarDedDepen( codIrrf: number, codReceita: string, valorIrrf: number, valorIrrf13: number){
    const teveAlteracao: boolean = await EsocialDedDepenModal_v1_2_Component.exibeModalAlterar(this.codF2501, codIrrf, codReceita, valorIrrf, valorIrrf13, false);
    if (teveAlteracao) {
      await this.buscarTabela();
      this.carregado.emit(true);
    }
  }

  async alterarPenAlim(codIrrf: number, codReceita: string, valorIrrf: number, valorIrrf13: number){
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean = await EsocialPenAlimModal_v1_2_Component.exibeModalAlterar(this.codF2501, codIrrf, codReceita, valorIrrf, valorIrrf13, this.temPermissaoBlocoCeDeE);
    if (teveAlteracao) {
      await this.buscarTabela();
      this.carregado.emit(true);
    }
  }

  async consultarPenAlim( codIrrf: number, codReceita: string, valorIrrf: number, valorIrrf13: number){
    const teveAlteracao: boolean = await EsocialPenAlimModal_v1_2_Component.exibeModalAlterar(this.codF2501, codIrrf, codReceita, valorIrrf, valorIrrf13, false);
    if (teveAlteracao) {
      await this.buscarTabela();
      this.carregado.emit(true);
    }
  }

  async consultarInfoRendRRA(codIrrf: number, codReceita: string, valorIrrf: number, valorIrrf13: number): Promise<void> {
    const teveAlteracao: boolean = await InfoRendRraComponent.exibeModalAlterar(this.codF2501, codIrrf, codReceita, valorIrrf, valorIrrf13, false);
    if (teveAlteracao) {
      await this.buscarTabela();
      this.carregado.emit(true);
    }
  }

  async expandirImposto(){
    let listId = this.dataSource.map(x => x.idEsF2501Infocrirrf).sort(function (a, b) { return b - a; });
    this.impostoSelecionado = listId[0];
  }
}
