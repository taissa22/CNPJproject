import { RiscoPerda } from './enum/risco-perda.enum';
import { CredoraDevedora } from './enum/credora-devedora.enum';
import { Vara } from './vara.model';
import { TipoVara } from './tipo-vara.model';
import { ResponsavelInterno } from './responsavel-interno.model';
import { FatoGerador } from './fato-gerador.model';
import { Escritorio } from './escritorio.model';
import { EmpresaDoGrupo } from './empresa-do-grupo.model';
import { ComplementoAreaEnvolvida } from './complemento-area-envolvida.model';
import { Comarca } from './comarca.model';
import { Assunto } from './assunto.model';
import { AreaEnvolvida } from './area-envolvida.model';
import { AdvogadoEscritorio } from './advogado-escritorio.model';
import { Acao } from './acao.model';
import { Estado } from './estado.model';

export class Processo {
  constructor(id: number, numeroPasta: number, numeroProcesso: string, numeroProcessoAntigo: string,
              observacao: string, observacaoFatoGerador: string, recuperacaoJudicial: boolean, acaoPrioritaria: boolean,
              checado: boolean, dataFatoGerador: Date, dataUltimaAtualizacao: Date, classificacao: string,
              dataCadastro: Date, ativo: boolean, acao: Acao, advogadoDoEscritorio: AdvogadoEscritorio,
              areaEnvolvida: AreaEnvolvida, assunto: Assunto, comarca: Comarca, complementoDeAreaEnvolvida: ComplementoAreaEnvolvida,
              credoraDevedora: CredoraDevedora, empresaDoGrupo: EmpresaDoGrupo, escritorio: Escritorio, escritorioAcompanhante: Escritorio,
              estado: Estado, fatoGerador: FatoGerador, responsavelInterno: ResponsavelInterno, tipoVara: TipoVara, vara: Vara, riscoPerda: RiscoPerda,
              dataUltimaFinalizacaoContabil?: Date, autoresDoProcesso?: any) {
    this.id = id;
    this.numeroPasta = numeroPasta;
    this.numeroProcesso = numeroProcesso;
    this.numeroProcessoAntigo = numeroProcessoAntigo;
    this.observacao = observacao;
    this.observacaoFatoGerador = observacaoFatoGerador;
    this.recuperacaoJudicial = recuperacaoJudicial;
    this.acaoPrioritaria = acaoPrioritaria;
    this.checado = checado;
    this.dataFatoGerador = dataFatoGerador;
    this.dataUltimaAtualizacao = dataUltimaAtualizacao;
    this.acao = acao;
    this.advogadoDoEscritorio = advogadoDoEscritorio;
    this.areaEnvolvida = areaEnvolvida;
    this.assunto = assunto;
    this.comarca = comarca;
    this.complementoDeAreaEnvolvida = complementoDeAreaEnvolvida;
    this.credoraDevedora = credoraDevedora;
    this.empresaDoGrupo = empresaDoGrupo;
    this.escritorio = escritorio;
    this.escritorioAcompanhante = escritorioAcompanhante;
    this.estado = estado;
    this.fatoGerador = fatoGerador;
    this.responsavelInterno = responsavelInterno;
    this.tipoVara = tipoVara;
    this.vara = vara;
    this.classificacao = classificacao;
    this.dataCadastro = dataCadastro;
    this.ativo = ativo;
    this.dataUltimaFinalizacaoContabil = dataUltimaFinalizacaoContabil;
    this.riscoPerda = riscoPerda;
    this.autoresDoProcesso = autoresDoProcesso;
  }

  acao: Acao;
  acaoPrioritaria: boolean;
  advogadoDoEscritorio: AdvogadoEscritorio;
  areaEnvolvida: AreaEnvolvida;
  assunto: Assunto;
  checado: boolean;
  comarca: Comarca;
  complementoDeAreaEnvolvida: ComplementoAreaEnvolvida;
  credoraDevedora: CredoraDevedora;
  dataFatoGerador: Date;
  dataUltimaAtualizacao: Date;
  empresaDoGrupo: EmpresaDoGrupo;
  escritorio: Escritorio;
  escritorioAcompanhante: Escritorio;
  estado: Estado;
  fatoGerador: FatoGerador;
  id: number;
  numeroPasta: number;
  numeroProcesso: string;
  numeroProcessoAntigo: string;
  observacao: string;
  observacaoFatoGerador: string;
  recuperacaoJudicial: boolean;
  responsavelInterno: ResponsavelInterno;
  tipoVara: TipoVara;
  vara: Vara;
  classificacao: string;
  dataCadastro: Date;
  ativo: boolean;
  dataUltimaFinalizacaoContabil?: Date;
  riscoPerda: RiscoPerda;
  autoresDoProcesso?: any;
  pedidosDoProcesso?: any;
  partesDoProcesso?: any;
}

