import { Injectable } from '@angular/core';
import { ConsultaSaldoGarantiaService } from './consulta-saldo-garantia.service';
import { getEntreValores, tipoGarantiaString, riscoPerdaString, simNaoindiferente, ativoInativoAmbos, TiposProcessosMapped, statusProcessoString, considerarMigrados } from '@shared/utils';
import { ConsultaTipoProcessoService } from './consulta-tipo-processo.service';
import { ConsultaSaldoGarantiaBancoService } from './consulta-saldo-garantia-banco.service';
import { ConsultaFiltroEmpresaGrupoService } from './consulta-filtro-empresa-grupo.service';
import { ConsultaSaldoGarantiaEstadoService } from './consulta-saldo-garantia-estado.service';
import { AgendamentosSaldoGarantiasService } from 'src/app/core/services/sap/agendamentos-saldo-garantias.service';
import { take, pluck, map } from 'rxjs/operators';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';

@Injectable({
  providedIn: 'root'
})
export class SaldoGarantiaCriteriosPesquisaService {

  constructor(private consultaSaldoService: ConsultaSaldoGarantiaService,
    private tipoProcessoService: ConsultaTipoProcessoService,
    private filtroBanco: ConsultaSaldoGarantiaBancoService,
    private filtroEmpresa: ConsultaFiltroEmpresaGrupoService,
    private filtroEstado: ConsultaSaldoGarantiaEstadoService,
    private agendamentoService: AgendamentosSaldoGarantiasService) { }

  get json() {
    return;
    // return this.consultaSaldoService.json;
  }

  get listaEmpresaGrupo() {
    return this.filtroEmpresa.listaEmpresaGrupo;
  }

  get listaBanco() {
    return this.filtroBanco.listaBanco;
  }

  get listaEstado() {
    return this.filtroEstado.listaEstado;
  }

  converterValores(valores) {
    return {
      tipoProcesso: TiposProcessosMapped.filter(processo => processo.idTipo == valores.tipoProcesso).map(item => item.nome),
      statusProcesso: statusProcessoString(valores.statusDoProcesso),
      dataFinalizacaoContabilInicio: valores.dataFinalizacaoContabil,
      valorDepositoInicio: valores.valorDeposito,
      valorBloqueioInicio: valores.valorBloqueio,
      umBloqueio: valores.umBloqueio == 1 ? 'Sim' : null,
      tipoGarantia:
        valores.listaTipoGarantia ? valores.listaTipoGarantia.split(',') : null,
      riscoPerda: valores.listaRiscosPerdas ? riscoPerdaString(valores.listaRiscosPerdas.split(', ')) : null,
      considerarMigrados:
        (valores.tipoProcesso == TipoProcessoEnum.tributarioAdministrativo ||
          valores.tipoProcesso == TipoProcessoEnum.tributarioJudicial) ? null :
          considerarMigrados(valores.considerarMigrados),
      agencia: valores.numeroAgencia,
      conta: valores.numeroConta,
      idsProcessos: valores.listaProcessos,
      idsBancos: valores.listaBancos ? valores.listaBancos.split(',') : null,
      idsEstados: valores.listaEstados ? valores.listaEstados.split(',') : null,
      idsEmpresasGrupo: valores.listaEmpresas ? valores.listaEmpresas.split(',') : null
    }
  }



  valoresConvertidosCriterios(codigoAgendamento) {
    return this.agendamentoService.consultasCriteriosPesquisa(codigoAgendamento)
      .pipe(map(item => item['data']));

  }




  /**
   * Essa função retorna os nomes dos valores escolhidos na dualList
   * @param nomeService o nome dado ao service que contém a lista
   * @param lista o nome da lista no service
   * @param valores a lista de ids selecionadas no filtro
   * @param hastitulo verifica se possui ou não titulo no dualList
   */
  getNomesDuallistSelecionados(lista, valores: any[], hastitulo: boolean): string[] {
    const componente = this;
    let listaFinal = [];
    if (valores) {
      if (!hastitulo) {
        listaFinal = componente[lista].filter(item => valores.includes(item.id))
          .map(nome => nome.label);
      } else {
        componente[lista].forEach(item =>
          item.dados.forEach(dados => listaFinal.push(dados))
        );

        listaFinal = listaFinal.filter(item => valores.includes(item.id)).map(nome => nome.descricao);
      }

      return listaFinal;
    }
    return;
  }
}
