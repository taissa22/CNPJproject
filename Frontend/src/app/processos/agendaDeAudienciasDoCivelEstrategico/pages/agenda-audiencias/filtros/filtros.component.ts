import { AssuntoService } from './../services/assunto.service';
import { isNullOrUndefined } from 'util';
import { FeriadoService } from './../../../../../core/services/feriado.service';
import { Component, OnInit, Output, EventEmitter, Input, AfterContentInit, AfterViewInit } from '@angular/core';
import { ComarcaService } from './../services/comarca.service';
import { EmpresaDoGrupoService } from './../services/empresa-do-grupo.service';
import { EscritorioService } from './../services/escritorio.service';
import { PrepostoService } from './../services/preposto.service';
import { FormControl, Validators, AbstractControl, ValidatorFn, ValidationErrors, FormGroup } from '@angular/forms';
import { Estados } from './../../../../../core/models/estados.model';
import { BsLocaleService, ptBrLocale, defineLocale } from 'ngx-bootstrap';
import { Closings } from '@core/models/closings.model';
import { ClassificacoesHierarquicas } from '@core/models/classificacoes-hierarquicas';

@Component({
  selector: 'app-filtros-agenda-audiencias',
  templateUrl: './filtros.component.html'
})
export class FiltrosAgendaDeAudienciaComponent implements OnInit, AfterContentInit, AfterViewInit {

  empresaDoGrupo: any = null;
  empresasDoGrupoTotal: number;
  empresasDoGrupoBuffer = [];
  carregandoEmpresaDoGrupo = false;

  assuntosTotal: number;
  assuntosBuffer = [];

  carregandoAssunto = false;
  quantidadeRegistrosACarregar = 200;

  dataPrimeiroPeriodoFormControl: FormControl = new FormControl(new Date());
  dataSegundoPeriodoFormControl: FormControl = new FormControl(new Date());
  escritorioFormControl: FormControl = new FormControl(null);
  prepostoFormControl: FormControl = new FormControl(null);
  estadosFormControl: FormControl = new FormControl(null);
  comarcaFormControl: FormControl = new FormControl(null);
  empresaGrupoFormControl: FormControl = new FormControl(null);
  assuntoFormControl: FormControl = new FormControl(null);
  closingFormControl: FormControl = new FormControl(null);
  clientCoFormControl: FormControl = new FormControl(null);
  classificacaoHierarquicaFormControl: FormControl = new FormControl(null);

  form = new FormGroup({
    dataInicial: this.dataPrimeiroPeriodoFormControl,
    dataFinal: this.dataSegundoPeriodoFormControl,
    escritorio: this.escritorioFormControl,
    estado: this.estadosFormControl,
    comarca: this.comarcaFormControl,
    empresaDoGrupo: this.empresaGrupoFormControl,
    preposto: this.prepostoFormControl,
    assunto: this.assuntoFormControl,
    closing: this.closingFormControl,
    clientCo: this.clientCoFormControl,
    classificacaoHierarquica: this.classificacaoHierarquicaFormControl
  });

  estados: Array<any> = Estados.obterUfs().map(e => ({ id: e.id, nome: e.descricao }));
  closings: Array<any> = Closings.obterClosings().map(e => ({ id: e.id, nome: e.nome }));
  classificacoes: Array<any> = ClassificacoesHierarquicas.obterClassicacoes().map(e => ({ id: e.id, nome: e.nome }));
  @Input() escritorios: Array<any> = [];
  @Input() prepostos: Array<any> = [];
  comarcas: Array<any> = [];
  empresasDoGrupo: Array<any> = [];
  assuntos: Array<any> = [];
  numeroMaximoDias = 365;


  hoje: Date = new Date();

  @Output() obterParametros: EventEmitter<{
    dataInicial: Date,
    dataFinal: Date,
    escritorioId: number,
    estadoId: string,
    comarcaId: number,
    empresaGrupoId: number,
    prepostoId: number,
    assuntoId: number,
    classificacaoProcessoId: string,
    closing: string
    clientCo: string
  }> = new EventEmitter();

  constructor(
    private empresasDoGrupoService: EmpresaDoGrupoService,
    private comarcasService: ComarcaService,
    private configLocalizacao: BsLocaleService,
    private feriadosService: FeriadoService,
    private assuntoService: AssuntoService
  ) {
    this.configLocalizacao.use('pt-br');
    ptBrLocale.invalidDate = 'Data Invalida';
    defineLocale('pt-br', ptBrLocale);
  }

  ngOnInit() {
    this.comarcaFormControl.disable();
    this.obterEmpresasDoGrupo();

  }

  ngAfterViewInit() {
    const proximoDiaUtil = new Date(this.feriadosService.obterProximoDiaUtil(this.hoje));

    this.dataPrimeiroPeriodoFormControl.setValidators([Validators.required, this.validaData(this.numeroMaximoDias)]);
    this.dataSegundoPeriodoFormControl.setValidators([Validators.required, this.validaData(this.numeroMaximoDias)]);

    const currentDate = new Date();
    this.dataPrimeiroPeriodoFormControl.setValue(currentDate);
    this.dataSegundoPeriodoFormControl.setValue(proximoDiaUtil);

    if (this.escritorios.length === 1) {
      const escritorio = this.escritorios[0];
      this.escritorioFormControl.setValue(escritorio.id);
    }
  }

