import {
  Component,
  OnInit,
  ViewChildren,
  QueryList,
  Input,
  Output,
  EventEmitter
} from '@angular/core';
import {
  NgbdSortableHeader,
  SortEvent
} from '@shared/directive/sortable.directive';

@Component({
  selector: 'app-relatorio-generica',
  templateUrl: './relatorio-generica.component.html',
  styleUrls: ['./relatorio-generica.component.scss']
})
export class RelatorioGenericaComponent implements OnInit {
  // One-way binding
  @Input() titulo: string;
  @Input() breadcrump: string;
  @Input() colunas: Array<any>;
  @Input() registros: Array<any>;
  @Input() totalDeRegistros: number;
  @Input() urlParaExportacao: string;
  @Input() tituloLista: string;
  @Input() tituloAdicionar: string;
  get hasInfo() {
    return this.registros ? this.registros.length > 0 : false;
  }

  // Two-way binding
  @Input() pagina: number = 1;
  @Output() paginaChange: EventEmitter<number> = new EventEmitter<number>();
  @Input() ordenacaoColuna: string = '';
  @Output() ordenacaoColunaChange: EventEmitter<string> =
    new EventEmitter<string>();
  @Input() ordenacaoDirecao: string = '';
  @Output() ordenacaoDirecaoChange: EventEmitter<string> =
    new EventEmitter<string>();
  @Input() totalDeRegistrosPorPagina: number;
  @Output() totalDeRegistrosPorPaginaChange: EventEmitter<number> =
    new EventEmitter<number>();

  // Events
  @Output() aoClicarNovoRegistro = new EventEmitter();
  @Output() aoClicarEditarRegistro = new EventEmitter<any>();
  @Output() aoRemoverRegistro = new EventEmitter<any>();
  @Output() aoCarregarRegistros = new EventEmitter();
  @Output() aoClicarExportar = new EventEmitter<any>();

  @ViewChildren(NgbdSortableHeader) headers: QueryList<NgbdSortableHeader>;
  listaQuantidadeDeRegistrosPorPagina: Array<number> = [8, 15, 35, 50];

  constructor() {}

  ngOnInit() {}

  novo() {
    this.aoClicarNovoRegistro.emit();
  }

  aoEditar(id: any) {
    this.aoClicarEditarRegistro.emit(id);
  }

  aoRemover(id: any) {
    this.aoRemoverRegistro.emit(id);
  }

  aoExportar() {
    this.aoClicarExportar.emit();
  }

  onSort({ column, direction }: SortEvent) {
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
    this.carregarDados();
  }

  exibirDirecao(
    ordenacaoColunaEsperada: string,
    ordenacaoDirecaoEsperada: string
  ) {
    return (
      this.ordenacaoColuna === ordenacaoColunaEsperada &&
      this.ordenacaoDirecao === ordenacaoDirecaoEsperada
    );
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
    this.aoCarregarRegistros.emit();
  }
}
