import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'fornecedoresContingenciaSapAlias'
})
export class FornecedoresContingenciaSapAliasPipe implements PipeTransform {

  transform(key: string): string {
    return {
      id: 'Código',
      nome: 'Nome do Fornecedor',
      codigo: 'Código do Fornecedor SAP',
      cnpj: 'CNPJ',
      valorCartaFianca: 'Valor da Carta de Fiança',
      dataVencimentoCartaFianca: 'Data de Vencimento da Carta de Fiança',
      statusFornecedor: 'Status do Fornecedor'
    }[key];
  }
}
