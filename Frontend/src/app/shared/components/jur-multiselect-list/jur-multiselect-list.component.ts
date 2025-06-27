import { AfterViewChecked, AfterViewInit, Component, ElementRef, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { itemSelecaoMultiSelectList } from './models/itemSelecaoMultiSelectList';
import * as _ from 'lodash';
import { Element } from '@angular/compiler';
@Component({
  selector: 'jur-multiselect-list',
  templateUrl: './jur-multiselect-list.component.html',
  styleUrls: ['./jur-multiselect-list.component.scss']
})
export class JurMultiselectListComponent implements OnInit  {
  @ViewChild('divBotoes', { static: true }) divBotoes: ElementRef;
  @Input() titulo1: string = "";
  @Input() titulo2: string = "";
  @Input() height: number = 200;
  @Input() itensParaSelecao: Array<itemSelecaoMultiSelectList> = [];
  @Input() manterOrdenacao  = false;
  itensSelecionados: Array<itemSelecaoMultiSelectList> = [];
  @Output() itens = new EventEmitter<Array<itemSelecaoMultiSelectList>>();
  itensParaSelecaoCount: number = 0;
  marginSetas = 0;
  constructor() { }

 
  ngOnInit() { 
    this.marginSetas = (this.height / 100) * 30; 
    var interval = setInterval(() => {
      if(this.itensParaSelecao && this.itensParaSelecao.length){
        this.marginSetas = (this.height / 100) * 30; 
        this.contarItensParaSelecao();
        clearInterval(interval);
      } 
    },100);
    
  }

  selecionarItensPorIds(ids : string[]){
    this.limparItensSelecionados(); 
    ids.map(id =>{
      this.itensParaSelecao.map(item => {
        if(item.id == id) item.marcado = true;
      });
    });
    this.selecionarItensMarcados();
    this.contarItensParaSelecao();
  }

  public selecionarTodosOsItens() {
    this.itensParaSelecao.map(i => {
      i.hide = true;
      i.marcado = false;
    });
    this.itensSelecionados = _.cloneDeep(this.itensParaSelecao);
    this.itens.emit(this.itensSelecionados);
    this.contarItensParaSelecao();
    this.ordenaItensSelecionadosConformeItensParaSelecao(); 
  }

  public selecionarItensMarcados() {
    this.itensParaSelecao.filter(i => i.marcado).map(i => i.hide = true);
    this.itensParaSelecao
      .filter(i => i.marcado && (this.itensSelecionados.find(s => s.id == i.id) == undefined))
      .map(i => {
        i.marcado = false;
        this.itensSelecionados.push(_.cloneDeep(i));
      });
    this.itens.emit(this.itensSelecionados);
    this.contarItensParaSelecao();
    this.ordenaItensSelecionadosConformeItensParaSelecao();
  }
  public limparItensMarcados() {
    this.itensSelecionados.filter(i => i.marcado)
      .map(i => {
        if (this.itensParaSelecao.find(s => s.id == i.id) != undefined) this.itensParaSelecao.find(s => s.id == i.id).hide = false;
      });
    this.itensSelecionados = this.itensSelecionados.filter(i => !i.marcado);
    this.itens.emit(this.itensSelecionados);
    this.contarItensParaSelecao();
  }

  public limparItensSelecionados() {
    this.itensParaSelecao.map(i => {
      i.hide = false
      i.marcado = false;
    });
    this.itensSelecionados = [];
    this.itens.emit(this.itensSelecionados);
    this.contarItensParaSelecao();
  }
  public contarItensParaSelecao() { 
    this.itensParaSelecaoCount = this.itensParaSelecao.filter(i => !i.hide).length;
  }

  public ordenaItensSelecionadosConformeItensParaSelecao() {

    if(!this.manterOrdenacao) return;
  
    let listaOrdenada = [];

    this.itensSelecionados.map(itemSelecionado => {
      let itemParaSelecao = this.itensParaSelecao.find(i => i.id == itemSelecionado.id);
      listaOrdenada.push({
        index: this.itensParaSelecao.indexOf(itemParaSelecao),
        id: itemSelecionado.id,
        descricao: itemSelecionado.descricao,
        marcado: itemSelecionado.marcado,
        hide: itemSelecionado.hide
      })
    })

    listaOrdenada = listaOrdenada.sort(function (a, b) { 
        return a.index - b.index; 
    });

    this.itensSelecionados = [];
    listaOrdenada.map(itemLista => {
      let itemSelecionado = new itemSelecaoMultiSelectList(itemLista.id, itemLista.descricao);
      itemSelecionado.hide = itemLista.hide;
      itemSelecionado.marcado = itemLista.marcado;
      this.itensSelecionados.push(itemSelecionado);
    });

  }
}
