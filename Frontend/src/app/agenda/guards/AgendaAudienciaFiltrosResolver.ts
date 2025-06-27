import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Observable, of } from 'rxjs';
import { AgendaAudienciaService } from 'src/app/core/services/processos/AgendaAudiencia/AgendaAudiencia.service';
import { IAgendaAudienciaTrabalhistaFiltros } from 'src/app/core/services/processos/AgendaAudiencia/IAgendaAudienciaTrabalhistaFiltros';
import { TrabalhistaFiltrosService } from './../agendaDeAudiencias/trabalhista-filtros/services/trabalhista-filtros.service';

/**
 *
 * @description Resolver de tela para carregamento dos filtros antes de mostrar a tela
 * @class AgendaAudienciaFiltrosResolver
 * @implements {Resolve any}
 *
 */
@Injectable()
export class AgendaAudienciaFiltrosTrabalhistaResolver implements Resolve<any> {

  constructor(private agendaService: AgendaAudienciaService,
    private filterService: TrabalhistaFiltrosService) {
  }

  resolve(): Observable<IAgendaAudienciaTrabalhistaFiltros> {
    if (!this.filterService.manter)
    return this.agendaService.Carregarfiltros();


  }
}
