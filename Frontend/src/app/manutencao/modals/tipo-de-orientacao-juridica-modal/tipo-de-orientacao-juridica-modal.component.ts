import { TipoDeOrientacaoJuridicaService } from '@manutencao/services/tipo-de-orientacao-juridica.service';
import { TipoDeOrientacaoJuridica } from './../../models/tipo-de-orientacao-juridica';
import { TipoDeOrientacaoJuridicaServiceMock } from './../../data/tipo-de-orientacao-juridica.service.mock';
import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';
import { BehaviorSubject } from 'rxjs';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { StaticInjector } from '@manutencao/static-injector';
import { HttpErrorResult } from '@core/http/http-error-result';

@Component({
  selector: 'app-tipo-de-orientacao-juridica-modal',
  templateUrl: './tipo-de-orientacao-juridica-modal.component.html',
  styleUrls: ['./tipo-de-orientacao-juridica-modal.component.scss'],
  providers: [{ provide:TipoDeOrientacaoJuridicaServiceMock , useClass: TipoDeOrientacaoJuridicaServiceMock}]
})
export class TipoDeOrientacaoJuridicaModalComponent implements OnInit {

  TipoDeOrientacaoJuridica: TipoDeOrientacaoJuridica;
  titulo: string;


  constructor(
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private service: TipoDeOrientacaoJuridicaService
  ) { }

  @Input() tipoOrientacaoJuridica: any;

  descricaoFormControl: FormControl = new FormControl('', [
    Validators.required,
    Validators.maxLength(100)
  ]);

  formGroup: FormGroup = new FormGroup({
    descricao: this.descricaoFormControl
  });

  ngOnInit(): void {
    if (this.TipoDeOrientacaoJuridica) {
      this.descricaoFormControl.setValue(this.TipoDeOrientacaoJuridica.descricao);
    }
  }

  close(): void {
    this.modal.close(false);
  }

  async save(): Promise<void> {
    const operacao = this.TipoDeOrientacaoJuridica ? 'Alteração': 'Inclusão';
    try {
      if (!this.descricaoFormControl.value.replace(/\s/g, '').length) {
        await this.dialogService.err(`${operacao} não realizada`, `O campo descrição não pode contar apenas espaços.`);
        return;
      }

      if (this.TipoDeOrientacaoJuridica) {

        await this.service.alterar(
          this.TipoDeOrientacaoJuridica.codigo,
          this.descricaoFormControl.value
        );
      } else {
        await this.service.incluir(
          this.descricaoFormControl.value
        );
      }
      await this.dialogService.alert(`${operacao} realizada com sucesso`);
      this.modal.close(true);
    } catch (error) {
      await this.dialogService.err(`${operacao} não realizada`, (error as HttpErrorResult).messages.join('\n'));
    }
  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeIncluir(): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(TipoDeOrientacaoJuridicaModalComponent, { centered: true, backdrop: 'static' });
    modalRef.componentInstance.titulo = 'Incluir Tipo de Orientação Jurídica';
    return modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeAlterar(TipoDeOrientacaoJuridica: TipoDeOrientacaoJuridica): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(TipoDeOrientacaoJuridicaModalComponent, { centered: true, backdrop: 'static' });
    
    modalRef.componentInstance.titulo = 'Alterar Tipo de Orientação Jurídica';
    modalRef.componentInstance.TipoDeOrientacaoJuridica = TipoDeOrientacaoJuridica;
    return modalRef.result;
  }

}
