import { Injectable } from '@angular/core';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { ConsultaSaldoGarantiaService } from './consulta-saldo-garantia.service';
import { ListaFiltroSaldoGarantiaRadio } from '@shared/enums/lista-filtro-saldo-garantia-radio.enum';

@Injectable({
  providedIn: 'root'
})
export class ConsultaSaldoGarantiaEstadoService {

  public listaEstado: Array<DualListModel> = [];

  constructor(private consultaService: ConsultaSaldoGarantiaService) { }



  limpar() {
    this.listaEstado = [];
    this.consultaService.atualizaCount(this.listaEstado.length,
      ListaFiltroSaldoGarantiaRadio.estado);
  }


  adicionarEstadoDTO(lista: any) {
    lista.length == 0 ? lista = null : lista;

    this.consultaService.json.idsEstado = lista
  }
}
