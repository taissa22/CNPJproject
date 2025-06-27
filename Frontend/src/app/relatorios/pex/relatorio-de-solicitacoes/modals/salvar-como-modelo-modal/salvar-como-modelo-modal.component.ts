import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { RelatorioDeSolicitacoesService } from '@relatorios/services/relatorios-de-solicitacoes.service';
import { DialogService } from '@shared/services/dialog.service';
import { StaticInjector } from '../../static-injector';
import { ModeloModel } from '../../models/modelo.model';

@Component({
  selector: 'app-salvar-como-modelo-modal',
  templateUrl: './salvar-como-modelo-modal.component.html',
  styleUrls: ['./salvar-como-modelo-modal.component.scss']
})
export class SalvarComoModeloModalComponent implements OnInit {
  NomeFormControl: FormControl = new FormControl("");
  DescricaoFormControl: FormControl = new FormControl("");
  formGroup: FormGroup = new FormGroup({
    nome: this.NomeFormControl,
    descricao: this.DescricaoFormControl
  });
  model: ModeloModel = null;

  constructor(
    public modal: NgbActiveModal,
    private dialog: DialogService,
    public relatoriosService: RelatorioDeSolicitacoesService) { }

  ngOnInit() {
  }

  static exibeModal(model: ModeloModel): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(SalvarComoModeloModalComponent, { centered: true, size: 'lg', backdrop: 'static' });
    modalRef.componentInstance.model = model;
    return modalRef.result;
  }

  close() {
    this.modal.close();
  }

  async salvar() { 
    if (!this.validoParaSalvar()) return false; 
    try {
      await  this.relatoriosService.salvarModelo(this.preencherModelModelo()).then(); 
      await this.dialog.alert('Modelo salvo com sucesso');
      this.close();
    } catch (error) { 
      await this.dialog.err( error.error);
    }  
  }

  preencherModelModelo() {
    let form = this.formGroup.value;
    this.model.nome = form.nome;
    this.model.descricao = form.descricao;
    return this.model;
  }

  validoParaSalvar():boolean { 
    if (!this.NomeFormControl.value || !this.NomeFormControl.value.replace(/\s/g, '').length) {
      this.dialog.err(
        'Preencha o nome do modelo'
      );
       return false;
    }
    return true;
  }
  desabilitaTooltip(formControl: FormControl): boolean {
    return formControl.untouched || formControl.valid;
  }
}
