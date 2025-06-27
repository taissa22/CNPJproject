import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'cadastrosCompartilhadosTipoAudienciaAlias'
})
export class CadastrosCompartilhadosTipoAudienciaAliasPipe implements PipeTransform {

  transform(key: string): string {
    return {
      CodTipoAudiencia: 'Codigo Audiencia',
      Sigla: 'Sigla',
      Descricao: 'Descricao',
      TipoProcesso:'Tipo Processo',
      EstaAtivo:'Estado Ativo'
    }[key];
  }

}
