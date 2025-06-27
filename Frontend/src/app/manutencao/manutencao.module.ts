import { TipoDeOrientacaoJuridicaComponent } from './pages/tipo-de-orientacao-juridica/tipo-de-orientacao-juridica.component';
import { TipoDeOrientacaoJuridicaModalComponent } from './modals/tipo-de-orientacao-juridica-modal/tipo-de-orientacao-juridica-modal.component';
import { ManutencaoRoutingModule } from './manutencao-routing.module';
import { ManterParteComponent } from '../manutencao/pages/partes/pages/manter/manter.component';
import { FiltrosParteComponent } from '../manutencao/pages/partes/pages/filtros/filtros.component';
import { PartesComponent } from '../manutencao/pages/partes/pages/consulta/partes.component';
import { NgModule, LOCALE_ID } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from './../shared';
import { ModalModule, TabsModule } from 'ngx-bootstrap';

import { NgSelectModule } from '@ng-select/ng-select';

import { ManutencaoGenericaComponent } from './components/manutencao-generica/manutencao-generica.component';
import { NgbModule, NgbModalModule } from '@ng-bootstrap/ng-bootstrap';

import { AlertModule } from 'ngx-bootstrap/alert';

import { NgxSelectModule, INgxSelectOptions } from 'ngx-select-ex';
import { BsDatepickerModule } from 'ngx-bootstrap';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { TextMaskModule } from 'angular2-text-mask';

import { OrgaosComponent } from './pages/orgaos/orgaos.component';
import { OrgaosModalComponent } from './pages/orgaos/orgaos-modal/orgaos-modal.component';

import { EmpresaCentralizadoraComponent } from '@manutencao/pages/empresas-centralizadora/empresas-centralizadora.component';
import { EmpresaCentralizadoraModalComponent } from '@manutencao/pages/empresas-centralizadora/empresa-centralizadora-modal/empresa-centralizadora-modal.component';

import { EmpresaDoGrupoComponent } from './pages/empresas-do-grupo/empresas-do-grupo.component';
import { FiltrosComponent } from './pages/empresas-do-grupo/components/filtros/filtros.component';
import { ManterComponent } from './pages/empresas-do-grupo/components/manter/manter.component';

import { ConsultaAcoesManutencaoComponent } from './pages/acoes/pages/consulta/consulta.component';
import { ManterAcaoManutencaoComponent } from './pages/acoes/pages/manter-civel-estrategico/manter-civel-estrategico.component';
import { ManterCivelConsumidorComponent } from './pages/acoes/pages/manter-civel-consumidor/manter-civel-consumidor.component';
import { ManterTributarioJudicialComponent } from './pages/acoes/pages/manter-tributario-judicial/manter-tributario-judicial.component';
import { ManterAcaoTrabalhistaComponent } from './pages/acoes/pages/manter-trabalhista/manter-trabalhista.component';

import { ConsultaPedidosManutencaoComponent } from './pages/pedidos/pages/consulta/consulta.component';
import { ManterPedidoEstrategicoManutencaoComponent } from './pages/pedidos/pages/manter-civel-estrategico/manter-civel-estrategico.component';
import { ManterPedidoConsumidorManutencaoComponent } from './pages/pedidos/pages/manter-civel-consumidor/manter-civel-consumidor.component';
import { ManterTrabalhistaComponent } from './pages/pedidos/pages/manter-trabalhista/manter-trabalhista.component';

import { ConsultaAssuntosManutencaoComponent } from './pages/assuntos/pages/consulta/consulta.component';
import { ManterAssuntoCivelConsumidorComponent } from './pages/assuntos/pages/manter-assunto-civel-consumidor/manter-assunto-civel-consumidor.component';
import { ManterAssuntoCivelEstrategicoComponent } from './pages/assuntos/pages/manter-assunto-civel-estrategico/manter-assunto-civel-estrategico.component';

import { ConsultaEstabelecimentosManutencaoComponent } from './pages/estabelecimentos/pages/consulta/consulta.component';
import { ManterEstabelecimentosComponent } from './pages/estabelecimentos/pages/manter/manter.component';

import { ConsultaFielDepositarioComponent } from './pages/fieis-depositarios/pages/consulta/consulta.component';
import { ManterFielDepositarioComponent } from './pages/fieis-depositarios/pages/manter/manter.component';

