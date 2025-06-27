import { Component, OnInit } from '@angular/core';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { TrabalhistaFiltrosService } from '../../services/trabalhista-filtros.service';
import { EscritorioFiltroService } from '../../services/EscritorioFiltro.service';
import { ListaFiltroAgendaAudienciaTrabalhistaEnum } from '../../interface/ListaFiltroAgendaAudienciaTrabalhista.enum';

@Component({
  selector: 'app-escritorio',
  templateUrl: './escritorio.component.html',
  styleUrls: ['./escritorio.component.scss']
})
export class EscritorioComponent implements OnInit {

 /** Lista de escritorios */
 listaEscritorios: Array<DualListModel> = [];

 tipoEscritorio: string = this.service.getTipoEscritorio()

 constructor(private consultaService: TrabalhistaFiltrosService,
   private service: EscritorioFiltroService) { }

 ngOnInit() {
   this.getListaAdvogados();
 }

 getListaAdvogados() {
   this.listaEscritorios = this.service.listaEscritorio;
 }

 mostraSelecionados(event) {
   let listaID = event.map(item =>  parseInt(item.id))

   this.consultaService.atualizarContagem(event.length,
     ListaFiltroAgendaAudienciaTrabalhistaEnum.escritorio);
   this.service.adicionarFiltro(listaID, false);

 }

 onItemChange(){
   this.service.adicionarFiltroRadio(this.tipoEscritorio)
 }
}
