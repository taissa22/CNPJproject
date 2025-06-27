import {
  AfterViewInit,
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  ViewChild
} from '@angular/core';
import { FormControl } from '@angular/forms';
import { ErrorLib } from '@esocial/libs/error-lib';
import { Unicidade } from '@esocial/models/subgrupos/unicidade';
import { UnicidadeService } from '@esocial/services/formulario_v1_1/subgrupos/unicidade.service';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { DialogService } from '@shared/services/dialog.service';
import { UnicidadeModalComponent } from '../../subgrupos-modals/unicidade/unicidade-modal.component';

@Component({
  selector: 'app-esocial-unicidade-subgrupo',
  templateUrl: './esocial-unicidade-subgrupo.component.html',
  styleUrls: ['./esocial-unicidade-subgrupo.component.css']
})
export class EsocialUnicidadeSubgrupoComponent implements AfterViewInit, OnInit {
  breadcrumb: string;
  constructor(
    private service: UnicidadeService,
    private dialog: DialogService
  ) {}

  @Input() formularioId: number;
  @Input() contratoId: number;
  @Input() temPermissaoEsocialBlocoABCDEFHI: boolean;

  @Output() aoAbrirPopup = new EventEmitter();

  total: number = 0;
  dataSource: Array<any> = [];

  buscarDescricaoFormControl: FormControl = new FormControl(null);
  codigoFormularioFormControl: FormControl = new FormControl(null);

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  ngOnInit(): void {
    this.codigoFormularioFormControl.setValue(this.contratoId);
    this.buscarTabela();
  }
  async ngAfterViewInit(): Promise<void> {}

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
    if ( this.contratoId > 0) {
      let view = this.iniciaValoresDaView();
      try {
        const data = await this.service.obterPaginado(
          this.contratoId,
          view.pageIndex,
          view.sortColumn,
          view.sortDirection == 'asc'
        );
        if (data) {
          this.total = data.total;
          this.dataSource = data.lista.map(unicidade => {
            let dataString = unicidade.uniccontrDtinicio ? unicidade.uniccontrDtinicio.split('T')[0] : null ;
            let dataFinal = dataString ? dataString.split('-').reverse().join('/') : null
            return ({
              idF2500 : unicidade.idF2500,
              logDataOperacao : unicidade.logDataOperacao,
              logCodUsuario: unicidade.logCodUsuario,
              uniccontrMatunic : unicidade.uniccontrMatunic,
              uniccontrCodcateg : unicidade.uniccontrCodcateg,
              uniccontrDtinicio : unicidade.uniccontrDtinicio,
              uniccontrDtinicioFormatada : dataFinal,
              idEsF2500Uniccontr : unicidade.idEsF2500Uniccontr,
              uniccontrDesccateg: unicidade.uniccontrDesccateg,
            })
          });
        }

      } catch (error) {
        const mensagem = ErrorLib.ConverteMensagemErro(error);
        this.dialog.err(
          'Informações não carregadas',
          mensagem
        );
        this.dataSource = []; // ou atribua o valor desejado em caso de erro
      }
    }
  }

  async incluir(): Promise<void> {
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean =
      await UnicidadeModalComponent.exibirModalIncluir(this.formularioId, this.contratoId);

    if (teveAlteracao) {
      this.buscarDescricaoFormControl.setValue('');
      this.buscarTabela();
    }
  }

  async alterar(item: Unicidade): Promise<void> {
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean =
      await UnicidadeModalComponent.exibirModalAlterar(this.formularioId, this.contratoId, item, this.temPermissaoEsocialBlocoABCDEFHI );

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

  async excluir(item: Unicidade): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Unicidade',
      `Deseja excluir a unicidade<br><b>${item.uniccontrDesccateg == null ? item.uniccontrMatunic : item.uniccontrDesccateg.substring(0,100)}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(this.formularioId, this.contratoId,item.idEsF2500Uniccontr);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Unicidade excluída!'
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