import { ConsultaProfissionaisComponent } from './pages/profissionais/pages/consulta/consulta/consulta.component';
import { FiltrosProfissionaisComponent } from './pages/profissionais/pages/filtros/filtros/filtros.component';
import { ManterProfissionalComponent } from './pages/profissionais/pages/manter/manter/manter.component';
import { CadastrosCompartilhadosTipoaudienciaComponent } from './manutencao-cadastros-compartilhados//cadastros-compartilhados-tipoaudiencia/cadastros-compartilhados-tipoaudiencia.component';

import { Injector } from '@angular/core';
import { StaticInjector } from './static-injector';
import { ComponentesModule } from '../componentes/componentes.module';

import { defineLocale, enGbLocale } from 'ngx-bootstrap/chronos';
import { ptBrLocale } from 'ngx-bootstrap/locale';
import { CadastrosTipoaudienciaModalComponent } from './manutencao-cadastros-compartilhados/cadastros-compartilhados-tipoaudiencia/cadastros-tipoaudiencia-modal/cadastros-tipoaudiencia-modal.component';

// pages
import { BasesDeCalculoComponent } from './pages/bases-de-calculo/bases-de-calculo.component';
import { TiposDeAudienciaComponent } from './pages/tipos-de-audiencia/tipos-de-audiencia.component';
import { TiposDeParticipacaoComponent } from './pages/tipos-de-participacao/tipos-de-participacao.component';
import { CotacaoComponent } from './pages/cotacao/cotacao.component';
import {ResultadoImportacaoCotacaoComponent}  from  './pages/resultado-importacao-cotacao/resultado-importacao-cotacao.component'
import { IndiceComponent } from './pages/indice/indice.component';
import { IndicesVigenciasCiveisComponent } from './pages/indices_vigencias_civeis/indices_vigencias_civeis.component';
import { TipoDePrazoComponent } from './pages/tipo-de-prazo/tipo-de-prazo.component';
import { TipoDePendenciaComponent } from './pages/tipo-de-pendencia/tipo-de-pendencia.component';
import { TipoDeVaraComponent } from './pages/tipo-de-vara/tipo-de-vara.component';
import { ComarcaComponent } from './pages/comarca/comarca.component';


// modals
import { BaseDeCalculoModalComponent } from './modals/base-de-calculo-modal/base-de-calculo-modal.component';
import { TipoDeAudienciaModalComponent } from './modals/tipo-de-audiencia-modal/tipo-de-audiencia-modal.component';
import { TipoDeParticipacaoModalComponent } from './modals/tipo-de-participacao-modal/tipo-de-participacao-modal.component';
import { TipoDePrazoModalComponent } from './modals/tipo-de-prazo-modal/tipo-de-prazo-modal.component';
import { GrupoEmpresaContabilSapNovoComponent } from './manutencao-cadastros-compartilhados/grupo-empresa-contabil-sap-novo/grupo-empresa-contabil-sap-novo.component';
import { ManutencaoPercentualAtmComponent } from './manutencao-cadastros-compartilhados/cadastros-compartilhados-percentual-atm/manutencao-percentual-atm.component';
import { ModalImportarPercentualAtmComponent } from './manutencao-cadastros-compartilhados/cadastros-compartilhados-percentual-atm/modal-importar-percentual-atm/modal-importar-percentual-atm.component';
import { JurosVigenciasCiveisComponent } from './pages/juros-vigencias-civeis/juros-vigencias-civeis.component';
import { JurosVigenciasCiveisModalComponent } from './modals/juros-vigencias-civeis-modal/juros-vigencias-civeis-modal.component';
import { TiposDeDocumentoComponent } from './pages/tipos-de-documento/tipos-de-documento.component';
import { TiposDeDocumentoModalComponent } from './modals/tipos-de-documento-modal/tipos-de-documento-modal.component';
import { FormsModule } from '@angular/forms';
import { EsferaComponent } from './pages/esfera/esfera.component';
import { IndiceCorrecaoEsferaComponent } from './pages/esfera/indice-correcao-esfera/indice-correcao-esfera.component';


