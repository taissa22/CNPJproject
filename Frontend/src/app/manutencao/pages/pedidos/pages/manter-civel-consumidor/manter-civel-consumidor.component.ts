import { Component, OnInit, Input, AfterViewInit, ComponentFactoryResolver } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NgbActiveModal, NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { HttpErrorResult } from '@core/http/http-error-result';
import { DialogService } from '@shared/services/dialog.service';

import { Pedido } from '@manutencao/models';
import { PedidosService } from '@manutencao/services';
import { StaticInjector } from '@manutencao/static-injector';

@Component({
  selector: 'app-manter',
  templateUrl: './manter-civel-consumidor.component.html'
})
export class ManterPedidoConsumidorManutencaoComponent implements OnInit, AfterViewInit {
  @Input() titulo;
  @Input() entidade: Pedido;
  comboDepara : any;

  descricaoFormControl = new FormControl('', [Validators.required, Validators.maxLength(50)]);
  ativoFormControl = new FormControl(true);
  audienciaFormControl = new FormControl(true);
  comboDeparaFormControl = new FormControl();

  formulario = new FormGroup({
    descricao: this.descricaoFormControl,
    ativo: this.ativoFormControl,
	  audiencia: this.audienciaFormControl,
    depara: this.comboDeparaFormControl
  });

  constructor(
    public activeModal: NgbActiveModal,
    private service: PedidosService,
    private dialog: DialogService) { }

  ngAfterViewInit(): void {
    console.log(this.comboDepara);
  }

  ngOnInit() {
    if (this.entidade) {
      this.descricaoFormControl.setValue(this.entidade.descricao);
      this.audienciaFormControl.setValue(this.entidade.audiencia);
      this.ativoFormControl.setValue(this.entidade.ativo);
      this.comboDeparaFormControl.setValue(this.entidade.idMigracao);
    }
    this.obterDeParaEstrategico();
  }

  // tslint:disable-next-line: member-ordering
  private static exibeModal(): NgbModalRef {
    const modalService: NgbModal = StaticInjector.Instance.get(NgbModal);
    const modalRef = modalService.open(ManterPedidoConsumidorManutencaoComponent, { centered: true, backdrop: 'static' });
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
      this.activeModal.dismiss();
    } catch (error) {
      console.error(error);
    }
  }

  private async criar(): Promise<void> {
    try {
      await this.service.criar('civel-consumidor', {
        descricao: this.descricaoFormControl.value,
  	    audiencia: this.audienciaFormControl ? this.audienciaFormControl.value : false,
        ativo: this.ativoFormControl.value ? this.ativoFormControl.value : false,
        idEstrategico: this.comboDeparaFormControl.value
      });
      await this.dialog.showAlert('Cadastro realizado com sucesso', 'O pedido foi registrado no sistema.');
    } catch (error) {
      console.error(error);
      await this.dialog.showErr('Operação não realizada', (error as HttpErrorResult).messages.join('\n'));
      throw error;
    }
  }

  private async atualizar(): Promise<void> {
    try {
      await this.service.atualizar('civel-consumidor', {
        pedidoId: this.entidade.id,
        descricao: this.descricaoFormControl.value,
		    audiencia: this.audienciaFormControl.value ? this.audienciaFormControl.value : false,
        ativo: this.ativoFormControl.value ? this.ativoFormControl.value : false,
        idEstrategico: this.comboDeparaFormControl.value
      });
      await this.dialog.showAlert('Alteração realizada com sucesso', 'O pedido foi alterado no sistema.');
    } catch (error) {
      console.error(error);
      await this.dialog.showErr('Operação não realizada', (error as HttpErrorResult).messages.join('\n'));
      throw error;
    }
  }

  desabilitaTooltip(formControl: FormControl) {
    return formControl.untouched || formControl.valid;
  }

  obterDeParaEstrategico (): void {
    this.service.ObterDescricaoDeParaEstrategico().then((e) => {
      this.comboDepara = e.map(r => ({ idEstrategico: r.id, nome: r.descricao + (r.ativo ? '' : ' [INATIVO]') }));
    });

  }
}
