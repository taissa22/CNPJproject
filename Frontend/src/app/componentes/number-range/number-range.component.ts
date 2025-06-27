import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { NgbDateAdapter, NgbDateNativeAdapter, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from '@shared/dateformat';
import { text } from '@fortawesome/fontawesome-svg-core';


@Component({
  selector: 'app-number-range',
  templateUrl: './number-range.component.html',
  styleUrls: ['./number-range.component.scss'],
  // tslint:disable-next-line: max-line-length
  providers: [{ provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }, { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }]
})
export class NumberRangeComponent implements OnInit {

  @Input() label: string;
  // Ret == Retorno?
  @Output() retNumeroInicio: EventEmitter<number> = new EventEmitter<number>();
  @Output() retNumeroFim: EventEmitter<number> = new EventEmitter<number>();
  @Output() rangeValido: EventEmitter<boolean> = new EventEmitter<boolean>();

  @Input() maxlengthNumber: number;
  @Input() formatarParaReal: boolean = true;

  @Input() texto1: string  = "0";
  @Input() texto2: string= "0" ;
  public isInvalid;

  public valorInicial = {
    data: null,
    erro: false,
    msg: ''
  };

  public valorFinal  = {
    data: null,
    erro: false,
    msg: ''
  };
  constructor(

  ) { }

  ngOnInit() {
  }



  validarNumero() {
    this.texto1 =  !this.texto1 && (this.texto2 || this.texto2 === '0') ? '0' : this.texto1;

    this.valorInicial.data = parseFloat(this.texto1);
    this.valorInicial.erro = false;
    this.valorInicial.msg = '';
    this.valorFinal = {
      data: parseFloat(this.texto2),
      erro: false,
      msg: ''
    };

    this.valorInicial.msg = '';
    if (this.valorInicial.data || this.valorFinal.data) {
      if (this.valorInicial.data > this.valorFinal.data) {
        this.valorInicial.erro = true;
        this.valorInicial.msg = 'Valor inicial não pode ser maior que o final';
        this.valorFinal.erro = true;
        this.valorFinal.msg = 'Valor final não pode ser menor que o inicial';
      } else if (!this.valorInicial.data && this.valorInicial.data !== 0) {
        this.valorInicial.erro = true;
        this.valorInicial.msg = 'Valor inicial deve ser preenchido.';
      } else if (!this.valorFinal.data) {
        this.valorFinal.erro = true;
        this.valorFinal.msg = 'Valor final deve ser preenchido.';
      }
    }

    this.isInvalid = this.valorInicial.erro || this.valorFinal.erro;
    this.rangeValido.emit(!this.isInvalid);

    if (!this.isInvalid) {
      this.retNumeroInicio.emit(parseFloat(this.texto1));
    this.retNumeroFim.emit(parseFloat(this.texto2));

    }

  }
}
