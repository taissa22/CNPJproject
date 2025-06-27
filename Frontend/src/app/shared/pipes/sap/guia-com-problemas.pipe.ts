import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'guiaComProblemas'
})
export class GuiaComProblemasPipe implements PipeTransform {


  transform(key: string): string {
    return {

      numeroProcesso: 'Processo',
       codigoComarca: 'Cód Comarca BB',
       nomeComarca: 'Nome Comarca BB',
       codigoOrgaoBB: 'Cód Órgão BB',
       nomeOrgaoBB: 'Nome Órgão BB',
       codigoNaturezaAcaoBB: 'Cód Natureza Ação BB',
       nomeNaturezaBB: 'Nome Natureza BB',
       nomeAutor: 'Nome de Autor',
       autorCPF_CNPJ: 'CPF/CNPJ Autor',
       nomeReu: 'Nome do Réu',
       reuCPF_CNPJ: 'CPF/CNPJ Réu',
       valorParcela:'Valor da Parcela',
       dataGuia:'Data da Guia',
       guia: 'Guia',
       numeroConta: 'Número da Conta',
       numeroParcela: 'Número Parcela',
       idBBStatusParcela: 'Cód Status Parcela',
       descricaoErroGuia: 'Situação Processamento SISJUR x BB',
       dataEfetivacaoParcelaBB: 'Efetivação da Parcela' ,
       statusParcelaBB: 'Descrição Status Parcela',
       idProcesso: 'ID Processo',
       idLancamento: 'ID Lançamento'

    }[key];
  }

}
