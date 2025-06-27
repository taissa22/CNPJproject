import { Injectable } from '@angular/core';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { TrabalhistaFiltrosService } from './trabalhista-filtros.service';

@Injectable({
  providedIn: 'root'
})
export class AdvogadoService {

  /** Lista de advogados e advogados acompanhantes */
  listaAdvogados: Array< DualListModel> = [];
  listaAdvogadoAcompanahnte: Array< DualListModel> = [];

  constructor(private trabalhistaService: TrabalhistaFiltrosService) { }

  adicionarFiltro(advogado: number[], isAdvogadoAcompanhante: boolean = false) {



    isAdvogadoAcompanhante ?
    // Verificar se escolheu todos advogados acompanhantes e mandar nulo
    (this.listaAdvogadoAcompanahnte.length == advogado.length ?
    (this.trabalhistaService.json.advogadoAcompanhante = [], this.trabalhistaService.allAdvogadoAcompanhante = true) :
     (this.trabalhistaService.json.advogadoAcompanhante = advogado, this.trabalhistaService.allAdvogadoAcompanhante = false) )

      :
       // Verificar se escolheu todos advogados e mandar nulo
     ( this.listaAdvogados.length == advogado.length ?
      (this.trabalhistaService.json.advogadoAudiencia = [], this.trabalhistaService.allAdvogado = true ):
       (this.trabalhistaService.json.advogadoAudiencia= advogado, this.trabalhistaService.allAdvogado = false))
      ;
  }

  limpar() {
    this.listaAdvogados = [];
    this.listaAdvogadoAcompanahnte = [];
  }

}
