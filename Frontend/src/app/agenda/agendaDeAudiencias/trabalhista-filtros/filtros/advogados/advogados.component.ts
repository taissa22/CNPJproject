import { Component, OnInit } from '@angular/core';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { TrabalhistaFiltrosService } from '../../services/trabalhista-filtros.service';
import { AdvogadoService } from './../../services/Advogado.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ListaFiltroAgendaAudienciaTrabalhistaEnum } from '../../interface/ListaFiltroAgendaAudienciaTrabalhista.enum';

@Component({
  selector: 'advogados-filtros',
  templateUrl: './advogados.component.html',
  styleUrls: ['./advogados.component.scss']
})
export class AdvogadosComponent implements OnInit {

  /** Lista de advogados */
  listaAdvogados: Array<DualListModel> = [];

  constructor(private consultaService: TrabalhistaFiltrosService,
    private service: AdvogadoService, private route: Router) { }

  ngOnInit() {
    this.getListaAdvogados();
  }

  getListaAdvogados() {
    this.listaAdvogados = this.service.listaAdvogados;
  }

  mostraSelecionados(event) {
    let listaID = event.map(item => ({id:item.id, codigoChave: item.codigoChave}))

    this.consultaService.atualizarContagem(event.length,
      ListaFiltroAgendaAudienciaTrabalhistaEnum.advogado);
    this.service.adicionarFiltro(listaID, false);

  }

}
