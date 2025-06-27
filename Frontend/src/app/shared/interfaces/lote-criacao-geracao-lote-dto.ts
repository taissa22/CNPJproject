import { LoteCriacaoBorderoDto } from './lote-criacao-bordero-dto';
import { LoteCriacaoLancamentoDto } from './lote-criacao-lancamento-dto';

export interface LoteCriacaoGeracaoLoteDto {
  IdentificacaoLote: string;
  ValorLote: number;
  codigoTipoProcesso: number;
  CodigoParteEmpresa: number;
  CodigoFornecedor: number;
  CodigoCentroCusto: number;
  CodigoFormaPagamento: number;
  CodigoCentroSAP: number;
  Borderos: LoteCriacaoBorderoDto[];
  DadosLancamentoDTOs: LoteCriacaoLancamentoDto[];
}
