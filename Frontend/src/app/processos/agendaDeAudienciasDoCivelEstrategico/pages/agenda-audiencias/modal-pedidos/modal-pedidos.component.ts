import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Component, OnInit, Input, AfterViewInit } from '@angular/core';
import { StaticInjector } from '@shared/static-injector';
import { PedidoProcessoService } from '../services/pedido-processo.service';
import { Processo } from '../../../models';
import { map, filter } from 'rxjs/operators';

@Component({
  templateUrl: './modal-pedidos.component.html',
  styleUrls: ['./modal-pedidos.component.scss']
})
export class ModalPedidosComponent implements OnInit, AfterViewInit {

  @Input() entidade: Processo;

  listaPedidos: Array<any>;


  constructor(public activeModal: NgbActiveModal, private pedidosDoProcessoService: PedidoProcessoService) { }

  ngOnInit() { }

  async ngAfterViewInit() {
    if (this.entidade) {
      const response = await this.pedidosDoProcessoService.obterPedidosDoProcesso(this.entidade.id);
      this.listaPedidos = response.map(({pedido}) => {
        return {
          id: pedido.id,
          nome: pedido.descricao
        };
      });
    }
  }

  // tslint:disable-next-line: member-ordering
  public static async exibeModalDeConsultar(processo: Processo): Promise<void> {
    const modalService: NgbModal = StaticInjector.Instance.get(NgbModal);
    const modalRef = modalService.open(ModalPedidosComponent, { centered: true, backdrop: 'static' });
    modalRef.componentInstance.entidade = processo;
    await modalRef.result;
  }

}
