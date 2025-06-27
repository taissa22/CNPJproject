import { LoteCriacaoLancamentoDto } from './../../shared/interfaces/lote-criacao-lancamento-dto';
import { LoteService } from 'src/app/core/services/sap/lote.service';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from 'src/app/core';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { AbasContentService } from './resultado-criacao/lotes-container/detalhamento-lancamento/abasContent/abas-content.service';
import { BehaviorSubject, Subject } from 'rxjs';
import { LoteCriacaoBorderoDto } from '@shared/interfaces/lote-criacao-bordero-dto';
import { TiposProcessosMapped } from '@shared/utils';
import { CriacaoLancamentosModel } from 'src/app/core/models/criacao-lancamentos.model';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';
import { take } from 'rxjs/operators';
import { LoteCriacaoEmpresaGrupoDTO } from '@shared/interfaces/lote-criacao-empresa-grupo-dto';
import Swal from 'sweetalert2';
import { LoteCriacaoGeracaoLoteDto } from '@shared/interfaces/lote-criacao-geracao-lote-dto';
import { formatDate } from '@angular/common';
import { IFiltroCriacaoLote } from './interfaces/IFiltroCriacaoLote';
import { IFiltroEmpresaGrupo } from './interfaces/IFiltroEmpresaGrupo';


const getListaColunasBordero = [
  {
    id: 1,
    titulo: 'Beneficiário'
  },
  {
    id: 2,
    titulo: 'CPF/CNPJ'
  },
  {
    id: 3,
    titulo: 'Banco'
  },
  {
    id: 4,
    titulo: 'DV'
  },
  {
    id: 5,
    titulo: 'Agência'
  },
  {
    id: 6,
    titulo: 'DV'
  },
  {
    id: 7,
    titulo: 'N° C/C'
  },
  {
    id: 8,
    titulo: 'DV'
  },
  {
    id: 9,
    titulo: 'Valor'
  },
  {
    id: 10,
    titulo: 'Cidade'
  },
  {
    id: 11,
    titulo: 'Histórico'
  },
  {
    id: 12,
    titulo: ''
  }
];


@Injectable({
  providedIn: 'root'
})




export class CriacaoService {
  constructor(private router: Router,
    private loteService: LoteService,
    private helperAngular: HelperAngular,
    private abasService: AbasContentService) {
    this.borderosSubject = new BehaviorSubject<LoteCriacaoBorderoDto[]>([]);
  }

  manterDados = false;

  // pegar valores de quantidades de bordero e lancamentos
  public quantidadeLancamentosSelecionados = 0;
  public valorTotalLancamentosSelecionados = 0;
  public QuantidadeBordero = 0;
  public valorTotalBordero = 0;
  ///////////////////////////////

  public onLancamentoInfoUpdated = new BehaviorSubject(false);

  get tipoProcessoSelecionado() {
    return this.tipoProcesso.value;
  }

  set tipoProcessoSelecionado(v) {
    this.tipoProcesso.next(v);
  }

  get nomeProcesso() {
    let r;
    TiposProcessosMapped.filter(i => i.idTipo === this.tipoProcessoSelecionado).map(n => r = n.nome); return r;
  }

  get getListaLancamentos() {
    return this.getListaLancamento.filter(item => item.codigoProcesso === this.tipoProcessoSelecionado);
  }


  get listaColunasBordero() {
    return getListaColunasBordero;
  }
  public codigoFornecedor = new BehaviorSubject<number>(null);
  public codigoFormaPagamento = new BehaviorSubject<number>(null);
  public uf = new BehaviorSubject<number>(null);
  /** Lote escolhido com os filtros */
  public filtroLoteEscolhido = new BehaviorSubject<any>(null);

  public tipoProcesso = new BehaviorSubject<number>(1);

  dataInicio: Date;
  dataFim: Date;
  valorInicial: number;
  valorFinal: number;
  txtIdentificacaoLote = new BehaviorSubject<string>(null);
  public onOpen = new Subject();
  public isOpen = new BehaviorSubject<boolean>(false);

  public borderosSubject = new BehaviorSubject<LoteCriacaoBorderoDto[]>([]);

  public listaEmpresaCentralizadoras = new BehaviorSubject<any[]>(null);
  public listaLancamentoLotes = new BehaviorSubject<CriacaoLancamentosModel[]>(null);
  public listaLotes = new BehaviorSubject<any>(null);
  public codigoEmpresaSelecionada = new BehaviorSubject<number>(null);
  public centroCusto = new BehaviorSubject<number>(null);

  public borderoSelecionado = new BehaviorSubject<LoteCriacaoBorderoDto>(null);

