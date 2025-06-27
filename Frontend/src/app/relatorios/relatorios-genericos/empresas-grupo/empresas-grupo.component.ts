import { Component, OnInit, DoCheck, AfterContentInit } from '@angular/core';
import { RelatorioService } from 'src/app/core/services/relatorio.service';
import { DualListModel } from 'src/app/core/models/dual-list.model';

@Component({
  selector: 'app-empresas-grupo',
  templateUrl: './empresas-grupo.component.html',
  styleUrls: ['./empresas-grupo.component.scss']
})
export class EmpresasGrupoComponent implements OnInit , AfterContentInit, DoCheck {

  constructor(private reatorioService: RelatorioService) { }

  listaEmpresa: Array<DualListModel> = [];

  ngDoCheck(): void {
  }

  ngAfterContentInit(): void {
  }

   ngOnInit() {
    this.listaEmpresa = this.reatorioService.getListaEmpresasGrupo();
  }

  mostraSelecionados(event) {
    const lista = event.map(teste => {
      return teste.id;
    });
    this.reatorioService.AddlistaEmpresas(lista);
    this.reatorioService.atualizaCountEmpresaGrupo(event.length);

  }

}
