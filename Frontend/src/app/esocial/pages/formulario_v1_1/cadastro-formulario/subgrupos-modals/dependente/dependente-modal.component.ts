import { Component, AfterViewInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ErrorLib } from '@esocial/libs/error-lib';
import { Dependente } from '@esocial/models/subgrupos/dependente';
import { TipoDependente } from '@esocial/models/tipo-dependente';
import { ESocialListaFormularioService } from '@esocial/services/aplicacao/e-social-lista-formulario.service';
import { DependenteService } from '@esocial/services/formulario_v1_1/subgrupos/dependente.service';
// import { TipoDependenteService } from '@esocial/services/formulario_v1_1/subgrupos/tipo-dependente.services';
import { StaticInjector } from '@esocial/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';
import { FormControlCustomValidators } from '@shared/validators/form-control-custom-validators';


@Component({
  selector: 'app-dependente-modal',
  templateUrl: './dependente-modal.component.html',
  styleUrls: ['./dependente-modal.component.scss']
})
export class DependenteModalComponent implements AfterViewInit {
  titulo: string;
  f2500 : number;
  dependente: Dependente = null;
  listaTipoDependente : TipoDependente[];
  cpfTrabalhador: string;
  cpfigualtrabalhador : boolean = false;
  temPermissaoEsocialBlocoABCDEFHI : boolean = false;

  idFormControl: FormControl = new FormControl(null);
  idFormularioFormControl: FormControl = new FormControl(null);
  cpfFormControl: FormControl = new FormControl(null,[Validators.required, this.customValidators.cpfValido()]);
  tipoDependenteFormControl: FormControl = new FormControl(null, Validators.required);
  descricaoDependenteFormControl: FormControl = new FormControl(null, Validators.maxLength(30));
  logCodUsuarioFormControl: FormControl = new FormControl(null);
  logDataOperacaoFormControl: FormControl = new FormControl(null);

  mascaraCpf = [/[0-9]/, /\d/, /\d/, '.', /\d/, /\d/, /\d/, '.', /\d/, /\d/, /\d/, '-', /\d/, /\d/];


  formGroup: FormGroup = new FormGroup({
    idEsF2500Dependente: this.idFormControl,
    idF2500: this.idFormularioFormControl,
    dependenteCpfdep: this.cpfFormControl,
    dependenteTpdep: this.tipoDependenteFormControl,
    dependenteDescdep: this.descricaoDependenteFormControl,
    logCodUsuario: this.logCodUsuarioFormControl,
    logDataOperacao: this.logDataOperacaoFormControl
  });

  constructor(
    private service: DependenteService,
    private serviceTpDependente : ESocialListaFormularioService,
    private dialogService: DialogService,
    private modal: NgbActiveModal,
    private customValidators: FormControlCustomValidators

  ) {}


  ngAfterViewInit() {
    this.obterTipoDependente();
    this.iniciarForm();
    this.cpfChange();
  }

  iniciarForm() {
    if (this.dependente) {
      this.idFormControl.setValue(this.dependente.idEsF2500Dependente);
      this.idFormularioFormControl.setValue(this.dependente.idF2500);
      this.cpfFormControl.setValue(this.dependente.dependenteCpfdep);
      this.tipoDependenteFormControl.setValue(this.dependente.dependenteTpdep ? parseInt(this.dependente.dependenteTpdep) : 0);
      this.descricaoDependenteFormControl.setValue(
        this.dependente.dependenteDescdep
      );
      this.logCodUsuarioFormControl.setValue(this.dependente.logCodUsuario);
      this.logDataOperacaoFormControl.setValue(this.dependente.logDataOperacao);

      if (this.cpfTrabalhador != ''){
        this.cpfFormControl.setValidators([Validators.required, this.customValidators.cpfDuplicado(this.cpfTrabalhador)]);
      }

      if (!this.temPermissaoEsocialBlocoABCDEFHI) {
        this.idFormControl.disable();
        this.idFormularioFormControl.disable();
        this.cpfFormControl.disable();
        this.tipoDependenteFormControl.disable();
        this.descricaoDependenteFormControl.disable();
        this.logCodUsuarioFormControl.disable();
        this.logDataOperacaoFormControl.disable();
      }
    }
  }

  async salvar() {
    this.cpfFormControl;

    try {
      if (this.dependente) {
        await this.service.alterar(this.formGroup.value);
      } else {
        let obj = this.formGroup.value;
        obj.idF2500 = this.f2500;
        await this.service.incluir(this.formGroup.value);
      }

      await this.dialogService.alert(`${this.titulo} realizada com sucesso`);
      this.modal.close(true);
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
        await this.dialogService.err(
          'Desculpe, a operação não poderá ser realizada',
          mensagem
        );
    }
  }

  static exibeModal(dependente: Dependente, f2500 : number, cpfTrabalhador : string, temPermissaoEsocialBlocoABCDEFHI : boolean): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      DependenteModalComponent,
      { windowClass: 'modal-dependente', centered: true, size: 'lg', backdrop: 'static' }
    );
    modalRef. componentInstance.dependente = dependente;
    modalRef.componentInstance.titulo = dependente == null ? 'Incluir' : temPermissaoEsocialBlocoABCDEFHI ? 'Alterar' : 'Consultar'
    modalRef.componentInstance.f2500 = f2500;
    modalRef.componentInstance.cpfTrabalhador = cpfTrabalhador;
    modalRef.componentInstance.temPermissaoEsocialBlocoABCDEFHI = temPermissaoEsocialBlocoABCDEFHI;
    return modalRef.result;
  }

  close(): void {
    this.modal.close(false);
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

  tipoDependenteChange(){
    if (this.tipoDependenteFormControl.value == 99){
      this.descricaoDependenteFormControl.enable();
      this.descricaoDependenteFormControl.setValidators(Validators.required);
      this.descricaoDependenteFormControl.updateValueAndValidity();
    }
    else{
      this.descricaoDependenteFormControl.clearValidators();
      this.descricaoDependenteFormControl.updateValueAndValidity();
      this.descricaoDependenteFormControl.disable();
      this.descricaoDependenteFormControl.setValue(null);
    }
  }


  cpfChange(){
    this.cpfFormControl.setValidators([Validators.required, this.customValidators.cpfDuplicado(this.cpfTrabalhador.toString()), this.customValidators.cpfValido()]);
    this.cpfFormControl.updateValueAndValidity();
  }

  desabilitaTooltip(control: FormControl):boolean{
    return control.valid || control.untouched || control.disabled
  }


}
