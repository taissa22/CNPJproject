export class Observacao {
  constructor( idF2500 : number,
    logDataOperacao: Date,
    logCodUsuario: string,
    observacoesObservacao : string,
    idEsF2500Observacoes : number){

    this.idF2500 = idF2500,
    this.logDataOperacao = logDataOperacao,
    this.logCodUsuario = logCodUsuario,
    this.observacoesObservacao = observacoesObservacao,
    this.idEsF2500Observacoes = idEsF2500Observacoes
  }

  readonly idF2500 : number;
  readonly logDataOperacao: Date;
  readonly logCodUsuario: string;
  readonly observacoesObservacao : string;
  readonly idEsF2500Observacoes : number;


  static  fromObj(item: any){
    return new Observacao(item.idF2500,item.logDataOperaca,item.logCodUsuari,item.observacoesObservacao,item.idEsF2500Observacoes);
  }

}
