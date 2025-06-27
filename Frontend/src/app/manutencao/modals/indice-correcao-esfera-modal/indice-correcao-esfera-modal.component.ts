import { IndiceCorrecaoEsfera } from '@manutencao/models/IndiceCorrecaoEsfera.model';
import { IndiceCorrecaoEsferaService } from '@manutencao/services/IndiceCorrecaoEsfera.service';

import { IndiceService } from '@manutencao/services/indice.service';
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
import { Indice } from '@manutencao/models/indice';
import { InconsistenciaComponent } from '@manutencao/pages/esfera/inconsistencias/inconsistencias.component';
import { ProcessoTributarioInconsistenteService } from '@manutencao/services/processo-tributario-inconsistente.service';

@Component({
  selector: 'app-indice-correcao-esfera-modal',
  templateUrl: './indice-correcao-esfera-modal.component.html',
  styleUrls: ['./indice-correcao-esfera-modal.component.scss'],
})
export class IndicecorrecaoesferaModalComponent implements AfterViewInit {
  indice : Indice[]; 

  esferaId: number;

  constructor(
    private modal: NgbActiveModal,
    private service: IndiceCorrecaoEsferaService,
    private serviceIndice : IndiceService,
    private serviceInconsistencia : ProcessoTributarioInconsistenteService,    
    private dialogService: DialogService
  ) { }

  dataVigenciaFormControl: FormControl = new FormControl('', [ Validators.required  ]);
  indiceFromControl: FormControl = new FormControl(null,[ Validators.required  ]);

  formGroup: FormGroup = new FormGroup({
    dataVigencia : this.dataVigenciaFormControl,
    indice : this.indiceFromControl
  });

  async ngAfterViewInit(): Promise<void> {  
    this.buscarIndice();
  }

  close(): void {
    this.modal.close(false);
  }

  async save(): Promise<void> {
    try {     
      this.serviceInconsistencia.excluir();
      let resultado = await this.service.incluir(this.esferaId,this.dataVigenciaFormControl.value, this.indiceFromControl.value);      

      if (resultado && (resultado.total > 0)){
        await InconsistenciaComponent.exibeModal(resultado.total, resultado.data); 
      }
      else{
        await this.dialogService.alert(`Inclusão realizada com sucesso`);
      }

      this.modal.close(true);
    } catch (error) {
      await this.dialogService.err(`Inclusão não realizada`, (error as HttpErrorResult).messages.join('\n'));
    }
  }

  desabilitaTooltip(formControl: FormControl) {
    return formControl.untouched || formControl.valid;
  }

  async buscarIndice() {
    this.indice =  await this.serviceIndice.obter();        
  }

  static exibeModalDeIncluir(esferaId : number): Promise<boolean> {    
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(IndicecorrecaoesferaModalComponent, {windowClass:'indice-modal', centered: true, size: 'lg', backdrop: 'static' });
    modalRef.componentInstance.esferaId = esferaId;   
    return modalRef.result;
  }

}
