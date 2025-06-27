import { DualListModel } from 'src/app/core/models/dual-list.model';
import { FormGroup, FormControl } from '@angular/forms';
import { formatCurrency, formatDate } from '@angular/common';
import xml2js from 'xml2js';

export function toInteger(value: any): number {
  return parseInt(`${value}`, 10);
}

export function toString(value: any): string {
  return (value !== undefined && value !== null) ? `${value}` : '';
}
export function getValueInRange(value: number, max: number, min = 0): number {
  return Math.max(Math.min(value, max), min);
}

export function isString(value: any): value is string {
  return typeof value === 'string';
}

// export function concatenarNomeExportacao(origem : string) {
//     return origem + '_' + formatDate(new Date(), 'yyyyMMdd_HHmmss', 'pt_BR') + '.csv';
//   }

export function isNumber(value: any): value is number {
  return !isNaN(toInteger(value));
}

export function isInteger(value: any): value is number {
  return typeof value === 'number' && isFinite(value) && Math.floor(value) === value;
}

export function isDefined(value: any): boolean {
  return value !== undefined && value !== null;
}

export function padNumber(value: number) {
  if (isNumber(value)) {
    return `0${value}`.slice(-2);
  } else {
    return '';
  }
}

export function money(value: number): string {
  return formatCurrency(value, 'pt', '', '', '1.2-2');
}

export function regExpEscape(text) {
  return text.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, '\\$&');
}


export function hasClassName(element: any, className: string): boolean {
  return element && element.className && element.className.split &&
    element.className.split(/\s+/).indexOf(className) >= 0;
}

if (typeof Element !== 'undefined' && !Element.prototype.closest) {
  // Polyfill for ie10+

  if (!Element.prototype.matches) {
    // IE uses the non-standard name: msMatchesSelector
    Element.prototype.matches = (Element.prototype as any).msMatchesSelector || Element.prototype.webkitMatchesSelector;
  }

  Element.prototype.closest = function (s: string) {
    let el = this;
    if (!document.documentElement.contains(el)) {
      return null;
    }
    do {
      if (el.matches(s)) {
        return el;
      }
      el = el.parentElement || el.parentNode;
    } while (el !== null && el.nodeType === 1);
    return null;
  };
}

export function closest(element: HTMLElement, selector): HTMLElement {
  if (!selector) {
    return null;
  }

  return element.closest(selector);
}

/**
   * Retorn o texto padronizado de error do formulário.
   *
   * @param formulario: formulário.
   * @param nomeControl: nome do formControl a ser validado. Ex dataInicial
   * @param nomeCampo: nome que aparecerá na tela. Ex: Data Inicial é obrigatório!
   * @param campoFemino: verifica se deve ser adicionado as verificações com feminino.
   * Ex: Classe obrigatória!
   */
export function textosValidacaoFormulario(formulario: FormGroup,
  nomeControl: string,
  nomeCampo: string, campoFeminino: boolean) {
  if (
    formulario.invalid &&
    formulario.get(nomeControl).touched &&
    formulario.get(nomeControl).hasError('required') ||
    formulario.get(nomeControl).touched &&
    formulario.get(nomeControl).hasError('whitespace')
  ) {
    if (!campoFeminino) {
      return `O ${nomeCampo} é obrigatório!`;
    } else {
      return `A ${nomeCampo} é obrigatória!`;
    }

  }
  if (
    formulario.invalid &&
    formulario.get(nomeControl).touched &&
    formulario.get(nomeControl).hasError('minlength')
  ) {
    return `${nomeCampo} deve possuir no mínimo
                  ${
      formulario.get(nomeControl).errors.minlength
        .requiredLength
      } caracteres!`;
  }
  if (
    formulario.invalid &&
    formulario.get(nomeControl).touched &&
    formulario.get(nomeControl).hasError('maxlength')
  ) {
    return `${nomeCampo} deve possuir no máximo
                  ${
      formulario.get(nomeControl).errors.maxlength
        .requiredLength
      } caracteres!`;
  }
}

