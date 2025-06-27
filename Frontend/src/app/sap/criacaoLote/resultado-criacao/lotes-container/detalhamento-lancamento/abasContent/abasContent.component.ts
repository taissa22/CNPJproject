import { LoteService } from 'src/app/core/services/sap/lote.service';
import { Component, OnInit, Input, AfterViewInit } from '@angular/core';
import { CriacaoService } from 'src/app/sap/criacaoLote/criacao.service';

import { AbasContentService } from './abas-content.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { FormBorderoService } from '../form-bordero/form-bordero.service';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { LoteCriacaoBorderoDto } from '@shared/interfaces/lote-criacao-bordero-dto';
import { take, distinctUntilChanged } from 'rxjs/operators';
import { BorderoCadastroComponent } from '../form-bordero/bordero-cadastro.component/bordero-cadastro.component';
import { TextareaLimitadoService } from 'src/app/sap/criacaoLote/services/textarea-limitado.service';
import { Subscription } from 'rxjs';


@Component({
  // tslint:disable-next-line: component-selector
  selector: 'abasContent',
  templateUrl: './abasContent.component.html',
  styleUrls: ['./abasContent.component.scss']
})
export class AbasContentComponent implements AfterViewInit {


  @Input() ativarBordero: boolean;

  lancamentos;
  empresaSelecionada;
  ifLancamento = true;

  public totalValorLiquidoTable = 0;

  public isAllCheckboxChecked = true;

  listaColunasBordero;
  listaValoresBordero: LoteCriacaoBorderoDto[];
  bsModalRef: BsModalRef;

  private _subscription: Subscription[] = [];

  public totalValorBordero = 0;
  public headerLancamento = [];

  constructor(private criacaoService: CriacaoService,
    private loteService: LoteService,
    private service: AbasContentService,
    private modalService: BsModalService,
    private borderoService: FormBorderoService,
    private helperService: HelperAngular,
    private textAreaLimitadoService: TextareaLimitadoService
  ) { }

  ngAfterViewInit(): void { 

    let inicio = true;

    this._subscription.push(this.criacaoService.filtroLoteEscolhido.pipe(take(1), distinctUntilChanged()).subscribe(lote => {
      this.ObterLancamentoDoCriacao(lote);
    }));

    this._subscription.push(this.criacaoService.borderosSubject.subscribe(borderos => {
      this.listaValoresBordero = borderos;
      this.updateValorTotalBordero();
    }));

    this.getColunasBordero();

    this.listaValoresBordero = this.criacaoService.getlistaValoresBordero();
    this._subscription.push(this.borderoService.onBorderoChanges.subscribe(() => {
      this.updateValorTotalBordero();
    }));

    // Coleta dados para criação dos lançamentos sleecionados
    this._subscription.push(this.service.onCriacaoLote.subscribe(() => {
      this.getCriacaoData();
    }));

    this._subscription.push(this.loteService.sucessoCriarLote.subscribe(sucesso => {


      if (sucesso === true) {
        this.criacaoService.borderosSubject.next([]);
        if (this.lancamentos) {
          for (var i = 0; i < this.lancamentos.length; i++) {
            let obj = this.lancamentos[i];

            if (obj.checked) {
              this.lancamentos.splice(i, 1);
              i--;
            }
          }
          this.service.lancamentos.next(this.lancamentos);

        }
      } else {
        this.loteService.ERROMSG.pipe(take(1)).subscribe(erro => {
          if (!inicio) {
            this.loteService.lancamentosTableErro.pipe(take(1)).subscribe(lancamentosErro => {
              lancamentosErro.forEach((lancamentoErro) => {
                this.lancamentos.forEach((lancamento, index) => {
                  if (lancamento.codigoLancamento == lancamentoErro.codigoLancamento &&
                    lancamento.codigoProcesso == lancamentoErro.codigoProcesso) {
                    this.lancamentos[index].mensagemErro = lancamentoErro.mensagemErro;
                  }
                });
              });
            });
            this.helperService.MsgBox2(erro || 'Ocorreu um erro com a comunicação com o banco de dados',
              `Desculpe! O lote não pode ser gerado`,
              'warning', 'OK');
          }
          this.loteService.ERROMSG.next('');
        });
      }
      this.checkAll();
      this.limparIdentificacaoLote();
    }));

    inicio = false;
  }

