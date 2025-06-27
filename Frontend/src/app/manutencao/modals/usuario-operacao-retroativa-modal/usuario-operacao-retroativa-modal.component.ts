import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Form, FormControl, FormGroup, Validators } from '@angular/forms';

// 3rd party
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

// core & shared
import { StaticInjector } from '@manutencao/static-injector';
import { DialogService } from '@shared/services/dialog.service';

// local
import { Objeto } from '@manutencao/models/objeto.model';
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';
import { HttpErrorResult } from '@core/http';
import { UsuarioOperacaoRetroativa } from '@manutencao/models/usuario-operacao-retroativa';
import { UsuarioOperacaoRetroativaService } from '@manutencao/services/usuario-operacao-retroativa';
import { UsuarioModel } from '@core/models/fechamento-cc-media.model';
import { TrabalhistaJudicialComponent } from '@manutencao/manutencao-parametrizacao-closing/trabalhista-judicial/trabalhista-judicial.component';
import { catchError } from 'rxjs/operators';
import { EMPTY } from 'rxjs';
import { Usuario } from '@manutencao/models/usuario';

@Component({
  selector: 'app-usuario-operacao-retroativa-modal',
  templateUrl: './usuario-operacao-retroativa-modal.component.html',
  styleUrls: ['./usuario-operacao-retroativa-modal.component.scss']
})
export class UsuarioOperacaoRetroativaModalComponent implements AfterViewInit {
  UsuarioOperacao : UsuarioOperacaoRetroativa;

  UsuariosLista : Array<Usuario>;

  constructor(
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private service: UsuarioOperacaoRetroativaService
  ) {}

  limiteFormControl: FormControl = new FormControl(0,[Validators.required, Validators.max(31), Validators.min(1)]);
  nomeFormControl: FormControl = new FormControl("",[Validators.required]);
  codUsuarioFormControl : FormControl = new FormControl();  
  ativoFormControl : FormControl = new FormControl(true);
  
  ngAfterViewInit(){
    if (this.UsuarioOperacao){
      this.limiteFormControl.setValue(this.UsuarioOperacao.limiteAlteracao);      
      this.nomeFormControl.setValue(this.UsuarioOperacao.usuario.nome);      
      this.codUsuarioFormControl.setValue(this.UsuarioOperacao.codUsuario);
      this.ativoFormControl.setValue(this.UsuarioOperacao.usuario.ativo);      
    }      
  }

  formGroup: FormGroup = new FormGroup({
    nome: this.nomeFormControl,
    limite: this.limiteFormControl,
    codusuario : this.codUsuarioFormControl
  });
  
  close(): void {
    this.modal.close(false);
  }
  
  async save(): Promise<void> {
  
    const operacao = this.UsuarioOperacao ? 'Alteração' : 'Inclusão';        

    if ((!this.ativoFormControl.value) || 
       (this.UsuariosLista && (!this.UsuariosLista.find(x => x.id == this.nomeFormControl.value  ).ativo) ) ) {
      await this.dialogService.err(
        `${operacao} não realizada`,
        `Usuário não poderá ser selecionado, pois não está ativo no momento.`
      );
      return;
    }
    

    if (this.limiteFormControl.value > 31 || this.limiteFormControl.value < 1) {
      await this.dialogService.err(
        `${operacao} não realizada`,
        `O dia Limite deve estar entre 1 e 31.`
      );
      return;
    }

    if (!this.nomeFormControl.value.replace(/\s/g, '').length) {
      await this.dialogService.err(
        `${operacao} não realizada`,
        `O campo nome não pode contar apenas espaços.`
      );
      return;
    }
    try {
      
      if (this.UsuarioOperacao) {
        await this.service.alterar(this.codUsuarioFormControl.value,this.limiteFormControl.value);
      } else {
        await this.service.incluir(this.nomeFormControl.value,this.limiteFormControl.value);
      }
      await this.dialogService.alert(`${operacao} realizada com sucesso`);
      this.modal.close(true);
    }  catch (error) {
      await this.dialogService.err(`Desculpe, não foi possivel a ${operacao}`, (error as HttpErrorResult).messages.join('\n'));
    };       
  }

  public static exibeModalDeIncluir(usuariosLista : Array<Usuario>): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(UsuarioOperacaoRetroativaModalComponent, { centered: true, size: 'lg', backdrop: 'static' });  
      modalRef.componentInstance.UsuariosLista = usuariosLista;  
    return modalRef.result;
  }

  public static exibeModalDeAlterar(usuarioOperacao: UsuarioOperacaoRetroativa): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(UsuarioOperacaoRetroativaModalComponent, { centered: true, size: 'lg', backdrop: 'static' });
      modalRef.componentInstance.UsuarioOperacao = usuarioOperacao;
      
    return modalRef.result;
  }
}
