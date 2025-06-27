import { Injectable } from '@angular/core';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { ConsultaSaldoGarantiaService } from './consulta-saldo-garantia.service';
import { ListaFiltroSaldoGarantiaRadio } from '@shared/enums/lista-filtro-saldo-garantia-radio.enum';

@Injectable({
  providedIn: 'root'
})
export class ConsultaFiltroEmpresaGrupoService {

  constructor(private consultaService: ConsultaSaldoGarantiaService) { }

  public listaEmpresaGrupo: Array<DualListModel> = [];

  limpar() {
    this.listaEmpresaGrupo = [];
    this.consultaService.atualizaCount(this.listaEmpresaGrupo.length,
      ListaFiltroSaldoGarantiaRadio.empresaGrupo);
  }

  adicionarEmpresaDTO(lista: any) {

    this.consultaService.json.idsEmpresaGrupo = lista;
  }

}
