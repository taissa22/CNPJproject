import { DialogService } from './../../../../../shared/services/dialog.service';

import { Acao } from '@manutencao/models/acao.model';
import { AcoesService } from '@manutencao/services/acoes.service';

import { NaturezaAcaoBB } from '@manutencao/models/naturezaAcaoBB.model';
import { NaturezaAcaoBbService } from '@manutencao/services/natureza-acao-bb.service';

import { Component, OnInit, Input  } from '@angular/core';
import { HttpErrorResult } from '@core/http/http-error-result';
import { FormGroup, FormControl, Validators  } from '@angular/forms';
import { NgbActiveModal, NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { StaticInjector } from '@manutencao/static-injector';


@Component({
  selector: 'app-manter-civel-consumidor',
  templateUrl: './manter-civel-consumidor.component.html',
})
export class ManterCivelConsumidorComponent implements OnInit {

  @Input() titulo;
  @Input() entidade: Acao;

  naturezasAcoesBB: Array<any>;
  naturezaAcaoBB: NaturezaAcaoBB = null;
  comboDeparaEstrategico : any;

  descricaoFormControl = new FormControl('', [Validators.required, Validators.maxLength(30)]);
  naturezaAcaoBBFormControl = new FormControl(null);

  comboDeparaEstrategicoFormControl = new FormControl();

  enviarAppPrepostoFormControl = new FormControl(false);

  formulario = new FormGroup({
    descricao: this.descricaoFormControl,
    naturezaAcaoBB: this.naturezaAcaoBBFormControl,
    idMigracao: this.comboDeparaEstrategicoFormControl
  });

  constructor(
              public activeModal: NgbActiveModal,
              private service: AcoesService,
              private serviceNaturezaAcao: NaturezaAcaoBbService,
              private dialog: DialogService) { }

  async ngOnInit() {
    if (this.entidade) {
      const naturezaSelecionada = this.entidade.naturezaAcaoBB ? this.entidade.naturezaAcaoBB.id : null;

      this.descricaoFormControl.setValue(this.entidade.descricao);
      this.naturezaAcaoBBFormControl.setValue(naturezaSelecionada);
      this.comboDeparaEstrategicoFormControl.setValue(this.entidade.idMigracao);
      this.enviarAppPrepostoFormControl.setValue(this.entidade.enviarAppPreposto == "Sim");
    }
    this.naturezasAcoesBB = await this.serviceNaturezaAcao.obter();
    this.obterDeParaEstrategico();
  }

  // tslint:disable-next-line: member-ordering
  private static exibeModal(): NgbModalRef {
    const modalService: NgbModal = StaticInjector.Instance.get(NgbModal);
    const modalRef = modalService.open(ManterCivelConsumidorComponent, { centered: true, backdrop: 'static' });
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
      await this.service.criar('civel-consumidor', {
        idEstrategico: this.comboDeparaEstrategicoFormControl.value,
        descricao: this.descricaoFormControl.value,
        naturezaAcaoBB: this.naturezaAcaoBBFormControl.value,
        enviarAppPreposto : this.enviarAppPrepostoFormControl.value
      });
      this.dialog.showAlert('Cadastro realizado com sucesso', 'A ação foi registrada no sistema.');
    } catch (error) {
      this.dialog.showErr('Cadastro não realizado', (error as HttpErrorResult).messages.join('\n'));
      throw error;
    }
  }

  private async atualizar(): Promise<void> {
    try {
      await this.service.atualizar('civel-consumidor', {
        id: this.entidade.id,
        idEstrategico: this.comboDeparaEstrategicoFormControl.value,
        descricao: this.descricaoFormControl.value,
        naturezaAcaoBB: this.naturezaAcaoBBFormControl.value,
        enviarAppPreposto : this.enviarAppPrepostoFormControl.value
      });
      this.dialog.showAlert('Alteração realizada com sucesso', 'A ação foi alterada no sistema.');
    } catch (error) {
      this.dialog.showAlert('Alteração não realizada', (error as HttpErrorResult).messages.join('\n'));
      throw error;
    }
  }

  obterDeParaEstrategico(): void {
    this.service.ObterDescricaoDeParaCivelEstrategico().then((e) => {
      this.comboDeparaEstrategico = e.map(r => ({ idEstrategico: r.id, nome: r.descricao + (r.ativo == "SIM" ? '' : ' [INATIVO]') }));
    });

  }

}
