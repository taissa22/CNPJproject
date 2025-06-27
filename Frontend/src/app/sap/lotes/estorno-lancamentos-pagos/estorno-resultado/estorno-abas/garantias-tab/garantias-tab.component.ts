import { Component, OnInit, Input, OnDestroy, Output, EventEmitter } from '@angular/core';
import { GarantiasTabService } from '../../../services/garantias-tab.service';
import { Subscription } from 'rxjs';
import { EstornoAbasService } from './../../../services/estorno-abas.service';

@Component({
  selector: 'estorno-garantias-tab',
  templateUrl: './garantias-tab.component.html',
  styleUrls: ['./garantias-tab.component.scss']
})
export class GarantiasTabComponent implements OnInit, OnDestroy {
  garantias: any[];
  @Output() garantiaSelecionada = new EventEmitter();
  garantiasKeys: string[];

  //#region Subscriptions
  subscription: Subscription;
  //#endregion

  constructor(private service: GarantiasTabService, private abaService: EstornoAbasService) { }

  ngOnInit() {

    this.subscription = this.abaService.garantias.subscribe(item => {
      this.garantias = item;
      this.service.setGarantias(this.garantias);
    });

    this.service.setGarantias(this.garantias);

    this.subscription.add(this.service.garantiaSelecionadaSubject.subscribe(selecionada => this.garantiaSelecionada.emit({
      'isSelecionada': Object.keys(selecionada).length > 0,
      'value': selecionada
      })));
     this.subscription.add(this.service.garantiasKeysSubject.subscribe(garantiasKeys => this.garantiasKeys = garantiasKeys));
     this.subscription.add(this.service.garantiasSubject.subscribe(garantias => this.garantias = garantias));
    this.garantias.forEach(garantia => garantia.selected = false);
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  onClickGarantia(garantia) {
    this.service.setGarantiaSelecionada(garantia);
  }
}
