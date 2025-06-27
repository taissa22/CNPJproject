export class MovimentacaoCC {
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

export class FechamentoMovCC {
  id: number;
  dataFechamento: Date;
  numeroMeses: number;
  indMensal: string;
  dataIndMensal: Date;
  indBaseGerada: string;
  dataGeracao: Date;
  usrCodUsuario: string;
  valorCorte: number;
  percHaircut: number;
  codSolicFechamentoCont: number;
}