export const TiposProcessosMapped = [{
  idTipo: 1,
  nome: 'Cível Consumidor'
},
{
  idTipo: 2,
  nome: 'Trabalhista'
},
{
  idTipo: 7,
  nome: 'Juizado Especial'
},
{
  idTipo: 9,
  nome: 'Cível Estratégico'
},
{
  idTipo: 18,
  nome: 'PEX'
},
{
  idTipo: 4,
  nome: 'Tributário Administrativo'
},
{
  idTipo: 5,
  nome: 'Tributário Judicial'
},
{
  idTipo: 3,
  nome: 'Administrativo'
},
{
  idTipo: 17,
  nome: 'Procon'
}

];

export const TiposProcessosCivelPluralMapped = [{
  idTipo: 1,
  nome: 'Cíveis Consumidor'
},
{
  idTipo: 2,
  nome: 'Trabalhista'
},
{
  idTipo: 7,
  nome: 'Juizado Especial'
},
{
  idTipo: 9,
  nome: 'Cíveis Estratégico'
},
{
  idTipo: 18,
  nome: 'PEX'
},
{
  idTipo: 4,
  nome: 'Tributário Administrativo'
},
{
  idTipo: 5,
  nome: 'Tributário Judicial'
},
{
  idTipo: 3,
  nome: 'Administrativo'
},
{
  idTipo: 17,
  nome: 'Procon'
}

];



export const StatusPagamentos = [{
  idStatus: 1,
  nomeStatus: 'Novo - Aguardando Geração de Lote',
  detalhe: '',
  cor: ''
},
{
  idStatus: 2,
  nomeStatus: 'Lote Gerado - Aguardando Envio para o SAP',
  detalhe: '',
  cor: '#6DAFE9'
},
{
  idStatus: 3,
  nomeStatus: 'Lote Cancelado',
  detalhe: '',
  cor: '#F80000'
},
{
  idStatus: 4,
  nomeStatus: 'Lote Enviado - Aguardando Criação do Pedido SAP',
  detalhe: '',
  cor: '#3C89CC'
},
{
  idStatus: 5,
  nomeStatus: 'Erro na Criação do Pedido SAP - Aguardando Criação de Lote',
  detalhe: '',
  cor: '#FF8C00'
},
{
  idStatus: 6,
  nomeStatus: 'Pedido SAP Criado - Aguardando Recebimento Fiscal',
  detalhe: '',
  cor: '#3270A7'
},
{
  idStatus: 7,
  nomeStatus: 'Aguardando Envio para Cancelamento do Pedido SAP',
  detalhe: '',
  cor: '#FF8C00'
},
{
  idStatus: 8,
  nomeStatus: 'Pedido SAP Enviado - Aguardando Cancelamento',
  detalhe: '',
  cor: '#FC4646'
},
{
  idStatus: 9,
  nomeStatus: 'Erro no Cancelamento do Pedido SAP',
  detalhe: '',
  cor: '#FF8C00'
},
{
  idStatus: 10,
  nomeStatus: 'Pedido SAP Cancelado - Aguardando Geração de Lote',
  detalhe: '',
  cor: '#FF8C00'
},
{
  idStatus: 11,
  nomeStatus: 'Pedido SAP Pago',
  detalhe: '',
  cor: '#19A519'
},
{
  idStatus: 12,
  nomeStatus: 'Pedido SAP Pago Manualmente',
  detalhe: '',
  cor: '#19A519'
},
{
  idStatus: 13,
  nomeStatus: 'Estorno',
  detalhe: '',
  cor: ''
},
{
  idStatus: 14,
  nomeStatus: 'Sem Lançamento para o SAP',
  detalhe: '',
  cor: ''
},
{
  idStatus: 15,
  nomeStatus: 'Pedido SAP Recebido Fiscal - Aguardando Confirmação de Pagamento',
  detalhe: '',
  cor: '#19CE19'
},
{
  idStatus: 17,
  nomeStatus: 'Sem Lançamento para o SAP (Histórico)',
  detalhe: '',
  cor: ''
},
{
  idStatus: 18,
  nomeStatus: 'Lançamento de Controle',
  detalhe: '',
  cor: ''
},
{
  idStatus: 23,
  nomeStatus: 'Pedido SAP Retido - RJ',
  detalhe: '',
  cor: '#19A519'
},
{
  idStatus: 21,
  nomeStatus: 'Lote Automático Cancelado',
  detalhe: '',
  cor: '#FF0000'
},
{
  idStatus: 22,
  nomeStatus: 'Lançamento Automático Cancelado',
  detalhe: '',
  cor: ''
}];

