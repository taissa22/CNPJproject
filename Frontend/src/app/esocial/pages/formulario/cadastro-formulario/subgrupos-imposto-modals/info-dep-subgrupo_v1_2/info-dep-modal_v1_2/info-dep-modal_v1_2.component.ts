import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ErrorLib } from '@esocial/libs/error-lib';
import { InfoDep } from '@esocial/models/subgrupos/v1_2/info-dep';
import { TipoDependente } from '@esocial/models/tipo-dependente';
import { ESocialListaFormularioService } from '@esocial/services/aplicacao/e-social-lista-formulario.service';
import { InfoDepService } from '@esocial/services/formulario/subgrupos/info-dep.service';
import { StaticInjector } from '@esocial/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';
import { FormControlCustomValidators } from '@shared/validators/form-control-custom-validators';
import { promise } from 'protractor';

@Component({
  selector: 'app-info-dep-modal-v1-2',
  templateUrl: './info-dep-modal_v1_2.component.html',
  styleUrls: ['./info-dep-modal_v1_2.component.scss']
})
export class InfoDepModal_v1_2Component implements AfterViewInit {

  constructor(
    private modal: NgbActiveModal,
    private service: InfoDepService,
    private serviceList: ESocialListaFormularioService,
    private serviceTpDependente : ESocialListaFormularioService,
    private customValidators: FormControlCustomValidators,
    private dialog: DialogService,
  ) { }

  async ngAfterViewInit(): Promise<void> {
    await this.obterTipoDependente();
    await this.obterInfoDep();
    this.iniciarForm();
    this.tipoDependenteChange();
    this.infodepDepirrfChange();
  }

  temPermissaoBlocoG: boolean = false;
  titulo: string;
  codF2501: number;
  codInfoDep: number;
  codReceita: string;
  valorIrrf: number;
  infoDep: InfoDep;
  listaTipoDependente : TipoDependente[];
  regex12_2 = /^([0-9]{1,10}\,[0-9]{2})/g;
  descCount = 0;
  totalCaracterDescDep = 0;

  ehDependenteIr = [
    { id: 'S', descricao: 'Sim' }
  ];

  mascaraCpf = [
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    '.',
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    '.',
    /[0-9]/,
    /[0-9]/,
    /[0-9]/,
    '-',
    /[0-9]/,
    /[0-9]/
  ];

  dataAtual = new Date();
  dataNascimentoMinima = new Date(1890, 0, 1);


  //#region MÉTODOS
  async obterInfoDep() {
    if (this.codInfoDep) {
      const response = await this.service.obterInfoDep(this.codF2501, this.codInfoDep);
      if (response){
            this.infoDep = InfoDep.fromObj(response);          
      }      
    }
  }

  async obterTipoDependente() {
    const resposta = await this.serviceTpDependente.obterTipoDependenteAsync();
    if (resposta)
    {
      this.listaTipoDependente = resposta.map(dependente => {
        return ({
          id: dependente.id,
          descricao: dependente.descricao,
          descricaoConcatenada: `${dependente.id} - ${dependente.descricao}`
        })
      })
    }
  }

  async salvar() {
    let operacao = this.titulo == 'Alterar' ? 'Alteração' : 'Inclusão';
    let valueSubmit = this.formGroup.getRawValue();
    // valueSubmit.calctribPerref = Number(valueSubmit.calctribPerref);
    // valueSubmit.calctribVrbccpmensal = Number(valueSubmit.calctribVrbccpmensal);

    try {
      if (this.titulo == "Alterar") {
         await this.service.atualizar(this.codF2501, this.codInfoDep, valueSubmit);
      } else {
         await this.service.incluir(this.codF2501, valueSubmit);
      }

      await this.dialog.alert('Operação realizada', `${operacao} realizada com sucesso`);
      this.modal.close(true);

    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialog.err(`Desculpe, não foi possivel realizar a ${operacao}`,mensagem)
    }
  }

  //#endregion


