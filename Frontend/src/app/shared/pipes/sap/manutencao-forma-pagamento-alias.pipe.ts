import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'manutencaoFormaPagamentoAlias'
})
export class ManutencaoFormaPagamentoAliasPipe implements PipeTransform {

  transform(key: string): any {
    const keys = { 
      codigo: 'Código',
      descricaoFormaPagamento: 'Descrição da Forma de Pagamento',
      requerBordero: 'Requer Borderô',
      restrita: 'Restrita'
    }
    return keys.hasOwnProperty(key) ? keys[key] : key;
  }

}
