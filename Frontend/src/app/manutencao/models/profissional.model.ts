import { Estado } from '@core/models/estado.model';

export class Profissional {
  id: number;
  tipoPessoa: { valor: string; descricao: string };
  cpf: string;
  cnpj: string;
  nome: string;
  email: string;
  ehContador: boolean;
  ehContadorPex: boolean;
  telefoneDDD: string;
  telefone: string;
  celularDDD: string;
  celular: string;
  faxDDD: string;
  fax: string;
  ehAdvogado: boolean;
  registroOAB: string;
  estadoOAB: Estado;
  endereco: string;
  enderecosAdicionais: string;
  telefonesAdicionais: string;
  cidade: string;
  estado: Estado;
  bairro: string;
  cep: number;

  get nomeTratado(): string {
    if (this.nome.length <= 30) return this.nome;
    return this.nome.substring(0, 30) + '...';
  }

  get documento(): string {
    if (this.cpf !== null && this.cpf !== '') {
      const cpf = this.cpf.replace(/\D/g, '');
      return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/g, '\$1.\$2.\$3-\$4');
    }
    if (this.cnpj !== null && this.cnpj !== '') {
      const cnpj = this.cnpj.replace(/\D/g, '');
      return cnpj.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/g, '\$1.\$2.\$3/\$4-\$5');
    }
    return '';
  }

  static fromJson(p: Profissional): Profissional {
    if(!p) return null;
    const profissional = new Profissional();
    profissional.id = p.id;
    profissional.tipoPessoa = p.tipoPessoa;
    profissional.cpf = p.cpf;
    profissional.cnpj = p.cnpj;
    profissional.nome = p.nome;
    profissional.email = p.email;
    profissional.ehContador = p.ehContador;
    profissional.ehContadorPex = p.ehContadorPex;
    profissional.telefoneDDD = p.telefoneDDD;
    profissional.telefone = p.telefone;
    profissional.celularDDD = p.celularDDD;
    profissional.celular = p.celular;
    profissional.faxDDD = p.faxDDD;
    profissional.fax = p.fax;
    profissional.ehAdvogado = p.ehAdvogado;
    profissional.registroOAB = p.registroOAB;
    profissional.estadoOAB = p.estadoOAB;
    profissional.endereco = p.endereco;
    profissional.enderecosAdicionais = p.enderecosAdicionais;
    profissional.telefonesAdicionais = p.telefonesAdicionais;
    profissional.cidade = p.cidade;
    profissional.estado = p.estado;
    profissional.bairro = p.bairro;
    profissional.cep = p.cep;
    return profissional;
  }
}
