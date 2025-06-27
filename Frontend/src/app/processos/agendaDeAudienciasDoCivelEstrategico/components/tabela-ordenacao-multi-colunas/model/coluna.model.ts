import { PipeTransform } from '@angular/core';

export class Coluna {
  nomeExibicao: string;
  nomePropriedade: string;
  cssClass: string;
  largura: string;
  format: PipeTransform;
  pattern: Array<any>;

  constructor(nomeExibicao: string, nomePropriedade: string, largura?: string,
              cssClass?: string, format?: PipeTransform, pattern?: Array<any>) {
    this.nomeExibicao = nomeExibicao;
    this.nomePropriedade = nomePropriedade;
    this.cssClass = cssClass;
    this.largura = largura;
    this.format = format;
    this.pattern = pattern;
  }

  getData(registro: any) {
    const info  = registro[this.nomePropriedade];
    if (this.format) return this.format.transform(info, this.pattern);
    return info;
  }
}
