import { AfterViewInit, Component  } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

// 3rd party
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

// core & shared
import { StaticInjector } from '@manutencao/static-injector';
import { DialogService } from '@shared/services/dialog.service';

// local
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';
import { HttpErrorResult } from '@core/http';
import { Advogado } from '@manutencao/models/advogado.model';
import { AdvogadoService } from '@manutencao/services/advogado.service';
import { Estados } from '@core/models';
@Component({
  selector: 'app-advogado-modal',
  templateUrl: './advogado-modal.component.html',
  styleUrls: ['./advogado-modal.component.scss']
})
export class AdvogadoModalComponent implements  AfterViewInit {
  advogado: Advogado = null;
  abaDadosGerais : boolean = true;
  ativo : boolean = true;
  escritorioId : number;
  temContato : boolean = false;

  estados: Array<Estados> = Estados.obterUfs();

  constructor(
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private service : AdvogadoService
  ) {}


  nomeFormControl: FormControl = new FormControl('', [
    Validators.required,
    Validators.maxLength(50)
  ]);

  estadoFormControl : FormControl = new FormControl('',[Validators.required]);
  numeroOABFormControl : FormControl = new FormControl('',[Validators.required]);
  telefoneFormControl : FormControl = new FormControl('',[]);
  emailFormControl : FormControl = new FormControl('',[]);
  ehContatoFormControl : FormControl = new FormControl(false,[]);
  escritorioIdFormControl : FormControl = new FormControl('',[]);
  telefoneDDDFormControl : FormControl = new FormControl('',[]);


  formGroup: FormGroup = new FormGroup({
    Nome: this.nomeFormControl,
    EhContato : this.ehContatoFormControl,
    email : this.emailFormControl,
    telefone : this.telefoneFormControl,
    telefoneDDD : this.telefoneDDDFormControl,
    numeroOAB : this.numeroOABFormControl,
    estado : this.estadoFormControl,
    escritorioId : this.escritorioIdFormControl
  });

  ngAfterViewInit(): void {
    
    this.escritorioIdFormControl.setValue(this.escritorioId);

    if (this.advogado) {
      this.nomeFormControl.setValue(this.advogado.nome);
      this.estadoFormControl.setValue(this.advogado.estadoOAB);
      this.telefoneFormControl.setValue(this.advogado.telefone);
      this.telefoneDDDFormControl.setValue(this.advogado.telefoneDDD);
      this.numeroOABFormControl.setValue(this.advogado.numeroOAB);
      this.ehContatoFormControl.setValue(this.advogado.ehContato);
      this.emailFormControl.setValue(this.advogado.email);
    }
  }

  close(): void {
    this.modal.close(false);
  }


  async save(): Promise<void> {
    const operacao = this.advogado ? 'Alteração' : 'Inclusão';

    if ( (this.emailFormControl.value.replace(/\s/g, '').length )  && (!this.emailValido(this.emailFormControl.value)) )
    {
      await this.dialogService.err(
        `${operacao} não realizada`,
        `Email informado está inválido.`
      );
      return;
    }


    if (this.temContato && this.ehContatoFormControl.value){
      if (!await this.dialogService.confirm(
        `Já existe um advogado como contato deste escritório.`,
        `Deseja alterar o contato?`)
      )
      {
        this.ehContatoFormControl.setValue(false);
      }
    }

    if (!this.nomeFormControl.value.replace(/\s/g, '').length) {
      await this.dialogService.err(
        `${operacao} não realizada`,
        `O campo nome não pode conter apenas espaços.`
      );
      return;
    }


    try {
      if (this.advogado)
      {
        
        let obj :any =  this.formGroup.value;
        obj.Id = this.advogado.id;
        await this.service.alterar(obj);
      }
      else
      {
        await this.service.incluir(this.formGroup.value);
      }

      await this.dialogService.alert(`${operacao} realizada com sucesso`);
      this.modal.close(true);

    }  catch (error) {
      console.log(error);
      await this.dialogService.err(`${operacao} não realizada`, (error as HttpErrorResult).messages.join('\n'));
    };
  }
  // tslint:disable-next-line: member-ordering
  public static exibeModalDeIncluir(escritorioId : number, temContato : boolean ): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(AdvogadoModalComponent, {windowClass: 'advogado-modal', centered: true, size: 'lg', backdrop: 'static' });
      modalRef.componentInstance.escritorioId = escritorioId;
      modalRef.componentInstance.temContato = temContato;
    return modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeAlterar(advogado: Advogado,escritorioId : number, temContato : boolean): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(AdvogadoModalComponent, {windowClass: 'advogado-modal', centered: true, size: 'lg', backdrop: 'static' });
      modalRef.componentInstance.advogado = advogado;
      modalRef.componentInstance.escritorioId = escritorioId;
      modalRef.componentInstance.temContato = temContato;
    return modalRef.result;
  }


  emailValido (email) {
    var reg = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/
    if (reg.test(email)){
       return true; }
    else{
      return false;
    }
  }

}
