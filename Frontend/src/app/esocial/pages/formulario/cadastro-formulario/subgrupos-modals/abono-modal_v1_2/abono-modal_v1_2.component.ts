import { AfterContentInit, AfterViewInit, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ErrorLib } from '@esocial/libs/error-lib';
import { Abono } from '@esocial/models/subgrupos/v1_2/abono';
import { AbonoService } from '@esocial/services/formulario/subgrupos/abono.service';
import { StaticInjector } from '@esocial/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';

@Component({
  selector: 'app-abono-modal_v1_2',
  templateUrl: './abono-modal_v1_2.component.html',
  styleUrls: ['./abono-modal_v1_2.component.scss']
})
export class AbonoModal_v1_2Component implements AfterViewInit {
  constructor(
    private modal: NgbActiveModal,
    private service: AbonoService,
    // private serviceList: ESocialListaFormularioService,
    private dialog: DialogService,
  ) { }

  ngAfterViewInit(): void {
    // this.obterCodigoReceita();
  }

  temPermissaoEsocialBlocoJValores: boolean = false;
  titulo: string;
  codContrato: number;
  codAbono: number;
  codF2500:number;
  abonoAnobase:string;
  abono: Abono;
  codigoReceitaList = [];
  regex12_2 = /^([0-9]{1,10}\,[0-9]{2})/g;

  //#region MÉTODOS
  async obterAbono() {
    const response = await this.service.obterAbono(this.codContrato, this.codAbono);
    if (response){
          this.abono = response;
          this.iniciarForm();
    }
  }

  async salvar() {
    let operacao = this.titulo == 'Alterar' ? 'Alteração' : 'Inclusão';
    let valueSubmit = this.formGroup.value
    valueSubmit.idEsF2500Abono = this.codAbono;
    try {
      if (this.titulo == "Alterar") {
        await this.service.atualizar(this.codF2500, this.codContrato, valueSubmit);
      } else {
        await this.service.incluir(this.codF2500, this.codContrato, valueSubmit);
      }

      await this.dialog.alert('Operação realizada', `${operacao} realizada com sucesso`);
      this.modal.close(true);

    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialog.err(`Desculpe, não foi possivel realizar a ${operacao}`,mensagem)
    }
  }

  //#endregion


  static exibeModalAlterar(codF2500: number, codContrato: number, codAbono: number, abonoAnobase: string, temPermissaoEsocialBlocoJValores: boolean): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      AbonoModal_v1_2Component,
      { windowClass: 'modal-abono', centered: true, size: 'lg', backdrop: 'static' }
    );
    modalRef.componentInstance.codF2500 = codF2500;
    modalRef.componentInstance.codAbono = codAbono;
    modalRef.componentInstance.codContrato = codContrato;
    modalRef.componentInstance.abonoAnobase = abonoAnobase;
    modalRef.componentInstance.temPermissaoEsocialBlocoJValores = temPermissaoEsocialBlocoJValores;
    modalRef.componentInstance.titulo = temPermissaoEsocialBlocoJValores ? 'Alterar' : 'Consultar';
    modalRef.componentInstance.iniciarForm();
    return modalRef.result;
  }

  static exibeModalConsultar(codContrato: number, codAbono: number, temPermissaoEsocialBlocoJValores: boolean): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      AbonoModal_v1_2Component,
      { windowClass: 'modal-abono', centered: true, size: 'lg', backdrop: 'static' }
    );
    modalRef.componentInstance.codAbono = codAbono;
    modalRef.componentInstance.codContrato = codContrato;
    modalRef.componentInstance.temPermissaoEsocialBlocoJValores = temPermissaoEsocialBlocoJValores;
    modalRef.componentInstance.titulo = 'Consultar';
    modalRef.componentInstance.obterAbono();
    return modalRef.result;
  }

  static exibeModalIncluir(codF2500: number, codContrato: number): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      AbonoModal_v1_2Component,
      { windowClass: 'modal-abono', centered: true, size: 'lg', backdrop: 'static' }
    );
    modalRef.componentInstance.codF2500 = codF2500;
    modalRef.componentInstance.codContrato = codContrato;    
    modalRef.componentInstance.temPermissaoEsocialBlocoJValores = true;
    modalRef.componentInstance.titulo = 'Incluir';
    modalRef.componentInstance.obterAbono();
    return modalRef.result;
  }

  close(): void {
    this.modal.close(false);
  }

  //#region FORMCONTROL

  anoAbonoFormControl: FormControl = new FormControl(null);
  idEsF2500AbonoFormControl: FormControl = new FormControl(null);

  formGroup: FormGroup = new FormGroup({
    abonoAnobase: this.anoAbonoFormControl,
    idEsF2500Abono: this.idEsF2500AbonoFormControl
  });

  iniciarForm() {
    if(this.codAbono > 0){
      if (this.abonoAnobase) {
        this.anoAbonoFormControl.setValue(this.abonoAnobase);
      }else{
        if (this.abono) {
          this.anoAbonoFormControl.setValue(this.abono.abonoAnobase);
        }else{
          this.anoAbonoFormControl.setValue(null);
        }      
      }
      if (!this.temPermissaoEsocialBlocoJValores) {
        this.anoAbonoFormControl.disable();
      }
    }else{
      if (this.abonoAnobase) {
        this.anoAbonoFormControl.setValue(this.abonoAnobase);
      }else{
        this.anoAbonoFormControl.setValue(this.abono.abonoAnobase);
      }
    }
  }

  desabilitaTooltip(control: FormControl):boolean{
    return control.valid || control.untouched || control.disabled
  }

  validarAno(input) {
    this.anoAbonoFormControl.setValue(input.replace(/\D/g, ''))

    if ( input.value && input.value.length > 4) {
      this.anoAbonoFormControl.setValue(input.slice(0, 4));
    }
}
  //#endregion
}
