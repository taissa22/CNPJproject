
import { HistoricoModel } from 'src/app/core/models/historico.model';
import { DownloadService } from 'src/app/core/services/sap/download.service';
import { TipoProcessoService } from 'src/app/core/services/sap/tipo-processo.service';
import { PermissoesSapService } from '../../../../permissoes-sap.service';
import { NgbDateAdapter, NgbDateNativeAdapter, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from '@shared/dateformat';
import { take } from 'rxjs/operators';
import { BorderoService } from 'src/app/core/services/sap/bordero.service';
import { Component, OnInit, Input } from '@angular/core';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { SapService } from 'src/app/core/services/sap/sap.service';
import { LancamentoService } from 'src/app/core/services/sap/lancamento.service';
import { HistoricoService } from 'src/app/sap/consultaLote/services/historico.service';
import { DetalheResultadoService } from 'src/app/core/services/sap/detalhe-resultado.service';
import { RelatorioLancamentoModel } from 'src/app/core/models/relatorioLancamento.model';
import { Observable } from 'rxjs';
import { BorderoModel } from 'src/app/core/models/bordero.model';
import { LancamentoModel } from 'src/app/core/models/lancamento.model';
import { FilterService } from '../../../services/filter.service';

@Component({

  // tslint:disable-next-line: component-selector
  selector: 'lancamento-historico',
  templateUrl: './lancamento-historico.component.html',
  styleUrls: ['./lancamento-historico.component.scss'],
  providers: [{ provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }, { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }]
})
// tslint:disable-next-line: class-name
export class lancamentohistoricoComponent implements OnInit {
  faPlus = faPlus;

  listaLancamentos = [];

  @Input() seletor: number;
  @Input() borderoExibir: boolean;
/**
    * verifica se o estatus de pagamento do lançamento é Pedido Sap Pago
    * e habilita o otão de edição
   */
  isStatusPagamentoPago = false;

  temPermissao;
  temPermissaoEditar;
  /**
    * Lancamento selecionado na edição da data de envio que fará o meio termo na hora
    * de editar ou cancelar
   */
  lancamento = {
    data: null,
    erro: false,
    msg: ''
  };
  /**
   * Desativa caso já tenha alguma data em edição que não seja o atual
  */
  isDisabled = false;

  constructor(private sapService: SapService,
              private borderoService: BorderoService,
              private lancamentoService: LancamentoService,
              private historicoService: HistoricoService,
              private detalherService: DetalheResultadoService,
              private downloadService: DownloadService,
              private filterService: FilterService,
              private permissoesSapService: PermissoesSapService
  ) { }


  public detalhamentoSelecionado: string = null;
  public tituloDetalhamento: string;
  public colunasExibir: any[20];

  /**
   * Verifica se o campo Data Escritorio está editavel no momento.
   */
  isEditavel = false;


  lancamentoItens: RelatorioLancamentoModel[];

  lancamentoColuna: any[];



  public observableBordero: Observable<any>;
  public borderoCampos: BorderoModel[];
  public lancamentoCampos: LancamentoModel[];
  public historicoCampos: HistoricoModel[];
  public tipoProcesso = 1;
  public open: boolean;
  public enableBtn: boolean;

  ngOnInit() {
    // seletor 1 = lancamentos; 2 = bordero; 3 = historico
    this.verificarPermissao();

    this.lancamento.data = null;
    this.lancamento.erro = false;
    this.lancamento.msg = '';


    if (this.seletor === 1) {
      this.tituloDetalhamento = this.lancamentoService.tituloDetalhamento;
      this.lancamentoColuna = this.lancamentoService.getColulasLancamento();
      this.enableBtn = true;
    }

    if (this.seletor === 2) {
      this.tituloDetalhamento = this.borderoService.tituloDetalhamento;
      this.lancamentoColuna = this.borderoService.colunasGrid;
    }

    if (this.seletor === 3) {
      this.tituloDetalhamento = this.historicoService.tituloDetalhamento;
      this.lancamentoColuna = this.historicoService.colunasGrid;
    }
  }

  private pegarDados() {
    if (this.seletor === 1) {
      this.lancamentoService.getLancamento(this.detalherService.itemReceived.value, this.filterService.tipoProcessoTracker.value)
        .subscribe(item => {
          item.map(item => {
            if (item.dataEnvioEscritorio == null)
              {}else
            item.dataEnvioEscritorio = new Date(item.dataEnvioEscritorio)
          })
          this.lancamentoCampos = item;
          this.verificarEstorno();
          this.tipoProcesso = this.filterService.tipoProcessoTracker.value;
          this.verificarStatus(item);

        });

    }

    if (this.seletor === 2) {
      this.borderoService.getBordero(this.detalherService.itemReceived.value)
        .subscribe(item => { this.borderoCampos = item; });
    }

    if (this.seletor === 3) {
      this.historicoService.getHistorico(this.detalherService.itemReceived.value)
        .subscribe(item => { this.historicoCampos = item; });
    }


  }

  verificarPermissao() {
    this.temPermissao = this.permissoesSapService.f_ExportarLotesCivelCons ||
      this.permissoesSapService.f_ExportarLotesEstrat ||
      this.permissoesSapService.f_ExportarLotesJuizado ||
      this.permissoesSapService.f_ExportarLotesPex ||
      this.permissoesSapService.f_ExportarLotesTrabalhista;

    this.temPermissaoEditar = this.permissoesSapService.f_AlterarDatEscritorioCons ||
      this.permissoesSapService.f_AlterarDatEscritorioEstrat ||
      this.permissoesSapService.f_AlterarDatEscritorioJuizado ||
      this.permissoesSapService.f_AlterarDatEscritorioPex ||
      this.permissoesSapService.f_AlterarDatEscritorioTrab;

  }

  public plusClick(detalhamentoSelecionado) {
    if (this.seletor !== 2 || !this.borderoExibir !== false) {
      if (this.detalhamentoSelecionado === detalhamentoSelecionado) {
        this.open = false;
        this.detalhamentoSelecionado = null;
      } else {
        this.open = true;
        this.detalhamentoSelecionado = detalhamentoSelecionado;
        this.pegarDados();
      }
    }
  }


  public isExcluido

  verificarEstorno() {
    let verificar;
    verificar = this.lancamentoCampos.filter(item => item.lancamentoEstornado == true)
    if (verificar.length > 0) {
      this.isExcluido = true;}
      else {
        this.isExcluido = false;
      }
    }



  exportar() {
    this.downloadService.baixarExportacaoLancamentos(this.detalherService.itemReceived.value,
      this.filterService.tipoProcessoTracker.value);
  }

  exportarBordero() {
    this.borderoService.baixarExportacaoBordero(this.detalherService.itemReceived.value,
                                                this.filterService.tipoProcessoTracker.value);
  }
/**
    * verifica se o estatus de pagamento do lançamento é Pedido Sap Pago
    * e habilita o botão de edição
    * @param lancamento : lancamneto selecionado para verificar o status
   */
  verificarStatus(lancamento) {
    let t = lancamento.filter(item =>
      item.statusPagamento === 'Pedido SAP Pago');
    if (t.length > 0) {
      this.isStatusPagamentoPago = true;
    } else {
      this.isStatusPagamentoPago = false;
    }
  }


  /**
    * Verifica se deve mostrar a label da Data Envio escritorio ou o input para edição
    * @param coluna : coluna da tabela
   */
  verificarEdicao(coluna) {

    if (coluna === 'Data Envio Escritório' && this.isEditavel) {
      return false;
    } else {
      return true;
    }
  }

  /**
    * Habilitar a edição do campo data escritório.
    * @param lancamento : verifica qual lançamento foi selecionado
    * para a data selecionada abrir para edição
    * caso a data venha nula, o valor global deve ser aberto.
   */
  openEditar(lancamento) {
    this.lancamento.erro = false;
    this.lancamento.msg = '';


    if (lancamento) {
      if (!lancamento.isDisabled) {
        this.lancamento.data = lancamento.dataEnvioEscritorio;

        // libera para edição
        this.lancamentoCampos.filter(item => item === lancamento)
          .map(item => item.isEditavel = true);
        // desativa o resto dos botões
        this.lancamentoCampos.filter(item => item !== lancamento)
          .map(item => {
            item.isDisabled = true;

          });
          this.isDisabled = true;
      }
    } else {
      if (!this.isDisabled) {
        this.lancamento.data = '';
        this.isEditavel = true;
        this.lancamentoCampos.filter(item => item !== lancamento)
          .map(item => item.isDisabled = true);
      }
    }
  }




  /**
    * Salva a data após clicar no botão de sucesso
    * @param dataModificada : verifica qual a data será modificada, caso a data seja nula,
    * todas as datas receberão o valor.
    */
  salvarDatas(dataModificada) {
    this.listaLancamentos = []
    if (!this.lancamento.data) {
      this.lancamento.erro = true;
      this.lancamento.msg = 'Data não pode ser vazia.';
    }
    if (!this.lancamento.erro) {
      if (dataModificada) {
        this.lancamentoCampos.filter(item =>
          item === dataModificada)
          .map(item => {
            item.dataEnvioEscritorio = this.lancamento.data;
            item.isEditavel = false;
            this.listaLancamentos.push(item);
          });

      } else {
         this.lancamentoCampos
          .map(item => {
            item.dataEnvioEscritorio = this.lancamento.data;
            this.listaLancamentos.push(item)
          });
        this.isEditavel = false;
      }

      this.lancamentoCampos.map(item => {
        item.isDisabled = false;
        this.isDisabled = false;
      });
      this.lancamentoService.alterarDataEnvioEscritorio(this.listaLancamentos)
        .pipe(take(1)).subscribe();
    }
  }
  /**
     * Cancela a edição da data de envio escritorio sem nenhuma alteração
     * @param lancamentoCancelado: Verifica o lancamento e retorna o editavel como false,
     * caso não tenha lancamento selecionado, cancela o global.
     *     */
  cancelar(lancamentoCancelado) {

      if (lancamentoCancelado) {
        this.lancamentoCampos.filter(item => item === lancamentoCancelado)
          .map(item => item.isEditavel = false);
      } else {
        this.isEditavel = false;
      }
      this.lancamentoCampos.map(item => {
        item.isDisabled = false;
        this.isDisabled = false;
      });

  }

  /**
     * Verifica se a data é maior que o dia atual ou se está invalida
     * @param data: Define a data que foi escrita no input no blur
     *     */
  validaData(data: any) {
    const hoje = new Date();
    hoje.setHours(12);
    hoje.setMilliseconds(0);
    hoje.setSeconds(0);
    hoje.setMinutes(0);
    data.msg = '';

    if (typeof (data.data) === 'string' || data.data == null) {
      data.erro = true;
      data.msg = 'Data Inválida.';
    }
    if (typeof (data.data) === 'object') {
      if (data.data > hoje) {
        data.erro = true;
        data.msg = 'A data envio escritório não pode ser maior que a data atual';
        // return;
      } else {
        data.erro = false;
        data.msg = '';
      }
    }
  }
}
