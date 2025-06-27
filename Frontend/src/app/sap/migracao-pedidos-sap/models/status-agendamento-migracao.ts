import { IStatusAgendamentoMigracao } from './i-status-agendamento-migracao';

export abstract class StatusAgendamentoMigracao {

    static readonly NAO_DEFINIDO: IStatusAgendamentoMigracao = { id: -1, nome: 'Não Definido' };
    static readonly AGENDADO: IStatusAgendamentoMigracao = { id: 1, nome: 'Agendado' };
    static readonly PROCESSANDO: IStatusAgendamentoMigracao = { id: 2, nome: 'Processando' };
    static readonly FINALIZADO: IStatusAgendamentoMigracao = { id: 3, nome: 'Finalizado' };
    static readonly CANCELADO: IStatusAgendamentoMigracao = { id: 4, nome: 'Cancelado' };
    static readonly ERRO: IStatusAgendamentoMigracao = { id: 5, nome: 'Erro' };

    static obterTodos(): Array<IStatusAgendamentoMigracao> {
        return [
            this.NAO_DEFINIDO,
            this.AGENDADO,
            this.PROCESSANDO,
            this.FINALIZADO,
            this.CANCELADO,
            this.ERRO
        ];
    }

    static obterTexto(id: number | string): string {
        switch (id) {
            case this.NAO_DEFINIDO.id:
                return this.NAO_DEFINIDO.nome;
            case this.AGENDADO.id:
                return this.AGENDADO.nome;
            case this.PROCESSANDO.id:
                return this.PROCESSANDO.nome;
            case this.FINALIZADO.id:
                return this.FINALIZADO.nome;
            case this.CANCELADO.id:
                return this.CANCELADO.nome;
            case this.ERRO.id:
                return this.ERRO.nome;
            default:
                return this.AGENDADO.nome;
        }
    }
}
