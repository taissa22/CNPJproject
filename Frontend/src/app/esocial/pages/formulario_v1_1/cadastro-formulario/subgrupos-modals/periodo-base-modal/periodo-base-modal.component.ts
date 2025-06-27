import { AfterViewInit, Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ErrorLib } from '@esocial/libs/error-lib';
import { PeriodoBase } from '@esocial/models/subgrupos/periodo-base';
import { PeriodoBaseService } from '@esocial/services/formulario_v1_1/subgrupos/periodo-base.service';
import { StaticInjector } from '@esocial/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';

@Component({
  selector: 'app-periodo-base-modal',
  templateUrl: './periodo-base-modal.component.html',
  styleUrls: ['./periodo-base-modal.component.scss']
})
export class PeriodoBaseModalComponent implements AfterViewInit {

  constructor(
    private modal: NgbActiveModal,
    private service: PeriodoBaseService,
    private dialog: DialogService,
  ) { }

  ngAfterViewInit(): void {
  }

  temPermissaoBlocoCeDeE: boolean = false;
  titulo: string;
  codF2501: number;
  codCalTrib: number;
  periodoBase: PeriodoBase;
  regex12_2 = /^([0-9]{1,12}\,[0-9]{2})/g;
  mesAtual: Date = new Date(new Date().getFullYear(), new Date().getMonth(), 1);

  //#region MÉTODOS
  async obterPeriodoBase() {
    try {
      const resposta = await this.service.obterPeriodoBase(this.codF2501, this.codCalTrib);
      if (resposta) {
        this.periodoBase = resposta;
        this.iniciarForm();
      } else {
        await this.dialog.err(
          'Informações não carregadas',
          'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte'
        );
      }
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialog.err(`Desculpe, não foi possivel obter os registros`, mensagem)
    }
  }

  async salvar() {
    let operacao = this.titulo == 'Alterar' ? 'Alteração' : 'Inclusão';

    try {
      this.formGroup.markAllAsTouched();
      this.calctribVrbccpmensalFormControl.value;
      if (this.formGroup.invalid) { return; }

      if (this.titulo == "Alterar") {
        this.calctribPerrefFormControl.setValue(
          new Date(new Date(this.calctribPerrefFormControl.value).getFullYear(), new Date(this.calctribPerrefFormControl.value).getMonth(), (new Date(this.calctribPerrefFormControl.value).getDate() + 1))
          );
        console.log(this.formGroup.value,)
        await this.service.atualizar(this.periodoBase.idEsF2501, this.periodoBase.idEsF2501Calctrib, this.formGroup.value);
      } else {
        await this.service.incluir(this.codF2501, this.formGroup.value);
      }

      await this.dialog.alert('Operação realizada', `${operacao} realizada com sucesso`);
      this.modal.close(true);

    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialog.err(`Desculpe, não foi possivel realizar a ${operacao}`, mensagem)
    }
  }

  //#endregion


  static exibeModalAlterar(codF2501: number, codCalTrib: number, temPermissaoBlocoCeDeE: boolean): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      PeriodoBaseModalComponent,
      { windowClass: 'modal-periodo-base', centered: true, size: 'lg', backdrop: 'static' }
    );

    modalRef.componentInstance.codF2501 = codF2501;
    modalRef.componentInstance.codCalTrib = codCalTrib;
    modalRef.componentInstance.temPermissaoBlocoCeDeE = temPermissaoBlocoCeDeE;
    modalRef.componentInstance.titulo = temPermissaoBlocoCeDeE ? 'Alterar' : 'Consultar';
    modalRef.componentInstance.obterPeriodoBase();
    return modalRef.result;
  }

  static exibeModalIncluir(codF2501: number): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      PeriodoBaseModalComponent,
      { windowClass: 'modal-periodo-base', centered: true, size: 'lg', backdrop: 'static' }
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

  calctribPerrefFormControl: FormControl = new FormControl(null);
  calctribVrbccpmensalFormControl: FormControl = new FormControl(null, [Validators.required]);
  calctribVrbccp13FormControl: FormControl = new FormControl(null, [Validators.required]);
  calctribVrrendirrfFormControl: FormControl = new FormControl(null, [Validators.required]);
  calctribVrrendirrf13FormControl: FormControl = new FormControl(null, [Validators.required]);

  formGroup: FormGroup = new FormGroup({
    calctribPerref: this.calctribPerrefFormControl,
    calctribVrbccpmensal: this.calctribVrbccpmensalFormControl,
    calctribVrbccp13: this.calctribVrbccp13FormControl,
    calctribVrrendirrf: this.calctribVrrendirrfFormControl,
    calctribVrrendirrf13: this.calctribVrrendirrf13FormControl,
  });

  iniciarForm() {
    if (this.codCalTrib > 0) {
      this.calctribPerrefFormControl.setValue(this.periodoBase.calctribPerref == null ? null : new Date(this.periodoBase.calctribPerref));
      this.calctribVrbccpmensalFormControl.setValue(this.periodoBase.calctribVrbccpmensal);
      this.calctribVrbccp13FormControl.setValue(this.periodoBase.calctribVrbccp13);
      this.calctribVrrendirrfFormControl.setValue(this.periodoBase.calctribVrrendirrf);
      this.calctribVrrendirrf13FormControl.setValue(this.periodoBase.calctribVrrendirrf13);

      if (!this.temPermissaoBlocoCeDeE) {
        this.calctribPerrefFormControl.disable();
        this.calctribVrbccpmensalFormControl.disable();
        this.calctribVrbccp13FormControl.disable();
        this.calctribVrrendirrfFormControl.disable();
        this.calctribVrrendirrf13FormControl.disable();
      }
    }
  }

  desabilitaTooltip(control: FormControl):boolean{
    return control.valid || control.untouched || control.disabled
  }
}
