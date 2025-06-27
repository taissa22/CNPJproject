export class Estabelecimento {

  // constructor(id: number, cnpj: string, nome: string, endereco: string, bairro: string, cidade: string,
  //             estado: any, cep: any, telefone: any, celular: any) {
  //   this.id = id;
  //   this.cnpj = cnpj;
  //   this.nome = nome;
  //   this.endereco = endereco;
  //   this.bairro = bairro;
  //   this.cidade = cidade;
  //   this.estado = estado;
  //   this.cep = cep;
  //   this.telefone = telefone;
  //   this.celular = celular;
  // }

  id: number;
  cnpj: string;
  nome: string;
  endereco: string;
  bairro: string;
  cidade: string;
  estado: any;
  cep: any;
  telefone: any;
  telefoneDDD: any;
  celular: any;
  celularDDD: any;

  static fromJson(e: Estabelecimento): Estabelecimento {
    const estabelecimento = new Estabelecimento();
    estabelecimento.id = e.id,
    estabelecimento.cnpj = e.cnpj;
    estabelecimento.nome = e.nome;
    estabelecimento.endereco = e.endereco;
    estabelecimento.bairro = e.bairro;
    estabelecimento.cidade = e.cidade;
    estabelecimento.estado = e.estado;
    estabelecimento.cep = e.cep;
    estabelecimento.telefone = e.telefone;
    estabelecimento.telefoneDDD = e.telefoneDDD;
    estabelecimento.celular = e.celular;
    estabelecimento.celularDDD = e.celularDDD;

    return estabelecimento;
  }

  get idEstado(): string {
    return this.estado !== null ? this.estado.id : '';
  }

  get nomeTratado(): string {
    if (this.nome.length <= 40) return this.nome;
    return this.nome.substring(0, 40) + '...';
  }

  get CNPJ(): string {
    if (this.cnpj !== null && this.cnpj !== '') {
        const cnpj = this.cnpj.replace(/\D/g, '');
        return cnpj.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/g, '\$1.\$2.\$3/\$4-\$5');
    }
    return '';
}
}
