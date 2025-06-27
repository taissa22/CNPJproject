import { Injectable } from '@angular/core';
import { LoteService } from 'src/app/core/services/sap/lote.service';
import swal from 'sweetalert2';
import { bindCallback } from 'rxjs';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { StatusPagamentoEnum } from '@shared/enums/status-pagamento.enum';
@Injectable({
  providedIn: 'root'
})
export class ResultadoSapService {

  private readonly statusPagamentoMessages = [
    {
      id: StatusPagamentoEnum.loteGerado,
      message: 'Deseja cancelar o lote?',
      title: 'Cancelar Lote'
    },
    {
      id: StatusPagamentoEnum.pedidoSapCriado,
      message: 'Deseja solicitar o cancelamento do pedido SAP?',
      title: 'Solicitar Cancelamento'
    },
    {
      id: StatusPagamentoEnum.aguardandoConfirmacaoPagamento,
      message: `O pedido correspondente a este lote já deve ter
                sido cancelado no SAP, pois esta operação não enviará solicitação de cancelamento através
                da interface.<br><br>Deseja confirmar o cancelamento manual deste lote?`,
      title: 'Cancelar Manualmente'
    },
  ];

  constructor(private loteService: LoteService, private helperAngular: HelperAngular) { }

  cancelarLote(lote,tipoProcesso) {
    
    const idStatusPagamento = lote.estado.id;

    const indexStatusPagamento = this.statusPagamentoMessages.findIndex(e => e.id === idStatusPagamento);

    this.helperAngular.MsgBox2(this.statusPagamentoMessages[indexStatusPagamento].message,
      this.statusPagamentoMessages[indexStatusPagamento].title || null, 'question',
      'Sim', 'Não' ).then( result =>{
        if (result.value) { this.loteService.verificarCompromisso(lote,tipoProcesso); }

      })

  }
}
