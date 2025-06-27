export class ProcessoCriminalModel{
    id: number;
    nro_processo: string;
    procedimento: string;
    orgao: string;
    competencia: string;
    estado: string;
    municipio: string;
    assunto: Array<string>;
    criticidade: string;
    status: string;
    instauracao: string;
    acao: string;
    comarca: string;
    vara: string;
    nro_processo_antigo: string;
    data_ultimo_acesso: string;
    partes: {key: string,value:Array<string>};
}