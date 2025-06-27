import { Component, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';
import { StaticInjector } from '../../static-injector';
import { ManutencaoContratoEscritorioService } from '../../services/manutencao-contrato-escritorio.service';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-manutencao-contrato-escritorio-modal',
  templateUrl: './manutencao-contrato-escritorio-modal.component.html',
  styleUrls: ['./manutencao-contrato-escritorio-modal.component.scss']
})
export class ManutencaoContratoEscritorioModalComponent implements OnInit {

  constructor(
    private modal: NgbActiveModal,
    private dialog: DialogService,
    private service: ManutencaoContratoEscritorioService,
    private formBuilder: FormBuilder,
    private datePipe: DatePipe,
  ) { }

  codContrato = 0;
  tipoContratosList: Array<any>;
  escritoriosList: Array<any>;
  estadosList: Array<any>;
  atuacaoList: Array<any>;
  diretoriaList: Array<any>;

  async ngOnInit() {
    await this.iniciarCombosAsync();
    this.contratoDiretoriaFormControl.setValue(this.diretoriaList.filter(item => item.indicador === 'S').map(item => item.codigo)[0])
  }

  async iniciarCombosAsync() {
    try {
      this.tipoContratosList = await this.service.obterTipoContrato();
      this.escritoriosList = await this.service.obterEscritorio();
      this.estadosList = await this.service.obterEstados();
      this.atuacaoList = await this.service.obterAtuacao();
      this.diretoriaList = await this.service.obterDiretoria();
    } catch (erro) {
      await this.dialog.err(`Exclusão não realizada`, erro);
    }
  }

  codTipoContratoEscritorioFormControl: FormControl = new FormControl();
  datInicioVigenciaFormControl: FormControl = new FormControl();
  datFimVigenciaFormControl: FormControl = new FormControl();
  cnpjFormControl: FormControl = new FormControl();
  numContratoJecVcFormControl: FormControl = new FormControl();
  numContratoProconFormControl: FormControl = new FormControl();
  nomContratoFormControl: FormControl = new FormControl();
  valUnitarioJecCcFormControl: FormControl = new FormControl();
  valUnitarioProconFormControl: FormControl = new FormControl();
  valUnitAudCapitalFormControl: FormControl = new FormControl();
  valUnitAudInteriorFormControl: FormControl = new FormControl();
  valVepFormControl: FormControl = new FormControl();
  numSgpagJecVcFormControl: FormControl = new FormControl();
  numSgpagProconFormControl: FormControl = new FormControl();
  numMesesPermanenciaFormControl: FormControl = new FormControl();
  valDescontoPermanenciaFormControl: FormControl = new FormControl();
  indPermanenciaLegadoFormControl: FormControl = new FormControl(false);
  indAtivoFormControl: FormControl = new FormControl(true);
  indConsideraCalculoVepFormControl: FormControl = new FormControl(true);
  contratoAtuacaoFormControl: FormControl = new FormControl(1);
  contratoDiretoriaFormControl: FormControl = new FormControl();
  
  escritoriosSelecionados: number[] = [];
  estadosSelecionados = [];

  formGroup: FormGroup = this.formBuilder.group({
    codTipoContratoEscritorio: this.codTipoContratoEscritorioFormControl,
    datInicioVigencia: this.datInicioVigenciaFormControl,
    datFimVigencia: this.datFimVigenciaFormControl,
    cnpj: this.cnpjFormControl,
    numContratoJecVc: this.numContratoJecVcFormControl,
    numContratoProcon: this.numContratoProconFormControl,
    nomContrato: this.nomContratoFormControl,

    valUnitarioJecCc: this.valUnitarioJecCcFormControl,
    valUnitarioProcon: this.valUnitarioProconFormControl,
    valUnitAudCapital: this.valUnitAudCapitalFormControl,
    valUnitAudInterior: this.valUnitAudInteriorFormControl,
    valDescontoPermanencia: this.valDescontoPermanenciaFormControl,
    numMesesPermanencia: this.numMesesPermanenciaFormControl,
    
    numSgpagJecVc: this.numSgpagJecVcFormControl,
    numSgpagProcon: this.numSgpagProconFormControl,
    valVep: this.valVepFormControl,
    indPermanenciaLegado: this.indPermanenciaLegadoFormControl,
    indAtivo: this.indAtivoFormControl,
    indConsideraCalculoVep: this.indConsideraCalculoVepFormControl,
    codContratoAtuacao: this.contratoAtuacaoFormControl,
    codContratoDiretoria: this.contratoDiretoriaFormControl,

    escritorios: null,
    estados: null
  });

