export const txtErroCriacaoLote: {
  identificaoLote: string,
  bordero: string,
  lancamento: string,
  valorTotal: string,
  loteValido: string,
  gerarLote: string
  } = {
  identificaoLote: `Para gerar o lote, o campo "Identificação do Lote" deve ser preenchido.`,
  bordero: `A forma de pagamento do lote obriga a existência de borderô.`,
  lancamento: `Para gerar o lote, pelo menos um lançamento deve ser selecionado.`,
  valorTotal: `O valor total dos borderôs deve ser igual ao total dos lançamentos.`,
  loteValido: `Os campos centro de custo SAP e Código Fornecedor SAP
                                    devem estar devidamente configurado. Por favor, entre em contato com o administrador
                                    do sistema.`,
  gerarLote: 'Desculpe! O lote não pode ser gerado.'
  };
