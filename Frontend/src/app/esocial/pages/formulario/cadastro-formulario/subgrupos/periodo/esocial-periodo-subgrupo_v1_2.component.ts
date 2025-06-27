import {
  AfterViewInit,
  Component,
  EventEmitter,
  Input,
  Output,
  ViewChild
} from '@angular/core';
import { FormControl } from '@angular/forms';
import { ErrorLib } from '@esocial/libs/error-lib';
import { Periodo } from '@esocial/models/subgrupos/v1_2/periodo';
import { PeriodoService } from '@esocial/services/formulario/subgrupos/periodo.service';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { DialogService } from '@shared/services/dialog.service';
import { CarregarBloco_v1_2_Component } from '../../subgrupos-modals/carregar-bloco/carregar-bloco_v1_2.component';
import { PeriodoModal_v1_2_Component } from '../../subgrupos-modals/periodo/periodo-modal_v1_2.component';

@Component({
  selector: 'app-esocial-periodo-subgrupo-v1-2',
  templateUrl: './esocial-periodo-subgrupo_v1_2.component.html',
  styleUrls: ['./esocial-periodo-subgrupo_v1_2.component.css']
})
export class EsocialPeriodoSubgrupo_v1_2_Component implements AfterViewInit {
  breadcrumb: string;
  constructor(private service: PeriodoService, private dialog: DialogService) { }

  @Input() formularioId: number;
  @Input() contratoId: number;
  @Input() temPermissaoEsocialBlocoGK: boolean;
  @Input() infocontrCodcateg: number;
  @Input() infocontrIndcontr: string;
  @Input() infovlrindreperc: number;

  @Output() aoAbrirPopup = new EventEmitter();

  total: number = 0;
  dataSource: Array<any>;
  temDados: boolean = false;

  buscarDescricaoFormControl: FormControl = new FormControl(null);

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
    if (this.contratoId > 0) {
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
        console.log(this.dataSource);         
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
    this.aoAbrirPopup.emit();
    const teveAlteracao: boolean = await CarregarBloco_v1_2_Component.exibeModal(this.formularioId, "periodo", this.contratoId);

    if (teveAlteracao) {
      this.buscarDescricaoFormControl.setValue('');
      await this.buscarTabela();
    }
  }

  async incluir(): Promise<void> {
    let repercProcesso = localStorage.getItem('repercProcesso');
    if (repercProcesso != '1' && repercProcesso != '5') {
      await this.dialog.alert('Inclusão','Se o campo Repercussão do Processo (Bloco J)  estiver preenchido com “2 - Decisão sem repercussão tributária ou FGTS”, “3 - Decisão com repercussão exclusiva para declaração de rendimentos para fins de Imposto de Renda com rendimentos informados em S-2501” ou “4 - Decisão com repercussão exclusiva para declaração de rendimentos para fins de Imposto de Renda com pagamento através de depósito judicial” os campos do Bloco K não deverão ser informados.');
      return;
    }

    this.aoAbrirPopup.emit();
    const teveAlteracao: boolean = await PeriodoModal_v1_2_Component.exibeModal(this.formularioId, this.contratoId, null, this.infocontrIndcontr, this.infocontrCodcateg, this.infovlrindreperc);

    if (teveAlteracao) {
      this.buscarDescricaoFormControl.setValue('');
      this.buscarTabela();
    }
  }

  async alterar(item: Periodo): Promise<void> {
    this.aoAbrirPopup.emit();
    const teveAlteracao: boolean = await PeriodoModal_v1_2_Component.exibeModal(this.formularioId, this.contratoId, item, this.infocontrIndcontr, this.infocontrCodcateg, this.infovlrindreperc);

    if (teveAlteracao) {
      this.buscarDescricaoFormControl.setValue('');
      this.buscarTabela();
    }
  }

  async consultar(item: Periodo): Promise<void> {
    await PeriodoModal_v1_2_Component.exibeModalConsultar(this.formularioId, this.contratoId, item, this.infocontrIndcontr, this.infocontrCodcateg);
  }

  async excluir(item: Periodo): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Período',
      `Deseja excluir o Período<br><b>${item.ideperiodoPerref}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(this.formularioId, this.contratoId, item.idEsF2500Ideperiodo);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Período excluído!'
      );
      this.buscarDescricaoFormControl.setValue('');
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

  onClearInputPesquisar() {
    if (!this.buscarDescricaoFormControl.value) {
      this.buscarTabela();
    }
  }

  async exportar() {
    try {
      let view = this.iniciaValoresDaView();
      return await this.service.exportarListaAsync(this.contratoId, view.sortDirection == 'asc');
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      return await this.dialog.err("Erro ao exportar lista", mensagem);
    }
  }
}