export function concatenarNomeExportacao(origem: string) {
  return origem + '_' + formatDate(new Date(), 'yyyyMMdd_HHmmss', 'pt_BR') + '.csv';
}

export function removerAcentos(newStringComAcento) {
  let string = newStringComAcento;
  const mapaAcentosHex = {
    a: /[\xE0-\xE6]/g,
    e: /[\xE8-\xEB]/g,
    i: /[\xEC-\xEF]/g,
    o: /[\xF2-\xF6]/g,
    u: /[\xF9-\xFC]/g,
    c: /\xE7/g,
    n: /\xF1/g
  };

  for (const letra in mapaAcentosHex) {
    const expressaoRegular = mapaAcentosHex[letra];
    string = string.replace(expressaoRegular, letra);
  }

  return string;
}

/**
    * Função para ordenação de valores
    * @param header valores a serem ordenados
    * @param ordem ordem a ser seguida
    */
export function ordenateHeader(header, ordem: string[]) {
  const headerOrder = ordem;

  const newHeader = [];
  headerOrder.forEach(order => {
    if (header.includes(order)) {
      newHeader.push(order);
    }
  });
  return newHeader;
}

export function validarObrigatoriedadeCombo(c: FormControl) {
  return c.value ? null : { validarObrigatoriedadeCombo: false };
}

export function isArrayNotEmpty(arr) {
  return Array.isArray(arr) && arr.length > 0;
}

export function renameProperty(obj, newKeys) {
  const keyValues = Object.keys(obj).map(key => {
    const newKey = newKeys[key] || key;
    return { [newKey]: obj[key] };
  });
  return Object.assign({}, ...keyValues);
}


// Funções para ajudar no Crtiérios de consulta

/**
* Essa função retorna o range de datas ou numeros em string para o criterio de pesquisa
* @param data1 primeira data
* @param data2 segunda data
* @param number1 primerio numero
* @param number2 segundo numero
*/
export function getEntreValores(data1?: Date, data2?: Date,
  number1?: number, number2?: number) {
  if (data1 && data2) {
    const optionsLocale = { year: 'numeric', month: 'numeric', day: 'numeric' };
    return data1.toLocaleString('pt-BR', optionsLocale) + ' Até ' +
      data2.toLocaleString('pt-BR', optionsLocale);
  }
  if (number1 && number2) {
    return money(number1) + ' Até' +
      money(number2);
  }
}


  /**
   * Essa função retorna os nomes dos valores escolhidos na dualList
   * @param lista lista no modelo dual list
   * @param valores a lista de ids selecionadas no filtro
   * @param hastitulo (opicional) verifica se possui ou não titulo no dualList
   * @param selecionarTudoSeZerado (opicional) Caso o valor venha zerado, enviar true caso deva selecionar tudo
   */
  export function getNomesDuallistSelecionados(lista: Array<DualListModel>, valores: any[], hastitulo = false, selecionarTudoSeZerado = false, valoresCompostos: boolean = false ): string[] {
    // const componente = this;
    let listaFinal = [];
    if (valores.length>0) {
      if (!hastitulo && !valoresCompostos) {
        listaFinal = lista.filter(item => valores.includes(item.id))
          .map(nome => nome.label);
      } else if (valoresCompostos) {
        listaFinal = lista.filter((item) => {return valores.some(valor => valor.id === item.id && valor.codigoChave === item.codigoChave)})
          .map(nome => nome.label);
      } else {
        lista.forEach(item =>
          item.dados.forEach(dados => listaFinal.push(dados))
        );

        listaFinal = listaFinal.filter(item => valores.includes(item.id)).map(nome => nome.descricao);
      }

      return listaFinal;
    } else if(valores.length == 0 && selecionarTudoSeZerado){

      lista.forEach(element => {
        listaFinal = [...listaFinal, element.label]
      });
      return listaFinal;
    }
    return;
  }


