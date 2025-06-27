import { TipoAudiencia } from './../../../models/tipo-audiencia.model';
import { Escritorio } from './escritorio.model';
import { Advogado } from './advogado.model';
import { Preposto } from './preposto.model';


export class Audiencia {

  advogadoEscritorio: Advogado;
  comentario: string;
  dataAudiencia: Date;
  horaAudiencia: Date;
  escritorio: Escritorio;
  id: number;
  preposto: Preposto;
  sequencial: number;
  tipoAudiencia: TipoAudiencia;
  processo: any;

  /*
  constructor(
    advogadoEscritorio: Advogado,
    comentario: string,
    data: Date,
    escritorio: Escritorio,
    preposto: Preposto,
    sequencial: number,
    tipoAudiencia: TipoAudiencia,
    processo: any
  ) {
    this.advogadoEscritorio = advogadoEscritorio;
    this.comentario = comentario;
    this.data = data;
    this.escritorio = escritorio;
    this.preposto = preposto;
    this.sequencial = sequencial;
    this.tipoAudiencia = tipoAudiencia;
    this.processo = processo;
  }
  */

  static fromJson(a: Audiencia): Audiencia {
    const audiencia = new Audiencia();
    audiencia.advogadoEscritorio = a.advogadoEscritorio;
    audiencia.comentario = a.comentario;
    audiencia.dataAudiencia = a.dataAudiencia;
    audiencia.horaAudiencia = a.horaAudiencia;
    audiencia.escritorio = a.escritorio;
    audiencia.preposto = a.preposto;
    audiencia.sequencial = a.sequencial;
    audiencia.tipoAudiencia = a.tipoAudiencia;
    audiencia.processo = a.processo;
    return audiencia;
  }

  get tipoAudienciaNome(): string {
    return this.tipoAudiencia ? this.tipoAudiencia.descricao : '';
  }

  get estadoId(): string {
    if (!(this.processo && this.processo.comarca)) {
      return '';
    }
    return this.processo.comarca.estadoId;
  }

  get comarcaNome(): string {
    if (!(this.processo && this.processo.vara && this.processo.vara.comarca)) {
      return '';
    }
    return this.processo.vara.comarca.nome;
  }

  get varaTipoVaraNome(): string {
    if (!(this.processo && this.processo.vara && this.processo.tipoVara)) {
      return '';
    }
    return `${this.processo.vara.numero}ª ${this.processo.tipoVara.nome}`;
  }

  get estrategicoId(): string {
    return this.processo ? (this.processo.tipoProcessoId === 9 ? 'Sim' : 'Não') : '';
  }

  get numeroProcesso(): string {
    return this.processo ? this.processo.numeroProcesso : '';
  }

  get prepostoNome(): string {
    if (!this.preposto) {
      return '';
    }
    const sufixo = this.preposto.ativo ? '' : ' [INATIVO]';
    return `${this.preposto.nome}${sufixo}`;
  }

  get escritorioNome(): string {
    if (!this.escritorio) {
      return '';
    }
    const sufixo = this.escritorio.ativo ? '' : ' [INATIVO]';
    return `${this.escritorio.nome}${sufixo}`;
  }

  get advogadoEscritorioNome(): string {
    return this.advogadoEscritorio ? this.advogadoEscritorio.nome : '';
  }

  get escritorioProcessoNome(): string {
    if (!this.processo || !this.processo.escritorio) {
      return '';
    }
    const escritorio = this.processo.escritorio;
    const sufixo = escritorio.ativo ? '' : ' [INATIVO]';
    return `${escritorio.nome}${sufixo}`;
  }

  get endereco(): string {
    if (!this.processo || !this.processo.escritorio) {
      return '';
    }
    return this.processo.escritorio.endereco;
  }

  get closing(): string {
    return this.processo ? (this.processo.closing === 1 ? 'Pré' :  this.processo.closing === 2 ? 'Pós' : this.processo.closing === 3 ? 'Híbrido' : this.processo.closing === 4 ? 'N/A' : this.processo.closing === 0 ? 'A Definir' : '') : '';
  }

  get closingClientCo(): string {
    return this.processo ? (this.processo.closingClientCo === 1 ? 'Pré' :  this.processo.closingClientCo === 2 ? 'Pós' : this.processo.closingClientCo === 3 ? 'Híbrido' :this.processo.closingClientCo === 4 ? 'N/A' : this.processo.closingClientCo === 0 ? 'A Definir' : '') : '';
  }

  get classificacao(): string {
    return this.processo.classificacaoProcesso.descricao;
  }

}
