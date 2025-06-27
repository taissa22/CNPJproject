import { Paginacao } from './model/paginacao.model';
import { Coluna } from './model/coluna.model';
import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-tabela-ordenacao-multi-colunas',
  templateUrl: './tabela-ordenacao-multi-colunas.component.html',
  styleUrls: ['./tabela-ordenacao-multi-colunas.component.scss']
})
export class TabelaOrdenacaoMultiColunasComponent {
  @Input() colunas: Array<Coluna>;
  @Input() colunasParaOrdenacao: Array<any>;
  @Input() lista: Array<any>;
  @Input() totalDeRegistros: number;

  @Input() itemSelecionado: any;
  @Output() itemSelecionadoChange: EventEmitter<any> = new EventEmitter<any>();

  @Input() paginaAtual: number;
  @Output() paginaAtualChange: EventEmitter<number> = new EventEmitter<number>();

  @Input() totalDeRegistrosPorPagina: number;
  @Output() totalDeRegistrosPorPaginaChange: EventEmitter<number> = new EventEmitter<number>();

  @Output() aoAlterarPagina: EventEmitter<any> = new EventEmitter();

  quantidadeRegistrosPorPagina: Array<number> = [8, 15, 35, 50];

  constructor() { }

  selecionar(item: any) {
    this.itemSelecionado = this.itemSelecionado !== item ? item : null;
    this.itemSelecionadoChange.emit(this.itemSelecionado);
  }

  onPageChange() {
    this.paginaAtualChange.emit(this.paginaAtual);
    this.aoAlterarPagina.emit();
  }

  alterarQuantidadePorPagina() {
    this.totalDeRegistrosPorPaginaChange.emit(this.totalDeRegistrosPorPagina);

    this.paginaAtual = 1;
    this.paginaAtualChange.emit(this.paginaAtual);
    this.aoAlterarPagina.emit();
  }

  exibirPaginacao() {
    return this.totalDeRegistros > this.totalDeRegistrosPorPagina;
  }
}
