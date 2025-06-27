export class AgendamentoRelatorioNegociacaoRequest{
    indProcessoCc:boolean;
    indProcessoJec:boolean;
    indProcessoProcon:boolean;
    periodicidadeExecucao:number;
    datProxExec?: Date;
    diaDaSemana?:number;
    indUltimoDiaMes:boolean;
    diaDoMes?:number;
    datInicioNegociacao?: Date;
    datFimNegociacao?: Date;
    periodoSemanal?:number;
    periodoMensal?:number;
    indNegociacoesAtivas:boolean;
}