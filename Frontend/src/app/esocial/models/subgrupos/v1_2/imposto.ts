export class Imposto {
    constructor(
      idEsF2501Infocrirrf: number,
      logDataOperacao: Date,
      logCodUsuario: string,
      infocrcontribTpcr: number,
      infocrcontribVrcr: number,
      descricaoTpcr: string,
      infoirVrrendtrib: number,
      infoirVrrendtrib13: number,
      infoirVrrendmolegrave: number,
      infoirVrrendisen65: number,
      infoirRrendisen65dec: number,
      infoirVrjurosmora: number,
      infoirVrrendisenntrib: number,
      infoirDescisenntrib: string,
      infoirVrprevoficial: number,
      infocrcontribVrcr13: number,
      infoirVlrDiarias: number,
      infoirVlrAjudaCusto: number,
      infoirVlrIndResContrato: number,
      infoirVlrAbonoPec: number,
      infoirVlrAuxMoradia: number,
    ) {
      this.idEsF2501Infocrirrf = idEsF2501Infocrirrf;
      this.logDataOperacao = logDataOperacao;
      this.logCodUsuario = logCodUsuario;
      this.infocrcontribTpcr = infocrcontribTpcr;
      this.infocrcontribVrcr = infocrcontribVrcr;
      this.descricaoTpcr = descricaoTpcr;
      this.infoirVrrendtrib = infoirVrrendtrib;
      this.infoirVrrendtrib13 = infoirVrrendtrib13;
      this.infoirVrrendmolegrave = infoirVrrendmolegrave;
      this.infoirVrrendisen65 = infoirVrrendisen65;
      this.infoirRrendisen65dec = infoirRrendisen65dec;
      this.infoirVrjurosmora = infoirVrjurosmora;
      this.infoirVrrendisenntrib = infoirVrrendisenntrib;
      this.infoirDescisenntrib = infoirDescisenntrib;
      this.infoirVrprevoficial = infoirVrprevoficial;
      this.infocrcontribVrcr13 = infocrcontribVrcr13;
      this.infoirVlrDiarias = infoirVlrDiarias;
      this.infoirVlrAjudaCusto = infoirVlrAjudaCusto;
      this.infoirVlrIndResContrato = infoirVlrIndResContrato;
      this.infoirVlrAbonoPec = infoirVlrAbonoPec;
      this.infoirVlrAuxMoradia = infoirVlrAuxMoradia;
    }
  
    readonly idEsF2501Infocrirrf: number;
    readonly logDataOperacao: Date;
    readonly logCodUsuario: string;
    readonly infocrcontribTpcr: number;
    readonly infocrcontribVrcr: number;
    readonly descricaoTpcr: string;
    readonly infoirVrrendtrib: number;
    readonly infoirVrrendtrib13: number;
    readonly infoirVrrendmolegrave: number;
    readonly infoirVrrendisen65: number;
    readonly infoirVrjurosmora: number;
    readonly infoirVrrendisenntrib: number;
    readonly infoirDescisenntrib: string;
    readonly infoirVrprevoficial: number;
    readonly infocrcontribVrcr13: number;

    readonly infoirVrrendmolegrave13: number;
    readonly infoirRrendisen65dec: number;
    readonly infoirVrjurosmora13: number;
    readonly infoirVrprevoficial13: number;

    readonly infoirVlrDiarias : number;
    readonly infoirVlrAjudaCusto : number;
    readonly infoirVlrIndResContrato : number;
    readonly infoirVlrAbonoPec : number;
    readonly infoirVlrAuxMoradia : number;
  
    static fromObj(item: any) {
      return new Imposto(
        item.idEsF2501Infocrirrf,
        item.logDataOperacao,
        item.logCodUsuario,
        item.infocrcontribTpcr,
        item.infocrcontribVrcr,
        item.descricaoTpcr,
        item.infoirVrrendtrib,
        item.infoirVrrendtrib13,
        item.infoirVrrendmolegrave,
        item.infoirVrrendisen65,
        item.infoirRrendisen65dec,
        item.infoirVrjurosmora,
        item.infoirVrrendisenntrib,
        item.infoirDescisenntrib,
        item.infoirVrprevoficial,
        item.infocrcontribVrcr13,
        item.infoirVlrDiarias,
        item.infoirVlrAjudaCusto,
        item.infoirVlrIndResContrato,
        item.infoirVlrAbonoPec,
        item.infoirVlrAuxMoradia
      );
    }
  }
  