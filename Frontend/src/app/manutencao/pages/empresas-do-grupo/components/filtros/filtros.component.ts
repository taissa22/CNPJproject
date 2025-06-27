import { isNullOrUndefined } from 'util';
import { FormControl } from '@angular/forms';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'empresa-do-grupo-filtros',
  templateUrl: './filtros.component.html',
  styleUrls: ['./filtros.component.scss']
})
export class FiltrosComponent implements OnInit {

  nomeFormControl: FormControl = new FormControl('');
  cnpjFormControl: FormControl = new FormControl('');
  centroSapFormControl: FormControl = new FormControl('');

  mascaraCnpj = [/[0-9]/, /\d/, '.', /\d/, /\d/, /\d/, '.', /\d/, /\d/, /\d/, '/', /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/];

  @Output() obterParametros: EventEmitter<object> = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  obterValoresDoFiltro() {
    if (this.validarCamposVazios())
      this.obterParametros.emit({
        nome: this.nomeFormControl.value,
        cnpj: this.cnpjFormControl.value,
        codCentroSap: this.centroSapFormControl.value,
      });
  }

  validarCamposVazios(): boolean {
    if (
      isNullOrUndefined(this.nomeFormControl.value) &&
      isNullOrUndefined(this.cnpjFormControl.value) &&
      isNullOrUndefined(this.centroSapFormControl.value)
    ) {
      return false;
    }
    return true;
  }

}
