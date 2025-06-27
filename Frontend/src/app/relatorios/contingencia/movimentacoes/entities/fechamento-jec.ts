export class FechamentoJec {
  id: number;
  dataFechamento: Date;
  percHaircut: number;
  valorCorteOutliers: number;
  numeroMeses: number;
  empresas: string;
  indicaFechamentoParcial: boolean;
  indicaFechamentoMensal: string;
}

declare interface usuSolicFechNavigation {
  codUsuario: string;
  nomeUsuario: string;
}
