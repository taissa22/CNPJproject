// -------------- Esse é um arquivo de constantes de todo o SAP --------------------------  //


//#region Nomeclatura das telas para ser enviadas ao endpoint e fazer a permissão das informações
/**
   * Variavel que contem todas as telas e seus respectivos nomes.
   */
export const tela = {
  consultaControleAcompanhamentoLote: 'consultaLote',
  criacaoLote: 'criaLote',
  consultaSaldoGarantia: 'consultaSaldoGarantia'
};
//#endregion

//#region Permissões do Menu a serem atribuidas no CanActive do SAP routing.
/**
   * Variavel que contem todas as permissoes referentes ao menu do SAP.
   */
export const role = {
  menuConsultaControleAcompanhamentoLote: 'm_ConsultaLotesSAP',
  menuCriacaoLote: 'm_CriarLotesSAP',
  menuCategoriaPagamento: 'm_CategoriasDePagamento',
  menuManutencaoCentroCusto: "m_CentrosDeCustoDoSap",
  menuManutencaoEmpresaSap: 'm_EmpresasDoSap',
  menuManutencaoFormaPagamento: "m_FormasDePagamento",
  menuManutencaoFornecedores: "m_Fornecedores",
  menuInterfaceComarcarBB: 'm_ComarcasBB',
  menuInterfaceTribunaisBB: 'm_TribunaisBB',
  menuInterfaceModalidadeProdutoBB: 'm_ModalidadeProdutoBB',
  menuInterfaceNaturezacaoAcoesBB: 'm_NaturezaAcoesBB',
  menuInterfaceOrgaosBB: 'm_OrgaosBB',
  menuInterfaceStatusParcelaBB: 'm_StatusParcelaBB',
  menuEstornoLancamento: 'm_EstornoDeLancamentos',
  menuManutencaoGrupoLoteJuizado: 'm_GrupoLoteJuizado',
    menuInterfaceImportacaoConsultaArquivoRetorno: 'm_ImportaConsultaArquivoRetorno',
    menuManutencaoFornecedoresContingencia: 'm_FornecedoresContingenciaSAP',
  menuConsultaSaldoGarantia: 'm_ConsultaSaldoGarantias',
  menuAgendaAudiencia: 'm_AgendaAudienciaTrabalhista',
  menuMigracaoPedidoSAP: 'm_MigrarPedidoSAP'
};
//#endregion


//#region
/**
   * Valor referente à um MB
   */
export const UM_MEGABYTE = 1048576;
//#endregion
