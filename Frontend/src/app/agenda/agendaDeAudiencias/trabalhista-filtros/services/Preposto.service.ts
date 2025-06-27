import { Injectable } from '@angular/core';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { TrabalhistaFiltrosService } from './trabalhista-filtros.service';

@Injectable({
  providedIn: 'root'
})
export class PrepostoService {


  listaPreposto: Array<DualListModel> = [];
  listaPrepostoAcompanhante: Array<DualListModel> = [];

  constructor(private trabalhistaService: TrabalhistaFiltrosService) { }

  adicionarFiltro(preposto: number[], isprespostoAcompanhante: boolean = false) {
    isprespostoAcompanhante ?
      // Verificar se escolheu todos prepostos acompanhantes e mandar nulo
      (this.listaPrepostoAcompanhante.length == preposto.length ?
        (this.trabalhistaService.json.prepostoAcompanhante = [], this.trabalhistaService.allPrepostoAcompanhante = true) :
        (this.trabalhistaService.json.prepostoAcompanhante = preposto, this.trabalhistaService.allPrepostoAcompanhante = false))

      :
      // Verificar se escolheu todos prepostos e mandar nulo
      (this.listaPreposto.length == preposto.length ?
        (this.trabalhistaService.json.preposto = [], this.trabalhistaService.allPreposto = true) :
        (this.trabalhistaService.json.preposto = preposto , this.trabalhistaService.allPreposto = false))
      ;
  }

  limpar() {
    this.listaPreposto = [];
    this.listaPrepostoAcompanhante = [];

  }

}
