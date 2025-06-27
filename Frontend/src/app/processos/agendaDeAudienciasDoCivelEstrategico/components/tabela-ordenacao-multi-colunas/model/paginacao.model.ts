export class Paginacao {
  constructor(paginaAtual: number, tamanhoPagina: number) {
    this.paginaAtual = paginaAtual;
    this.tamanhoPagina = tamanhoPagina;
  }

  paginaAtual: number;
  tamanhoPagina: number;
}
