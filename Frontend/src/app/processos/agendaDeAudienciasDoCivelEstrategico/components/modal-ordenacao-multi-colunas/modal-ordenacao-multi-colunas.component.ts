import { StaticInjector } from './../../../../shared/static-injector';
import { SortOrderView } from './../../../../shared/interfaces/Sort/SortOrderView';
import { NgbActiveModal, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormControl } from '@angular/forms';

import { ColunaModal } from './model/ColunaModal.model';
import { ColunaParaOrdenacao } from './model/colunaOrdenacao.model';

@Component({
  templateUrl: './modal-ordenacao-multi-colunas.component.html',
  styleUrls: ['./modal-ordenacao-multi-colunas.component.scss']
})
export class ModalOrdenacaoMultiColunasComponent implements OnInit {
  @Input() titulo: string;
  @Input() listaOrdenacao: Array<ColunaParaOrdenacao>;

  // colunas: Array<ColunaModal> = [ new ColunaModal('Nome Coluna', 'nomeColuna') ];
  colunas: Array<SortOrderView> = [
    {
      direction: 'asc',
      property: 'nomeColuna',
      columnName: 'Nome Coluna',
    }
  ];
  lista: Array<ColunaParaOrdenacao>;

  constructor(public activeModal: NgbActiveModal) { }

  ngOnInit() {
    this.lista = this.listaOrdenacao
      .map(x => {
        return new ColunaParaOrdenacao(x.id, x.prioridade, x.nomeColuna, x.tipoOrdenacao, new FormControl(x.formControl.value));
      });
  }

  get listaOrdenacaoPorPrioridade() {
    return this.lista.sort((a, b) => a.prioridade - b.prioridade);
  }

  get indexDaUltimaPrioridade() {
    return this.lista ? (this.lista.length - 1) : 0;
  }

  aumentarPrioridade(coluna) {
    if (coluna.prioridade === 0) { return; }

    const novaPrioridade = coluna.prioridade - 1;
    const colunaComPrioridade = this.lista.find(c => c.prioridade === novaPrioridade);

    colunaComPrioridade.prioridade = coluna.prioridade;
    coluna.prioridade = novaPrioridade;
  }

  diminuirPrioridade(coluna: ColunaParaOrdenacao) {
    if (coluna.prioridade === this.indexDaUltimaPrioridade) { return; }

    const novaPrioridade = coluna.prioridade + 1;
    const colunaComPrioridade = this.lista.find(c => c.prioridade === novaPrioridade);

    colunaComPrioridade.prioridade = coluna.prioridade;
    coluna.prioridade = novaPrioridade;
  }

  ordenar() {
    this.activeModal.close(this.lista);
  }

  //#region MODAL

  // tslint:disable-next-line: member-ordering
  private static exibeModal(): NgbModalRef {
    const modalService: NgbModal = StaticInjector.Instance.get(NgbModal);
    const modalRef = modalService.open(ModalOrdenacaoMultiColunasComponent, { centered: true, size: 'lg', backdrop: 'static' });
    return modalRef;
  }

  // tslint:disable-next-line: member-ordering
  public static async abrirModal(listaOrdenacao: any): Promise<any> {
    const modalRef = this.exibeModal();
    modalRef.componentInstance.titulo = 'Ordenação e Prioridade - Agenda de Audiências';
    modalRef.componentInstance.listaOrdenacao = listaOrdenacao;
    return await modalRef.result;
  }

  teste(teste) {
    console.log(teste);
  }

  //#endregion MODAL
}
