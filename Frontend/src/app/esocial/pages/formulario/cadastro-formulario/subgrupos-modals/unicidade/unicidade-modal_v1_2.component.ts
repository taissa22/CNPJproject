import { AfterViewInit, Component } from '@angular/core';
import {
  FormControl,
  FormGroup,
  Validators
} from '@angular/forms';
import { ErrorLib } from '@esocial/libs/error-lib';
import { CodigoCategoria } from '@esocial/models/codigo-categoria';
import { Unicidade } from '@esocial/models/subgrupos/v1_2/unicidade';
import { ESocialListaFormularioService } from '@esocial/services/aplicacao/e-social-lista-formulario.service';
import { UnicidadeService } from '@esocial/services/formulario/subgrupos/unicidade.service';
import { StaticInjector } from '@esocial/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';

@Component({
  selector: 'app-unicidade-modal-v1-2',
  templateUrl: './unicidade-modal_v1_2.component.html',
  styleUrls: ['./unicidade-modal_v1_2.component.scss']
})
export class UnicidadeModal_v1_2_Component implements AfterViewInit {
  titulo: string;
  listaCodigoCategoria: CodigoCategoria[];

  formularioId: number;
  contratoId: number;
  temPermissaoEsocialBlocoABCDEFHI: boolean = false;

  constructor(
    private service: UnicidadeService,
    private dialogService: DialogService,
    private modal: NgbActiveModal,
    private listaService: ESocialListaFormularioService
  ) {}

  unicidade: Unicidade = null;

  idFormularioFormControl: FormControl = new FormControl(null);
  logDataOperacaoFormControl: FormControl = new FormControl(null);
  logCodUsuarioFormControl: FormControl = new FormControl(null);
  matriculaIncorporadaFormControl: FormControl = new FormControl(null);
  codigoCategoriaFormControl: FormControl = new FormControl(null);
  dataInicioFormControl: FormControl = new FormControl(null);
  idFormControl: FormControl = new FormControl(null);

  formGroup: FormGroup = new FormGroup({
    idF2500: this.idFormularioFormControl,
    logDataOperacao: this.logDataOperacaoFormControl,
    logCodUsuario: this.logCodUsuarioFormControl,
    uniccontrMatunic: this.matriculaIncorporadaFormControl,
    uniccontrCodcateg: this.codigoCategoriaFormControl,
    uniccontrDtinicio: this.dataInicioFormControl,
    idEsF2500Uniccontr: this.idFormControl
  });

  ngAfterViewInit() {
    this.obterCodigoCategoria();
    this.iniciarForm();
  }

  iniciarForm() {
    if (this.unicidade) {
      this.idFormularioFormControl.setValue(this.unicidade.idF2500);
      this.logDataOperacaoFormControl.setValue(this.unicidade.logDataOperacao);
      this.logCodUsuarioFormControl.setValue(this.unicidade.logCodUsuario);
      this.matriculaIncorporadaFormControl.setValue(
        this.unicidade.uniccontrMatunic
      );
      this.codigoCategoriaFormControl.setValue(
        this.unicidade.uniccontrCodcateg
      );
      this.dataInicioFormControl.setValue(
        this.unicidade.uniccontrDtinicio == null
          ? null
          : new Date(this.unicidade.uniccontrDtinicio)
      );
      this.idFormControl.setValue(this.unicidade.idEsF2500Uniccontr);

      if (!this.temPermissaoEsocialBlocoABCDEFHI) {
        this.idFormularioFormControl.disable();
        this.logDataOperacaoFormControl.disable();
        this.logCodUsuarioFormControl.disable();
        this.matriculaIncorporadaFormControl.disable();
        this.codigoCategoriaFormControl.disable();
        this.dataInicioFormControl.disable();
        this.idFormControl.disable();
      }
    }
  }

