import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormControl } from '@angular/forms';
import { NgbDateAdapter, NgbDateNativeAdapter, NgbDateParserFormatter, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from '@shared/dateformat';
import { merge } from 'rxjs';
import { switchMap } from 'rxjs/operators';
@Component({
  selector: 'app-data-range',
  templateUrl: './data-range.component.html',
  styleUrls: ['./data-range.component.scss'],
  // tslint:disable-next-line: max-line-length
  providers: [{ provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }, { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }]
})
export class DataRangeComponent implements OnInit {
  @Input() label: string;
  @Input() dataInicio: any;
  @Input() modoVisualizar = false;
  @Input() dataFim: any;
  @Input() exibirBotaoBuscar = false;
  @Input() habilitarBotaoBuscar: boolean;
  @Input() showWeekNumbers: any;
  @Output() botaoBuscarData = new EventEmitter<any>();
  @Output() retDataInicio: EventEmitter<string> = new EventEmitter<string>();
  @Output() retDataFim: EventEmitter<Date> = new EventEmitter<Date>();
  @Output() rangeValido: EventEmitter<boolean> = new EventEmitter<boolean>();

  mostrarSemanas = false;
  emitir = false;

  ress: any;
  dtInicio = {
    data: null,
    erro: false,
    msg: ''
  };
  dtFim = {
    data: null,
    erro: false,
    msg: ''
  };
  constructor() { }

  resetaDatas() {
    // tslint:disable-next-line: triple-equals
      this.dtInicio.erro = false;
      this.dtInicio.msg = '';
      this.dtFim.erro = false;
      this.dtFim.msg = '';
    // tslint:disable-next-line: triple-equals

    }


  ngOnChanges() {
    this.dtInicio.data = this.dataInicio;
    this.dtFim.data = this.dataFim;
  }

  ngOnInit() {
    this.dtInicio.data = this.dataInicio;
    this.dtInicio.erro = false;
    this.dtInicio.msg = '';
    this.dtFim.data = this.dataFim;
    this.dtFim.erro = false;
    this.dtFim.msg = '';

    setInterval(() => {
      if (this.dtFim.data == null && this.dtInicio.data == null) {
        this.dtInicio.erro = false;
        this.dtInicio.msg = '';
        this.dtFim.erro = false;
        this.dtFim.msg = '';
        this.retDataInicio.emit(this.dtInicio.data);
        this.retDataFim.emit(this.dtFim.data);
        this.rangeValido.emit(true);
      }
      this.retDataInicio.emit(this.dtInicio.data);
      this.retDataFim.emit(this.dtFim.data);
    }, 0);
  }
  cpfCnpjMask() {
    return '00/00/0000';
  }
  validarData(data: any) {

    if (data.data == null) {
      if (this.dtInicio.data == null) {
        this.retDataInicio.emit(this.dtInicio.data);
      }
      if (this.dtFim.data == null) {
        this.retDataFim.emit(this.dtFim.data);
      }

      return;
    }
    if (typeof (data.data) === 'string' && data.data != null) {
      data.erro = true;
      data.msg = 'Data Inválida.';
      this.rangeValido.emit(false);
      // return;
    }
    if (typeof (data.data) === 'object' && data.data != null) {
      if (data.data.getYear() < 0) {
        data.erro = true;
        data.msg = 'Data Inválida.';
        return;
      } else {

        data.erro = false;
        data.msg = '';

      }
    }

  }
  //
  retornaMinMax(variavel: Date) {
    if (variavel == null) {
      return;
    }
    if (typeof (variavel) === 'object') {
      return { year: variavel.getFullYear(), month: (variavel.getMonth() + 1), day: variavel.getDate() };
    }
  }
  verificarange() {
    this.resetaDatas();
    if (typeof(this.dtInicio) === 'object' && this.dtInicio.data != null) {
      if (new Date(this.dtInicio.data).getFullYear() < 1900) {
        this.dtInicio.erro = true;
        this.dtInicio.msg = 'Data Inválida.';
        this.rangeValido.emit(false);
        this.emitir = false;
      }
      if (this.dtFim.data == null) {
        this.dtFim.erro = true;
        this.dtFim.msg = 'Data fim deve ser preenchida.';
        this.rangeValido.emit(false);
        this.emitir = false;
      }
      if (typeof (this.dtFim.data) == 'object' && this.dtFim.data != null) {
        if (this.dtInicio.data > this.dtFim.data) {
          this.dtInicio.erro = true;
          this.dtInicio.msg = 'Data de início  não pode ser maior que data fim.';
          this.rangeValido.emit(false);
          this.emitir = false;
        }
      }
    }
    if (typeof(this.dtFim) === 'object' && this.dtFim.data != null) {
      if (new Date(this.dtFim.data).getFullYear() < 1900) {
        this.dtFim.erro = true;
        this.dtFim.msg = 'Data Inválida.';
        this.rangeValido.emit(false);
        this.emitir = false;
      }
      if (this.dtInicio.data == null) {
        this.dtInicio.erro = true;
        this.dtInicio.msg = 'Data início  deve ser preenchida.';
        this.rangeValido.emit(false);
        this.emitir = false;
      }
      if (typeof (this.dtInicio.data) === 'object' && this.dtInicio.data != null) {
        if (this.dtFim.data < this.dtInicio.data) {
          this.dtFim.erro = true;
          this.dtFim.msg = 'Data fim não pode ser menor que data início.';
          this.rangeValido.emit(false);
          this.emitir = false;
        }
      }
      if (typeof (this.dtInicio.data) === 'object'
        && this.dtInicio.data != null
        && typeof (this.dtFim.data) === 'object'
        && this.dtFim.data != null
        && (this.dtFim.data.getTime() == this.dtInicio.data.getTime()
          || this.dtFim.data > this.dtInicio.data)
         ) {
        this.dtInicio.erro = false;
        this.dtFim.erro = false;
        this.rangeValido.emit(true);
        this.emitir = true;
      }
    }
    return this.emitir;
  }

  aoAlterarDatas() {
    this.verificarange();

    if (this.dtInicio.data != null) {
      this.validarData(this.dtInicio.data);
    }
    if (this.dtFim.data != null) {
      this.validarData(this.dtFim.data);
    }

    if (this.dtFim.data == null && this.dtInicio.data == null) {
      this.dtInicio.erro = false;
      this.dtInicio.msg = '';
      this.dtFim.erro = false;
      this.dtFim.msg = '';
      this.retDataInicio.emit(this.dtInicio.data);
      this.retDataFim.emit(this.dtFim.data);
      this.rangeValido.emit(true);
    }

    // if (this.verificarange()) {
      this.retDataInicio.emit(this.dtInicio.data);
      this.retDataFim.emit(this.dtFim.data);
    // }
  }

  // Método @override para o click do botao buscar
  buscarData() {
    this.botaoBuscarData.emit(true);
  }
}
