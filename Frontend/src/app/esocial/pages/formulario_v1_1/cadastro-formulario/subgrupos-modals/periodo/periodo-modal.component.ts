import { AfterViewInit, Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ErrorLib } from '@esocial/libs/error-lib';
import { Periodo } from '@esocial/models/subgrupos/periodo';
import { ESocialListaFormularioService } from '@esocial/services/aplicacao/e-social-lista-formulario.service';
import { PeriodoService } from '@esocial/services/formulario_v1_1/subgrupos/periodo.service';
import { StaticInjector } from '@esocial/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';

@Component({
  selector: 'app-periodo-modal',
  templateUrl: './periodo-modal.component.html',
  styleUrls: ['./periodo-modal.component.scss']
})
export class PeriodoModalComponent implements AfterViewInit {
  periodo: Periodo = null;
  titulo: string
  consulta: boolean = false;

  idF2500: number;
  contratoId: number;

  idFormularioFormControl: FormControl = new FormControl(null);
  PeriodoReferenciaFormControl: FormControl = new FormControl(null);
  logCodUsuarioFormControl: FormControl = new FormControl(null);
  logDataOperacaoFormControl: FormControl = new FormControl(null);
  bcContribuicaoPrevidenciariaFormControl: FormControl = new FormControl(null);
  bcContribuicaoPrevidenciaria13FormControl: FormControl = new FormControl(null);
  bcremuneracaoFgtsSem13FormControl: FormControl = new FormControl(null);
  bcremuneracaoFgtsSobre13FormControl: FormControl = new FormControl(null); //base de calculo fgts sobre decimo terceiro
  grauExposicaoFormControl: FormControl = new FormControl(null);
  baseGerarGuiafgtsFormControl: FormControl = new FormControl(null);
  baseGerarGuiafgtssobre13FormControl: FormControl = new FormControl(null);
  pagDiretoTrabalhadorFormControl: FormControl = new FormControl(null);
  codigoCategoriaFormControl: FormControl = new FormControl(null);
  vrBcCPrevFormControl: FormControl = new FormControl(null);
  idFormControl: FormControl = new FormControl(null);

  formGroup: FormGroup = new FormGroup({
    idF2500: this.idFormularioFormControl,
    logCodUsuario: this.logCodUsuarioFormControl,
    logDataOperacao: this.logDataOperacaoFormControl,
    idEsF2500Ideperiodo: this.idFormControl,
    ideperiodoPerref: this.PeriodoReferenciaFormControl,
    basecalculoVrbccpmensal: this.bcContribuicaoPrevidenciariaFormControl,
    basecalculoVrbccp13: this.bcContribuicaoPrevidenciaria13FormControl,
    basecalculoVrbcfgts: this.bcremuneracaoFgtsSem13FormControl,
    basecalculoVrbcfgts13: this.bcremuneracaoFgtsSobre13FormControl,
    infoagnocivoGrauexp: this.grauExposicaoFormControl,
    infofgtsVrbcfgtsguia: this.baseGerarGuiafgtsFormControl,
    infofgtsVrbcfgts13guia: this.baseGerarGuiafgtssobre13FormControl,
    infofgtsPagdireto: this.pagDiretoTrabalhadorFormControl,
    basemudcategCodcateg: this.codigoCategoriaFormControl,
    basemudcategVrbccprev: this.vrBcCPrevFormControl
  });

  grauExposicaoList = []
  simNaoList = [];
  categoriaList = [];

