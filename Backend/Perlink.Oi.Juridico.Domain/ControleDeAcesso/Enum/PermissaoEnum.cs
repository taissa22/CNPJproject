namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.Enum {
    public enum PermissaoEnum {
        m_ConsultaLotesSAP,                         // - Menu > SAP > Lotes > Consulta, Controle e Acompanhamento de Lotes.
        f_ConsultaLotesSapCC,                       // - Funcionalidade - Permite consultar lotes Cível Consumidor.
        f_ConsultaLotesSapCE,                       // - Funcionalidade - Permite consultar lotes Cível Estratégicos.
        f_ConsultaLotesSapJEC,                      // - Funcionalidade - Permite consultar lotes Juizado Especial Cível.
        f_ConsultaLotesSapPEX,                      // - Funcionalidade - Permite consultar lotes PEX.
        f_ConsultaLotesSapTrabalhista,              // - Funcionalidade - Permite consultar lotes Trabalhista.
        f_AlterarDatEscritorioCons,                 //Funcionalidade SAP Cível Consumidor - Permite alterar a data de envio para o escritório dos lançamentos cível consumidor.
        f_CancelarLotesCivelCons,                   //Funcionalidade SAP Cível Consumidor - Permite cancelar lotes cível consumidor
        f_ExportarLotesCivelCons,                   //Funcionalidade SAP Cível Consumidor - Permite exportar os registros de lotes, lançamentos e borderôs.
        f_RegerarLotesBBCivelCons,                  //Funcionalidade SAP Cível Consumidor - Permite regerar o número do lote BB
        f_AlterarDatEscritorioEstrat,               //Funcionalidade SAP Cível Estratégico - Permite alterar o lote cível estratégico.
        f_CancelarLotesCivelEstrat,                 //Funcionalidade SAP Cível Estratégico - Permite cancelar o lote do cível estratégico 
        f_ExportarLotesEstrat,                      //Funcionalidade SAP Cível Estratégico - Permite exportar os registros de lotes, lançamentos e borderôs
        f_AlterarDatEscritorioJuizado,              //Funcionalidade SAP Juizado - Permite alterar lotes juizado
        f_CancelarLotesJuizado,                     //Funcionalidade SAP Juizado - Permite cancelar lotes Juizado 
        f_ExportarLotesJuizado,                     //Funcionalidade SAP Juizado - Permite exportar os registros de lotes, lançamentos, borderô e histórico do status de pagamento.
        f_RegerarLotesBBJuizado,                    //Funcionalidade SAP Juizado - Permite que o número do lote BB seja regerado.
        f_AlterarDatEscritorioPex,                  //Funcionalidade SAP Pex - Permite alterar lotes Pex
        f_CancelarLotesPex,                         //Funcionalidade SAP Pex - Permite cancelar lotes Pex
        f_ExportarLotesPex,                         //Funcionalidade SAP Pex - Permite exportar os registros de lotes, lançamentos, borderô e histórico do status de pagamento.
        f_AlterarDatEscritorioTrab,                 //Funcionalidade SAP Trabalhista - Permite alterar lotes trabalhistas
        f_CancelarLotesTrabalhista,                 //Funcionalidade SAP Trabalhista - Permite cancelar lotes Trabalhistas
        f_ExportarLotesTrabalhista,                 //Funcionalidade SAP Trabalhista - Permite exportar os registros de lotes, lançamentos e borderô.
        m_CriarLotesSAP,                            //Menu –  SAP > Lotes > Criação de Lotes
        f_CriarLotesCivelConsumidor,                //Funcionalidade SAP Cível Consumidor - Permite criar lotes cível consumidor
        f_CriarLotesCivelEstrategico,               //Funcionalidade SAP Cível Estratégico - Permite criar lotes cível estratégico
        f_CriarLotesJuizado,                        //Funcionalidade SAP Juizado - Permite criar lotes Juizado
        f_CriarLotesPex,                            //Funcionalidade SAP Pex - Permite criar lotes Pex
        f_CriarLotesTrabalhista,                    //Funcionalidade SAP Trabalhista - Permite criar lotes trabalhistas
        m_EstornoDeLancamentos,                     //Menu > SAP > Lotes > Estorno de Lançamentos Pagos
        f_EstornaLancamentosCC,                     //Funcionalidade Cível Consumidor – Permite estornar lançamentos pagos do cível consumidor
        f_EstornaLancamentosCE,                     //Funcionalidade Cível Estratégico – Permite estornar lançamentos pagos do cível estratégico
        f_EstornaLancamentosTrab,                   //Funcionalidade Trabalhista – Permite estornar lançamentos pagos do trabalhista
        f_EstornaLancamentosPex,                    //Funcionalidade Pex – Permite estornar lançamentos pagos do Pex
        m_CategoriasDePagamento,                    //Menu > SAP > Manutenção > Categorias de Pagamento
        f_CategoriasDePagamentoCC,                  //Funcionalidade Cível Consumidor – Permite acessar a funcionalidade de manutenção das categorias de pagamento cível consumidor
        f_CategoriasDePagamentoCE,                  //Funcionalidade Cível Estratégico – Permite acessar a funcionalidade de manutenção das categorias de pagamento cível estratégico
        f_CategoriasDePagamentoJuiz,                //Funcionalidade Juizado Especial Cível – Permite acessar a funcionalidade de manutenção das categorias de pagamento Juizado
        f_CategoriasDePagamentoTrab,                //Funcionalidade Trabalhista – Permite acessar a funcionalidade de manutenção das categorias de pagamento trabalhista
        f_CategoriasDePagamentoTribAdm,             //Funcionalidade Tributário Administrativo – Permite acessar a funcionalidade de manutenção das categorias de pagamento tributário administrativo
        f_CategoriasDePagamentoTribJud,             //Funcionalidade Tributário Judicial – Permite acessar a funcionalidade de manutenção das categorias de pagamento tributário judicial
        f_CategoriasDePagamentoAdm,                 //Funcionalidade Administrativo – Permite acessar a funcionalidade de manutenção das categorias de pagamento administrativo.
        f_CategoriasDePagamentoProc,                //Funcionalidade Procon – Permite acessar a funcionalidade de manutenção das  categorias de pagamento do procon
        f_CategoriasDePagamentoPex,                 //Funcionalidade Pex – Permite acessar a funcionalidade de manutenção das categorias de pagamento do pex
        m_CentrosDeCustoDoSap,                      //Menu > SAP > Manutenção > Centros de Custo do SAP
        m_EmpresasDoSap,                            //Menu > SAP > Manutenção > Empresas do SAP
        m_FormasDePagamento,                        //Menu > SAP > Manutenção > Formas de Pagamento
        m_Fornecedores,                             //Menu > SAP > Manutenção > Fornecedores
        m_ComarcasBB,                               //Menu > SAP > Interface BB > Comarcas BB
        m_TribunaisBB,                              //Menu > SAP > Interface BB > Tribunais BB
        m_ModalidadeProdutoBB,                      //Menu > SAP > Interface BB > Modalidade do Produto BB
        m_NaturezaAcoesBB,                          //Menu > SAP > Interface BB > Natureza das Ações BB
        m_OrgaosBB,                                 //Menu > SAP > Interface BB > Órgãos BB
        m_StatusParcelaBB,                          //Menu > SAP > Interface BB >  Status Parcela BB
        m_GrupoLoteJuizado,                         //​​Menu > SAP > Manutenção > Grupo de Lote de Juizado
        m_FornecedoresContingenciaSAP,              //Menu > SAP > Manutenção > Fornecedores da Contingência SAP
        m_ImportaConsultaArquivoRetorno,            //Menu > SAP > Interface BB > Importação e Consulta do Arquivo de Retorno
        f_JurosCivelConsumidor,                     //Menu > Manutenção > Juros > Vigências Cíveis
        f_JurosCivelEstrategico,                    //Menu > Manutenção > Juros > Vigências Cíveis
        f_TipoDeAudienciaCivCon,                    //Funcionalidade Cível Consumidor - Permite acessar a funcionalidade de manutenção de Tipo de Audiência.
        f_TipoDeAudienciaCivEst,                    //Funcionalidade Cível Estratégico - Permite acessar a funcionalidade de manutenção de Tipo de Audiência.
        f_TipoDeAudienciaTrab,                      //Funcionalidade Trabalhista - Permite acessar a funcionalidade de manutenção de Tipo de Audiência. 
        f_TipoDeAudienciaTrabAdm,                   //Funcionalidade Trabalhista Administrativo- Permite acessar a funcionalidade de manutenção de Tipo de Audiência.
        f_TipoDeAudienciaTribAdm,                   //Funcionalidade Tributário Administrativo- Permite acessar a funcionalidade de manutenção de Tipo de Audiência.
        f_TipoDeAudienciaTribJud,                   //Funcionalidade Tributário Judicial- Permite acessar a funcionalidade de manutenção de Tipo de Audiência.
        f_TipoDeAudienciaAdm,                       //Funcionalidade Administrativo- Permite acessar a funcionalidade de manutenção de Tipo de Audiência.
        f_TipoDeAudienciaCivAdm,                    //Funcionalidade Cível Administrativo- Permite acessar a funcionalidade de manutenção de Tipo de Audiência.
        f_TipoDeAudienciaCriJud,                    //Funcionalidade Criminal Judicial - Permite acessar a funcionalidade de manutenção de Tipo de Audiência.
        f_TipoDeAudienciaCriAdm,                    //Funcionalidade Criminal Administrativo- Permite acessar a funcionalidade de manutenção de Tipo de Audiência
        f_TipoDeAudienciaJec,                       //Funcionalidade Juizado Especial - Permite acessar a funcionalidade de manutenção de Tipo de Audiência.
        f_TipoDeAudienciaProc,                      //Funcionalidade Procon- Permite acessar a funcionalidade de manutenção de Tipo de Audiência.
        f_TipoDeAudienciaPex,                       //Funcionalidade Pex - Permite acessar a funcionalidade de manutenção de Tipo de Audiência.
        m_AlteracaoProcBlocoWeb,                     //Menu > Relatórios > Alteração de Processos em Bloco Web
        f_AlteraProcBlocoWebAdm,                     //Funcionalidade Administrativo - Permite alterar processos em bloco, através da funcionalidade Relatórios > Alterar processos em bloco web.
        f_AlteraProcBlocoWebCC,                      //Funcionalidade Cível Consumidor - Permite alterar processos em bloco, através da funcionalidade Relatórios > Alterar processos em bloco web.
        f_AlteraProcBlocoWebCE,                      //Funcionalidade Cível Estratégico - Permite alterar processos em bloco, através da funcionalidade Relatórios > Alterar processos em bloco web.
        f_AlteraProcBlocoWebJEC,                    //Funcionalidade Juizado Especial Cível - Permite alterar processos em bloco, através da funcionalidade Relatórios > Alterar processos em bloco web.
        f_AlteraProcBlocoWebPEX,                    //Funcionalidade PEX - Permite alterar processos em bloco, através da funcionalidade Relatórios > Alterar processos em bloco web.
        f_AlteraProcBlocoWebProcon,                 //Funcionalidade Procon - Permite alterar processos em bloco, através da funcionalidade Relatórios > Alterar processos em bloco web.
        f_AlteraProcBlocoWebTrab,                   //Funcionalidade Trabalhista - Permite alterar processos em bloco, através da funcionalidade Relatórios > Alterar processos em bloco web.
        f_AlteraProcBlocoWebTribJud,                //Funcionalidade Tributário Judicial - Permite alterar processos em bloco, através da funcionalidade Relatórios > Alterar processos em bloco web.
        f_VisualizaTodasAlteracoesProc,             //Funcionalidade - Permite visualizar todas os agendamentos de alteração em bloco de processos realizados na funcionalidade Relatórios > Alteração de Processos em Bloco Web    
        m_ExtracaoBasePrePos,                        //Menu > Relatórios > D-1 > Extração base Pré/Pós RJ;
        m_GruposEstadosPEX,                        //Menu > Relatórios > D-1 > Extração base Pré/Pós RJ;                                             //
    }
}