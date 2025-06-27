import { IStatusAgendamento } from "src/app/pagamentos/models/i-status-agendamento.interface";



export abstract class StatusAgendamentoATMCC {

  static readonly AGENDADO: IStatusAgendamento = { id: 1, nome: 'Agendado' };
  static readonly PROCESSANDO: IStatusAgendamento = { id: 2, nome: 'Processando' };
  static readonly FINALIZADO: IStatusAgendamento = { id: 3, nome: 'Finalizado' }; 
  static readonly ERRO: IStatusAgendamento = { id: 4, nome: 'Erro' };

  static obterTodos(): Array<IStatusAgendamento> {
    return [ 
      this.AGENDADO,
      this.PROCESSANDO,
      this.FINALIZADO, 
      this.ERRO
    ];
  }

  static obterTexto(id: number | string): string {
    switch (id) {
      
      case this.AGENDADO.id:
        return this.AGENDADO.nome;
      case this.PROCESSANDO.id:
        return this.PROCESSANDO.nome;
      case this.FINALIZADO.id:
        return this.FINALIZADO.nome;     
      case this.ERRO.id:
        return this.ERRO.nome;
      default:
        return this.AGENDADO.nome;
    }
  }
}
