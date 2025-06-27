import { PipeTransform } from '@angular/core';

export class ColunaModal {
  nomeExibicao: string;
  nomePropriedade: string;
  format: PipeTransform;
  pattern: Array<any>;

  constructor(nomeExibicao: string, nomePropriedade: string, format?: PipeTransform,
              pattern?: Array<any>) {
    this.nomeExibicao = nomeExibicao;
    this.nomePropriedade = nomePropriedade;
    this.format = format;
    this.pattern = pattern;
  }

  getData(registro: any) {
    const info  = registro[this.nomePropriedade];
    if (this.format) return this.format.transform(info, this.pattern);
    return info;
  }
}
