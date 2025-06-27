import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { ColunaGenerica } from '@manutencao/models/coluna-generica.model';
import { Permissoes, PermissoesService } from '@permissoes';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Page } from '@shared/types/page';
import { EscritorioModalComponent } from '../modals/escritorio-modal/escritorio-modal.component';
import { NovaParametrizacaoModalComponent } from '../modals/nova-parametrizacao-modal/nova-parametrizacao-modal.component';
import { DistribuicaoModel } from '../models/parametrizar-distribuicao-processos/distribuicao.model';
import { ParametrizarDistribuicaoProcessosService } from '../services/parametrizar-distribuicao-processos.service';
import { AnexoModalComponent } from '../modals/anexo-modal/anexo-modal.component';

@Component({
  selector: 'app-parametrizar-distribuicao-processos-escritorios',
  templateUrl: './parametrizar-distribuicao-processos-escritorios.component.html',
  styleUrls: ['./parametrizar-distribuicao-processos-escritorios.component.scss']
})
export class ParametrizarDistribuicaoProcessosEscritoriosComponent implements AfterViewInit {

  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  constructor(
    private messageService: HelperAngular,
    private service: ParametrizarDistribuicaoProcessosService,
    private breadcrumbsService: BreadcrumbsService,
    private permissoes: PermissoesService,
  ) { }

  // async ngOnInit() {
  // }

