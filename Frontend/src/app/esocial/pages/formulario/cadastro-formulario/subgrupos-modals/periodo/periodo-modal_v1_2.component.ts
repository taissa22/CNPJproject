import { AfterViewInit, Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ErrorLib } from '@esocial/libs/error-lib';
import { Periodo } from '@esocial/models/subgrupos/v1_2/periodo';
import { ESocialListaFormularioService } from '@esocial/services/aplicacao/e-social-lista-formulario.service';
import { ContratoService } from '@esocial/services/formulario/subgrupos/contrato.service';
import { InfoIntermService } from '@esocial/services/formulario/subgrupos/infointerm.service';
import { PeriodoService } from '@esocial/services/formulario/subgrupos/periodo.service';
import { StaticInjector } from '@esocial/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';

@Component({
  selector: 'app-periodo-modal-v1-2',
  templateUrl: './periodo-modal_v1_2.component.html',
  styleUrls: ['./periodo-modal_v1_2.component.scss']
})
export class PeriodoModal_v1_2_Component implements AfterViewInit {
  periodo: Periodo = null;
  titulo: string
  consulta: boolean = false;

  idF2500: number;
  contratoId: number;
  infocontrCodcateg: number;
  infocontrIndcontr: string;
  codIdePeriodo: number;
  mesperiodo: string;

  infovlrindreperc: number;

  hrs: string = '';

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
  infoFGTSvrBcFGTSProcTrabFormControl: FormControl = new FormControl(null);
  infoFGTSvrBcFGTSSefipFormControl: FormControl = new FormControl(null);
  infoFGTSvrBcFGTSDecAntFormControl: FormControl = new FormControl(null);

  formGroup: FormGroup = new FormGroup({
    idF2500: this.idFormularioFormControl,
    logCodUsuario: this.logCodUsuarioFormControl,
    logDataOperacao: this.logDataOperacaoFormControl,
    idEsF2500Ideperiodo: this.idFormControl,
    ideperiodoPerref: this.PeriodoReferenciaFormControl,
    basecalculoVrbccpmensal: this.bcContribuicaoPrevidenciariaFormControl,
    basecalculoVrbccp13: this.bcContribuicaoPrevidenciaria13FormControl,
    infoagnocivoGrauexp: this.grauExposicaoFormControl,
    basemudcategCodcateg: this.codigoCategoriaFormControl,
    basemudcategVrbccprev: this.vrBcCPrevFormControl,
    infoFGTSvrBcFGTSProcTrab: this.infoFGTSvrBcFGTSProcTrabFormControl,
    infoFGTSvrBcFGTSSefip: this.infoFGTSvrBcFGTSSefipFormControl,
    infoFGTSvrBcFGTSDecAnt: this.infoFGTSvrBcFGTSDecAntFormControl
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
  tooltipDia: string = 'Preencher com o dia do mês efetivamente trabalhado pelo empregado com contrato de trabalho intermitente.';
  tooltipHoraTrab: string = 'Preencher com as horas trabalhadas no dia pelo empregado com contrato de trabalho intermitente. Preenchimento obrigatório e exclusivo se a classificação  tributária em S-1000 = [22 - Segurado especial, inclusive quando for empregador doméstico].';

  constructor(
    private service: PeriodoService,
    private dialogService: DialogService,
    private modal: NgbActiveModal,
    private serviceList: ESocialListaFormularioService,
    private serviceInfoInterm : InfoIntermService,
    private contratoService: ContratoService,
  ) { }

  async ngAfterViewInit() {
    this.iniciarForm();
    this.obterSimNao();
    await this.obterDados();
  }

  ngOnInit() {
    if (this.periodo) {
      this.codIdePeriodo = this.periodo.idEsF2500Ideperiodo;

      this.PeriodoReferenciaFormControl.valueChanges.subscribe(value => {
        this.mesperiodo = value;
      });
    }
    let repercProcesso = localStorage.getItem('repercProcesso');
    if (repercProcesso != '1' && repercProcesso != '5') {
      this.consulta = true;
    }
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
      this.grauExposicaoFormControl.setValue(this.periodo.infoagnocivoGrauexp);

      this.codigoCategoriaFormControl.setValue(
        this.periodo.basemudcategCodcateg
      );
      this.vrBcCPrevFormControl.setValue(this.periodo.basemudcategVrbccprev);
      this.idFormControl.setValue(this.periodo.idEsF2500Ideperiodo);

      this.infoFGTSvrBcFGTSProcTrabFormControl.setValue(
        this.periodo.infoFGTSvrBcFGTSProcTrab
      );
      this.infoFGTSvrBcFGTSSefipFormControl.setValue(
        this.periodo.infoFGTSvrBcFGTSSefip
      );
      this.infoFGTSvrBcFGTSDecAntFormControl.setValue(
        this.periodo.infoFGTSvrBcFGTSDecAnt
      );
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

        if (!await this.serviceInfoInterm.verificaInfoInterm(this.idFormControl.value, this.contratoId)) {
          await this.dialogService.alert(`${Operacao}`,`O campo Dia do Mês Trabalhado, referente ao grupo "Trabalho Intermitente" (Bloco K) será registrado com 0 (zero) indicando que não houve trabalho para este Mês/Ano.`);
          await this.serviceInfoInterm.incluirZerado(this.idF2500,this.contratoId,this.idFormControl.value);
        }

      } else {
        let obj = this.formGroup.value;
        obj.idF2500 = this.idF2500;
        await this.service.incluir(this.idF2500, this.contratoId, this.formGroup.value);
        
        if (this.infocontrCodcateg == 111 && this.infocontrIndcontr =='N') {
          await this.dialogService.alert(`${Operacao}`,`O campo Dia do Mês Trabalhado, referente ao grupo "Trabalho Intermitente" (Bloco K) será registrado com 0 (zero) indicando que não houve trabalho para este Mês/Ano.`);          
        }
      }

      

      await this.dialogService.alert(`${Operacao} realizada com sucesso`);
      this.modal.close(true);
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.err(
        `Desculpe, não foi possível realizar a ${Operacao}`,
        mensagem
      );
    }
  }

