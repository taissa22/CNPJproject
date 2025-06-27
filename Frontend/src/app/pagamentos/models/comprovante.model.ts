import { IStatusAgendamento } from './i-status-agendamento.interface';
import { isNullOrUndefined } from 'util';
import { StatusAgendamento } from './status-agendamento.enum';
export class Comprovante {
  constructor() {}
  id: number | string;
  usuarioSolicitanteId: number | string;
  statusAgendamentoId: number | string;
  dataFinalizacao: Date;
  dataAgendamento: Date;
  dataExecucao: Date;
  statusAgendamento: IStatusAgendamento;
  resultadoCarga: number;
  nomeArquivoComprovante: string;
  nomeArquivoBaseSap: string;
  descricaoArquivoGerado: string;
  descricaoResultadoCarga: string;
  mensagemErro: string;


  static fromJson(c: Comprovante): Comprovante {
    const comprovante: Comprovante = new Comprovante();
    comprovante.id = c.id;
    comprovante.usuarioSolicitanteId = c.usuarioSolicitanteId;
    comprovante.statusAgendamentoId = c.statusAgendamentoId;
    comprovante.dataFinalizacao = isNullOrUndefined(c.dataFinalizacao) ? null : new Date(c.dataFinalizacao);
    comprovante.dataAgendamento = isNullOrUndefined(c.dataAgendamento) ? null : new Date(c.dataAgendamento);
    comprovante.dataExecucao = isNullOrUndefined(c.dataExecucao) ? null : new Date(c.dataExecucao);
    comprovante.statusAgendamento = c.statusAgendamento;
    comprovante.resultadoCarga = c.resultadoCarga;
    comprovante.nomeArquivoComprovante = c.nomeArquivoComprovante;
    comprovante.nomeArquivoBaseSap = c.nomeArquivoBaseSap;
    comprovante.descricaoArquivoGerado = c.descricaoArquivoGerado;
    comprovante.descricaoResultadoCarga = c.descricaoResultadoCarga;
    comprovante.mensagemErro = c.mensagemErro;

    return comprovante;
  }

  obterMensagemStatusAgendamento(): string {
    switch (this.statusAgendamento.id) {
      case StatusAgendamento.FINALIZADO.id:
        return this.descricaoResultadoCarga;
      case StatusAgendamento.PROCESSANDO.id:
        return 'Esta carga de comprovantes está sendo processada!';
      case StatusAgendamento.ERRO.id:
        return 'Ocorreu um erro ao tentar processar a carga de comprovantes. Por favor, faça uma nova carga.';
      case StatusAgendamento.AGENDADO.id:
        return 'Esta carga de comprovantes será processada em breve. Por favor, aguarde.';
      default:
        return 'Esta carga de comprovantes será processada em breve. Por favor, aguarde.';
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
