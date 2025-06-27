export class Permissoes {
    static readonly ACESSAR_ASSUNTO_CIVEL_CONSUMIDOR: string = 'm_CadastroAssuntoCC';
    static readonly ACESSAR_ACAO_CIVEL_CONSUMIDOR: string = 'm_CadastroAcaoCC';
    static readonly ACESSAR_PEDIDO_CIVEL_CONSUMIDOR: string = 'mPedidoCivel';
    static readonly ACESSAR_ASSUNTO_CIVEL_ESTRATEGICO: string = 'm_CadastroAssuntoCE';
    static readonly ACESSAR_ACAO_CIVEL_ESTRATEGICO: string = 'm_CadastroAcaoCE';
    static readonly ACESSAR_PEDIDO_CIVEL_ESTRATEGICO: string = 'm_CadastroPedidoCE';
    static readonly ACESSAR_ACAO_TRABALHISTA: string = 'm_CadastroAcaoTrab';
    static readonly ACESSAR_PEDIDO_TRABALHISTA: string = 'm_CadastroPedidoTrab';
    static readonly ACESSAR_ACAO_TRIBUTARIA_JUDICIAL: string = 'm_CadastroAcaoTribJud';
    static readonly ACESSAR_ESTABELECIMENTO: string = 'm_CadastroEstabelecimento';
    static readonly ACESSAR_FIEL_DEPOSITARIO: string = 'm_CadastroFielDepositario';
    static readonly ACESSAR_PARTE: string = 'm_CadastroParte';
    static readonly ACESSAR_GRUPO_DE_ESTADOS: string = 'm_GruposEstadosPEX';
    static readonly ACESSAR_COMARCA: string = 'm_ComarcaWeb';

    // PERMISSÕES DE ÓRGÃO
    static readonly ACESSAR_ORGAO: string = 'm_Orgaos';
    static readonly ALTERAR_ORGAO_CRIMINAL_ADMINISTRATIVO: string = 'f_OrgaosCriminalAdministrativo';
    static readonly ALTERAR_ORGAO_CIVEL_ADMINISTRATIVO: string = 'f_OrgaosCivelAdministrativo';
    static readonly ALTERAR_ORGAO_DEMAIS_TIPOS: string = 'f_OrgaosDemaisTipos';

    static readonly ACESSAR_PROFISSIONAL: string = 'm_CadastroProfissional';
    static readonly ACESSAR_EMPRESA_DO_GRUPO: string = 'm_CadastrarEmpresaGrupo';
    static readonly ACESSAR_EMPRESA_CENTRALIZADORA: string = 'm_EmpresaCentralizadora';


    static readonly ALTERAR_CARTA_FIANCA: string = 'f_AlteraCartaFianca';

    static readonly ACESSAR_AGENDA_CE: string = 'm_AgendaAudienciasCE';
    static readonly ALTERAR_AUDIENCIA_CE: string = 'f_EditaAudienciasCE';

    // PERMISSÕES PROVISÃO TRABALHISTA POR MÉDIA
    static readonly ACESSAR_PROVISAO_TRABALHISTA: string = 'mRelatoriosFechTrabProvisao';
    static readonly FILTRAR_PROVISAO_TRABALHISTA_OUTLIER_SEM_EXCLUSAO: string = 'f_RelProvTrabOutliersSemExcl';
    static readonly FILTRAR_PROVISAO_TRABALHISTA_OUTLIER_POR_PERCENTUAL: string = 'f_RelProvTrabOutliersPercent';
    static readonly FILTRAR_PROVISAO_TRABALHISTA_OUTLIER_POR_DESVIO_PADRAO: string = 'f_RelProvTrabOutliersDesvPad';

    // PERMISSÕES ESTOQUE DE PEDIDOS
    static readonly ACESSAR_ESTOQUE_DE_PEDIDOS: string = 'mRelatoriosEstoquePedidoTrab';
    static readonly FILTRAR_ESTOQUE_DE_PEDIDOS_OUTLIER_SEM_EXCLUSAO: string = 'f_RelEstoquePedOutliersSemExcl';
    static readonly FILTRAR_ESTOQUE_DE_PEDIDOS_OUTLIER_POR_PERCENTUAL: string = 'f_RelEstoquePedOutliersPercent';
    static readonly FILTRAR_ESTOQUE_DE_PEDIDOS_OUTLIER_POR_DESVIO_PADRAO: string = 'f_RelEstoquePedOutliersDesvPad';

    // PERMISSÕES RELATÓRIOS > GRUPO EMPRESA CONTABIL SAP
    static readonly ACESSAR_GRUPO_EMPRESA_CONTABIL_SAP: string = 'm_GrupoEmpContSAP';

    static readonly ACESSAR_BASE_CALCULO: string = 'm_BaseCalculo';
    static readonly ACESSAR_TIPO_AUDIENCIA: string = 'm_TipoDeAudiencia';
    static readonly ACESSAR_TIPO_PARTICIPACAO: string = 'm_TipoParticipacao';
    static readonly ACESSAR_JUROS: string = 'm_Juros';
    static readonly ACESSAR_TIPO_DOCUMENTO: string = 'm_TipoDocumento';
    static readonly ACESSAR_TIPO_PRAZO: string = 'm_TipoPrazo';

    static readonly ACESSAR_ESFERA = "m_EsferasWeb";

    static readonly ACESSAR_ATM_PEX: string = 'm_RelatorioAtmPex';

    static readonly ACESSAR_COTACAO: string = 'm_Cotacao';
    static readonly ACESSAR_CADASTRO_INDICES: string = 'm_CadastroIndices';

    static readonly ACESSAR_ATM_JEC: string = 'm_RelatorioAtmJEC';
    static readonly ACESSAR_ATM_CC: string = 'm_RelatorioAtmCC';

    static readonly ACESSAR_TAB_PARAMETRIZACAO_CLOSING_CC: string = 'f_ParamClosingConsumidor';
    static readonly ACESSAR_TAB_PARAMETRIZACAO_CLOSING_CE: string = 'f_ParamClosingEstrategico';
    static readonly ACESSAR_TAB_PARAMETRIZACAO_CLOSING_JEC: string = 'f_ParamClosingJuizado';
    static readonly ACESSAR_TAB_PARAMETRIZACAO_CLOSING_TRAB_ADM: string = 'f_ParamClosingTrabalhistaAdm';
    static readonly ACESSAR_TAB_PARAMETRIZACAO_CLOSING_TRAB_JUD: string = 'f_ParamClosingTrabalhistaJud';

    static readonly ACESSAR_TIPO_PENDENCIA: string = 'm_TipoPendencia';
    static readonly ACESSAR_TIPO_PROCEDIMENTO: string = 'm_TipoProcedimento';
    static readonly ACESSAR_TIPO_ORIENTACAO_JURIDICA: string = 'm_TipoOrientacaoJuridica';
    static readonly ACESSAR_TIPO_VARA: string = 'm_TipoVara';

    // PERMISSÔES DE COMPLEMENTO AREA ENVOLVIDA
    static readonly ACESSAR_COMPLEMENTO_AREA_ENVOLVIDA = "m_ComplementoAreaEnvolvida";

    static readonly ACESSAR_COMPLEMENTO_AREA_ENVOLVIDA_TRIBUTARIO : string = "f_ComplAreaEnvolvidaTrib";
    static readonly ACESSAR_COMPLEMENTO_AREA_ENVOLVIDA_CIVEL_ESTRATATEGICO : string = "f_ComplAreaEnvolvidaCivEstrat";
    static readonly ACESSAR_COMPLEMENTO_AREA_ENVOLVIDA_ADMINISTRATIVO : string = "f_ComplAreaEnvolvidaAdm";

    static readonly ACESSAR_INDICES_VIGENCIA_CIVEL_CONSUMIDOR : string = "f_VigenciaIndiceCivelConsumidorWeb";
    static readonly ACESSAR_INDICES_VIGENCIA_CIVEL_ESTRATEGICO : string = "f_VigenciaIndiceCivelEstrategicoWeb";

    static readonly ACESSAR_COMPLEMENTO_AREA_ENVOLVIDA_CRIMINAL : string = "f_ComplAreaEnvolvidaCriminal";
    static readonly ACESSAR_COMPLEMENTO_AREA_ENVOLVIDA_TRABALHISTA : string = "f_ComplAreaEnvolvidaTrab";

    static readonly ACESSAR_ESTADO: string = 'm_EstadoWeb';


    static readonly ACESSAR_OBJETO : string = 'm_Objeto';
    static readonly ACESSAR_OBJETO_TRABALHISTA_ADMINISTRATIVO: string = 'f_ObjetoTrabAdm';
    static readonly ACESSAR_OBJETO_TRIBUTARIO: string = 'f_ObjetoTrib';

    static readonly ACESSAR_PERCENTUAL_ATM: string = 'm_PercentualAtm';
    static readonly ACESSAR_MOVIMENTACAO_PEX: string = 'm_RelatorioMovimentacaoPex';
    static readonly ACESSAR_MOVIMENTACAO_CC: string = 'm_RelatorioMovimentacaoCC';
    static readonly ACESSAR_MOVIMENTACAO_TRABALHISTA: string = 'm_RelatorioMovimentacaoTrabalhistaJud';
    static readonly ACESSAR_OPERACOES_RETROATIVAS: string = 'm_UsuarioOpRetroWeb';

    static readonly ACESSAR_PREPOSTO: string = 'm_CadastroPrepostoWeb';

    static readonly ACESSAR_EVENTO: string = 'm_EventoWeb';
    static readonly ACESSAR_EVENTO_CIVEL_CONSUMIDOR: string = 'f_EventCivConsumidorWeb';
    static readonly ACESSAR_EVENTO_CIVEL_ESTRATEGICO: string = 'f_EventCivEstrategicoWeb';
    static readonly ACESSAR_EVENTO_ADMINSTRATIVO: string = 'f_EventoAdminWeb';
    static readonly ACESSAR_EVENTO_TRABALHISTA_ADMINISTRATIVO: string = 'f_EventTrabAdminWeb';
    static readonly ACESSAR_EVENTO_TRABALHISTA: string = 'f_EventoTrabWeb';
    static readonly ACESSAR_EVENTO_TRIBUTARIO_ADMINISTRATIVO: string = 'f_EventTributaAdm';

    static readonly ACESSAR_EVENTO_TRIBUTARIO_JUDICIAL: string = 'f_EventTributaJud';


    static readonly ACESSAR_FATO_GERADOR: string = 'm_FatoGeradorWeb';

    static readonly ACESSAR_ORIENTACAO_JURIDICA_TRABALHISTA: string = 'm_OrientacaoTrabWeb';
    static readonly ACESSAR_MOVIMENTACAO_JEC: string = 'm_RelatorioMovimentacaoJuizado';

    static readonly ACESSAR_PERMISSAO: string = 'mPermissao';
    static readonly ACESSAR_PERFIL: string = 'mPerfil';

    static readonly ACESSAR_RELATORIO_MOVIMENTACAO_PEX: string = 'm_RelatorioMovimentacaoPex';
    static readonly ACESSAR_PARAMETRO_CLOSING: string = 'm_ParametrizacaoClosing';
    static readonly ACESSAR_AGENDA_AUDIENCIA_TRABALHISTA: string = 'm_AgendaAudienciaTrabalhista';
    static readonly ACESSAR_ALTERAR_CONSULTAR_PROCESSO_TRABALHISTA: string = 'mTrabalhista';

    static readonly ACESSAR_MIGRAR_PEDIDO_SAP: string = 'm_MigrarPedidoSAP';
    static readonly ACESSAR_RELATORIO_CONTINGENCIA_CC_MEDIA: string = 'm_RelContCCMedia';
    static readonly ACESSAR_RELATORIO_CONTINGENCIA_PEX_MEDIA: string = 'm_RelContPEXMedia';
    static readonly ACESSAR_ALTERACAO_PROC_BLOCO_WEB: string = 'm_AlteracaoProcBlocoWeb';
    static readonly ACESSAR_EXTRACAO_BASE_PRE_POS: string = 'm_ExtracaoBasePrePos';
    static readonly ACESSAR_APURAR_VALOR_CORTE_OUTLIERS_JEC: string = 'm_ApurarValorCorteOutliersJEC';
    static readonly ACESSAR_CARGA_COMPROVANTE_PAGAMENTO_RJ: string = 'm_CargaCompromissosRJ';
    static readonly ACESSAR_CARGA_DOCUMENTOS_PAGAMENTO_RJ: string = 'm_CargaDocumentosPagRJ';
    static readonly ACESSAR_ACOMPANHAR_COMPROMISSOS_RJ: string = 'm_AcompanharCompromissosRJ';

    static readonly ACESSAR_MOTIVO_PROVAVEL_ZERO: string = 'm_CadMotivoProvavelZero';

    static readonly ACESSAR_SOLICITACOES_PEX: string = 'm_RelatorioSolicitacoesPEX';
    static readonly ACESSAR_ESCRITORIO: string = 'mCadastroEscritorio';
    static readonly ACESSAR_COMPOSICAO_JEC: string = 'mComposicao';
    static readonly ACESSAR_COMPOSICAO_CC: string = 'mAgendaPautaCivel_Composicao';
    static readonly ACESSAR_COMPOSICAO_PROCON: string = 'mComposicaoProcon';
    static readonly ACESSAR_SOLICITANTE: string = 'm_ManutencaoSolicitantes';

    static readonly ACESSAR_RELATORIO_LOG: string = 'm_RelatorioLogProcessos';
     
    /******************************************
    * Permissões dos processos criminais:
    *******************************************/
    //Menu:
    static readonly ACESSAR_CRIMINAL_ADM: string = 'mConsultaCriminalAdm';
    static readonly ACESSAR_CRIMINAL_JUDICIAL: string = 'mConsultaCriminalJud';

    //Inclusão:
    static readonly INCLUIR_CRIMINAL_ADM: string = 'CrimAdm_IncluirProcedimento';
    static readonly INCLUIR_CRIMINAL_JUD: string = 'CrimJud_IncluirProcesso';

    //Alteração:
    static readonly ALTERAR_CRIMINAL_ADM: string = 'CrimAdm_AlterarDadosGerais';
    static readonly ALTERAR_CRIMINAL_JUD: string = 'CrimJud_AlterarDadosGerais';

    // AGENDAR / SOLICITAR FECHAMENTO
    static readonly ACESSAR_AGENDAR_SOLICITAR_FECHAMENTO_CONTINGENCIA: string = 'mSolFechamentoContingencia';
    static readonly F_SOLICITAR_CONSULTAR_FECH_CONTING_CC: string = 'f_SolicitarConsultarFechContingCC';
    static readonly F_SOLICITAR_CONSULTAR_FECH_CONTING_CE: string = 'f_SolicitarConsultarFechContingCE';
    static readonly F_SOLICITAR_CONSULTAR_FECH_CONTING_JEC: string = 'f_SolicitarConsultarFechContingJEC';
    static readonly F_SOLICITAR_CONSULTAR_FECH_CONTING_TRAB: string = 'f_SolicitarConsultarFechContingTRAB';
    static readonly F_SOLICITAR_CONSULTAR_FECH_CONTING_PEX: string = 'f_SolicitarConsultarFechContingPEX';
    static readonly ACESSAR_CONSULTAR_LOGS_FECHAMENTO_CONTINGENCIA: string = 'mConsultarLogsFechContingencia';

    /******************************************
     * Permissões Manutencao app preposto
    *******************************************/

    static readonly ACESSAR_DEPARASTATUS: string = 'm_DeXParaStatusAudiencia';
    static readonly ACESSAR_ACAO: string = 'm_TipoAcao';
    static readonly F_TIPO_ACAO_JEC: string = 'f_TipoAcaoJuizadoEspecial';
    static readonly F_TIPO_ACAO_PROCON: string = 'f_TipoAcaoProcon';
    static readonly F_TIPO_ACAO_CIVEL_CONSUMIDOR: string = 'f_TipoAcaoCivelConsumidor';

    static readonly F_TIPO_ACAO_CIVEL_ESTRATEGICO: string = 'f_TipoAcaoCivelEstrategico';
    static readonly F_TIPO_ACAO_CRIMINAL_JUDICIAL: string = 'f_TipoAcaoCriminalJudicial';
    static readonly F_TIPO_ACAO_PEX: string = 'f_TipoAcaoPex';
    static readonly F_TIPO_ACAO_TRABALHISTA: string = 'f_TipoAcaoTrabalhista';
    static readonly F_TIPO_ACAO_TRIBUTARIO_JUDICIAL: string = 'f_TipoAcaoTributarioJudicial';
    
    static readonly ACESSAR_EMPRESA_CONTRATADA: string = 'm_ManutencaoEmpContratada';
    static readonly ACESSAR_RESULTADO_NEGOCIACAO: string = 'm_ManutencaoResulNegociacao';
    static readonly ACESSAR_STATUS_CONTATO: string = 'mStatusContato';
    


   // ESOCIAL
   static readonly ACESSAR_ACOMPANHAMENTO_ENVIO_ESOCIAL: string = 'm_AcompanhamentoEnvio_eSocial';
   static readonly ACESSAR_CADASTRO_ESOCIAL: string = 'm_Cadastro_eSocial';
   static readonly LIMPAR_FORMULARIO_ESOCIAL: string = 'f_LimparFormulario_eSocial';

   static readonly ALTERAR_STATUS_RECLAMANTE_ESOCIAL: string = 'f_AlterarStatusReclamante_eSocial';
   static readonly CANCELA_STATUS_ELEGIVEL_RECLAMANTE_ESOCIAL: string = 'f_CancStatusElegi_Recla_eSocial';
   static readonly CADASTRAR_FORMULARIO_2500_ESOCIAL: string = 'f_CadastrarForm2500_eSocial';
   static readonly CADASTRAR_FORMULARIO_2501_ESOCIAL: string = 'f_CadastrarForm2501_eSocial';
   static readonly INCLUIR_FORMULARIO_2501_ESOCIAL: string = 'f_IncluirForm2501_eSocial';
   static readonly REMOVER_FORMULARIO_2501_ESOCIAL: string = 'f_RemoverForm2501_eSocial';
   static readonly RETIFICAR_FORMULARIO_2500_ESOCIAL: string = 'f_RetificarForm2500_eSocial';
   static readonly RETIFICAR_FORMULARIO_2501_ESOCIAL: string = 'f_RetificarForm2501_eSocial';
   static readonly EXCLUIR_FORMULARIO_2500_ESOCIAL: string = 'f_ExcluirForm2500_eSocial';
   static readonly EXCLUIR_FORMULARIO_2501_ESOCIAL: string = 'f_ExcluirForm2501_eSocial';

   static readonly ENVIAR_2500_PARA_ESOCIAL: string = 'f_Form2500_EnviarparaeSocial';
   static readonly ESOCIAL_BLOCO_ABCDEFHI_2500: string = 'f_Form2500_eSocial_blocoABCDEFHI';
   static readonly ESOCIAL_BLOCO_GK_2500: string = 'f_Form2500_eSocial_blocoGK';
   static readonly ESOCIAL_BLOCO_J_DADOS_ESTABELECIMENTO_2500: string = 'f_Form2500_eSocial_blocoJ_DadosEstabe';
   static readonly ESOCIAL_BLOCO_J_VALORES_2500: string = 'f_Form2500_eSocial_blocoJ_Valores';
   static readonly ESOCIAL_BLOCO_E_PENSAO_ALIMENTICIA_2500: string = 'f_Form2500_eSocial_PensaoAlim';


   static readonly ENVIAR_2501_PARA_ESOCIAL: string = 'f_Form2501_EnviarparaeSocial';
   static readonly ESOCIAL_BLOCO_AB_2501: string = 'f_Form2501_eSocial_blocoAB';
   static readonly ESOCIAL_BLOCO_CDE_2501: string = 'f_Form2501_eSocial_blocoCDE';

   static readonly ESOCIAL_BLOCO_F_2501: string = 'f_Form2501_eSocial_blocoF';
   static readonly ESOCIAL_BLOCO_G_2501: string = 'f_Form2501_eSocial_blocoG';

   static readonly ESOCIAL_RETORNA_STATUS_RASCUNHO: string = 'f_RetornarFormularioParaRascunho_eSocial';

   static readonly ESOCIAL_ATUALIZAR_RECIBO: string = 'f_Informar_Num_Recibo_eSocial';

   static readonly ACESSAR_DASHBOARD_ESOCIAL: string = 'm_Dashboard_eSocial';

    // PERMISSÕES WORKFLOW
    static readonly ACESSAR_PARAM_DISTRIBU_ESCRITORIO: string = 'm_ParamDistribuEscritorio';
    static readonly ACESSAR_AGEND_VALOR_ESPERADO_PROCESSO: string = 'm_RelatorioCalculoVEP';
    static readonly ACESSAR_MANUTENCAO_CONTRATO_ESCRITORIO: string = 'm_ManutencaoContratos';
    static readonly ACESSAR_RELATORIO_PAGAMENTO_ESCRITORIO: string = 'm_RelatorioPagEscritorios';
    static readonly CONSULTAR_PARAM_DISTRIBU_ESCRIT: string = 'f_ConsultarParamDistribuEscrit';

   static readonly FINALIZAR_ESCRITORIO_FORM2500: string = 'f_Form2500_FinalizarEscritorio';
   static readonly FINALIZAR_CONTADOR_FORM2500: string = 'f_Form2500_FinalizarContador';
   static readonly FINALIZAR_ESCRITORIO_FORM2501: string = 'f_Form2501_FinalizarEscritorio';
   static readonly FINALIZAR_CONTADOR_FORM2501: string = 'f_Form2501_FinalizarContador';

   static readonly ESOCIAL_PERIODO_APURACAO_BLOCO_A_2501: string = 'f_Form2501_eSocial_BlocoA_PerApur';

   static readonly CRIAR_NOVO_FORMULARIO_2500_ESOCIAL: string = 'f_CriarNovoForm2500_eSocial';

   static readonly ACESSAR_DEPARASTATUSNEGOCIACAO: string = 'm_DeXParaStatusNegociacao';
   
   static readonly ACESSAR_RELATORIOATMCC: string = 'm_RelatorioAtmCC';

}
