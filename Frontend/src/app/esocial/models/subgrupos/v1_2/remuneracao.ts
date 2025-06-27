export class Remuneracao {
  constructor(idF2500 : number,
              remuneracaoDtremun : Date,
              remuneracaoVrsalfx : number,
              remuneracaoUndsalfixo : number,
              remuneracaoDscsalvar: string,
              logDataOperacao : Date,
              logCodUsuario : string,
              idEsF2500Remuneracao : number,
              descricaoUnidadePagamento : string){

    this.idF2500 = idF2500,
    this.remuneracaoDtremun = remuneracaoDtremun,
    this.remuneracaoVrsalfx = remuneracaoVrsalfx,
    this.remuneracaoUndsalfixo = remuneracaoUndsalfixo,
    this.remuneracaoDscsalvar= remuneracaoDscsalvar,
    this.logDataOperacao = logDataOperacao,
    this.logCodUsuario = logCodUsuario,
    this.idEsF2500Remuneracao = idEsF2500Remuneracao,
    this.descricaoUnidadePagamento = descricaoUnidadePagamento
  }


  readonly idF2500 : number;
  readonly remuneracaoDtremun : Date;
  readonly remuneracaoVrsalfx : number;
  readonly remuneracaoUndsalfixo : number;
  readonly remuneracaoDscsalvar: string;
  readonly logDataOperacao : Date;
  readonly logCodUsuario : string;
  readonly idEsF2500Remuneracao : number;
  readonly descricaoUnidadePagamento : string;

  static  fromObj(item: any){
    return new Remuneracao(item.idF2500 , item.remuneracaoDtremun , item.remuneracaoVrsalfx ,item.remuneracaoUndsalfixo ,
                           item.remuneracaoDscsalvar,item.logDataOperacao ,item.logCodUsuario ,item.idEsF2500Remuneracao,
                           item.descricaoUnidadePagamento );
  }

}
