export interface ResultadoAgendamento {
  id: number;
  nomeAgendamento: string;
  dataAgendamento: string;
  statusAgendamento: string;
  dataFinalizacao: string;
  mensagemErro?: string;
  nomeArquivo?: string;
  tipoProcesso: string;
}
