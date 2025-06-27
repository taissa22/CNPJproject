import { Component, OnInit } from '@angular/core';
import { FilterService } from '../../services/filter.service';
import { SapService } from 'src/app/core/services/sap/sap.service';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { ListaFiltroRadioConsultaLoteEnum } from '@shared/enums/lista-filtro-radio-consulta-lote.enum';

@Component({
  selector: 'filtro-status-lancamento',
  templateUrl: './filtro-status-lancamento.component.html',
  styleUrls: ['./filtro-status-lancamento.component.scss']
})
export class FiltroStatusLancamentoComponent implements OnInit {

  constructor(private filterService: FilterService, private sapService: SapService) { }
  listaPagamento: Array<DualListModel> = [];



  ngDoCheck(): void {
  }

  ngAfterContentInit(): void {
  }

   ngOnInit() {
     this.listaPagamento = this.filterService.getListaStatusPagamento();

     this.listaPagamento.filter(item1 =>
      this.filterService.auxStatusPagamento.includes(item1.id)).map(
        item => item.selecionado = true
      );
  }

  mostraSelecionados(event) {
    const lista = event.map(teste => {
      return teste.id;
    });

    this.filterService.auxStatusPagamento = lista
    if(lista.length == this.listaPagamento.length){
      this.filterService.AddlistaStatusPagamentos([]);
    }else{
      this.filterService.AddlistaStatusPagamentos(lista);
    }
    
    this.sapService.atualizaCount(event.length, ListaFiltroRadioConsultaLoteEnum.statusPagamento);
  }
}
