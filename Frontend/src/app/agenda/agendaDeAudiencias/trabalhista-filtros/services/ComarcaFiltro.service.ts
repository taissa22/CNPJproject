import { Injectable } from '@angular/core';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { TrabalhistaFiltrosService } from './trabalhista-filtros.service';

@Injectable({
  providedIn: 'root'
})
export class ComarcaFiltroService {

  listaComarcas: Array<DualListModel> = [];
constructor(private trabalhistaService: TrabalhistaFiltrosService) { }

adicionarFiltro(comarca: number[]){
this.listaComarcas.length == comarca.length ?
(this.trabalhistaService.json.codComarca = [] , this.trabalhistaService.allComarca =true) :
(this.trabalhistaService.json.codComarca = comarca, this.trabalhistaService.allComarca = false);
}
limpar() {
  this.listaComarcas = [];

}
}
