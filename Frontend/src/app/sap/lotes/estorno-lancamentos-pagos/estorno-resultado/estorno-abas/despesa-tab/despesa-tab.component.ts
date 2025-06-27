import { Component, OnInit, OnDestroy, Input, Output, EventEmitter } from '@angular/core';
import { Subscription } from 'rxjs';
import { DespesasTabService } from '../../../services/despesas-tab.service';
import { EstornoAbasService } from './../../../services/estorno-abas.service';

@Component({
  selector: 'estorno-despesa-tab',
  templateUrl: './despesa-tab.component.html',
  styleUrls: ['./despesa-tab.component.scss']
})
export class DespesaTabComponent implements OnInit, OnDestroy {

  despesas: any[];
  @Output() despesaSelecionada = new EventEmitter();
  despesasKeys: string[];

  //#region Subscriptions
  subscription: Subscription;
  //#endregion

  constructor(private service: DespesasTabService, private abaService: EstornoAbasService) { }

  ngOnInit() {
    this.subscription = this.abaService.despesas.subscribe(item =>{
      this.despesas = item;
      this.service.setDespesas(item);
    });

    this.subscription.add(this.service.despesaSelecionadaSubject.subscribe(selecionada => {
      this.despesaSelecionada.emit({
      'isSelecionada': Object.keys(selecionada).length > 0,
      'value': selecionada
      })
    }));
    this.subscription.add(this.service.despesasKeysSubject.subscribe(despesasKeys => this.despesasKeys = despesasKeys));
    this.subscription.add(this.service.despesasSubject.subscribe(despesas => this.despesas = despesas));
    this.despesas.forEach(despesa => despesa.selected = false);
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();

  }

  onClickDespesa(despesa) {
    this.service.setDespesaSelecionada(despesa);
  }

}
