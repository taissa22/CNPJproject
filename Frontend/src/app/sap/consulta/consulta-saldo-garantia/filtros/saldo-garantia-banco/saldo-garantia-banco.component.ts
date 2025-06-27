import { DualListModel } from 'src/app/core/models/dual-list.model';
import { Component, OnInit } from '@angular/core';
import { ConsultaSaldoGarantiaService } from '../../service/consulta-saldo-garantia.service';
import { ConsultaSaldoGarantiaBancoService } from '../../service/consulta-saldo-garantia-banco.service';

@Component({
  selector: 'app-saldo-garantia-banco',
  templateUrl: './saldo-garantia-banco.component.html',
  styleUrls: ['./saldo-garantia-banco.component.scss']
})
export class SaldoGarantiaBancoComponent implements OnInit {

  listaBanco: Array<DualListModel> = [];

  constructor(private consultaService: ConsultaSaldoGarantiaService,
    private service:ConsultaSaldoGarantiaBancoService) { }

  ngOnInit() {
    this.getListaBanco();
  }

  getListaBanco(){
    this.listaBanco = this.service.listaBanco;
  
  }

  mostraSelecionados(event) {
    let listaID = event.map(item => item.id)
    // const lista = event.map(teste => {
    //   return teste.id;
    // });

    // this.service.auxFornecedor = lista;
    // if(lista.length == this.listaBanco.length){
    //   this.filterService.AddlistaFornecedor(null);
    // }else{
    //   this.filterService.AddlistaFornecedor(lista);
    // }
    
    this.consultaService.atualizaCount(event.length,2)

    this.service.adicionarBancoDTO(listaID);
    
    //ListaFiltroRadioConsultaLoteEnum.fornecedor);
  }

}