  async salvar() {
    const operacao = this.unicidade ? 'Alteração' : 'Inclusão';
    try {

      if (this.matriculaIncorporadaFormControl.value != null && this.matriculaIncorporadaFormControl.value == "") {
        this.matriculaIncorporadaFormControl.setValue(null);
      }

      if (this.formGroup.value.uniccontrCodcateg == null && this.formGroup.value.uniccontrMatunic == null) {
        this.codigoCategoriaFormControl.setValidators([Validators.required]);
        this.codigoCategoriaFormControl.updateValueAndValidity();
        this.formGroup.markAllAsTouched();
      }

      if (this.formGroup.value.uniccontrDtinicio == null && this.formGroup.value.uniccontrMatunic == null) {
        this.dataInicioFormControl.setValidators([Validators.required]);
        this.dataInicioFormControl.updateValueAndValidity();
        this.formGroup.markAllAsTouched();
      }

      if (this.formGroup.invalid) {
        return false;
      }

      if (this.unicidade) {
        await this.service.atualizar(
          this.formularioId,
          this.contratoId,
          this.formGroup.value
        );
      } else {
        await this.service.incluir(
          this.formularioId,
          this.contratoId,
          this.formGroup.value
        );
      }

      await this.dialogService.alert(`${operacao} realizada com sucesso`);
      this.modal.close(true);
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.err(
        `Desculpe, não foi possivel a ${operacao}`,
        mensagem
      );
    }
  }

  static exibirModalAlterar(
    formularioId: number,
    contratoId: number,
    unicidade: Unicidade,
    temPermissaoEsocialBlocoABCDEFHI: boolean
  ): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      UnicidadeModal_v1_2_Component,
      { windowClass: 'modal-unicidade',centered: true, size: 'sm', backdrop: 'static'}
    );
    modalRef.componentInstance.unicidade = unicidade;
    modalRef.componentInstance.contratoId = contratoId;
    modalRef.componentInstance.formularioId = formularioId;
    modalRef.componentInstance.temPermissaoEsocialBlocoABCDEFHI = temPermissaoEsocialBlocoABCDEFHI;
    modalRef.componentInstance.titulo = temPermissaoEsocialBlocoABCDEFHI ? 'Alterar' : 'Consultar';
    return modalRef.result;
  }

  static exibirModalIncluir(
    formularioId: number,
    contratoId: number
  ): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      UnicidadeModal_v1_2_Component,
      { windowClass: 'modal-unicidade',centered: true, size: 'sm', backdrop: 'static' }
    );
    modalRef.componentInstance.formularioId = formularioId;
    modalRef.componentInstance.contratoId = contratoId;
    modalRef.componentInstance.temPermissaoEsocialBlocoABCDEFHI = true;
    modalRef.componentInstance.titulo = 'Incluir';
    return modalRef.result;
  }

  close(): void {
    this.modal.close(false);
  }

  async obterCodigoCategoria() {
    const resposta = await this.listaService.obterCodigoCategoriaAsync();
   if (resposta) {
    this.listaCodigoCategoria = resposta.map(categoria => {
      return ({
        id: categoria.id,
        descricao: categoria.descricao,
        descricaoConcatenada: `${categoria.id} - ${categoria.descricao}`
      })
    });
   }
  }

  MatriculaChange() {
    if (this.matriculaIncorporadaFormControl.value != null && this.matriculaIncorporadaFormControl.value != "") {
      this.codigoCategoriaFormControl.clearValidators();
      this.codigoCategoriaFormControl.updateValueAndValidity();
      this.dataInicioFormControl.clearValidators();
      this.dataInicioFormControl.updateValueAndValidity();
      this.formGroup.markAllAsTouched();

      this.codigoCategoriaFormControl.setValue(null);
      this.codigoCategoriaFormControl.disable();
      this.dataInicioFormControl.setValue(null);
      this.dataInicioFormControl.disable();
    } else {

      this.codigoCategoriaFormControl.enable();
      this.dataInicioFormControl.enable();

      if (
        this.codigoCategoriaFormControl.value == null ||
        this.codigoCategoriaFormControl.value == ''
      ) {
        this.codigoCategoriaFormControl.setValidators([Validators.required]);
        this.codigoCategoriaFormControl.updateValueAndValidity();
        this.formGroup.markAllAsTouched();
      } else {
        this.codigoCategoriaFormControl.clearValidators();
        this.codigoCategoriaFormControl.updateValueAndValidity();
        this.formGroup.markAllAsTouched();
      }

      if (
        this.dataInicioFormControl.value == null ||
        this.dataInicioFormControl.value == ''
      ) {
        this.dataInicioFormControl.setValidators([Validators.required]);
        this.dataInicioFormControl.updateValueAndValidity();
        this.formGroup.markAllAsTouched();
      } else {
        this.dataInicioFormControl.clearValidators();
        this.dataInicioFormControl.updateValueAndValidity();
        this.formGroup.markAllAsTouched();
      }
    }
  }

  desabilitaTooltip(control: FormControl):boolean{
    return control.valid || control.untouched || control.disabled
  }
}
