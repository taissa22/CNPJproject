import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Subscription } from 'rxjs';
import { PagamentosTabService } from '../../../services/pagamentos-tab.service';
import { EstornoAbasService } from './../../../services/estorno-abas.service';

@Component({
  selector: 'estorno-pagamentos-tab',
  templateUrl: './pagamentos-tab.component.html',
  styleUrls: ['./pagamentos-tab.component.scss']
})
export class PagamentosTabComponent implements OnInit {

  pagamentos: any[];
  @Output() pagamentoSelecionado = new EventEmitter();
  pagamentosKeys: string[];

  //#region Subscriptions
  subscription: Subscription;
  //#endregion

  constructor(private service: PagamentosTabService, private abaService: EstornoAbasService) { }

  ngOnInit() {
   this.subscription = this.abaService.pagamentos.subscribe(item => {
     this.pagamentos = item;
     this.service.setPagamentos(this.pagamentos);
  });


    this.subscription.add(this.service.pagamentoSelecionadaSubject.subscribe(selecionada => this.pagamentoSelecionado.emit({
      'isSelecionada': Object.keys(selecionada).length > 0,
      'value': selecionada
      })));
    this.subscription.add(this.service.pagamentosKeysSubject.subscribe(pagamentosKeys => this.pagamentosKeys = pagamentosKeys));
    this.subscription.add(this.service.pagamentosSubject.subscribe(pagamentos => this.pagamentos = pagamentos));
    this.pagamentos.forEach(pagamento => pagamento.selected = false);
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  onClickPagamento(pagamento) {
    this.service.setPagamentoSelecionada(pagamento);
  }

}
