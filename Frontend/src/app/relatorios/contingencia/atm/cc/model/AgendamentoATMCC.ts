export class AgendamentoATMCCModel {
  codAgendRelatorioAtm: number;
  codFechContCcMedia: number;
  mesAnoContabil: string | null;
  datFechamento: string;
  indFechMensal: string | null;
  empGrupoFech: string | null;
  mensagem: string;
  mensagemErroTrace: string | null;
  datFimExec: string;
  usrCodUsuario: string;
  nomArquivoGerado: string | null;
  status: StatusAgendamento;
  datIniExec: string;
  agendRelatAtmCcIndiceUf: [];
  codFechContCcMediaNavigation: string | null;
  datSolicitacao: string;
}

export interface AgendamentosResponse {
  data: AgendamentoATMCCModel[];
  total: number;
}

export enum StatusAgendamento {
  Agendado = 1,
  Processando = 2,
  Processado = 3,
  Erro = 4
}

export const StatusDescription = {
  [StatusAgendamento.Agendado]: 'Agendado',
  [StatusAgendamento.Processando]: 'Processando',
  [StatusAgendamento.Processado]: 'Finalizado',
  [StatusAgendamento.Erro]: 'Erro'
};

export interface FechamentoProps {
  id : number;
  codSolicFechamentoCont: number;
  mesAnoFechamento: string;
  dataFechamento: string;
  numeroMeses: number;
  empceCodEmpCentralizadora: number;
  empresaCentralizador: string;
  nomeUsuario : string;
  empresas : string;
}