  tooltipPeriodoRef: string = "Preencher com o mês/ano (formato AAAAMM) de referência das informações.";
  tooltipBaseCalcInssMen: string = "Preencher com o valor da base de cálculo da contribuição previdenciária sobre a remuneração mensal do trabalhador, decorrente de processo trabalhista e ainda não declarado no eSocial.";
  tooltipBaseCalcInss13: string = "Preencher com o valor da base de cálculo da contribuição previdenciária sobre a remuneração do trabalhador, referente ao 13º salário, decorrente de processo trabalhista e ainda não declarado no eSocial.";
  tooltipBaseCalcFgts: string = "Preencher com o valor da base de cálculo do FGTS sobre a remuneração do trabalhador (sem 13° salário), decorrentes de processo trabalhista e ainda não declaradas no eSocial.";
  tooltipBaseCalcFgts13: string = "Preencher com o valor da base de cálculo do FGTS sobre a remuneração do trabalhador sobre o 13º salário, decorrentes de processo trabalhista e ainda não declaradas no eSocial.";
  tooltipBaseCalcFgtsGuia: string = "Preencher com o valor da base de cálculo do FGTS sobre a remuneração do trabalhador (sem 13° salário) para geração de guia.";
  tooltipBaseCalcFgts13Guia: string = "Preencher com o valor da base de cálculo do FGTS sobre a remuneração do trabalhador sobre o 13º salário.";
  tooltipFgtsPgDirTrab: string = "Preencher com o indicador se o FGTS transacionado referente ao período de apuração pago diretamente ao trabalhador mediante decisão/autorização judicial.";
  tooltipInssMudCateg: string = "Preencher com o valor da remuneração do trabalhador a ser considerada para fins previdenciários declarada em GFIP ou em S-1200 de trabalhador sem cadastro no S-2300, no caso de reconhecimento de mudança de código de categoria.";
  tooltipCateg: string = "Preencher com o código da categoria do trabalhador declarado no período de referência conforme Tabela 01 do eSocial, no caso de reconhecimento de mudança de código de categoria.";
  tooltipGrauExp: string = "Preencher com o código que representa o grau de exposição a agentes nocivos, conforme Tabela 02 do eSocial.";

  constructor(
    private service: PeriodoService,
    private dialogService: DialogService,
    private modal: NgbActiveModal,
    private serviceList: ESocialListaFormularioService,
  ) { }

  async ngAfterViewInit() {
    this.iniciarForm();
    this.obterSimNao();
    await this.obterDados();
    this.adicionaValidators();
  }

  iniciarForm() {
    if (this.periodo) {
      this.idFormularioFormControl.setValue(this.idF2500);
      const dataArray = this.periodo.ideperiodoPerref.split('/');
      console.log(new Date(Number(dataArray[1]), Number(dataArray[0]), 1))
      this.PeriodoReferenciaFormControl.setValue(this.periodo.ideperiodoPerref ? new Date(Number(dataArray[1]), Number(dataArray[0]) - 1, 1) : null);
      this.logCodUsuarioFormControl.setValue(this.periodo.logCodUsuario);
      this.logDataOperacaoFormControl.setValue(this.periodo.logDataOperacao);
      this.bcContribuicaoPrevidenciariaFormControl.setValue(
        this.periodo.basecalculoVrbccpmensal
      );
      this.bcContribuicaoPrevidenciaria13FormControl.setValue(
        this.periodo.basecalculoVrbccp13
      );
      this.bcremuneracaoFgtsSem13FormControl.setValue(
        this.periodo.basecalculoVrbcfgts
      );
      this.bcremuneracaoFgtsSobre13FormControl.setValue(
        this.periodo.basecalculoVrbcfgts13
      );
      this.grauExposicaoFormControl.setValue(this.periodo.infoagnocivoGrauexp);
      this.baseGerarGuiafgtsFormControl.setValue(
        this.periodo.infofgtsVrbcfgtsguia
      );
      this.baseGerarGuiafgtssobre13FormControl.setValue(
        this.periodo.infofgtsVrbcfgts13guia
      );
      this.pagDiretoTrabalhadorFormControl.setValue(
        this.periodo.infofgtsPagdireto
      );
      this.codigoCategoriaFormControl.setValue(
        this.periodo.basemudcategCodcateg
      );
      this.vrBcCPrevFormControl.setValue(this.periodo.basemudcategVrbccprev);
      this.idFormControl.setValue(this.periodo.idEsF2500Ideperiodo);
    }
  }

