import { Injectable } from '@angular/core';
import { List } from 'linqts';
import { BehaviorSubject } from 'rxjs';
import { FilterService } from 'src/app/sap/consultaLote/services/filter.service';


@Injectable({
  providedIn: 'root'
})
export class FiltroService {

  constructor(private filterService: FilterService) { }

  public sapsSelecionados: List<string> = new List();

  public guiasSelecionadas: List<string> = new List();

  public dataInicioLote = new BehaviorSubject<Date>(null);
  public dataFimLote = new BehaviorSubject<Date>(null);

  public dataInicioPedido = new BehaviorSubject<Date>(null);
  public dataFimPedido = new BehaviorSubject<Date>(null);

  public limpar() {
    this.sapsSelecionados = new List();
    this.guiasSelecionadas = new List();
    this.dataInicioLote = new BehaviorSubject<Date>(null);
    this.dataFimLote = new BehaviorSubject<Date>(null);
    this.dataInicioPedido = new BehaviorSubject<Date>(null);
    this.dataFimPedido = new BehaviorSubject<Date>(null);
    this.filterService.updateTipoProcesso(null);
}



}
