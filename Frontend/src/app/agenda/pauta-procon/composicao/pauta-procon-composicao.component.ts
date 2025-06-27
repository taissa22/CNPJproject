import { environment } from 'src/environments/environment';
import { AfterViewInit, ChangeDetectorRef, Component, OnInit, ViewChild } from "@angular/core";
import { Page } from "@shared/types/page";
import { BsLocaleService, ptBrLocale, defineLocale } from 'ngx-bootstrap';
import { DialogService } from '@shared/services/dialog.service';
import { EMPTY } from 'rxjs';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { FormControl } from "@angular/forms";
import { itemSelecaoPicklist } from "@shared/components/jur-picklist/models/itemSelecaoPicklist";
import { JurPicklistComponent } from "@shared/components/jur-picklist/jur-picklist.component";
import { ActivatedRoute, Router } from '@angular/router';
import { PautaProconComposicaoService } from '../../services/pauta-procon-composicao.service';
import { PautaProconComposicaoCommandModel } from '../../models/pauta-procon-composicao-command.model';
import { PautaProconComposicaoLista } from '../../models/pauta-procon-composicao-lista';
import { PautaProconComposicaoListaAudiencia } from '../../models/pauta-procon-composicao-lista-audiencia';
import { PautaProconComposicaoListaPreposto } from '../../models/pauta-procon-composicao-lista-preposto';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from 'src/app/permissoes/permissoes';
import { PautaProconPesquisaModel } from '../../models/pauta-procon-pesquisa.model';
import { PautaProconPesquisaComponent } from '../pesquisa/pauta-procon-pesquisa.component';
import { PautaProconPesquisaService } from '../../services/pauta-procon-pesquisa.service';
import { LocalStorageService } from '@core/services/local-storage.service';

@Component({
  selector: 'pauta-procon-composicao',
  templateUrl: './pauta-procon-composicao.component.html',
  styleUrls: ['./pauta-procon-composicao.component.scss']
})
export class PautaProconComposicaoComponent implements OnInit, AfterViewInit {


  opcoesAlocacao: { id: number; nome: string }[] = [
    { id: 2, nome: 'Preposto Próprio' },
    { id: 3, nome: 'Terceirizado Escritório' }
  ];



  public opcaoAlocaoTodasPaginas: FormControl = new FormControl(null);
  public opcaoAlocaoTodosGrid: FormControl = new FormControl(null);


  //currentUrl = 'http://sisjurprocon';
  currentUrl = 'http://' + window.location.host.replace('juridico', 'procon');
  numeroMaximoPrepostosSelecionados: number = 10;
  picklistAltura: number = 270;
  colunas: itemSelecaoPicklist[] = [];
  colunasSelecionadas: itemSelecaoPicklist[] = [];
  apenasPrepostoNaoAlocadoNaData: FormControl = new FormControl(true);
  urlPorProcon = this.route.snapshot.paramMap.get('porJui');
  urlPeriodoInicio = this.route.snapshot.paramMap.get('dtIni');
  urlPeriodoFim = this.route.snapshot.paramMap.get('dtFim');
  urlTipoAudiencia = this.route.snapshot.paramMap.get('tpAud');
  urlEmpresaGrupo = this.route.snapshot.paramMap.get('empGrp');
  urlEstado = this.route.snapshot.paramMap.get('UF');
  urlComarca = this.route.snapshot.paramMap.get('com');
  urlJuizado = this.route.snapshot.paramMap.get('jui');
  urlSituacaoProcesso = this.route.snapshot.paramMap.get('sitProc');
  urlAudienciaSemPreposto = this.route.snapshot.paramMap.get('audSemPrep');
  urlEmpresaCentralizadora = this.route.snapshot.paramMap.get('empCent');
  urlGrupoProcon = this.route.snapshot.paramMap.get('grpJui');
  urlPreposto = this.route.snapshot.paramMap.get('prep');
  pautaPageSize = 1;
  pautaTotal: number = 0;
  pautaDataAudiencia: string;
  pautaJuizado: string;
  pautaVara: number;
  pautaTipoVara: number;
  pautaComarca: number;
  pautaParteEmpresa: string = "";
  pautaCodEstado: string = "";
  pautaQtdAudiencia: number = 0;
  total: number = 0;
  dadosAudiencia: Array<any> = [];
  breadcrumb: string;

