import { Component, OnInit } from '@angular/core';
import { PrepostoService } from './../../services/Preposto.service';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { TrabalhistaFiltrosService } from '../../services/trabalhista-filtros.service';
import { ListaFiltroAgendaAudienciaTrabalhistaEnum } from '../../interface/ListaFiltroAgendaAudienciaTrabalhista.enum';

@Component({
  selector: 'preposto-filtros',
  templateUrl: './preposto.component.html',
  styleUrls: ['./preposto.component.scss']
})
export class PrepostoComponent implements OnInit {

   /** Lista de Preposto */
 listaPreposto: Array<DualListModel> = [];

 constructor(private consultaService: TrabalhistaFiltrosService,
   private service: PrepostoService) { }

 ngOnInit() {
   this.getListaPrepostos();
 }

 getListaPrepostos() {
   this.listaPreposto = this.service.listaPreposto;
 }

 mostraSelecionados(event) {
   let listaID = event.map(item => item.id)

   this.consultaService.atualizarContagem(event.length,
     ListaFiltroAgendaAudienciaTrabalhistaEnum.preposto);
   this.service.adicionarFiltro(listaID, false);

 }
}
