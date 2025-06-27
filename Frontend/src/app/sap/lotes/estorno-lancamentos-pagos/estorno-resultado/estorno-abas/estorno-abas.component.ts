import { Component, OnInit, Input } from '@angular/core';
import { EstornoAbasService } from '../../services/estorno-abas.service';
import { Subscription, BehaviorSubject } from 'rxjs';


@Component({
  selector: 'estorno-abas',
  templateUrl: './estorno-abas.component.html',
  styleUrls: ['./estorno-abas.component.scss']
})
export class EstornoAbasComponent implements OnInit {

  dadosLancamentos: any[];
  garantias = [];
  despesa = [];
  pagamentos = [];

  despesaSelecionada = null;
  pagamentoSelecionado = null;
  garantiaSelecionada = null;

  isDespesaSelecionada = false;
  isPagamentoSelecionado = false;
  isGarantiaSelecionada = false;
  //#region verificação das abas
  ifGarantia;
  ifDespesa;
  ifPagamento = true;
  //#endregion


  //#region Total valor Lançamentos
  // Soma dos valores de cada aba
  totalGarantias;
  totalDespesa;
  totalPagamentos;
  //#endregion

  //#region Total quantidade lançamentos
  totalQtdGarantias = new BehaviorSubject<number>(0);
  totalQtdDespesas = new BehaviorSubject<number>(0);
  totalQtdPagamentos = new BehaviorSubject<number>(0);
  //#endregion

  private subscription: Subscription;

  constructor(private service: EstornoAbasService) { }


  ngOnInit() {
    // Soma dos valores de cada aba

    this.subscription = (this.service.garantias.subscribe(item => {
      this.totalGarantias =
        item.reduce((total, obj) => obj['valor'] ? obj['valor'] + total : 0, 0)
      this.garantias = item;
      this.totalQtdGarantias.next(item.length);
    }));

    this.subscription.add(this.service.despesas.subscribe(item => {
      this.totalDespesa =
        item.reduce((total, obj) => obj['valor'] ? obj['valor'] + total : 0, 0)
      this.despesa = item;
      this.totalQtdDespesas.next(item.length);
    }));

    this.subscription.add(this.service.pagamentos.subscribe(item => {
      this.totalPagamentos =
        item.reduce((total, obj) => obj['valor'] ? obj['valor'] + total : 0, 0)
      this.pagamentos = item;
      this.totalQtdPagamentos.next(item.length);
    }
    ));
  }


  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }


  /**
   * Verificar qual aba foi aberta pelo usuário
   *
   * @param aba: nome da aba aberta. garantia para Garantias, despesa para
   * Despesas e pagamento para Pagamentos
   */
  verificarAbaAberta(aba: string) {
    this.service.limparSelecao();
    if (aba === 'garantia') {
      this.ifGarantia = true;
      this.ifDespesa = false;
      this.ifPagamento = false;
    } else if (aba === 'despesa') {
      this.ifDespesa = true;
      this.ifGarantia = false;
      this.ifPagamento = false;
    } else {
      this.ifPagamento = true;
      this.ifDespesa = false;
      this.ifGarantia = false;
    }
  }

  setPagamentoSelecionado($event) {
    this.pagamentoSelecionado = $event['value'];
    this.isPagamentoSelecionado = $event['isSelecionada']
  }

  setGarantiaSelecionado($event) {
    this.garantiaSelecionada = $event['value'];
    this.isGarantiaSelecionada = $event['isSelecionada']
  }

  setDespesaSelecionado($event) {
    this.despesaSelecionada = $event['value'];
    this.isDespesaSelecionada = $event['isSelecionada']
  }

  onClickEstornar() {
    if (this.ifDespesa) {

      this.service.estornar(this.despesaSelecionada).then(e => {
        if (e.value) {
          this.service.realizaEstorno(this.despesaSelecionada);

          this.despesa.forEach((item, index) => {
            if (item === this.despesaSelecionada) {
              this.despesa.splice(index, 1);
            }
          });

          this.service.despesas.next(this.despesa);

          this.totalQtdDespesas.next(this.despesa.length);
          this.totalDespesa = this.despesa.reduce((total, obj) => obj['valor'] ? obj['valor'] + total : 0, 0);
        }
      });

    } else if (this.ifGarantia) {
      this.service.estornar(this.garantiaSelecionada).then(e => {
        if (e.value) {
          this.service.realizaEstorno(this.garantiaSelecionada);

          this.garantias.forEach((item, index) => {
            if (item === this.garantiaSelecionada) {
              this.garantias.splice(index, 1);
            }
          });

          this.service.garantias.next(this.garantias);

          this.totalQtdGarantias.next(this.garantias.length);
          this.totalGarantias = this.garantias.reduce((total, obj) => obj['valor'] ? obj['valor'] + total : 0, 0);
        }
      });

    } else if (this.ifPagamento) {
      this.service.estornar(this.pagamentoSelecionado).then(e => {
        if (e.value) {
          this.service.realizaEstorno(this.pagamentoSelecionado);

          this.pagamentos.forEach((item, index) => {
            if (item === this.pagamentoSelecionado) {
              this.pagamentos.splice(index, 1);
            }
          });

          this.service.pagamentos.next(this.pagamentos);

          this.totalQtdPagamentos.next(this.pagamentos.length);
          this.totalPagamentos = this.pagamentos.reduce((total, obj) => obj['valor'] ? obj['valor'] + total : 0, 0);
          
        }
      });
    }
  }
}
