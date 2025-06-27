import { DatePipe } from '@angular/common';
import { AfterViewInit, Component, ElementRef, OnInit, QueryList, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { environment } from '@environment';
import { ErrorLib } from '@esocial/libs/error-lib';
import { NgSelectComponent } from '@ng-select/ng-select';
import { Permissoes, PermissoesService } from '@permissoes';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { DialogService } from '@shared/services/dialog.service';
import { AgendaAudienciaListaResponse } from 'src/app/agenda/models/agenda-audiencia-lista-response';
import { AgendaAudienciaPrepostoRequest } from 'src/app/agenda/models/agenda-audiencia-preposto-request';
import { AgendaAudienciaTrabalhistaService } from 'src/app/agenda/services/agenda-audiencia-trabalhista.service';

@Component({
  selector: 'app-agenda-audiencia-trabalhista',
  templateUrl: './agenda-audiencia-trabalhista.component.html',
  styleUrls: ['./agenda-audiencia-trabalhista.component.scss']
})
export class AgendaAudienciaTrabalhistaComponent implements OnInit, AfterViewInit  {

  @ViewChild('dataRange', {read: ElementRef, static: false}) dataRange: ElementRef;
  @ViewChild(NgSelectComponent, { static: true }) ngSelect: NgSelectComponent;

  constructor(
    private service : AgendaAudienciaTrabalhistaService,
    private breadcrumbsService: BreadcrumbsService,
    private dialog : DialogService,
    private elementRef: ElementRef,
    private permissaoService: PermissoesService,
    private datePipe: DatePipe
  ) { }
  
  async ngOnInit() {
    this.caminhoPaginaAgendaAudiencia = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_AGENDA_AUDIENCIA_TRABALHISTA);
    this.temPermissaoParaAlterarConsultarProcesso = this.permissaoService.temPermissaoPara(Permissoes.ACESSAR_ALTERAR_CONSULTAR_PROCESSO_TRABALHISTA);
    await this.obterOrdenarPorList();
    await this.definirDataPadrao();
    await this.verificaUsuarioAssociado();
    await this.obterEstadoLista();
    await this.obterTipoAudienciaLista();
    await this.obterModalidaeLista();
    await this.obterLocalidadeLista();
    await this.buscar(false, true);
  }
  
 async ngAfterViewInit() {
    this.disableTabIndex(this.dataRange.nativeElement);
    await this.obterPrepostoLista();
    
  }

//#region Variaveis

  caminhoPaginaAgendaAudiencia : string
  temPermissaoParaAlterarConsultarProcesso : boolean;
  estadoCustomList = [];
  estadoLista = [];
  tipoAudienciaLista = [];
  prepostoLista = [];
  ehUsuarioAssociado = false;
  ordenarPor = [];
  modalidadeLista = [];
  localidadeLista = [];

  audienciasLista: Array<AgendaAudienciaListaResponse>;
  audienciasListaOriginal: Array<AgendaAudienciaListaResponse>;
  audienciasModificadas: Array<AgendaAudienciaPrepostoRequest> = [];
  totalAudiencias : number = 0;
  totalGeralAudiencia : number = 0;
  totalAudienciasRestante : number;
  paginaCarregarMais = 1;

  ultimoPrepostoSelecionado = null;

//#endregion

//#region FORM CONTROL

  estadoCustomFormControl: FormControl = new FormControl(null);
  dataInicialFormControl: FormControl = new FormControl(null);
  dataFinalFormControl: FormControl = new FormControl(null);

  indUnicoFormControl: FormControl = new FormControl(false);
  indPrimarioFormControl: FormControl = new FormControl(false);
  indSecundarioFormControl: FormControl = new FormControl(false);
  indProprioFormControl: FormControl = new FormControl(true);
  indTerceiroFormControl: FormControl = new FormControl(true);

  estadoListaFormControl: FormControl = new FormControl(null);
  tipoAudienciaListaFormControl: FormControl = new FormControl(null);
  processoEstrategicoFormControl: FormControl = new FormControl('3');

  ordenarPorFormControl: FormControl = new FormControl(0);
  permitirInativosFormControl: FormControl = new FormControl(false);
  modalidadeListaFormControl: FormControl = new FormControl(null);
  localidadeListaFormControl: FormControl = new FormControl(null);

  formGroup: FormGroup = new FormGroup({
    dataAudienciaDe: this.dataInicialFormControl,
    dataAudienciaAte: this.dataFinalFormControl,
    classificHierarUnico: this.indUnicoFormControl,
    classificHierarPrimario: this.indPrimarioFormControl,
    classificHierarSecundario: this.indSecundarioFormControl,
    classificProcessoPrimario: this.indProprioFormControl,
    classificProcessoTerceiro: this.indTerceiroFormControl,
    estado: this.estadoListaFormControl,
    tipoAudiencia: this.tipoAudienciaListaFormControl,
    processoEstrategico: this.processoEstrategicoFormControl,
    estadoSelecionado : this.estadoCustomFormControl,
    LocalidadeAudiencia: this.localidadeListaFormControl,
    ModalidadeAudiencia: this.modalidadeListaFormControl
  });

//#endregion

//#region Services

  async obterEstadoCustomList(){
    this.estadoCustomList = [
      {id: 0, descricao: 'RJ - Rio de Janeiro'},
      {id: 1, descricao: 'SP - São Paulo'},
      {id: 2, descricao: 'BA - Bahia'},
    ]
  }
  
  async obterOrdenarPorList(){
    this.ordenarPor = [
      {id: 0, descricao: 'Data/Hora da Audiencia'},
      {id: 1, descricao: 'Data/Comarca/Vara/Hora'}
    ]
  }

  async verificaUsuarioAssociado() {
    this.ehUsuarioAssociado = await this.service.obterUsuarioAssociadoAsync();
    this.marcarOpcoesPadrao();
  }

  async obterEstadoLista(){
    this.service.obterEstadoListaAsync().then(x => {
      this.estadoLista = x
    });
  }

  async obterTipoAudienciaLista(){
    this.service.obterTipoAudienciaListaAsync().then(x => {
      this.tipoAudienciaLista = x
    });
  }
  
  async obterPrepostoLista(){
    var retorno = await this.service.obterPrepostoListaAsync(this.formGroup.value, this.permitirInativosFormControl.value)
    this.prepostoLista = retorno;
    // if (this.prepostoLista && !this.permitirInativosFormControl.value) {
    //   for (let index = 0; index < this.prepostoLista.length; index++) {
    //     const element = this.prepostoLista[index];
    //     if (element.descricao.includes("[INATIVO]")) {
    //       element.disabled = true;
    //     }
        
    //   }
    // }
  }

  async buscar(carregarMais?: boolean, zerarEstado?: boolean) {
    this.paginaCarregarMais = carregarMais ? this.paginaCarregarMais : 1;
    await this.obterPrepostoLista();
    if(zerarEstado) {
      this.estadoCustomFormControl.setValue(null);
    }

    const estadoSelecionado = this.estadoCustomFormControl.value;

    var resultado = await this.service.obterAudienciaListaAsync(this.formGroup.value, this.ordenarPorFormControl.value, this.paginaCarregarMais);

    if (resultado) {
      this.audienciasLista = carregarMais ? this.audienciasLista.concat(JSON.parse(JSON.stringify(resultado.lista))) : JSON.parse(JSON.stringify(resultado.lista));
      this.audienciasListaOriginal = carregarMais ? this.audienciasListaOriginal.concat(JSON.parse(JSON.stringify(resultado.lista))) : JSON.parse(JSON.stringify(resultado.lista));
      this.totalAudiencias = resultado.total;
      this.totalGeralAudiencia = resultado.totalGeral;
      this.totalAudienciasRestante = this.totalAudiencias - this.audienciasLista.length; 
      this.estadoCustomList = resultado.listaEstados;
    }
    
    if(this.estadoCustomList.length > 0 && zerarEstado) {
      this.estadoCustomFormControl.setValue(this.estadoCustomList[0].id );
    }else{
      this.estadoCustomFormControl.setValue(estadoSelecionado);
    }
    
    if(!carregarMais)
      this.selecionarPrimeiroPreposto();
  }

  async buscarPorEstado(carregarMais?: boolean) {
    this.paginaCarregarMais = carregarMais ? this.paginaCarregarMais : 1;  

    const estadoSelecionado = this.estadoCustomFormControl.value;

    var resultado = await this.service.obterAudienciaListaPorEstadoAsync(this.formGroup.value, this.ordenarPorFormControl.value, this.paginaCarregarMais);

    if (resultado) {
      this.audienciasLista = carregarMais ? this.audienciasLista.concat(JSON.parse(JSON.stringify(resultado.lista))) : JSON.parse(JSON.stringify(resultado.lista));
      this.audienciasListaOriginal = carregarMais ? this.audienciasListaOriginal.concat(JSON.parse(JSON.stringify(resultado.lista))) : JSON.parse(JSON.stringify(resultado.lista));
      this.totalAudiencias = resultado.total;
      this.totalAudienciasRestante = this.totalAudiencias - this.audienciasLista.length; 
    }
     
    this.estadoCustomFormControl.setValue(estadoSelecionado);
       
    if(!carregarMais)
      this.selecionarPrimeiroPreposto();
  }

  async carregarMais(){
    this.paginaCarregarMais += 1;
    await this.buscarPorEstado(true);
  }

  async salvar(avancar: boolean) {
    await this.obterAudienciasModificadas();
    if(this.audienciasModificadas.length == 0)
      return await this.dialog.err('Inclusão não realizada', 'Deve ser feito ao menos 1 alteração.')
    try{
      await this.service.salvarPrepostoAsync(this.audienciasModificadas);
      await this.dialog.info('Inclusão realizada com sucesso', 'Preposto(s) incluído(s).');
      if(avancar)
        this.nextOption();
      this.audienciasModificadas = [];
      this.audienciasListaOriginal = JSON.parse(JSON.stringify(this.audienciasLista));
    } catch(err){
      await this.dialog.err('Erro na inclusão', err)
    }
  }

  async obterModalidaeLista(){
    this.service.obterModalidadeListaAsync().then(x => {
      this.modalidadeLista = x
    });
  }

  async obterLocalidadeLista(){
    this.service.obterLocalidadeListaAsync().then(x => {
      this.localidadeLista = x
    });
  }

//#endregion

//#region Metodos

  disableTabIndex(element: HTMLElement): void {
    const inputs = element.querySelectorAll('input');
    inputs.forEach(input => {
      input.setAttribute('tabIndex', '-1');
    });
  }

  selecionarPrimeiroPreposto(){
    setTimeout(() => {
      const elementeOrdenarPor = document.getElementById('ordenar');
      elementeOrdenarPor.classList.remove('ng-select-focused');
      const inputElement = this.elementRef.nativeElement.querySelector('.ng-select-focused input');
      if (inputElement) {
        inputElement.focus();
      }
    }, 500);   
  }

  async definirDataPadrao() {
    const today = new Date();
    const dayOfWeek = today.getDay();
    const daysUntilNextMonday = (7 - dayOfWeek + 1) % 7 || 7; // Garantir pelo menos 7 dias se hoje for segunda-feira
    const nextMonday = new Date(today);
    nextMonday.setDate(today.getDate() + daysUntilNextMonday);

    const nextFriday = new Date(nextMonday);
    nextFriday.setDate(nextMonday.getDate() + 4);

    this.dataInicialFormControl.setValue(null);
    this.dataFinalFormControl.setValue(null);
    this.dataInicialFormControl.updateValueAndValidity()
    this.dataFinalFormControl.updateValueAndValidity()
    setTimeout(() => {
      this.dataInicialFormControl.setValue(nextMonday);
      this.dataFinalFormControl.setValue(nextFriday);
      this.dataInicialFormControl.updateValueAndValidity()
      this.dataFinalFormControl.updateValueAndValidity()
    }, 100);  // Timeout de 1 segundo

  }

  marcarOpcoesPadrao(){
    this.indSecundarioFormControl.setValue(false);
    this.indUnicoFormControl.setValue(false);
    this.indPrimarioFormControl.setValue(false);
    this.processoEstrategicoFormControl.setValue('3');
    if(this.ehUsuarioAssociado){
      this.indSecundarioFormControl.setValue(true);
    }else{
      this.indUnicoFormControl.setValue(true);
      this.indPrimarioFormControl.setValue(true);
    }     
  }

  limparFiltros(){
    this.definirDataPadrao();
    this.marcarOpcoesPadrao();
    this.indProprioFormControl.setValue(true);  
    this.indTerceiroFormControl.setValue(true); 
    this.estadoListaFormControl.setValue(null);
    this.tipoAudienciaListaFormControl.setValue(null);   
    this.modalidadeListaFormControl.setValue(null);
    this.localidadeListaFormControl.setValue(null); 
    this.buscar(false, true);
  }

  atualizarPreposto(iAudiencia: number, iReclamada: number,event: any) {
      this.audienciasLista[iAudiencia].reclamadas[iReclamada].codPreposto = event != undefined ? event.id : null;
      this.ultimoPrepostoSelecionado = event != undefined ? event.id : this.ultimoPrepostoSelecionado;
  }

  replicarPreposto(iAudiencia: number, iReclamada: number){
    if(this.ultimoPrepostoSelecionado != null)
      this.audienciasLista[iAudiencia].reclamadas[iReclamada].codPreposto = this.ultimoPrepostoSelecionado;
  }

  async obterAudienciasModificadas(){
    this.audienciasLista.forEach((audiencia, audienciaIndex) => {
      audiencia.reclamadas.forEach((reclamada, reclamadaIndex) => {
        if (reclamada.codPreposto !== this.audienciasListaOriginal[audienciaIndex].reclamadas[reclamadaIndex].codPreposto) {
          let dataHora = `${this.datePipe.transform(audiencia.dateAudiencia, 'dd/MM/yyyy')} ${this.datePipe.transform(audiencia.horarioAudiencia, 'HH:mm')}`
          let [datePart, timePart] = dataHora.split(' ');
          let [day, month, year] = datePart.split('/');
          let [hours, minutes] = timePart.split(':');
          let dataHoraAudiencia = new Date(
            parseInt(year), // Ano
            parseInt(month) - 1, // Mês (0-based, então subtraímos 1)
            parseInt(day), // Dia
            parseInt(hours), // Hora
            parseInt(minutes) // Minutos
          );


          console.log(dataHoraAudiencia)

          this.audienciasModificadas.push({
            codProcesso: audiencia.codProcesso,
            seqAudiencia: audiencia.seqAudiencia,
            codParte: reclamada.id,
            codPreposto: reclamada.codPreposto,
            datAudiencia: new Date(dataHoraAudiencia)
          });
          
        }
      });
    });
  }

  editar(codProcesso:number){
    try {
      const url = environment.s2_url + '/Trabalhista/Trabalhista.mvc/Alterar?codigoDoProcesso=' + codProcesso;
      window.open(url, '_blank');
    } catch (error) {
      console.log(error.message);
    }
  }
  
  consultar(codProcesso:number){
    try {
      const url = environment.s2_url + '/Trabalhista/Trabalhista.mvc/Consultar?codigoDoProcesso=' + codProcesso;
      window.open(url, '_blank');
    } catch (error) {
      console.log(error.message);
    }
  }

  async exportarAgenda(){
    try {
      return await this.service.exportarListaAsync(this.formGroup.value, this.ordenarPorFormControl.value);
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      return await this.dialog.err("Erro ao exportar agenda", mensagem);
    }
    
  }

  titleSelectEstado(){
    for (const estados of this.estadoCustomList) {
      if (this.estadoCustomFormControl.value && this.estadoCustomFormControl.value == estados.id) {
        return estados.idDescricao;
      }else if (!this.estadoCustomFormControl.value) {
        return '';
      }
    }
  }

  ajustaToolTipReclamantes(reclamantes: string): string{
    const listaReclamantes = reclamantes.split(",");

    let listaReclamantesFormatada = `<div><ul style="text-align: left;">`

    listaReclamantes.forEach(reclamante => {
      listaReclamantesFormatada += `<li> ${reclamante} </li>`
    });

    listaReclamantesFormatada += `</ul> </div>`

    return listaReclamantesFormatada;
  }

  ajustaReclamantes(reclamantes: string): string {
    return reclamantes.split(',').length > 1 ? 'Reclamantes' : 'Reclamante';
  }

  descModalidadeLocalidade(localidade: string, modalidade: string): string{
    if (modalidade || localidade) {
      return `(${localidade} - ${modalidade})`;
    } else{
      return '';
    }
  }

//#endregion

//#region ESTADO PAGINATION

  async nextOption() {
    let indexEstado = this.estadoCustomList.findIndex(estado => estado.id === this.estadoCustomFormControl.value);
    if (indexEstado < this.estadoCustomList.length - 1) {
      this.estadoCustomFormControl.setValue(this.estadoCustomList[indexEstado + 1].id);
      await this.buscarPorEstado(false)
    }
  }

  async previousOption() {
    let indexEstado = this.estadoCustomList.findIndex(estado => estado.id === this.estadoCustomFormControl.value);
    if (indexEstado > 0) {
      this.estadoCustomFormControl.setValue(this.estadoCustomList[indexEstado - 1].id);
      await this.buscarPorEstado(false) 
    }
  }

  isNextDisabled() {
    let indexEstado = this.estadoCustomList.findIndex(estado => estado.id === this.estadoCustomFormControl.value);
    return indexEstado >= this.estadoCustomList.length - 1;
  }
  
  isPreviousDisabled() {
    let indexEstado = this.estadoCustomList.findIndex(estado => estado.id === this.estadoCustomFormControl.value);
    return indexEstado <= 0;
  }

//#endregion

}