import { FormControl } from '@angular/forms';
import { Component, OnInit, Output, EventEmitter, ChangeDetectionStrategy } from '@angular/core';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'profissionais-filtros',
  templateUrl: './filtros.component.html',
  styleUrls: ['./filtros.component.scss'],
  changeDetection: ChangeDetectionStrategy.Default
})
export class FiltrosProfissionaisComponent implements OnInit {

  nomeFormControl: FormControl;
  cpfCnpjFormControl: FormControl;
  advogadoAutorFormControl: FormControl;
  tipoFormControl: FormControl;

  // tslint:disable-next-line: max-line-length
  @Output() obterParametros: EventEmitter<{ nome: string, documento: string, tipoPessoa: 'F' | 'J' | '', advogadoDoAutor: true | false | '' }> = new EventEmitter();

  mascaraCpf = [/[0-9]/, /[0-9]/, /[0-9]/, '.', /[0-9]/, /[0-9]/, /[0-9]/, '.', /[0-9]/, /[0-9]/, /[0-9]/, '-', /[0-9]/, /[0-9]/];
  // tslint:disable-next-line: max-line-length
  mascaraCnpj = [/[0-9]/, /[0-9]/, '.', /[0-9]/, /[0-9]/, /[0-9]/, '.', /[0-9]/, /[0-9]/, /[0-9]/, '/', /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, '-', /[0-9]/, /[0-9]/];

  constructor() { }

  ngOnInit() {
    this.nomeFormControl = new FormControl('');
    this.tipoFormControl = new FormControl('');
    this.cpfCnpjFormControl = new FormControl('');
    this.advogadoAutorFormControl = new FormControl('');
  }

  obterValoresDoFiltro() {
    this.obterParametros.emit({
      nome: this.nomeFormControl.value,
      documento: this.cpfCnpjFormControl.value.replace(/[^0-9]+/g, ''),
      tipoPessoa: this.tipoFormControl.value,
      advogadoDoAutor: this.advogadoAutorFormControl.value
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