import { ImportarCotacaoModalComponent } from './modals/importar-cotacao-modal/importar-cotacao-modal.component';
import { CotacaoModalComponent } from './modals/cotacao-modal/cotacao-modal.component';
import { IndiceModalComponent } from './modals/indice-modal/indice-modal.component';
import { IndiceVigenciaModalComponent } from './modals/indice-vigencia-modal/indice-vigencia-modal.component';
import { ComplementoAreaEnvolvidaComponent } from './pages/complemento-area-envolvida/complemento-area-envolvida.component';
import { ComplementoAreaEnvolvidaModalComponent } from './modals/complemento-area-envolvida-modal/complemento-area-envolvida-modal.component';
import { ComarcaModalComponent } from './modals/comarca-modal/comarca-modal.component';
import { VaraModalComponent } from './modals/vara-modal/vara-modal.component';
import { VaraComponent } from './pages/comarca/vara/vara.component';


import { ManutencaoParametrizacaoClosingComponent } from './manutencao-parametrizacao-closing/manutencao-parametrizacao-closing.component';
import { CivelConsumidorComponent } from './manutencao-parametrizacao-closing/civel-consumidor/civel-consumidor.component';
import { CivelEstrategicoComponent } from './manutencao-parametrizacao-closing/civel-estrategico/civel-estrategico.component';
import { JuizadoEspecialComponent } from './manutencao-parametrizacao-closing/juizado-especial/juizado-especial.component';
import { TrabalhistaAdministrativoComponent } from './manutencao-parametrizacao-closing/trabalhista-administrativo/trabalhista-administrativo.component';
import { TrabalhistaJudicialComponent } from './manutencao-parametrizacao-closing/trabalhista-judicial/trabalhista-judicial.component';
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { TipoDePendenciaModalComponent } from './modals/tipo-de-pendencia-modal/tipo-de-pendencia-modal.component';
import { TiposDeProcedimentoComponent } from './pages/tipos-de-procedimento/tipos-de-procedimento.component';
import { TipoDeProcedimentoModalComponent } from './modals/tipo-de-procedimento-modal/tipo-de-procedimento-modal.component';
import { TipoDeVaraModalComponent } from './modals/tipo-de-vara-modal/tipo-de-vara-modal.component';
import { EstadosComponent } from './pages/estados/estados.component';
import { EstadoModalComponent } from './modals/estado-modal/estado-modal.component';
import { MunicipiosComponent } from './pages/estados/municipios/municipios.component';
import { MunicipioModalComponent } from './modals/municipio-modal/municipio-modal.component';
import { ObjetoComponent } from './pages/objeto/objeto.component';
import { ObjetoModalComponent } from './modals/objeto-modal/objeto-modal.component';
import { OrientacaoJuridicaTrabalhistaComponent } from './pages/orientacao-juridica-trabalhista/orientacao-juridica-trabalhista.component';
import { OrientacaoJuridicaTrabalhistaModalComponent } from './modals/orientacao-juridica-trabalhista-modal/orientacao-juridica-trabalhista-modal.component';
import { UsuarioOperacaoRetroativaModalComponent } from './modals/usuario-operacao-retroativa-modal/usuario-operacao-retroativa-modal.component';
import { UsuarioOperacaoRetroativaComponent } from './pages/usuario-operacao-retroativa/usuario-operacao-retroativa.component';

