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
import { DeduçoesDependente } from '@esocial/models/subgrupos/v1_2/deducoes-dependente';
import { DeducoesDependenteService } from '@esocial/services/formulario/subgrupos/deducoes-dependente.service';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { DialogService } from '@shared/services/dialog.service';
import { StaticInjector } from '@esocial/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DedDepenModal_v1_2_Component } from '../info-deducoes-dependente-edit-modal/deducoes-dependente-edit-modal.component';


@Component({
  selector: 'app-esocial-deducoes-dependente-modal',
  templateUrl: './esocial-deducoes-dependente-modal.component.html',
  styleUrls: ['./esocial-deducoes-dependente-modal.component.scss']
})
export class EsocialDedDepenModal_v1_2_Component implements AfterViewInit {
  breadcrumb: string;
  constructor(
    private modal: NgbActiveModal,
    private service: DeducoesDependenteService,
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
  dataSource: Array<DeduçoesDependente> = [];

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

  async obterImposto() {
    // const response = await this.service.obterImposto(this.codF2501, this.codIrrf);
    // if (response){
    //       this.imposto = response;
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
    const teveAlteracao: boolean = await DedDepenModal_v1_2_Component.exibeModal(
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

  async alterar(item: DeduçoesDependente): Promise<void> {
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean = await DedDepenModal_v1_2_Component.exibeModal(
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

  async excluir(item: DeduçoesDependente): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Dependente',
      `Deseja excluir o Dependente<br><b>${item.deddepenCpfdep}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(item.idEsF2501Deddepen, this.codF2501, item.idEsF2501Infocrirrf);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Dependente excluído!'
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
      EsocialDedDepenModal_v1_2_Component,
      { windowClass: 'modal-imposto', centered: true, size: 'lg', backdrop: 'static' }
    );
    modalRef.componentInstance.codF2501 = codF2501;
    modalRef.componentInstance.codIrrf = codIrrf;
    modalRef.componentInstance.codReceita = codReceita;
    modalRef.componentInstance.valorIrrf = valorIrrf;
    modalRef.componentInstance.valorIrrf13 = valorIrrf13;
    modalRef.componentInstance.temPermissaoBlocoCeDeE = temPermissaoBlocoCeDeE;
    modalRef.componentInstance.titulo = temPermissaoBlocoCeDeE ? 'Alterar' : 'Consultar';
    modalRef.componentInstance.obterImposto();
    return modalRef.result;
  }
}

