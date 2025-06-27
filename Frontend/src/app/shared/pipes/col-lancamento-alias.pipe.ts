import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'colLancamentoAlias'
})
export class ColLancamentoAliasPipe implements PipeTransform {

  transform(key: string): string {
    const keys = {
      numeroProcesso: 'Nº Processo',
      descricaoEscritorio: 'Escritório',
      nomeAutor: 'Autor',
      valorLiquido: 'Valor',
      descricaoComarca: 'Comarca',
      descricaoVara: 'Vara',
      descricaoTipoLancamento: 'Tipo de Lançamento',
      descricaoCategoriaPagamento: 'Categoria de Pagamento',
      dataCriacaoLancamento: 'Data de Criação do Lançamento',
      textoSAPIdentificacaoDoUsuario: 'Identificação do Usuário',
      textoSAP: 'Texto SAP',
      mensagemErro: 'Descrição do Erro',
      numeroGuia: 'Número da Guia',
    };
    if(keys.hasOwnProperty(key)) {
      return keys[key];
    }
    return key;
  }

}
