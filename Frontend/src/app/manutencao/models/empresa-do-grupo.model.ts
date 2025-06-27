import { InterfaceBb } from './interface-bb.model';
import { CentroCusto } from '@shared/models/centro-custo.model';
import { EmpresaCentralizadora } from './empresa-centralizadora.model';
import { Regional } from '@shared/models/regional.model';
import { Estado } from '@core/models/estado.model';
import { Fornecedor } from '@shared/models/fornecedor.model';
export class EmpresaDoGrupo {
  constructor(e: EmpresaDoGrupo) {
    this.id = e.id;
    this.cnpj = e.cnpj;
    this.nome = e.nome;
    this.endereco = e.endereco;
    this.bairro = e.bairro;
    this.estado = e.estado;
    this.cidade = e.cidade;
    this.cep = e.cep;
    this.telefoneDDD = e.telefoneDDD;
    this.telefone = e.telefone;
    this.faxDDD = e.faxDDD;
    this.fax = e.fax;
    this.regional = e.regional;
    this.empresaCentralizadora = e.empresaCentralizadora;
    this.empresaSap = e.empresaSap;
    this.fornecedor = e.fornecedor;
    this.codCentroSap = e.codCentroSap;
    this.centroCusto = e.centroCusto;
    this.geraArquivoBB = e.geraArquivoBB;
    this.diretorioBB = e.diretorioBB;
    this.empRecuperanda = e.empRecuperanda;
    this.empTrio = e.empTrio;
  }
  id: number;
  cnpj: string;
  nome: string;
  endereco: string;
  bairro: string;
  estado: Estado;
  cidade: string;
  cep: string;
  telefoneDDD: string;
  telefone: string;
  faxDDD: string;
  fax: string;
  regional: Regional;
  empresaCentralizadora: EmpresaCentralizadora;
  empresaSap: any;
  fornecedor: Fornecedor;
  codCentroSap: string;
  centroCusto: CentroCusto;
  geraArquivoBB: boolean;
  diretorioBB: number;
  empRecuperanda: string;
  empTrio: string;

  get cnpjFormatado(): string {
    if (this.cnpj !== null && this.cnpj !== '') {
      const cnpj = this.cnpj.replace(/\D/g, '');
      return cnpj.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/g, '\$1.\$2.\$3/\$4-\$5');
    }
    return '';
  }

  get nomeTratado(): string {
    if (this.nome.length <= 30) return this.nome;
    return this.nome.substring(0, 30) + '...';
  }
}
