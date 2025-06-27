import { Injectable } from '@angular/core';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { TrabalhistaFiltrosService } from './trabalhista-filtros.service';

@Injectable({
  providedIn: 'root'
})
export class EstadoService {
  listaEstados: Array<DualListModel> = [];
  constructor(private trabalhistaService: TrabalhistaFiltrosService) { }

  adicionarFiltro(estado: number[]) {
    this.listaEstados.length == estado.length ?
    (this.trabalhistaService.json.siglaEstado = [], this.trabalhistaService.allEstado = true)  :
    (this.trabalhistaService.json.siglaEstado = estado, this.trabalhistaService.allEstado = false);
  }
  limpar() {
    this.listaEstados = [];

  }

}