  //#region MÉTODO PROMISE.ALL PARA FORMULARIO

  async obterDados() {
    try {
      const [
        grauExposicaoList,
        categoriaList,
      ] = await Promise.all([
        this.serviceList.obterGrauExposicaoAsync(),
        this.serviceList.obterCodigoCategoriaAsync(),
      ]);

      if (!grauExposicaoList || grauExposicaoList.length === 0) {
        throw new Error('Não foi possível obter a lista de tipos de regime previdenciário.');
      }
      if (!categoriaList || categoriaList.length === 0) {
        throw new Error('Não foi possível obter a lista de tipos de contrato de trabalho parcial.');
      }

      this.grauExposicaoList = grauExposicaoList.map(categoria => {
        return ({
          id: categoria.id,
          descricao: categoria.descricao,
          descricaoConcatenada: `${categoria.id} - ${categoria.descricao}`
        })
      });


      this.categoriaList = categoriaList.map(categoria => {
        return ({
          id: categoria.id,
          descricao: categoria.descricao,
          descricaoConcatenada: `${categoria.id} - ${categoria.descricao}`
        })
      });

    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.err('Erro ao Buscar', mensagem);
    }
  }

  //#endregion

  async obterSimNao() {
    return this.simNaoList = [
      { id: 'S', descricao: 'Sim' },
      { id: 'N', descricao: 'Não' }
    ];
  }

  async salvar() {
    this.adicionaValidators();
    this.formGroup.markAllAsTouched();

    if (this.formGroup.invalid) { return; }

    let Operacao = this.periodo ? 'Alteração' : 'Inclusão';

    try {
      if (this.periodo) {
        await this.service.atualizar(this.idF2500, this.contratoId, this.formGroup.value);
      } else {
        let obj = this.formGroup.value;
        obj.idF2500 = this.idF2500;
        await this.service.incluir(this.idF2500, this.contratoId, this.formGroup.value);
      }

      await this.dialogService.alert(`${Operacao} realizada com sucesso`);
      this.modal.close(true);
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.err(
        `Desculpe, não foi possivel a ${Operacao}`,
        mensagem
      );
    }
  }

