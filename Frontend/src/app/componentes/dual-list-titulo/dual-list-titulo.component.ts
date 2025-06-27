import { DualListTitulo } from './../../core/models/dual-list-titulo';
import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { DualListModel } from 'src/app/core/models/dual-list.model';
import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling';

@Component({
  selector: 'app-dual-list-titulo',
  templateUrl: './dual-list-titulo.component.html',
  styleUrls: ['./dual-list-titulo.component.scss']
})
export class DualListTituloComponent implements OnInit {
  @ViewChild(CdkVirtualScrollViewport, { read: CdkVirtualScrollViewport, static: true }) viewport: CdkVirtualScrollViewport;

  @Input() dualList: Array<DualListTitulo>;
  dualListActive: Array<DualListTitulo>;
  dualListInactive: Array<DualListTitulo>;
  @Input() label: string;

  @Output() listaSelecionados: EventEmitter<any[]> = new EventEmitter<any[]>();

  constructor() { }
  marcarItem(item: any) {
    item.marcado = !item.marcado;
  }

  selecionarTodos() {
    this.dualList.forEach(item => {
      item.dados.forEach(dados => {
        dados.selecionado = (dados.somenteLeitura) ? false : true;
        dados.marcado = false;

      });

    });
    this.emitirLista();
  }
  selecionar() {
    this.dualList.forEach(item => {

      item.dados.forEach(dados => {
        if (dados.marcado === true) {
          dados.marcado = false;
          dados.selecionado = true;
        }

      });

    });
    this.emitirLista();
  }
  desselecionar() {
    this.dualList.forEach(item => {

      item.dados.forEach(dados => {
        if (dados.marcado === true) {
          dados.marcado = false;
          dados.selecionado = false;
        }

      });

    });
    this.emitirLista();
  }

  desselecionarTodo() {
    this.dualList.forEach(item => {


      item.dados.forEach(dados => {
        dados.selecionado = false;
        dados.marcado = false;
      });

    });
    this.emitirLista();
  }

  private pegarItensSelecionados() {
    return this.dualList.filter(itens => {


      itens.dados.forEach(dados => {
        if (dados.selecionado === true) {
          dados.marcado = false;
        }
      });
      return itens;
    });
  }

  private pegarItensDesselecionados() {

    return this.dualList.filter(itens => {

      itens.dados.forEach(dados => {
        if (dados.selecionado === false) {
          dados.marcado = false;
        }
      });
      return itens;


    });
  }
  emitirLista() {
    // tslint:disable-next-line: prefer-const
    let selecionados = this.pegarItensSelecionados();
    this.dualListActive = selecionados;
    this.dualListInactive = this.pegarItensDesselecionados();
    this.listaSelecionados.emit(selecionados);
  }
  ngOnInit() {
    // this.emitirLista();
    this.dualListInactive = this.dualList;
    this.dualListActive = this.dualList;
  }


  verificarExibicaoTitulo(selecionado: boolean, id: number) {
    let exibir = false
    this.dualListInactive.forEach(item => {

      if (item.id == id) {
        item.dados.forEach(dados => {
          dados.selecionado ? selecionado? exibir = true : exibir : selecionado? exibir : exibir = true
         })

      }

    });
    return exibir

  }



}