import { EsferaModalComponent } from './modals/esfera-modal/esfera-modal.component';
import { IndicecorrecaoesferaModalComponent } from './modals/indice-correcao-esfera-modal/indice-correcao-esfera-modal.component';
import { SisjurTableModule } from '@libs/sisjur/sisjur-table/sisjur-table.module';
import { SisjurPaginatorModule } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.module';
import { EventoDependenteComponent } from './pages/evento/evento-dependente/evento-dependente.component';
import { EventoDependenteModalComponent } from './modals/evento-dependente-modal/evento-dependente-modal.component';
import { EventoComponent } from './pages/evento/evento.component';
import { DecisaoEventoComponent } from './pages/evento/decisao-evento/decisao-evento.component';
import { EventoModalComponent } from './modals/evento-modal/evento-modal.component';
import { DecisaoEventoModalComponent } from './modals/decisao-evento-modal/decisao-evento-modal.component';
import { PrepostoComponent } from './pages/preposto/preposto.component';
import { PrepostoModalComponent } from './modals/preposto-modal/preposto-modal.component';
import { AlocacaoModalComponent } from './modals/preposto-modal/alocacao-preposto/alocacao-preposto.component';
import { FatoGeradorModalComponent } from './modals/fato-gerador-modal/fato-gerador-modal.component';
import { FatoGeradorComponent } from './pages/fato-gerador/fato-gerador.component';
import { PermissaoComponent } from './pages/permissao/permissao.component';
import { PermissaoModalComponent } from './modals/permissao-modal/permissao-modal.component';
import { ManutencaoPerfilComponent } from './manutencao-cadastros-compartilhados/manutencao-perfil/manutencao-perfil.component';
import { FilterDualList } from './manutencao-cadastros-compartilhados/manutencao-perfil/filter-dual-list.pipe';
import { SearchDualListComponent } from '../componentes/search-dual-list/search-dual-list.component';
import { InconsistenciaComponent } from './pages/esfera/inconsistencias/inconsistencias.component';
import { MotivoProvavelZeroModalComponent } from './modals/motivo-provavel-zero-modal/motivo-provavel-zero-modal.component';
import { EscritorioComponent } from './pages/escritorio/escritorio.component';
import { EstadosSelecaoComponent } from '../componentes/estados-selecao/estados-selecao.component';
import { MotivoProvavelZeroComponent } from './pages/motivo-provavel-zero/motivo-provavel-zero.component';
import { AdvogadoComponent } from './pages/escritorio/Advogado/advogado.component';
import { AdvogadoModalComponent } from './pages/escritorio/Advogado/advogado-modal/advogado-modal.component';
import { PerfilComponent } from './pages/perfil/perfil.component';
import { EscritorioModalComponent } from './modals/escritorio-modal/escritorio-modal.component';
import { SolicitantesComponent } from './pages/solicitantes/solicitantes.component';
import { SolicitantesModalComponent } from './modals/solicitantes/solicitantes.component';
import { DeparaStatusAudienciaComponent } from './pages/depara-status-audiencia/depara-status-audiencia.component';
import { DeparaStatusAudienciaModalComponent } from './modals/depara-status-audiencia-modal/depara-status-audiencia-modal.component';
import { AcaoComponent } from './pages/acao/acao.component';
import { AcaoModalComponent } from './modals/acao-modal/acao-modal.component';
import { DeparaStatusNegociacaoComponent } from './pages/depara-status-negociacao/depara-status-negociacao.component';
import { DeparaStatusNegociacaoModalComponent } from './modals/depara-status-negociacao-modal/depara-status-negociacao-modal.component';
import { EmpresaContratadaComponent } from './pages/empresa-contratada/empresa-contratada.component';
import { EmpresaContratadaModalComponent } from './modals/empresa-contratada-modal/empresa-contratada-modal.component';
import { ResultadoNegociacaoComponent } from './pages/resultado-negociacao/resultado-negociacao.component';
import { ResultadoNegociacaoModalComponent } from './modals/resultado-negociacao-modal/resultado-negociacao-modal.component';
import { StatusContatoComponent } from './pages/status-contato/status-contato.component';
import { StatusContatoModalComponent } from './modals/status-contato-modal/status-contato-modal.component';


const opcoesSelect: INgxSelectOptions = {
  optionValueField: 'id',
  optionTextField: 'nome',
  keepSelectedItems: true,
  autoSelectSingleOption: true
};

const maskConfig: Partial<IConfig> = {
  validation: false,
};

ptBrLocale.invalidDate = 'Data Inválida';
defineLocale('pt-br', ptBrLocale);

