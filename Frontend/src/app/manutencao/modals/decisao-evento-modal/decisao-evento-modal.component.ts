import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

// 3rd party
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

// core & shared
import { StaticInjector } from '@manutencao/static-injector';
import { DialogService } from '@shared/services/dialog.service';

// local
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';
import { HttpErrorResult } from '@core/http';
import { DecisaoEvento } from '@manutencao/models/decisao-evento.model';
import { DecisaoEventoService } from '@manutencao/services/decisao-evento.service';
import { Evento } from '@manutencao/models/evento.model';

@Component({
  selector: 'app-decisao-evento-modal',
  templateUrl: './decisao-evento-modal.component.html',
  styleUrls: ['./decisao-evento-modal.component.scss']
})
export class DecisaoEventoModalComponent implements OnInit {

  listaDecisao: Array<DecisaoEvento>

  decisaoevento: DecisaoEvento;

  perdaPotencial : any = [ {id: "PR", descricao: "Provável"},{id: "RE", descricao: "Remoto"},{id: "PO", descricao: "Possível"}];

  evento : Evento;

  constructor(
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private service: DecisaoEventoService
  ) {}

  descricaoFormControl: FormControl = new FormControl('', [
    Validators.required,
    Validators.maxLength(50)
  ]);

  alteracaoRiscoPerdaFormControl : FormControl = new FormControl(false);
  perdaPotencialFormControl: FormControl = new FormControl(null);  
  reverCalculoFormControl: FormControl = new FormControl(false);  
  decisaoDefalutFormControl: FormControl = new FormControl(false );  


  formGroup: FormGroup = new FormGroup({
    descricao: this.descricaoFormControl,
    alteracaoRiscoPerda: this.alteracaoRiscoPerdaFormControl,
    perdaPotencial: this.perdaPotencialFormControl,
    decisaoDefault : this.decisaoDefalutFormControl,
    reverCalculo : this.reverCalculoFormControl
  });

  ngOnInit(): void {
      this.InicilizaForm();
  }

  InicilizaForm() {

    if (this.evento.ehTrabalhista){
      if (this.listaDecisao.length == 0 && !this.decisaoevento){
        this.decisaoDefalutFormControl.setValue(true);
        this.decisaoDefalutFormControl.disable();
      }
    }

    if (this.decisaoevento){
      this.alteracaoRiscoPerdaFormControl.setValue(this.decisaoevento.riscoPerda);
      this.perdaPotencialFormControl.setValue(this.decisaoevento.perdaPotencial);
      this.descricaoFormControl.setValue(this.decisaoevento.descricao);    
      this.perdaPotencialFormControl.setValue(this.decisaoevento.perdaPotencial);
      this.reverCalculoFormControl.setValue(this.decisaoevento.reverCalculo);  
      this.decisaoDefalutFormControl.setValue(this.decisaoevento.decisaoDefault);

      if (this.evento.ehTrabalhista){
        if (this.decisaoevento.decisaoDefault) {
          this.decisaoDefalutFormControl.disable();
        }
      }
    }
  }

  close(): void {
    this.modal.close(false);
  }

  async save(): Promise<void> {
  
    const operacao = this.decisaoevento ? 'Alteração' : 'Inclusão';

    if (!this.descricaoFormControl.value.replace(/\s/g, '').length) {
      await this.dialogService.err(
        `${operacao} não realizada`,
        `O campo nome não pode contar apenas espaços.`
      );
      return;
    }

    if (this.alteracaoRiscoPerdaFormControl.value && (this.perdaPotencialFormControl.value == null) ) {
      await this.dialogService.err(
        `${operacao} não realizada`,
        `Informe o Risco de perda potencial.`
      );
      return;
    }

    if (this.evento.ehTrabalhista){
      if (this.decisaoDefalutFormControl.value){
        if (this.listaDecisao.find(x => x.decisaoDefault)){
          let resposta : Boolean;
          resposta = await  this.dialogService.confirm(
            `Atualmente, este evento tem como default a decisão ${this.listaDecisao.find(x => x.decisaoDefault).descricao}`,
            `Deseja alterar?.`
          );  
  
          if (!resposta){
            return;
          }
  
        }
      }
    }
   
    try {
      
      if (this.decisaoevento) {
        await this.service.alterar(this.decisaoevento.id,
                                   this.decisaoevento.eventoId,
                                   this.descricaoFormControl.value,
                                   this.alteracaoRiscoPerdaFormControl.value,
                                   this.alteracaoRiscoPerdaFormControl.value ? this.perdaPotencialFormControl.value : '',
                                   this.reverCalculoFormControl.value,
                                   this.decisaoDefalutFormControl.value);
      } else {
        await this.service.incluir(this.evento.id,
                                   this.descricaoFormControl.value,
                                   this.alteracaoRiscoPerdaFormControl.value,
                                   this.perdaPotencialFormControl.value,
                                   this.reverCalculoFormControl.value,
                                   this.decisaoDefalutFormControl.value);
      }
      await this.dialogService.alert(`${operacao} realizada com sucesso`);
      this.modal.close(true);
    }  catch (error) {
      await this.dialogService.err(`Desculpe, não foi possivel a ${operacao}`, (error as HttpErrorResult).messages.join('\n'));
    };       
  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeIncluir(evento : Evento, listaDecisao: Array<DecisaoEvento>): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(DecisaoEventoModalComponent, {windowClass: 'decisao-evento', centered: true, size: 'sm', backdrop: 'static' });
      modalRef.componentInstance.evento = evento;            
      modalRef.componentInstance.listaDecisao = listaDecisao;
    return modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeAlterar(decisaoevento: DecisaoEvento, evento : Evento, listaDecisao: Array<DecisaoEvento>): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(DecisaoEventoModalComponent, {windowClass: 'decisao-evento', centered: true, size: 'sm', backdrop: 'static' });
      modalRef.componentInstance.decisaoevento = decisaoevento; 
      modalRef.componentInstance.evento = evento;  
      modalRef.componentInstance.listaDecisao = listaDecisao;          
    return modalRef.result;
  }
}
