import { Closing } from './../../../../core/models/closing.model';
import { EscritorioService } from './services/escritorio.service';
import { PrepostoService } from './services/preposto.service';
import { HttpErrorResult } from '@core/http/http-error-result';
import { Subscription, BehaviorSubject } from 'rxjs';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { SortOrderView } from '@shared/interfaces/Sort/SortOrderView';
import { OrdenacaoDragDropComponent } from './../../../../componentes/ordenacao-drag-drop/ordenacao-drag-drop.component';
import { DialogService } from './../../../../shared/services/dialog.service';
import { Ordenacao } from './../../components/tabela-ordenacao-multi-colunas/model/ordenacao.model';
import { ColunaParaOrdenacao } from './../../components/modal-ordenacao-multi-colunas/model/colunaOrdenacao.model';
import { Coluna } from './../../components/tabela-ordenacao-multi-colunas/model/coluna.model';
import { ModalEditarAudienciaComponent } from './modal-editar-audiencia/modal-editar-audiencia.component';
import { FormControl } from '@angular/forms';
import { Component, OnInit, Inject, LOCALE_ID, Input } from '@angular/core';
import { NgbModal, NgbModalRef, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DatePipe } from '@angular/common';

import { AudienciaService } from './services/audiencia.service';
import { Audiencia } from './model/audiencia.model';

import { ModalPedidosComponent } from './modal-pedidos/modal-pedidos.component';
import { ModalPartesAgendaComponent } from './modal-partes/modal-partes.component';
import { PermissoesService } from 'src/app/permissoes/permissoes.service';
import { Permissoes } from 'src/app/permissoes/permissoes';
import { isNullOrUndefined } from 'util';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';

@Component({
  templateUrl: './agenda-audiencias.component.html',
  styleUrls: ['./agenda-audiencias.component.scss']
})
export class AgendaAudienciasComponent implements OnInit {

  pagina = 1;
  totalDeRegistrosPorPagina = 8;

  registros: Array<Audiencia> = [];
  totalDeRegistros: number;

  escritorios: Array<any> = [];
  prepostos: Array<any> = [];
  breadcrumb: string;

  mensagemSemRegistros = 'Nenhuma Busca Realizada';

  get temPermissaoParaEditarAudiencia(): boolean {
    return this.permissoes.temPermissaoPara(Permissoes.ALTERAR_AUDIENCIA_CE);
  }

  modal: any;
  modalEditar: NgbModalRef;
  modalOrdenacao: BsModalRef;
  subscription: Subscription;
  colunasOrdenacao: Array<SortOrderView> = [
    { direction: 'asc', property: 'prioridadeEstado', columnName: 'Estado' },
    { direction: 'asc', property: 'prioridadeComarca', columnName: 'Comarca' },
    { direction: 'asc', property: 'prioridadeVaraAudiencia', columnName: 'Vara' },
    { direction: 'asc', property: 'prioridadeTipoVaraAudiencia', columnName: 'Tipo Vara' },
    { direction: 'desc', property: 'prioridadeDataAudiencia', columnName: 'Data Audiência' },
    { direction: 'asc', property: 'prioridadeHoraAudiencia', columnName: 'Hora Audiência' },
  ];
  audienciaSelecionada: any;
  colunas: Array<Coluna> = [
    new Coluna('Estado', 'estadoId', '3%', 'pl-3'),
    new Coluna('Comarca', 'comarcaNome', '6%'),
    new Coluna('Vara/Tipo Vara', 'varaTipoVaraNome', '5%'),
    new Coluna('Data da audiência', 'dataAudiencia', '4%', '', new DatePipe(this.locale), ['dd/MM/yyyy']),
    new Coluna('Hora da audiência', 'horaAudiencia', '4%', '', new DatePipe(this.locale), ['HH:mm']),
    new Coluna('Tipo de audiência', 'tipoAudienciaNome', '7%'),
    new Coluna('Preposto', 'prepostoNome', '8.4%'),
    new Coluna('Escritório Audiência', 'escritorioNome', '9.9%'),
    new Coluna('Advogado Escritório da Audiência', 'advogadoEscritorioNome', '8.4%'),
    new Coluna('Estratégico', 'estrategicoId', '3%'),
    new Coluna('Número do Processo', 'numeroProcesso', '6%'),
    new Coluna('Endereço', 'endereco', '8.9%'),
    new Coluna('Escritório do Processo', 'escritorioProcessoNome', '9%'),
    new Coluna('Classificação Closing Móvel', 'closing', '6%'),
    new Coluna('Classificação Closing Fibra', 'closingClientCo', '6%'),
    new Coluna('Classificação Hierárquica', 'classificacao', '8%'),


  ];

