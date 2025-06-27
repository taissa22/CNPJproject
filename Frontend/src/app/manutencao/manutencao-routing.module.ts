import { TipoDeOrientacaoJuridicaComponent } from './pages/tipo-de-orientacao-juridica/tipo-de-orientacao-juridica.component';
import { TipoDePrazoComponent } from './pages/tipo-de-prazo/tipo-de-prazo.component';
import { ConsultaProfissionaisComponent } from './pages/profissionais/pages/consulta/consulta/consulta.component';
import { ConsultaFielDepositarioComponent } from './pages/fieis-depositarios/pages/consulta/consulta.component';
import { ConsultaEstabelecimentosManutencaoComponent } from './pages/estabelecimentos/pages/consulta/consulta.component';
import { PartesComponent } from '../manutencao/pages/partes/pages/consulta/partes.component';
import { EmpresaCentralizadoraComponent } from './pages/empresas-centralizadora/empresas-centralizadora.component';
import { EmpresaDoGrupoComponent } from './pages/empresas-do-grupo/empresas-do-grupo.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CadastrosCompartilhadosTipoaudienciaComponent } from './manutencao-cadastros-compartilhados//cadastros-compartilhados-tipoaudiencia/cadastros-compartilhados-tipoaudiencia.component';
import { OrgaosComponent } from './pages/orgaos/orgaos.component';
import { ConsultaAcoesManutencaoComponent } from './pages/acoes/pages/consulta/consulta.component';
import { ConsultaPedidosManutencaoComponent } from './pages/pedidos/pages/consulta/consulta.component';
import { ConsultaAssuntosManutencaoComponent } from './pages/assuntos/pages/consulta/consulta.component';
// import { ModalPartesComponent } from '../sap/lotes/estorno-lancamentos-pagos/estorno-resultado/modal-partes/modal-partes.component';

import { BasesDeCalculoComponent } from './pages/bases-de-calculo/bases-de-calculo.component';
import { TiposDeAudienciaComponent } from './pages/tipos-de-audiencia/tipos-de-audiencia.component';
import { TiposDeParticipacaoComponent } from './pages/tipos-de-participacao/tipos-de-participacao.component';
import { GrupoEmpresaContabilSapNovoComponent } from './manutencao-cadastros-compartilhados/grupo-empresa-contabil-sap-novo/grupo-empresa-contabil-sap-novo.component';
import { JurosVigenciasCiveisComponent } from './pages/juros-vigencias-civeis/juros-vigencias-civeis.component';
import { ManutencaoParametrizacaoClosingComponent } from './manutencao-parametrizacao-closing/manutencao-parametrizacao-closing.component';
import { TipoDePendenciaComponent } from './pages/tipo-de-pendencia/tipo-de-pendencia.component';
import { RoleGuardService } from '@core/services/Roles.guard.ts.service';
import { ManutencaoPercentualAtmComponent } from './manutencao-cadastros-compartilhados/cadastros-compartilhados-percentual-atm/manutencao-percentual-atm.component';
import { TiposDeDocumentoComponent } from './pages/tipos-de-documento/tipos-de-documento.component';
import { Permissoes } from '@permissoes';
import { TiposDeProcedimentoComponent } from './pages/tipos-de-procedimento/tipos-de-procedimento.component';
import { TipoDeVaraComponent } from './pages/tipo-de-vara/tipo-de-vara.component';
import { EsferaComponent } from './pages/esfera/esfera.component';
import { OrientacaoJuridicaTrabalhistaComponent } from './pages/orientacao-juridica-trabalhista/orientacao-juridica-trabalhista.component';

import { CotacaoComponent } from './pages/cotacao/cotacao.component';
import {ResultadoImportacaoCotacaoComponent}  from  './pages/resultado-importacao-cotacao/resultado-importacao-cotacao.component'

import { IndiceComponent } from './pages/indice/indice.component';
import { IndicesVigenciasCiveisComponent } from './pages/indices_vigencias_civeis/indices_vigencias_civeis.component';
import { ComplementoAreaEnvolvidaComponent } from './pages/complemento-area-envolvida/complemento-area-envolvida.component';

import { ComarcaComponent } from './pages/comarca/comarca.component';
import { Estados } from '@core/models';
import { EstadosComponent } from './pages/estados/estados.component';
import { ObjetoComponent } from './pages/objeto/objeto.component';

import { EventoComponent } from './pages/evento/evento.component';
import { PermissaoComponent } from './pages/permissao/permissao.component';
import { ManutencaoPerfilComponent } from './manutencao-cadastros-compartilhados/manutencao-perfil/manutencao-perfil.component';

import { UsuarioOperacaoRetroativaComponent } from './pages/usuario-operacao-retroativa/usuario-operacao-retroativa.component';
import { PrepostoComponent } from './pages/preposto/preposto.component';