  static exibeModal(idF2500: number, contratoId: number, periodo: Periodo, infocontrIndcontr: string, infocontrCodcateg: number, infovlrindreperc:number): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      PeriodoModal_v1_2_Component,
      { windowClass: 'modal-periodo', centered: true, size: 'sm', backdrop: 'static' }
    );

    modalRef.componentInstance.periodo = periodo;
    modalRef.componentInstance.idF2500 = idF2500;
    modalRef.componentInstance.contratoId = contratoId;
    modalRef.componentInstance.infocontrIndcontr = infocontrIndcontr;
    modalRef.componentInstance.infocontrCodcateg = infocontrCodcateg;    
    modalRef.componentInstance.infovlrindreperc = infovlrindreperc;
    modalRef.componentInstance.titulo = periodo == null ? 'Incluir' : 'Alterar'
    return modalRef.result;
  }

  static exibeModalConsultar(idF2500: number, contratoId: number, periodo: Periodo, infocontrIndcontr: string, infocontrCodcateg: number): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      PeriodoModal_v1_2_Component,
      { windowClass: 'modal-periodo', centered: true, size: 'sm', backdrop: 'static' }
    );

    modalRef.componentInstance.periodo = periodo;
    modalRef.componentInstance.idF2500 = idF2500;
    modalRef.componentInstance.contratoId = contratoId;
    modalRef.componentInstance.infocontrIndcontr = infocontrIndcontr;
    modalRef.componentInstance.infocontrCodcateg = infocontrCodcateg;
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
    this.grauExposicaoFormControl.disable();
    this.codigoCategoriaFormControl.disable();
    this.vrBcCPrevFormControl.disable();
    this.infoFGTSvrBcFGTSProcTrabFormControl.disable();
    this.infoFGTSvrBcFGTSSefipFormControl.disable();
    this.infoFGTSvrBcFGTSDecAntFormControl.disable();
  }

  //#region VALIDATOR



  adicionaValidators() {    
    this.bcContribuicaoPrevidenciariaFormControl.setValidators([Validators.required, Validators.min(0)]);
    this.bcContribuicaoPrevidenciariaFormControl.updateValueAndValidity();

    this.bcContribuicaoPrevidenciaria13FormControl.setValidators([Validators.min(0)]);
    this.bcContribuicaoPrevidenciaria13FormControl.updateValueAndValidity();

    if (this.codigoCategoriaFormControl.value != null || this.vrBcCPrevFormControl.value != null) {
      this.codigoCategoriaFormControl.setValidators([Validators.required]);

      this.vrBcCPrevFormControl.setValidators([Validators.required, Validators.min(0.01)]);

    }else{
      this.codigoCategoriaFormControl.clearValidators();
      this.vrBcCPrevFormControl.setValidators([Validators.min(0.01)]);
    }
    
    if ((this.infoFGTSvrBcFGTSSefipFormControl.value != null)
      || (this.infoFGTSvrBcFGTSDecAntFormControl.value != null) 
      ||  (this.infoFGTSvrBcFGTSProcTrabFormControl.value != null )) {
      this.infoFGTSvrBcFGTSProcTrabFormControl.setValidators([Validators.required, Validators.min(0.01)]);      
    }else{
      this.infoFGTSvrBcFGTSProcTrabFormControl.clearValidators();
    }

    if (this.infoFGTSvrBcFGTSSefipFormControl.value != null 
      ) {
        this.infoFGTSvrBcFGTSSefipFormControl.setValidators([Validators.min(0.01)]);     
    }else{
      this.infoFGTSvrBcFGTSSefipFormControl.clearValidators();
    }

    if (this.infoFGTSvrBcFGTSDecAntFormControl.value != null 
      ) {
        this.infoFGTSvrBcFGTSDecAntFormControl.setValidators([Validators.min(0.01)]);    
    }else{
      this.infoFGTSvrBcFGTSDecAntFormControl.clearValidators();
    }
    
      

    this.codigoCategoriaFormControl.updateValueAndValidity();
    this.codigoCategoriaFormControl.markAsTouched();

    this.vrBcCPrevFormControl.updateValueAndValidity();
    this.vrBcCPrevFormControl.markAsTouched();

    this.infoFGTSvrBcFGTSProcTrabFormControl.updateValueAndValidity();
    this.infoFGTSvrBcFGTSProcTrabFormControl.markAsTouched();
    this.infoFGTSvrBcFGTSSefipFormControl.updateValueAndValidity();
    this.infoFGTSvrBcFGTSSefipFormControl.markAsTouched();
    this.infoFGTSvrBcFGTSDecAntFormControl.updateValueAndValidity();
    this.infoFGTSvrBcFGTSDecAntFormControl.markAsTouched();
    

  }

  desabilitaTooltip(control: FormControl):boolean{
    return control.valid || control.untouched || control.disabled
  }

  removerNaoNumericos(control: FormControl) {
    control.setValue(control.value.replace(/[^0-9]/g, ''));
  }

  //#endregion

}
