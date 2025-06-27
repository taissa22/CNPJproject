import { DialogService } from './../../../../../shared/services/dialog.service';
import { Component, OnInit, Input } from '@angular/core';
import { HttpErrorResult } from '@core/http/http-error-result';
import { Acao } from '@manutencao/models/acao.model';
import { AcoesService } from '@manutencao/services/acoes.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NgbActiveModal, NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { StaticInjector } from '@manutencao/static-injector';

@Component({
  selector: 'app-manter-trabalhista',
  templateUrl: './manter-trabalhista.component.html'
})
export class ManterAcaoTrabalhistaComponent implements OnInit {

  @Input() titulo;
  @Input() entidade: Acao;

  descricaoFormControl = new FormControl('', [Validators.required, Validators.maxLength(30)]);

  formulario = new FormGroup({
    descricao: this.descricaoFormControl,
  });

  constructor(
    public activeModal: NgbActiveModal,
    private service: AcoesService,
    private dialog: DialogService) { }

  ngOnInit() {
    if (this.entidade) {
      this.descricaoFormControl.setValue(this.entidade.descricao);
    }
  }

  // tslint:disable-next-line: member-ordering
  private static exibeModal(): NgbModalRef {
    const modalService: NgbModal = StaticInjector.Instance.get(NgbModal);
    const modalRef = modalService.open(ManterAcaoTrabalhistaComponent, { centered: true, backdrop: 'static' });
    return modalRef;
  }

  // tslint:disable-next-line: member-ordering
  public static async exibeModalDeCriar(): Promise<void> {
    const modalRef = this.exibeModal();
    modalRef.componentInstance.titulo = 'Inclusão de Ação';
    await modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  public static async exibeModalDeAlterar(entidade: Acao): Promise<void> {
    const modalRef = this.exibeModal();
    modalRef.componentInstance.titulo = 'Edição de Ação';
    modalRef.componentInstance.entidade = entidade;
    await modalRef.result;
  }

  desabilitaTooltip(formControl: FormControl) {
    return formControl.untouched || formControl.valid;
  }

  async onSubmit() {
    this.formulario.markAllAsTouched();
    if (this.formulario.invalid) {
      return;
    }

    try {
      if (this.entidade) {
        await this.atualizar();
      } else {
        await this.criar();
      }

      this.activeModal.dismiss();
    } catch (error) {
      console.error(error);
    }
  }

  private async criar(): Promise<void> {
    try {
      await this.service.criar('trabalhista', {
        descricao: this.descricaoFormControl.value,
        enviarAppPreposto : false
      });
      this.dialog.showAlert('Cadastro realizado com sucesso', 'A ação foi registrada no sistema.');
    } catch (error) {
      this.dialog.showErr('Cadastro não realizado', (error as HttpErrorResult).messages.join('\n'));
      throw error;
    }
  }

  private async atualizar(): Promise<void> {
    try {
      await this.service.atualizar('trabalhista', {
        id: this.entidade.id,
        descricao: this.descricaoFormControl.value,
        enviarAppPreposto : false
      });
      this.dialog.showAlert('Alteração realizada com sucesso', 'A ação foi alterada no sistema.');
    } catch (error) {
      this.dialog.showErr('Alteração não realizada', (error as HttpErrorResult).messages.join('\n'));
      throw error;
    }
  }

}
