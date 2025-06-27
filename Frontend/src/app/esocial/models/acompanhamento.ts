class listaFormulario{
  idAcompExecFormularios : number;
  idF2500 : number;
  idF2501 : number;
}
export class Acompanhamento {
  constructor(idAcompanhamentoExecutor : number,
              codTipoFormulario : number,
              datInicioExecucao : Date,
              statusExecucao :number,
              datFimExecucao : Date,
              erroMensagem : string,
              erroDetalhamento : string,
              nomeArquivo : string,
              listaFormularios : Array<listaFormulario>,
              descricaoTipoFormulario : string,
              decricaoStatusExecucao : string
              ){

  this.idAcompanhamentoExecutor = idAcompanhamentoExecutor;
  this.codTipoFormulario = codTipoFormulario;
  this.datInicioExecucao = datInicioExecucao;
  this.statusExecucao =statusExecucao;
  this.datFimExecucao = datFimExecucao;
  this.erroMensagem = erroMensagem;
  this.erroDetalhamento = erroDetalhamento;
  this.nomeArquivo = nomeArquivo;
  this.listaFormularios = listaFormularios;
  this.descricaoTipoFormulario = descricaoTipoFormulario;
  this.decricaoStatusExecucao = decricaoStatusExecucao;

  }


readonly idAcompanhamentoExecutor : number;
readonly codTipoFormulario : number;
readonly datInicioExecucao : Date;
readonly statusExecucao : number;
readonly datFimExecucao : Date;
readonly erroMensagem : string;
readonly erroDetalhamento : string;
readonly nomeArquivo : string;
readonly listaFormularios : Array<listaFormulario>;
readonly descricaoTipoFormulario :string;
readonly decricaoStatusExecucao : string;


static fromObj(lista :any ) : Acompanhamento{
 return new Acompanhamento(lista.idAcompanhamentoExecutor,
                           lista.codTipoFormulario,
                           lista.datInicioExecucao,
                           lista.statusExecucao,
                           lista.datFimExecucao,
                           lista.erroMensagem,
                           lista.erroDetalhamento,
                           lista.nomeArquivo,
                           lista.listaFormularios,
                           lista.descricaoTipoFormulario,
                           lista.decricaoStatusExecucao);
}

}
