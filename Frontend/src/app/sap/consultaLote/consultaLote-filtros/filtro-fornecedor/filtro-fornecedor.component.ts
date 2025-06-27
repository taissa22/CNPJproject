import { Component, OnInit } from '@angular/core';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { FilterService } from '../../services/filter.service';
import { SapService } from 'src/app/core/services/sap/sap.service';
import { ListaFiltroRadioConsultaLoteEnum } from '@shared/enums/lista-filtro-radio-consulta-lote.enum';

@Component({
  selector: 'filtro-fornecedor',
  templateUrl: './filtro-fornecedor.component.html',
  styleUrls: ['./filtro-fornecedor.component.scss']
})
export class FiltroFornecedorComponent implements OnInit {

  constructor(private filterService: FilterService, private sapService: SapService) { }
  listaFornecedor: Array<DualListModel> = [];

  ngDoCheck(): void {
  }

  ngAfterContentInit(): void {
  }

   ngOnInit() {
     this.listaFornecedor = this.filterService.getListaFornecedor();

     this.listaFornecedor.filter(item1 =>
      this.filterService.auxFornecedor.includes(item1.id)).map(
        item => item.selecionado = true
      );

  }

  mostraSelecionados(event) {
    const lista = event.map(teste => {
      return teste.id;
    });

    this.filterService.auxFornecedor = lista;
    if(lista.length == this.listaFornecedor.length){
      this.filterService.AddlistaFornecedor(null);
    }else{
      this.filterService.AddlistaFornecedor(lista);
    }
    
    this.sapService.atualizaCount(event.length,
    ListaFiltroRadioConsultaLoteEnum.fornecedor);
  }
}