  public loteValido = new BehaviorSubject<boolean>(true);


  colunas: [{
    nomeColuna: 'descricaoEscritorio',
    nome: 'Escritório',
    valores: []
  }];
  private getListaLancamento = [{
    codigoProcesso: TipoProcessoEnum.civelConsumidor,
    titulo: ['Escritório', 'Autor', 'N° Processo', 'Valor Líquido', 'Comarca', 'Vara', 'Tipo de Lançamento',
      'Categoria de Pagamento', 'Data Lançamento', 'Texto Sap Identificação do Usuário',
      'Texto Sap', 'Descrição do Erro', 'codigoStatus', 'codigoLancamento', 'codigoProcesso'],
    tituloKey: ['descricaoEscritorio', 'nomeAutor', 'numeroProcesso', 'valorLiquido',
      'descricaoComarca', 'descricaoVara', 'descricaoTipoLancamento',
      'descricaoCategoriaPagamento', 'dataCriacaoLancamento',
      'textoSAPIdentificacaoDoUsuario', 'textoSAP', 'mensagemErro',
      'codigoStatusPagamento', 'codigoLancamento', 'codigoProcesso']
  },
  {
    codigoProcesso: TipoProcessoEnum.civelEstrategico,
    titulo: ['N° Processo', 'Comarca', 'Vara', 'Categoria de Pagamento', 'Data Lançamento',
      'Valor Líquido', 'Texto Sap Identificação do Usuário', 'Texto Sap', 'Descrição do Erro'],
    tituloKey: ['numeroProcesso', 'descricaoComarca', 'descricaoVara',
      'descricaoCategoriaPagamento', 'dataCriacaoLancamento',
      'valorLiquido', 'textoSAPIdentificacaoDoUsuario', 'textoSAP',
      'descrição Erro']
  }, {
    codigoProcesso: TipoProcessoEnum.juizadoEspecial,
    titulo: ['Escritório', 'Autor', 'N° Processo', 'Valor Líquido', 'Comarca', 'Juizado',
      'Categoria de Pagamento', 'N° Guia', 'Data Lançamento',
      'Texto Sap Identificação do Usuário', 'Texto Sap',
      'Descrição do Erro'],
    tituloKey: ['descricaoEscritorio', 'nomeAutor', 'numeroProcesso', 'valorLiquido',
      'descricaoComarca', 'descricaoVara', 'descricaoCategoriaPagamento',
      'numeroGuia', 'dataCriacaoLancamento', 'textoSAPIdentificacaoDoUsuario',
      'textoSAP', 'mensagemErro']
  }, {
    codigoProcesso: TipoProcessoEnum.PEX,
    titulo: ['Escritório', 'N° Processo', 'Valor Líquido', 'Comarca', 'Vara',
      'Tipo de Lançamento', 'Categoria de Pagamento', 'Data Lançamento',
      'Texto Sap Identificação do Usuário', 'Texto Sap', 'Descrição do Erro'],
    tituloKey: ['descricaoEscritorio', 'numeroProcesso', 'valorLiquido',
      'descricaoComarca', 'descricaoVara', 'descricaoTipoLancamento',
      'descricaoCategoriaPagamento', 'dataCriacaoLancamento',
      'textoSAPIdentificacaoDoUsuario', 'textoSAP', 'mensagemErro']
  }, {
    codigoProcesso: TipoProcessoEnum.trabalhista,
    titulo: ['N° Processo', 'Comarca', 'Vara',
      'Categoria de Pagamento', 'N° Guia', 'Data Lançamento',
      'Valor Líquido', 'Texto Sap Identificação do Usuário', 'Texto Sap', 'Descrição Erro'],
    tituloKey: ['numeroProcesso', 'descricaoComarca', 'descricaoVara',
      'descricaoCategoriaPagamento', 'numeroGuia', 'dataCriacaoLancamento',
      'valorLiquido', 'textoSAPIdentificacaoDoUsuario', 'textoSAP',
      'mensagemErro']
  }];
  public borderExcluir;


  public getlistaValoresBordero(): LoteCriacaoBorderoDto[] {
    return this.borderosSubject.getValue();
  }

  public setlistaValoresBordero(v: LoteCriacaoBorderoDto) {
    const borderos = this.borderosSubject.getValue();
    borderos.push(v);
    this.borderosSubject.next(borderos);
  }

  get filtro() : IFiltroCriacaoLote {
    return {
      codigoTipoProcesso: this.tipoProcessoSelecionado,
      dataCriacaoLancamentoInicio: this.dataInicio ? formatDate(this.dataInicio, 'yyyy/MM/dd', 'en_US') : null,
      dataCriacaoLancamentoFim: this.dataFim ? formatDate(this.dataFim, 'yyyy/MM/dd', 'en_US') : null,
      valorLancamentoInicio: this.valorInicial,
      valorLancamentoFim: this.valorFinal
    };
  }

