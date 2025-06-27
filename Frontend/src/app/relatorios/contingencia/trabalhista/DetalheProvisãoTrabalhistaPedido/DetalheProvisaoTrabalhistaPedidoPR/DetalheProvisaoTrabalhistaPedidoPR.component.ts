import { Component, EventEmitter, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { environment } from '@environment';
import { DetalhesEmpresa } from '@relatorios/models/detalhe-provisao-trabalhista-empresa';
import { DetalheProvisaoTrabalhistaPedidoModel } from '@relatorios/models/detalhe-provisao-trabalhista-pedido';
import { totalDetalhes } from '@relatorios/models/detalhe-provisao-trabalhista-total';
import { DetalheProvisaoTrabalhistaPedidoPRService } from '../../services/detalhe-provisao-trabalhista-pedido-PR.service';

@Component({
  selector: 'app-DetalheProvisaoTrabalhistaPedido',
  templateUrl: './DetalheProvisaoTrabalhistaPedidoPR.component.html',
  styleUrls: ['./DetalheProvisaoTrabalhistaPedidoPR.component.scss']
})
export class DetalheProvisaoTrabalhistaPedidoPRComponent implements OnInit {

  dadosReais: DetalheProvisaoTrabalhistaPedidoModel[];

  dadosCabecalho: DetalhesEmpresa;

  resultadoTotal: totalDetalhes;

  possuiHibrido: string;

  userId: string;
  DataFechamento: string;
  Meses: string;
  Outlier: string;
  DataFechamentoAnterior: string;
  MesesAnterior: string;
  OutlierAnterior: string;
  CodEmpresaCentralizadora: string;
  FiltraCentralizadora: string;
  CodigoEmpresaCentralizadora: string;

  href = environment.s1_url;

  constructor(
    private detalheService: DetalheProvisaoTrabalhistaPedidoPRService,
    private activeRoute: ActivatedRoute) { }

  ngOnInit() {
    this.activeRoute.queryParams.subscribe(params => {
      this.userId = params['Id'];
      this.DataFechamento = params['DataFechamento'];
      this.Meses = params['Meses'];
      this.Outlier = params['Outlier'];
      this.DataFechamentoAnterior = params['DataFechamentoAnterior'];
      this.MesesAnterior = params['MesesAnterior'];
      this.OutlierAnterior = params['OutlierAnterior'];
      this.CodEmpresaCentralizadora = params['CodEmpresaCentralizadora'];
      this.FiltraCentralizadora = params['FiltraCentralizadora'];
      this.CodigoEmpresaCentralizadora = params['CodigoEmpresaCentralizadora'];
    });
    this.paginaAtual = 1;
    this.obterDados();
    this.obterTotais();
    this.obterCabecalho();
    this.obterHibrido();
    this.alterarQuantidadePorPagina();
  }

  //#region paginação

  filterPagRegex = /[^0-9]/g;
  paginaAtual: number;
  totalDeRegistrosPorPagina = 15;
  totalDeRegistros: number;
  totalDePaginas: number;

  paginaAtualChange: EventEmitter<number> = new EventEmitter<number>();
  aoAlterarPagina: EventEmitter<any> = new EventEmitter();
  totalDeRegistrosPorPaginaChange: EventEmitter<number> = new EventEmitter<number>();

  mudancaDePagina() {
    this.paginaAtualChange.emit(this.paginaAtual);
    this.aoAlterarPagina.emit();
  }

  alterarQuantidadePorPagina() {
    this.totalDeRegistrosPorPaginaChange.emit(this.totalDeRegistrosPorPagina);

    this.paginaAtualChange.emit(this.paginaAtual);
    this.aoAlterarPagina.emit();
  }

  selecionarPagina(page: number) {
    if (page > this.totalDePaginas) {
      page = this.totalDePaginas;
    }
    if (page < 1) {
      page = 1;
    }
    this.paginaAtual = page;
    this.obterDados()
  }

  paginaAnterior() {
    if (this.paginaAtual == 1) {
      return this.paginaAtual += 0;
    }
    this.paginaAtual -= 1;
    this.obterDados()
  }

  proximaPagina() {
    if (this.paginaAtual == this.totalDePaginas) {
      return this.paginaAtual += 0;
    }
    this.paginaAtual += 1;
    this.obterDados()
  }

  primeiraPagina() {
    this.paginaAtual = 1;
    this.obterDados()
  }

  ultimaPagina() {
    this.paginaAtual = this.totalDePaginas;
    this.obterDados()
  }

  formatInput(input: HTMLInputElement) {
    input.value = input.value.replace(this.filterPagRegex, '');
  }

  //#endregion paginação

  obterDados() {
    this.detalheService.dadosDetalhes(this.userId, this.paginaAtual)
      .subscribe(result => {
        this.dadosReais = result;
      }
      );
  }

  async obterTotais() {
    this.resultadoTotal = await this.detalheService.dadosTotais(this.userId);
    this.totalDeRegistros = this.resultadoTotal.qtdRegistros;
    this.totalDePaginas = Math.ceil(this.totalDeRegistros / this.totalDeRegistrosPorPagina);
  }

  async obterCabecalho() {
    this.dadosCabecalho = await this.detalheService.dadosCabecalho(this.userId);
  }

  async obterHibrido() {
    this.possuiHibrido = await this.detalheService.possuiHibrido(this.userId);
  }

  async exportar() {
    await this.detalheService.exportar(this.userId);
  }

  abrirPopUp(codigo: number) {
    let Url = `${this.href}/2.0/relatorios/fechamento/provisao/trabalhista/DetalheProvisaoTrabalhistaHistoricoPedidoPopup.aspx?CodigoItemFechamentoProvisao=${this.userId}&CodigoPedido=${codigo}`

    window.open(Url, 'popup', 'width=1200,height=500,scrollbars=no,resizable=no'); return false;
  }

}
