export class AcompanhamentoPesquisaRequest {
  periodoStatusDe?: Date;
  periodoStatusAte?: Date;
  periodoApuracaoDe?: Date;
  periodoApuracaoAte?: Date;
  periodoSentencaDe?: Date;
  periodoSentencaAte?: Date;
  processo?: string;
  reclamante?: string;
  cpf?: string;
  statusExecucao?: number[];
  empresa?: number[];
  tipoFormulario?: number;
  tipoFormularioTipo?: number;
  escritorio?: string;
  contador?: string;
  uf?: string;
}
