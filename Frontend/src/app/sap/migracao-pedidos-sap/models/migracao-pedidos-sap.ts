import { IStatusAgendamentoMigracao } from './i-status-agendamento-migracao';
import { isNullOrUndefined } from 'util';
import { StatusAgendamentoMigracao } from './status-agendamento-migracao';
export class MigracaoPedidos {
    constructor() { }

    id: string | number;
    dataAgendamento: Date;
    dataExecucao: Date;
    dataFinalizacao: Date;
    usuarioSolicitanteId: string | number;
    statusAgendamentoId: string | number;
    statusAgendamento: IStatusAgendamentoMigracao;
    nomeArquivoGerado: string;
    nomeArquivoDeParaMigracao: string;
    mensagemErro: string;
    mensagemErroTrace: string;
    descricaoResultadoCarga: string;

    static fromJson(d: MigracaoPedidos): MigracaoPedidos {
        const migracaoPedidos = new MigracaoPedidos();
        migracaoPedidos.id = d.id;
        migracaoPedidos.dataAgendamento = isNullOrUndefined(d.dataAgendamento) ? null : new Date(d.dataAgendamento);
        migracaoPedidos.dataExecucao = isNullOrUndefined(d.dataExecucao) ? null : new Date(d.dataExecucao);
        migracaoPedidos.dataFinalizacao = isNullOrUndefined(d.dataFinalizacao) ? null : new Date(d.dataFinalizacao);
        migracaoPedidos.usuarioSolicitanteId = d.usuarioSolicitanteId;
        migracaoPedidos.statusAgendamentoId = d.statusAgendamentoId;
        migracaoPedidos.statusAgendamento = d.statusAgendamento;
        migracaoPedidos.nomeArquivoGerado = d.nomeArquivoGerado;
        migracaoPedidos.nomeArquivoDeParaMigracao = d.nomeArquivoDeParaMigracao;
        migracaoPedidos.mensagemErro = d.mensagemErro;
        migracaoPedidos.mensagemErroTrace = d.mensagemErroTrace;
        migracaoPedidos.descricaoResultadoCarga = d.descricaoResultadoCarga;
        return migracaoPedidos;
    }

    obterMensagemStatusAgendamento(): string {
        switch (this.statusAgendamento.id) {
            case StatusAgendamentoMigracao.FINALIZADO.id:
                return this.descricaoResultadoCarga;
            case StatusAgendamentoMigracao.PROCESSANDO.id:
                return 'Esta migração está sendo processada!';
            case StatusAgendamentoMigracao.ERRO.id:
                return 'Ocorreu um erro ao tentar processar a migração. Por favor, faça um novo agendamento.';
            case StatusAgendamentoMigracao.AGENDADO.id:
                return 'Esta migração será processada em breve. Por favor, aguarde.';
            default:
                return 'Esta migração será processada em breve. Por favor, aguarde.';
        }
    }

    get obterDataFinalizacao(): string {
        const optionsLocale = { year: 'numeric', month: 'numeric', day: 'numeric', hour: 'numeric', minute: 'numeric' };
        return this.dataFinalizacao ? this.dataFinalizacao.toLocaleString('pt-BR', optionsLocale) : '';
    }
    get obterDataAgendamento(): string {
        const optionsLocale = { year: 'numeric', month: 'numeric', day: 'numeric', hour: 'numeric', minute: 'numeric' };
        return this.dataAgendamento ? this.dataAgendamento.toLocaleString('pt-BR', optionsLocale) : '';
    }
}
