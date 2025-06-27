import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'SaldoGarantianResultadoAlias'
})
export class SaldoGarantiaResultadoAliasPipe implements PipeTransform {

  transform(key: any): any {
    return {
    idProcesso: 'Código Interno do Processo',
    numeroProcesso: 'Processo',
    codigoEstado: 'Estado',
    descricaoComarca: 'Comarca',
    codigoVara: 'Vara',
    descricaoTipoVara: 'Tipo de Vara',
    descricaoEmpresaGrupo: 'Empresa do Grupo',
    ativo: 'Ativo',
    descricaoBanco: 'Banco',
    descricaoEscritorio: 'Escritório',
    dataFinalizacaoContabil: 'Data Finalização Contábil',
    descricaoTipoGarantia: 'Tipo Garantia',
    valorPrincipal: 'Valor Principal',
    valorCorrecaoPrincipal: 'Correção Principal',
    valorAjusteCorrecao: 'Ajuste Correção',
    valorJurosPrincipal: 'Juros Principal',
    valorAjusteJuros: 'Ajuste Juros',
    valorPagamentoPrincipal: 'Pagamento Principal',
    valorPagamentoCorrecao: 'Pagamento Correção',
    valorPagamentosJuros: 'Pagamentos Juros',
    valorLevantadoPrincipal: 'Levantado Principal',
    valorLevantadoCorrecao: 'Levantado Correção',
    valorLevantadoJuros: 'Levantado Juros',
    valorSaldoPrincipal: 'Saldo Principal',
    valorSaldoCorrecao: 'Saldo Correção',
      valorSaldoJuros: 'Saldo Juros'
    }[key];
  }

}