  static exibeModalAlterar(codF2501: number, codInfoDep: number, temPermissaoBlocoG: boolean): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      InfoDepModal_v1_2Component,
      { windowClass: 'modal-info-dep', centered: true, size: 'lg', backdrop: 'static' }
    );
    modalRef.componentInstance.codF2501 = codF2501;
    modalRef.componentInstance.codInfoDep = codInfoDep;
    modalRef.componentInstance.temPermissaoBlocoG = temPermissaoBlocoG;
    modalRef.componentInstance.titulo = 'Alterar';
    return modalRef.result;
  }

  static exibeModalConsultar(codF2501: number, codInfoDep: number): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      InfoDepModal_v1_2Component,
      { windowClass: 'modal-info-dep', centered: true, size: 'lg', backdrop: 'static' }
    );
    modalRef.componentInstance.codF2501 = codF2501;
    modalRef.componentInstance.codInfoDep = codInfoDep;
    modalRef.componentInstance.temPermissaoBlocoG = false;
    modalRef.componentInstance.titulo = 'Consultar';
    return modalRef.result;
  }

  static exibeModalIncluir(codF2501: number, temPermissaoBlocoG: boolean): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      InfoDepModal_v1_2Component,
      { windowClass: 'modal-info-dep', centered: true, size: 'lg', backdrop: 'static' }
    );
    modalRef.componentInstance.codF2501 = codF2501;
    modalRef.componentInstance.temPermissaoBlocoG = temPermissaoBlocoG;
    modalRef.componentInstance.titulo = 'Incluir';
    return modalRef.result;
  }

  close(): void {
    this.modal.close(false);
  }

  //#region FORMCONTROL

  infodepCpfdepFormControl: FormControl = new FormControl(null, [Validators.required, this.customValidators.cpfValido()]);
  infodepDtnasctoFormControl: FormControl = new FormControl(null); 
  infodepNomeFormControl : FormControl = new FormControl(null,[Validators.minLength(2)]); 
  infodepDepirrfFormControl : FormControl = new FormControl(null);
  infodepTpdepFormControl : FormControl = new FormControl(null);
  infodepDescrdepFormControl : FormControl = new FormControl(null, Validators.maxLength(100));

  formGroup: FormGroup = new FormGroup({
    infodepCpfdep: this.infodepCpfdepFormControl,
    infodepDtnascto: this.infodepDtnasctoFormControl,
    infodepNome : this.infodepNomeFormControl, 
    infodepDepirrf : this.infodepDepirrfFormControl, 
    infodepTpdep : this.infodepTpdepFormControl, 
    infodepDescrdep : this.infodepDescrdepFormControl
  });

  iniciarForm() {
    if(this.codInfoDep > 0){
      this.infodepCpfdepFormControl.setValue(this.infoDep.infodepCpfdep);
      this.infodepDtnasctoFormControl.setValue(this.infoDep.infodepDtnascto);
      this.infodepNomeFormControl.setValue(this.infoDep.infodepNome);
      this.infodepDepirrfFormControl.setValue(this.infoDep.infodepDepirrf);
      this.infodepTpdepFormControl.setValue(this.infoDep.infodepTpdep != null ? Number(this.infoDep.infodepTpdep) : null);
      this.infodepDescrdepFormControl.setValue(this.infoDep.infodepDescrdep);
      if (!this.temPermissaoBlocoG) {
        this.infodepCpfdepFormControl.disable();
        this.infodepDtnasctoFormControl.disable();
        this.infodepNomeFormControl.disable();
        this.infodepDepirrfFormControl.disable();
        this.infodepTpdepFormControl.disable();
        this.infodepDescrdepFormControl.disable();
      }
    }else{
      this.infodepCpfdepFormControl.setValue(null);
      this.infodepDtnasctoFormControl.setValue(null);
      this.infodepNomeFormControl.setValue(null);
      this.infodepDepirrfFormControl.setValue(null);
      this.infodepTpdepFormControl.setValue(null);
      this.infodepDescrdepFormControl.setValue(null);
    }
  }

  desabilitaTooltip(control: FormControl):boolean{
    return control.valid || control.untouched || control.disabled
  }

  tipoDependenteChange(){
    if (this.infodepTpdepFormControl.value == 99){
      this.infodepDescrdepFormControl.enable();
      this.infodepDescrdepFormControl.setValidators(Validators.required);
      this.infodepDescrdepFormControl.updateValueAndValidity();
    }
    else{
      this.infodepDescrdepFormControl.clearValidators();
      this.infodepDescrdepFormControl.updateValueAndValidity();
      this.infodepDescrdepFormControl.disable();
      this.infodepDescrdepFormControl.setValue(null);
    }
  }

  calculaTamanho(item) {
    return (this.totalCaracterDescDep = item.length);
  }

  checkTeste(value: any) {
    alert(value);
  }

  infodepDepirrfChange(){
    if (!this.infodepDepirrfFormControl.value) {
      this.infodepTpdepFormControl.setValue(null);
      this.infodepTpdepFormControl.disable();
      this.infodepTpdepFormControl.updateValueAndValidity();
    }else{
      this.infodepTpdepFormControl.enable();
      this.infodepTpdepFormControl.setValidators(Validators.required);
      this.infodepTpdepFormControl.updateValueAndValidity();
    }
  }
  
  //#endregion
}