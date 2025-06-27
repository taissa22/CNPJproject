import { Injectable } from '@angular/core';
import { MenuSap } from '@shared/interfaces/menu-sap';
import { UserService } from '../core';
import { TipoProcessoEnum } from './../shared/enums/tipo-processoEnum.enum';
import { FilterService } from './consultaLote/services/filter.service';
import { CriacaoService } from './criacaoLote/criacao.service';
import { role } from './sap.constants';




@Injectable({
  providedIn: 'root'
})
export class PermissoesSapService {

  /**
   * Tipo processo da tela de SAP/Lotes/Consulta
   */
  tipoProcessoSelect;
  /**
   * Tipo processo da tela de SAP/Lotes/Criacao
   */
  tipoProcessoCriacaoSelect;

  permissoes

  constructor(private userService: UserService,
    private filterService: FilterService,
    private criacaoService: CriacaoService,
  ) {
    this.filterService.tipoProcessoTracker.subscribe(item =>
      this.tipoProcessoSelect = item);

    this.criacaoService.tipoProcesso.subscribe(
      item => this.tipoProcessoCriacaoSelect = item);

    this.permissoes = this.userService.getCurrentUser().permissoes

  }

  /**
   * Filtra no usuário logado a permissão de acordo com o tipo de processo.
   * @param permissao Nome da permissão a verificar.
   * @param tipoProcesso Nome da variavel em string, sem o this a ser verficada.
   * Para pagina de criacao -> tipoProcessoCriacaoSelect, para consulta
   * tipoProcessoSelect
   * @param tipoProcessoEnum Enum para o tipo de processo a ser verificado.
   */
  verificarPermissao(permissao: string, tipoProcesso: string, tipoProcessoEnum) {
    const componente = this;
    return this.userService.getCurrentUser().permissoes.find(item => item === permissao) !== undefined
      && componente[tipoProcesso] === tipoProcessoEnum;
  }


  /**
    * Verifica se o usuário possui permissao.
     */
  hasRole(permissaoMenu) {
    return permissaoMenu.some(r => this.permissoes.includes(r));


  }

  // ---------------------------- Menu ------------------------------

  /**
   * Permissões do Menu do SAP
    */
  permissoesMenu: MenuSap = {
    menuConsultaLote: this.m_ConsultaLotesSAP,
    menuCriacaoLote: this.m_CriarLotesSAP,
    menuManutencao: this.hasRoleManutencao,
    menuInterfaceBB: this.hasRoleInterfaceBB,
    menuLote: this.hasRoleLote
  }
  /**
    * m_ConsultaLotesSAP : Menu –  SAP > Lotes > Consulta e Acompanhamento de Lotes
     */
  private get m_ConsultaLotesSAP(): boolean {
    return this.userService.getCurrentUser().permissoes.find(element => element ===
      role.menuConsultaControleAcompanhamentoLote) !== undefined;
  }
  /**
  * m_CriarLotesSAP : Menu –  SAP > Lotes > Criação de Lotes 
   */
  private get m_CriarLotesSAP(): boolean {
    return this.userService.getCurrentUser().permissoes.find(element => element === role.menuCriacaoLote) !== undefined;
  }

  /**
 * Verifica se é viavel mostrar o menu manutenção
  */
  private get hasRoleManutencao(): boolean {
    return this.m_CategoriasDePagamento || this.m_CentrosDeCustoDoSap
      || this.m_EmpresasDoSap || this.m_FormasDePagamento || this.m_Fornecedores
      || this.m_GrupoLoteJuizado || this.m_FornecedoresContingenciaSAP
      ;
  }

  /**
 * Verifica se é viavel mostrar o menu interfaceBB
  */
  private get hasRoleInterfaceBB(): boolean {
    return this.m_ModalidadeProdutoBB || this.m_NaturezaAcoesBB ||
      this.m_StatusParcelaBB || this.m_OrgaosBB || this.m_TribunaisBB
      || this.m_ComarcasBB || this.m_ImportaConsultaArquivoRetorno;
  }

  private get hasRoleLote(): boolean {
    return this.m_ConsultaLotesSAP || this.m_CriarLotesSAP;
  }
  /**
    * m_CategoriasDePagamento: Menu - SAP > Manutenção > Categorias de Pagamento
     */
  private get m_CategoriasDePagamento(): boolean {
    return this.userService.getCurrentUser().permissoes.find(element => element === role.menuCategoriaPagamento) !== undefined;
  }
  /**
  * m_CentrosDeCustoDoSap: Menu - SAP > Manutenção > Centros de Custo do SAP
   */
  private get m_CentrosDeCustoDoSap(): boolean {
    return this.userService.getCurrentUser().permissoes.find(element => element === role.menuManutencaoCentroCusto) !== undefined;
  }
  /**
  * m_EmpresasDoSap: Menu - SAP > Manutenção > Empresas do SAP
   */
  private get m_EmpresasDoSap(): boolean {
    return this.userService.getCurrentUser().permissoes.find(element => element === role.menuManutencaoEmpresaSap) !== undefined;
  }

