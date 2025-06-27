import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { StatusContatoService } from '@manutencao/services/status-contato.service';
import { StaticInjector } from '@manutencao/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';

@Component({
  selector: 'app-status-contato-modal',
  templateUrl: './status-contato-modal.component.html',
  styleUrls: ['./status-contato-modal.component.scss']
})
export class StatusContatoModalComponent implements OnInit {

  constructor(
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private service: StatusContatoService,
    private formBuilder: FormBuilder,
  ) { }

  ngOnInit() {
  }

  operacao = '';
  codStatusContato = 0;
  dscStatusContatoFormControl  : FormControl = new FormControl('',[Validators.required]);
  indContatoRealizadoFormControl  : FormControl = new FormControl(false);
  indAcordoRealizadoFormControl  : FormControl = new FormControl(null);
  indAtivoFormControl  : FormControl = new FormControl(true);

  formGroup: FormGroup = this.formBuilder.group({
    codStatusContato : this.codStatusContato,
    dscStatusContato : this.dscStatusContatoFormControl,
    indContatoRealizado : this.indContatoRealizadoFormControl,
    indAcordoRealizado : this.indAcordoRealizadoFormControl,
    indAtivo : this.indAtivoFormControl
  });

  public static exibeModalDeIncluir(): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(StatusContatoModalComponent, { windowClass: 'status-contato-modal', centered: true, size: 'lg', backdrop: 'static' });
      modalRef.componentInstance.operacao = "Inclusão";
      return modalRef.result;
  }

  public static exibeModalDeAlterar(codStatusContato: number): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(StatusContatoModalComponent, { windowClass: 'status-contato-modal', centered: true, size: 'lg', backdrop: 'static' });
      modalRef.componentInstance.operacao = "Alteração";
      modalRef.componentInstance.codStatusContato = codStatusContato;
      modalRef.componentInstance.obterStatusContatoAsync();
    return modalRef.result;
  }

  desabilitaTooltip(formControl: FormControl) {
    return formControl.untouched || formControl.valid;
  }

  close(): void {
    this.modal.close(false);
  }

  async save(){
    this.formGroup.get('codStatusContato').setValue(this.codStatusContato);
    console.log(this.formGroup.value)
    
    if(!this.dscStatusContatoFormControl.value){
      await this.dialogService.err(`${this.operacao} Status de Contato`, 'Campo descrição deve ser preenchido.' );
      return this.formGroup.markAllAsTouched();
    }
    
    if(this.codStatusContato == 0){
      try{
        await this.service.salvarStatusContatoAsync(this.formGroup.value);      
        await this.dialogService.alert('Cadastrar Status de Contato.', "Status de Contato incluído com sucesso.");
        this.modal.close(true);
      } catch (error) {      
        return this.dialogService.err('Cadastrar Status de Contato', error );
      }
    }else{
      try{
        await this.service.editarStatusContatoAsync(this.formGroup.value);      
        await this.dialogService.alert('Editar Status de Contato.', "Status de Contato alterado com sucesso.");
        this.modal.close(true);
      } catch (error) {      
        return this.dialogService.err('Editar Status de Contato', error );
      }
    }
  }

  async obterStatusContatoAsync(){
    try {
      const statusContatoRetorno = await this.service.obterStatusContatoAsync(this.codStatusContato);
      console.log(statusContatoRetorno)
      this.dscStatusContatoFormControl.setValue(statusContatoRetorno.dscStatusContato);
      this.indContatoRealizadoFormControl.setValue(statusContatoRetorno.indContatoRealizado == 'S');
      this.indAcordoRealizadoFormControl.setValue(statusContatoRetorno.indAcordoRealizado != null ? (statusContatoRetorno.indAcordoRealizado == 'S') : null);
      this.indAtivoFormControl.setValue(statusContatoRetorno.indAtivo == 'S');
      console.log(statusContatoRetorno)
    } catch (error) {
      await this.dialogService.err(
        "Erro ao buscar.",
        "Não foi possível realizar a busca do Status de Contato."
      );
    }
  }

}
