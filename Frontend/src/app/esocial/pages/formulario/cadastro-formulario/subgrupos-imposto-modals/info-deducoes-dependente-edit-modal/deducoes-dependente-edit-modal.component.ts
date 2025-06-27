import { Component, AfterViewInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ErrorLib } from '@esocial/libs/error-lib';
import { DeduçoesDependente } from '@esocial/models/subgrupos/v1_2/deducoes-dependente';
import { TipoDependente } from '@esocial/models/tipo-dependente';
import { ESocialListaFormularioService } from '@esocial/services/aplicacao/e-social-lista-formulario.service';
import { DeducoesDependenteService } from '@esocial/services/formulario/subgrupos/deducoes-dependente.service';
// import { TipoDependenteService } from '@esocial/services/formulario_v1_1/subgrupos/tipo-dependente.services';
import { StaticInjector } from '@esocial/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';
import { FormControlCustomValidators } from '@shared/validators/form-control-custom-validators';


@Component({
  selector: 'app-dedDepen-modal',
  templateUrl: './deducoes-dependente-edit-modal.component.html',
  styleUrls: ['./deducoes-dependente-edit-modal.component.scss']
})
export class DedDepenModal_v1_2_Component implements AfterViewInit {
  titulo: string;
  f2501 : number;
  dependente: DeduçoesDependente = null;
  listaTipoRendimento : TipoDependente[];
  cpfTrabalhador: string;
  cpfigualtrabalhador : boolean = false;
  temPermissaoBlocoCeDeE : boolean = false;
  codIrrf:number;

  idFormControl: FormControl = new FormControl(null);
  idFormularioFormControl: FormControl = new FormControl(null);
  cpfFormControl: FormControl = new FormControl(null,[Validators.required, this.customValidators.cpfValido()]);
  tipoRendimentoFormControl: FormControl = new FormControl(null, Validators.required);
  deddepenVlrdeducaoFormControl: FormControl = new FormControl(null, [Validators.required]);
  logCodUsuarioFormControl: FormControl = new FormControl(null);
  logDataOperacaoFormControl: FormControl = new FormControl(null);
  codigoIrrfFormControl: FormControl = new FormControl(null);

  mascaraCpf = [/[0-9]/, /\d/, /\d/, '.', /\d/, /\d/, /\d/, '.', /\d/, /\d/, /\d/, '-', /\d/, /\d/];


  formGroup: FormGroup = new FormGroup({
    deddepenCpfdep: this.cpfFormControl,
    deddepenTprend: this.tipoRendimentoFormControl,
    deddepenVlrdeducao: this.deddepenVlrdeducaoFormControl,
  });

  constructor(
    private service: DeducoesDependenteService,
    private serviceTpRendimento : ESocialListaFormularioService,
    private dialogService: DialogService,
    private modal: NgbActiveModal,
    private customValidators: FormControlCustomValidators

  ) {}


  ngAfterViewInit() {
    this.obterTipoRendimento();
    this.iniciarForm();
    this.cpfChange();
    
  }

  iniciarForm() {
    if (this.dependente) {
      this.idFormControl.setValue(this.dependente.idEsF2501Deddepen);
      this.idFormularioFormControl.setValue(this.f2501);
      this.cpfFormControl.setValue(this.dependente.deddepenCpfdep);
      this.tipoRendimentoFormControl.setValue(this.dependente.deddepenTprend);
      this.deddepenVlrdeducaoFormControl.setValue(
        this.dependente.deddepenVlrdeducao
      );
      this.logCodUsuarioFormControl.setValue(this.dependente.logCodUsuario);
      this.logDataOperacaoFormControl.setValue(this.dependente.logDataOperacao);
      

      // if (this.cpfTrabalhador != ''){
      //   this.cpfFormControl.setValidators([Validators.required, this.customValidators.cpfDuplicado(this.cpfTrabalhador)]);
      // }

      if (!this.temPermissaoBlocoCeDeE) {
        this.idFormControl.disable();
        this.idFormularioFormControl.disable();
        this.cpfFormControl.disable();
        this.tipoRendimentoFormControl.disable();
        this.deddepenVlrdeducaoFormControl.disable();
        this.logCodUsuarioFormControl.disable();
        this.logDataOperacaoFormControl.disable();
      }
    }

    this.codigoIrrfFormControl.setValue(this.codIrrf);
  }

  async salvar() {
    this.cpfFormControl;

    try {
      if (this.dependente) {
        var obj = this.formGroup.value;
        obj.deddepenCpfdep = obj.deddepenCpfdep.replace(/[.-]/g, "")
        await this.service.alterar(this.f2501, this.idFormControl.value, this.codIrrf, obj);
      } else {
        var obj = this.formGroup.value;
        obj.deddepenCpfdep = obj.deddepenCpfdep.replace(/[.-]/g, "")
        await this.service.incluir(this.f2501, this.codIrrf, obj);
      }

      await this.dialogService.alert(`${this.titulo == 'Incluir' ? 'Inclusão': 'Alteração'} realizada com sucesso`);
      this.modal.close(true);
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
        await this.dialogService.err(
          'Desculpe, a operação não poderá ser realizada',
          mensagem
        );
    }
  }

  static exibeModal(dependente: DeduçoesDependente, f2501 : number, codIrrf : number, temPermissaoBlocoCeDeE : boolean): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      DedDepenModal_v1_2_Component,
      { windowClass: 'modal-dependente', centered: true, size: 'lg', backdrop: 'static' }
    );
    modalRef. componentInstance.dependente = dependente;
    modalRef.componentInstance.titulo = dependente == null ? 'Incluir' : temPermissaoBlocoCeDeE ? 'Alterar' : 'Consultar'
    modalRef.componentInstance.f2501 = f2501;
    modalRef.componentInstance.codIrrf = codIrrf;
    modalRef.componentInstance.temPermissaoBlocoCeDeE = temPermissaoBlocoCeDeE;
    return modalRef.result;
  }

  close(): void {
    this.modal.close(false);
  }

  async obterTipoRendimento() {
    const resposta = await this.serviceTpRendimento.obterTipoRendimentoAsync();
    if (resposta)
    {
      this.listaTipoRendimento = resposta.map(rendimento => {
        return ({
          id: rendimento.id,
          descricao: rendimento.descricao,
          descricaoConcatenada: `${rendimento.id} - ${rendimento.descricao}`
        })
      });

      //this.listaTipoRendimento = this.listaTipoRendimento.filter(x => x.id == 11 || x.id == 12); //11 - Remuneração mensal, 12 - 13º salário
    }
  }

  tipoRendimentoChange(){
   
  }


  cpfChange(){
    this.cpfFormControl.setValidators([Validators.required, this.customValidators.cpfValido()]);
    this.cpfFormControl.updateValueAndValidity();
  }

  desabilitaTooltip(control: FormControl):boolean{
    return control.valid || control.untouched || control.disabled
  }


}
