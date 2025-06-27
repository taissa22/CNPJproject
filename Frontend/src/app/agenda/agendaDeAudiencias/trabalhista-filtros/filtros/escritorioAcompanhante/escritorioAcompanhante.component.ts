import { Component, OnInit } from '@angular/core';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { TrabalhistaFiltrosService } from '../../services/trabalhista-filtros.service';
import { EscritorioFiltroService } from '../../services/EscritorioFiltro.service';
import { ListaFiltroAgendaAudienciaTrabalhistaEnum } from '../../interface/ListaFiltroAgendaAudienciaTrabalhista.enum';

@Component({
  selector: 'app-escritorio',
  templateUrl: './escritorioAcompanhante.component.html',
  styleUrls: ['./escritorioAcompanhante.component.scss']
})
export class EscritorioAcompanhanteComponent implements OnInit {

 /** Lista de escritorios */
 listaEscritorios: Array<DualListModel> = [];

 constructor(private consultaService: TrabalhistaFiltrosService,
   private service: EscritorioFiltroService) { }

 ngOnInit() {
   this.getListaAdvogados();
 }

 getListaAdvogados() {
   this.listaEscritorios = this.service.listaEscritorioAcompanahnte;
 }

 mostraSelecionados(event) {
   let listaID = event.map(item => parseInt(item.id))

   this.consultaService.atualizarContagem(event.length,
     ListaFiltroAgendaAudienciaTrabalhistaEnum.escritorioAcompanhante);
   this.service.adicionarFiltro(listaID, true);

 }
}