  //#region alteração
   teveAlteracao: boolean;
   terceirizadasIniciais = [];
   pageCurrent = 0
   //#endregion

   historicoPesquisa: PautaProconPesquisaModel;

  constructor(
    private configLocalizacao: BsLocaleService,
    private servicePautaComposicao: PautaProconComposicaoService,
    private pautaProconPesquisaService: PautaProconPesquisaService,private dialog: DialogService,
    private changeDetector: ChangeDetectorRef,
    private route: ActivatedRoute,
    private router: Router,
    private breadcrumbsService: BreadcrumbsService
  ) {
    this.configLocalizacao.use('pt-br');
    ptBrLocale.invalidDate = 'Data Invalida';
    defineLocale('pt-br', ptBrLocale);
  }

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginatorPauta: SisjurPaginator;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;
  @ViewChild('colunasSelect', { static: true }) colunasSelect: JurPicklistComponent;

  async ngOnInit() {
    this.historicoPesquisa = this.pautaProconPesquisaService.rescue();
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_COMPOSICAO_PROCON);
  }

  ngAfterViewInit() {
    this.paginatorPauta.pageSize = this.pautaPageSize;
  }

  listarPauta(): void {
    this.servicePautaComposicao.obterPauta(this.listarPautaPreencheDados()).subscribe(
      res => {
        if (res.data && res.data.length) {

          this.pautaTotal = res.total;

          res.data.map(q => {
            this.pautaDataAudiencia = q.data;
            this.pautaJuizado = q.juizado;
            this.pautaVara = q.codVara;
            this.pautaTipoVara = q.codTipoVara;
            this.pautaComarca = q.codComarca;
            this.pautaCodEstado = q.codEstado
          });
          this.listarPautaAudiencia();
          this.pageCurrent = this.iniciaValoresPautaComposicao().pageIndex
        } else {
          this.voltar(false)
          this.dialog.alert('Nenhum registro foi encontrado');
        }
      },
      error => {
        this.dialog.err(
          'Informações não carregadas',
          'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte'
        );
        return EMPTY;
      }
    );
    this.atualizaTodosTerceirizado();
  }


  atualizaTodosTerceirizado() {

    this.dadosAudiencia.find(a => a.alocacaoTipo != 3) == undefined
      ? (this.colunasSelect.todosTerceirizados = true)
      : (this.colunasSelect.todosTerceirizados = false);

     this.audienciasTerceirizadas(this.colunasSelect.itensSelecionados.length > 0)
  }



  listarPautaAudiencia(): void {
    //this.opcaoAlocaoTodosGrid.setValue(null);
    this.servicePautaComposicao.obterPautaAudiencia(this.listarPautaAudienciaPreencheDados()).subscribe(
      data => {
        this.dadosAudiencia = data.data;
        this.total = data.total;
        this.pautaParteEmpresa = this.dadosAudiencia.map(e => e.codParte).join(',');
      },
      error => {
        this.dialog.err(
          'Informações não carregadas',
          'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte'
        );
        return EMPTY;
      },
      ()=> {
       this.listarPrepostosAlocados();
       this.listarPrepostosNaoAlocados();

       this.dadosAudiencia.find(a => a.alocacaoTipo != 3) == undefined
       ? (this.colunasSelect.todosTerceirizados = true)
       : (this.colunasSelect.todosTerceirizados = false);

      }
    );
  }

  listarPrepostosAlocados(): void {
    this.servicePautaComposicao.obterPrepostosAlocados(this.listarPrepostosPreencheDados()).subscribe(res => {
      this.colunasSelect.itensSelecionados = [];
      if (res.data && res.data.length) {
        res.data.map(q => {
          this.colunasSelect.itensSelecionados.push(new itemSelecaoPicklist(q.id.toString(), q.preposto, q.principal));
        });
      }
    });
  }

  listarPrepostosNaoAlocados(): void {
    this.servicePautaComposicao.obterPrepostosNaoAlocados(this.listarPrepostosPreencheDados()).subscribe(
      res => {
      this.colunas = [];

      if (res.data && res.data.length) {
        res.data.map(q => {
          this.colunas.push(new itemSelecaoPicklist(q.id.toString(), q.preposto, false));
        });

        this.changeDetector.detectChanges();
      }
     // this.iniciarTerceirizadas(this.dadosAudiencia)
     // this.iniciarMarcarTodas(this.dadosAudiencia)
    });
  }

  async salvar() {
    if (!this.salvarValidacao()) return false;

    try {
      let salvaDadosPauta = this.salvarPreencheDados();

      await this.servicePautaComposicao.salvarAudiencia(this.dadosAudiencia).then();
      await this.servicePautaComposicao.salvarPautaComposicao(salvaDadosPauta).then();
      await this.dialog.alert('Composição de pauta efetuada com sucesso!');

      this.listarPauta();
    } catch (error) {
      await this.dialog.err(error.error);
    }
  }

  salvarValidacao(): boolean {
    if (this.colunasSelect.itensSelecionados.length > 0 && this.colunasSelect.itensSelecionados.filter(s => s.principal == true).length == 0) {
      this.dialog.err('Por favor marque o preposto alocado principal!');
      return false;
    }

    if (this.colunasSelect.itensSelecionados.length > 10) {
      this.dialog.err(
        'Limite máximo de prepostos alocados são ' + this.numeroMaximoPrepostosSelecionados
      );
      return false;
    }

    return true;
  }

  salvarPreencheDados(): PautaProconComposicaoCommandModel {
    let salvaPautaComposicaoModel: PautaProconComposicaoCommandModel = new PautaProconComposicaoCommandModel();
    salvaPautaComposicaoModel.porProcon= this.urlPorProcon;
    salvaPautaComposicaoModel.codParteEmpresa = this.pautaParteEmpresa;
    salvaPautaComposicaoModel.codComarca = this.pautaComarca;
    salvaPautaComposicaoModel.codTipoVara = this.pautaTipoVara;
    salvaPautaComposicaoModel.codVara = this.pautaVara;
    salvaPautaComposicaoModel.dataAudiencia = this.pautaDataAudiencia;
    salvaPautaComposicaoModel.codGrupoProcon = Number(this.urlGrupoProcon);
    salvaPautaComposicaoModel.codPreposto = this.colunasSelect.itensSelecionados.length == 0 ? [] : this.colunasSelect.itensSelecionados.map(e => parseInt(e.id));
    salvaPautaComposicaoModel.codPrepostoPrincipal = this.colunasSelect.itensSelecionados.length == 0 ? 0 : Number(this.colunasSelect.itensSelecionados.find(f => f.principal).id);
    return salvaPautaComposicaoModel;
  }

  listarPautaPreencheDados(): PautaProconComposicaoLista {
    const page: Page = { index: this.iniciaValoresPautaComposicao().pageIndex, size: this.iniciaValoresPautaComposicao().pageSize };

    let urlVara = "0";
    let urlTipoVara = "0";

    if (this.urlJuizado != "0") {
      let juizado = this.urlJuizado.split(",");
      urlVara = juizado[0];
      urlTipoVara = juizado[1];
    }

    let lstPauta: PautaProconComposicaoLista = new PautaProconComposicaoLista();

    lstPauta.numeroPagina = page.index;
    lstPauta.quantidadePorPagina = page.size;
    lstPauta.porProcon = this.urlPorProcon;
    lstPauta.periodoInicio = this.urlPeriodoInicio;
    lstPauta.periodoFim = this.urlPeriodoFim;
    lstPauta.situacaoProcesso = this.urlSituacaoProcesso == "true" ? "S" : "N";
    lstPauta.codEstado = this.urlEstado;
    lstPauta.codTipoAudiencia = Number(this.urlTipoAudiencia);
    lstPauta.codComarca = Number(this.urlComarca);
    lstPauta.codVara = Number(urlVara);
    lstPauta.codTipoVara = Number(urlTipoVara);
    lstPauta.codEmpresaGrupo = Number(this.urlEmpresaGrupo);
    lstPauta.codEmpresaCentralizadora = Number(this.urlEmpresaCentralizadora);
    lstPauta.audienciaSemPreposto = this.urlAudienciaSemPreposto;
    lstPauta.codGrupoProcon = Number(this.urlGrupoProcon);
    lstPauta.prepostoAlocado = this.urlPreposto;

    return lstPauta;
  }

  listarPautaAudienciaPreencheDados(): PautaProconComposicaoListaAudiencia {
    let lstAudiencia: PautaProconComposicaoListaAudiencia = new PautaProconComposicaoListaAudiencia();
    lstAudiencia.coluna = this.iniciaValoresDaView().sortColumn;
    lstAudiencia.direcao = this.iniciaValoresDaView().sortDirection;
    lstAudiencia.dataAudiencia = this.pautaDataAudiencia;
    lstAudiencia.codComarca = this.pautaComarca
    lstAudiencia.codVara = this.pautaVara;
    lstAudiencia.codTipoVara = this.pautaTipoVara;
    lstAudiencia.situacaoProcesso = this.urlSituacaoProcesso == "true" ? "S" : "N";
    return lstAudiencia;
  }

  listarPrepostosPreencheDados(): PautaProconComposicaoListaPreposto {
    let lstPreposto: PautaProconComposicaoListaPreposto = new PautaProconComposicaoListaPreposto();
    lstPreposto.prepUFJuizado = false;
    lstPreposto.prepNaoAlocadoJuizado = (<HTMLInputElement>document.getElementById("apenasPrepostoNaoAlocadoNaData")).checked;
    lstPreposto.porProcon = this.urlPorProcon;
    lstPreposto.dataAudiencia = this.pautaDataAudiencia;
    lstPreposto.codParteEmpresa = this.pautaParteEmpresa;
    lstPreposto.codEstado = this.pautaCodEstado;
    lstPreposto.codComarca = this.pautaComarca
    lstPreposto.codVara = this.pautaVara;
    lstPreposto.codTipoVara = this.pautaTipoVara;
    lstPreposto.codGrupoProcon = Number(this.urlGrupoProcon);
    return lstPreposto;
  }

  iniciaValoresPautaComposicao() {
    return {
      pageIndex: this.paginatorPauta === undefined ? 1 : this.paginatorPauta.pageIndex + 1,
      pageSize: this.pautaPageSize,
    }
  }

  iniciaValoresDaView() {
    return {
      sortColumn: this.table === undefined || !this.table.sortColumn ? "nome" : this.table.sortColumn,
      sortDirection: this.table === undefined || !this.table.sortDirection ? "asc" : this.table.sortDirection,
    }
  }

  // iniciarMarcarTodas(dados) {
  //   let marcarTodas = (<HTMLInputElement>document.getElementById('marcarTodas'))
  //   marcarTodas.checked = false;
  //   let todasTerceirizada = true;
  //   this.colunasSelect.todosTerceirizados = false
  //   for (let i = 0; i < dados.length; i++) {
  //     if (dados[i].terceirizado == "N") return todasTerceirizada = false;
  //   }
  //   if (todasTerceirizada) {
  //     marcarTodas.checked = true;
  //     this.colunasSelect.todosTerceirizados = true
  //     this.marcarTodas()
  //   }
  // }


  iniciarMarcarTodas(dados) {
    // let marcarTodas = (<HTMLInputElement>document.getElementById('marcarTodas'))
    // marcarTodas.checked = false;
     let todasTerceirizada = true;
     this.colunasSelect.todosTerceirizados = false
     for (let i = 0; i < dados.length; i++) {
       if (dados[i].terceirizado == "N") return todasTerceirizada = false;
     }
     if (todasTerceirizada) {
       //marcarTodas.checked = true;
       this.colunasSelect.todosTerceirizados = true
       this.marcarTodas()
     }
   }



  // marcarTodas() {
  //   let marcarTodas = (<HTMLInputElement>document.getElementById('marcarTodas'))
  //   this.colunasSelect.todosTerceirizados = false

  //   if (marcarTodas.checked) {
  //     let marcarTerceirizada = document.querySelectorAll("input[name='terceirizada']:not(:checked)")
  //     let terceirizada = (<HTMLInputElement><unknown>document.querySelectorAll("input[name='terceirizada']:not(:checked)"))
  //     for (let i = 0; i < marcarTerceirizada.length; i++) {
  //       terceirizada[i].checked = true
  //     }
  //     this.colunasSelect.todosTerceirizados = true
  //     this.dadosAudiencia.map(t => t.terceirizado = 'S')
  //     this.audienciasTerceirizadas(this.colunasSelect.itensSelecionados.length > 0)
  //   }
  //   else {
  //     let marcarTerceirizada = document.querySelectorAll("input[name='terceirizada']:checked")
  //     let terceirizada = (<HTMLInputElement><unknown>document.querySelectorAll("input[name='terceirizada']:checked"))

  //     for (let i = 0; i < marcarTerceirizada.length; i++) {
  //       terceirizada[i].checked = false
  //     }
  //     this.colunasSelect.todosTerceirizados = false
  //     this.dadosAudiencia.map(t => t.terceirizado = 'N')
  //   }
  // }

  // desmarcarTodas(id, index, seq) {
  //   let marcarTodas = (<HTMLInputElement>document.getElementById('marcarTodas'))
  //   let marcarTerceirizado = this.dadosAudiencia.find(d => d.codProcesso == id).terceirizado
  //   this.dadosAudiencia.find(d => d.codProcesso == id).terceirizado = marcarTerceirizado == 'S' ? 'N' : 'S'
  //   let terceirizada = this.dadosAudiencia.find(t => t.terceirizado == 'N')
  //   marcarTodas.checked = terceirizada == undefined
  //   marcarTodas.checked ? this.marcarTodas() : this.colunasSelect.todosTerceirizados = false
  //   this.atualizarTerceirizadas(id, index, seq);
  //   this.audienciasTerceirizadas(this.colunasSelect.itensSelecionados.length > 0)
  // }



  marcarTodas() {
    this.dadosAudiencia.forEach(
      i => (i.alocacaoTipo = this.opcaoAlocaoTodosGrid.value)
    );
    this.atualizaTodosTerceirizado();

    //let marcarTodas = (<HTMLInputElement>document.getElementById('marcarTodas'))
   // this.colunasSelect.todosTerceirizados = false

    if (this.opcaoAlocaoTodosGrid.value != 2 ) {
     // let marcarTerceirizada = document.querySelectorAll("input[name='terceirizada']:not(:checked)")
     // let terceirizada = (<HTMLInputElement><unknown>document.querySelectorAll("input[name='terceirizada']:not(:checked)"))
    //  for (let i = 0; i < marcarTerceirizada.length; i++) {
        //terceirizada[i].checked = true
     // }
      this.colunasSelect.todosTerceirizados = true
      this.dadosAudiencia.map(t => t.terceirizado = 'S')
      this.audienciasTerceirizadas(this.colunasSelect.itensSelecionados.length > 0)
    }
    else {
     // let marcarTerceirizada = document.querySelectorAll("input[name='terceirizada']:checked")
      //let terceirizada = (<HTMLInputElement><unknown>document.querySelectorAll("input[name='terceirizada']:checked"))

    //  for (let i = 0; i < marcarTerceirizada.length; i++) {
     ///   terceirizada[i].checked = false
      //}
      this.colunasSelect.todosTerceirizados = false
      this.dadosAudiencia.map(t => t.terceirizado = 'N')
    }
  }


  desmarcarTodas(id, index, seq) {
    //let marcarTodas = <HTMLInputElement>document.getElementById('marcarTodas');
    let marcarTerceirizado = this.dadosAudiencia.find(d => d.codProcesso == id ).terceirizado;
    this.dadosAudiencia.find(d => d.codProcesso == id).terceirizado =    marcarTerceirizado == 'S' ? 'N' : 'S';
    let terceirizada = this.dadosAudiencia.find(t => t.terceirizado == 'N');
    //marcarTodas.checked = terceirizada == undefined;
    this.opcaoAlocaoTodosGrid.value != 2   ? this.marcarTodas()      : (this.colunasSelect.todosTerceirizados = false);
    this.atualizarTerceirizadas(id, index, seq);
    this.audienciasTerceirizadas(this.colunasSelect.itensSelecionados.length > 0);
  }


  audienciasTerceirizadas(alocado?: boolean) {
    if (alocado && this.colunasSelect.todosTerceirizados) {
      this.dialog.info('Só existem audiências terceirizadas neste juizado, nesta data. Assim, os prepostos indicados serão automaticamente desalocados.').then(res => {
        this.colunasSelect.limparItensSelecionados()
      });
    }
  }

  iniciarTerceirizadas(dados) {
    this.terceirizadasIniciais = []
    for (let i = 0; i < dados.length; i++) {
      this.terceirizadasIniciais.push(
        {
          id: dados[i].codProcesso,
          terceirizado: dados[i].terceirizado,
          seqAudiencia: dados[i].seqAudiencia
        }
      )
    }
  }

  atualizarTerceirizadas(id, index, seq) {
    let terceirizada = document.querySelectorAll(`input[id='terceirizado${index}']:checked`).length > 0 ? 'S' : 'N'
    this.dadosAudiencia.find(s => s.codProcesso == id && s.seqAudiencia == seq).terceirizado = terceirizada
  }

  validarTerceirizados(): boolean {
    let alterado = []
    for (let i = 0; i < this.dadosAudiencia.length; i++) {
      let audiencia = this.dadosAudiencia.find(s => s.codProcesso == this.terceirizadasIniciais[i].id && s.seqAudiencia == this.terceirizadasIniciais[i].seqAudiencia);
      audiencia.terceirizado != this.terceirizadasIniciais[i].terceirizado ? alterado.push(true) : alterado.push(false)
    }
    return alterado.find(a => true)
  }

  async salvarAvancar() {
    this.salvarValidacao();
    await this.salvar();
    this.paginatorPauta.pageIndex = this.pageCurrent;
  }

  voltar(novaPesquisa : boolean) {
    this.router.navigate(["/agenda/agenda-audiencia/pauta-procon/pesquisa"]);
    novaPesquisa ? this.pautaProconPesquisaService.guard(null) : this.pautaProconPesquisaService.guard(this.historicoPesquisa);
  }







  async alterarTodasPaginas() {

    let mensagem : string;
    mensagem =  'Deseja realmente marcar todas as audiências de todas as páginas apresentadas (conforme o filtro de pesquisa selecionado na tela anterior) como ' +
    this.opcoesAlocacao.find(
      e => e.id == this.opcaoAlocaoTodasPaginas.value
    ).nome + '?';

    if (this.opcaoAlocaoTodasPaginas.value != 2){
     mensagem = mensagem + ' Se existirem alocações de prepostos próprios em audiências, estas serão desfeitas.'
    }

     const Ok: boolean = await this.dialog.confirm(mensagem);


    if (Ok) {
      this.dadosAudiencia.forEach(item => item.alocacaoTipo = this.opcaoAlocaoTodasPaginas.value);

      if (this.opcaoAlocaoTodasPaginas.value == 3) {
        this.colunasSelect.limparItensSelecionados();
      }

      await this.servicePautaComposicao.alterarTodasPaginas(this.opcaoAlocaoTodasPaginas.value,  this.listarPautaPreencheDados() );
      this.dadosAudiencia.forEach(item => item.alocacaoTipo = this.opcaoAlocaoTodasPaginas.value);

      this.dialog.info("Ok, audiências de todas as páginas atualizadas!");
      this.dadosAudiencia.forEach(item => item.alocacaoTipo = this.opcaoAlocaoTodasPaginas.value);
      this.opcaoAlocaoTodasPaginas.setValue(null);
      this.opcaoAlocaoTodosGrid.setValue(null);
      this.salvar();


    }



  }

  desfazerAlteracaoTodasPaginas() {
    this.opcaoAlocaoTodasPaginas.setValue(null);
  }




}
