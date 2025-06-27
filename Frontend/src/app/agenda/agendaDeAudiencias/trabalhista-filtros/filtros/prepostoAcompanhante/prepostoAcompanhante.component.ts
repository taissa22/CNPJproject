import { Component, OnInit } from '@angular/core';
import { PrepostoService } from '../../services/Preposto.service';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { TrabalhistaFiltrosService } from '../../services/trabalhista-filtros.service';
import { ListaFiltroAgendaAudienciaTrabalhistaEnum } from '../../interface/ListaFiltroAgendaAudienciaTrabalhista.enum';

@Component({
  selector: 'prepostoAc-filtros',
  templateUrl: './prepostoAcompanhante.component.html',
  styleUrls: ['./prepostoAcompanhante.component.scss']
})
export class PrepostoAcompanhanteComponent implements OnInit {

   /** Lista de Preposto */
 listaPreposto: Array<DualListModel> = [];

 constructor(private consultaService: TrabalhistaFiltrosService,
   private service: PrepostoService) { }

 ngOnInit() {
   this.getListaPrepostos();
 }

 getListaPrepostos() {
   this.listaPreposto = this.service.listaPrepostoAcompanhante;
 }

 mostraSelecionados(event) {
   let listaID = event.map(item => item.id)

   this.consultaService.atualizarContagem(event.length,
     ListaFiltroAgendaAudienciaTrabalhistaEnum.prepostoAcompanhante);
   this.service.adicionarFiltro(listaID, true);

 }
}
