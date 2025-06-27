import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NgbActiveModal, NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { HttpErrorResult } from '@core/http/http-error-result';
import { DialogService } from './../../../../../shared/services/dialog.service';

import { Assunto } from '@manutencao/models';
import { AssuntoService } from '@manutencao/services';
import { StaticInjector } from '@manutencao/static-injector';

@Component({
  selector: 'app-manter-assunto-civel-estrategico',
  templateUrl: './manter-assunto-civel-estrategico.component.html'
})
export class ManterAssuntoCivelEstrategicoComponent implements OnInit {

  @Input() titulo;
  @Input() entidade: Assunto;

  comboDeparaConsumidor : any;

  descricaoFormControl = new FormControl('', [Validators.required, Validators.maxLength(40)]);
  ativoFormControl = new FormControl(true);
  comboDeparaConsumidorFormControl = new FormControl();

  formulario = new FormGroup({
    descricao: this.descricaoFormControl,
    ativo: this.ativoFormControl,
    descricaoMigracao: this.comboDeparaConsumidorFormControl
  });

  constructor(
    public activeModal: NgbActiveModal,
    private service: AssuntoService,
    private dialog: DialogService) { }

  ngOnInit() {
    if (this.entidade) {
      this.descricaoFormControl.setValue(this.entidade.descricao);
      this.ativoFormControl.setValue(this.entidade.ativo);
      this.comboDeparaConsumidorFormControl.setValue(this.entidade.idMigracao);
    }

    this.obterDeParaConsumidor();
  }

  // tslint:disable-next-line: member-ordering
  private static exibeModal(): NgbModalRef {
    const modalService: NgbModal = StaticInjector.Instance.get(NgbModal);
    const modalRef = modalService.open(ManterAssuntoCivelEstrategicoComponent, { centered: true, backdrop: 'static' });
    return modalRef;
  }

  // tslint:disable-next-line: member-ordering
  public static async exibeModalDeCriar(): Promise<void> {
    const modalRef = this.exibeModal();
    modalRef.componentInstance.titulo = 'Inclusão de Assunto';
    await modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  public static async exibeModalDeAlterar(entidade: Assunto): Promise<void> {
    const modalRef = this.exibeModal();
    modalRef.componentInstance.titulo = 'Edição de Assunto';
    modalRef.componentInstance.entidade = entidade;
    await modalRef.result;
  }

  async confirmar(): Promise<void> {
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
      this.activeModal.close();
    } catch (error) {
      console.error(error);
    }
  }

  private async criar(): Promise<void> {
    try {
      await this.service.criar('civel-estrategico', {
        idConsumidor: this.comboDeparaConsumidorFormControl.value,
        descricao: this.descricaoFormControl.value,
        ativo: this.ativoFormControl.value ? this.ativoFormControl.value : false
      });
      this.dialog.showAlert('Cadastro realizado com sucesso', 'O assunto foi registrado no sistema.');
    } catch (error) {
      console.error(error);
      await this.dialog.showErr('Operação não realizada', (error as HttpErrorResult).messages.join('\n'));
      throw error;
    }
  }

  private async atualizar(): Promise<void> {
    try {
      await this.service.atualizar('civel-estrategico', {
        assuntoId: this.entidade.id,
        idConsumidor: this.comboDeparaConsumidorFormControl.value,
        descricao: this.descricaoFormControl.value,
        ativo: this.ativoFormControl.value ? this.ativoFormControl.value : false
      });
      this.dialog.showAlert('Alteração realizada com sucesso', 'O assunto foi alterado no sistema.');
    } catch (error) {
      console.error(error);
      await this.dialog.showErr('Operação não realizada', (error as HttpErrorResult).messages.join('\n'));
      throw error;
    }
  }

  desabilitaTooltip(formControl: FormControl) {
    return formControl.untouched || formControl.valid;
  }

  obterDeParaConsumidor(): void {
    this.service.ObterDescricaoDeParaCivelConsumidor().then((e) => {
      this.comboDeparaConsumidor = e.map(r => ({ idConsumidor: r.id, nome: r.descricao + (r.ativo ? '' : ' [INATIVO]') }));
    });
  }
}
