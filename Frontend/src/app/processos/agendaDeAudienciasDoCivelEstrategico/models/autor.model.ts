import { isNullOrUndefined } from 'util';
export class Autor {

  constructor(nome: string, cpf: string, cnpj: string, carteiraTrabalho: string) {
    this.nome = nome;
    this.cpf = cpf;
    this.cnpj = cnpj;
    this.carteiraTrabalho = carteiraTrabalho;
  }

  private id: number;
  public nome: string;
  public cpf: string;
  public cnpj: string;
  public carteiraTrabalho: string;

  get obterId(): number {
    return this.id;
  }

  get obterCpfCnpj() {
    if (isNullOrUndefined(this.cpf)) return this.cnpj;
    if (isNullOrUndefined(this.cnpj)) return this.cpf;

    return '';
  }
}
