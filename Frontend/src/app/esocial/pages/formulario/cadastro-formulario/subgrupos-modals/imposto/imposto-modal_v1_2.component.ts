import { AfterViewInit, Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ErrorLib } from '@esocial/libs/error-lib';
import { Imposto } from '@esocial/models/subgrupos/v1_2/imposto';
import { ESocialListaFormularioService } from '@esocial/services/aplicacao/e-social-lista-formulario.service';
import { ImpostoService } from '@esocial/services/formulario/subgrupos/imposto.service';
import { StaticInjector } from '@esocial/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';

@Component({
  selector: 'app-imposto-modal-v1-2',
  templateUrl: './imposto-modal_v1_2.component.html',
  styleUrls: ['./imposto-modal_v1_2.component.scss']
})
export class ImpostoModal_v1_2_Component implements AfterViewInit {
  constructor(
    private modal: NgbActiveModal,
    private service: ImpostoService,
    private serviceList: ESocialListaFormularioService,
    private dialog: DialogService
  ) {}

  ngAfterViewInit(): void {
    this.obterCodigoReceita();
    this.adicionaValidators();
  }

  temPermissaoBlocoCeDeE: boolean = false;
  titulo: string;
  codF2501: number;
  codIrrf: number;
  imposto: Imposto;
  codigoReceitaList = [];
  regex12_2 = /^([0-9]{1,10}\,[0-9]{2})/g;

  //#region MÉTODOS
  async obterImposto() {
    const response = await this.service.obterImposto(
      this.codF2501,
      this.codIrrf
    );
    if (response) {
      this.imposto = response;
      this.iniciarForm();
    }
  }

  async obterCodigoReceita() {
    const response = await this.serviceList.obterCodigoReceitaIRRFAsync();
    if (response) {
      this.codigoReceitaList = response.map(cr => {
        return {
          id: cr.id,
          descricao: cr.descricao,
          descricaoConcatenada: `${cr.id.toString().padStart(6, '0')} - ${cr.descricao}`
        };
      });
      this.iniciarForm();
    }
  }

  async salvar() {
    let operacao = this.titulo == 'Alterar' ? 'Alteração' : 'Inclusão';
    let valueSubmit = this.formGroup.value;
    valueSubmit.calctribPerref = Number(valueSubmit.calctribPerref);
    valueSubmit.calctribVrbccpmensal = Number(valueSubmit.calctribVrbccpmensal);

    try {
      if (this.titulo == 'Alterar') {
        await this.service.atualizar(this.codF2501, this.codIrrf, valueSubmit);
      } else {
        await this.service.incluir(this.codF2501, valueSubmit);
      }

      await this.dialog.alert(
        'Operação realizada',
        `${operacao} realizada com sucesso`
      );
      this.modal.close(true);
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialog.err(
        `Desculpe, não foi possivel realizar a ${operacao}`,
        mensagem
      );
    }
  }

  //#endregion

  static exibeModalAlterar(
    codF2501: number,
    codIrrf: number,
    temPermissaoBlocoCeDeE: boolean
  ): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      ImpostoModal_v1_2_Component,
      {
        windowClass: 'modal-imposto',
        centered: true,
        size: 'lg',
        backdrop: 'static'
      }
    );
    modalRef.componentInstance.codF2501 = codF2501;
    modalRef.componentInstance.codIrrf = codIrrf;
    modalRef.componentInstance.temPermissaoBlocoCeDeE = temPermissaoBlocoCeDeE;
    modalRef.componentInstance.titulo = temPermissaoBlocoCeDeE
      ? 'Alterar'
      : 'Consultar';
    modalRef.componentInstance.obterImposto();
    return modalRef.result;
  }

  static exibeModalIncluir(codF2501: number): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      ImpostoModal_v1_2_Component,
      {
        windowClass: 'modal-imposto',
        centered: true,
        size: 'lg',
        backdrop: 'static'
      }
    );
    modalRef.componentInstance.codF2501 = codF2501;
    modalRef.componentInstance.temPermissaoBlocoCeDeE = true;
    modalRef.componentInstance.titulo = 'Incluir';
    modalRef.componentInstance.iniciarForm();
    return modalRef.result;
  }

  close(): void {
    this.modal.close(false);
  }

  //#region FORMCONTROL

  infocrcontribTpcrFormControl: FormControl = new FormControl(null, [
    Validators.required
  ]);
  infocrcontribVrcrFormControl: FormControl = new FormControl(null, [
    Validators.required
  ]);
  infocrcontribVrcr13FormControl: FormControl = new FormControl(null, [
    Validators.min(0.01)
  ]);
  formGroup: FormGroup = new FormGroup({
    infocrcontribTpcr: this.infocrcontribTpcrFormControl,
    infocrcontribVrcr: this.infocrcontribVrcrFormControl,
    infocrcontribVrcr13: this.infocrcontribVrcr13FormControl
  });

  iniciarForm() {
    if (this.codIrrf > 0) {
      this.infocrcontribTpcrFormControl.setValue(
        this.imposto.infocrcontribTpcr.toString().padStart(6, '0')
      );
      this.infocrcontribVrcrFormControl.setValue(
        this.imposto.infocrcontribVrcr
      );
      this.infocrcontribVrcr13FormControl.setValue(
        this.imposto.infocrcontribVrcr13
      );
      if (!this.temPermissaoBlocoCeDeE) {
        this.infocrcontribTpcrFormControl.disable();
        this.infocrcontribVrcrFormControl.disable();
        this.infocrcontribVrcr13FormControl.disable();
      }
    } else {
      this.infocrcontribTpcrFormControl.setValue(null);
      this.infocrcontribVrcrFormControl.setValue(null);
      this.infocrcontribVrcr13FormControl.setValue(null);
    }
  }

  adicionaValidators() {
    if (this.infocrcontribVrcr13FormControl.value == 0.0) {
      this.infocrcontribVrcr13FormControl.setValidators([Validators.min(0.01)]);
      this.infocrcontribVrcr13FormControl.updateValueAndValidity();
      this.infocrcontribVrcr13FormControl.markAsTouched();
    } else {
      this.infocrcontribVrcr13FormControl.clearValidators();
      this.infocrcontribVrcr13FormControl.updateValueAndValidity();
      this.infocrcontribVrcr13FormControl.markAsTouched();
    }
  }

  desabilitaTooltip(control: FormControl): boolean {
    return control.valid || control.untouched || control.disabled;
  }
  //#endregion
}
