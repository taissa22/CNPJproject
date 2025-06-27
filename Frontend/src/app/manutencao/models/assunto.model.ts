export class Assunto {

  /*
  constructor(id: number, descricao: string, proposta: string, negociacao: string, ativo: boolean) {
    this.id = id;
    this.descricao = descricao;
    this.proposta = proposta;
    this.negociacao = negociacao;
    this.ativo = ativo;
  }
  */

  id: number;
  idMigracao: number;
  descricao: string;
  proposta: string;
  negociacao: string;
  ativo: boolean;
  ativoDePara: boolean;
  codTipoContingencia:string;  
  descricaoMigracao : string;
  
  

  static fromJson(a: Assunto): Assunto {
    const assunto = new Assunto();
    assunto.id = a.id;
    assunto.idMigracao = a.idMigracao;
    assunto.descricao = a.descricao;
    assunto.proposta = a.proposta;
    assunto.negociacao = a.negociacao;
    assunto.ativo = a.ativo;
    assunto.codTipoContingencia = a.codTipoContingencia;    
    assunto.descricaoMigracao = a.descricaoMigracao ? a.descricaoMigracao +  (a.ativoDePara ? '' : ' [INATIVO]') : '';
    
    return assunto;
  }

  obterDescricaoLimitada() {
    if (this.descricao.length < 300) return this.descricao;
    return this.descricao.substring(0, 299) + '...';
  }

  obterPropostaLimitada() {
    if (!this.proposta || (this.proposta && this.proposta.length < 300)) return this.proposta;
    return this.proposta.substring(0, 299) + '...';
  }

  obterNegociacaoLimitada() {
    if (!this.negociacao || (this.negociacao && this.negociacao.length < 300)) return this.negociacao;
    return this.negociacao.substring(0, 299) + '...';
  }

  get ativoEmTexto(): string {
    return this.ativo ? 'Sim' : 'Não';
  }

  get descricaoTratada(): string {
    if (this.descricao.length <= 30) return this.descricao;
    return this.descricao.substring(0, 30) + '...';
  }

  get propostaTratada(): string {
    if (!this.proposta || (this.proposta && this.proposta.length <= 30)) return this.proposta;
    return this.proposta.substring(0, 30) + '...';
  }

  get negociacaoTratada(): string {
    if (!this.negociacao || (this.negociacao && this.negociacao.length <= 30)) return this.negociacao;
    return this.negociacao.substring(0, 30) + '...';
  }

  get calculoContingenciaTratada(): string {
    return this.codTipoContingencia ? this.codTipoContingencia.substring(0, 1) == "M" ? "MÉDIA" : "PROCESSO A PROCESSO" : "";
  }  

}