  ngOnDestroy(): void {
    this._subscription.map(s => {
      if(s != undefined) s.unsubscribe();
    });
  }


  private checkAll() {
    // Simulando click na checkbox que marca tudo.
    this.isAllCheckboxChecked = true;
    this.onClickAllCheckbox();
    this.updateTotalValorLiquido();
  }

  private limparIdentificacaoLote() {
    // Limpando identificação do lote
    this.textAreaLimitadoService.updateTexto('');
  }

  atualizaLançamentosLista(lancamentosNovos) {
    lancamentosNovos.forEach(lancamento => {
      // tslint:disable-next-line: variable-name
      const index = this.lancamentos.findIndex(lancamento_ => lancamento.codigoLancamento === lancamento_.codigoLancamento);
      if (this.lancamentos[index]) {
        this.lancamentos[index].mensagemErro = lancamento.mensagemErro;
      }
    });
  }

  public updateTableLancamento() {
    this.createHeader();
    if (this.lancamentos) {
      this.lancamentos.forEach(lancamento => lancamento.checked = true);
    }
    this.isAllCheckboxChecked = true;
    this.updateTotalValorLiquido();
  }

  /**
   * Cria/atualiza o header da grid.
   */
  public createHeader() {
    if (this.lancamentos) {
      this.headerLancamento = Object.keys(this.lancamentos[0]);
      this.headerLancamento.push('mensagemErro');
      this.headerLancamento = this.headerLancamento.filter(e => (e != 'codigoProcesso' &&
        e != 'codigoLancamento' &&
        e != 'codigoParte' &&
        e != 'codigoStatusPagamento'))
    }
  }

  /**
   * Coleta dados para criação dos lançamentos selecionados
   */
  private getCriacaoData() {
    const lancamentosSelecionados = this.getSelectedRows();

    // Transformando os lançamentos no DTO que a API precisa.
    const lancamentosParaAPI = [];
    lancamentosSelecionados.forEach(lancamento => {
      const lancamentoAPI = {
        codigoProcesso: '',
        codigoLancamento: '',
        valorLancamento: '',
        codigoStatusPagamento: '',
        mensagemErro: ''
      };

      lancamentoAPI.codigoProcesso = lancamento.codigoProcesso;
      lancamentoAPI.codigoLancamento = lancamento.codigoLancamento;
      lancamentoAPI.valorLancamento = lancamento.valorLiquido;
      lancamentoAPI.codigoStatusPagamento = lancamento.codigoStatusPagamento;
      lancamentoAPI.mensagemErro = lancamento.mensagemErro || '';
      lancamentosParaAPI.push(lancamentoAPI);
    });

    this.service.lancamentosAPIData.next(lancamentosParaAPI);
    return lancamentosParaAPI;
  }


  public ObterLancamentoDoCriacao(lote) {
    const json = {
      codigoTipoProcesso: this.criacaoService.tipoProcessoSelecionado,
      codigoEmpresaGrupo: lote.codigoEmpresaGrupo,
      codigoCentroCusto: lote.codigoCentroCusto,
      codigoFornecedor: lote.codigoFornecedor,
      codigoFormaPagamento: lote.codigoFormaPagamento,
      codigoCentroSap: lote.centroSAP,
      uf: lote.uf,
      dataInicialLancamento: lote.dataCriacaoLancamentoInicio,
      dataFinalLancamento: lote.dataCriacaoLancamentoFim,
      valorInicialLancamento: lote.valorLancamentoInicio,
      ValorFinalLancamento: lote.valorLancamentoFim

    };

    this.criacaoService.listaLotes.next(json);



    this.criacaoService.getObterLancamentoDoCriacao(json).pipe(distinctUntilChanged()).subscribe(response => {

      this.lancamentos = response.data;
      this.createHeader();
      this.lancamentos.forEach(lancamento => {
        lancamento.checked = true;
        lancamento.mensagemErro = '';
      });
      this.checkAll();
    });
  }

