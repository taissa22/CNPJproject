import { TipoProcesso } from 'src/app/core/models/tipo-processo';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';
// angular
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

// 3rd party
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

// core & shared
import { StaticInjector } from '@manutencao/static-injector';
import { DialogService } from '@shared/services/dialog.service';

// local
//import { TipoDeAudiencia } from '@manutencao/models/tipo-de-audiencia';
import { ComplementoAreaEnvolvida } from '@manutencao/models/complemento-area-envolvida.model';
import { TiposProcesso } from '@manutencao/services/tipos-de-processos';
import { ComplementoAreaEnvolvidaService } from '@manutencao/services/complemento-area-envolvida.service';

@Component({
  selector: 'app-complemento-area-envolvida-modal',
  templateUrl: './complemento-area-envolvida-modal.component.html',
  styleUrls: ['./complemento-area-envolvida-modal.component.scss'],

})

export class ComplementoAreaEnvolvidaModalComponent implements OnInit {
  complementoAreaEnvolvida: ComplementoAreaEnvolvida;


  operacao: string;
  tipoProcessoId : number;

  constructor(
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private service: ComplementoAreaEnvolvidaService
  ) { }

  ativoFormControl: FormControl = new FormControl(true);

  nomeFormControl: FormControl = new FormControl('', [
    Validators.required,
    Validators.maxLength(100)
  ]);

  formGroup: FormGroup = new FormGroup({
    ativo: this.ativoFormControl,
    nome: this.nomeFormControl
  });

  ngOnInit(): void {

    if(this.complementoAreaEnvolvida){
      this.operacao = "Alteração ";
    } else {
      this.operacao = "Inclusão ";
    }

    this.InicilizaForm();
  }


  InicilizaForm(){
    if (this.complementoAreaEnvolvida) {
      this.ativoFormControl.setValue(this.complementoAreaEnvolvida.ativo);
      this.nomeFormControl.setValue(this.complementoAreaEnvolvida.nome);
    }
  }

  close(): void {
    this.modal.close(false);
  }

  // onChangeComboTipoProcesso(tipoProcesso: TiposProcesso) {
  //   this.tipoDeProcessoFormControl.setValue(tipoProcesso.id);
  // }

  async save(): Promise<void> {
    let request: Promise<void>;


    if (!this.nomeFormControl.value.replace(/\s/g, '').length) {
      await this.dialogService.err(`${this.operacao} não realizada`, `O campo nome não pode contar apenas espaços.`);
      return;
    }

    if (this.complementoAreaEnvolvida) {
      request = this.service.alterar(this.complementoAreaEnvolvida.id, this.nomeFormControl.value,this.ativoFormControl.value);
    } else {
      request = this.service.incluir(this.nomeFormControl.value, this.ativoFormControl.value, this.tipoProcessoId);
    }


    try {
      await request;
      await this.dialogService.alert(`${this.operacao} realizada com sucesso`);
      this.modal.close(true);
    } catch (error) {
      await this.dialogService.info(
        `Desculpe, não é possível fazer a ${this.operacao}`,
        'Já existe um Complemento com o nome informado para esse Tipo de Processo'
      );
      await this.dialogService.err(`${this.operacao} não realizada`);
    }
  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeIncluir(tipoProcessoId : number): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(ComplementoAreaEnvolvidaModalComponent, { centered: true, size: 'sm', backdrop: 'static' });
      modalRef.componentInstance.tipoProcessoId = tipoProcessoId;
    return modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeAlterar(complementoAreaEnvolvida: ComplementoAreaEnvolvida): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(ComplementoAreaEnvolvidaModalComponent, { centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.complementoAreaEnvolvida = complementoAreaEnvolvida;
    return modalRef.result;
  }



}
