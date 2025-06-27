import { Component, OnInit } from '@angular/core';
import { FilterService } from '../../services/filter.service';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { SapService } from 'src/app/core/services/sap/sap.service';
import { ListaFiltroRadioConsultaLoteEnum } from '@shared/enums/lista-filtro-radio-consulta-lote.enum';

@Component({
  selector: 'filtro-empresa-grupo',
  templateUrl: './filtro-empresa-grupo.component.html',
  styleUrls: ['./filtro-empresa-grupo.component.scss']
})
export class FiltroEmpresaGrupoComponent implements OnInit {

  constructor(private filterService: FilterService, private sapService: SapService) { }
  listaEmpresa: Array<DualListModel> = [];


  ngDoCheck(): void {
  }

  ngAfterContentInit(): void {
  }

  ngOnInit() {
    this.listaEmpresa = this.filterService.getListaEmpresasGrupo();
    

    this.listaEmpresa.filter(item1 =>
      this.filterService.auxEmpresaGrupo.includes(item1.id)).map(
        item => item.selecionado = true
      );


  }

  mostraSelecionados(event) {
    const lista = event.map(teste => {
      return teste.id;
    });
    this.filterService.auxEmpresaGrupo = lista;
    if (lista.length == this.listaEmpresa.length) {
      this.filterService.AddlistaEmpresas(null);
    } else {
      this.filterService.AddlistaEmpresas(lista);
    }
    //this.filterService.AddlistaEmpresas(lista);
    this.sapService.atualizaCount(event.length,
      ListaFiltroRadioConsultaLoteEnum.empresaGrupo);

  }
}