  /**
  * m_FornecedoresContingenciaSAP: Menu - SAP > Manutenção > Fornecedores da Contingência SAP
   */
  private get m_FornecedoresContingenciaSAP(): boolean {
    return this.userService.getCurrentUser().permissoes.find(element => element === role.menuManutencaoFornecedoresContingencia) !== undefined;
  }

  /**
  * m_GrupoLoteJuizado: Menu > SAP > Manutenção > Grupo de Lote de Juizado
   */
  private get m_GrupoLoteJuizado(): boolean {
    return this.userService.getCurrentUser().permissoes.find(element => element === role.menuManutencaoGrupoLoteJuizado) !== undefined;
  }

  /**
 * m_FormasDePagamento: Menu - SAP > Manutenção > Formas de Pagamento
  */
  private get m_FormasDePagamento(): boolean {
    return this.userService.getCurrentUser().permissoes.find(element => element === role.menuManutencaoFormaPagamento) !== undefined;
  }
  /**
  * m_Fornecedores: Menu > SAP > Manutenção > Fornecedores
   */
  private get m_Fornecedores(): boolean {
    return this.userService.getCurrentUser().permissoes.find(element => element === role.menuManutencaoFornecedores) !== undefined;
  }
  /**
  * m_ComarcasBB: Menu - SAP > Interface BB > Comarcas BB
   */
  private get m_ComarcasBB(): boolean {
    return this.userService.getCurrentUser().permissoes.find(element => element === role.menuInterfaceComarcarBB) !== undefined;
  }
  /**
  * m_TribunaisBB: Menu > SAP > Interface BB > Tribunais BB
   */
  private get m_TribunaisBB(): boolean {
    return this.userService.getCurrentUser().permissoes.find(element => element === role.menuInterfaceTribunaisBB) !== undefined;
  }

  /**
  * m_ImportaConsultaArquivoRetorno - Menu > SAP > Interface BB > Importação e Consulta do Arquivo de Retorno
   */
  private get m_ImportaConsultaArquivoRetorno(): boolean {
    return this.userService.getCurrentUser().permissoes.find(element => element === role.menuInterfaceImportacaoConsultaArquivoRetorno) !== undefined;
  }

  /**
 * m_ModalidadeProdutoBB: Menu - SAP > Interface BB
  */
  private get m_ModalidadeProdutoBB(): boolean {
    return this.userService.getCurrentUser().permissoes.find(element => element === role.menuInterfaceModalidadeProdutoBB) !== undefined;
  }
  /**
  * m_NaturezaAcoesBB: Menu - SAP > Interface BB > Natureza das Ações BB
   */
  private get m_NaturezaAcoesBB(): boolean {
    return this.userService.getCurrentUser().permissoes.find(element => element === role.menuInterfaceNaturezacaoAcoesBB) !== undefined;
  }
  /**
  * m_OrgaosBB: Menu - SAP > Interface BB > Órgãos BB
   */
  private get m_OrgaosBB(): boolean {
    return this.userService.getCurrentUser().permissoes.find(element => element === role.menuInterfaceOrgaosBB) !== undefined;
  }
  /**
  * m_StatusParcelaBB: Menu - SAP > Interface BB >  Status Parcela BB
   */
  private get m_StatusParcelaBB(): boolean {
    return this.userService.getCurrentUser().permissoes.find(element => element === role.menuInterfaceStatusParcelaBB) !== undefined;
  }
  /**
  * m_EstornoDeLançamentos: Menu - SAP > Lotes > Estorno de Lançamentos Pagos;
   */
  private get m_EstornoDeLançamentos(): boolean {
    return this.userService.getCurrentUser().permissoes.find(element => element === role.menuEstornoLancamento) !== undefined;
  }

  // ---------------------------------------------------------------------------------

  //  ------------------------------ Cível Consumidor --------------------------------


  //#region SAP -> Lote -> Criacao

  /**
   *   f_CriarLotesCivelConsumidor – Funcionalidade SAP Cível Consumidor - Permite criar lotes cível consumidor
   */
  get f_CriarLotesCivelConsumidor(): boolean {
    return this.verificarPermissao('f_CriarLotesCivelConsumidor', 'tipoProcessoCriacaoSelect', TipoProcessoEnum.civelConsumidor);
  }

  //#endregion

  //#region  SAP -> Lote -> Consulta