  limparDados() {

    if (!this.manterDados) {
      this.dataInicio = null;
      this.dataFim = null;
      this.valorInicial = null;
      this.valorFinal = null;
    }
    this.manterDados = false;
  }
  getObterLancamentoDoCriacao(json) {
    return this.loteService.obterLancamentoDoCriacao(json)
      .pipe(take(1));
  }

  private openModalSemResultado() {

    const swalButton = Swal.mixin({
      customClass: {
        confirmButton: 'btn btn-primary'
      },
      buttonsStyling: false
    });
    swalButton.fire({
      html:
        'Nenhum resultado foi encontrado',
      showCloseButton: true,
      cancelButtonText:
        'OK '
    });
  }

  getEmpresaCentralizadora() {
    this.loteService.obterLotesAgrupadoPorEmpresaCentralizadora(this.filtro)
      .pipe(take(1))
      .subscribe(response => {
        if (response.sucesso) {
          
          response.data.forEach(item => {item.isEmpty = item.totalLote <= 0});
          this.listaEmpresaCentralizadoras.next(response.data);
          let valor;
          response.data.forEach(item => {item.isEmpty = item.totalLote <= 0
            if (item.codigoEmpresaCentralizadora) {
              valor = true;
            } else {
              valor = false;
            }
          });
          if (valor) {
            this.goToResultado();
          } else {
            this.openModalSemResultado();
          }


        } else {
          this.helperAngular.MsgBox2(
            response.mensagem,
            'Atenção!',
            'warning',
            'OK'
          );
        }
      }
      );
  }

  obterLotesEmpresaGrupo(json: IFiltroEmpresaGrupo) {
    return this.loteService.obterLotesAgrupadoPorEmpresaDoGrupo(json) // TODO: Atualizar p/ DTO
      .pipe(take(1));
  }

  goToResultado() {
    this.router.navigate(['sap/lote/criar/resultado']);
  }
  async excluirBordero(bordero) {
    this.borderExcluir = bordero;




    Swal.fire({
      title: 'Deseja excluir o borderô selecionado?',
      html: 'Excluir Borderô',
      icon: 'question',
      confirmButtonColor: '#6F62B2',
      confirmButtonText: 'Sim',
      cancelButtonText: 'Não',
      cancelButtonColor: '#9597a6',
      showCancelButton: true,
      showConfirmButton: true
    }).then(item => {
      if (item.value) {
        this.confirmeDelete();
      }
    });


  }

  confirmeDelete() {

    const listanovaBordero: any[] = this.getlistaValoresBordero();
    listanovaBordero.forEach((item, index) => {
      if (item === this.borderExcluir) { listanovaBordero.splice(index, 1); }
    });
    this.borderosSubject.next(listanovaBordero);

  }

  criarLote() {
    this.abasService.onCriacaoLote.next(true);
    let listaLancamentos: LoteCriacaoLancamentoDto[] = [];
    this.abasService.lancamentosSelecionados.pipe(take(1)).subscribe(lancamentos => {
      lancamentos.map(item => {

        let lista: LoteCriacaoLancamentoDto = {
         codigoProcesso: item.codigoProcesso,
         codigoLancamento: item.codigoLancamento,
         codigoStatusPagamento: item.codigoStatusPagamento,
         mensagemErro: item.mensagemErro,
         valorLancamento: item.valorLiquido
        }

        item.valorLiquido = 1;

        listaLancamentos= [...listaLancamentos, lista]
      });

      const criarLote: LoteCriacaoGeracaoLoteDto = {
        IdentificacaoLote: this.txtIdentificacaoLote.value,
        ValorLote: this.abasService.totalValorLiquidoSelecionados.value,
        codigoTipoProcesso: this.tipoProcessoSelecionado,
        CodigoParteEmpresa: this.listaLotes.value.codigoEmpresaGrupo,
        CodigoFornecedor: this.listaLotes.value.codigoFornecedor,
        CodigoCentroCusto: this.listaLotes.value.codigoCentroCusto,
        CodigoFormaPagamento: this.listaLotes.value.codigoFormaPagamento,
        CodigoCentroSAP: this.listaLotes.value.codigoCentroSap,
        Borderos: this.borderosSubject.value,
        DadosLancamentoDTOs: listaLancamentos
      };


      this.loteService.criarLote(criarLote);

      this.txtIdentificacaoLote.next(null);

    });
  }


}
