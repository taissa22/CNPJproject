import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'empresasSAPManutencaoAlias'
})
export class EmpresasSAPManutencaoAliasPipe implements PipeTransform {

  transform(key: string): string {
    return {
      id: 'Código',
      sigla: 'Sigla',
      nome: 'Nome',
      indicaEnvioArquivoSolicitacao: 'Envio SAP',
      indicaAtivo: 'Ativa',
      codigoOrganizacaocompra: 'Organização de Compras'
    }[key];
  }

}
