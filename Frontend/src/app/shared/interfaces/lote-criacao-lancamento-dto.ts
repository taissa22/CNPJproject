export interface LoteCriacaoLancamentoDto {
  codigoProcesso: number;
  codigoLancamento: number;
  valorLancamento: number;
  codigoStatusPagamento: number;
  mensagemErro: string;
}

export interface LancamentoDTO {
  DescricaoEscritorio: string;
  Autor: string;
  nProcesso: string;
  valorLiquido: string;
  comarca: string;
  vara: string;
  tipoLancamento: string;
  categoriaPagamento: string;
  dataLancamento: string;
  textoSapIdUsuario: string;
  textoSap: string;
  descricaoErro: string;
  checked: boolean;
}
