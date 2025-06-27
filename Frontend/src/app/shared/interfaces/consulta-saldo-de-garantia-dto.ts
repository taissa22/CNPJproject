import { DataRange } from './data-range';
import { NumberValueAccessor } from '@angular/forms';
export interface ConsultaSaldoDeGarantiaDTO {
    tipoProcesso?: number;
    statusDoProcesso?: number;
    dataFinalizacaoContabilInicio?: string;
    dataFinalizacaoContabilFim?: string;
    valorDepositoInicio?: number;
    valorDepositoFim?: number;
    valorBloqueioInicio?: number;
    valorBloqueioFim?: number;
    UmBloqueio?: boolean;
    tipoGarantia?:number[];
    riscoPerda?: number[]
    //TODO: [marcus]
    agencia?: string;
    conta?: string;
    numeroAgencia?: string;
    numeroConta?: string;
    considerarMigrados?: number;
    idsBanco?: number[];
    idsEmpresaGrupo?: number[];
    idsEstado?: string[];
    idsProcesso?: number[];

}
