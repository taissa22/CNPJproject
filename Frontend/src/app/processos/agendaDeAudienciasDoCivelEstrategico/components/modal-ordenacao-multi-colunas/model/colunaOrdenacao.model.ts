import { FormControl } from '@angular/forms';

export class ColunaParaOrdenacao {
  id: string;
  prioridade: number;
  nomeColuna: string;
  tipoOrdenacao: number;
  formControl: FormControl;

  constructor(id: string, prioridade?: number, nomeColuna?: string, tipoOrdenacao?: number,
              formControl?: FormControl) {
    this.id = id;
    this.prioridade = prioridade;
    this.nomeColuna = nomeColuna;
    this.tipoOrdenacao = tipoOrdenacao;
    this.formControl = formControl;
  }
}
