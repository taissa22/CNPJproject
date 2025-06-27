import { Component, OnInit } from '@angular/core';
import { EmpresaGrupoFiltroService } from './../../services/EmpresaGrupoFiltro.service';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { TrabalhistaFiltrosService } from '../../services/trabalhista-filtros.service';
import { ListaFiltroAgendaAudienciaTrabalhistaEnum } from '../../interface/ListaFiltroAgendaAudienciaTrabalhista.enum';

@Component({
  selector: 'empresaGrupo-filtro',
  templateUrl: './empresaGrupo.component.html',
  styleUrls: ['./empresaGrupo.component.scss']
})
export class EmpresaGrupoComponent implements OnInit {

   /** Lista de empresa */
   listaEmpresaGrupo: Array<DualListModel> = [];

   constructor(private consultaService: TrabalhistaFiltrosService,
     private service: EmpresaGrupoFiltroService) { }

   ngOnInit() {
     this.getListaAdvogados();
   }

   getListaAdvogados() {
     this.listaEmpresaGrupo = this.service.listaEmpresas;
   }

   mostraSelecionados(event) {
     let listaID = event.map(item => item.id)

     this.consultaService.atualizarContagem(event.length,
       ListaFiltroAgendaAudienciaTrabalhistaEnum.empresaGrupo);
     this.service.adicionarFiltro(listaID);

   }

}
