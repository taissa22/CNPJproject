import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Solicitante } from '@manutencao/models/solicitante';
import { SolicitanteService } from '@manutencao/services/solicitante.service';
import { StaticInjector } from '@manutencao/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';

@Component({
  selector: 'app-solicitantes',
  templateUrl: './solicitantes.component.html',
  styleUrls: ['./solicitantes.component.scss']
})
export class SolicitantesModalComponent implements OnInit {

  constructor(
    private modal: NgbActiveModal,
    private dialog: DialogService,
    private service: SolicitanteService
  ) { }

  ngOnInit() {
    this.setValidators();
  }

  titulo: string;

  codigoFormControl: FormControl = new FormControl(null);
  nomeFormControl: FormControl = new FormControl(null);
  emailFormControl: FormControl = new FormControl(null);

  solicitanteForm: FormGroup = new FormGroup({
    codSolicitante: this.codigoFormControl,
    nome: this.nomeFormControl,
    email: this.emailFormControl,
  });

  initForm(item: Solicitante) {
    this.codigoFormControl.setValue(item.codSolicitante);
    this.nomeFormControl.setValue(item.nome);
    this.emailFormControl.setValue(item.email);
  }

  setValidators() {
    this.nomeFormControl.setValidators([Validators.required]);
    this.nomeFormControl.updateValueAndValidity();
    this.emailFormControl.setValidators([Validators.required, this.customEmailValidator()]);
    this.emailFormControl.updateValueAndValidity();
  }

  public static exibeModalDeIncluir(): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(SolicitantesModalComponent, { windowClass: 'modal-solicitante', centered: true, size: 'lg', backdrop: 'static' });
    modalRef.componentInstance.titulo = "Inclusão"
    return modalRef.result;
  }

  public static exibeModalDeAlterar(solicitante: Solicitante): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(SolicitantesModalComponent, { windowClass: 'modal-solicitante', centered: true, size: 'lg', backdrop: 'static' });
    modalRef.componentInstance.titulo = "Alteração"
    modalRef.componentInstance.initForm(solicitante);

    return modalRef.result;
  }

  async salvar() {
    try {
      const method = this.titulo == "Inclusão" ? "Incluído" : "Alterado";
      await this.service.salvar(this.solicitanteForm.value);
      await this.dialog.alert(`${method} com sucesso!`, `${this.titulo} realizada com sucesso.`)
      return this.fechar();
    } catch (error) {
      await this.dialog.err('Erro ao incluir!', 'Não foi possível realizar a inclusão do Solicitante.')
    }
  }

  fechar() {
    this.modal.close(true);
  }

  customEmailValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      const email: string = control.value;
      // Regex para validar o formato do email
      const emailRegex: RegExp = /^[a-zA-Z0-9._%+-çÇáéíóúÁÉÍÓÚâêîôÂÊÎÔãõÃÕàÀüÜ]+@[a-zA-Z0-9.-çÇáéíóúÁÉÍÓÚâêîôÂÊÎÔãõÃÕàÀüÜ]+\.[a-zA-Z]{2,}$/;
  
      // Verifica se o email corresponde ao formato regex e tem um comprimento aceitável
      if (!emailRegex.test(email) || email.length > 250) {
        return { customEmail: true }; // Retorna um objeto indicando a falha na validação
      }
  
      return null; // Email válido
    };
  }

}