@NgModule({
  declarations: [
    ManutencaoGenericaComponent,
    OrgaosComponent,
    OrgaosModalComponent,
    EmpresaCentralizadoraComponent,
    EmpresaCentralizadoraModalComponent,
    EmpresaDoGrupoComponent,
    FiltrosComponent,
    ManterComponent,
    PartesComponent,
    FiltrosParteComponent,
    ManterParteComponent,
    ConsultaAcoesManutencaoComponent,
    ManterAcaoManutencaoComponent,
    ManterCivelConsumidorComponent,
    ManterTributarioJudicialComponent,
    ManterAcaoTrabalhistaComponent,
    ConsultaPedidosManutencaoComponent,
    ManterPedidoConsumidorManutencaoComponent,
    ManterPedidoEstrategicoManutencaoComponent,
    ManterTrabalhistaComponent,
    ConsultaAssuntosManutencaoComponent,
    ManterAssuntoCivelConsumidorComponent,
    ManterAssuntoCivelEstrategicoComponent,
    ConsultaEstabelecimentosManutencaoComponent,
    ManterEstabelecimentosComponent,
    ConsultaFielDepositarioComponent,
    ManterFielDepositarioComponent,
    ConsultaProfissionaisComponent,
    FiltrosProfissionaisComponent,
    ManterProfissionalComponent,
    CadastrosCompartilhadosTipoaudienciaComponent,
    CadastrosTipoaudienciaModalComponent,
    GrupoEmpresaContabilSapNovoComponent,
    ManutencaoPercentualAtmComponent,
    ManutencaoParametrizacaoClosingComponent,
    CivelConsumidorComponent,

    // pages
    BasesDeCalculoComponent,
    TiposDeAudienciaComponent,
    TiposDeParticipacaoComponent,
    TipoDePrazoComponent,
    TiposDeDocumentoComponent,
    TiposDeDocumentoComponent,
    CotacaoComponent,
    ResultadoImportacaoCotacaoComponent,
    IndiceComponent,
    IndicesVigenciasCiveisComponent,
    TipoDePendenciaComponent,
    TipoDeOrientacaoJuridicaComponent,
    TipoDeVaraComponent,
    ComplementoAreaEnvolvidaComponent,
    ComarcaComponent,
    ObjetoComponent,
    EventoComponent,
    DecisaoEventoComponent,
    EventoDependenteComponent,

    EsferaComponent,
    IndiceCorrecaoEsferaComponent,
    OrientacaoJuridicaTrabalhistaComponent,
    UsuarioOperacaoRetroativaComponent,
    PermissaoComponent,
    PerfilComponent,
    SolicitantesComponent,
    EmpresaContratadaComponent,
    ResultadoNegociacaoComponent,
    StatusContatoComponent,

    // modals
    BaseDeCalculoModalComponent,
    TipoDeAudienciaModalComponent,
    TipoDeParticipacaoModalComponent,
    TipoDePrazoModalComponent,
    ModalImportarPercentualAtmComponent,

    JurosVigenciasCiveisComponent,
    JurosVigenciasCiveisModalComponent,
    TiposDeDocumentoComponent,
    TiposDeDocumentoModalComponent,

    CotacaoModalComponent,
    ImportarCotacaoModalComponent,
    IndiceModalComponent,
    IndiceVigenciaModalComponent,
    EsferaModalComponent,
    IndicecorrecaoesferaModalComponent,
    InconsistenciaComponent,
    SolicitantesModalComponent,
    EmpresaContratadaModalComponent,
    ResultadoNegociacaoModalComponent,
    StatusContatoModalComponent,

    CivelEstrategicoComponent,
    JuizadoEspecialComponent,
    TrabalhistaAdministrativoComponent,
    TrabalhistaJudicialComponent,
    TipoDePendenciaModalComponent,
    TiposDeProcedimentoComponent,
    TipoDeProcedimentoModalComponent,
    TiposDeDocumentoModalComponent,
    TipoDeOrientacaoJuridicaModalComponent,
    TipoDeVaraModalComponent,
    CotacaoModalComponent,
    ImportarCotacaoModalComponent,
    ComplementoAreaEnvolvidaModalComponent,
    ComarcaComponent,
    ComarcaModalComponent,
    VaraModalComponent,
    VaraComponent,
    PrepostoComponent,
    FatoGeradorComponent,
    MotivoProvavelZeroComponent,
    EscritorioComponent,
    AdvogadoComponent,
    DeparaStatusAudienciaComponent,
    AcaoComponent,
    DeparaStatusNegociacaoComponent,

    EstadosComponent,
    EstadoModalComponent,
    MunicipiosComponent,
    MunicipioModalComponent,
    ObjetoModalComponent,
    OrientacaoJuridicaTrabalhistaComponent,
    OrientacaoJuridicaTrabalhistaModalComponent,
    UsuarioOperacaoRetroativaModalComponent,
    EventoModalComponent,
    DecisaoEventoModalComponent,
    EventoDependenteModalComponent,
    FatoGeradorModalComponent,
    MotivoProvavelZeroModalComponent,
    EscritorioModalComponent,

    PermissaoModalComponent,

    PrepostoModalComponent,
    AlocacaoModalComponent,
    AdvogadoModalComponent,
    DeparaStatusAudienciaModalComponent,
    AcaoModalComponent,
    DeparaStatusNegociacaoModalComponent,

    ManutencaoPerfilComponent,
    SearchDualListComponent,
    FilterDualList,
    EstadosSelecaoComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    ManutencaoRoutingModule,
    NgbModule,
    NgbModalModule,
    AlertModule.forRoot(),
    NgxSelectModule.forRoot(opcoesSelect),
    BsDatepickerModule.forRoot(),
    ModalModule.forRoot(),
    CurrencyMaskModule,
    TextMaskModule,
    NgSelectModule,
    ComponentesModule,
    FormsModule,
    TabsModule.forRoot(),
    NgxMaskModule.forRoot(maskConfig),
    SisjurTableModule,
    SisjurPaginatorModule
  ],
  exports: [ManutencaoGenericaComponent],
  providers: [{ provide: LOCALE_ID, useValue: 'pt-BR' }],
  entryComponents: [
    ManterComponent,
    OrgaosModalComponent,
    EmpresaCentralizadoraModalComponent,
    ManterParteComponent,
    ManterAcaoManutencaoComponent,
    ManterCivelConsumidorComponent,
    ManterTributarioJudicialComponent,
    ManterAcaoTrabalhistaComponent,
    ManterPedidoConsumidorManutencaoComponent,
    ManterPedidoEstrategicoManutencaoComponent,
    ManterTrabalhistaComponent,
    ManterAssuntoCivelConsumidorComponent,
    ManterAssuntoCivelEstrategicoComponent,
    ManterEstabelecimentosComponent,
    GrupoEmpresaContabilSapNovoComponent,
    ManterFielDepositarioComponent,
    ManterProfissionalComponent,
    CadastrosTipoaudienciaModalComponent,
    BaseDeCalculoModalComponent,
    TipoDeAudienciaModalComponent,
    TipoDeParticipacaoModalComponent,
    TipoDePrazoModalComponent,
    ModalImportarPercentualAtmComponent,
    JurosVigenciasCiveisModalComponent,
    TiposDeDocumentoModalComponent,
    CotacaoModalComponent,
    ImportarCotacaoModalComponent,
    IndiceModalComponent,
    IndiceVigenciaModalComponent,
    TipoDePendenciaModalComponent,
    TipoDeProcedimentoModalComponent,
    TiposDeDocumentoModalComponent,
    TipoDeOrientacaoJuridicaModalComponent,
    TipoDeVaraModalComponent,
    ComplementoAreaEnvolvidaModalComponent,
    ComarcaModalComponent,
    VaraModalComponent,
    EstadoModalComponent,
    MunicipioModalComponent,
    ObjetoModalComponent,
    EsferaModalComponent,
    IndicecorrecaoesferaModalComponent,
    OrientacaoJuridicaTrabalhistaModalComponent,
    UsuarioOperacaoRetroativaModalComponent,
    EventoModalComponent,
    EventoComponent,
    DecisaoEventoComponent,
    DecisaoEventoModalComponent,
    PrepostoModalComponent,
    AlocacaoModalComponent,
    EventoDependenteComponent,
    EventoDependenteModalComponent,
    FatoGeradorComponent,
    FatoGeradorModalComponent,
    AdvogadoModalComponent,
    SolicitantesModalComponent,
    EmpresaContratadaModalComponent,
    ResultadoNegociacaoModalComponent,
    StatusContatoModalComponent,

    DeparaStatusAudienciaModalComponent,
    AcaoModalComponent,
    DeparaStatusNegociacaoModalComponent,

    MotivoProvavelZeroModalComponent,
    MotivoProvavelZeroComponent,

    PermissaoModalComponent,
    InconsistenciaComponent,

    EscritorioComponent,
    EscritorioModalComponent,
    EstadosSelecaoComponent,
    AdvogadoComponent,
  ]
})
export class ManutencaoModule {
  constructor(private injector: Injector) {
    StaticInjector.setInjectorInstance(this.injector);
  }
}
