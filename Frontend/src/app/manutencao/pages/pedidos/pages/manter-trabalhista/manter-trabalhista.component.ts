import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NgbActiveModal, NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { HttpErrorResult } from '@core/http/http-error-result';
import { DialogService } from '@shared/services/dialog.service';

import { Pedido } from '@manutencao/models';
import { PedidosService } from '@manutencao/services';
import { StaticInjector } from '@manutencao/static-injector';

@Component({
  selector: 'app-manter-trabalhista',
  templateUrl: './manter-trabalhista.component.html'
})
export class ManterTrabalhistaComponent implements OnInit {

  @Input() titulo;
  @Input() entidade: Pedido;

  riscoPerdaFormControl = new FormControl(null, [Validators.required]);
  descricaoFormControl = new FormControl('', [Validators.required, Validators.maxLength(50)]);
  ativoFormControl = new FormControl(true);
  provavelZeroFormControl = new FormControl(false);
  proprioTerceiroFormControl = new FormControl('P');

  formulario = new FormGroup({
    descricao: this.descricaoFormControl,
    riscoPerda: this.riscoPerdaFormControl,
    ativo: this.ativoFormControl,
    provavelZero: this.provavelZeroFormControl,
    proprioTerceiro: this.proprioTerceiroFormControl
  });

  riscosPerdas: Array<{ key: string, value: string }> = [{ key: 'PO', value: 'POSSÍVEL' },
  { key: 'PR', value: 'PROVÁVEL' },
  { key: 'RE', value: 'REMOTO' }];
  riscoPerda: { id: string, valor: string } = null;

  constructor(
    public activeModal: NgbActiveModal,
    private service: PedidosService,
    private dialog: DialogService) { }

  ngOnInit() {
    if (this.entidade) {
      const riscoPerdaSelecionada = this.entidade.riscoPerda ? this.entidade.riscoPerda.id : '';
      const proprioTerceiroSelecionado = this.entidade.proprioTerceiro ? this.entidade.proprioTerceiro.id : 'P';

      this.riscoPerdaFormControl.setValue(riscoPerdaSelecionada);
      this.proprioTerceiroFormControl.setValue(proprioTerceiroSelecionado);
      this.descricaoFormControl.setValue(this.entidade.descricao);
      this.ativoFormControl.setValue(this.entidade.ativo);
      this.provavelZeroFormControl.setValue(this.entidade.provavelZero);
    }
  }

  // tslint:disable-next-line: member-ordering
  private static exibeModal(): NgbModalRef {
    const modalService: NgbModal = StaticInjector.Instance.get(NgbModal);
    const modalRef = modalService.open(ManterTrabalhistaComponent, { centered: true, backdrop: 'static' });
    return modalRef;
  }

  // tslint:disable-next-line: member-ordering
  public static async exibeModalDeCriar(): Promise<void> {
    const modalRef = this.exibeModal();
    modalRef.componentInstance.titulo = 'Inclusão de Pedido';
    await modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  public static async exibeModalDeAlterar(entidade: Pedido): Promise<void> {
    const modalRef = this.exibeModal();
    modalRef.componentInstance.titulo = 'Edição de Pedido';
    modalRef.componentInstance.entidade = entidade;
    await modalRef.result;
  }

  async confirmar() {
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
        ativo: this.ativoFormControl.value,
        riscoPerdaId: this.riscoPerdaFormControl.value,
        provavelZero: this.provavelZeroFormControl.value,
        proprioTerceiroId: this.proprioTerceiroFormControl.value
      });
      this.dialog.showAlert('Cadastro realizado com sucesso', 'O pedido foi registrado no sistema.');
    } catch (error) {
      console.error(error);
      await this.dialog.showErr('Operação não realizada', (error as HttpErrorResult).messages.join('\n'));
      throw error;
    }
  }

  private async atualizar() {
    try {
      await this.service.atualizar('trabalhista', {
        pedidoId: this.entidade.id,
        descricao: this.descricaoFormControl.value,
        ativo: this.ativoFormControl.value,
        riscoPerdaId: this.riscoPerdaFormControl.value,
        provavelZero: this.provavelZeroFormControl.value,
        proprioTerceiroId: this.proprioTerceiroFormControl.value
      });
      this.dialog.showAlert('Alteração realizada com sucesso', 'O pedido foi alterado no sistema.');
    } catch (error) {
      console.error(error);
      await this.dialog.showErr('Operação não realizada', (error as HttpErrorResult).messages.join('\n'));
      throw error;
    }
  }

  desabilitaTooltip(formControl: FormControl) {
    return formControl.untouched || formControl.valid;
  }
}
