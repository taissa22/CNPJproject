import { Injectable } from "@angular/core";

import { TrabalhistaFiltrosService } from "./trabalhista-filtros.service";

import { DualListModel } from "src/app/core/models/dual-list.model";

@Injectable({
  providedIn: "root",
})
export class EscritorioFiltroService {
  listaEscritorio: Array<DualListModel> = [];

  listaEscritorioAcompanahnte: Array<DualListModel> = [];

  constructor(private trabalhistaService: TrabalhistaFiltrosService) {}
  adicionarFiltro(
    escritorio: number[],
    isEscritorioAcompanhante: boolean = false
  ) {
    let usuario = this.trabalhistaService.getusuario();

    usuario.ehEscritorio
      ? (isEscritorioAcompanhante
          ? (this.trabalhistaService.json.escritorioAcompanhante = escritorio)
          : (this.trabalhistaService.json.escritorioAudiencia = escritorio),
        (this.trabalhistaService.allEscritorioAcompanhante = false))
      : isEscritorioAcompanhante
      ? // Verificar se escolheu todos escritorios acompanhantes e mandar nulo

        this.listaEscritorioAcompanahnte.length == escritorio.length
        ? ((this.trabalhistaService.json.escritorioAcompanhante = []),
          (this.trabalhistaService.allEscritorioAcompanhante = true))
        : ((this.trabalhistaService.json.escritorioAcompanhante = escritorio),
          (this.trabalhistaService.allEscritorioAcompanhante = false))
      : // Verificar se escolheu todos escritorios e mandar nulo

      this.listaEscritorio.length == escritorio.length
      ? ((this.trabalhistaService.json.escritorioAudiencia = []),
        (this.trabalhistaService.allEscritorio = true))
      : ((this.trabalhistaService.json.escritorioAudiencia = escritorio),
        (this.trabalhistaService.allEscritorio = false));
  }
  adicionarFiltroRadio(value) {
    this.trabalhistaService.json.tipoEscritorio = value;
  }
  limpar() {
    this.listaEscritorio = [];

    this.listaEscritorioAcompanahnte = [];
  }
  getTipoEscritorio(): string {
    return this.trabalhistaService.json.tipoEscritorio;
  }
}
