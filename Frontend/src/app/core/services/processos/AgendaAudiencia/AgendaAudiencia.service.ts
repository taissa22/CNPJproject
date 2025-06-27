import { ApiService } from "../..";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs/internal/Observable";
import { IAgendaAudienciaTrabalhistaFiltros } from "./IAgendaAudienciaTrabalhistaFiltros";
import { of } from "rxjs";
import { SearchCommand } from "@shared/interfaces/SearchCommand";
import { IPedidos, IPedidosResultado } from "./IPedidos";
import { ValorParteListas } from "src/app/componentes/partes-modal/IPartes";
import { IAgendaProcessoResultadoPesquisar } from "@shared/interfaces/IAgendaProcessoPesquisar";

@Injectable({
  providedIn: "root",
})
export class AgendaAudienciaService {
  constructor(private api: ApiService) {}
  private endpoint = "/AudienciaProcesso/";
  private endpointv2 = "/AgendaAudiencia/";

  /**
   *@description Recupera do back a lista de filtros da tela de agenda de audiencia
   * @memberof AgendaAudienciaService
   */
  Carregarfiltros(): Observable<IAgendaAudienciaTrabalhistaFiltros> {
    return this.api.getv2(this.endpointv2 + "CarregarFiltros");
  }

  /**
   *@description Recupera a lista com a grid filtrada pelo json dos filtros
   * @memberof AgendaAudienciaService
   */
  RecuperarListaResultado(json): Observable<any> {
    //return this.api.get(this.endpoint + 'CarregarFiltros');
    return of();
  }

  pesquisar(
    filter: SearchCommand
  ): Observable<IAgendaProcessoResultadoPesquisar> {
    return this.api.post(this.endpoint + "pesquisar", filter);
  }

  RecuperarPedidos(codigoProcesso: number): Observable<IPedidosResultado> {
    return this.api.get(
      this.endpoint + "RecuperarPedidos?codigoProcesso=" + codigoProcesso
    );
  }

  RecuperarPartes(codigoProcesso: number): Observable<ValorParteListas> {
    return this.api.get(
      this.endpoint + "RecuperarPartes?codigoProcesso=" + codigoProcesso
    );
  }

  exportar(filter: SearchCommand) {
    return this.api.post(this.endpoint + "exportar", filter);
  }
}