  /**
   * f_AlterarDatEscritorioCons - Funcionalidade SAP Cível Consumidor - Permite alterar a data de envio para o escritório dos lançamentos cível consumidor.
    */
  get f_AlterarDatEscritorioCons(): boolean {
    return this.verificarPermissao('f_AlterarDatEscritorioCons', 'tipoProcessoSelect', TipoProcessoEnum.civelConsumidor);
  }
  /**
  * f_CancelarLotesCivelCons – Funcionalidade SAP Cível Consumidor - Permite cancelar lotes cível consumidor
   */
  get f_CancelarLotesCivelCons(): boolean {
    return this.userService.getCurrentUser().permissoes.find(item => item === 'f_CancelarLotesCivelCons') !== undefined
      && this.tipoProcessoSelect === TipoProcessoEnum.civelConsumidor;
  }
  /**
 * f_ExportarLotesCivelCons – Funcionalidade SAP Cível Consumidor - Permite exportar os registros de lotes, lançamentos e borderôs.
  */
  get f_ExportarLotesCivelCons(): boolean {
    return this.verificarPermissao('f_ExportarLotesCivelCons', 'tipoProcessoSelect', TipoProcessoEnum.civelConsumidor);
  }

  /**
    * f_RegerarLotesBBCivelCons – Funcionalidade SAP Cível Consumidor - Permite regerar o número do lote BB
     */
  get f_RegerarLotesBBCivelCons(): boolean {
    return this.verificarPermissao('f_RegerarLotesBBCivelCons', 'tipoProcessoSelect', TipoProcessoEnum.civelConsumidor);
  }

  /**
   * Funcionalidade SAP Cível Consumidor - Permite que o arquivo do Banco do Brasil seja regerado.
   */
  get f_RegerarArquivoBBLotesCivelCons(): boolean {
    return this.verificarPermissao('f_RegerarArquivoBBLotesCivelCons', 'tipoProcessoSelect', TipoProcessoEnum.civelConsumidor);
  }


  /**
 * Funcionalidade SAP Pex - Permite que o arquivo do Banco do Brasil seja regerado.
 */
  get f_RegerarArquivoBBLotesPEX(): boolean {
    return this.verificarPermissao('f_RegerarArquivoBBLotesPEX', 'tipoProcessoSelect', TipoProcessoEnum.PEX);
  }

  //#endregion


  //  ------------------------------ Cível Estratégico --------------------------------

  //#region SAP -> Lote -> Criacao

  /**
  *f_CriarLotesCivelEstrategico – Funcionalidade SAP Cível Estratégico - Permite criar lotes cível estratégico
   */
  get f_CriarLotesCivelEstrategico(): boolean {
    return this.verificarPermissao('f_CriarLotesCivelEstrategico', 'tipoProcessoCriacaoSelect', TipoProcessoEnum.civelEstrategico);

  }
  //#endregion

  //#region  SAP -> Lote -> Consulta

  /**
 * f_AlterarDatEscritorioEstrat - Funcionalidade SAP Cível Estratégico - Permite alterar o lote cível estratégico.
  */
  get f_AlterarDatEscritorioEstrat(): boolean {
    return this.verificarPermissao('f_AlterarDatEscritorioEstrat', 'tipoProcessoSelect', TipoProcessoEnum.civelEstrategico);
  }

  /**
  * f_CancelarLotesCivelEstrat – Funcionalidade SAP Cível Estratégico - Permite cancelar o lote do cível estratégico
  */
  get f_CancelarLotesCivelEstrat(): boolean {
    return this.verificarPermissao('f_CancelarLotesCivelEstrat', 'tipoProcessoSelect', TipoProcessoEnum.civelEstrategico);

  }

  /**
  * f_ExportarLotesEstrat – Funcionalidade SAP Cível Estratégico - Permite exportar os registros de lotes, lançamentos e borderôs
  */
  get f_ExportarLotesEstrat(): boolean {
    return this.verificarPermissao('f_ExportarLotesEstrat', 'tipoProcessoSelect', TipoProcessoEnum.civelEstrategico);
  }

  //#endregion


  //  ------------------------------ JUIZADO --------------------------------

  //#region SAP -> Lote -> Criacao

  /**
 * f_CriarLotesJuizado – Funcionalidade SAP Juizado - Permite criar lotes Juizado
 */
  get f_CriarLotesJuizado(): boolean {
    return this.verificarPermissao('f_CriarLotesJuizado', 'tipoProcessoCriacaoSelect', TipoProcessoEnum.juizadoEspecial);

  }


  //#endregion

  //#region SAP -> Lote -> Consulta

  /**
    * f_AlterarDatEscritorioJuizado - Funcionalidade SAP Juizado - Permite alterar lotes juizado
    */
  get f_AlterarDatEscritorioJuizado(): boolean {
    return this.verificarPermissao('f_AlterarDatEscritorioJuizado', 'tipoProcessoSelect', TipoProcessoEnum.juizadoEspecial);

  }

