export class FiltroProcessoCriminalModel {
    cod_tipo_processo:number;
    cod_escritorio: number; 
    situacao:number;
    numero_processo:string;
    tipo_filtro_numero_processo1:number; //se não passar; filtra pelos dois
    tipo_filtro_numero_processo2:number; //se não passar; filtra por igual
    cod_orgao: number;
    cod_competencia: number;
    cod_estado: string;
    cod_municipio: number;
    cod_comarca: number;
    cod_empresa:number;
    documento_parte:string;
    tipo_filtro_documento_parte:number; //se não passar; filtra pelos dois
    nome_parte:string;
    tipo_filtro_nome_parte:number; //se não passar; filtra por igual
    cod_tipo_procedimento: number;
    cod_assunto:number ;
    cod_tipo_participacao:number; 
    cod_criticidade:number;
    cod_acao:number;
    cpf_testemunha:string;
    checado:number;
    page:number; //se não passar; padrão é 1
    size:number; //se não passar; padrão é 10
}