  public static exibeModalDeIncluir(): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(ManutencaoContratoEscritorioModalComponent, { windowClass: 'contrato-escritorio-modal', centered: true, size: 'lg', backdrop: 'static' });
      modalRef.componentInstance.codContrato = 0;
    modalRef.componentInstance.validator();

    return modalRef.result;
  }

  async save() {
    this.formGroup.get('escritorios').setValue(this.escritoriosSelecionados);
    this.formGroup.get('estados').setValue(this.estadosSelecionados);
    await this.validator();

    if(this.formGroup.invalid) {
      await this.dialog.info(
        `A ${this.codContrato == 0 ? 'Inclusão' : 'Alteração'} não poderá ser realizada`,
        'Verifique os campos e tente novamente.'
      );
      this.formGroup.markAllAsTouched();
      return;
    }
    
    if(this.escritoriosSelecionados.length == 0 || this.estadosSelecionados.length == 0) {
      await this.dialog.info(
        `A ${this.codContrato == 0 ? 'Inclusão' : 'Alteração'} não poderá ser realizada`,
        'O contrato deve conter <br>ao menos 1 Escritório e 1 Estado'
      );
      return;
    }

    if(this.formGroup.valid){
      var indPermanenciaLegadoOriginal = this.indPermanenciaLegadoFormControl.value;
      var indAtivoOriginal = this.indAtivoFormControl.value;
      var indConsideraCalculoVepOriginal = this.indConsideraCalculoVepFormControl.value;

      this.formGroup.get('indPermanenciaLegado').setValue(this.indPermanenciaLegadoFormControl.value ? "S" : "N");
      this.formGroup.get('indAtivo').setValue(this.indAtivoFormControl.value ? "S" : "N");
      this.formGroup.get('indConsideraCalculoVep').setValue(this.indConsideraCalculoVepFormControl.value ? "S" : "N");
      if(this.codContrato == 0){
        try{
          await this.service.incluirContrato(this.formGroup.value);
          this.formGroup.get('indPermanenciaLegado').setValue(indPermanenciaLegadoOriginal);
          this.formGroup.get('indAtivo').setValue(indAtivoOriginal);
          this.formGroup.get('indConsideraCalculoVep').setValue(indConsideraCalculoVepOriginal);
          await this.dialog.alert('Cadastrar contrato.', "Contrato incluído com sucesso.");
          this.modal.close(true);
        } catch (error) {
          this.formGroup.get('indPermanenciaLegado').setValue(indPermanenciaLegadoOriginal);
          this.formGroup.get('indAtivo').setValue(indAtivoOriginal);
          this.formGroup.get('indConsideraCalculoVep').setValue(indConsideraCalculoVepOriginal);
          await this.validator();
          this.formGroup.markAllAsTouched();
          return this.dialog.err('Cadastrar contrato', error );
        }
      }else{
        try{
          await this.service.editarContrato(this.codContrato, this.formGroup.value);
          this.formGroup.get('indPermanenciaLegado').setValue(indPermanenciaLegadoOriginal);
          this.formGroup.get('indAtivo').setValue(indAtivoOriginal);
          this.formGroup.get('indConsideraCalculoVep').setValue(indConsideraCalculoVepOriginal);
          await this.dialog.alert('Alterar contrato.', "Contrato alterado com sucesso.");
          this.modal.close(true);
        } catch (error) {
          this.formGroup.get('indPermanenciaLegado').setValue(indPermanenciaLegadoOriginal);
          this.formGroup.get('indAtivo').setValue(indAtivoOriginal);
          this.formGroup.get('indConsideraCalculoVep').setValue(indConsideraCalculoVepOriginal);
          await this.validator();
          this.formGroup.markAllAsTouched();
          return this.dialog.err('Alterar contrato', error );
        }
      }
      
    }
  }

  async   validator() {
    this.codTipoContratoEscritorioFormControl.value == null ? this.codTipoContratoEscritorioFormControl.setValidators([Validators.required]) : this.codTipoContratoEscritorioFormControl.clearValidators();
    this.codTipoContratoEscritorioFormControl.updateValueAndValidity();
    
    this.datInicioVigenciaFormControl.value == null ? this.datInicioVigenciaFormControl.setValidators([Validators.required]) : this.datInicioVigenciaFormControl.clearValidators();
    this.datInicioVigenciaFormControl.updateValueAndValidity();
    
    this.datFimVigenciaFormControl.value == null ? this.datFimVigenciaFormControl.setValidators([Validators.required]) : this.datFimVigenciaFormControl.clearValidators();
    this.datFimVigenciaFormControl.updateValueAndValidity();

    if(this.datInicioVigenciaFormControl.value && this.datFimVigenciaFormControl.value && this.datInicioVigenciaFormControl.value > this.datFimVigenciaFormControl.value){
      this.datInicioVigenciaFormControl.setErrors({ 'invalidDate': true})
    }

    if(this.cnpjFormControl.value == null || this.cnpjFormControl.value == '') {
      this.cnpjFormControl.setValidators([Validators.required]);
    } 
    else if(this.cnpjFormControl.value != null){
      this.cnpjFormControl.setValidators([Validators.minLength(14), Validators.maxLength(14)]);
    }else{
      this.cnpjFormControl.clearValidators();
    }
    this.cnpjFormControl.updateValueAndValidity();
      
    this.nomContratoFormControl.value == null || this.nomContratoFormControl.value == '' ? this.nomContratoFormControl.setValidators([Validators.required]) : this.nomContratoFormControl.clearValidators();
    this.nomContratoFormControl.updateValueAndValidity();
    
    this.valVepFormControl.value != null ? this.valVepFormControl.setValidators([Validators.min(0.01)]) : this.valVepFormControl.clearValidators();
    this.valVepFormControl.updateValueAndValidity();
    
    this.valUnitAudCapitalFormControl.setValidators([Validators.required]) == null ? this.valUnitAudCapitalFormControl.setValidators([Validators.required]) : this.valUnitAudCapitalFormControl.clearValidators();
    this.valUnitAudCapitalFormControl.updateValueAndValidity();
    
    this.valUnitAudInteriorFormControl.setValidators([Validators.required]) == null ? this.valUnitAudInteriorFormControl.setValidators([Validators.required]) : this.valUnitAudInteriorFormControl.clearValidators();
    this.valUnitAudInteriorFormControl.updateValueAndValidity();

    
    if(this.indPermanenciaLegadoFormControl.value){
      this.numMesesPermanenciaFormControl.enable();
      this.numMesesPermanenciaFormControl.setValidators([Validators.required]);
    }else{
      this.numMesesPermanenciaFormControl.disable();
      this.numMesesPermanenciaFormControl.setValue(null);
      this.numMesesPermanenciaFormControl.clearValidators();
    }
    this.numMesesPermanenciaFormControl.updateValueAndValidity();
    
    if(this.indPermanenciaLegadoFormControl.value){
      this.valDescontoPermanenciaFormControl.enable();
      this.valDescontoPermanenciaFormControl.setValidators([Validators.required]);
    }else{
      this.valDescontoPermanenciaFormControl.setValue(null);
      this.valDescontoPermanenciaFormControl.disable();
      this.valDescontoPermanenciaFormControl.clearValidators();
    }
    this.valDescontoPermanenciaFormControl.updateValueAndValidity();

    this.contratoAtuacaoFormControl.value == null ? this.contratoAtuacaoFormControl.setValidators([Validators.required]) : this.contratoAtuacaoFormControl.clearValidators();
    this.contratoAtuacaoFormControl.updateValueAndValidity();
    
    this.contratoDiretoriaFormControl.value == null ? this.contratoDiretoriaFormControl.setValidators([Validators.required]) : this.contratoDiretoriaFormControl.clearValidators();
    this.contratoDiretoriaFormControl.updateValueAndValidity();

    // valUnitarioJecCcFormControl
    // valUnitarioProconFormControl  
    switch (this.contratoAtuacaoFormControl.value) {
      case 1:
        this.numContratoJecVcFormControl.enable();
        this.numContratoJecVcFormControl.setValidators([Validators.required]);
        this.numContratoJecVcFormControl.updateValueAndValidity();
        this.numSgpagJecVcFormControl.enable();
        this.numSgpagJecVcFormControl.setValidators([Validators.required]);
        this.numSgpagJecVcFormControl.updateValueAndValidity();
        this.valUnitarioJecCcFormControl.enable();
        this.valUnitarioJecCcFormControl.setValidators([Validators.required]);
        this.valUnitarioJecCcFormControl.updateValueAndValidity();
    
        this.numContratoProconFormControl.disable();
        this.numContratoProconFormControl.setValue(null);
        this.numContratoProconFormControl.clearValidators();
        this.numContratoProconFormControl.updateValueAndValidity();
        this.numSgpagProconFormControl.disable();
        this.numSgpagProconFormControl.setValue(null);
        this.numSgpagProconFormControl.clearValidators();
        this.numSgpagProconFormControl.updateValueAndValidity();
        this.valUnitarioProconFormControl.disable();
        this.valUnitarioProconFormControl.setValue(null);
        this.valUnitarioProconFormControl.clearValidators();
        this.valUnitarioProconFormControl.updateValueAndValidity();
        break;
      case 2:
        this.numContratoJecVcFormControl.enable();
        this.numContratoJecVcFormControl.setValidators([Validators.required]);
        this.numContratoJecVcFormControl.updateValueAndValidity();
        this.numSgpagJecVcFormControl.enable();
        this.numSgpagJecVcFormControl.setValidators([Validators.required]);
        this.numSgpagJecVcFormControl.updateValueAndValidity();
        this.valUnitarioJecCcFormControl.enable();
        this.valUnitarioJecCcFormControl.setValidators([Validators.required]);
        this.valUnitarioJecCcFormControl.updateValueAndValidity();

        this.numContratoProconFormControl.enable();
        this.numContratoProconFormControl.setValidators([Validators.required]);
        this.numContratoProconFormControl.updateValueAndValidity();
        this.numSgpagProconFormControl.enable();
        this.numSgpagProconFormControl.setValidators([Validators.required]);
        this.numSgpagProconFormControl.updateValueAndValidity();
        this.valUnitarioProconFormControl.enable();
        this.valUnitarioProconFormControl.setValidators([Validators.required]);
        this.valUnitarioProconFormControl.updateValueAndValidity();
        break;
    }

  }

  async validatorIndPermanecia(){
    // if(this.codContrato == 0)
    this.indPermanenciaLegadoFormControl.setValue(!this.indPermanenciaLegadoFormControl.value)
    await this.validator()
  }

  async checkEstado(uf: string){
    if(this.estadosSelecionados.find(estado => estado == uf)){
      this.estadosSelecionados = this.estadosSelecionados.filter(estado => estado != uf);
    }
    else{
      this.estadosSelecionados.push(uf);
    }
  }

  marcarTodas(marcar?: boolean) {
      let marcarEstados = document.querySelectorAll("input[name='ufs']:not(:checked)")
      let desmarcarEstados = document.querySelectorAll("input[name='ufs']:checked")
      let estadosNaoMarcados = (<HTMLInputElement><unknown>document.querySelectorAll("input[name='ufs']:not(:checked)"))
      let estadosMarcados = (<HTMLInputElement><unknown>document.querySelectorAll("input[name='ufs']:checked"))
      if(marcarEstados.length != 0){
        this.estadosSelecionados = [];
        for (let i = 0; i < marcarEstados.length; i++) {
          estadosNaoMarcados[i].checked = true
          this.estadosSelecionados.push(estadosNaoMarcados[i].id);
        }
      }
      if(desmarcarEstados.length == this.estadosList.length){
        this.estadosSelecionados = [];
        for (let i = 0; i < desmarcarEstados.length; i++) {
          estadosMarcados[i].checked = false
        }
      }
  }

  async marcarEstadosEdit(ufs: string[]){
    let marcarEstados = document.querySelectorAll("input[name='ufs']:not(:checked)")
    let estadosNaoMarcados = (<HTMLInputElement><unknown>document.querySelectorAll("input[name='ufs']:not(:checked)"))
    this.estadosSelecionados = [];

    for (let i = 0; i < marcarEstados.length; i++) {
      if(ufs.find(x => x == estadosNaoMarcados[i].id)){
        estadosNaoMarcados[i].checked = true
        this.estadosSelecionados.push(estadosNaoMarcados[i].id);
      }
    }

  }
    
