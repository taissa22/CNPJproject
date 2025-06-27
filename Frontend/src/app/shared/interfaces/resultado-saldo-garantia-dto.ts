export interface SaldoGarantiaResultadoDto {
  idProcesso: number;
  numeroProcesso: string;
  codigoEstado: string;
  descricaoComarca: string;
  codigoVara: string;
  descricaoTipoVara: string;
  descricaoEmpresaGrupo: string;
  ativo: string;
  descricaoBanco: string;
  descricaoEscritorio: string;
  dataFinalizacaoContabil: string;
  descricaoTipoGarantia: string;
  valorPrincipal: number;
  valorCorrecaoPrincipal: number;
  valorAjusteCorrecao: number;
  valorJurosPrincipal: number;
  valorAjusteJuros: number;
  valorPagamentoPrincipal: number;
  valorPagamentoCorrecao: number;
  valorPagamentosJuros: number;
  valorLevantadoPrincipal: number;
  valorLevantadoCorrecao: number;
  valorLevantadoJuros: number;
  valorSaldoPrincipal: number;
  valorSaldoCorrecao: number;
  valorSaldoJuros: number;
}
