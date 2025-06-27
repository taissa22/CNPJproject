export class ColunaGenerica {

  constructor(titulo: string, data: any, ordernar: boolean, largura?: string) {
    this.titulo = titulo;
    this.data = data;
    this.ordernar = ordernar;
    this.largura = largura;
    if (!this.largura || this.largura.length === 0) this.largura = 'auto';
  }

  titulo: string;
  data: any;
  ordernar: boolean;
  largura: string;
}
