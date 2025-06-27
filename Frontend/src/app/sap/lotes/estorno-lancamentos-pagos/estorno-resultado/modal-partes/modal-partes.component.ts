import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { EstornoResultadoService } from '../../services/estorno-resultado.service';
import { Enumerable } from 'linqts';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';
import { EstornoLancamentosPagosService } from '../../services/estorno-lancamentos-pagos.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'modal-partes',
  templateUrl: './modal-partes.component.html',
  styleUrls: ['./modal-partes.component.scss']
})
export class ModalPartesComponent implements OnInit {
  reclamantes = [];
  reclamadas = [];
  titulo = 'Partes do Processo'
  subTitulo = 'Consulta dos dados dos autores e réus vinculados ao processo.'
  subscription: Subscription;
  constructor(public bsModalRef: BsModalRef,
    private estornoResultadoService: EstornoResultadoService,
   private estornoLancamentosService: EstornoLancamentosPagosService) { }

  ngOnInit() {
    this.subscription = this.estornoResultadoService.getProcessoSelecionado()
    .subscribe( processo => {
      this.reclamantes = processo['reclamantes'];
    this.reclamadas = processo['reclamadas'];
    }
    )


    if (this.estornoLancamentosService.currentItemComboboxTipoProcesso.value
      == TipoProcessoEnum.trabalhista) {
      this.tituloReclamandas = 'Reclamadas';
      this.tituloReclamanetes = 'Reclamantes';
    }

  }

  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.
    this.subscription.unsubscribe();
  }

  tituloReclamanetes = 'Autores'
  tituloReclamandas = 'Réus'

}