  verificarEmpresaSelecionada() {
    this.criacaoService.codigoEmpresaSelecionada.subscribe(val => this.empresaSelecionada = val);
    return this.empresaSelecionada;
  }

  definirLancamento(lancamento: boolean) {
    this.ifLancamento = lancamento;
  }

  getColunasBordero() {
    this.listaColunasBordero = this.criacaoService.listaColunasBordero;
    this.updateValorTotalBordero();
  }

  openModal() {
    this.bsModalRef = this.modalService.show(BorderoCadastroComponent);

    this.modalService.onHidden.subscribe(() => {
      this.criacaoService.borderoSelecionado.next(null);
      this.updateValorTotalBordero();
    });
  }

  excluirBordero(index) {
    const bordero = this.listaValoresBordero[index];
    this.criacaoService.excluirBordero(bordero);
  }

  onClickAllCheckbox() {
    if (this.isAllCheckboxChecked && this.lancamentos) {
      this.lancamentos.forEach(lancamento => {
        lancamento.checked = true;
      });
    } else {
      if (this.lancamentos) {
        this.lancamentos.forEach(lancamento => { lancamento.checked = false; });
      }
    }
    this.updateQtdLancamentosSelecionados();
    this.updateTotalSelectedValorLiquido();
  }

  updateTotalSelectedValorLiquido() {
    const lancamentosSelecionados = this.getSelectedRows();
    let totalLancamentos = 0
    if (lancamentosSelecionados) {
      totalLancamentos = lancamentosSelecionados.reduce((total, lancamento) => total + lancamento.valorLiquido, 0);
    }

    this.criacaoService.valorTotalLancamentosSelecionados = totalLancamentos;
    this.service.lancamentosSelecionados.next(lancamentosSelecionados);
    this.service.totalValorLiquidoSelecionados.next(totalLancamentos);
  }


  onCheckBoxClick() {
    if (this.isAllCheckboxChecked) {
      this.isAllCheckboxChecked = false;
    }
    if (this.lancamentos.every(lancamento => lancamento.checked === true)) {
      this.isAllCheckboxChecked = true;
    }
    this.updateQtdLancamentosSelecionados();
    this.updateTotalSelectedValorLiquido();
  }

  private updateTotalValorLiquido() {
    if (this.lancamentos) {
      this.totalValorLiquidoTable = this.lancamentos.reduce((total, obj) => total + obj.valorLiquido, 0);
    } else {
      this.totalValorLiquidoTable = 0;
    }
  }

  private getSelectedRows() {
    if (this.lancamentos) {
      const lancamentosSelecionados = this.lancamentos.filter(lancamento => lancamento.checked === true);
      return lancamentosSelecionados
    }
  }

  editarBordero(bordero: LoteCriacaoBorderoDto) {
    this.criacaoService.borderoSelecionado.next(bordero);
    this.bsModalRef = this.modalService.show(BorderoCadastroComponent);
  }

  private updateValorTotalBordero() {
    if (this.listaValoresBordero) {
      this.totalValorBordero = this.listaValoresBordero.reduce((total, bordero) => total + bordero.valor, 0);
      this.criacaoService.QuantidadeBordero = this.listaValoresBordero.length;
      this.criacaoService.valorTotalBordero = this.totalValorBordero;
    }


  }
  private updateQtdLancamentosSelecionados() {
    const lancamentosSelecionados = this.getSelectedRows();
    if (lancamentosSelecionados) {
      this.criacaoService.quantidadeLancamentosSelecionados = lancamentosSelecionados.length
    }
  }

}

