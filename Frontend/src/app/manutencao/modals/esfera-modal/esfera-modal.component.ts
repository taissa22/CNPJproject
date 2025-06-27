import { EsferaService } from '../../services/esfera.service';
import { Esfera } from '@manutencao/models/esfera.model';
// angular
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

// 3rd party
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

// core & shared
import { StaticInjector } from '@manutencao/static-injector';
import { DialogService } from '@shared/services/dialog.service';

// local
import { HttpErrorResult } from '@core/http/http-error-result';
import { IndiceCorrecaoEsferaComponent } from '@manutencao/pages/esfera/indice-correcao-esfera/indice-correcao-esfera.component';
import { max } from 'rxjs/operators';
import { InconsistenciaComponent } from '@manutencao/pages/esfera/inconsistencias/inconsistencias.component';

@Component({
  selector: 'app-esfera-modal',
  templateUrl: './esfera-modal.component.html',
  styleUrls: ['./esfera-modal.component.scss'],

})
export class EsferaModalComponent implements AfterViewInit {
  esfera: Esfera; 
  titulo = ""; 

  constructor(
    private modal: NgbActiveModal,
    private service: EsferaService,
    private dialogService: DialogService
  ) { }

  nomeFormControl: FormControl = new FormControl('', [ Validators.required, Validators.maxLength(66)  ]);
  corrigePrincipalFormControl: FormControl = new FormControl(true);
  corrigeMultaFormControl: FormControl = new FormControl(true);
  corrigeJurosFormControl: FormControl = new FormControl(true);

  formGroup: FormGroup = new FormGroup({
    descricao: this.nomeFormControl,
    corrigePrincipal: this.corrigePrincipalFormControl,
    corrigeMulta: this.corrigeMultaFormControl,
    corrigeJuros: this.corrigeJurosFormControl,
  });

  async ngAfterViewInit(): Promise<void> {

    if (this.esfera) {      
      this.nomeFormControl.setValue(this.esfera.nome);
      this.corrigePrincipalFormControl.setValue(this.esfera.corrigePrincipal)      
      this.corrigeMultaFormControl.setValue(this.esfera.corrigeMultas)      
      this.corrigeJurosFormControl.setValue(this.esfera.corrigeJuros)
    }
  }

  close(): void {
    this.modal.close(false);
  }

  async save(): Promise<void> {
    const operacao = this.esfera ? 'Alteração' : 'Inclusão';
    try {
      if (!this.nomeFormControl.value.replace(/\s/g, '').length) {
        await this.dialogService.err(`${operacao} não realizada`, `O campo descrição não pode contar apenas espaços.`);
        return;
      }

      if (this.esfera) {

        let resultado = await this.service.alterar(
          this.esfera.id,
          this.nomeFormControl.value,
          this.corrigePrincipalFormControl.value,
          this.corrigeMultaFormControl.value ,
          this.corrigeJurosFormControl.value);

          if (resultado && resultado.total > 0){
            await InconsistenciaComponent.exibeModal(resultado.total, resultado.data);
          }
          else{
            await this.dialogService.alert(`${operacao} realizada com sucesso`); 
          }
      } else {
        await this.service.incluir(
          this.nomeFormControl.value,
          this.corrigePrincipalFormControl.value,
          this.corrigeMultaFormControl.value,
          this.corrigeJurosFormControl.value);
          
          await this.dialogService.alert(`${operacao} realizada com sucesso`);
      }
      
      this.modal.close(true);
    } catch (error) {
      await this.dialogService.err(`${operacao} não realizada`, (error as HttpErrorResult).messages.join('\n'));
    }
  }

  //#region MODAL

  // tslint:disable-next-line: member-ordering
  static exibeModalDeIncluir(): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(EsferaModalComponent, { centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.titulo = 'Incluir Esfera';
    return modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  static exibeModalDeAlterar(esfera: Esfera): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(EsferaModalComponent, {windowClass: 'esfera-modal', centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.titulo = 'Alterar Esfera';
    modalRef.componentInstance.esfera = esfera;
    return modalRef.result;
  }

  //#endregion MODAL
}
