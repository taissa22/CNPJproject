export interface IAgendaProcessoResultadoPesquisar {
  dado: Array<IAgendaProcessoPesquisar>;
  totalElementos: number;
}

export interface IAgendaProcessoPesquisar{
  codProcesso: number;
  seqAudiencia: number;
  siglaEstado: string;
  comarca: string;
  codVara: number
  tipoVara: string;
  dataAudiencia: string;
  horarioAudiencia: string;
  tipoAudiencia: string;
  preposto: string;
  escritorioAudiencia: string;
  advogadoAudiencia: string;
  prepostoAcompanhante: string;
  escritorioAcompanhante: string;
  advogadoAcompanhante: string;
  tipoProcesso: string;
  estrategico: string;
  numeroProcesso: string;
  classificacaoHierarquica: string;
  empresaGrupo: string;
  endereco: string;
  escritorioProcesso: string
}