export function statusProcessoString(value) {
  return value === 1 || value === 'S' ? ' Ativo' : value === 2 || value === 'N' ? ' Inativo' : ' Ativo e Inativo';
}

export function considerarMigrados(value) {
  return value === 1 || value === 'S' ? 'Sim' : value === 2 || value === 'N' ? 'Não' : 'Indiferente';
}

export function tipoGarantiaString(valores: any[]) {

  const valor = [];
  if (valores) {
    valores.forEach(item => {
      if (item == '1') {
        valor.push('Depósito');
      } else if (item == '2') {
        valor.push('Bloqueio');
      } else {
        valor.push('Outros');
      }
    });
    return valor;
  }
  return;
}

export function riscoPerdaString(valores: any[]) {
  const valor = [];
  if (valores) {
    valores.forEach(item => {
      if (item == '1' || item == 'PR') {
        valor.push('Provável');
      } else if (item == '2' || item == 'PO') {
        valor.push('Possível');
      } else {
        valor.push('Remoto');
      }
    });
    return valor;
  }
  return;
}

export function simNaoindiferente(valor) {
  if (valor) {
    valor == '1' ? valor = 'Sim' : valor == '2' ? valor = 'Não' :
      valor == '3' ? valor = 'Indiferente' : valor;
    return valor;
  }
  return;
}

export function ativoInativoAmbos(value) {
  if (value) {
    return value === 1 ? ' Ativo' : value === 2 ? ' Inativo' : ' Ativo e Inativo';
  }
  return;

}

// end Critérios de consulta

export function toUpperFirstLetter(str) {
  return str.charAt(0).toUpperCase() + str.slice(1);
}

export function fileListToArray(fileList): File[] {
  return [...fileList]
}

// Funções para o menu

/**
  * Essa função converte o tipo de menu do xml para um objeto legível
  * @param arr xml
  */
export const convertToItemMenu = (arr) => arr.map(menuItemElement => convertCallback(menuItemElement));
const hasChild = element => element.hasOwnProperty("MenuItem");
const elementToItemMenu = item => ({
  titulo: item["Text"],
  permissoes: item["Grants"].split(";"),
  url: item["Url"],
  tooltip: item["ToolTip"],
  filhos: []
});
const convertCallback = menuItemElement => {
  let newElement = elementToItemMenu(menuItemElement["$"]);
  if (hasChild(menuItemElement)) {
    newElement["filhos"] = menuItemElement["MenuItem"].map(subMenuItemElement => convertCallback(subMenuItemElement));
  }
  return newElement;
}

//end funções pro menu

/**
  * Converte um xml para json
  */
export function parseXML(data) {
  let resultado;
  const parser = new xml2js.Parser(
    {
      trim: true,
      explicitArray: true
    });
  parser.parseString(data, (err, result) =>
    resultado = result
  );
  return resultado;
}

/**
 *
 *@description - Formatar a data para enviar para o endpoint
 * @export
 * @param  data - Data a ser formatada
 * @returns - Data em formato ano-mes-dia
 */
export function formatarData(data){
  return formatDate(data, 'yyyy-MM-dd', 'pt_Br')
}
