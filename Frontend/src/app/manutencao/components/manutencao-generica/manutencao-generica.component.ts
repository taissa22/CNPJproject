import { Component, OnInit, ViewChildren, QueryList, Input, Output, EventEmitter } from '@angular/core';
import { FormControl } from '@angular/forms';
import { NgbdSortableHeader, SortEvent } from '@shared/directive/sortable.directive';
import { debug } from 'console';
import { ApurarValorCorteOutliersJuizadoEspecialComponent } from 'src/app/fechamento/contigencia/apurar-valor-corte-outliers-juizado-especial/apurar-valor-corte-outliers-juizado-especial.component';

@Component({
  selector: 'app-manutencao-generica',
  templateUrl: './manutencao-generica.component.html',
  styleUrls: ['./manutencao-generica.component.scss']
})
export class ManutencaoGenericaComponent implements OnInit {
  // One-way binding
  @Input() titulo: string;
  @Input() breadcrump: string;
  @Input() colunas: Array<any>;
  @Input() registros: Array<any>;
  @Input() totalDeRegistros: number;
  @Input() urlParaExportacao: string;
  @Input() tituloLista: string;
  @Input() tituloAdicionar: string;
  @Input() exibirAdicionar: boolean = true;
  @Input() exibirPesquisar: boolean = false;
  @Input() nomeCampoPesquisado : string;

  get hasInfo() {
    if (this.exibirPesquisar){
      return true;
    }

    if (this.registros){
      return (this.registros.length > 0 || this.buscarDescricaoFormControl.value != '');
    }
    else
    {
      return false;
    }    
  }

  // Two-way binding
  @Input() pagina: number = 1;
  @Output() paginaChange: EventEmitter<number> = new EventEmitter<number>();
  @Input() ordenacaoColuna: string = '';
  @Output() ordenacaoColunaChange: EventEmitter<string> = new EventEmitter<string>();
  @Input() ordenacaoDirecao: string = '';
  @Output() ordenacaoDirecaoChange: EventEmitter<string> = new EventEmitter<string>();
  @Input() totalDeRegistrosPorPagina: number;
  @Output() totalDeRegistrosPorPaginaChange: EventEmitter<number> = new EventEmitter<number>();
  
  @Input() textopesquisa: string = '';
  @Output() textopesquisaChange: EventEmitter<string> = new EventEmitter<string>();

  // Events
  @Output() aoClicarNovoRegistro = new EventEmitter();
  @Output() aoClicarEditarRegistro = new EventEmitter<any>();
  @Output() aoRemoverRegistro = new EventEmitter<any>();
  @Output() aoCarregarRegistros = new EventEmitter();  
  @Output() aoClicarExportar = new EventEmitter<any>();


  buscarDescricaoFormControl : FormControl = new FormControl(null);

  @ViewChildren(NgbdSortableHeader) headers: QueryList<NgbdSortableHeader>;
  listaQuantidadeDeRegistrosPorPagina: Array<number> = [ 8, 15, 35, 50 ];

  constructor() { }

  ngOnInit() { }

  novo() {
    this.aoClicarNovoRegistro.emit();
  }

  aoEditar(id: any) {
    this.aoClicarEditarRegistro.emit(id);
  }

  aoRemover(id: any) {
    this.aoRemoverRegistro.emit(id);
  }

  aoExportar(){
    this.aoClicarExportar.emit();
  }

  onSort({column, direction}: SortEvent) {
    // resetting other headers
    this.headers.forEach(header => {
      if (header.sortable !== column) {
        header.direction = '';
      }
    });

    this.ordenacaoColuna = column;
    this.ordenacaoDirecao = direction;
    if (direction.length === 0) {
      this.ordenacaoColuna = '';
    }

    this.ordenacaoColunaChange.emit(this.ordenacaoColuna);
    this.ordenacaoDirecaoChange.emit(this.ordenacaoDirecao);
    
  
    this.carregarDados();    
  }

  onPageChange() {
    
    this.paginaChange.emit(this.pagina);

    if (this.exibirPesquisar){
      this.textopesquisaChange.emit(this.textopesquisa);
    }
    
    this.carregarDados();    
  }

  exibirDirecao(ordenacaoColunaEsperada: string, ordenacaoDirecaoEsperada: string) {
    return this.ordenacaoColuna === ordenacaoColunaEsperada && this.ordenacaoDirecao === ordenacaoDirecaoEsperada;
  }

  exibirPaginacao() {
    return this.totalDeRegistros > this.totalDeRegistrosPorPagina;
  }

  alterarTotalDeRegistros() {
    
    this.pagina = 1;
    this.paginaChange.emit(this.pagina);

    this.totalDeRegistrosPorPaginaChange.emit(this.totalDeRegistrosPorPagina);
    this.carregarDados();     
  }

  carregarDados() {      
    this.textopesquisa = this.buscarDescricaoFormControl.value;

    if (this.exibirPesquisar){
      this.textopesquisaChange.emit(this.textopesquisa);
    }

    this.aoCarregarRegistros.emit();
  }  
  
  onClearInputPesquisar(){
     
    if (this.exibirPesquisar){
      this.textopesquisa = null;
      this.textopesquisaChange.emit(this.textopesquisa);
    }

    this.carregarDados();   
  }
}