  ngAfterContentInit() {
    this.obterAssuntos({});
  }

  obterEmpresasDoGrupo(): void {
    this.empresasDoGrupoService.obter().then((e) => {
      this.empresasDoGrupo = e.map(r => ({ id: r.id, nome: r.nome.toUpperCase() }));
    });
  }



  async obterComarcas(estado: any): Promise<void> {
    if (isNullOrUndefined(estado)) {
      return;
    }

    try {
      const comarcas = await this.comarcasService.obterPorEstado(estado.id);
      this.comarcas = comarcas.map(c => ({ id: c.id, nome: c.nome }));
      this.comarcaFormControl.enable();
    } catch (error) {
      console.error(error);
    }
  }

  obterValoresDoFiltro() {
    this.form.markAllAsTouched();
    if (this.form.invalid) {
      return false;
    }

    this.obterParametros.emit({
      dataInicial: this.dataPrimeiroPeriodoFormControl.value,
      dataFinal: this.dataSegundoPeriodoFormControl.value,
      escritorioId: this.escritorioFormControl.value,
      estadoId: this.estadosFormControl.value,
      comarcaId: this.comarcaFormControl.value,
      empresaGrupoId: this.empresaGrupoFormControl.value,
      prepostoId: this.prepostoFormControl.value,
      assuntoId: this.assuntoFormControl.value,
      classificacaoProcessoId: this.classificacaoHierarquicaFormControl.value,
      closing: this.closingFormControl.value,
      clientCo: this.clientCoFormControl.value,
    });
  }

  async obterAssuntos({ end = 0 }): Promise<void> {
    // tslint:disable-next-line: curly
    if (this.carregandoEmpresaDoGrupo || this.assuntosTotal === this.assuntosBuffer.length || end < this.assuntosBuffer.length) return;

    this.carregandoAssunto = true;
    const paginaAtual = Math.floor(this.assuntosBuffer.length / this.quantidadeRegistrosACarregar) + 1;
    try {
      const { data, total }  = await this.assuntoService.obterPaginado(paginaAtual, this.quantidadeRegistrosACarregar, 0);

      this.assuntosTotal = total;
      this.carregandoAssunto = false;

      this.assuntosBuffer = [
        ...this.assuntosBuffer,
        ...data.map(({id, descricao }) => ({ id, descricao }))
      ];
    } catch (error) {
      console.error(error);
    }
  }

  private removerDias(data: Date, dias: number) {
    const dataCalculada = new Date(data.valueOf());
    dataCalculada.setDate(dataCalculada.getDate() - dias);
    return dataCalculada;
  }

  desabilitaTooltip(formControl: FormControl) {
    return formControl.untouched || formControl.valid;
  }

  onScrollToEnd() {
    this.carregarMais();
  }

  async onScroll({ end }) {
    this.carregarMais(end);
  }


  async carregarMais(end: number = 0) {
    // tslint:disable-next-line: max-line-length
    if (this.carregandoEmpresaDoGrupo || this.empresasDoGrupoTotal === this.empresasDoGrupoBuffer.length || end < this.empresasDoGrupoBuffer.length) {
      return;
    }
    this.carregandoEmpresaDoGrupo = true;
    // tslint:disable-next-line: max-line-length
    const maisEmpresasDoGrupo = await this.empresasDoGrupoService.obterPaginado(
      Math.floor(this.empresasDoGrupoBuffer.length / this.quantidadeRegistrosACarregar) + 1,
      this.quantidadeRegistrosACarregar, null);
    this.carregandoEmpresaDoGrupo = false;
    this.empresasDoGrupoBuffer = [...this.empresasDoGrupoBuffer, ...maisEmpresasDoGrupo.data.filter((item) => {
      return (item.id !== this.empresaDoGrupo);
    })];

    this.empresasDoGrupoBuffer.sort((a, b) => {
      return a.nome.localeCompare(b.nome);
    });
  }

  validaData(maximoDeDias: number) {
    return (): Validators => {
      if (this.dataSegundoPeriodoFormControl === null || this.dataPrimeiroPeriodoFormControl === null) {
        return null;
      }

      let primeiraData = new Date(this.dataPrimeiroPeriodoFormControl.value);
      primeiraData = new Date(primeiraData.getFullYear(), primeiraData.getMonth(), primeiraData.getDate());
      let segundaData = new Date(this.dataSegundoPeriodoFormControl.value);
      segundaData = new Date(segundaData.getFullYear(), segundaData.getMonth(), segundaData.getDate());

      if (primeiraData > segundaData) {
        return { primeiraDataMaior: true };
      }

      if ((segundaData.getTime() - primeiraData.getTime() ) / (1000 * 3600 * 24) > maximoDeDias) {
        return { maxDiasExcedido: true };
      }

      this.dataPrimeiroPeriodoFormControl.setErrors(null);
      this.dataSegundoPeriodoFormControl.setErrors(null);

      return null;
    };
  }


}

