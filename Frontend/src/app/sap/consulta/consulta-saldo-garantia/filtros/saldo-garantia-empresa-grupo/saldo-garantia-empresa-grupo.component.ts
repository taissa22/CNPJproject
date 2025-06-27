import { Component, OnInit } from '@angular/core';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { ListaFiltroRadioConsultaLoteEnum } from '@shared/enums/lista-filtro-radio-consulta-lote.enum';
import { ListaFiltroSaldoGarantiaRadio } from '@shared/enums/lista-filtro-saldo-garantia-radio.enum';
import { ConsultaFiltroEmpresaGrupoService } from '../../service/consulta-filtro-empresa-grupo.service';
import { ConsultaSaldoGarantiaService } from '../../service/consulta-saldo-garantia.service';

@Component({
  selector: 'app-saldo-garantia-empresa-grupo',
  templateUrl: './saldo-garantia-empresa-grupo.component.html',
  styleUrls: ['./saldo-garantia-empresa-grupo.component.scss']
})
export class SaldoGarantiaEmpresaGrupoComponent implements OnInit {

  listaEmpresaGrupo: Array<DualListModel> = [];

  constructor(private consultaService: ConsultaSaldoGarantiaService,
              private service: ConsultaFiltroEmpresaGrupoService) { }

  ngOnInit() {
    this.getListaEmpresaGrupo();
  }

  getListaEmpresaGrupo() {
    this.listaEmpresaGrupo = this.service.listaEmpresaGrupo;
  }

  mostraSelecionados(event) {
    let listaID = event.map(item => item.id)
 

    this.consultaService.atualizaCount(event.length,
      ListaFiltroSaldoGarantiaRadio.empresaGrupo);


      this.service.adicionarEmpresaDTO(listaID);
    // this.filterService.auxFornecedor = lista;
    // if(lista.length == this.listaFornecedor.length){
    //   this.filterService.AddlistaFornecedor(null);
    // }else{
    //   this.filterService.AddlistaFornecedor(lista);
    // }

    // this.sapService.atualizaCount(event.length,
    // ListaFiltroRadioConsultaLoteEnum.fornecedor);
  }

}
