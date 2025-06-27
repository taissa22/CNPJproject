import { formatDate } from '@angular/common';

export class  MudancaCategoria {
  constructor(idF2500 : number,
              logDataOperacao : Date,
              logCodUsuario : string,
              mudcategativCodcateg : number,
              mudcategativNatatividade : number,
              mudcategativDtmudcategativ : Date,
              idEsF2500Mudcategativ : number,
              descricaoNaturezaDeAtividade :string,
              descricaoCodCategoria: string ){
      this.idF2500 = idF2500,
      this.logDataOperacao = logDataOperacao,
      this.logCodUsuario = logCodUsuario,
      this.mudcategativCodcateg = mudcategativCodcateg,
      this.mudcategativNatatividade = mudcategativNatatividade,
      this.mudcategativDtmudcategativ = mudcategativDtmudcategativ,
      this.idEsF2500Mudcategativ = idEsF2500Mudcategativ,
      this.descricaoNaturezaDeAtividade = descricaoNaturezaDeAtividade,
      this.descricaoCodCategoria = descricaoCodCategoria

  }

  readonly idF2500 : number;
  readonly logDataOperacao : Date;
  readonly logCodUsuario : string;
  readonly mudcategativCodcateg : number;
  readonly mudcategativNatatividade : number;
  readonly mudcategativDtmudcategativ : Date;
  readonly idEsF2500Mudcategativ : number;
  readonly descricaoNaturezaDeAtividade :string;
  readonly descricaoCodCategoria :string;

  static fromObj(item: any){
    return new  MudancaCategoria(item.idF2500 ,item.logDataOperacao ,item.logCodUsuario ,item.mudcategativCodcateg ,
                          item.mudcategativNatatividade , item.mudcategativDtmudcategativ  ,item.idEsF2500Mudcategativ, 
                          item.descricaoNaturezaDeAtividade, item.descricaoCodCategoria );
  }

}
