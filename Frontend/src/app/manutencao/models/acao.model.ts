export class Acao {
  id: number;
  idMigracao: number;
  descricao: string;
  ativo: string;
  naturezaAcaoBB: any;
  descricaoMigracao : string;
  ativoDePara : boolean;
  enviarAppPreposto : string;
  naturezaAcaoBBId: any;
  indRequerEscritorio: any;
  naturezaAcaoBBDesc: any;
  acaoCivelEstrategicoDesc: any;
  acaoCivelEstrategicoId: any;
  acaoCivelConsumidorDesc: any;
  acaoCivelConsumidorId: any;

  static fromJson(a: Acao): Acao {
    const acao = new Acao();
    acao.id = a.id;
    acao.idMigracao = a.idMigracao;
    acao.descricao = a.descricao;
    acao.ativo = a.ativo;
    acao.naturezaAcaoBB = a.naturezaAcaoBB;
    acao.descricaoMigracao = a.descricaoMigracao ? a.descricaoMigracao +  (a.ativoDePara ? '' : ' [INATIVO]') : '';
    acao.enviarAppPreposto = a.enviarAppPreposto  ? "Sim" : "Não";
    return acao;
  }

  static fromObj(obj: any): Acao {
    const acao = new Acao();
    acao.id = obj.id;
    acao.descricao = obj.descricao;
    acao.ativo = obj.ativo;
    acao.enviarAppPreposto = obj.enviarAppPreposto;
    acao.naturezaAcaoBBId = obj.naturezaAcaoBBId;
    acao.indRequerEscritorio = obj.indRequerEscritorio;
    acao.naturezaAcaoBBDesc = obj.naturezaAcaoBBDesc;
    acao.acaoCivelEstrategicoDesc = obj.acaoCivelEstrategicoDesc;
    acao.acaoCivelEstrategicoId = obj.acaoCivelEstrategicoId;
    acao.acaoCivelConsumidorDesc = obj.acaoCivelConsumidorDesc;
    acao.acaoCivelConsumidorId = obj.acaoCivelConsumidorId;
    return acao;
  }

  get nomeNatureza(): string {
    return this.naturezaAcaoBB !== null ? this.naturezaAcaoBB.nome : '';
  }

  get descricaoTratada(): string {
    if (this.descricao.length <= 30) return this.descricao;
    return this.descricao.substring(0, 30) + '...';
  }

}
