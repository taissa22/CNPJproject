import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { NgbDateCustomParserFormatter } from '@shared/dateformat';
import { NgbDateAdapter, NgbDateNativeAdapter, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';


@Component({
  selector: 'app-number',
  templateUrl: './number.component.html',
  styleUrls: ['./number.component.scss']
})
export class NumberComponent implements OnInit {

  @Input() label: string;
  // Ret == Retorno?
  @Output() retNumeroInicio: EventEmitter<number> = new EventEmitter<number>();
  // @Output() retNumeroFim: EventEmitter<number> = new EventEmitter<number>();


  @Input() maxlengthNumber: number;
  @Input() formatarParaReal: boolean = true;

  @Input() texto1: string  = "0";
  // @Input() texto2: string= "0" ;
  // public isInvalid;

  public valorInicial = {
    data: null,
    erro: false,
    msg: ''
  };

  // public valorFinal  = {
  //   data: null,
  //   erro: false,
  //   msg: ''
  // };

  constructor() { }

  ngOnInit() {
  }

  retornarNumero() {
    // console.log(this.texto1)
    this.retNumeroInicio.emit(parseFloat(this.texto1));

    // if (!this.isInvalid) {
    //   this.retNumeroInicio.emit(parseFloat(this.texto1));
    // this.retNumeroFim.emit(parseFloat(this.texto2));
// 
    // }

  }

}