//================================================================

    public static exibeModalDeAlterar(codContrato: number): Promise<boolean> {
      const modalRef = StaticInjector.Instance.get(NgbModal)
        .open(ManutencaoContratoEscritorioModalComponent, { windowClass: 'contrato-escritorio-modal', centered: true, size: 'lg', backdrop: 'static' });
        modalRef.componentInstance.codContrato = codContrato;
      modalRef.componentInstance.carregarContrato(codContrato);
      return modalRef.result;
    }

    async carregarContrato(codContrato: number){
      const contratoCarregado = await this.service.obterContrato(codContrato);
      console.log(contratoCarregado[0]);
      let contratosList = contratoCarregado[0].escritorios.filter((item, index) => contratoCarregado[0].escritorios.indexOf(item) === index);
      this.escritoriosSelecionados = contratosList;

      let estadosList = contratoCarregado[0].uf.filter((item, index) => contratoCarregado[0].uf.indexOf(item) === index);
      setTimeout(() => {
        this.marcarEstadosEdit(estadosList);
        
        this.formGroup.patchValue(contratoCarregado[0]);
        this.validator();
        this.formGroup.markAllAsTouched();
        this.datInicioVigenciaFormControl.setValue(new Date(contratoCarregado[0].datInicioVigencia));
        this.datFimVigenciaFormControl.setValue(new Date(contratoCarregado[0].datFimVigencia));
      }, 1500);
    }


  close(): void {
    this.modal.close(false);
  }
}
