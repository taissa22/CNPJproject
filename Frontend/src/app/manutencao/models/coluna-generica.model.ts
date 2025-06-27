export class ColunaGenerica {

  constructor(titulo: string, data: any, ordernar: boolean, largura?: string, dataTitle?: string, style?: object, dataLimited?: boolean) {
    this.titulo = titulo;
    this.data = data;
    this.ordernar = ordernar;
    this.largura = largura;
    if (!this.largura || this.largura.length === 0) this.largura = 'auto';
    this.dataTitle = dataTitle;
    this.style = style;
    this.dataLimited = dataLimited;
  }

  titulo: string;
  data: any;
  ordernar: boolean;
  largura: string;
  dataTitle: string;
  style: object;
  dataLimited: boolean;
}
