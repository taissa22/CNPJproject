import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Observable, of } from 'rxjs';
import { AgendaAudienciaService } from 'src/app/core/services/processos/AgendaAudiencia/AgendaAudiencia.service';
import { IAgendaAudienciaTrabalhistaFiltros } from 'src/app/core/services/processos/AgendaAudiencia/IAgendaAudienciaTrabalhistaFiltros';

/**
 *
 * @description Resolver de tela para carregamento dos filtros antes de mostrar a tela
 * @class AgendaAudienciaFiltrosResolver
 * @implements {Resolve any}
 *
 */
@Injectable()
export class AgendaAudienciaFiltrosTrabalhistaResolver implements Resolve<any> {

  constructor(private agendaService: AgendaAudienciaService) {
  }

  resolve(): Observable<IAgendaAudienciaTrabalhistaFiltros> {
    
    return this.agendaService.Carregarfiltros();
  }
}
