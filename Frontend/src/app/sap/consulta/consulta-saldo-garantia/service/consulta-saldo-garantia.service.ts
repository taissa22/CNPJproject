import { HelperAngular } from '@shared/helpers/helper-angular';
import { SaldoGarantiaService } from 'src/app/core/services/sap/saldo-garantia.service';
import { FiltroModel } from 'src/app/core/models/filtro.model';

import {  take } from 'rxjs/operators';
import { Injectable, Output, EventEmitter, Pipe } from '@angular/core';

import { ConsultaTipoProcessoService } from './consulta-tipo-processo.service';
import {  BehaviorSubject } from 'rxjs';

import { ExibirCampos } from '../interfaces/exibirCampos';
import { ConsultaSaldoDeGarantiaDTO } from '../../../../shared/interfaces/consulta-saldo-de-garantia-dto';



@Injectable({
  providedIn: 'root'
})
export class ConsultaSaldoGarantiaService {

  public processoSelecionado = null;
  public currentCount: number;

  private tituloCriterio = 'Critérios Gerais';
  private tituloBancos = 'Bancos';
  private tituloEmpresa = 'Empresa do Grupo';
  private tituloEstado = 'Estado';
  private tituloProcessos = 'Processos';
  @Output() limparConsulta = new EventEmitter<boolean>(false);

  /**
   * Variavel para verificar se o tipo de garantia e o risco perda estão sendo mostrados
   * na tela
  */
  public exibirCamposCriterios$ = new BehaviorSubject<ExibirCampos>(null);

  //dados para o DTO
  json: ConsultaSaldoDeGarantiaDTO = {
    tipoProcesso: null,
    statusDoProcesso: null,
    dataFinalizacaoContabilInicio: null,
    dataFinalizacaoContabilFim: null,
    valorDepositoInicio: null,
    valorDepositoFim: null,
    valorBloqueioInicio: null,
    valorBloqueioFim: null,
    UmBloqueio: false,
    tipoGarantia: null,
    riscoPerda: null,
    numeroAgencia: null,
    numeroConta: null,
    considerarMigrados: null,
    idsBanco: null,
    idsEmpresaGrupo: null,
    idsEstado: null,
    idsProcesso: null,
  }


  //listas
  //public listaBanco: Array<DualListModel> = [];
  //public listaEstado: Array<DualListModel> = [];

  public getListaFiltro: FiltroModel[] = [{
    id: 1,
    titulo: this.tituloCriterio,
    linkMenu: 'criteriosGeraisGuia',
    selecionado: false,
    ativo: this.consultaTipoProcessoService.tipoProcessoAtual ? true : false,
    marcado: false,
    tituloPadrao: this.tituloCriterio
  },
  {
    id: 2,
    titulo: this.tituloBancos,
    linkMenu: 'bancoGuia',
    selecionado: false,
    ativo: false,
    marcado: false,
    tituloPadrao: this.tituloBancos
  },
  {
    id: 3,
    titulo: this.tituloEmpresa,
    linkMenu: 'empresaGrupoGuia',
    selecionado: false,
    ativo: false,
    marcado: false,
    tituloPadrao: this.tituloEmpresa
  },
  {
    id: 4,
    titulo: this.tituloEstado,
    linkMenu: 'estadoGuia',
    selecionado: false,
    ativo: false,
    marcado: false,
    tituloPadrao: this.tituloEstado
  },
  {
    id: 5,
    titulo: this.tituloProcessos,
    linkMenu: 'processosGuia',
    selecionado: false,
    ativo: false,
    marcado: false,
    tituloPadrao: this.tituloProcessos
  },

  ];

  constructor(private consultaTipoProcessoService: ConsultaTipoProcessoService,
    public tipoProcessoService: ConsultaTipoProcessoService,
    private service: SaldoGarantiaService, private messageService: HelperAngular) { }

  limparFiltros() {
    this.json = {
      tipoProcesso: null,
      statusDoProcesso: null,
      dataFinalizacaoContabilInicio: null,
      dataFinalizacaoContabilFim: null,
      valorDepositoInicio: null,
      valorDepositoFim: null,
      valorBloqueioInicio: null,
      valorBloqueioFim: null,
      UmBloqueio: false,
      tipoGarantia: null,
      riscoPerda: null,
      numeroAgencia: null,
      numeroConta: null,
      considerarMigrados: null,
      idsBanco: null,
      idsEmpresaGrupo: null,
      idsEstado: null,
      idsProcesso: null,
    }

  }

  getFiltros(): FiltroModel[] {
    return this.getListaFiltro;

  }

  atualizaCount(counter: number, id: number) {
    // let tituloAux :string
    if (!id) {
      this.getListaFiltro.map(item => {
        item.titulo = item.tituloPadrao;
        item.selecionado = false;
      });
    } else {
      this.getListaFiltro.map(item => {
        if (item.id === id) {
          this.currentCount = counter;
          item.selecionado = true;
          if (counter === 0) {
            item.titulo = item.tituloPadrao;
            item.selecionado = false;
          } else {
            item.titulo = item.tituloPadrao + ' (' + counter + ') ';
            item.selecionado = true;
          }
        }
      });
    }


  }

  verificarFiltroIncompleto(filtros: ConsultaSaldoDeGarantiaDTO){
    const listaErros: string[] = [];
    let textoErros: string = '';
    if (!filtros.tipoGarantia && this.exibirCamposCriterios$.getValue().tipoGarantia) {
      listaErros.push(
        `Selecione pelo menos um tipo de garantia!`
      );
    }
    if (!filtros.riscoPerda && this.exibirCamposCriterios$.getValue().riscoPerda) {
      listaErros.push(
        `Selecione pelo menos um risco de perda!`
      );
    }
    if (listaErros.length > 0) {
      listaErros.forEach(erro => {
        textoErros += `${erro} <br>`;
      });
    }
    return textoErros;
  }

  fecharModal = new BehaviorSubject<boolean>(false);

  realizarAgendamento(nomeAgendamento: string, filtros: ConsultaSaldoDeGarantiaDTO) {
    if (this.verificarFiltroIncompleto(filtros) != '') {
      this.messageService.MsgBox2(this.verificarFiltroIncompleto(filtros),
        'Filtro Incompleto', 'warning', 'Ok')
        this.fecharModal.next(true);
    } else {
      this.service.agendar(nomeAgendamento, filtros)
        .pipe(take(1))
        .subscribe(
          response => {
            if (response['sucesso']) {
              this.messageService.MsgBox2('', 'Agendamento realizado com sucesso', 'success', 'Ok')
              this.limparFiltros();
              this.tipoProcessoService.LimparTipoProcesso()
              this.limparConsulta.next(true)
              this.fecharModal.next(true)
            } else this.messageService.MsgBox2(response['mensagem'], 'Ops!', 'warning', 'Ok')
          }
        )
    }
  }






}
