import { AfterViewInit, Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { NgbDateAdapter, NgbDateNativeAdapter, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { RelatorioDeSolicitacoesService } from '@relatorios/services/relatorios-de-solicitacoes.service';
import { JurMultiselectListComponent } from '@shared/components/jur-multiselect-list/jur-multiselect-list.component';
import { itemSelecaoMultiSelectList } from '@shared/components/jur-multiselect-list/models/itemSelecaoMultiSelectList';
import { NgbDateCustomParserFormatter } from '@shared/dateformat';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { DialogService } from '@shared/services/dialog.service';
import moment from 'moment';
import { Permissoes } from 'src/app/permissoes/permissoes';
import { AgendamentoRelatorioSolicitacoesModalComponent } from './modals/agendamento-relatorio-solicitacoes-modal/agendamento-relatorio-solicitacoes-modal.component';
import { BuscarBaixarAgendamentosModalComponent } from './modals/buscar-baixar-agendamentos-modal/buscar-baixar-agendamentos-modal.component';
import { ModelosSalvosModalComponent } from './modals/modelos-salvos-modal/modelos-salvos-modal.component';
import { SalvarComoModeloModalComponent } from './modals/salvar-como-modelo-modal/salvar-como-modelo-modal.component';
import { AgendamentoRelatorioSolicitacoesModel } from './models/agendamento-relatorio-de-solicitacoes.model';
import { ModeloModel } from './models/modelo.model';

@Component({
  selector: 'app-relatorio-de-solicitacoes',
  templateUrl: './relatorio-de-solicitacoes.component.html',
  styleUrls: ['./relatorio-de-solicitacoes.component.scss'],
  providers: [{ provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }, { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }]
})
export class RelatorioDeSolicitacoesComponent implements OnInit, AfterViewInit {
  @ViewChild('colunasSelect', { static: true }) colunasSelect: JurMultiselectListComponent;
  @ViewChild('escritorioSelect', { static: true }) escritorioSelect: JurMultiselectListComponent;
  @ViewChild('solicSelect', { static: true }) solicSelect: JurMultiselectListComponent;
  @ViewChild('lancSelet', { static: true }) lancSelet: JurMultiselectListComponent;
  @ViewChild('ufSelect', { static: true }) ufSelect: JurMultiselectListComponent;

  modeloIdSelecionado: number = 0;

  dataIniSolicitacaoFormControl: FormControl = new FormControl('');
  dataFimSolicitacaoFormControl: FormControl = new FormControl('');
  dataIniVencimentoFormControl: FormControl = new FormControl('');
  dataFimVencimentoFormControl: FormControl = new FormControl('');

  escritorios: itemSelecaoMultiSelectList[] = [];
  statusSolicitacoes: itemSelecaoMultiSelectList[] = [];
  lancamentos: itemSelecaoMultiSelectList[] = [];
  ufs: itemSelecaoMultiSelectList[] = [];
  colunas: itemSelecaoMultiSelectList[] = [];

  escritoriosSelecionados: itemSelecaoMultiSelectList[] = [];
  statusSolicitacoesSelecionados: itemSelecaoMultiSelectList[] = [];
  lancamentosSelecionados: itemSelecaoMultiSelectList[] = [];
  ufsSelecionadas: itemSelecaoMultiSelectList[] = [];
  colunasSelecionadas: itemSelecaoMultiSelectList[] = [];
  breadcrumb: string;

  constructor(
    public relatoriosService: RelatorioDeSolicitacoesService,
    public dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService
  ) { }

  ngOnInit() {
    this.listarColunasSelecionadas();
    this.listaEscritoriosSelecionaveis();
    this.listaEstadosSelecionaveis();
    this.listaTipoLancamentoSelecionaveis();
    this.listaStatusSolicitacaoLancamentoSelecionaveis();
    this.eventos();
  }

  async ngAfterViewInit() {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_SOLICITACOES_PEX);
  }

  eventos() {
    this.relatoriosService.verModelo$.subscribe(modelo => {
      this.preencherTelaComModelo(modelo);
    });
    this.relatoriosService.salvarModelo$.subscribe(modelo => {
      if (this.modeloIdSelecionado) {
        this.EditarModelo();
      }
      else {
        let modeloModel = this.gerarModelModelo();
        modeloModel.nome = modelo.nome;
        modeloModel.descricao = modelo.descricao;
        this.salvarModelo(modeloModel);
      }
    });
  }

  // -------- listagem -----

  listarColunasSelecionadas() {
    this.relatoriosService.listaColunasSelecionaveis().then(res => {
      if (res.query && res.query.length) {
        res.query.map(q => {
          this.colunas.push(new itemSelecaoMultiSelectList(q.identificacaoColuna, q.nomeColuna));
        })
        this.colunasSelect.selecionarTodosOsItens();
      }
    });
  }


  listaEscritoriosSelecionaveis() {
    this.relatoriosService.listaEscritoriosSelecionaveis().then(res => {
      if (res.query && res.query.length) {
        res.query.map(q => {
          this.escritorios.push(new itemSelecaoMultiSelectList(q.codigo, q.nome));
        })
      }
    })
  }

  listaEstadosSelecionaveis() {
    this.relatoriosService.listaEstadosSelecionaveis().then(res => {
      if (res.query && res.query.length) {
        res.query.map(q => {
          this.ufs.push(new itemSelecaoMultiSelectList(q.uf, q.nomeUf));
        })
      }
    })
  }

  listaTipoLancamentoSelecionaveis() {
    this.relatoriosService.listaTipoLancamentoSelecionaveis().then(res => {
      if (res.query && res.query.length) {
        res.query.map(q => {
          this.lancamentos.push(new itemSelecaoMultiSelectList(q.codigo, q.nome));
        })
      }
    })
  }

  listaStatusSolicitacaoLancamentoSelecionaveis() {
    this.relatoriosService.listaStatusSolicitacaoLancamentoSelecionaveis().then(res => {
      console.log(res);
      if (res.listaStatusSolicitacaoLancamento && res.listaStatusSolicitacaoLancamento.length) {

        res.listaStatusSolicitacaoLancamento.map(q => {
          this.statusSolicitacoes.push(new itemSelecaoMultiSelectList(q.codigo, q.nome));
        })
      }
    })
  }

  // ------------

  // ------------ Modais ----

  async agendarExecucao(): Promise<void> {

    await AgendamentoRelatorioSolicitacoesModalComponent.exibeModal(this.gerarModelAgendamento());
  }


  async AbrirModalSalvarModeloComo() {
    await SalvarComoModeloModalComponent.exibeModal(this.gerarModelModelo());
  }

  async abrirModalModelosSalvos() {
    await ModelosSalvosModalComponent.exibeModal();
  }

  async abrirModalAgendamentos() {
    await BuscarBaixarAgendamentosModalComponent.exibeModal();
  }

  // -----------

  // ------ Validacoes ---
  verificarDatas(dataIni: FormControl, dataFim: FormControl, formControlClicado: FormControl, focusout: boolean = false) {
    if (typeof (dataIni.value) == "string" || typeof (dataFim.value) == "string") return false;
    if ((dataIni.value.getFullYear() < 1000 || dataFim.value.getFullYear() < 1000) && !focusout) return false;
    if (moment(dataFim.value).format(moment.HTML5_FMT.DATE) < moment(dataIni.value).format(moment.HTML5_FMT.DATE)) {
      this.dialog.err(
        'A data final não pode ser maior que a data inicial'
      );
      formControlClicado.reset();
    }
  }


  resetarFormControlInvalido(formControl: FormControl) {
    if (formControl.invalid) {
      formControl.reset();
    }
  }

  limparFiltros() {
    this.colunasSelect.limparItensSelecionados();
    this.solicSelect.limparItensSelecionados();
    this.escritorioSelect.limparItensSelecionados();
    this.lancSelet.limparItensSelecionados();
    this.ufSelect.limparItensSelecionados();
    this.dataIniSolicitacaoFormControl = new FormControl('');
    this.dataFimSolicitacaoFormControl = new FormControl('');
    this.dataIniVencimentoFormControl = new FormControl('');
    this.dataFimVencimentoFormControl = new FormControl('');
  }


  /// mascaras ---------

  mascaraDataKeyUp(formControl: FormControl, id: string) {
    if (typeof (formControl.value) != "string") return false;
    let el: any = document.getElementById(id);
    if (formControl.value.length == 2) {
      el.value = formControl.value + "/";
    }
    else if (formControl.value.length == 5) {
      el.value = formControl.value + "/";
    }
  }
  // ------- 
  /// ------ preencher objetos --

  gerarModelAgendamento(): AgendamentoRelatorioSolicitacoesModel {
    let obj: AgendamentoRelatorioSolicitacoesModel = new AgendamentoRelatorioSolicitacoesModel();
    obj.escritoriosSelecionados = this.escritoriosSelecionados.map(e => parseInt(e.id));
    obj.colunasRelatorioSelecionadas = this.colunasSelecionadas.map(e => parseInt(e.id));
    obj.statusSolicitacoesSelecionados = this.statusSolicitacoesSelecionados.map(e => parseInt(e.id));
    obj.tiposDeLancamentoSelecionados = this.lancamentosSelecionados.map(e => parseInt(e.id));
    obj.ufsSelecionadas = this.ufsSelecionadas.map(e => e.id);

    obj.dataIniSolicitacao = this.dataIniSolicitacaoFormControl.value ? moment(this.dataIniSolicitacaoFormControl.value).format(moment.HTML5_FMT.DATE) : "";
    obj.dataFimSolicitacao = this.dataFimSolicitacaoFormControl.value ? moment(this.dataFimSolicitacaoFormControl.value).format(moment.HTML5_FMT.DATE) : "";
    obj.dataIniVencimento = this.dataIniVencimentoFormControl.value ? moment(this.dataIniVencimentoFormControl.value).format(moment.HTML5_FMT.DATE) : "";
    obj.dataFimVencimento = this.dataFimVencimentoFormControl.value ? moment(this.dataFimVencimentoFormControl.value).format(moment.HTML5_FMT.DATE) : "";
    return obj;
  }
  gerarModelModelo(): ModeloModel {
    let obj: ModeloModel = new ModeloModel();
    obj.escritoriosSelecionados = this.escritoriosSelecionados.map(e => parseInt(e.id));
    obj.colunasRelatorioSelecionadas = this.colunasSelecionadas.map(e => parseInt(e.id));
    obj.statusSolicitacoesSelecionados = this.statusSolicitacoesSelecionados.map(e => parseInt(e.id));
    obj.tiposDeLancamentoSelecionados = this.lancamentosSelecionados.map(e => parseInt(e.id));
    obj.ufsSelecionadas = this.ufsSelecionadas.map(e => e.id);

    obj.dataIniSolicitacao = this.dataIniSolicitacaoFormControl.value ? moment(this.dataIniSolicitacaoFormControl.value).format(moment.HTML5_FMT.DATE) : "";
    obj.dataFimSolicitacao = this.dataFimSolicitacaoFormControl.value ? moment(this.dataFimSolicitacaoFormControl.value).format(moment.HTML5_FMT.DATE) : "";
    obj.dataIniVencimento = this.dataIniVencimentoFormControl.value ? moment(this.dataIniVencimentoFormControl.value).format(moment.HTML5_FMT.DATE) : "";
    obj.dataFimVencimento = this.dataFimVencimentoFormControl.value ? moment(this.dataFimVencimentoFormControl.value).format(moment.HTML5_FMT.DATE) : "";
    return obj;
  }

  /// modelo ---

  preencherTelaComModelo(modelo: ModeloModel) {
    this.modeloIdSelecionado = modelo.id;
    this.colunasSelect.selecionarItensPorIds(modelo.colunasRelatorioSelecionadas.join("-").split("-"));
    this.escritorioSelect.selecionarItensPorIds(modelo.escritoriosSelecionados.join("-").split("-"));
    this.solicSelect.selecionarItensPorIds(modelo.statusSolicitacoesSelecionados.join("-").split("-"));
    this.lancSelet.selecionarItensPorIds(modelo.tiposDeLancamentoSelecionados.join("-").split("-"));
    this.ufSelect.selecionarItensPorIds(modelo.ufsSelecionadas);
    if (modelo.dataIniSolicitacao) {
      let arr = modelo.dataIniSolicitacao.split("/");
      this.dataIniSolicitacaoFormControl.setValue(new Date(parseInt(arr[2]), parseInt(arr[1]) - 1, parseInt(arr[0])));
    }
    if (modelo.dataFimSolicitacao) {
      let arr = modelo.dataFimSolicitacao.split("/");
      this.dataFimSolicitacaoFormControl.setValue(new Date(parseInt(arr[2]), parseInt(arr[1]) - 1, parseInt(arr[0])));
    }
    if (modelo.dataIniVencimento) {
      let arr = modelo.dataIniVencimento.split("/");
      this.dataIniVencimentoFormControl.setValue(new Date(parseInt(arr[2]), parseInt(arr[1]) - 1, parseInt(arr[0])));
    }
    if (modelo.dataFimVencimento) {
      let arr = modelo.dataFimVencimento.split("/");
      this.dataFimVencimentoFormControl.setValue(new Date(parseInt(arr[2]), parseInt(arr[1]) - 1, parseInt(arr[0])));
    }
  }

  async EditarModelo() {
    let modelo = this.gerarModelModelo();
    modelo.id = this.modeloIdSelecionado;
    try {
      await this.relatoriosService.editarModelo(modelo).then();
      await this.dialog.alert('Modelo salvo com sucesso');
    } catch (error) {

      await this.dialog.err(error.error);
    };
  }
  private async salvarModelo(modelo: ModeloModel) {
    try {
      await this.relatoriosService.salvarModelo(modelo).then();
      await this.dialog.alert('Modelo salvo com sucesso');
    } catch (error) {
      await this.dialog.err(error.error);
    }
  }
}
