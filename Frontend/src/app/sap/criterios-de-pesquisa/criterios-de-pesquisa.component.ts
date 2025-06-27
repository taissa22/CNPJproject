import { Component, OnInit, Input, ChangeDetectionStrategy, Output, EventEmitter } from '@angular/core';
import { NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { ordenateHeader } from '@shared/utils';
import { formatDate } from '@angular/common';
import { AngularCsv } from 'angular-csv-ext/dist/Angular-csv';

export interface Exportacao {
  criterio: string;
  valor: any[];
}

@Component({
  selector: 'app-criterios-de-pesquisa',
  templateUrl: './criterios-de-pesquisa.component.html',
  styleUrls: ['./criterios-de-pesquisa.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CriteriosDePesquisaComponent implements OnInit {

  constructor() { }

  @Input() listaSelecionados: any[];
  @Input() referenciaTooltip: NgbTooltip;
  @Input() ordenacaoColuna: string[];
  @Input() origem: string;
  @Output() modalFechou = new EventEmitter();

  public nomeColuna;

  criterios: Exportacao[] = [{ criterio: '', valor: [] }];

  ngOnInit() {
    this.inicializacaoColunas();
  }

  inicializacaoColunas() {
    this.listaSelecionados = this.deleteEmpty(this.listaSelecionados);
    this.nomeColuna = Object.keys(this.listaSelecionados);
    this.ordenacaoColuna ?
      this.nomeColuna = ordenateHeader(this.nomeColuna, this.ordenacaoColuna) : '';

  }


  private deleteEmpty(array) {
    Object.keys(array).forEach(key => {
      if (!array[key] || (Array.isArray(array[key]) &&
        !(array[key].length > 0))) {
        delete array[key];
      }
    })
    return array;
  }

  isArray(obj: any) {
    return Array.isArray(obj);
  }

  exportarFiltros() {
    const options = {
      fieldSeparator: ';',
      quoteStrings: '"',
      decimalSeparator: '.',
      showLabels: true,
      showTitle: false,
      useTextFile: false,
      useBom: true,
      useKeysAsHeaders: false,
      headers: ['Critérios de Pesquisa', 'Valor']
    };
    // tslint:disable-next-line: no-unused-expression
    new AngularCsv(this.criterios,
      'Criterio_de_ Pesquisa_' + this.origem +
      formatDate(new Date(), 'yyyyMMdd_hhmmss', 'pt_BR'), options);
  }

  trackByFn(index, item) {
    return index; // or item.id
  }

  valoresExportacao(valor, nomelista) {
    if (this.criterios.length <= this.nomeColuna.length) {
      this.criterios.push({
        criterio: nomelista,
        valor: [this.listaSelecionados[valor]]
      });
    }

  }

  enviarEmitClickFechar() {
    this.modalFechou.emit();
  }
}
