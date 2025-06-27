import { Component, OnInit } from '@angular/core';
import { FilterService } from '../../services/filter.service';
import { SapService } from 'src/app/core/services/sap/sap.service';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { ListaFiltroRadioConsultaLoteEnum } from '@shared/enums/lista-filtro-radio-consulta-lote.enum';

@Component({
  selector: 'filtro-escritorio',
  templateUrl: './filtro-escritorio.component.html',
  styleUrls: ['./filtro-escritorio.component.scss']
})
export class FiltroEscritorioComponent implements OnInit {


  constructor(private filterService: FilterService, private sapService: SapService) { }
  listaEscritorio: Array<DualListModel> = [];


  ngDoCheck(): void {
  }

  ngAfterContentInit(): void {
  }

   ngOnInit() {
     this.listaEscritorio = this.filterService.getListaEscritorio();


     this.listaEscritorio.filter(item1 =>
      this.filterService.auxEscritorios.includes(item1.id)).map(
        item => item.selecionado = true
      );
  }

  mostraSelecionados(event) {
    const lista = event.map(teste => {
      return teste.id;
    });
    this.filterService.auxEscritorios = lista;
    if(lista.length === this.listaEscritorio.length){
      this.filterService.AddlistaEscritorios([]);
    }else{
      this.filterService.AddlistaEscritorios(lista);
    }

    this.sapService.atualizaCount(event.length,
     ListaFiltroRadioConsultaLoteEnum.escritorio);
  }
}
