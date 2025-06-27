// tslint:disable-next-line: no-empty-interface
export class CriteriosModel {
  statusProcesso: number;
  statusContabilProcesso: number;
  inicioPeriodoAudiencia: Date;
  fimPeriodoAudiencia: Date;
  inicioDataCadastro: Date;
  fimDataCadastro: Date;
  inicioDataDistribuicao: Date;
  fimDataDistribuicao: Date;
  inicioDataFinalizacaoContabil: Date;
  fimDataFinalizacaoContabil: Date;
  inicioDataFinalizacaoEscritorio: Date;
  fimDataFinalizacaoEscritorio: Date;

  listaEmpresas: Array<number>;
  listaEscritorios: Array<number>;

}
