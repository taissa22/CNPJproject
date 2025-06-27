import { FormControl } from '@angular/forms';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'partes-filtros',
  templateUrl: './filtros.component.html',
  styleUrls: ['./filtros.component.scss']
})
export class FiltrosParteComponent implements OnInit {

  @Output() obterParametros: EventEmitter<any> = new EventEmitter<any>();

  nomeFormControl: FormControl;
  tipoFormControl: FormControl;
  cpfCnpjFormControl: FormControl;
  carteiraTrabalhoFormControl: FormControl;

  mascaraCpf = [/[0-9]/, /\d/, /\d/, '.', /\d/, /\d/, /\d/, '.', /\d/, /\d/, /\d/, '-', /\d/, /\d/];
  mascaraCnpj = [/[0-9]/, /\d/, '.', /\d/, /\d/, /\d/, '.', /\d/, /\d/, /\d/, '/', /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/];

  constructor() { }

  ngOnInit() {
    this.inicializarForm();
  }

  inicializarForm() {
    this.nomeFormControl = new FormControl('');
    this.tipoFormControl = new FormControl('');
    this.cpfCnpjFormControl = new FormControl('');
    this.carteiraTrabalhoFormControl = new FormControl('');
  }

  pesquisar() {
    this.obterParametros.emit({
      nome: this.nomeFormControl.value,
      tipo: this.tipoFormControl.value,
      documento: this.cpfCnpjFormControl.value.replace(/[^0-9]+/g, ''),
      carteiraTrabalho: this.carteiraTrabalhoFormControl.value
    });
  }

  cpfCnpjMask(tipo: FormControl): any {
    if (tipo.value === 'F') {
      return this.mascaraCpf;
    } else if (tipo.value === 'J') {
      return this.mascaraCnpj;
    }
    this.cpfCnpjFormControl = new FormControl('');
  }

}
