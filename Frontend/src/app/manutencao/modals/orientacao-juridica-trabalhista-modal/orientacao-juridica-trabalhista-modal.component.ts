import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';
// angular
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

// 3rd party
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

// core & shared
import { StaticInjector } from '@manutencao/static-injector';
import { DialogService } from '@shared/services/dialog.service';
import { OrientacaoJuridicaTrabalhista } from '@manutencao/models/orientacao-juridica-trabalhista';
import { OrientacaoJuridicaService } from '@manutencao/services/orientacaoJuridica.service';
import { TipoDeOrientacaoJuridica } from '@manutencao/models/tipo-de-orientacao-juridica';
import { TipoDeOrientacaoJuridicaService } from '@manutencao/services/tipo-de-orientacao-juridica.service';
import { HttpErrorResult } from '@core/http';

@Component({
  selector: 'app-orientacao-juridica-trabalhista-modal',
  templateUrl: './orientacao-juridica-trabalhista-modal.component.html',
  styleUrls: ['./orientacao-juridica-trabalhista-modal.component.scss']
})
export class OrientacaoJuridicaTrabalhistaModalComponent implements AfterViewInit {
  orientacaoJuridicaTrabalhista: OrientacaoJuridicaTrabalhista;
  titulo: string;

  tiposOrientacoesJuridicas: Array<TipoDeOrientacaoJuridica> = [];

  constructor(
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private service: OrientacaoJuridicaService,
    private serviceTipoOrientacao: TipoDeOrientacaoJuridicaService
  ) { }

  tipoDeOrientacaoFormControl: FormControl = new FormControl(null, Validators.required);

  ativoFormControl: FormControl = new FormControl(true, [Validators.required]);
  nomeFormControl: FormControl  = new FormControl('', [Validators.required, Validators.maxLength(100)]);
  descricaoFormControl: FormControl     = new FormControl('', [Validators.required]);
  palavrasChaveFormControl: FormControl = new FormControl('', [Validators.required]);
  
  formGroup: FormGroup = new FormGroup({
    tipoOrientacao: this.tipoDeOrientacaoFormControl,
    ativo: this.ativoFormControl,
    nome: this.nomeFormControl,
    descricao: this.descricaoFormControl,
    palavrasChave: this.palavrasChaveFormControl
  });

  async ngAfterViewInit() {
    this.buscarTipoOrientacao();

    if (this.orientacaoJuridicaTrabalhista) {     
      this.tipoDeOrientacaoFormControl.setValue(this.orientacaoJuridicaTrabalhista.tipoOrientacaoJuridica? this.orientacaoJuridicaTrabalhista.tipoOrientacaoJuridica.id : null);
      this.ativoFormControl.setValue(this.orientacaoJuridicaTrabalhista.ativo);
      this.nomeFormControl.setValue(this.orientacaoJuridicaTrabalhista.nome);
      this.descricaoFormControl.setValue(this.orientacaoJuridicaTrabalhista.descricao);
      this.palavrasChaveFormControl.setValue(this.orientacaoJuridicaTrabalhista.palavrasChave);
    }   
  }

  async buscarTipoOrientacao() {    
    this.serviceTipoOrientacao.obter().subscribe(x => this.tiposOrientacoesJuridicas = x)
  }  

  close(): void {
    this.modal.close(false);
  }

  async save(): Promise<void> {
    let operacao = this.orientacaoJuridicaTrabalhista ? 'Alteração' : 'Inclusão' 

    if (!this.nomeFormControl.value.replace(/\s/g, '').length) {
      await this.dialogService.err(`${operacao} não realizada`, `O campo nome não pode contar apenas espaços.`);
      return;
    }
    try {
      if (this.orientacaoJuridicaTrabalhista) {
        await this.service.alterar(
          this.orientacaoJuridicaTrabalhista.codOrientacaoJuridica,
          this.tipoDeOrientacaoFormControl.value,
          this.nomeFormControl.value,         
          this.descricaoFormControl.value,          
          this.palavrasChaveFormControl.value,
          true,
          this.ativoFormControl.value
        );
      } else {
        await this.service.incluir(
          this.tipoDeOrientacaoFormControl.value,
          this.nomeFormControl.value,         
          this.descricaoFormControl.value,          
          this.palavrasChaveFormControl.value,
          true,
          this.ativoFormControl.value
        );
     }
      await this.dialogService.alert(`${operacao} realizada com sucesso`);
      this.modal.close(true);

    } catch (error) {
      await this.dialogService.err(`${operacao} não realizada`, (error as HttpErrorResult).messages.join('\n'));      
    }
  }

  public static exibeModalDeIncluir(): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(OrientacaoJuridicaTrabalhistaModalComponent, { centered: true, backdrop: 'static' });

    modalRef.componentInstance.titulo = 'Incluir Orientação Jurídica Trabalhista';  
    return modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeAlterar(    
    OrientacaoJuridicaTrabalhista: OrientacaoJuridicaTrabalhista
  ): Promise<boolean> {
    // prettier-ignore    
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(OrientacaoJuridicaTrabalhistaModalComponent, { centered: true, backdrop: 'static' });
            modalRef.componentInstance.titulo = 'Alterar Orientação Jurídica Trabalhista';  
            modalRef.componentInstance.orientacaoJuridicaTrabalhista = OrientacaoJuridicaTrabalhista;
    return modalRef.result;
  }

}

