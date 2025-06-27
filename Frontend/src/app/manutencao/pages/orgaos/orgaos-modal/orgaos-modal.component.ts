import { Component, OnInit } from '@angular/core';
import { FormControl, Validators, FormGroup, FormArray } from '@angular/forms';
import { NgbModalRef, NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { HttpErrorResult } from '@core/http';
import { DialogService } from '@shared/services/dialog.service';

import { Orgao, Competencia, TipoOrgao } from '@manutencao/models';
import { OrgaosService } from '@manutencao/services';
import { StaticInjector } from '@manutencao/static-injector';

@Component({
  selector: 'app-orgaos-modal',
  templateUrl: './orgaos-modal.component.html',
  styleUrls: ['./orgaos-modal.component.scss']
})
export class OrgaosModalComponent implements OnInit {
  titulo: string = 'Incluir Órgão';
  orgao: Orgao;
  nomeCompetencia: string = 'Competência';
  tipoOrgao: TipoOrgao;

  nomeFormControl: FormControl = new FormControl('', [Validators.required, Validators.maxLength(400)]);
  // tslint:disable-next-line: max-line-length
  telefoneFormControl: FormControl = new FormControl('', [Validators.pattern(/^\([1-9]{2}\) ([1-9] [1-9][0-9]{3}-[0-9]{4}|[1-9][0-9]{3}-[0-9]{4})$/)]);
  competenciaFormArray: FormArray = new FormArray([]);

  formulario: FormGroup = new FormGroup({
    nome: this.nomeFormControl,
    telefone: this.telefoneFormControl,
    competencias: this.competenciaFormArray
  });

  constructor(
    private activeModal: NgbActiveModal, private dialog: DialogService,
    private orgaosService: OrgaosService) { }

  async ngOnInit(): Promise<void> {
    if (this.orgao) {
      this.nomeFormControl.setValue(this.orgao.nome);
      this.telefoneFormControl.setValue(this.orgao.telefoneCompleto);
      this.orgao.competencias
        .sort((a, b) => {
          if (a.nome < b.nome) {
            return -1;
          }
          if (a.nome > b.nome) {
            return 1;
          }
          return 0;
        })
        .forEach(p => this.competenciaFormArray.push(this.competenciaFormulario(p)));
    }
  }

  telefoneMascara(value: string): Array<string | RegExp> {
    if (value.length > 15) {
      return ['(', /[1-9]/, /\d/, ')', ' ', /[1-9]/, ' ', /[1-9]/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/];
    }
    if (value.length === 15 && value.indexOf(' ') === value.lastIndexOf(' ')) {
      return ['(', /[1-9]/, /\d/, ')', ' ', /[1-9]/, ' ', /[1-9]/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/];
    }
    return ['(', /[1-9]/, /\d/, ')', ' ', /[1-9]/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/];
  }

  private competenciaFormulario(competencia?: Competencia): FormGroup {
    const procuradoriaForGroup = new FormGroup({
      sequencial: new FormControl(null),
      nome: new FormControl('', [Validators.required, Validators.maxLength(40)])
    });

    if (competencia) {
      procuradoriaForGroup.get('sequencial').setValue(competencia.sequencial);
      procuradoriaForGroup.get('nome').setValue(competencia.nome);
    }
    return procuradoriaForGroup;
  }

  //#region MODAL

  // tslint:disable-next-line: member-ordering
  private static exibeModal(tipoOrgao: TipoOrgao): NgbModalRef {
    const modalService: NgbModal = StaticInjector.Instance.get(NgbModal);
    const modalRef = modalService.open(OrgaosModalComponent, { windowClass: 'orgao-modal', centered: true, size: 'lg', backdrop: 'static' });
    modalRef.componentInstance.tipoOrgao = tipoOrgao;
    if (tipoOrgao === TipoOrgao.CIVEL_ADMINISTRATIVO) {
      modalRef.componentInstance.nomeCompetencia = 'Procuradoria';
    }
    return modalRef;
  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeCriar(tipoOrgao: TipoOrgao): any {
    const modalRef = this.exibeModal(tipoOrgao);
    modalRef.componentInstance.titulo = 'Inclusão de Órgão';
    return modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeAlterar(orgao: Orgao): any {
    const modalRef = this.exibeModal(TipoOrgao.Todos.filter((t) => t.valor === (typeof orgao.tipoOrgao === 'number' ? orgao.tipoOrgao : orgao.tipoOrgao.valor))[0]);
    modalRef.componentInstance.titulo = 'Alterar Órgão';
    modalRef.componentInstance.orgao = orgao;
    return modalRef.result;
  }

  //#endregion MODAL

  criarCompetencia(): void {
    this.competenciaFormArray.push(this.competenciaFormulario());
  }

  removerCompetencia(index: number): void {
    this.competenciaFormArray.removeAt(index);
  }

  cancelar(): void {
    this.activeModal.close('cancel');
  }

  async confirmar(): Promise<void> {
    this.formulario.markAllAsTouched();
    if (this.formulario.invalid) {
      return;
    }

    try {
      if (this.orgao) {
        await this.atualizar();
      } else {
        await this.criar();
      }
      this.activeModal.close();
    } catch (error) {
      console.error(error);
    }
  }

  private async criar(): Promise<void> {
    try {
      const telefone: string = this.telefoneFormControl.value ? this.telefoneFormControl.value.replace(/\D/g, '') : '';
      await this.orgaosService.criar({
        nome: this.nomeFormControl.value,
        telefone: telefone,
        tipoOrgao: this.tipoOrgao,
        competencias: this.competenciaFormArray.controls.map(x => x.get('nome').value)
      });
      await this.dialog.showAlert('Cadastro realizado com sucesso', 'O Órgão foi registrado no sistema.');
    } catch (error) {
      await this.dialog.showErr('Operação não realizada', (error as HttpErrorResult).messages.join('\n'));
      throw error;
    }
  }

  private async atualizar(): Promise<void> {
    try {
      const telefone: string = this.telefoneFormControl.value ? this.telefoneFormControl.value.replace(/\D/g, '') : '';
      await this.orgaosService.atualizar({
        id: this.orgao.id,
        nome: this.nomeFormControl.value,
        telefone: telefone,
        competencias: this.competenciaFormArray.controls.map(x => {
          return {
            sequencial: x.get('sequencial').value,
            nome: x.get('nome').value
          };
        })
      });
      await this.dialog.showAlert('Cadastro atualizado com sucesso', 'O Órgão foi atualizado no sistema.');
    } catch (error) {
      await this.dialog.showErr('Operação não realizada', (error as HttpErrorResult).messages.join('\n'));
      throw error;
    }
  }

  desabilitaTooltip(formControl: FormControl): boolean {
    return formControl.untouched || formControl.valid;
  }
}
