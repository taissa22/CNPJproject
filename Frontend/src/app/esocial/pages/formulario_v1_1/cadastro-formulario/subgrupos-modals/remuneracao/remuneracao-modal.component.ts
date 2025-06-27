import { registerLocaleData } from '@angular/common';
import { AfterViewInit, Component, LOCALE_ID } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ErrorLib } from '@esocial/libs/error-lib';
import { Remuneracao } from '@esocial/models/subgrupos/remuneracao';
import { UnidadePagamento } from '@esocial/models/unidade-pagamento';
import { ESocialListaFormularioService } from '@esocial/services/aplicacao/e-social-lista-formulario.service';
import { RemuneracaoService } from '@esocial/services/formulario_v1_1/subgrupos/remuneracao.service';
import { StaticInjector } from '@esocial/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';

@Component({
  selector: 'app-remuneracao-modal',
  templateUrl: './remuneracao-modal.component.html',
  styleUrls: ['./remuneracao-modal.component.scss'],
  providers: [{ provide: LOCALE_ID, useValue: 'pt-BR' }
  ]
})
export class RemuneracaoModalComponent implements AfterViewInit {

  titulo: string
  listaUnidadePagamento: UnidadePagamento[];
  formularioId: number;
  contratoId: number;
  totalCaracterDesc: number = 0;
  temPermissaoEsocialBlocoGK: boolean = false;

  dtMin: Date;
  dtMax: Date;

  tooltipDtVigencia: string = "Preencher com a data a partir da qual as informações de remuneração e periodicidade de pagamento estão vigentes.";
  tooltipSalBase: string = "Preencher com a data a partir da qual as informações de remuneração e periodicidade de pagamento estão vigentes.";
  tooltipUniPag: string = "Preencher com o indicador da unidade de pagamento da parte fixa da remuneração.";
  tooltipDescSal: string = "Preencher com a descrição do salário por tarefa ou variável e como este é calculado. Ex.: Comissões pagas no percentual de 10% sobre as vendas.";


  constructor(
    private service: RemuneracaoService,
    private dialogService: DialogService,
    private modal: NgbActiveModal,
    private listasService: ESocialListaFormularioService

  ) {
    registerLocaleData(LOCALE_ID, 'pt');

  }

  remuneracao: any = null;
  dataRemuneracaoFormControl: FormControl = new FormControl(null);
  valorSalaBaseFormControl: FormControl = new FormControl(null);
  unidadePagamentoIdFormControl: FormControl = new FormControl(null, [Validators.required]);
  descricaoSalarioVariavelIdFormControl: FormControl = new FormControl(null);
  logDataOperacaoFormControl: FormControl = new FormControl(null);
  logCodUsuarioFormControl: FormControl = new FormControl(null);
  idFormularioFormControl: FormControl = new FormControl(null);
  idFormControl: FormControl = new FormControl(null);

  formGroup: FormGroup = new FormGroup({
    idF2500: this.idFormControl,
    remuneracaoDtremun: this.dataRemuneracaoFormControl,
    remuneracaoVrsalfx: this.valorSalaBaseFormControl,
    remuneracaoUndsalfixo: this.unidadePagamentoIdFormControl,
    remuneracaoDscsalvar: this.descricaoSalarioVariavelIdFormControl,
    logDataOperacao: this.logDataOperacaoFormControl,
    logCodUsuario: this.logCodUsuarioFormControl,
    idEsF2500Remuneracao: this.idFormularioFormControl
  });

  ngAfterViewInit() {
    this.obterUnidadePagamento();
    this.iniciarForm();
    this.validator();
  }

