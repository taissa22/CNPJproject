export class MovimentacaoJec {
  id: number;
  datAgendamento: Date;
  idBaseMovIni: number;
  iniDataFechamento: Date;
  iniNumMesesMediaHistorica: number;
  iniIndMensal: string;
  iniPercentualHaircut: number;
  iniValCorteOutliers: number;
  iniIndFechamentoParcial: boolean;
  iniEmpresas: string;
  idBaseMovFim: number;
  fimDataFechamento: Date;
  fimNumMesesMediaHistorica: number;
  fimIndMensal: string;
  fimPercentualHaircut: number;
  fimValCorteOutliers: number;
  fimIndFechamentoParcial: boolean;
  fimEmpresas: string;
  datInicioExecucao: Date;
  datFimExecucao: Date;
  status: number;
  msgErro: string;
  usuarioId: string;
  usuarioNome: string;
}

// declare interface FechamentoMovJec {
//   idBaseMov: number;
//   datFechamento: Date;
//   mesAnoFechamento: Date;
//   numMesesFechamento: number;
//   codEmpresaCentralizadora: number;
//   valorCorteOutliers: number;
//   percHaircut: number;
//   datSolFec: Date;
//   usuSolicFech: string;
//   indFechamentoMensal: string;
//   usuSolicFechNavigation: usuSolicFechNavigation
// }

// declare interface usuSolicFechNavigation {
//   codUsuario: string;
//   nomeUsuario: string;
// }
