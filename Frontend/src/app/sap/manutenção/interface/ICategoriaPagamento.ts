export interface ICategoriaPagamento {
  codigo: number;
  descricao: string;
  tipoLancamento: number;
  indAtivo: boolean;
  codigoMaterialSAP?: number;
  indEnvioSap: boolean;
  clgarCodigoClasseGarantia: number;
  descricaoclassegarantia: string;
  indicadorNumeroGuia: boolean;
  registrarProcessos: boolean;
  tipoFornecedorPermitido: number;
  fornecedoresPermitidos: string;
  indEscritorioSolicitaLan? : boolean;
  grpcgIdGrupoCorrecaoGar? : number;
  grupoCorrecao: string;
  indEncerraProcessoContabil: boolean;
  indComprovanteSolicitacao?: boolean;
  indicadorRequerDataVencimento?: boolean;
  indicadorContingencia: boolean;
  indicadorCivelConsumidor: boolean;
  indicadorCivelEstrategico: boolean;
  indicadorTrabalhista: boolean;
  indicadorTributarioJudicial: boolean;
  ind_TributarioAdministrativo: boolean;
  indicadorBaixaGarantia: boolean;
  indBaixaPagamento: boolean;
  indicadorBloqueioDeposito? : boolean;
  indicadorJuizado: boolean;
  indicadorAdministrativo: boolean;
  indicadorHistorico: boolean;
  indicadorProcon: boolean;
  indicadorPex: boolean;
  tmgarCodigoMovicadorGarantia?: number;
  indicadorFinalizacaoContabil: boolean;
  descricaoJustificativa: string;
  descricaoJustificativainfuenciaacontigencia: string;
  selected?: boolean;
  pagamentoA: number;
  responsabilidadeOi: number;
  codMigEstrategico? :number;
  descricaoEstrategico: string;
  codMigConsumidor? :number;
  descricaoConsumidor: string;
}

export interface IndicadoresTipoProcesso{
  indicadorCivelConsumidor: boolean;
  indicadorCivelEstrategico: boolean;
  indicadorTrabalhista: boolean;
  indicadorTributarioJudicial: boolean;
  ind_TributarioAdministrativo: boolean;
  indicadorJuizado: boolean;
  indicadorAdministrativo: boolean;
  indicadorProcon: boolean;
  indicadorPex: boolean;
}


