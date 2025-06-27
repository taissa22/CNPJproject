import { TipoParte } from './tipo-parte.enum';
import { EstadoEnum } from './estado.enum';

export class Parte {
    id: number;
    nome: string;
    cpf: string;
    cnpj: string;
    carteiraDeTrabalho: string;
    tipoParte: TipoParte;
    estado: EstadoEnum;
    endereco: string;
    cep: string;
    cidade: string;
    bairro: string;
    enderecosAdicionais: string;
    telefoneDDD: string;
    telefone: string;
    celularDDD: string;
    celular: string;
    telefonesAdicionais: string;
    valorCartaFianca: string;
    dataCartaFianca: Date;

    get CPF(): string {
        if (this.cpf !== null && this.cpf !== '') {
            const cpf = this.cpf.replace(/\D/g, '');
            return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/g, '\$1.\$2.\$3-\$4');
        }
        return '';
    }

    get CNPJ(): string {
        if (this.cnpj !== null && this.cnpj !== '') {
            const cnpj = this.cnpj.replace(/\D/g, '');
            return cnpj.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/g, '\$1.\$2.\$3/\$4-\$5');
        }
        return '';
    }

    get documento(): string {
        if ((this.cpf))
            return this.CNPJ;
        else
            return this.CPF;
    }

    get nomeTratado(): string {
      if (this.nome.length <= 30) return this.nome;
      return this.nome.substring(0, 30) + '...';
    }

    static fromJson(p: Parte): Parte {
        const parte = new Parte();
        parte.id = p.id;
        parte.nome = p.nome;
        parte.cpf = p.cpf;
        parte.cnpj = p.cnpj;
        parte.carteiraDeTrabalho = p.carteiraDeTrabalho;
        parte.tipoParte = TipoParte.fromJson(p.tipoParte);
        parte.estado = EstadoEnum.fromJson(p.estado);
        parte.endereco = p.endereco;
        parte.cep = p.cep;
        parte.cidade = p.cidade;
        parte.bairro = p.bairro;
        parte.enderecosAdicionais = p.enderecosAdicionais;
        parte.telefoneDDD = p.telefoneDDD;
        parte.telefone = p.telefone;
        parte.celularDDD = p.celularDDD;
        parte.celular = p.celular;
        parte.telefonesAdicionais = p.telefonesAdicionais;
        parte.valorCartaFianca = p.valorCartaFianca;
        parte.dataCartaFianca =  new Date(p.dataCartaFianca);
        return parte;
    }
}



