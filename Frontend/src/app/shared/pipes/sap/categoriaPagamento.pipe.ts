import { Pipe, PipeTransform } from '@angular/core';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';

@Pipe({
  name: 'categoriaPagamentoAlias'
})
export class CategoriaPagamentoPipe implements PipeTransform {


  transform(key: string, tipoProcesso: number): string {
    return {
      codigo: 'Código',
      descricao: 'Descrição da Categoria de Pagamento',
      indAtivo: 'Ativo',
      codigoMaterialSAP: 'Código do Material SAP',
      indEnvioSap: 'Envia SAP',
      indInfluenciaContigencia: 'Influenciar a Contigencia',
      descricaoclassegarantia: 'Classe de Garantia',
      indicadorNumeroGuia: 'Exige Nº Guia',
      indicadorFinalizacaoContabil: 'Registrar em Processos Finalizados Contabilmente',
      fornecedoresPermitidos: 'Fornecedores Permitidos',
      indEscritorioSolicitaLan: 'Escritório pode Solicitar',
      grupoCorrecao: 'Grupo de Correção',
      indEncerraProcessoContabil: 'Encerrar Processos Contabilmente',
      indComprovanteSolicitacao: 'Requer Comprovante na Solicitação',
      indicadorRequerDataVencimento: 'Requer Data Vencimento Documento',
      indicadorContingencia: tipoProcesso == TipoProcessoEnum.trabalhista ?
        'Influenciar a Contingência Por Média' :
        'Influenciar a Contingência',
      descricaoJustificativa: 'Justificativa',
      responsabilidadeOi: '% de Responsabilidade Oi',
      indicadorHistorico: 'Histórica',
      descricaoEstrategico: 'Correspondente Cível Estratégico (De x PARA migração de processo)',
      descricaoConsumidor: 'Correspondente Cível Consumidor (De x PARA migração de processo)',
    }[key];
  }
}
