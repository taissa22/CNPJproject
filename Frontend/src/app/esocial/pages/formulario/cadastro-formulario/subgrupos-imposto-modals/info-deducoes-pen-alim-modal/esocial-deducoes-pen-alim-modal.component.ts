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
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { DialogService } from '@shared/services/dialog.service';
import { StaticInjector } from '@esocial/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DedDepenModal_v1_2_Component } from '../info-deducoes-dependente-edit-modal/deducoes-dependente-edit-modal.component';
import { PenAlimService } from '@esocial/services/formulario/subgrupos/PenAlim.service';
import { PenAlim } from '@esocial/models/subgrupos/v1_2/pen-alim';
import { ESocialListaFormularioService } from '@esocial/services/aplicacao/e-social-lista-formulario.service';
import { TipoDependente } from '@esocial/models/tipo-dependente';
import { PenAlimModal_v1_2_Component } from '../info-deducoes-pen-alim-edit-modal/deducoes-pen-alim-edit-modal.component';


@Component({
  selector: 'app-esocial-deducoes-pen-alim-modal',
  templateUrl: './esocial-deducoes-pen-alim-modal.component.html',
  styleUrls: ['./esocial-deducoes-pen-alim-modal.component.scss']
})
export class EsocialPenAlimModal_v1_2_Component implements AfterViewInit {
  breadcrumb: string;
  constructor(
    private modal: NgbActiveModal,
    private service: PenAlimService,
    private serviceTpRendimento : ESocialListaFormularioService,
    private dialog: DialogService
  ) {}

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  @Input() temPermissaoBlocoCeDeE: boolean;

  @Input() cpfTrabalhador: string;

  @Output() carregado = new EventEmitter();
  @Output() aoAbrirPopup = new EventEmitter();

  codIrrf:number;
  codReceita: string;
  valorIrrf: number;
  valorIrrf13: number;
  codF2501: number;
  total: number = 0;
  // temPermissaoBlocoCeDeE: boolean = false;
  dataSource: Array<PenAlim> = [];
  penAlim: PenAlim;

  buscarDescricaoFormControl: FormControl = new FormControl(null);
  codigoFormularioFormControl: FormControl = new FormControl(null);
  codigoIrrfFormControl: FormControl = new FormControl(null);

  async ngAfterViewInit(): Promise<void> {
    this.codigoFormularioFormControl.setValue(this.codF2501);
    this.codigoIrrfFormControl.setValue(this.codIrrf);
    await this.buscarTabela();

  }

  iniciaValoresDaView() {
    let sort: any =
      this.table === undefined || !this.table.sortColumn
        ? 'descricao'
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

  async obterPenAlim() {
    // const response = await this.service.obterPenAlim(this.codF2501, this.codIrrf);
    // if (response){
    //       this.penAlim = response;
    //       this.iniciarForm();
    // }
  }

  buscarTabela() {
    if (this.codigoIrrfFormControl.value > 0) {
      let view = this.iniciaValoresDaView();
      this.service
        .obterPaginado(
          this.codigoIrrfFormControl.value,
          view.pageIndex,
          view.sortColumn,
          view.sortDirection == 'asc'
        )
        .then(data => {
          this.total = data.total;
          this.dataSource = data.lista;
          this.carregado.emit(true);
        })
        .catch((error) => {
          const mensagem = ErrorLib.ConverteMensagemErro(error);
          this.dialog.err(
            'Informações não carregadas',
            mensagem
          );
        });
    }
  }

  async incluir(): Promise<void> {
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean = await PenAlimModal_v1_2_Component.exibeModal(
      null,
      this.codF2501,
      this.codIrrf,
      true
    );

    if (teveAlteracao) {
      // this.buscarDescricaoFormControl.setValue('');
      this.buscarTabela();
    }
  }

  async alterar(item: PenAlim): Promise<void> {
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean = await PenAlimModal_v1_2_Component.exibeModal(
      item,
      this.codF2501,
      this.codIrrf,
      this.temPermissaoBlocoCeDeE
    );

    if (teveAlteracao) {
      this.buscarDescricaoFormControl.setValue('');
      this.buscarTabela();
    }
  }

  private emitAoAbrirPopup() {
    if (this.temPermissaoBlocoCeDeE) {
      this.aoAbrirPopup.emit();
    }
  }

  async excluir(item: PenAlim): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Pensão Alimentícia',
      `Deseja excluir o Beneficiário<br><b>${item.penalimCpfdep}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(item.idEsF2501Penalim, this.codF2501, item.idEsF2501Infocrirrf);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Pensão Alimentícia excluída!'
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

  close(): void {
    this.modal.close(false);
  }

  static exibeModalAlterar(codF2501: number, codIrrf: number, codReceita: string, valorIrrf: number, valorIrrf13: number, temPermissaoBlocoCeDeE: boolean): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      EsocialPenAlimModal_v1_2_Component,
      { windowClass: 'modal-pen-alim', centered: true, size: 'lg', backdrop: 'static' }
    );
    modalRef.componentInstance.codF2501 = codF2501;
    modalRef.componentInstance.codIrrf = codIrrf;
    modalRef.componentInstance.codReceita = codReceita;
    modalRef.componentInstance.valorIrrf = valorIrrf;
    modalRef.componentInstance.valorIrrf13 = valorIrrf13;
    modalRef.componentInstance.temPermissaoBlocoCeDeE = temPermissaoBlocoCeDeE;
    modalRef.componentInstance.titulo = temPermissaoBlocoCeDeE ? 'Alterar' : 'Consultar';
    modalRef.componentInstance.obterPenAlim();
    return modalRef.result;
  }
}

