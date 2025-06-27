import { DetalheResultadoService } from './../../../../core/services/sap/detalhe-resultado.service';
import { DetalhamentoResultadoLote } from './../../../../core/models/detalhamento-resultado-lote';
import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { Observable } from 'rxjs';

import { FilterService } from '../../services/filter.service';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'detalhamento-resultado',
  templateUrl: './detalhamento-resultado.component.html',
  styleUrls: ['./detalhamento-resultado.components.scss']
})
export class DetalhamentoResultadoComponent implements OnInit, OnDestroy {

  @Input() borderoOn: string;

  public borderoExist: boolean;

  public detalheResultado: Observable<DetalhamentoResultadoLote>;

  public detalhamentoSubscription;

  constructor(private detalhamentoService: DetalheResultadoService, private filterService: FilterService) { }

  sinalTrocou: boolean;

  public temPermissao: boolean;

  public exibirLoteBB: number;

  ngOnInit() {

    this.borderoExist = (this.borderoOn === null);

    this.detalhamentoService.sinalChange.subscribe(item => {
      this.sinalTrocou = item
    });

    this.detalhamentoService.objectReceived.subscribe(obj => {

      if (obj) {
        obj.isOpen = false;
      }
    });
    this.detalhamentoSubscription = this.detalhamentoService.itemReceived
      .subscribe(lote => {
        if (this.sinalTrocou === true) { this.getValoresDetalhe(lote); }
      });

      this.temPermissao = this.detalhamentoService.verificarPermissaoGerarLoteBB();

      this.detalhamentoService.newLoteBB.subscribe(loteBB => {
        if(loteBB != null){
          this.exibirLoteBB = loteBB;
        }
      })

    


  }

  getValoresDetalhe(lote: number) {
    this.detalheResultado = this.detalhamentoService.getDetalhamentoLote(lote);
    this.detalheResultado.subscribe(lote => this.exibirLoteBB = lote.numeroLoteBB);
  }

  ngOnDestroy(): void {
    this.detalhamentoSubscription.unsubscribe();
  }

  regerarLoteBB(numeroLote:any){
    this.detalhamentoService.regerarLoteBB(numeroLote);
  }




}
