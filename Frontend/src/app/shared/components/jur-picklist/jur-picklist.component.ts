import { AfterViewInit, Component, ElementRef, EventEmitter, Input, OnChanges, OnInit, Output, ViewChild } from '@angular/core';
import { itemSelecaoPicklist } from './models/itemSelecaoPicklist';
import * as _ from 'lodash';
import { DialogService } from '@shared/services/dialog.service';
import { PautaJuizadoComposicaoComponent } from 'src/app/agenda/pauta-juizado/composicao/pauta-juizado-composicao.component';

@Component({
  selector: 'jur-picklist',
  templateUrl: './jur-picklist.component.html',
  styleUrls: ['./jur-picklist.component.scss']
})
export class JurPicklistComponent implements OnInit {
  @ViewChild('divBotoes', { static: true }) divBotoes: ElementRef;
  @Input() titulo1: string = "";
  @Input() titulo2: string = "";
  @Input() height: number = 200;
  @Input() itensParaSelecao: Array<itemSelecaoPicklist> = [];
  @Input() manterOrdenacao = false;
  @Input() mostraSetaTodosOsItens = true;
  @Input() numeroMaximoSelecionados: number = -1;
  @Input() tipoProcesso: number = -1;
  itensSelecionados: Array<itemSelecaoPicklist> = [];
  @Output() itens = new EventEmitter<Array<itemSelecaoPicklist>>();
  itensParaSelecaoCount: number = 0;
  marginSetas = 0;
  todosTerceirizados: boolean = false;
  constructor(public dialog: DialogService) { }


  ngOnInit() {
    this.marginSetas = (this.height / 100) * 30;
    var interval = setInterval(() => {
      if (this.itensParaSelecao && this.itensParaSelecao.length) {
        this.marginSetas = (this.height / 100) * 30;
        this.contarItensParaSelecao();
        clearInterval(interval);
      }
    }, 100);
  }

  principal(id: string) {
    this.itensSelecionados.map(item => {
      if (item.principal == true) item.principal = false;
    });
    this.itensSelecionados.find(s => s.id == id).principal = true;
  }

  selecionarItensPorIds(ids: string[]) {
    this.limparItensSelecionados();
    ids.map(id => {
      this.itensParaSelecao.map(item => {
        if (item.id == id) item.marcado = true;
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
    let msg :string = 'Não pode ser feita a alocação de prepostos, pois todas as audiências deste juizado nesta data estão terceirizadas ou Automática pelo App Preposto';


    if (this.tipoProcesso == 17){
      msg = 'Não pode ser feita a alocação de prepostos, pois todas as audiências deste juizado nesta data estão terceirizadas';
    }

    if(this.todosTerceirizados){
      return this.dialog.info(msg)
    }
    this.itensParaSelecao.filter(i => i.marcado).map(i => i.hide = true);

    let totalItensDaSelecaoAtual = this.itensParaSelecao.filter(i => i.marcado && (this.itensSelecionados.find(s => s.id == i.id) == undefined)).length;

    if (this.numeroMaximoSelecionados != -1 && (totalItensDaSelecaoAtual + this.itensSelecionados.length) > this.numeroMaximoSelecionados) {
      this.itensParaSelecao
        .filter(i => i.marcado && (this.itensSelecionados.find(s => s.id == i.id) == undefined))
        .map(i => {
          i.marcado = false;
          i.hide = false;
        });
      this.dialog.err(
        'Limite máximo de registros selecionáveis são ' + this.numeroMaximoSelecionados
      );
      return;
    }

    let marcados = this.itensParaSelecao.filter(i => i.marcado);

    this.itensParaSelecao = this.itensParaSelecao.filter(i => !i.marcado);

    for (let i = 0; i < marcados.length; i++) {
      if (!(this.itensSelecionados.find(s => s.id == marcados[i].id))) {
        marcados[i].marcado = false;
        this.itensSelecionados.push(marcados[i])
      }
    }

    this.sortTable('Selecionados');
    this.itens.emit(this.itensSelecionados);
    this.contarItensParaSelecao();
    this.ordenaItensSelecionadosConformeItensParaSelecao();
  }

  public limparItensMarcados() {
    this.itensSelecionados.filter(i => i.marcado)
      .map(i => {
        if (this.itensParaSelecao.find(s => s.id == i.id) != undefined) this.itensParaSelecao.find(s => s.id == i.id).hide = false;
      });

    let marcados = this.itensSelecionados.filter(i => i.marcado);

    this.itensSelecionados = this.itensSelecionados.filter(i => !i.marcado);

    for (let i = 0; i < marcados.length; i++) {
      if (!(this.itensParaSelecao.find(s => s.id == marcados[i].id))) {
        marcados[i].marcado = false;
        marcados[i].hide = false;
        this.itensParaSelecao.push(marcados[i])
      }
    }

    this.sortTable('paraSelecao');
    this.itens.emit(this.itensSelecionados);
    this.contarItensParaSelecao();
  }

  public limparItensSelecionados() : boolean {
    this.itensParaSelecao.map(i => {
      i.hide = false
      i.marcado = false;
    });

    for (let i = 0; i < this.itensSelecionados.length; i++) {
      if (!(this.itensParaSelecao.find(s => s.id == this.itensSelecionados[i].id))) {
        this.itensSelecionados[i].marcado = false;
        this.itensSelecionados[i].hide = false;
        this.itensParaSelecao.push(this.itensSelecionados[i])
      }
    }

    this.sortTable('paraSelecao');
    this.itensSelecionados = [];
    this.itens.emit(this.itensSelecionados);
    this.contarItensParaSelecao();
    return true;
  }

  sortTable(type: string) {
    if (type == 'paraSelecao') {
      this.itensParaSelecao.sort(function (a, b) {
        return a.descricao > b.descricao ? 1 : a.descricao < b.descricao ? -1 : 0;
      });
    } else {
      this.itensSelecionados.sort(function (a, b) {
        return a.descricao > b.descricao ? 1 : a.descricao < b.descricao ? -1 : 0;
      });
    }
  }

  public contarItensParaSelecao() {
    this.itensParaSelecaoCount = this.itensParaSelecao.filter(i => !i.hide).length;
  }

  public ordenaItensSelecionadosConformeItensParaSelecao() {

    if (!this.manterOrdenacao) return;

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
      let itemSelecionado = new itemSelecaoPicklist(itemLista.id, itemLista.descricao, false);
      itemSelecionado.hide = itemLista.hide;
      itemSelecionado.marcado = itemLista.marcado;
      this.itensSelecionados.push(itemSelecionado);
    });

  }

}
