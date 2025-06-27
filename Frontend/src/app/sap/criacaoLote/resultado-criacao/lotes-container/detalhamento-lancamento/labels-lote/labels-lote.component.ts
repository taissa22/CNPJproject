import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AbasContentService } from '../abasContent/abas-content.service';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'labels-lote',
  templateUrl: './labels-lote.component.html',
  styleUrls: ['./labels-lote.component.scss']
})
export class LabelsLoteComponent implements OnInit {

  public lancamentos = [];
  valorLote = 0;
  private _subscription: Subscription;

  constructor(private abasContentService: AbasContentService) { }

  ngOnInit() {
    this._subscription = this.abasContentService.lancamentosSelecionados
      .subscribe(lancamentos => {
        {
          if (lancamentos) {
            this.lancamentos = lancamentos;
          } else {
            this.lancamentos = [];
          }
        }
      });

    this._subscription.add(this.abasContentService.totalValorLiquidoSelecionados.subscribe(total => this.valorLote = total || 0));
  }

  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.
    this._subscription.unsubscribe();
  }

}