import { FatoGeradorComponent } from './pages/fato-gerador/fato-gerador.component';
import { InconsistenciaComponent } from './pages/esfera/inconsistencias/inconsistencias.component';
import { MotivoProvavelZeroComponent } from './pages/motivo-provavel-zero/motivo-provavel-zero.component';
import { EscritorioComponent } from './pages/escritorio/escritorio.component';
import { DeparaStatusAudienciaComponent } from './pages/depara-status-audiencia/depara-status-audiencia.component';
import { DeparaStatusNegociacaoComponent } from './pages/depara-status-negociacao/depara-status-negociacao.component';
import { PerfilComponent } from './pages/perfil/perfil.component';
import { SolicitantesComponent } from './pages/solicitantes/solicitantes.component';
import { AcaoComponent } from './pages/acao/acao.component';
import { EmpresaContratadaComponent } from './pages/empresa-contratada/empresa-contratada.component';
import { ResultadoNegociacaoComponent } from './pages/resultado-negociacao/resultado-negociacao.component';
import { StatusContatoComponent } from './pages/status-contato/status-contato.component';

const routes: Routes = [
  {
    path: 'perfil/:codigoPerfil',
    component: ManutencaoPerfilComponent,
    data: {
      role: 'm_GrupoEmpContSAP'
    },
    canActivate: [RoleGuardService]
  },
  {
    path: 'grupo-empresa-contabil-sap',
    component: GrupoEmpresaContabilSapNovoComponent,
    data: {
      role: 'm_GrupoEmpContSAP'
    },
    canActivate: [RoleGuardService]
  },
  {
    path: 'manutencao-parametrizacao-closing',
    component: ManutencaoParametrizacaoClosingComponent,
    data: {
      role: 'm_ParametrizacaoClosing'
    },
    canActivate: [RoleGuardService]
  },

  { path: 'partes', component: PartesComponent , canActivate: [RoleGuardService], data: {role: Permissoes.ACESSAR_PARTE}},
  { path: 'empresas-do-grupo', component: EmpresaDoGrupoComponent , canActivate: [RoleGuardService], data: {role: Permissoes.ACESSAR_EMPRESA_DO_GRUPO}},
  { path: 'empresas-centralizadoras', component: EmpresaCentralizadoraComponent , canActivate: [RoleGuardService], data: {role: Permissoes.ACESSAR_EMPRESA_CENTRALIZADORA}},
  { path: 'orgaos', component: OrgaosComponent , canActivate: [RoleGuardService], data: {role: Permissoes.ACESSAR_ORGAO}},

  { path: 'percentual-atm', component: ManutencaoPercentualAtmComponent },

  { path: 'trabalhista/acoes', component: ConsultaAcoesManutencaoComponent, canActivate: [RoleGuardService], data: {role: Permissoes.ACESSAR_ACAO_TRABALHISTA} },
  { path: 'acoes/trabalhista', redirectTo: 'trabalhista/acoes'},
  { path: 'tributario-judicial/acoes', component: ConsultaAcoesManutencaoComponent, canActivate: [RoleGuardService], data: {role: Permissoes.ACESSAR_ACAO_TRIBUTARIA_JUDICIAL} },
  { path: 'acoes/tributario-judicial', redirectTo: 'tributario-judicial/acoes'},
  { path: 'civel-estrategico/acoes', component: ConsultaAcoesManutencaoComponent, canActivate: [RoleGuardService], data: {role: Permissoes.ACESSAR_ACAO_CIVEL_ESTRATEGICO} },
  { path: 'acoes/civel-estrategico', redirectTo: 'civel-estrategico/acoes'},

  { path: 'trabalhista/pedidos', component: ConsultaPedidosManutencaoComponent, canActivate: [RoleGuardService], data: {role: Permissoes.ACESSAR_PEDIDO_TRABALHISTA} },
  { path: 'pedidos/trabalhista', redirectTo: 'trabalhista/pedidos'},
  { path: 'civel-consumidor/pedidos', component: ConsultaPedidosManutencaoComponent, canActivate: [RoleGuardService], data: {role: Permissoes.ACESSAR_PEDIDO_CIVEL_CONSUMIDOR} },
  { path: 'pedidos/civel-consumidor', redirectTo: 'civel-consumidor/pedidos'},
  { path: 'civel-estrategico/pedidos', component: ConsultaPedidosManutencaoComponent, canActivate: [RoleGuardService], data: {role: Permissoes.ACESSAR_PEDIDO_CIVEL_ESTRATEGICO} },
  { path: 'pedidos/civel-estrategico', redirectTo: 'civel-estrategico/pedidos'},

  { path: 'civel-consumidor/assuntos', component: ConsultaAssuntosManutencaoComponent, canActivate: [RoleGuardService], data: {role: Permissoes.ACESSAR_ASSUNTO_CIVEL_CONSUMIDOR} },
  { path: 'assuntos/civel-consumidor', redirectTo: 'civel-consumidor/assuntos'},
  { path: 'civel-estrategico/assuntos', component: ConsultaAssuntosManutencaoComponent, canActivate: [RoleGuardService], data: {role: Permissoes.ACESSAR_ASSUNTO_CIVEL_ESTRATEGICO}  },
  { path: 'assuntos/civel-estrategico', redirectTo: 'civel-estrategico/assuntos'},

  { path: 'estabelecimentos', component: ConsultaEstabelecimentosManutencaoComponent, canActivate: [RoleGuardService], data: {role: Permissoes.ACESSAR_ESTABELECIMENTO} },
  { path: 'fiel-depositario', component: ConsultaFielDepositarioComponent, canActivate: [RoleGuardService], data: {role: Permissoes.ACESSAR_FIEL_DEPOSITARIO}},
  { path: 'objeto', component: ObjetoComponent , canActivate: [RoleGuardService], data: {role: Permissoes.ACESSAR_OBJETO} },
  { path: 'profissionais', component: ConsultaProfissionaisComponent, canActivate: [RoleGuardService], data: {role: Permissoes.ACESSAR_PROFISSIONAL} },
  { path: 'bases-de-calculo', component: BasesDeCalculoComponent, canActivate: [RoleGuardService], data: {role: Permissoes.ACESSAR_BASE_CALCULO}},
  { path: 'tipos-de-audiencia', component: TiposDeAudienciaComponent, canActivate: [RoleGuardService], data: {role: Permissoes.ACESSAR_TIPO_AUDIENCIA}},
  { path: 'tipos-de-participacao', component: TiposDeParticipacaoComponent, canActivate: [RoleGuardService], data: {role: Permissoes.ACESSAR_TIPO_PARTICIPACAO}},
  { path: 'tipos-de-prazo', component: TipoDePrazoComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_TIPO_PRAZO}},
  { path: 'juros-vigencias-civeis', component: JurosVigenciasCiveisComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_JUROS}},
  { path: 'tipos-de-pendencia', component: TipoDePendenciaComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_TIPO_PENDENCIA}},
  { path: 'tipos-de-procedimento', component: TiposDeProcedimentoComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_TIPO_PROCEDIMENTO}},
  { path: 'tipos-de-documento', component: TiposDeDocumentoComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_TIPO_DOCUMENTO}},
  { path: 'tipos-de-orientacao-juridica', component: TipoDeOrientacaoJuridicaComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_TIPO_ORIENTACAO_JURIDICA}},
  { path: 'tipos-de-vara', component: TipoDeVaraComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_TIPO_VARA}},
  { path: 'cotacao', component: CotacaoComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_COTACAO}},
  { path: 'cotacao/resultado-importacao-cotacao', component: ResultadoImportacaoCotacaoComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_COTACAO}},
  { path: 'indice', component: IndiceComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_CADASTRO_INDICES}},
  { path: 'indice-vigencias', component: IndicesVigenciasCiveisComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_CADASTRO_INDICES}},
  { path: 'complemento-area-envolvida', component: ComplementoAreaEnvolvidaComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_COMPLEMENTO_AREA_ENVOLVIDA}},
  { path: 'comarca', component: ComarcaComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_COMARCA}},
  { path: 'estado', component: EstadosComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_ESTADO}},
  { path: 'esfera', component: EsferaComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_ESFERA}} ,
  { path: 'orientacao-juridica-trabalhista', component: OrientacaoJuridicaTrabalhistaComponent, canActivate: [RoleGuardService], data: {role: Permissoes.ACESSAR_ORIENTACAO_JURIDICA_TRABALHISTA}},
  { path: 'usuario-operacao-retroativa', component: UsuarioOperacaoRetroativaComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_OPERACOES_RETROATIVAS}},
  { path: 'evento', component: EventoComponent, canActivate: [RoleGuardService], data: {role: Permissoes.ACESSAR_EVENTO}},
  { path: 'preposto', component: PrepostoComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_PREPOSTO}},
  { path: 'fato-gerador', component: FatoGeradorComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_FATO_GERADOR}},
  { path: 'esfera/inconsistencias', component: InconsistenciaComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_ESFERA}},
  { path: 'permissao', component: PermissaoComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_PERMISSAO}},
  { path: 'perfil', component: PerfilComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_PERFIL}},
  { path: 'motivo-provavel-zero', component: MotivoProvavelZeroComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_MOTIVO_PROVAVEL_ZERO}},
  { path: 'escritorio', component: EscritorioComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_ESCRITORIO}},
  { path: 'solicitantes', component: SolicitantesComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_SOLICITANTE}},
  { path: 'depara-status-audiencia', component: DeparaStatusAudienciaComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_DEPARASTATUS}},
  { path: 'acao', component: AcaoComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_ACAO}},
  { path: 'depara-status-negociacao', component: DeparaStatusNegociacaoComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_DEPARASTATUSNEGOCIACAO}},
  { path: 'empresa-contratada', component: EmpresaContratadaComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_EMPRESA_CONTRATADA}},
  { path: 'resultado-negociacao', component: ResultadoNegociacaoComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_RESULTADO_NEGOCIACAO}},
  { path: 'status-contato', component: StatusContatoComponent, canActivate: [RoleGuardService], data: {role:  Permissoes.ACESSAR_STATUS_CONTATO}}

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ManutencaoRoutingModule {}
