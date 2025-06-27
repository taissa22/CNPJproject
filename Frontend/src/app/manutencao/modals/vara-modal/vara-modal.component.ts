import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { StaticInjector } from '@manutencao/static-injector';
import { DialogService } from '@shared/services/dialog.service';
import { Comarca } from '@manutencao/models/comarca.model';
import { HttpErrorResult } from '@core/http';
import { TipoDeVaraService } from '@manutencao/services/tipo-de-vara.service';
import { TipoDeVara } from '@manutencao/models/tipo-de-vara';
import { Vara } from '@manutencao/models/vara.model';
import { TribunalBB } from '@manutencao/models/tribunal-bb.model';
import { TribunalBBService } from '@manutencao/services/tribunal-bb.service';
import { OrgaoBB } from '@manutencao/models/orgao_bb.model';
import { OrgaoBBService } from '@manutencao/services/orgao-bb.service';
import { VaraService } from '@manutencao/services/vara.service';

@Component({
  selector: 'app-vara-modal',
  templateUrl: './vara-modal.component.html',
  styleUrls: ['./vara-modal.component.scss']

})
export class VaraModalComponent implements OnInit {

  vara: Vara;
  comarca: Comarca = null;
  operacaoTitulo: string = "";
  tiposDeVaras: Array<TipoDeVara> = [];
  tribunaisBB: Array<TribunalBB> = [];
  orgaosBB: Array<OrgaoBB> = [];
  placeholderOrgaoBB = "";
  placeholderTribunalBB = 'Selecione uma opção';

  varaIdFormControl: FormControl = new FormControl(null, [Validators.maxLength(3), Validators.required]);
  tipoVaraFormControl: FormControl = new FormControl(null, Validators.required);
  enderecoFormControl: FormControl = new FormControl(null);
  orgaoBBFormControl: FormControl = new FormControl(null,Validators.required);
  tribunalBBFormControl = new FormControl(null);

  formGroup: FormGroup = new FormGroup({
    VaraId: this.varaIdFormControl,
    TipoVaraId: this.tipoVaraFormControl,
    Endereco: this.enderecoFormControl,
    OrgaoBBId: this.orgaoBBFormControl,
    TribunalBBId: this.tribunalBBFormControl
  });

  constructor(
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private tipoDeVaraService: TipoDeVaraService,
    private tribunalBBService: TribunalBBService,
    private orgaoBBService: OrgaoBBService,
    private varaService: VaraService
  ) { }

  async ngOnInit() {
    this.buscarTipoVara();
    this.inicializaDadosForm();
    this.buscarTribunalBB()
  }

  inicializaDadosForm() {
    if(!this.comarca.comarcaBBId) {
      this.desabilitaTribunalBBFormControl();
      this.desabilitaOrgaoBBFormControl();
    }
    if (this.vara) {
      this.varaIdFormControl.setValue(this.vara.varaId);
      this.varaIdFormControl.disable();
      this.tipoVaraFormControl.setValue(this.vara.tipoVaraId);
      this.tipoVaraFormControl.disable();
      this.enderecoFormControl.setValue(this.vara.endereco);
      if (this.vara.orgaoBB) {
        this.tribunalBBFormControl.setValue(this.vara.orgaoBB.tribunalBBId);
        this.burcarOrgaoBB();
      }else{
        this.desabilitaOrgaoBBFormControl();
      }
      this.orgaoBBFormControl.setValue(this.vara.orgaoBBId);
    }else{
      this.desabilitaOrgaoBBFormControl();
    }
  }

  buscarTipoVara() {
    this.tipoDeVaraService.obterTodos().subscribe(res => this.tiposDeVaras = res);
  }

  buscarTribunalBB() {
    this.tribunalBBService.obterTodos().subscribe(res => this.tribunaisBB = res)
  }

  burcarOrgaoBB() {
    this.orgaoBBService.obterPorTribunal(this.tribunalBBFormControl.value,this.comarca.comarcaBBId).subscribe(res => {
      this.orgaosBB = res
    });
  }