  async ngAfterViewInit(): Promise<void> {
    this.apenasConsultarParamDistribuEscrit = await this.permissoes.temPermissaoPara(Permissoes.CONSULTAR_PARAM_DISTRIBU_ESCRIT);
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_PARAM_DISTRIBU_ESCRITORIO);
    await this.buscarDistribuicao();
    await this.buscarUf();
    await this.buscarComarca('-1');
    await this.obterEmpresaCentralizadora();
    await this.buscarVara();
    await this.buscarNatureza();
    await this.buscarEscritorio();
  }

  apenasConsultarParamDistribuEscrit: boolean;
  breadcrumb: string;
  //#region filtro
  ufList = [];
  ufSelecionada = '-1';
  comarcaList = [];
  comarcaSelecionada = -1;
  varaList = [];
  varaSelecionada = { codigos: '-1|-1', nome: 'TODAS' };
  naturezaList = [];
  naturezaSelecionada = -1;
  empCentList = [];
  empCentSelecionada = -1;
  escritorioList = [];
  escritorioSelecionada = -1;
  //#endregion
  dadosDistribuicao: Array<DistribuicaoModel>;
  totalRegistros: number
  distribuicaoSelecionada = -1;
  expandir = false;
  colunas: Array<ColunaGenerica> = [
    new ColunaGenerica('Código', 'Codigo', true, '6%', 'Codigo', {
      display: 'inline-block'
    }),
    new ColunaGenerica('Natureza', 'Natureza', true, '19%', 'Natureza', {
      display: 'inline-block'
    }),
    new ColunaGenerica('UF', 'UF', true, '5%', 'UF', {
      display: 'inline-block'
    }),
    new ColunaGenerica('Comarca', 'Comarca', true, '19%', 'Comarca', {
      display: 'inline-block'
    }),
    new ColunaGenerica('Vara', 'Vara', true, '19%', 'Vara', {
      display: 'inline-block'
    }),
    new ColunaGenerica('Empresa', 'Empresa', true, '19%', 'Empresa', {
      display: 'inline-block'
    }),
    new ColunaGenerica('Status', 'Status', true, '7%', 'Status', {
      display: 'inline-block'
    }),
  ];
  ordenacaoColuna: 'Codigo' | 'UF' | 'Comarca' | 'Vara' | 'Natureza' | 'Empresa' | 'Status';
  ordenacaoDirecao: 'asc' | 'desc' | null = 'asc';
  ehAsc: true | false;

  //#region BUSCAR DADOS
  async buscarDistribuicao() {
    const page: Page = { index: this.iniciaValoresDaView().pageIndex, size: this.iniciaValoresDaView().pageSize };

    this.service.obterDistribuicao(this.ufSelecionada, this.comarcaSelecionada, this.varaSelecionada.codigos, this.naturezaSelecionada, this.empCentSelecionada, this.escritorioSelecionada, this.ordenacaoColuna, this.ehAsc, page.index - 1, page.size).then(x => {
      this.dadosDistribuicao = x.lista.length == 0 ? undefined : x.lista;
      this.totalRegistros = x.total
    });
  }

  async buscarUf() {
    await this.service.obterUf().then(x => {
      this.ufList = x;
      this.ufList.unshift({ nome: 'TODAS', codigo: '-1' })
    });
  }

  async buscarComarca(uf: string) {
    await this.service.obterComarca(uf).then(x => {
      this.comarcaList = x;
    });
  }

  async buscarVara(codComarca?: number, natureza?: number) {
    await this.service.obterVara(codComarca, natureza).then(x => {
      this.varaList = x;
    });
  }

  async buscarNatureza() {
    await this.service.obterNatureza().then(x => {
      this.naturezaList = x;
      this.naturezaList.unshift({ nome: 'TODAS', codigo: -1 })
    });
  }

  async obterEmpresaCentralizadora() {
    await this.service.obterEmpresasCentralizadora().then(x => {
      this.empCentList = x;
    });
  }

  async buscarEscritorio() {
    await this.service.obterEscritorios([-1]).then(x => {
      this.escritorioList = x;
      this.escritorioList.unshift({ nome: 'TODOS', codigo: -1 })
    });
  }

  //#endregion

  iniciaValoresDaView() {
    return {
      pageIndex: this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize,
    }
  }

  selecionarDistribuicao(index: number): void {
    if (this.distribuicaoSelecionada == index) {
      this.distribuicaoSelecionada = -1;
      return;
    }
    this.distribuicaoSelecionada = index;
  }

  async ordenar(data) {
    this.ordenacaoColuna = data;
    if (this.ordenacaoDirecao == 'asc') this.ordenacaoDirecao = 'desc';
    else if (this.ordenacaoDirecao == 'desc') {
      this.ordenacaoDirecao = null;
      this.ordenacaoColuna = null;
    }
    else this.ordenacaoDirecao = 'asc';
    this.ehAsc = this.ordenacaoDirecao == 'asc'
    await this.buscarDistribuicao();
  }

  async removerDistribuicao(id) {
    await this.messageService.MsgBox2('Deseja excluir a Distribuição selecionado?', 'Excluir Distribuição',
      'question', 'Sim', 'Não').then(async res => {
        if (res.value) {
          await this.service.excluirDistribuicao(id).then();
          await this.buscarDistribuicao();
        }
      });
  }

  async downloadLog() {
    await this.service.downloadLog();
  }

  async downloadLista() {
    if (this.ordenacaoColuna == undefined || this.ehAsc == undefined) {
      this.ordenacaoColuna = 'Codigo'
      this.ehAsc = true
    }
    await this.service.downloadLista(this.ufSelecionada, this.comarcaSelecionada, this.varaSelecionada.codigos, this.naturezaSelecionada, this.empCentSelecionada, this.escritorioSelecionada, this.ordenacaoColuna, this.ehAsc);
  }

  async alterardistribuicao(distribuicao: any): Promise<void> {
    const teveAlteracao: boolean = await EscritorioModalComponent.exibeModalDeAlterarDistribuicao(distribuicao);
    if (teveAlteracao)
      this.buscarDistribuicao();
  }

  async novaParametrizacao(): Promise<void> {
    const teveAlteracao: boolean = await NovaParametrizacaoModalComponent.exibeModal();
    if (teveAlteracao)
      this.buscarDistribuicao();
  }

  async alterarAnexo(codParamDistribuicao: number): Promise<void> {
    const teveAlteracao: boolean = await AnexoModalComponent.exibeModalAnexo(codParamDistribuicao);
    if (teveAlteracao)
      this.buscarDistribuicao();
  }

  async fazerBuscaComarca(uf: string) {
    await this.buscarComarca(uf)
    this.comarcaSelecionada = -1
    this.varaSelecionada = { codigos: '-1|-1', nome: 'TODAS' }
    await this.buscarVara(-1)
  }

  async limparBusca() {
    this.ufSelecionada = '-1';
    this.comarcaSelecionada = -1;
    this.varaSelecionada = { codigos: '-1|-1', nome: 'TODAS' };
    this.naturezaSelecionada = -1;
    this.escritorioSelecionada = -1;
    this.empCentSelecionada = -1;
    await this.buscarDistribuicao();
  }
}