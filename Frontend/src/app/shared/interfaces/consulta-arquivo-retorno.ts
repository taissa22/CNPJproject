import { NumeroRange } from '@shared/interfaces/numero-range';
import { DataRange } from '@shared/interfaces/data-range';
export interface ConsultaArquivoRetorno {

    dataRemessa?: {dataInicio: string, dataFim: string};
    numeroRemessa?: NumeroRange;
    intervaloValoresGuia?: NumeroRange;
    guia?: number[];
    contaJudicial?: number[];
    juizadoEspecial?: number[];
    civelConsumidor?: number[];
    pex?: number[];

}
