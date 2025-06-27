import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

// 3rd party
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

// core & shared
import { StaticInjector } from '@manutencao/static-injector';
import { DialogService } from '@shared/services/dialog.service';

// local
import { Objeto } from '@manutencao/models/objeto.model';
import { ObjetoService } from '@manutencao/services/objeto.service';
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';
import { GrupoPedidoService } from '@manutencao/services/grupo-pedido.service';
import { HttpErrorResult } from '@core/http';

@Component({
  selector: 'app-objeto-modal',
  templateUrl: './objeto-modal.component.html',
  styleUrls: ['./objeto-modal.component.scss']
})
export class ObjetoModalComponent implements OnInit {
  objeto: Objeto;
  tipoProcesso: TiposProcesso;
  gruposObjetos : Array<any>;

  constructor(
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private serviceObjeto: ObjetoService,
    private serviceGrupoPedido : GrupoPedidoService
  ) {}

  descricaoFormControl: FormControl = new FormControl('', [
    Validators.required,
    Validators.maxLength(50)
  ]);

  objTribAdmFormControl: FormControl = new FormControl(true);
  objTribJudFormControl: FormControl = new FormControl(true);
  ativoTribAdmFormControl: FormControl = new FormControl(true);
  ativoTribJudFormControl: FormControl = new FormControl(true);
  grupoIdFormControl: FormControl = new FormControl(0);

  formGroup: FormGroup = new FormGroup({
    descricao: this.descricaoFormControl,
    ehTributarioAdminstrativo: this.objTribAdmFormControl,
    ehTributarioJudicial: this.objTribJudFormControl,
    grupoId: this.grupoIdFormControl,
    ativoTributarioAdminstrativo: this.ativoTribAdmFormControl,
    ativoTributarioJudicial: this.ativoTribJudFormControl    
  });

  ngOnInit(): void {   
    this.InicilizaForm();
    this.obterComboGrupoPedido();
  }

  InicilizaForm() {
    this.formGroup.addControl('id', new FormControl(this.objeto ? this.objeto.id : 0));
    this.descricaoFormControl.setValue(this.objeto  ? this.objeto.descricao : "");
    this.ativoTribAdmFormControl.setValue(this.objeto ? this.objeto.ativoTributarioAdminstrativo: this.tipoProcesso.id === 4);
    this.ativoTribJudFormControl.setValue(this.objeto ? this.objeto.ativoTributarioJudicial: this.tipoProcesso.id === 4);
    this.objTribAdmFormControl.setValue(this.objeto ? this.objeto.ehTributarioAdministrativo: this.tipoProcesso.id === 4);
    this.objTribJudFormControl.setValue(this.objeto ? this.objeto.ehTributarioJudicial: this.tipoProcesso.id === 4);

    this.grupoIdFormControl.setValue(this.objeto ? this.objeto.grupoPedidoId : 0);      
    this.formGroup.addControl('ehTrabalhistaAdministrativo', new FormControl(this.tipoProcesso.id === 4 ? false : true ));    
  }

  close(): void {
    this.modal.close(false);
  }

  async save(): Promise<void> {
  
    const operacao = this.objeto ? 'Alteração' : 'Inclusão';

    if (!this.descricaoFormControl.value.replace(/\s/g, '').length) {
      await this.dialogService.err(
        `${operacao} não realizada`,
        `O campo nome não pode contar apenas espaços.`
      );
      return;
    }
    try {
      
      if (this.objeto) {
        await this.serviceObjeto.alterar(this.formGroup.value);
      } else {
        await this.serviceObjeto.incluir(this.formGroup.value);
      }
      await this.dialogService.alert(`${operacao} realizada com sucesso`);
      this.modal.close(true);
    }  catch (error) {
      await this.dialogService.err(`Desculpe, não foi possivel a ${operacao}`, (error as HttpErrorResult).messages.join('\n'));
    };       
  }

  public obterComboGrupoPedido(){    
    this.tipoProcesso.id === 4 ?  this.grupoIdFormControl.setValidators(Validators.required) :  this.grupoIdFormControl.clearValidators;
    this.serviceGrupoPedido.obterComboGrupoPedido(this.tipoProcesso.id).subscribe(
      gruposObjetos => this.gruposObjetos = gruposObjetos
    );
  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeIncluir(tipoProcesso: TiposProcesso): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(ObjetoModalComponent, { centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.tipoProcesso = tipoProcesso;
    return modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeAlterar(tipoProcesso: TiposProcesso, objeto: Objeto): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(ObjetoModalComponent, { centered: true, size: 'sm', backdrop: 'static' });
      modalRef.componentInstance.tipoProcesso = tipoProcesso; 
    modalRef.componentInstance.objeto = objeto;
    return modalRef.result;
  }
}
