import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

// 3rd party
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

// core & shared
import { StaticInjector } from '@manutencao/static-injector';
import { DialogService } from '@shared/services/dialog.service';

// local
import { HttpErrorResult } from '@core/http';
import { FatoGeradorService } from '@manutencao/services/fato-gerador.service';
import { FatoGerador } from '@manutencao/models/fato-gerador.model';

@Component({
  selector: 'app-fato-gerador-modal',
  templateUrl: './fato-gerador-modal.component.html',
  styleUrls: ['./fato-gerador-modal.component.scss']
})
export class FatoGeradorModalComponent implements OnInit {
  fato : FatoGerador;

  constructor(
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private service: FatoGeradorService
  ) {}

  descricaoFormControl: FormControl = new FormControl('', [
    Validators.required,
    Validators.maxLength(50)
  ]);

  ativoFormControl: FormControl = new FormControl(true);
  
  formGroup: FormGroup = new FormGroup({
    nome: this.descricaoFormControl,   
    ativo: this.ativoFormControl    
  });

  ngOnInit(): void {   
    this.InicilizaForm();
  }

  InicilizaForm() {
    this.formGroup.addControl('id', new FormControl(this.fato ? this.fato.id : 0));
    this.descricaoFormControl.setValue(this.fato  ? this.fato.nome : "");
    this.ativoFormControl.setValue(this.fato ? (this.fato.ativo ? true : false) : true);     
  }

  close(): void {
    this.modal.close(false);
  }

  async save(): Promise<void> {
  
    const operacao = this.fato ? 'Alteração' : 'Inclusão';

    if (!this.descricaoFormControl.value.replace(/\s/g, '').length) {
      await this.dialogService.err(
        `${operacao} não realizada`,
        `O campo nome não pode contar apenas espaços.`
      );
      return;
    }
    try {
      
      if (this.fato) {
        await this.service.alterar(this.formGroup.value);
      } else {
        await this.service.incluir(this.formGroup.value);
      }
      await this.dialogService.alert(`${operacao} realizada com sucesso`);
      this.modal.close(true);
    }  catch (error) {
      await this.dialogService.err(`Desculpe, não foi possivel a ${operacao}`, (error as HttpErrorResult).messages.join('\n'));
    };       
  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeIncluir(): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(FatoGeradorModalComponent, { centered: true, size: 'sm', backdrop: 'static' });    
    return modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeAlterar(fato: FatoGerador): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(FatoGeradorModalComponent, { centered: true, size: 'sm', backdrop: 'static' });     
      modalRef.componentInstance.fato = fato;
    return modalRef.result;
  }
}