  desabilitaTribunalBBFormControl(){
    this.tribunalBBFormControl.disable();
    this.placeholderTribunalBB = 'Nenhuma Comarca BB selecionada';
  }

  desabilitaOrgaoBBFormControl(){
    this.orgaoBBFormControl.disable();
    this.placeholderOrgaoBB = 'Selecione um Tribunal de Justiça BB';
  }

  habilitaOrgaoBBFormControl(){
    this.orgaoBBFormControl.enable();
    this.placeholderOrgaoBB = 'Selecione uma opção';
  }

  tipoDeOperacao(): string {
    return this.vara ? 'Alteração' : 'Inclusão';
  }

  async salvar(): Promise<void> {

    if (!this.formValido()) return;

    try {

      if (!this.vara) {
        await this.criar();
      } else {
        await this.Atualizar();
      }

      await this.dialogService.alert(`${this.tipoDeOperacao()} realizada com sucesso`);
      this.modal.close(true);

    } catch (error) {

      await this.dialogService.err(`${this.tipoDeOperacao()} não realizada`, (error as HttpErrorResult).messages.join('\n'));
      console.log(error);
    }

  }

  formValido() {

    if (this.varaIdFormControl.value && !this.varaIdFormControl.value.toString().replace(/\s/g, '').length) {
      this.dialogService.err(`Desculpe, não foi possivel a ${this.tipoDeOperacao()}`, `O campo vara não pode contar apenas espaços.`);
      return false;
    }
    if (this.varaIdFormControl.value==null || !this.varaIdFormControl.value.toString().replace(/\s/g, '').length) {
      this.dialogService.err(`Desculpe, não foi possivel a ${this.tipoDeOperacao()}`, `O campo vara não pode ser vazio.`);
      return false;
    }
    if (this.varaIdFormControl.value && this.varaIdFormControl.value.length > 3) {
      this.dialogService.err(`Desculpe, não foi possivel a ${this.tipoDeOperacao()}`, `O campo vara não pode conter mais que 3 caracteres.`);
      return false;
    }
    if (this.enderecoFormControl.value && this.enderecoFormControl.value.length > 50) {
      this.dialogService.err(`Desculpe, não foi possivel a ${this.tipoDeOperacao()}`, `O campo endereço não conter mais que 50 caracteres.`);
      return false;
    }
    return true;

  }

  private montarObjParaOBackEnd(){
    let obj: any = this.formGroup.value;
    obj.ComarcaId = this.comarca.id;
    obj.VaraId = this.varaIdFormControl.value;
    obj.TipoVaraId = this.tipoVaraFormControl.value;
    obj.OrgaoBBId = this.orgaoBBFormControl.value;
    obj.TribunalBBId = this.tribunalBBFormControl.value;
    return obj;
  }

  async criar() {
    await this.varaService.criar(this.montarObjParaOBackEnd());
  }

  async Atualizar() {
    await this.varaService.atualizar(this.montarObjParaOBackEnd());
  }

  aoMudarTribunalDeJustica(){
    this.orgaoBBFormControl.setValue(null); 
    if(this.tribunalBBFormControl.value) this.habilitaOrgaoBBFormControl();
    else this.desabilitaOrgaoBBFormControl();
    this.burcarOrgaoBB();
  }
  
  desabilitaTooltip(formControl: FormControl): boolean {
    return formControl.untouched || formControl.valid;
  }

  //#region MODAL

  static exibeModalDeIncluir(comarca: Comarca): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(VaraModalComponent, { centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.operacaoTitulo = 'Incluir';
    modalRef.componentInstance.comarca = comarca;

    return modalRef.result;
  }

  static exibeModalDeAlterar(vara: Vara, comarca: Comarca): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(VaraModalComponent, { centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.vara = vara;
    modalRef.componentInstance.operacaoTitulo = 'Alterar';
    modalRef.componentInstance.comarca = comarca;
    return modalRef.result;
  }

  close(): void {
    this.modal.close(false);
  }

  //#endregion MODAL
}


