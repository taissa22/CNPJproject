import {
  AfterViewInit,
  Component,
  EventEmitter,
  Input,
  Output,
  ViewChild
} from '@angular/core';
import { FormControl } from '@angular/forms';
import { HttpErrorResult } from '@core/http';
import { ErrorLib } from '@esocial/libs/error-lib';
import { MudancaCategoria } from '@esocial/models/subgrupos/v1_2/mudancaCategoria';
import { MudancaCategoriaService } from '@esocial/services/formulario/subgrupos/mudanca-categoria.service';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { DialogService } from '@shared/services/dialog.service';
import { CategoriaModal_v1_2_Component } from '../../subgrupos-modals/categoria/categoria-modal_v1_2.component';

@Component({
  selector: 'app-esocial-categoria-subgrupo-v1-2',
  templateUrl: './esocial-categoria-subgrupo_v1_2.component.html',
  styleUrls: ['./esocial-categoria-subgrupo_v1_2.component.css']
})
export class EsocialCategoriaSubgrupo_v1_2_Component implements AfterViewInit {
  constructor(
    private service: MudancaCategoriaService,
    private dialog: DialogService
  ) {}

  @Input() formularioId: number;
  @Input() contratoId: number;
  @Input() temPermissaoEsocialBlocoABCDEFHI: boolean;

  @Output() aoAbrirPopup = new EventEmitter();

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  total: number = 0;
  dataSource: Array<any> = [];

  buscarDescricaoFormControl: FormControl = new FormControl(null);
  codigoFormularioFormControl: FormControl = new FormControl(null);

  async ngAfterViewInit(): Promise<void> {
    this.codigoFormularioFormControl.setValue(this.contratoId);
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
    try {
      if (this.codigoFormularioFormControl.value > 0) {
        let view = this.iniciaValoresDaView();
        const resposta = await this.service
        .obterPaginado(
          this.contratoId,
          view.pageIndex,
          view.sortColumn,
          view.sortDirection == 'asc'
        );
        if (resposta) {
          this.total = resposta.total;
          this.dataSource = resposta.lista.map(item => {
            let data = item.mudcategativDtmudcategativ ? item.mudcategativDtmudcategativ.split('T')[0] : null;
            let dataFinal = data ? data.split('-').reverse().join('/') : null;
            return ({
              idF2500 : item.idF2500,
              logDataOperacao : item.logDataOperacao,
              logCodUsuario : item.logCodUsuario,
              mudcategativCodcateg : item.mudcategativCodcateg,
              mudcategativNatatividade : item.mudcategativNatatividade,
              mudcategativDtmudcategativ : item.mudcategativDtmudcategativ,
              mudcategativDtmudcategativFormatada: dataFinal,
              idEsF2500Mudcategativ : item.idEsF2500Mudcategativ,
              descricaoNaturezaDeAtividade : item.descricaoNaturezaDeAtividade,
              descricaoCodCategoria: item.descricaoCodCategoria
            })
          });
        }
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      this.dialog.err(
        'Informações não carregadas',
        mensagem
      );
    }
  }

  async incluir(): Promise<void> {
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean =
      await CategoriaModal_v1_2_Component.exibeModalIncluir(this.formularioId, this.contratoId);

    if (teveAlteracao) {
      this.buscarDescricaoFormControl.setValue('');
      this.buscarTabela();
    }
  }

  async alterar(item: MudancaCategoria): Promise<void> {
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean =
      await CategoriaModal_v1_2_Component.exibeModalAlterar(this.formularioId,this.contratoId,item, this.temPermissaoEsocialBlocoABCDEFHI);

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

  async excluir(item: MudancaCategoria): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Categoria',
      `Deseja excluir a Categoria<br><b>${item.descricaoCodCategoria}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(this.formularioId,this.contratoId,item.idEsF2500Mudcategativ);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Categoria excluída!'
      );
      this.buscarDescricaoFormControl.setValue('');
      this.buscarTabela();
    } catch (error) {
      if ((error as HttpErrorResult).messages.join().includes('registrado')) {
        await this.dialog.info(
          `Exclusão não permitida`,
          (error as HttpErrorResult).messages.join('\n')
        );
        return;
      }
      await this.dialog.err(
        `Exclusão não realizada`,
        (error as HttpErrorResult).messages.join('\n')
      );
    }
  }

  onClearInputPesquisar() {
    if (!this.buscarDescricaoFormControl.value) {
      this.buscarTabela();
    }
  }
}
