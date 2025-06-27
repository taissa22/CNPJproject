import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { FormControl } from '@angular/forms';
import { HttpErrorResult } from '@core/http';
import { Estados } from '@core/models';
import { Estado } from '@manutencao/models/estado.model';
import { Municipio } from '@manutencao/models/municipio.model';
import { MunicipioService } from '@manutencao/services/municipio.service';
import { StaticInjector } from '@manutencao/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';
import { isObject } from 'ngx-bootstrap/chronos/utils/type-checks';

@Component({
  selector: 'app-municipio-modal',
  templateUrl: './municipio-modal.component.html',
  styleUrls: ['./municipio-modal.component.scss']
})
export class MunicipioModalComponent implements OnInit {
  municipio: Municipio = null;
  estado:Estado = null;
  estadosSelect = Estados.obterUfs();
  nomeFormControl: FormControl = new FormControl(null, [Validators.required,Validators.maxLength(50)]);
  formGroup: FormGroup = new FormGroup({
    nome: this.nomeFormControl,
  })

  constructor(
    private service: MunicipioService,
    private dialogService: DialogService,
    private modal: NgbActiveModal) { }

  ngOnInit(): void {
    this.iniciarForm()
  }

  iniciarForm() {
    if(this.municipio){
      this.formGroup.addControl('id', new FormControl(this.municipio.id));
      this.formGroup.addControl('estadoId',new FormControl(this.municipio.estadoId));
      this.nomeFormControl.setValue(this.municipio.nome);
    }
    else{
      this.formGroup.addControl('estadoId',new FormControl(this.estado.id));
    }
  }

  async salvar() {
    try {
      if (! await this.formValido()) return;

      if (this.municipio) {
        await this.atualizar();
        await this.dialogService.alert(`Alteração realizada com sucesso`);
      } else {
        await this.criar();
        await this.dialogService.alert(`Inclusão realizada com sucesso`);
      }

      this.modal.close(true);

    } catch (error) {
      await this.dialogService.err(`Desculpe, não foi possivel a ${ this.municipio ?'Alteração':'Inclusão'}`, (error as HttpErrorResult).messages.join('\n'));
    }
  }


  private async formValido() {

    if (this.nomeFormControl.value && !this.nomeFormControl.value.replace(/\s/g, '').length) {
      await this.dialogService.err(`Desculpe, não foi possivel a alteração`, `O campo nome não pode conter apenas espaços.`);
      return false;
    }
    if (this.nomeFormControl.value &&  this.nomeFormControl.value.length > 50) {
      await this.dialogService.err(`Desculpe, não foi possivel a alteração`, `O campo nome não pode conter mais de 50 caracteres.`);
      return false;
    }
    return true;
  }

  criarObjBackend(){
    let obj:any = Object.assign({},this.formGroup.value);
    obj.nome = obj.nome.toUpperCase();
    return obj;
  }

  async atualizar() {
    await this.service.atualizar(this.criarObjBackend());
  }

  async criar() {
    await this.service.criar(this.criarObjBackend());
  }


  desabilitaTooltip(formControl: FormControl): boolean {
    return formControl.untouched || formControl.valid;
  }

  static exibeModalDeAlterar(municipio: Municipio): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(MunicipioModalComponent, { centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.municipio = municipio;
    return modalRef.result;
  }

  static exibeModalDeCriar(estado:Estado): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(MunicipioModalComponent, { centered: true, size: 'sm', backdrop: 'static' });
      modalRef.componentInstance.estado = estado;
     return modalRef.result;
  }

  close(): void {
    this.modal.close(false);
  }
}
