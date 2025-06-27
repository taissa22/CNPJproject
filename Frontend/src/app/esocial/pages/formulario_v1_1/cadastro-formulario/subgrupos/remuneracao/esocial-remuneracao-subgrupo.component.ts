import { DatePipe } from '@angular/common';
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
import { Remuneracao } from '@esocial/models/subgrupos/remuneracao';
import { RemuneracaoService } from '@esocial/services/formulario_v1_1/subgrupos/remuneracao.service';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { DialogService } from '@shared/services/dialog.service';
import { CarregarBlocoComponent } from '../../subgrupos-modals/carregar-bloco/carregar-bloco.component';
import { RemuneracaoModalComponent } from '../../subgrupos-modals/remuneracao/remuneracao-modal.component';

@Component({
  selector: 'app-esocial-remuneracao-subgrupo',
  templateUrl: './esocial-remuneracao-subgrupo.component.html',
  styleUrls: ['./esocial-remuneracao-subgrupo.component.css']
})
export class EsocialRemuneracaoSubgrupoComponent implements AfterViewInit {
  breadcrumb: string;
  constructor(
    private service: RemuneracaoService,
    private dialog: DialogService,
    public datepipe: DatePipe,
  ) { }

  @Input() formularioId: number;
  @Input() contratoId: number;
  @Input() dtMin: Date;
  @Input() dtMax: Date;
  @Input() temPermissaoEsocialBlocoGK: boolean;

  @Output() aoAbrirPopup = new EventEmitter();

  total: number = 0;
  dataSource: Array<any> = [];
  temDados: boolean = false;

  buscarDescricaoFormControl: FormControl = new FormControl(null);
  codigoFormularioFormControl: FormControl = new FormControl(null);

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  async ngAfterViewInit(): Promise<void> {
    this.codigoFormularioFormControl.setValue(this.contratoId);
    await this.buscarTabela();
  }

  async buscarTabela() {
    if (this.contratoId > 0) {
      let view = this.iniciaValoresDaView();
      try {
        const resposta = await this.service.obterPaginado(this.contratoId, view.pageIndex, view.sortColumn, view.sortDirection == 'asc')
        if (resposta) {
          this.total = resposta.total;
          this.temDados = this.total > 0;
          this.dataSource = resposta.lista.map(item => {
            let dataString = item.remuneracaoDtremun ? item.remuneracaoDtremun.split('T')[0] : null;
            let dataFinal = dataString ? dataString.split('-').reverse().join('/') : null
            let valor = item.remuneracaoVrsalfx.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL', currencyDisplay: 'symbol', minimumFractionDigits: 2 });
            return ({
              idF2500: item.idF2500,
              remuneracaoDtremun: item.remuneracaoDtremun,
              remuneracaoDtremunFormatada: dataFinal,
              remuneracaoVrsalfx: item.remuneracaoVrsalfx,
              remuneracaoVrsalfxFormatado: valor,
              remuneracaoUndsalfixo: item.remuneracaoUndsalfixo,
              remuneracaoDscsalvar: item.remuneracaoDscsalvar,
              logDataOperacao: item.logDataOperacao,
              logCodUsuario: item.logCodUsuario,
              idEsF2500Remuneracao: item.idEsF2500Remuneracao,
              descricaoUnidadePagamento: item.descricaoUnidadePagamento,
            })
          });
        }
      } catch (error) {
        console.error(error)
        const mensagem = ErrorLib.ConverteMensagemErro(error);
        await this.dialog.err(
          'Informações não carregadas',
          mensagem
        );

      }
    }
  }

  iniciaValoresDaView() {
    return {
      sortColumn:
        this.table === undefined || !this.table.sortColumn
          ? 'Nome'
          : this.table.sortColumn,
      sortDirection:
        this.table === undefined || !this.table.sortDirection
          ? 'asc'
          : this.table.sortDirection,
      pageIndex:
        this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize
    };
  }

  async incluir(): Promise<void> {
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean =
      await RemuneracaoModalComponent.exibeModalIncluir(this.formularioId, this.contratoId, this.dtMin, this.dtMax);

    if (teveAlteracao) {
      await this.buscarTabela();
      this.buscarDescricaoFormControl.setValue('');
    }
  }

  async alterar(item: Remuneracao): Promise<void> {
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean =
      await RemuneracaoModalComponent.exibeModalAlterar(this.formularioId, this.contratoId, item, this.dtMin, this.dtMax, this.temPermissaoEsocialBlocoGK);

    if (teveAlteracao) {
      this.buscarDescricaoFormControl.setValue('');
      this.buscarTabela();
    }
  }

  private emitAoAbrirPopup() {
    if (this.temPermissaoEsocialBlocoGK) {
      this.aoAbrirPopup.emit();
    }
  }

  async excluir(item: any): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Remuneração',
      `Deseja excluir a Remuneração<br><b>${item.remuneracaoDtremunFormatada}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(this.formularioId, this.contratoId, item.idEsF2500Remuneracao);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Remuneração excluída!'
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

  async carregarBlocoPeriodo(): Promise<void> {
    this.aoAbrirPopup.emit();
    const teveAlteracao: boolean = await CarregarBlocoComponent.exibeModal(this.formularioId, "remuneracao", this.contratoId);

    if (teveAlteracao) {
      await this.buscarTabela();
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
