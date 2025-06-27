import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'ResultadoAgendaAudienciaAlias'
})
export class ResultadoAgendaAudienciaAliasPipe implements PipeTransform {


  transform(key: string): string {
    return {
    siglaEstado: 'Estado',
    comarca: 'Comarca',
    codVara: 'Vara',
    tipoVara: 'Tipo Vara',
    dataAudiencia: 'Data da Audiência',
    horarioAudiencia: 'Hora da Audiência',
    tipoAudiencia: 'Tipo de Audiência',
    preposto: 'Preposto',
    escritorioAudiencia: 'Escritório Audiência',
    advogadoAudiencia: 'Advogado Escritório da Audiência',
    prepostoAcompanhante: 'Preposto Acompanhante',
    escritorioAcompanhante: 'Escritório Acompanhante',
    advogadoAcompanhante: 'Advogado Acompanhante',
    tipoProcesso: 'Tipo de Processo',
    estrategico: 'Estratégico',
    numeroProcesso: 'Nº do Processo',
    classificacaoHierarquica: 'Classificação Hierárquica',
    empresaGrupo: 'Empresa do Grupo',
    endereco: 'Endereço da Audiência',
    escritorioProcesso: 'Escritório do Processo'
    }[key];
  }

}
