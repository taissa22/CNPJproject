import { Component, OnInit } from '@angular/core';
import { FilterService } from '../../services/filter.service';
import { SapService } from 'src/app/core/services/sap/sap.service';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { ListaFiltroRadioConsultaLoteEnum } from '@shared/enums/lista-filtro-radio-consulta-lote.enum';

@Component({
  selector: 'filtro-tipo-lancamento',
  templateUrl: './filtro-tipo-lancamento.component.html',
  styleUrls: ['./filtro-tipo-lancamento.component.scss']
})
export class FiltroTipoLancamentoComponent implements OnInit {

  constructor(private filterService: FilterService, private sapService: SapService) { }
  listaLancamentos: Array<DualListModel> = [];


  ngDoCheck(): void {
  }

  ngAfterContentInit(): void {
  }

   ngOnInit() {
     this.listaLancamentos = this.filterService.getListaTipoLancamento();

     this.listaLancamentos.filter(item1 =>
      this.filterService.auxTipoLancamento.includes(item1.id)).map(
        item => item.selecionado = true
      );
  }

  mostraSelecionados(event) {
    const lista = event.map(teste => {
      return teste.id;
    });

    this.filterService.auxTipoLancamento = lista;
    if(lista.length == this.listaLancamentos.length){
      this.filterService.AddlistaLancamento([]);
    }else{
      this.filterService.AddlistaLancamento(lista);
    }
    
    this.sapService.atualizaCount(event.length, ListaFiltroRadioConsultaLoteEnum.tipoLancamento);
  }

}
