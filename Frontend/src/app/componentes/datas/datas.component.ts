import { Component, OnInit, Input, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { NgbDateAdapter, NgbDateNativeAdapter, NgbDateParserFormatter, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from '@shared/dateformat';

@Component({
  selector: 'app-datas',
  templateUrl: './datas.component.html',
  styleUrls: ['./datas.component.scss'],
  providers: [{ provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }, { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }]
})
export class DatasComponent implements OnInit {

  @Input() label: string;
  @Input() dataInicio: any;
  @Input() dataAterior: any;
  @Input() modoVisualizar = false;
  @Input() isDisabled = false;
  @Input() minDate = null;
  @Input() maxDate = null;
  @Output() retDataInicio: EventEmitter<string> = new EventEmitter<string>();


  emitir = false;

  ress: any;
  dtInicio = {
    data: null,
    erro: false,
    msg: ''
  };

  constructor() { }




  // ngOnChanges() {
  //   this.dtInicio.data = this.dataInicio;
  // }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['dataInicio']) {
      this.dtInicio.data = this.dataInicio ? new Date(Date.parse(this.dataInicio)) : null;
      if (!this.dataInicio) {
        this.retDataInicio.emit(null);  // Emite null quando resetado
      }
    }
  }
  

  ngOnInit() {
    this.dtInicio.data = this.dataInicio ? new Date(Date.parse(this.dataInicio)) : null;
    this.dtInicio.erro = false;
    this.dtInicio.msg = '';
  
    setInterval(() => {
      // Só emitir se a data não for nula, ou emitir nulo no caso de reset
      if (!this.dtInicio.data) {
        this.retDataInicio.emit(null);
      } else {
        this.retDataInicio.emit(this.dtInicio.data);
      }
    }, 1000);
  }

  cpfCnpjMask() {
    return '00/00/0000';
  }
  
  // validarData(data: any) {

  //   if (data.data == null) {
  //     if (this.dtInicio.data == null) {
  //       this.retDataInicio.emit(this.dtInicio.data);
  //     }


  //     return;
  //   }
  //   if (typeof (data.data) === 'string' && data.data != null) {
  //     data.erro = true;
  //     data.msg = 'Data Inválida.';

  //   }
  //   if (typeof (data.data) === 'object' && data.data != null) {
  //     if (data.data.getYear() < 0) {
  //       data.erro = true;
  //       data.msg = 'Data Inválida.';
  //       return;
  //     } else {

  //       data.erro = false;
  //       data.msg = '';

  //     }
  //   }


  // }

validarData(data: any) {
  if (data.data == null) {
    data.erro = false;  // Não marca como erro
    data.msg = '';
    this.retDataInicio.emit(null);  // Emite null para indicar reset
    return;
  }

  if (typeof data.data === 'string' || data.data.getYear() < 0) {
    data.erro = true;
    data.msg = 'Data Inválida.';
    this.retDataInicio.emit(null);  // Se a data for inválida, emitir null
  } else {
    data.erro = false;
    data.msg = '';
    this.retDataInicio.emit(data.data);  // Emite a data válida
  }
}

  

  retornaMinMax(variavel: Date) {
    if (variavel == null) {
      return;
    }
    if (typeof (variavel) === 'object') {
      return { year: variavel.getFullYear(), month: (variavel.getMonth() + 1), day: variavel.getDate() };
    }
  }
  verificarange() {
    if (typeof (this.dtInicio.data) == 'object' && this.dtInicio.data != null) {
      if (this.dtInicio.data.getYear() < 0) {
        this.dtInicio.erro = true;
        this.dtInicio.msg = 'Data Inválida.';
        this.emitir = false;
      }
    }
  }
}
