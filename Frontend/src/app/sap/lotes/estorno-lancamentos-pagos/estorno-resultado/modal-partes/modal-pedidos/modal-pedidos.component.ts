import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { EstornoResultadoService } from '../../../services/estorno-resultado.service';
import { EstornoLancamentosPagosService } from '../../../services/estorno-lancamentos-pagos.service';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-modal-pedidos',
  templateUrl: './modal-pedidos.component.html',
  styleUrls: ['./modal-pedidos.component.scss']
})
export class ModalPedidosComponent implements OnInit {
  pedidos: any[];
  titulo = 'Pedidos do Processo'
  subTitulo ='Consulta dos dados dos pedidos vinculados ao processo.'
  subscription: Subscription;

  constructor(public bsModalRef: BsModalRef,
    private estornoResultadoService: EstornoResultadoService,
  private estornoLancamentoService : EstornoLancamentosPagosService) { }

  ngOnInit() {
    this.subscription = this.estornoResultadoService.getProcessoSelecionado().subscribe(
     processo => this.pedidos = processo['pedidos']

   )


    if (this.estornoLancamentoService.currentItemComboboxTipoProcesso.value
      == TipoProcessoEnum.PEX || this.estornoLancamentoService.currentItemComboboxTipoProcesso.value
      == TipoProcessoEnum.civelEstrategico) {
      this.tituloPedido = 'Contratos/Pedidos';
    }
  }

  tituloPedido = 'Pedidos';

  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.
    this.subscription.unsubscribe();
  }

}
