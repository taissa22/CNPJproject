import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-vertical-dual-list',
  templateUrl: './vertical-dual-list.component.html',
  styleUrls: ['./vertical-dual-list.component.scss']
})
export class VerticalDualListComponent implements OnInit {

  @Input() nomeContexto: string;
  @Input() headerAdicionados: any[] = null;
  @Input() headers: any[];
  @Input() itens: any[];
  @Input() artigo = 'o';
  @Input() botaoHabilitado = true;

  @Output() itensSelecionados = new EventEmitter<any[]>();
  @Output() itensAdicionados = new EventEmitter<any[]>();

  constructor() { }

  ngOnInit() {
    if(!this.headerAdicionados)
      this.headerAdicionados = [...this.headers];
    this.itens.map(e => {
      if (!e.hasOwnProperty('adicionado')) {
        e['adicionado'] = false;
      }
      e['selecionado'] = false;
    });
  }

  recuperarItensSelecionados() {
    return this.itens.filter(e => e['selecionado']);
  }

  recuperarItensAdicionados() {
    return this.itens.filter(e => e['adicionado']);
  }

  selecionaItem(index) {
    this.itens[index]['selecionado'] = !this.itens[index]['selecionado'];
    this.itensSelecionados.emit(this.recuperarItensSelecionados());
  }

  adicionaItens() {
    this.itens.map(item => {
      if(item['selecionado']) {
        item['adicionado'] = true;
        item['selecionado'] = false;
      }
    });

    this.itensAdicionados.emit(this.recuperarItensAdicionados());
  }

  removeItem(index) {
    this.itens[index]['adicionado'] = false;
    this.itensAdicionados.emit(this.recuperarItensAdicionados());

  }

  removeTodos() {
    this.itens.map(item => item['adicionado'] = false);
  }

  hasEmptyAdicionados() {
    return this.itens.every(item => !item['adicionado']);
  }

  hasEmptyNaoAdicionados() {
    return this.itens.filter(item => !item['adicionado']).length == 0

  }
}
