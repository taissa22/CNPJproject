import { Injectable } from '@angular/core';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { ConsultaSaldoGarantiaService } from './consulta-saldo-garantia.service';
import { ListaFiltroSaldoGarantiaRadio } from '@shared/enums/lista-filtro-saldo-garantia-radio.enum';

@Injectable({
  providedIn: 'root'
})
export class ConsultaSaldoGarantiaBancoService {
  public listaBanco: Array<DualListModel> = [];

  constructor(private consultaService: ConsultaSaldoGarantiaService) { }



  limpar() {
    this.listaBanco = [];
    this.consultaService.atualizaCount(this.listaBanco.length,
      ListaFiltroSaldoGarantiaRadio.bancos);
  }

  adicionarBancoDTO(lista: any) {
    lista.length == 0 ? lista = null : lista;

    this.consultaService.json.idsBanco = lista;
  }

}
