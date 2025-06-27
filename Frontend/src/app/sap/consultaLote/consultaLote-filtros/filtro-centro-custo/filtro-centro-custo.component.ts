import { Component, OnInit } from '@angular/core';
import { FilterService } from '../../services/filter.service';
import { SapService } from 'src/app/core/services/sap/sap.service';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import {  ListaFiltroRadioConsultaLoteEnum } from '@shared/enums/lista-filtro-radio-consulta-lote.enum';

@Component({
  selector: 'filtro-centro-custo',
  templateUrl: './filtro-centro-custo.component.html',
  styleUrls: ['./filtro-centro-custo.component.scss']
})
export class FiltroCentroCustoComponent implements OnInit {

  constructor(private filterService: FilterService, private sapService: SapService) { }
  listaCentro: Array<DualListModel> = [];


  ngDoCheck(): void {
  }

  ngAfterContentInit(): void {
  }

   ngOnInit() {
     this.listaCentro = this.filterService.getListaCentroCusto();

     this.listaCentro.filter(item1 =>
      this.filterService.auxCentroCusto.includes(item1.id)).map(
        item => item.selecionado = true
     );
  }

  mostraSelecionados(event) {
    const lista = event.map(teste => {
      return teste.id;
    });

    this.filterService.auxCentroCusto = lista;

    if(lista.length == this.listaCentro.length){
      this.filterService.AddlistaCusto([]);
    }else{
      this.filterService.AddlistaCusto(lista);
    }
    
    this.sapService.atualizaCount(event.length,ListaFiltroRadioConsultaLoteEnum.centroCusto);
  }
}
