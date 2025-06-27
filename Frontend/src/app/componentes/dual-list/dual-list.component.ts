
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { Component, OnChanges, OnInit, Input, Output, EventEmitter, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'app-dual-list',
  templateUrl: './dual-list.component.html',
  styleUrls: ['./dual-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DualListComponent implements OnInit, OnChanges {

  @Input() dualList: Array<DualListModel>;

  @Input() label: string;

  @Input() habilitaCheck: false;

  @Input() habilitaSelecionadas = false;

  @Output() listaSelecionados: EventEmitter<any[]> = new EventEmitter<any[]>();

  @Output() BotaoClicado: EventEmitter<string[]> = new EventEmitter<string[]>();


  ultimoSelecionado = -1;

  constructor() { }



  marcarItem(item: any, index: number, event: any) {
    item.marcado = !item.marcado;
    console.log(item);

    if (event.shiftKey) {

      if (this.ultimoSelecionado >= 0) {
        let select = false;
        let min;
        let max;
        if (this.ultimoSelecionado > index) {
          min = index;
          max = this.ultimoSelecionado
        } else {
          min = this.ultimoSelecionado
          max = index;
        }

        this.dualList.forEach((element, position) => {

          if (position >= min && position <= max){
            element.marcado = true;
          }
        });

      } else {
        this.ultimoSelecionado = index;
      }

    } else {
      if (item.marcado) {
        this.ultimoSelecionado = index;
      } else {
        this.ultimoSelecionado = -1
      }
    }

  }

  selecionarTodos() {
    this.dualList.forEach(item => {
      item.selecionado = true;
      item.marcado = false;

    });
    this.emitirLista();
  }
  selecionar() {
    this.dualList.forEach((item) => {
      if (item.marcado === true) {

        item.marcado = false;

        item.selecionado = true;

        this.dualList.map(x => {
          // tslint:disable-next-line: curly
          if (x.somenteLeitura && x.codigoChave == item.codigoChave && x.clone == true)
            x.selecionado = true;

          if (x.somenteLeitura && x.codigoChave == item.codigoChave && x.clone == false) {
            // tslint:disable-next-line: triple-equals
            // tslint:disable-next-line: max-line-length
            let qtd = this.dualList.filter(y => y.somenteLeitura == false && (y.codigoChave == item.codigoChave && y.selecionado == false)).length;
            if (qtd < 1) {
              x.selecionado = true;
            }
          }
        });
      }
    });
    this.emitirLista();

  }
  desselecionar() {
    this.dualList.forEach(item => {
      if (item.marcado === true) {

        item.marcado = false;

        item.selecionado = false;

        this.dualList.map(x => {
          // tslint:disable-next-line: triple-equals
          if (x.somenteLeitura && x.codigoChave == item.codigoChave && x.clone == false) {
            x.selecionado = false;
          }

          if (x.somenteLeitura && x.codigoChave == item.codigoChave && x.clone == true) {
            // tslint:disable-next-line: triple-equals
            // tslint:disable-next-line: max-line-length
            let qtd = this.dualList.filter(y => y.somenteLeitura == false && (y.codigoChave == item.codigoChave && y.selecionado == true)).length;
            if (qtd < 1) {
              x.selecionado = false;
            }
          }
        });
      }
    });
    this.emitirLista();
  }
  desselecionarTodo() {
    this.dualList.forEach(item => {
      item.selecionado = false;
      item.marcado = false;

    });
    this.emitirLista();
  }

  emitirLista() {
    // tslint:disable-next-line: prefer-const
    let selecionados = this.dualList.filter(itens => {
      if (itens.selecionado === true && itens.somenteLeitura == false || itens.selecionado === true && itens.somenteLeitura == undefined) {
        return itens;
      }
    });
    this.listaSelecionados.emit(selecionados);
  }

  ocultarParteFixaQuandoNaoHouverItens() {
    this.dualList.map((x, index) => {
      if (x.somenteLeitura && x.clone == false) {
        let qtd = this.dualList.filter(y => (y.codigoChave == x.codigoChave)).length - 2; // a qtd de subitens que pertencem a uma categoria
        if (qtd == 0) {
          this.dualList.splice(index); // remove o item que não possui subitens
        }
      }
    });
  }

  ngOnInit() {
    // this.emitirLista();
  }

  ngOnChanges() {
    this.ocultarParteFixaQuandoNaoHouverItens();
  }
}
