export class ContratoEscritorioRequest {
    codTipoContratoEscritorio: number;
    datInicioVigencia: Date;
    datFimVigencia: Date;
    cnpj: string = '';
    numContratoJecVc: string = '';
    numContratoProcon: string = '';
    nomContrato: string = '';
    valUnitarioJecCc: number | null;
    valUnitarioProcon: number | null;
    valUnitAudCapital: number | null;
    valUnitAudInterior: number | null;
    valVep: number | null;
    numSgpagJecVc: string = '';
    numSgpagProcon: number | null;
    indPermanenciaLegado: string = '';
    numMesesPermanencia: number | null;
    valDescontoPermanencia: number | null;
    indAtivo: string = '';
    indConsideraCalculoVep: string = '';
    contratoAtuacao: number | null;
    contratoDiretoria: number | null;
    escritorios: number[] = [];
    estados: string[] = [];
}