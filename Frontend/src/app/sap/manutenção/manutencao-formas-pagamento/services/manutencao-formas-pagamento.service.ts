import { Injectable } from '@angular/core';
import { FormasPagamentoService } from 'src/app/core/services/sap/formas-pagamento.service';
import { ManutencaoCommonService } from 'src/app/sap/shared/services/manutencao-common.service';
import { FormaPagamento } from '@shared/interfaces/sap/FormaPagamento';

@Injectable({
  providedIn: 'root'
})
export class ManutencaoFormasPagamentoService extends ManutencaoCommonService<FormaPagamento, number>{

  constructor(protected service: FormasPagamentoService) {
              super(service);
            }

}