  /**
   *
   *Funcionalidade SAP Juizado - Permite que o arquivo do Banco do Brasil seja regerado.
   * @readonly
   * @type {boolean}
   * @memberof PermissoesSapService
   */
  get f_RegerarArquivoBBLotesJuizado(): boolean {
    return this.verificarPermissao('f_RegerarArquivoBBLotesJuizado', 'tipoProcessoSelect', TipoProcessoEnum.juizadoEspecial);
  }

  /**
  * f_CancelarLotesJuizado – Funcionalidade SAP Juizado - Permite cancelar lotes Juizado
  */
  get f_CancelarLotesJuizado(): boolean {
    return this.verificarPermissao('f_CancelarLotesJuizado', 'tipoProcessoSelect', TipoProcessoEnum.juizadoEspecial);

  }

  /**
  * f_ExportarLotesJuizado – Funcionalidade SAP Juizado - Permite exportar os registros de lotes, lançamentos, borderô e histórico do status de pagamento.
  */
  get f_ExportarLotesJuizado(): boolean {
    return this.verificarPermissao('f_ExportarLotesJuizado', 'tipoProcessoSelect', TipoProcessoEnum.juizadoEspecial);
  }

  /**
  * f_RegerarLotesBBJuizado – Funcionalidade SAP Juizado - Permite que o número do lote BB seja regerado.
  */
  get f_RegerarLotesBBJuizado(): boolean {
    return this.verificarPermissao('f_RegerarLotesBBJuizado', 'tipoProcessoSelect', TipoProcessoEnum.juizadoEspecial);
  }
  //#endregion

  //  ------------------------------ PEX --------------------------------

  //#region SAP -> Lote -> Criacao

  /**
  * f_CriarLotesPex – Funcionalidade SAP Pex - Permite criar lotes Pex
  */
  get f_CriarLotesPex(): boolean {
    return this.verificarPermissao('f_CriarLotesPex', 'tipoProcessoCriacaoSelect', TipoProcessoEnum.PEX);
  }

  //#endregion

  //#region SAP - Lote -> Consulta

  /**
  * f_AlterarDatEscritorioPex - Funcionalidade SAP Pex - Permite alterar lotes Pex
  */
  get f_AlterarDatEscritorioPex(): boolean {
    return this.verificarPermissao('f_AlterarDatEscritorioPex', 'tipoProcessoSelect', TipoProcessoEnum.PEX);
  }

  /**
  * f_CancelarLotesPex – Funcionalidade SAP Pex - Permite cancelar lotes Pex
  */
  get f_CancelarLotesPex(): boolean {
    return this.verificarPermissao('f_CancelarLotesPex', 'tipoProcessoSelect', TipoProcessoEnum.PEX);
  }

  /**
  * f_ExportarLotesPex – Funcionalidade SAP Pex - Permite exportar os registros de lotes, lançamentos, borderô e histórico do status de pagamento.
  */
  get f_ExportarLotesPex(): boolean {
    return this.verificarPermissao('f_ExportarLotesPex', 'tipoProcessoSelect', TipoProcessoEnum.PEX);
  }

  //#endregion

  //  ------------------------------ Trabalhista --------------------------------

  //#region SAP -> Lote -> Criacao

  /**
  * f_CriarLotesTrabalhista – Funcionalidade SAP Trabalhista - Permite criar lotes trabalhistas
    */
  get f_CriarLotesTrabalhista(): boolean {
    return this.verificarPermissao('f_CriarLotesTrabalhista', 'tipoProcessoCriacaoSelect', TipoProcessoEnum.trabalhista);
  }

  //#endregion

  //#region SAP -> Lote -> Consulta

  /**
  * f_AlterarDatEscritorioTrab - Funcionalidade SAP Trabalhista - Permite alterar lotes trabalhistas
  */
  get f_AlterarDatEscritorioTrab(): boolean {
    return this.verificarPermissao('f_AlterarDatEscritorioTrab', 'tipoProcessoSelect', TipoProcessoEnum.trabalhista);
  }

  /**
 * f_CancelarLotesTrabalhista – Funcionalidade SAP Trabalhista - Permite cancelar lotes Trabalhistas
 */
  get f_CancelarLotesTrabalhista(): boolean {
    return this.verificarPermissao('f_CancelarLotesTrabalhista', 'tipoProcessoSelect', TipoProcessoEnum.trabalhista);
  }

  /**
   * f_ExportarLotesTrabalhista – Funcionalidade SAP Trabalhista - Permite exportar os registros de lotes, lançamentos e borderô.
   */
  get f_ExportarLotesTrabalhista(): boolean {
    return this.verificarPermissao('f_ExportarLotesTrabalhista', 'tipoProcessoSelect', TipoProcessoEnum.trabalhista);
  }

  //#endregion
}
