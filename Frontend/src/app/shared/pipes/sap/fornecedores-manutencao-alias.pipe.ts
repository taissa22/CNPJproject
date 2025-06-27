import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'fornecedoresManutencaoAlias'
})
export class FornecedoresManutencaoAliasPipe implements PipeTransform {

  transform(key) {
    return { 
      nomeFornecedor: 'Nome do Fornecedor',
      id: 'Código',
      codigoFornecedorSap: 'Código do Fornecedor SAP',
      nomeTipoFornecedor: 'Tipo',
      nomeProfissional: 'Profissional',
      nomeBanco: 'Banco',
      nomeEscritorio: 'Escritório'
    }[key];
  }

}
