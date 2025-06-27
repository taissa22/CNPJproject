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
import { Observacao } from '@esocial/models/subgrupos/v1_2/observacao';
import { ObservacaoService } from '@esocial/services/formulario/subgrupos/observacao.service';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { DialogService } from '@shared/services/dialog.service';
import { ObservacaoModal_v1_2_Component } from '../../subgrupos-modals/observacao/observacao-modal_v1_2.component';

@Component({
  selector: 'app-esocial-observacao-subgrupo-v1-2',
  templateUrl: './esocial-observacao-subgrupo_v1_2.component.html',
  styleUrls: ['./esocial-observacao-subgrupo_v1_2.component.css']
})
export class EsocialObservacaoSubgrupo_v1_2_Component implements AfterViewInit {
  breadcrumb: string;
  constructor(
    private service: ObservacaoService,
    private dialog: DialogService
  ) {}

  @Input() contratoId: number;
  @Input() formularioId : number;
  @Input() temPermissaoEsocialBlocoABCDEFHI: boolean;

  @Output() aoAbrirPopup = new EventEmitter();

  total: number = 0;
  dataSource: Array<Observacao> = [];

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
        const data = await this.service
          .obterPaginado(
            this.contratoId,
            view.pageIndex,
            view.sortColumn,
            view.sortDirection == 'asc'
          );
        this.total = data.total;
        this.dataSource = data.lista;
      } catch (error) {
        const mensagem = ErrorLib.ConverteMensagemErro(error);
        this.dialog.err(
          'Informações não carregadas',
          mensagem
        );
      }
    }
  }

  async incluir(): Promise<void> {
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean =
      await ObservacaoModal_v1_2_Component.exibeModalIncluir(this.formularioId, this.contratoId);

    if (teveAlteracao) {
      this.buscarDescricaoFormControl.setValue('');
      this.buscarTabela();
    }
  }

  async alterar(item: Observacao): Promise<void> {
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean =
      await ObservacaoModal_v1_2_Component.exibeModalAlterar(item, this.contratoId, this.formularioId, this.temPermissaoEsocialBlocoABCDEFHI);

    if (teveAlteracao) {
      this.buscarDescricaoFormControl.setValue('');
      this.buscarTabela();
    }
  }

  private emitAoAbrirPopup() {
    if (this.temPermissaoEsocialBlocoABCDEFHI) {
      this.aoAbrirPopup.emit();
    }
  }

  async excluir(item: Observacao): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Observação',
      `Deseja excluir a Observação<br><b>${item.observacoesObservacao.substring(0, 100)}${item.observacoesObservacao.length > 100 ? '...' : ''}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(this.formularioId, this.contratoId, item.idEsF2500Observacoes);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Observação excluída!'
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
}
