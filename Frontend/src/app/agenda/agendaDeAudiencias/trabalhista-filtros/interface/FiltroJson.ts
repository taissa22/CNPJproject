export interface FiltroJson {
  estrategico: string;
  periodoPendenciaCalculoInicio?: Date;
  periodoPendenciaCalculoFim?: Date;
  dataAudienciaInicio?: Date;
  dataAudienciaFim?: Date;
  classificacaoHierarquica: string;
  advogadoAudiencia?: Array<number>;
  advogadoAcompanhante?: Array<number>;
  codComarca?: Array<number>;
  empresaGrupo?: Array<number>;
  siglaEstado?: Array<number>;
  preposto?: Array<number>;
  prepostoAcompanhante?: Array<number>;
  escritorioAudiencia?: Array<number>;
  escritorioAcompanhante?: Array<number>;
  vara?: Array<string>;
  codProcesso?: Array<number>;
  codEscritorioAudiencia?: string;
  tipoEscritorio: string;
  classificacaoClosing: string;
}
