import { isNullOrUndefined } from 'util';
import { StatusAgendamento } from './status-agendamento.enum';
export class Compromisso {
  constructor() { }

  id: string | number;
  codAgendCargaComp : string | number;
  datAgendamento: Date;
  dataExecucao: Date;
  dataFinalizacao: Date;
  usuarioSolicitanteId: string | number;
  status: string | number;
  nomeArquivoGerado: string;
  nomeArquivoBase: string;
  mensagemErro: string; 
  descricaoResultadoCarga: string;
  mensagem: string;
  tipoProcesso: string;
  configExec: string;
  tipoProcessoDescription: string;
  configExecDescription: string;
  usrCodUsuario: string;

  static fromJson(d: Compromisso, tipoProcessoDescription:string ): Compromisso {
    const compromisso = new Compromisso();
    compromisso.id = d.codAgendCargaComp ;
    compromisso.datAgendamento = isNullOrUndefined(d.datAgendamento) ? null : new Date(d.datAgendamento);
    compromisso.dataExecucao = isNullOrUndefined(d.dataExecucao) ? null : new Date(d.dataExecucao);
    compromisso.dataFinalizacao = isNullOrUndefined(d.dataFinalizacao) ? null : new Date(d.dataFinalizacao);
    compromisso.usuarioSolicitanteId = d.usuarioSolicitanteId;
    compromisso.status = d.status;
    compromisso.nomeArquivoGerado = d.nomeArquivoGerado;
    compromisso.nomeArquivoBase= d.nomeArquivoBase;
    compromisso.mensagemErro = d.mensagemErro;
    compromisso.descricaoResultadoCarga = d.descricaoResultadoCarga;
    compromisso.tipoProcesso = d.tipoProcesso;
    compromisso.configExec = d.configExec;
    compromisso.tipoProcessoDescription = tipoProcessoDescription;
    compromisso.configExecDescription = d.configExec? this.obterDescriptionConfigExec(d.configExec): '';
    compromisso.usrCodUsuario = d.usrCodUsuario;
    compromisso.mensagem = d.mensagem;
    //this.obterMensagemStatusAgendamento(d.status.toString(), d.descricaoResultadoCarga);
    return compromisso;
  }

   static obterMensagemStatusAgendamento(status: string, descricaoResultado: string): string {
    switch (status) {
      case StatusAgendamento.FINALIZADO.id:
        return descricaoResultado;
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

  static obterDescriptionConfigExec(configExec: string):string {
    const configExecNumero = parseInt(configExec.trim(), 10);
    if (configExecNumero==0)
      return "Execução imediata";
    else
      return "Execução Data Específica";
  }

  get obterDataFinalizacao(): string {
    const optionsLocale = { year: 'numeric', month: 'numeric', day: 'numeric', hour: 'numeric', minute: 'numeric' };
    return this.dataFinalizacao ? this.dataFinalizacao.toLocaleString('pt-BR', optionsLocale) : '';
  }
  get obterDataAgendamento(): string {
    const optionsLocale = { year: 'numeric', month: 'numeric', day: 'numeric', hour: 'numeric', minute: 'numeric' };
    return this.datAgendamento ? this.datAgendamento.toLocaleString('pt-BR', optionsLocale) : '';
  }
}