  iniciarForm() {
    if (this.remuneracao) {
      this.idFormControl.setValue(this.remuneracao.idF2500);
      this.dataRemuneracaoFormControl.setValue( this.remuneracao.remuneracaoDtremun == null ? null :  new Date(this.remuneracao.remuneracaoDtremun));
      this.valorSalaBaseFormControl.setValue(
        this.remuneracao.remuneracaoVrsalfx
      );
      this.unidadePagamentoIdFormControl.setValue(
        this.remuneracao.remuneracaoUndsalfixo
      );
      this.descricaoSalarioVariavelIdFormControl.setValue(
        this.remuneracao.remuneracaoDscsalvar
      );
      this.logDataOperacaoFormControl.setValue(
        this.remuneracao.logDataOperacao
      );
      this.logCodUsuarioFormControl.setValue(this.remuneracao.logCodUsuario);
      this.idFormularioFormControl.setValue(
        this.remuneracao.idEsF2500Remuneracao
      );

      if (!this.temPermissaoEsocialBlocoGK) {
        this.idFormControl.disable();
      this.dataRemuneracaoFormControl.disable();
      this.valorSalaBaseFormControl.disable();
      this.unidadePagamentoIdFormControl.disable();
      this.descricaoSalarioVariavelIdFormControl.disable();
      this.logDataOperacaoFormControl.disable();
      this.logCodUsuarioFormControl.disable()
      this.idFormularioFormControl.disable();
      }
    }
    else {
      this.unidadePagamentoIdFormControl.setValue(5);
    }
  }

  async salvar() {
    const operacao = this.remuneracao ? 'Alteração' : 'Inclusão';

    try {
      if (this.remuneracao) {
        await this.service.atualizar(this.formularioId, this.contratoId, this.formGroup.value);
      } else {
        await this.service.incluir(this.formularioId, this.contratoId, this.formGroup.value);
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

  static exibeModalAlterar(formularioId: number, contratoId: number, remuneracao: Remuneracao, dtMin: Date, dtMax: Date, temPermissaoEsocialBlocoGK: boolean): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      RemuneracaoModalComponent,
      { windowClass: 'modal-remuneracao', centered: true, size: '540px', backdrop: 'static' }
    );
    modalRef.componentInstance.remuneracao = remuneracao;
    modalRef.componentInstance.formularioId = formularioId;
    modalRef.componentInstance.contratoId = contratoId;
    modalRef.componentInstance.dtMin = dtMin;
    modalRef.componentInstance.dtMax = dtMax;
    modalRef.componentInstance.temPermissaoEsocialBlocoGK = temPermissaoEsocialBlocoGK;

    modalRef.componentInstance.titulo = temPermissaoEsocialBlocoGK ? 'Alterar' : 'Consultar' ;
    return modalRef.result;
  }

  static exibeModalIncluir(formularioId: number, contratoId: number, dtMin: Date, dtMax: Date): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      RemuneracaoModalComponent,
      { windowClass: 'modal-remuneracao', centered: true, size: '540px', backdrop: 'static' }
    );
    modalRef.componentInstance.formularioId = formularioId;
    modalRef.componentInstance.contratoId = contratoId;
    modalRef.componentInstance.titulo = 'Incluir';
    modalRef.componentInstance.dtMin = dtMin;
    modalRef.componentInstance.dtMax = dtMax;
    modalRef.componentInstance.temPermissaoEsocialBlocoGK = true;
    return modalRef.result;
  }

  close(): void {
    this.modal.close(false);
  }

  async obterUnidadePagamento() {
    const respostaUnidadePagamento =
      await this.listasService.obterUnidadePagamentoAsync();
    if (respostaUnidadePagamento) {
      this.listaUnidadePagamento = respostaUnidadePagamento.map(unidadePagamento => {
        return {
          id: unidadePagamento.id,
          descricao: unidadePagamento.descricao,
          descricaoConcatenada: `${unidadePagamento.id} - ${unidadePagamento.descricao}`
        };
      });
    }
  }

  calculaTamanho(item) {
    this.totalCaracterDesc = item.length;
}

  validator() {

    if (this.unidadePagamentoIdFormControl.value == 6 || this.unidadePagamentoIdFormControl.value == 7) {
      this.descricaoSalarioVariavelIdFormControl.setValidators([Validators.required]);
    } else {
      this.descricaoSalarioVariavelIdFormControl.clearValidators();
    }
    this.descricaoSalarioVariavelIdFormControl.updateValueAndValidity();

    if (this.unidadePagamentoIdFormControl.value == 7) {
      this.valorSalaBaseFormControl.setValidators([Validators.required, Validators.min(0), Validators.max(0)]);
    } else {
      this.valorSalaBaseFormControl.setValidators([Validators.required]);
    }
    this.valorSalaBaseFormControl.updateValueAndValidity();

  }

  desabilitaTooltip(control: FormControl):boolean{
    return control.valid || control.untouched || control.disabled
  }
}
