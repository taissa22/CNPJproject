import { throwError } from "rxjs"
import { RetornoLista } from "../models/retorno-lista"

export class StatusEsocial {
  static defineCorTagStatusFormulario(tag : number) {
    switch (tag) {
      case 0:
        return 'white'
      case 1:
        return '#77AEDF'
      case 2:
        return '#3270A7'
      case 3:
        return '#E9CE1E'
      case 4:
        return '#19A519'
      case 5:
        return '#B1C40A'
      case 6:
        return '#FC0404'
      case 7:
        return '#C77B38' 
      case 10:
        return '#FF8C00'
      case 11:
        return '#A39E1F'
      case 12:
        return '#14273A'
      case 13:
        return '#4F6F8F'
      case 14:
        return '#29435D'
      case 15:
        return '#975757'
      case 16:
        return '#FC0404'
      case 17:
        return '#C77B38'
      default:
        return 'grey'
    }
  };

  static defineCorTagStatusReclamante(tag) {
    switch (tag) {
      case "Pendente de análise":
        return 'white';
      case "Não elegível para eSocial":
        return '#9597A6';
      case "Elegível para eSocial":
        return '#3E93DF';
    }
  };

  static defineCorRetificacao(){
    return "#800040"
  };

  static obtemDescricaoStatus(status: number, statusFormularioList: Array<RetornoLista>): string {
    if ( (typeof statusFormularioList) === (typeof new Array<RetornoLista>()) ) {
      throwError("Lista de status em formato inválido");
    }
    const statusAtual = statusFormularioList.find(statusItem => { return (statusItem.id == status)});
    const descricao = statusAtual != null ? statusAtual.descricao :  "[Status não implementado]";
    return descricao ;
  }

  static obtemDescricaoRetorno(status: number, textoComplementar: string) {
    let retorno = '';
    switch (status) {
      case 1:
        retorno = 'Salvo como rascunho por ';
        break;
      case 2:
        retorno = 'Salvo para envio por ';
        break;
      case 3:
        retorno = 'Enviado ';
        break;
      case 4:
        retorno = 'Retornado ';
        break;
      case 5:
        retorno = 'Retornado ';
        break;
      case 6:
        retorno = 'Erro retornado ';
        break;
      case 7:
        retorno = 'Retornado ';
        break;  
      case 10:
        retorno = 'Processado ';
        break;
      case 11:
        retorno = 'Retornado ';
        break;
      case 12:
        retorno = 'Excluído ';
        break;
      case 13:
        retorno = 'Exclusão solicitada';
        break;
      case 14:
        retorno = 'Exclusão enviada ';
        break;
      case 15:
        retorno = 'Exclusão não ok ';
        break;
      case 16:
        retorno = 'Erro retornado ';
        break;
      case 17:
        retorno = 'Retornado ';
        break;
      default:
        retorno = "[Status não implementado]"
    }
    return retorno + textoComplementar;
  }
}
