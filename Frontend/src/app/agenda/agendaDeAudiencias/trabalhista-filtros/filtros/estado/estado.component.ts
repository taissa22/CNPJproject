import { Component, OnInit } from '@angular/core';
import { EstadoService } from '../../services/Estado.service';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { TrabalhistaFiltrosService } from '../../services/trabalhista-filtros.service';
import { ListaFiltroAgendaAudienciaTrabalhistaEnum } from '../../interface/ListaFiltroAgendaAudienciaTrabalhista.enum';

@Component({
  selector: 'estado-filtros',
  templateUrl: './estado.component.html',
  styleUrls: ['./estado.component.scss']
})
export class EstadoComponent implements OnInit {


 /** Lista de escritorios */
 listaEstados: Array<DualListModel> = [];

 constructor(private consultaService: TrabalhistaFiltrosService,
   private service: EstadoService) { }

 ngOnInit() {
   this.getListaEstados();
 }

 getListaEstados() {
   this.listaEstados = this.service.listaEstados;
 }

 mostraSelecionados(event) {
   let listaID = event.map(item => item.id)

   this.consultaService.atualizarContagem(event.length,
     ListaFiltroAgendaAudienciaTrabalhistaEnum.estado);
   this.service.adicionarFiltro(listaID);

 }

}
