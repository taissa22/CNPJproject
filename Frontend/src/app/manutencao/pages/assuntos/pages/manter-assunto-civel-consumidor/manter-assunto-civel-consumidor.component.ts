import { Component, OnInit, Input, AfterViewInit, ComponentFactoryResolver } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { NgbActiveModal, NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { ChangeDetectorRef,AfterContentChecked} from '@angular/core'

import { HttpErrorResult } from '@core/http/http-error-result';
import { DialogService } from '@shared/services/dialog.service';

import { Assunto } from '@manutencao/models';
import { AssuntoService } from '@manutencao/services';
import { StaticInjector } from '@manutencao/static-injector';

@Component({
  selector: 'app-manter-assunto-civel-consumidor',
  templateUrl: './manter-assunto-civel-consumidor.component.html'
})
export class ManterAssuntoCivelConsumidorComponent implements OnInit, AfterViewInit {

  @Input() titulo;
  @Input() entidade: Assunto;

  comboDeparaEstrategico : any;

  descricaoFormControl = new FormControl('', [Validators.required, Validators.maxLength(40)]);
  propostaFormControl = new FormControl('', [Validators.maxLength(2000)]);
  negociacaoFormControl = new FormControl('', [Validators.maxLength(4000)]);
  codigoTipoContingenciaFormControl = new FormControl('',[Validators.required]);

  comboDeparaEstrategicoFormControl = new FormControl();

  formulario = new FormGroup({
    descricao: this.descricaoFormControl,
    proposta: this.propostaFormControl,
    negociacao: this.negociacaoFormControl,
    codigoTipoContingencia: this.codigoTipoContingenciaFormControl,
    descricaoMigracao: this.comboDeparaEstrategicoFormControl
  });

  constructor(
    public activeModal: NgbActiveModal,
    private service: AssuntoService,
    private dialog: DialogService) { }

  ngAfterViewInit(): void {
    console.log(this.comboDeparaEstrategico);
  }

  async ngOnInit() {
    if (this.entidade) {
      this.descricaoFormControl.setValue(this.entidade.descricao);
      this.propostaFormControl.setValue(this.entidade.proposta);
      this.negociacaoFormControl.setValue(this.entidade.negociacao);
      this.comboDeparaEstrategicoFormControl.setValue(this.entidade.idMigracao);

      if(this.entidade.codTipoContingencia == 'M'){
        this.codigoTipoContingenciaFormControl.setValue('M')
      }
      else{
        this.codigoTipoContingenciaFormControl.setValue('P')
      }
    }
    this.obterDeParaEstrategico();
  }

  // tslint:disable-next-line: member-ordering
  private static exibeModal(): NgbModalRef {
    const modalService: NgbModal = StaticInjector.Instance.get(NgbModal);
    const modalRef = modalService.open(ManterAssuntoCivelConsumidorComponent, { centered: true, backdrop: 'static' });
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
      this.activeModal.dismiss();
    } catch (error) {
      console.error(error);
    }
  }

  private async criar(): Promise<void> {
    try {
      await this.service.criar('civel-consumidor', {
        idEstrategico: this.comboDeparaEstrategicoFormControl.value,
        descricao: this.descricaoFormControl.value,
        proposta: this.propostaFormControl.value,
        negociacao: this.negociacaoFormControl.value,
        codigoTipoCalculoContingencia: this.codigoTipoContingenciaFormControl.value
      });
      await this.dialog.showAlert('Cadastro realizado com sucesso', 'O assunto foi registrado no sistema.');
    } catch (error) {
      console.error(error);
      await this.dialog.showErr('Operação não realizada', (error as HttpErrorResult).messages.join('\n'));
      throw error;
    }
  }

  private async atualizar(): Promise<void> {
    try {
      await this.service.atualizar('civel-consumidor', {
        assuntoId: this.entidade.id,
        idEstrategico: this.comboDeparaEstrategicoFormControl.value,
        descricao: this.descricaoFormControl.value,
        proposta: this.propostaFormControl.value,
        negociacao: this.negociacaoFormControl.value,
        codigoTipoContingencia: this.codigoTipoContingenciaFormControl.value
      });
      await this.dialog.showAlert('Alteração realizada com sucesso', 'O assunto foi alterado no sistema.');
      this.activeModal.close();
    } catch (error) {
      console.error(error);
      await this.dialog.showErr('Operação não realizada', (error as HttpErrorResult).messages.join('\n'));
      throw error;
    }
  }

  desabilitaTooltip(formControl: FormControl) {
    return formControl.untouched || formControl.valid;
  }

  obterDeParaEstrategico(): void {
    this.service.ObterDescricaoDeParaCivelEstrategico().then((e) => {
      this.comboDeparaEstrategico = e.map(r => ({ idEstrategico: r.id, nome: r.descricao + (r.ativo ? '' : ' [INATIVO]') }));
    });

  }
}
