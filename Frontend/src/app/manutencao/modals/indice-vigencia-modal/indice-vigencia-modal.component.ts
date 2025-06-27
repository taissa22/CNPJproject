import { IndicesVigencias } from '@manutencao/models/indice-vigencias';
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
import { IndicesVigenciasService } from '@manutencao/services/indices-vigencias.service';
import { BsLocaleService } from 'ngx-bootstrap';

@Component({
  selector: 'app-indice-vigencia-modal',
  templateUrl: './indice-vigencia-modal.component.html',
  styleUrls: ['./indice-vigencia-modal.component.scss'],

})
export class IndiceVigenciaModalComponent implements AfterViewInit, OnInit {
  indice: IndicesVigencias;
  titulo: string
  indiceColuna: string = '';
  acumulado: boolean = false;
  tiposindice: any[] = [];

  public IndiceVigenciaModalComponent = IndiceVigenciaModalComponent;

  static tipoProcessoId: number;
  constructor(
    private modal: NgbActiveModal,
    private service: IndicesVigenciasService,
    private dialogService: DialogService,
    private configLocalizacao: BsLocaleService,) 
    {
      this.configLocalizacao.use('pt-BR');
    }
  ngOnInit(): void {
     this.carregaIndices();
  }

  carregaIndices(){
    this.service.obterIndices(0).subscribe(data => {
      this.tiposindice.push({})
      this.tiposindice = data;
      console.log(data)
    });
  }

  
  dataFormControl: FormControl = new FormControl('', [
    Validators.required
  ]);
  tipoIndiceFormControl: FormControl = new FormControl(null, [
    Validators.required
  ]);
  formGroup: FormGroup = new FormGroup({
    tipoIndice: this.tipoIndiceFormControl,
    data: this.dataFormControl

  });

  async ngAfterViewInit(): Promise<void> {

  }

  close(): void {
    this.modal.close(false);
  }

  async save(): Promise<void> {
    const operacao =  'Inclusão';
    try {
      
      await this.service.incluir(IndiceVigenciaModalComponent.tipoProcessoId,this.dataFormControl.value,this.tipoIndiceFormControl.value)

      await this.dialogService.alert(`${operacao} realizada com sucesso`);
      this.modal.close(true);
    } catch (error) {
      this.dataFormControl.reset()
      this.dataFormControl.reset()
      await this.dialogService.err(`${operacao} não realizada`, (error as HttpErrorResult).messages.join('\n'));
    }
  }
  desabilitaTooltip(formControl: FormControl) {
    return formControl.untouched || formControl.valid;
  }
  setData(data){
    data ? this.dataFormControl.setValue(data) : this.dataFormControl.setValue('')
  }

   static setaProcesso(processoId: number){
      this.tipoProcessoId = processoId
  }

  //#region MODAL

  // tslint:disable-next-line: member-ordering
   static exibeModalDeIncluir(tipoProcesso: number): Promise<boolean> {
    IndiceVigenciaModalComponent.tipoProcessoId = tipoProcesso;
    this.tipoProcessoId = tipoProcesso
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(IndiceVigenciaModalComponent, { centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.titulo = 'Inclusão da Vigência';
    return modalRef.result;
  }

  //#endregion MODAL
}