  static exibeModal(idF2500: number, contratoId: number, periodo: Periodo): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      PeriodoModalComponent,
      { windowClass: 'modal-periodo', centered: true, size: 'sm', backdrop: 'static' }
    );

    modalRef.componentInstance.periodo = periodo;
    modalRef.componentInstance.idF2500 = idF2500;
    modalRef.componentInstance.contratoId = contratoId;
    modalRef.componentInstance.titulo = periodo == null ? 'Incluir' : 'Alterar'
    return modalRef.result;
  }

  static exibeModalConsultar(idF2500: number, contratoId: number, periodo: Periodo): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      PeriodoModalComponent,
      { windowClass: 'modal-periodo', centered: true, size: 'sm', backdrop: 'static' }
    );

    modalRef.componentInstance.periodo = periodo;
    modalRef.componentInstance.idF2500 = idF2500;
    modalRef.componentInstance.contratoId = contratoId;
    modalRef.componentInstance.consulta = true
    modalRef.componentInstance.titulo = 'Consultar'
    modalRef.componentInstance.disableAll();
    return modalRef.result;
  }

  close(): void {
    this.modal.close(false);
  }

  disableAll() {
    this.PeriodoReferenciaFormControl.disable();
    this.bcContribuicaoPrevidenciariaFormControl.disable();
    this.bcContribuicaoPrevidenciaria13FormControl.disable();
    this.bcremuneracaoFgtsSem13FormControl.disable();
    this.bcremuneracaoFgtsSobre13FormControl.disable();
    this.grauExposicaoFormControl.disable();
    this.baseGerarGuiafgtsFormControl.disable();
    this.baseGerarGuiafgtssobre13FormControl.disable();
    this.pagDiretoTrabalhadorFormControl.disable();
    this.codigoCategoriaFormControl.disable();
    this.vrBcCPrevFormControl.disable();
  }

  //#region VALIDATOR

  adicionaValidators() {

    this. bcContribuicaoPrevidenciariaFormControl.setValidators([Validators.required, Validators.min(0)]);
    this.bcContribuicaoPrevidenciariaFormControl.updateValueAndValidity();

    this.bcContribuicaoPrevidenciaria13FormControl.setValidators([Validators.required, Validators.min(0)]);
    this.bcContribuicaoPrevidenciaria13FormControl.updateValueAndValidity();

    this.bcremuneracaoFgtsSem13FormControl.setValidators([Validators.required, Validators.min(0)]);
    this.bcremuneracaoFgtsSem13FormControl.updateValueAndValidity();

    this.bcremuneracaoFgtsSobre13FormControl.setValidators([Validators.required, Validators.min(0)]);
    this.bcremuneracaoFgtsSobre13FormControl.updateValueAndValidity();

    // if (this.codigoCategoriaFormControl.value != null) {
    //   this.vrBcCPrevFormControl.setValidators([Validators.required, Validators.min(0.01)]);
    // } else {
    //   this.vrBcCPrevFormControl.setValidators([Validators.min(0.01)]);
    // }
    // this.vrBcCPrevFormControl.updateValueAndValidity();

    this.baseGerarGuiafgtsFormControl.setValidators([Validators.min(0)]);
    this.baseGerarGuiafgtsFormControl.updateValueAndValidity();

    this.baseGerarGuiafgtssobre13FormControl.setValidators([Validators.min(0)]);
    this.baseGerarGuiafgtssobre13FormControl.updateValueAndValidity();

    if (this.baseGerarGuiafgtsFormControl.value != null || this.baseGerarGuiafgtssobre13FormControl.value != null || this.pagDiretoTrabalhadorFormControl.value != null) {
      this.baseGerarGuiafgtsFormControl.setValidators([Validators.required]);

      this.baseGerarGuiafgtssobre13FormControl.setValidators([Validators.required]);

      this.pagDiretoTrabalhadorFormControl.setValidators([Validators.required]);
    }else{
      this.pagDiretoTrabalhadorFormControl.clearValidators();

      this.baseGerarGuiafgtsFormControl.setValidators([Validators.min(0)]);

      this.baseGerarGuiafgtssobre13FormControl.setValidators([Validators.min(0)]);
    }

    if (this.codigoCategoriaFormControl.value != null || this.vrBcCPrevFormControl.value != null) {
      this.codigoCategoriaFormControl.setValidators([Validators.required]);

      this.vrBcCPrevFormControl.setValidators([Validators.required, Validators.min(0.01)]);

    }else{
      this.codigoCategoriaFormControl.clearValidators();
      this.vrBcCPrevFormControl.setValidators([Validators.min(0.01)]);
    }

    this.pagDiretoTrabalhadorFormControl.updateValueAndValidity();
    this.pagDiretoTrabalhadorFormControl.markAsTouched();
    this.baseGerarGuiafgtssobre13FormControl.updateValueAndValidity();
    this.baseGerarGuiafgtssobre13FormControl.markAsTouched();
    this.baseGerarGuiafgtsFormControl.updateValueAndValidity();
    this.baseGerarGuiafgtsFormControl.markAsTouched();

    this.codigoCategoriaFormControl.updateValueAndValidity();
    this.codigoCategoriaFormControl.markAsTouched();

    this.vrBcCPrevFormControl.updateValueAndValidity();
    this.vrBcCPrevFormControl.markAsTouched();

    this.bcremuneracaoFgtsSem13FormControl.updateValueAndValidity();
    // this.bcremuneracaoFgtsSem13FormControl.markAsTouched();

    this.bcremuneracaoFgtsSobre13FormControl.updateValueAndValidity();
    // this.bcremuneracaoFgtsSobre13FormControl.markAsTouched();

  }

  desabilitaTooltip(control: FormControl):boolean{
    return control.valid || control.untouched || control.disabled
  }

  //#endregion

}
