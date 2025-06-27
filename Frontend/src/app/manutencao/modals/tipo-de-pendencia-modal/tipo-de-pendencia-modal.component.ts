import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';
// angular
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

// 3rd party
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

// core & shared
import { StaticInjector } from '@manutencao/static-injector';
import { DialogService } from '@shared/services/dialog.service';

// local
import { TipoDePendencia } from '@manutencao/models/tipo-de-pendencia';
//import { TipoDeAudienciaService } from '@manutencao/services/tipo-de-audiencia.service';
import { BehaviorSubject, Subscription } from 'rxjs';
import { TipoProcessoService } from '@core/services/sap/tipo-processo.service';

import { TipoProcesso } from '@core/models/tipo-processo';
import { take } from 'rxjs/operators';
import { Input } from '@angular/core';
import { TipoDePendenciaServiceMock } from '@manutencao/data/tipo-de-pendencia.service.mock';
import { TipoDePendenciaService } from '@manutencao/services/tipo-de-pendencia.service';
import { HttpErrorResult } from '@core/http';

@Component({
  selector: 'app-tipo-de-pendencia-modal',
  templateUrl: './tipo-de-pendencia-modal.component.html',
  styleUrls: ['./tipo-de-pendencia-modal.component.scss']
  //providers: [{ provide: TipoDePendenciaServiceMock, useClass: TipoDePendenciaServiceMock }]

})
export class TipoDePendenciaModalComponent implements OnInit {
  private tipoDePendencia: TipoDePendencia;
  public tipoprocessosservice: TipoProcessoService;
  codigo: number;

  constructor(
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private service: TipoDePendenciaService
  ) { }


  tiposProcesso: any[] = []

  descricaoFormControl: FormControl = new FormControl('', [
    Validators.required,
    Validators.maxLength(100)
  ]);

  formGroup: FormGroup = new FormGroup({
    descricao: this.descricaoFormControl
  });

  ngOnInit(): void {
    if (this.tipoDePendencia) {
      this.codigo = this.tipoDePendencia.id;

      this.descricaoFormControl.setValue(this.tipoDePendencia.descricao);
    }
  }

  close(): void {
    this.modal.close(false);
  }


  async save(): Promise<void> {
    let operacao = this.tipoDePendencia ? 'Alteração' : 'Inclusão'; 
    //let request: Promise<void>;

    if (!this.descricaoFormControl.value.replace(/\s/g, '').length) {
      await this.dialogService.err(`${operacao} não realizada`, `O campo descrição não pode contar apenas espaços.`);
      return;
    }

    try {

      if (this.tipoDePendencia) {
        await this.service.alterar(
          this.codigo,
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
      this.dialogService.err(`${operacao} não realizada`, (error as HttpErrorResult).messages.join('\n'));
      throw error;
    }
  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeIncluir(): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(TipoDePendenciaModalComponent, { centered: true, backdrop: 'static' });
    return modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeAlterar(tipoDePendencia: TipoDePendencia): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(TipoDePendenciaModalComponent, { centered: true, backdrop: 'static' });
    modalRef.componentInstance.tipoDePendencia = tipoDePendencia;
    return modalRef.result;
  }
}
