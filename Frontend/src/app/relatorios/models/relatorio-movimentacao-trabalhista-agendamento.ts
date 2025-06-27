export class RelatorioMovimentacaoTrabalhistaAgendamento{
id: number;
datAgendamento: Date;

iniDataFechamento: Date;
iniNumMesesMediaHistorica: number;
iniCodTipoOutlier: number;
iniValOutlier: number;
iniIndMensal: string;
iniIndFechamentoParcial: boolean;
iniEmpresas: string;

fimDataFechamento: Date;
fimNumMesesMediaHistorica: number;
fimCodTipoOutlier: number;
fimValOutlier: number;
fimIndMensal: string;
fimIndFechamentoParcial: boolean;
fimEmpresas: string;

datInicioExecucao: Date;
datFimExecucao: Date;
usrCodUsuario: string;
status: number; // Status do agendamento (0 - Agendado, 1 - Processando, 2 - Finalizado, 3 - Erro)
msgErro: string;
nomArquivo: string;

}