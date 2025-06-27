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
import { Periodo } from '@esocial/models/subgrupos/periodo';
import { PeriodoService } from '@esocial/services/formulario_v1_1/subgrupos/periodo.service';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { DialogService } from '@shared/services/dialog.service';
import { CarregarBlocoComponent } from '../../subgrupos-modals/carregar-bloco/carregar-bloco.component';
import { PeriodoModalComponent } from '../../subgrupos-modals/periodo/periodo-modal.component';

@Component({
  selector: 'app-esocial-periodo-subgrupo',
  templateUrl: './esocial-periodo-subgrupo.component.html',
  styleUrls: ['./esocial-periodo-subgrupo.component.css']
})
export class EsocialPeriodoSubgrupoComponent implements AfterViewInit {
  breadcrumb: string;
  constructor(private service: PeriodoService, private dialog: DialogService) { }

  @Input() formularioId: number;
  @Input() contratoId: number;
  @Input() temPermissaoEsocialBlocoGK: boolean;

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
        this.dataSource = data.lista.map(item => {
          let vrbccpmensal = item.basecalculoVrbccpmensal != null ? item.basecalculoVrbccpmensal.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL', currencyDisplay: 'symbol', minimumFractionDigits: 2 }) : '';
          let vrbccp13 = item.basecalculoVrbccp13 != null ? item.basecalculoVrbccp13.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL', currencyDisplay: 'symbol', minimumFractionDigits: 2 }) : '';
          let vrbcfgts = item.basecalculoVrbcfgts != null ? item.basecalculoVrbcfgts.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL', currencyDisplay: 'symbol', minimumFractionDigits: 2 }) : '';
          let vrbcfgts13 = item.basecalculoVrbcfgts13 != null ? item.basecalculoVrbcfgts13.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL', currencyDisplay: 'symbol', minimumFractionDigits: 2 }) : '';
          let vrbcfgtsguia = item.infofgtsVrbcfgtsguia != null ? item.infofgtsVrbcfgtsguia.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL', currencyDisplay: 'symbol', minimumFractionDigits: 2 }) : '';
          let vrbcfgts13guia = item.infofgtsVrbcfgts13guia != null ? item.infofgtsVrbcfgts13guia.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL', currencyDisplay: 'symbol', minimumFractionDigits: 2 }) : '';
          let vrbccprev = item.basemudcategVrbccprev != null ? item.basemudcategVrbccprev.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL', currencyDisplay: 'symbol', minimumFractionDigits: 2 }) : '';



          return ({
            idF2500: item.idF2500,
            ideperiodoPerref: item.ideperiodoPerref.split('-').reverse().join('/'),
            logCodUsuario: item.logCodUsuario,
            logDataOperacao: item.logDataOperacao,
            basecalculoVrbccpmensal: item.basecalculoVrbccpmensal,
            basecalculoVrbccpmensalFormatado: vrbccpmensal,
            basecalculoVrbccp13: item.basecalculoVrbccp13,
            basecalculoVrbccp13Formatado: vrbccp13,
            basecalculoVrbcfgts: item.basecalculoVrbcfgts,
            basecalculoVrbcfgtsFormatado: vrbcfgts,
            basecalculoVrbcfgts13: item.basecalculoVrbcfgts13,
            basecalculoVrbcfgts13Formatado: vrbcfgts13,
            infoagnocivoGrauexp: item.infoagnocivoGrauexp,
            infofgtsVrbcfgtsguia: item.infofgtsVrbcfgtsguia,
            infofgtsVrbcfgtsguiaFormatado: vrbcfgtsguia,
            infofgtsVrbcfgts13guia: item.infofgtsVrbcfgts13guia,
            infofgtsVrbcfgts13guiaFormatado: vrbcfgts13guia,
            infofgtsPagdireto: item.infofgtsPagdireto,
            basemudcategCodcateg: item.basemudcategCodcateg,
            basemudcategVrbccprev: item.basemudcategVrbccprev,
            basemudcategVrbccprevFormatada: vrbccprev,
            idEsF2500Ideperiodo: item.idEsF2500Ideperiodo,
          })
        });
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
    const teveAlteracao: boolean = await CarregarBlocoComponent.exibeModal(this.formularioId, "periodo", this.contratoId);

    if (teveAlteracao) {
      this.buscarDescricaoFormControl.setValue('');
      await this.buscarTabela();
    }
  }

  async incluir(): Promise<void> {
    this.aoAbrirPopup.emit();
    const teveAlteracao: boolean = await PeriodoModalComponent.exibeModal(this.formularioId, this.contratoId, null);

    if (teveAlteracao) {
      this.buscarDescricaoFormControl.setValue('');
      this.buscarTabela();
    }
  }

  async alterar(item: Periodo): Promise<void> {
    this.aoAbrirPopup.emit();
    const teveAlteracao: boolean = await PeriodoModalComponent.exibeModal(this.formularioId, this.contratoId, item);

    if (teveAlteracao) {
      this.buscarDescricaoFormControl.setValue('');
      this.buscarTabela();
    }
  }

  async consultar(item: Periodo): Promise<void> {
    await PeriodoModalComponent.exibeModalConsultar(this.formularioId, this.contratoId, item);
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
