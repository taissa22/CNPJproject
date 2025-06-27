import { IStatusAgendamento } from './i-status-agendamento.interface';
import { isNullOrUndefined } from 'util';
import { StatusAgendamento } from './status-agendamento.enum';
export class Documento {
  constructor() { }

  id: string | number;
  dataAgendamento: Date;
  dataExecucao: Date;
  dataFinalizacao: Date;
  usuarioSolicitanteId: string | number;
  statusAgendamentoId: string | number;
  statusAgendamento: IStatusAgendamento;
  nomeArquivoGerado: string;
  nomeArquivoBaseFGV: string;
  mensagemErro: string;
  descricaoResultadoCarga: string;

  static fromJson(d: Documento): Documento {
    const documento = new Documento();
    documento.id = d.id;
    documento.dataAgendamento = isNullOrUndefined(d.dataAgendamento) ? null : new Date(d.dataAgendamento);
    documento.dataExecucao = isNullOrUndefined(d.dataExecucao) ? null : new Date(d.dataExecucao);
    documento.dataFinalizacao = isNullOrUndefined(d.dataFinalizacao) ? null : new Date(d.dataFinalizacao);
    documento.usuarioSolicitanteId = d.usuarioSolicitanteId;
    documento.statusAgendamentoId = d.statusAgendamentoId;
    documento.statusAgendamento = d.statusAgendamento;
    documento.nomeArquivoGerado = d.nomeArquivoGerado;
    documento.nomeArquivoBaseFGV = d.nomeArquivoBaseFGV;
    documento.mensagemErro = d.mensagemErro;
    documento.descricaoResultadoCarga = d.descricaoResultadoCarga;
    return documento;
  }

  obterMensagemStatusAgendamento(): string {
    switch (this.statusAgendamento.id) {
      case StatusAgendamento.FINALIZADO.id:
        return this.descricaoResultadoCarga;
      case StatusAgendamento.PROCESSANDO.id:
        return 'Esta carga de documentos está sendo processada!';
      case StatusAgendamento.ERRO.id:
        return 'Ocorreu um erro ao tentar processar a carga de documentos. Por favor, faça uma nova carga.';
      case StatusAgendamento.AGENDADO.id:
        return 'Esta carga de documentos será processada em breve. Por favor, aguarde.';
      default:
        return 'Esta carga de documentos será processada em breve. Por favor, aguarde.';
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
