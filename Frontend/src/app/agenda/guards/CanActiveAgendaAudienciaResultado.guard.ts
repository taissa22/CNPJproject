import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ResultadoGuard } from 'src/app/sap/guards/resultado.guard';
import { TrabalhistaFiltrosService } from '../agendaDeAudiencias/trabalhista-filtros/services/trabalhista-filtros.service';

@Injectable({
  providedIn: 'root'
})
export class CanActiveAgendaAudienciaResultado extends ResultadoGuard {

  rotaRetorno = 'agenda/agendaAudienncia/trabalhista/criteriosGuia'

  constructor(private route: Router,
              public service: TrabalhistaFiltrosService
              ) {
    super(route, service);
  }

  isRouteAllowed() : boolean {
    return Object.keys(this.service.valoresTabela$.value.dado).length > 0
  }
}
