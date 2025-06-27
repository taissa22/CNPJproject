import { SapService } from 'src/app/core/services/sap/sap.service';
import { DualListTitulo } from './../../../../core/models/dual-list-titulo';
import { FilterService } from 'src/app/sap/consultaLote/services/filter.service';
import { Component, OnInit } from '@angular/core';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { ListaFiltroRadioConsultaLoteEnum } from '@shared/enums/lista-filtro-radio-consulta-lote.enum';

@Component({
  selector: 'filtro-categoria-pagamento',
  templateUrl: './filtro-categoria-pagamento.component.html',
  styleUrls: ['./filtro-categoria-pagamento.component.scss']
})
export class FiltroCategoriaPagamentoComponent implements OnInit {

  constructor(private filterService : FilterService, private sapService:SapService) { }
  listaCategoria: Array<DualListTitulo> = [];

  ngOnInit() {
    this.listaCategoria= this.filterService.getListaCategoriaPagamento();

  }

  mostraSelecionados(lista:DualListTitulo[]){

    let count = 0;
    lista.forEach(itens => {
      itens.dados.forEach(dados => {
        if(dados.selecionado){
          count++;
        }

      });

    });
    this.filterService.addCategoriadePagamento();
    let i = 0;
    let j = 0;
    let id = []
    this.listaCategoria.forEach(element => {
      element.dados.forEach(dados => {
        if(dados.selecionado){
          i++;
          id.push(dados.id)
        }
        j++

      });

    });
    if(i==j){
      this.filterService.colocarNull();
    }else{
      this.filterService.filtro.idsCategoriasPagamentos = id;
    }

    this.sapService.atualizaCount(count,ListaFiltroRadioConsultaLoteEnum.categoriaPagamento);
  }

}
