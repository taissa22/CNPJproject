import { Component, OnInit } from '@angular/core';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { ListaFiltroRadioConsultaLoteEnum } from '@shared/enums/lista-filtro-radio-consulta-lote.enum';
import { ListaFiltroSaldoGarantiaRadio } from '@shared/enums/lista-filtro-saldo-garantia-radio.enum';
import { ConsultaSaldoGarantiaService } from '../../service/consulta-saldo-garantia.service';
import { ConsultaSaldoGarantiaEstadoService } from '../../service/consulta-saldo-garantia-estado.service';

@Component({
  selector: 'app-saldo-garantia-estado',
  templateUrl: './saldo-garantia-estado.component.html',
  styleUrls: ['./saldo-garantia-estado.component.scss']
})
export class SaldoGarantiaEstadoComponent implements OnInit {


  listaEstado: Array<DualListModel> = [];

  constructor(private consultaService: ConsultaSaldoGarantiaService,
    private service: ConsultaSaldoGarantiaEstadoService) { }


  ngOnInit() {
    this.getListaEstado();
  }

  getListaEstado() {
    this.listaEstado = this.service.listaEstado;

  }

  mostraSelecionados(event) {
    let listaID = event.map(item => item.id)

    this.consultaService.atualizaCount(event.length,
      ListaFiltroSaldoGarantiaRadio.estado);

      this.service.adicionarEstadoDTO(listaID);
  }

}
