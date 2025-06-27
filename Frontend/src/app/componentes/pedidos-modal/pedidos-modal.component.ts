import { Component, Input, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';

@Component({
  selector: 'app-pedidos-modal',
  templateUrl: './pedidos-modal.component.html',
  styleUrls: ['./pedidos-modal.component.scss']
})
export class PedidosModalComponent implements OnInit {

  @Input() pedidos: Array<{descricao: string}>;
  @Input() titulo = 'Pedidos do Processo';
  @Input() subTitulo ='Consulta dos dados dos pedidos vinculados ao processo.';
  @Input() tituloPedido = 'Pedidos';

  constructor(public bsModalRef: BsModalRef  ) { }

  ngOnInit() {
  }



}
