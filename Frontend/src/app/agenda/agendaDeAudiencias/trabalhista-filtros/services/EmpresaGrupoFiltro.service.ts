import { Injectable } from '@angular/core';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { TrabalhistaFiltrosService } from './trabalhista-filtros.service';

@Injectable({
  providedIn: 'root'
})
export class EmpresaGrupoFiltroService {

  listaEmpresas: Array<DualListModel> = [];

  constructor(private trabalhistaService: TrabalhistaFiltrosService) { }

  adicionarFiltro(empresa: number[]) {
    this.listaEmpresas.length == empresa.length ?
      this.trabalhistaService.json.empresaGrupo = [] :
      this.trabalhistaService.json.empresaGrupo = empresa;
  }

  limpar() {
    this.listaEmpresas = [];

  }
}
