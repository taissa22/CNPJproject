import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'centrosCustoManutencaoAlias'
})
export class CentrosCustoManutencaoAliaPipe implements PipeTransform {

  transform(key: string): string {
    return {
      codigo: 'Código',
      descricaoCentroCusto: 'Descrição do Centro Custo',
      centroCustoSAP: 'Centro Custo SAP',
      indicaAtivo: 'Ativo',
    }[key];
  }

}
