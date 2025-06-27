import { Injectable, Output, EventEmitter } from "@angular/core";

import { defaultSortOrder } from "../../trabalhista-filtros.constants";

import { SortOrder } from "./../../../../../shared/interfaces/SortOrder";
import { Observable, BehaviorSubject } from "rxjs";

import { TrabalhistaFiltrosService } from "./../../services/trabalhista-filtros.service";
import { SearchCommand } from "./../../../../../shared/interfaces/SearchCommand";
import { AgendaAudienciaService } from "src/app/core/services/processos/AgendaAudiencia/AgendaAudiencia.service";
import {
  IAgendaProcessoPesquisar,
  IAgendaProcessoResultadoPesquisar,
} from "@shared/interfaces/IAgendaProcessoPesquisar";
import { DefaultPaginatorService } from "src/app/sap/shared/services/default-paginator.service";
import { DefaultQuantityService } from "src/app/sap/shared/services/default-quantity.service";
import {
  IPedidos,
  IPedidosResultado,
} from "src/app/core/services/processos/AgendaAudiencia/IPedidos";
import { DownloadService } from "src/app/core/services/sap/download.service";
import { ValorParteListas } from "src/app/componentes/partes-modal/IPartes";
import { RecuperarCombosEdicaoEndpointService } from "@core/services/processos/AgendaAudiencia/RecuperarCombosEdicaoEndpoint.service";

@Injectable({
  providedIn: "root",
})
export class TrabalhistaResultadoService {
  order: SortOrder[] = defaultSortOrder;

  audienciaSelecionada: IAgendaProcessoPesquisar;

  public atualizardadosTabela = new BehaviorSubject<boolean>(false);

  constructor(
    private audienciaProcessoService: AgendaAudienciaService,
    public trabalhistaFiltroSevice: TrabalhistaFiltrosService,
    public paginatorService: DefaultPaginatorService,
    public quntityService: DefaultQuantityService,
    private downloadService: DownloadService,
    public comboEdicaoEndpoint: RecuperarCombosEdicaoEndpointService
  ) {}

  atualizarTabela(): Observable<IAgendaProcessoResultadoPesquisar> {
    var filter: SearchCommand = {
      filters: this.trabalhistaFiltroSevice.convertJsonToFilterFormat,
      sortOrders: this.order,
      pageNumber: this.paginatorService.pageSubject.value,
      pageSize: this.quntityService.quantidadeSubject.value,
      isExportMethod: false,
    };

    return this.audienciaProcessoService.pesquisar(filter);
  }

  carregarCombos(): any {
    return this.comboEdicaoEndpoint.CarregarCombosEdicao();
  }

  carregarAdvogados(escritorio: any): any {
    return this.comboEdicaoEndpoint.CarregarAdvogados(escritorio);
  }

  alterar(json: any) {
    return this.comboEdicaoEndpoint.editar(json);
  }

  exportar() {
    var filter: SearchCommand = {
      filters: this.trabalhistaFiltroSevice.convertJsonToFilterFormat,
      sortOrders: this.order,
      pageNumber: 0,
      pageSize: 0,
      isExportMethod: true,
    };

    this.downloadService.baixarExportacaoAgenciaTrabalhista(
      filter,
      "Agenda_Audiência_Trabalhista"
    );
  }

  exportarCompleto() {
    var filter: SearchCommand = {
      filters: this.trabalhistaFiltroSevice.convertJsonToFilterFormat,
      sortOrders: this.order,
      pageNumber: 0,
      pageSize: 0,
      isExportMethod: true,
    };

    this.downloadService.baixarExportacaoCompletaAudienciaTrabalhista(
      filter,
      "Agenda_Audiência_Trabalhista"
    );
  }

  download() {
    var filter: SearchCommand = {
      filters: this.trabalhistaFiltroSevice.convertJsonToFilterFormat,
      sortOrders: this.order,
      pageNumber: 0,
      pageSize: 0,
      isExportMethod: true,
    };

    this.downloadService.baixarImpressaoAgenciaTrabalhista(
      filter,
      "Agenda_Audiência_Trabalhista"
    );
  }

  recuperarPedidos(): Observable<IPedidosResultado> {
    return this.audienciaProcessoService.RecuperarPedidos(
      this.audienciaSelecionada.codProcesso
    );
  }

  RecuperarPartes(): Observable<ValorParteListas> {
    return this.audienciaProcessoService.RecuperarPartes(
      this.audienciaSelecionada.codProcesso
    );
  }
}
