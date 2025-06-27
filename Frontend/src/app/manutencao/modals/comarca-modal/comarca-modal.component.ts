// angular
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

// 3rd party
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

// core & shared
import { StaticInjector } from '@manutencao/static-injector';
import { DialogService } from '@shared/services/dialog.service';


import { Comarca } from '@manutencao/models/comarca.model';
import { HttpErrorResult } from '@core/http';
import { Estados } from '@core/models';
import { ComarcaService } from '@manutencao/services/comarca.service';
import { ComarcaBBService } from '@manutencao/services/comarca-bb.service';
import { ComarcaBB } from '@manutencao/models/comarca-bb.model';
import { PadStartPipe } from '@shared/pipes/pad-start.pipe';
import { Vara } from '@manutencao/models/vara.model';
import { Estado } from '@manutencao/models';
import { EstadoService } from '@manutencao/services/estado.service';
@Component({
  selector: 'app-comarca-modal',
  templateUrl: './comarca-modal.component.html',
  styleUrls: ['./comarca-modal.component.scss']

})
export class ComarcaModalComponent implements OnInit {

  operacaoTitulo: string = '';
  comarca: Comarca;
  estados: Array<Estado> = []; //Estados.obterUfs();
  nomeError: any;
  comarcasBB: Array<any> = [];
  placeholderComarcaBB = "";

  estadoIdFormControl: FormControl = new FormControl(null, Validators.required);
  nomeFormControl = new FormControl(null, [Validators.required,Validators.maxLength(30)]);
  comarcaBBIdFormControl = new FormControl(null);

  formGroup: FormGroup = new FormGroup({
    estadoId: this.estadoIdFormControl,
    nome: this.nomeFormControl,
    comarcaBBId: this.comarcaBBIdFormControl
  });

  constructor(
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private service: ComarcaService,
    private serviceComarcaBB: ComarcaBBService,
    private estadoService: EstadoService
  ) { }

  ngOnInit(): void {
     this.estadoService.obterTodos().subscribe(estados => {
      this.estados = estados;
      this.inicializaForm();  
    });      
  }

  inicializaForm() {
    this.desabilitaComarcaBB();
    if (this.comarca) {    
      this.formGroup.addControl('id', new FormControl(this.comarca.id));
      if (this.comarca.estado) this.estadoIdFormControl.setValue(this.comarca.estado.id);
      this.nomeFormControl.setValue(this.comarca.nome);
      if (this.comarca.comarcaBB) this.comarcaBBIdFormControl.setValue(this.comarca.comarcaBB.id);
      this.obterTodosComarcaBB(() => {
        if (this.comarca.varas.find(v => v.orgaoBBId > 0) != undefined) {
          this.desabilitaComarcaBB();
        } else {
          this.habilitaComarcaBB();
        }
      });
    }
  }

  obterTodosComarcaBB(callback?) {
    this.serviceComarcaBB.ObterPorEstado(this.estadoIdFormControl.value).subscribe(comarcasBB => {
      this.comarcasBB = comarcasBB;
      this.criaAtributoBindLabelEmComarcaBB();
      if (callback) callback();
    });
  }

  // atributo criado para ser usado no componente ng-select
  criaAtributoBindLabelEmComarcaBB() {
    let padStartTransform = (new PadStartPipe()).transform;
    this.comarcasBB.map(comarcaBB =>
      comarcaBB.bindLabel = comarcaBB.estadoId + " - " + comarcaBB.nome + " - (" + padStartTransform(comarcaBB.id, 9, 0)) + ")";
  }

  desabilitaComarcaBB() {
    this.comarcaBBIdFormControl.disable()
    this.placeholderComarcaBB = "Selecione um estado";
  }

  habilitaComarcaBB() {
    this.comarcaBBIdFormControl.enable();
    this.placeholderComarcaBB = "selecione uma opção"
  }

  async salvar(): Promise<void> {
    try {
      if (! await this.formValido()) return;

      if (!this.comarca) {
        await this.criar();
      } else {
        await this.atualizar();
      }

      await this.dialogService.alert(`${this.tipoDeOperacao()} realizada com sucesso`);
      this.modal.close(true);

    } catch (error) {
      await this.dialogService.err(`Desculpe, não foi possivel a ${this.tipoDeOperacao()}`, (error as HttpErrorResult).messages.join('\n'));
    }
  }

  private async formValido() {
    if (this.nomeFormControl.value && !this.nomeFormControl.value.replace(/\s/g, '').length) {
      await this.dialogService.err(`Desculpe, não foi possivel a ${this.tipoDeOperacao()}`, `O campo nome não pode contar apenas espaços.`);
      return false;
    }
    if (this.nomeFormControl.value && this.possuiAcentos()) {
       await this.dialogService.info(`Atenção`, `O nome da Comarca será salvo sem acentuação, visando facilitar a pesquisa e evitar a duplicidade de nomes.`);
      return true;
    }
    if (this.nomeFormControl.value && this.nomeFormControl.value.length > 30) {
      await this.dialogService.err(`Desculpe, não foi possivel a ${this.tipoDeOperacao()}`, `O campo nome não pode conter mais que 30 caracteres.`);
      return false;
    }
    if (!this.estadoIdFormControl.value) {
      await  this.dialogService.err(`Desculpe, não foi possivel a ${this.tipoDeOperacao()}`, `O campo estado não pode ser vazio.`);
      return false;
    }
    return true;
  }

  geraObjetoParaOBackEnd() {
    let obj = this.formGroup.value;
    obj.nome = obj.nome.toUpperCase();
    obj.comarcaBBId = this.comarcaBBIdFormControl.value;
    return obj
  }

  private async criar() {
    await this.service.criar(this.geraObjetoParaOBackEnd()).then();
  }

  private async atualizar() {
    await this.service.atualizar(this.geraObjetoParaOBackEnd()).then();
  }

  tipoDeOperacao(): string {
    return this.comarca ? 'Alteração' : 'Inclusão';
  }

  aoMudarEstado(){
    this.comarcaBBIdFormControl.setValue(null);
    if(this.estadoIdFormControl.value) this.habilitaComarcaBB();
    else this.desabilitaComarcaBB();
    this.obterTodosComarcaBB();
  }

  desabilitaTooltip(formControl: FormControl): boolean {
    return formControl.untouched || formControl.valid;
  }

  possuiAcentos() :boolean
  {       
      let text =  this.nomeFormControl.value;           
      const tamanhoInicial =  text.length;                                      
      text = text.replace(new RegExp('[ÁÀÂÃ]','gi'), '');
      text = text.replace(new RegExp('[ÉÈÊ]','gi'), '');
      text = text.replace(new RegExp('[ÍÌÎ]','gi'), '');
      text = text.replace(new RegExp('[ÓÒÔÕ]','gi'), '');
      text = text.replace(new RegExp('[ÚÙÛ]','gi'), '');
      return tamanhoInicial > text.length;             
  }
  //#region MODAL

  static exibeModalDeIncluir(): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(ComarcaModalComponent, { centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.operacaoTitulo = 'Incluir';
    return modalRef.result;
  }

  static exibeModalDeAlterar(comarca: any): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(ComarcaModalComponent, { centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.comarca = comarca;
    modalRef.componentInstance.operacaoTitulo = 'Alterar';
    return modalRef.result;
  }

  close(): void {
    this.modal.close(false);
  }
  //#endregion MODAL
}

