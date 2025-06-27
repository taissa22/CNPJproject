import { OrdenacaoPaginacaoDTO } from './ordenacao-paginacao-dto';

export interface ConsultaArquivoRetornoFiltroDTO extends OrdenacaoPaginacaoDTO {
    numeroRemessaMenor?: string;
	numeroRemessaMaior?: string;
	idsProcessosCC?: string[];
	idsProcessosPEX?: string[];
	idsProcessosJEC?: string[];
	dataRemessaMenor?: string;
	dataRemessaMaior?: string;
	valorGuiaInicio?: string;
	valorGuiaFim?: string;
	numerosContasJudiciais?: string[];
	idsNumerosGuia?: number[];
}
