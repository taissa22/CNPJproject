import { Component, OnInit } from '@angular/core';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { TrabalhistaFiltrosService } from '../../services/trabalhista-filtros.service';
import { ComarcaFiltroService } from '../../services/ComarcaFiltro.service';
import { ListaFiltroAgendaAudienciaTrabalhistaEnum } from '../../interface/ListaFiltroAgendaAudienciaTrabalhista.enum';

@Component({
  selector: 'comarca-filtros',
  templateUrl: './comarca.component.html',
  styleUrls: ['./comarca.component.scss']
})
export class ComarcaComponent implements OnInit {

  /** Lista de comarcas */
  listaComarcas: Array<DualListModel> = [];

  constructor(private consultaService: TrabalhistaFiltrosService,
    private service: ComarcaFiltroService) { }

  ngOnInit() {
    this.getListaAdvogados();
  }

  getListaAdvogados() {
    this.listaComarcas = this.service.listaComarcas;
  }

  mostraSelecionados(event) {
    let listaID = event.map(item => item.id)

    this.consultaService.atualizarContagem(event.length,
      ListaFiltroAgendaAudienciaTrabalhistaEnum.comarca);
    this.service.adicionarFiltro(listaID);

  }

}
