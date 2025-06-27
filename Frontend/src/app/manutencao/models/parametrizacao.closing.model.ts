export class ParametizacaoClosing {
  codTipoProcesso: number;
  classificaoClosing: number;
  classificaoClosingClientCO: number;
  indClosingHibrido: string;
  indClosingHibridoClientCO: string;
  percResponsabilidade: number;
  percResponsabilidadeClientCO: number;
  idEscritorioPadrao: number;
  idEscritorioPadraoClientCO: number;

    constructor() {
      this.codTipoProcesso = 0;
      this.classificaoClosing = 0;
      this.classificaoClosingClientCO = 0;
      this.indClosingHibrido = "";
      this.indClosingHibridoClientCO = "";
      this.percResponsabilidade = 0;
      this.percResponsabilidadeClientCO = 0;
      this.idEscritorioPadrao = 0;
      this.idEscritorioPadraoClientCO = 0;
    }

    static fromJson(p: any): ParametizacaoClosing {
      const parte = new ParametizacaoClosing();
      parte.codTipoProcesso = p.codTipoProcesso;
      parte.classificaoClosing = p.classificaoClosing;
      parte.classificaoClosingClientCO = p.classificaoClosingClientCO;
      parte.indClosingHibrido = p.indClosingHibrido;
      parte.indClosingHibridoClientCO = p.indClosingHibridoFibra;
      parte.percResponsabilidade = p.percResponsabilidade;
      parte.percResponsabilidade = p.percResponsabilidadeClientCO;
      parte.idEscritorioPadrao = p.idEscritorioPadrao;
      parte.idEscritorioPadraoClientCO = p.IdEscritorioPadraoClientCO;
      return parte;
  }

}

  