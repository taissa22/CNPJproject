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
  selector: 'app-info-deducoes-isencoes-modal_v1_2',
  templateUrl: './info-deducoes-isencoes-modal_v1_2.component.html',
  styleUrls: ['./info-deducoes-isencoes-modal_v1_2.component.scss']
})
export class InfoDeducoesIsencoesModal_v1_2Component implements AfterViewInit {
  constructor(
    private modal: NgbActiveModal,
    private service: ImpostoService,
    private serviceList: ESocialListaFormularioService,
    private dialog: DialogService
  ) {}

  ngAfterViewInit(): void {
    this.obterCodigoReceita();
    this.validaRendimetoTributavel();
    this.validaOutrosRendimentos();
  }

  temPermissaoBlocoCeDeE: boolean = false;
  titulo: string;
  codF2501: number;
  codIrrf: number;
  codReceita: string;
  codReceitaId: string;
  valorIrrf: number;
  valorIrrf13: number;
  imposto: Imposto;
  codigoReceitaList = [];
  regex12_2 = /^([0-9]{1,10}\,[0-9]{2})/g;
  descCount = 0;

  isInputDisabled: boolean = true;
  inputValorDisabled: boolean = true;

  ngOnInit(): void {
    this.updateInputDisabled();
  }

  //#region MÉTODOS
  async obterImposto() {
    const response = await this.service.obterImpostoTribtaveis(
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
          descricaoConcatenada: `${cr.id} - ${cr.descricao}`
        };
      });
      // this.iniciarForm();
    }
  }

  async salvar() {
    this.formGroup.markAllAsTouched();
    if (this.formGroup.invalid) {
      return;
    }
    let operacao = this.titulo == 'Alterar' ? 'Alteração' : 'Inclusão';
    let valueSubmit = this.formGroup.value;
    if (valueSubmit.infoirVrrendtrib != null) {
      valueSubmit.infoirVrrendtrib = Number(valueSubmit.infoirVrrendtrib);
    }
    if (valueSubmit.infoirVrrendtrib13 != null) {
      valueSubmit.infoirVrrendtrib13 = Number(valueSubmit.infoirVrrendtrib13);
    }
    if (valueSubmit.infoirVrrendmolegrave != null) {
      valueSubmit.infoirVrrendmolegrave = Number(
        valueSubmit.infoirVrrendmolegrave
      );
    }
    if (valueSubmit.infoirVrrendisen65 != null) {
      valueSubmit.infoirVrrendisen65 = Number(valueSubmit.infoirVrrendisen65);
    }
    if (valueSubmit.infoirVrjurosmora != null) {
      valueSubmit.infoirVrjurosmora = Number(valueSubmit.infoirVrjurosmora);
    }
    if (valueSubmit.infoirVrrendisenntrib != null) {
      valueSubmit.infoirVrrendisenntrib = Number(
        valueSubmit.infoirVrrendisenntrib
      );
    }
    if (valueSubmit.infoirVrprevoficial != null) {
      valueSubmit.infoirVrprevoficial = Number(valueSubmit.infoirVrprevoficial);
    }

    try {
      await this.service.atualizarTributaveis(
        this.codF2501,
        this.codIrrf,
        valueSubmit
      );

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
    codReceita: string,
    valorIrrf: number,
    valorIrrf13: number,
    temPermissaoBlocoCeDeE: boolean
  ): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      InfoDeducoesIsencoesModal_v1_2Component,
      {
        windowClass: 'modal-imposto',
        centered: true,
        size: 'lg',
        backdrop: 'static'
      }
    );
    modalRef.componentInstance.codF2501 = codF2501;
    modalRef.componentInstance.codIrrf = codIrrf;
    modalRef.componentInstance.codReceita = codReceita;
    modalRef.componentInstance.valorIrrf = valorIrrf;
    modalRef.componentInstance.valorIrrf13 = valorIrrf13;
    modalRef.componentInstance.temPermissaoBlocoCeDeE = temPermissaoBlocoCeDeE;
    modalRef.componentInstance.titulo = temPermissaoBlocoCeDeE
      ? 'Alterar'
      : 'Consultar';
    modalRef.componentInstance.obterImposto();
    return modalRef.result;
  }

  static exibeModalIncluir(codF2501: number): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      InfoDeducoesIsencoesModal_v1_2Component,
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

  // infocrcontribTpcrFormControl: FormControl = new FormControl(null);
  // infocrcontribVrcrFormControl: FormControl = new FormControl(null);
  // descricaoFormControl: FormControl = new FormControl(null);

  infoirVrrendtribFormControl: FormControl = new FormControl(null); //rendimento tributável mensal
  infoirVrrendtrib13FormControl: FormControl = new FormControl(null); // rendimento tributável do Imposto de Renda referente ao 13º salário
  infoirVrrendmolegraveFormControl: FormControl = new FormControl(null); //rendimento isento por ser portador de moléstia grave
  infoirVrrendmolegrave13FormControl: FormControl = new FormControl(null); //rendimento isento por ser portador de moléstia grave 13º salário
  infoirVrrendisen65FormControl: FormControl = new FormControl(null); //Parcela Isenta Aposentadoria (65+)
  infoirVrrendisen65decFormControl: FormControl = new FormControl(null); //Parcela Isenta Aposentadoria (65+) (13º)
  infoirVrjurosmoraFormControl: FormControl = new FormControl(null); //Juros Mora (por atraso pagamento):
  infoirVrjurosmora13FormControl: FormControl = new FormControl(null); //Juros Mora (por atraso pagamento):(13º)
  infoirVrrendisenntribFormControl: FormControl = new FormControl(null); //Outros Rendimentos Isentos ou Não Tributáveis(Valor)
  infoirDescisenntribFormControl: FormControl = new FormControl(null); //Outros Rendimentos Isentos ou Não Tributáveis(Descrição)
  infoirVrprevoficialFormControl: FormControl = new FormControl(null); // Previdência Oficial
  infoirVrprevoficial13FormControl: FormControl = new FormControl(null); // Previdência Oficial (13º)

  // CR 0561
  infoirVlrDiariasFormControl: FormControl = new FormControl(null);
  infoirVlrAjudaCustoFormControl: FormControl = new FormControl(null);
  infoirVlrIndResContratoFormControl: FormControl = new FormControl(null);
  infoirVlrAbonoPecFormControl: FormControl = new FormControl(null);
  infoirVlrAuxMoradiaFormControl: FormControl = new FormControl(null);

  formGroup: FormGroup = new FormGroup({
    infoirVrrendtrib: this.infoirVrrendtribFormControl,
    infoirVrrendtrib13: this.infoirVrrendtrib13FormControl,
    infoirVrrendmolegrave: this.infoirVrrendmolegraveFormControl,

    infoirVrrendisen65: this.infoirVrrendisen65FormControl,
    infoirVrjurosmora: this.infoirVrjurosmoraFormControl,
    infoirVrrendisenntrib: this.infoirVrrendisenntribFormControl,
    infoirDescisenntrib: this.infoirDescisenntribFormControl,
    infoirVrprevoficial: this.infoirVrprevoficialFormControl,

    infoirVrrendmolegrave13: this.infoirVrrendmolegrave13FormControl,
    infoirRrendisen65dec: this.infoirVrrendisen65decFormControl,
    infoirVrjurosmora13: this.infoirVrjurosmora13FormControl,
    infoirVrprevoficial13: this.infoirVrprevoficial13FormControl,

    infoirVlrDiarias: this.infoirVlrDiariasFormControl,
    infoirVlrAjudaCusto: this.infoirVlrAjudaCustoFormControl,
    infoirVlrIndResContrato: this.infoirVlrIndResContratoFormControl,
    infoirVlrAbonoPec: this.infoirVlrAbonoPecFormControl,
    infoirVlrAuxMoradia: this.infoirVlrAuxMoradiaFormControl
  });

  iniciarForm() {
    if (this.codIrrf > 0) {
      this.infoirVrrendtribFormControl.setValue(this.imposto.infoirVrrendtrib);
      this.infoirVrrendtrib13FormControl.setValue(
        this.imposto.infoirVrrendtrib13
      );
      this.infoirVrrendmolegraveFormControl.setValue(
        this.imposto.infoirVrrendmolegrave
      );

      this.infoirVrrendisen65FormControl.setValue(
        this.imposto.infoirVrrendisen65
      );
      this.infoirVrjurosmoraFormControl.setValue(
        this.imposto.infoirVrjurosmora
      );

      this.infoirVrrendisenntribFormControl.setValue(
        this.imposto.infoirVrrendisenntrib
      );
      this.infoirDescisenntribFormControl.setValue(
        this.imposto.infoirDescisenntrib
      );
      this.infoirVrprevoficialFormControl.setValue(
        this.imposto.infoirVrprevoficial
      );

      this.infoirVrrendmolegrave13FormControl.setValue(
        this.imposto.infoirVrrendmolegrave13
      );
      this.infoirVrrendisen65decFormControl.setValue(
        this.imposto.infoirRrendisen65dec
      );
      this.infoirVrjurosmora13FormControl.setValue(
        this.imposto.infoirVrjurosmora13
      );
      this.infoirVrprevoficial13FormControl.setValue(
        this.imposto.infoirVrprevoficial13
      );

      this.infoirVlrDiariasFormControl.setValue(this.imposto.infoirVlrDiarias);
      this.infoirVlrAjudaCustoFormControl.setValue(
        this.imposto.infoirVlrAjudaCusto
      );
      this.infoirVlrIndResContratoFormControl.setValue(
        this.imposto.infoirVlrIndResContrato
      );
      this.infoirVlrAbonoPecFormControl.setValue(
        this.imposto.infoirVlrAbonoPec
      );
      this.infoirVlrAuxMoradiaFormControl.setValue(
        this.imposto.infoirVlrAuxMoradia
      );
    }
    this.validaRendimetoTributavel();
    this.validaOutrosRendimentos();
    this.countDesc();
    this.clearCr0561();

    if (!this.temPermissaoBlocoCeDeE) {
      this.infoirVrrendtribFormControl.disable();
      this.infoirVrrendtrib13FormControl.disable();
      this.infoirVrrendmolegraveFormControl.disable();
      this.infoirVrrendmolegrave13FormControl.disable();
      this.infoirVrrendisen65FormControl.disable();
      this.infoirVrrendisen65decFormControl.disable();
      this.infoirVrjurosmoraFormControl.disable();
      this.infoirVrjurosmora13FormControl.disable();
      this.infoirVrrendisenntribFormControl.disable();
      this.infoirDescisenntribFormControl.disable();
      this.infoirVrprevoficialFormControl.disable();
      this.infoirVrprevoficial13FormControl.disable();
    }

    if (this.inputValorDisabled || !this.temPermissaoBlocoCeDeE) {
      // this.infoirVrrendisenntribFormControl.setValue(null);
      this.infoirVrrendisenntribFormControl.disable();
    }

    if (this.isInputDisabled) {
      this.infoirVlrDiariasFormControl.disable();
      this.infoirVlrAjudaCustoFormControl.disable();
      this.infoirVlrIndResContratoFormControl.disable();
      this.infoirVlrAbonoPecFormControl.disable();
      this.infoirVlrAuxMoradiaFormControl.disable();
    }
  }

  desabilitaTooltip(control: FormControl): boolean {
    return control.valid || control.untouched || control.disabled;
  }

  clearCr0561() {
    if (this.isInputDisabled) {
      this.infoirVlrDiariasFormControl.setValue(null);
      this.infoirVlrAjudaCustoFormControl.setValue(null);
      this.infoirVlrIndResContratoFormControl.setValue(null);
      this.infoirVlrAbonoPecFormControl.setValue(null);
      this.infoirVlrAuxMoradiaFormControl.setValue(null);
    }
  }

  countDesc() {
    this.descCount = this.infoirDescisenntribFormControl.value
      ? this.infoirDescisenntribFormControl.value.length
      : 0;
  }

  validaRendimetoTributavel() {
    let result = this.codReceita.includes('188951');

    if (result) {
      this.infoirVrrendtrib13FormControl.setValue(null);
      this.infoirVrrendtrib13FormControl.disable();
    } else {
      this.infoirVrrendtrib13FormControl.enable();
    }
  }

  validaOutrosRendimentos() {
    if (
      !this.infoirVrrendisenntribFormControl.value ||
      (this.infoirVrrendisenntribFormControl.value &&
        this.infoirVrrendisenntribFormControl.value < 0)
    ) {
      this.infoirDescisenntribFormControl.setValue(null);
      this.infoirDescisenntribFormControl.disable();
      this.infoirDescisenntribFormControl.setValidators(Validators.required);
    } else {
      this.infoirDescisenntribFormControl.enable();
      this.infoirDescisenntribFormControl.clearValidators();
    }
  }

  validaCamposBtn() {
    if (
      this.infoirVrrendtribFormControl.value != null ||
      this.infoirVrrendtrib13FormControl.value != null ||
      this.infoirVrrendmolegraveFormControl.value != null ||
      this.infoirVrrendmolegrave13FormControl.value != null ||
      this.infoirVrrendisen65FormControl.value != null ||
      this.infoirVrrendisen65decFormControl.value != null ||
      this.infoirVrjurosmoraFormControl.value != null ||
      this.infoirVrjurosmora13FormControl.value != null ||
      this.infoirVrrendisenntribFormControl.value != null ||
      this.infoirDescisenntribFormControl.value != null ||
      this.infoirVrprevoficialFormControl.value != null ||
      this.infoirVrprevoficial13FormControl.value != null ||
      this.infoirVlrDiariasFormControl.value != null ||
      this.infoirVlrAjudaCustoFormControl.value != null ||
      this.infoirVlrIndResContratoFormControl.value != null ||
      this.infoirVlrAbonoPecFormControl.value != null ||
      this.infoirVlrAuxMoradiaFormControl.value != null
    ) {
      return false;
    } else {
      return true;
    }
  }

  updateInputDisabled(): void {
    const codReceitaValue = this.codReceita
      ? this.codReceita.split(' - ')[0]
      : '';
    this.isInputDisabled = codReceitaValue != '056152';
    this.inputValorDisabled = codReceitaValue == '188951';
  }
  //#endregion
}