  private ultimoFiltro: {
    dataInicial: Date, dataFinal: Date, escritorioId: number, estadoId: string,
    comarcaId: number, empresaGrupoId: number, prepostoId: number, assuntoId: number, closing: string, clientCo: string, classificacaoProcessoId: string
  };

  constructor(
    @Inject(LOCALE_ID) private locale: string,
    private audienciaService: AudienciaService,
    private permissoes: PermissoesService,
    private dialog: DialogService,
    private bsModalService: BsModalService,
    private modalService: NgbModal,
    private prepostosService: PrepostoService,
    private escritoriosService: EscritorioService,
    private breadcrumbsService: BreadcrumbsService
  ) { }

  ngOnInit() {
    this.obterEscritorios();
    this.obterPrepostos();
  }

  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_AGENDA_CE);
  }

  async obter(filtro?: {
    dataInicial: Date, dataFinal: Date, escritorioId: number, estadoId: string,
    comarcaId: number, empresaGrupoId: number, prepostoId: number, assuntoId: number, closing: string, clientCo: string, classificacaoProcessoId: string
  }): Promise<void> {
    this.ultimoFiltro = isNullOrUndefined(filtro) ? this.ultimoFiltro : filtro;

    const agendaFiltro = this.gerarFiltroCompleto();

    try {
      const { data, total } = await this.audienciaService.obterPaginado(agendaFiltro);

      if (total === 0) {
        this.mensagemSemRegistros = 'Nenhuma audiência encontrada com os filtros utilizados';
      }

      this.totalDeRegistros = total;
      this.registros = data.map(x => Audiencia.fromJson(x));
    } catch (error) {
      this.dialog.showErr('Não foi possível carregar as informações', (error as HttpErrorResult).messages.join('\n'));
    }
  }

  async exportar(): Promise<void> {
    try {
      await this.audienciaService.exportar(this.gerarFiltroCompleto());
    } catch (error) {
      console.error(error);
      this.dialog.showErr('Não foi possível exportar as informações', (error as HttpErrorResult).messages.join('\n'));
    }
  }

  async abrirModalOrdenacao(): Promise<void> {
    this.modalOrdenacao = this.bsModalService.show(OrdenacaoDragDropComponent);
    this.modalOrdenacao.content.title = 'Agenda de Audiências';
    this.modalOrdenacao.content.values = this.colunasOrdenacao;

    this.modalOrdenacao
      .content
      .orderedValues
      .subscribe((order: SortOrderView[]) => {
        this.colunasOrdenacao = order;
        console.log(order);
        this.obter();
      });
  }

  gerarFiltroCompleto(): FiltroReturn {
    const queryOrder = this.colunasOrdenacao.map((x, i) => ({ direction: x.direction, ordem: i, column: x.columnName }));

    return {
      pagina: this.pagina,
      quantidade: this.totalDeRegistrosPorPagina,
      dataInicial: this.ultimoFiltro.dataInicial,
      dataFinal: this.ultimoFiltro.dataFinal,
      escritorioId: isNullOrUndefined(this.ultimoFiltro.escritorioId) ? 0 : this.ultimoFiltro.escritorioId,
      estadoId: isNullOrUndefined(this.ultimoFiltro.estadoId) ? '' : this.ultimoFiltro.estadoId,
      comarcaId: isNullOrUndefined(this.ultimoFiltro.comarcaId) ? 0 : this.ultimoFiltro.comarcaId,
      empresaGrupoId: isNullOrUndefined(this.ultimoFiltro.empresaGrupoId) ? 0 : this.ultimoFiltro.empresaGrupoId,
      prepostoId: isNullOrUndefined(this.ultimoFiltro.prepostoId) ? 0 : this.ultimoFiltro.prepostoId,
      assuntoId: isNullOrUndefined(this.ultimoFiltro.assuntoId) ? 0 : this.ultimoFiltro.assuntoId,
      closing: isNullOrUndefined(this.ultimoFiltro.closing) ? '' : this.ultimoFiltro.closing,
      clientCo: isNullOrUndefined(this.ultimoFiltro.clientCo) ? '' : this.ultimoFiltro.clientCo,
      classificacaoProcessoId: isNullOrUndefined(this.ultimoFiltro.classificacaoProcessoId) ? '' : this.ultimoFiltro.classificacaoProcessoId,
      comarca: queryOrder.filter(x => x.column === 'Comarca')[0],
      estado: queryOrder.filter(x => x.column === 'Estado')[0],
      vara: queryOrder.filter(x => x.column === 'Vara')[0],
      tipoVara: queryOrder.filter(x => x.column === 'Tipo Vara')[0],
      dataAudiencia: queryOrder.filter(x => x.column === 'Data Audiência')[0],
      horaAudiencia: queryOrder.filter(x => x.column === 'Hora Audiência')[0],

    };
  }


  async abrirDialogoDeConsulta(tipoConsulta: string): Promise<void> {
    if (!this.audienciaSelecionada) {
      return;
    }

    try {
      switch (tipoConsulta) {
        case 'pedidos-do-processo':
          this.modal = ModalPedidosComponent;
          break;
        case 'partes-do-processo':
          this.modal = ModalPartesAgendaComponent;
          break;
        default:
          this.dialog.showErr('Ocorreu um erro interno', 'Tente novamente mais tarde.');
          break;
      }

      await this.modal.exibeModalDeConsultar(this.audienciaSelecionada.processo);
    } catch (error) {
      console.log(error);
    }
  }


  async abrirDialogoDeAtualizar(): Promise<void> {
    if (!this.audienciaSelecionada) {
      return;
    }
    try {
      ModalEditarAudienciaComponent.exibeModalDeAlterar(this.audienciaSelecionada,
        this.escritorios, this.prepostos).then( async (result: any) => {
          if (result !== 'cancel' && this.temFiltro()) {
            await this.obter(this.ultimoFiltro);
          }
        });
    } catch (error) {
      console.error(error);
    }
  }

  temFiltro() {
    return !isNullOrUndefined(this.ultimoFiltro) &&
      (!isNullOrUndefined(this.ultimoFiltro.dataInicial) ||
        !isNullOrUndefined(this.ultimoFiltro.dataFinal) ||
        !isNullOrUndefined(this.ultimoFiltro.comarcaId) ||
        !isNullOrUndefined(this.ultimoFiltro.empresaGrupoId) ||
        !isNullOrUndefined(this.ultimoFiltro.estadoId) ||
        !isNullOrUndefined(this.ultimoFiltro.prepostoId) ||
        !isNullOrUndefined(this.ultimoFiltro.closing) ||
        !isNullOrUndefined(this.ultimoFiltro.classificacaoProcessoId));
  }

  obterEscritorios(): void {
    this.escritoriosService.obter().then(e => {
      this.escritorios = e.map(r =>
        ({
          id: r.id, nome: r.ativo ?
            r.nome.toUpperCase() :
            r.nome.toUpperCase().replace('[INATIVO]', '').concat(' [INATIVO] '),
          ativo: r.ativo
        }));
    });
  }

  obterPrepostos(): void {
    this.prepostosService.obter().then(p => {
      this.prepostos = p.map(r =>
        ({
          id: r.id,
          nome: r.ativo ? r.nome.toUpperCase() : r.nome.toUpperCase().replace('[INATIVO]', '').concat(' [INATIVO] '),
          ativo: r.ativo
        }));
    });
  }
}


export interface FiltroReturn {
  pagina: any;
  quantidade: any;
  dataInicial: any;
  dataFinal: any;
  escritorioId: any;
  estadoId: any;
  comarcaId: any;
  empresaGrupoId: any;
  prepostoId: any;
  assuntoId: any;
  comarca: any;
  estado: any;
  vara: any;
  tipoVara: any;
  dataAudiencia: any;
  horaAudiencia: any;
  closing: any;
  clientCo: any;
  classificacaoProcessoId: any;
}
