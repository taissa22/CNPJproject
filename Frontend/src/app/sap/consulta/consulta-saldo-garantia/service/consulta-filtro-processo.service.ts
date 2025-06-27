import { Injectable } from '@angular/core';
import { List } from 'linqts';
import { SapService } from 'src/app/core/services/sap/sap.service';
import { ConsultaTipoProcessoService } from './consulta-tipo-processo.service';
import { ListaFiltroSaldoGarantiaRadio } from '@shared/enums/lista-filtro-saldo-garantia-radio.enum';
import { ConsultaSaldoGarantiaService } from './consulta-saldo-garantia.service';

@Injectable({
  providedIn: 'root'
})
export class ConsultaFiltroProcessoService {

  constructor(private sapService: SapService, private tipoProcesso: ConsultaTipoProcessoService,
    private consultaSaldoGarantiaService: ConsultaSaldoGarantiaService) { }

  public processosSelecionados: List<any> = new List();

  public limparProcesso() {
    this.processosSelecionados = new List();

    this.atualizarCount();

  }
  atualizarCount() {
    this.consultaSaldoGarantiaService.atualizaCount(this.processosSelecionados.ToArray().length,
      ListaFiltroSaldoGarantiaRadio.processos);

    this.adicionarDTO(this.processosSelecionados.ToArray())
  }


  adicionarDTO(selecionados: any) {
    let listaID = selecionados.map(item => item.id);
    listaID.length == 0 ? listaID = null : listaID;
    this.consultaSaldoGarantiaService.json.idsProcesso = listaID;
  }

}
