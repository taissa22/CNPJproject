import { AfterViewInit, Component, Input } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ErrorLib } from '@esocial/libs/error-lib';
import { Contrato } from '@esocial/models/subgrupos/contrato';
import { ESocialListaFormularioService } from '@esocial/services/aplicacao/e-social-lista-formulario.service';
import { ContratoService } from '@esocial/services/formulario_v1_1/subgrupos/contrato.service';
import { StaticInjector } from '@esocial/static-injector';
import { EsocialFormcontrolCustomValidators } from '@esocial/validators/esocial-formcontrol-custom-validators';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';
import { BsLocaleService } from 'ngx-bootstrap';

@Component({
  selector: 'app-contrato-modal',
  templateUrl: './contrato-modal.component.html',
  styleUrls: ['./contrato-modal.component.scss']
})
export class ContratoModalComponent implements AfterViewInit {

  @Input() idF2500 = null;
  @Input() contrato: Contrato = null;
  @Input() exibirGerarFicticia: boolean = false;

  consulta: boolean = false;
  titulo: string = '';

  tooltipIndContr: string = 'Preencher com o indicativo se o contrato possui informação no evento S-2190, S-2200 ou S-2300 no declarante.'
  tooltipTipContr: string = 'Preencher com o tipo de contrato a que se refere o processo judicial ou a demanda submetida à CCP.';
  tooltipDtAdmOriginal: string = 'Preencher com a data de admissão original do vínculo (data de admissão antes da alteração).';
  tooltipReintegracao: string = 'Preencher com o indicativo de reintegração do empregado.';
  tooltipCateDifCont: string = 'Preencher com o indicativo se houve reconhecimento de categoria do trabalhador diferente da cadastrada (no eSocial ou na GFIP) pelo declarante.';
  tooltipNatDifCont: string = 'Preencher com o indicativo se houve reconhecimento de natureza da atividade diferente da cadastrada pelo declarante.';
  tooltipMotDeslDifCont: string = 'Preencher com o indicativo se houve reconhecimento de motivo de desligamento diferente do informado pelo declarante';
  tooltipUnidContrat: string = 'Preencher com o indicativo se houve reconhecimento de unicidade contratual (declaração da continuidade do contrato de trabalho, considerando como único dois ou mais vínculos sucessivos informados no eSocial).';
  tooltipMotivoTermino : string = 'Preencher com o indicador do motivo do término do diretor não empregado, com FGTS.';
  tooltipMatricula: string = 'Preencher com a matrícula atribuída ao trabalhador pela empresa. Para processos de terceiros, caso essa informação seja desconhecida, clique no link ao lado do campo para que o sistema gere uma matrícula fictícia.';
  validaDataInicioTSVE: boolean = false;
  validaDataAdmissaoOriginal: boolean = false;
  dataAtual: Date = new Date();
  dataMaximaTerminoTSVE: Date = new Date(
    this.dataAtual.getFullYear(),
    this.dataAtual.getMonth(),
    this.dataAtual.getDate() + 10
  );
  dataMaximaAdmissaoOriginal: Date = new Date(
    this.dataAtual.getFullYear(),
    this.dataAtual.getMonth(),
    (this.dataAtual.getDate() - 1)
  );

  tipoContratoLista = [];
  listaCategoria = [];
  listaCBO = [];
  listaSimNao = [
    { id: 'S', descricao: 'Sim' },
    { id: 'N', descricao: 'Não' }
  ];
  listaMotivoDesligamento = [];
  listaNaturezaAtividade = [];

  IdFormulario: FormControl = new FormControl(null);
  logDataOperacaoFormControl: FormControl = new FormControl(null);
  lodCodUsuarioFormControl: FormControl = new FormControl(null);
  tipoContratoFormControl: FormControl = new FormControl(
    null,
    Validators.required
  );
  possuiInformacaoContribuicaoFormControl: FormControl = new FormControl(
    null,
    Validators.required
  );
  dataAdmissaoOriginalFormControl: FormControl = new FormControl(null);
  reintegracaoEmpregadoFormControl: FormControl = new FormControl(null);
  categoriaDiferenteCadastradaFormControl: FormControl = new FormControl(
    null,
    Validators.required
  );
  naturezaDiferenteCadastradaFormControl: FormControl = new FormControl(
    null,
    Validators.required
  );
  motivoDesligamentoDiferenteContratoFormControl: FormControl = new FormControl(
    null,
    Validators.required
  );
  unicidadeContratualFormControl: FormControl = new FormControl(null);
  matriculaFormControl: FormControl = new FormControl(null);
  codigoCategoriaFormControl: FormControl = new FormControl(null);
  dataInicioContratoFormControl: FormControl = new FormControl(null);
  idFormControl: FormControl = new FormControl(null);
  dataTerminoContratoFormControl: FormControl = new FormControl(null);
  motivoDesligamentoFormControl: FormControl = new FormControl(null);
  naturezaAtividadeFormControl: FormControl = new FormControl(null);
  codigoCboFormControl: FormControl = new FormControl(null);

  formGroup: FormGroup = new FormGroup({
    idF2500: this.IdFormulario,
    logDataOperacao: this.logDataOperacaoFormControl,
    logCodUsuario: this.lodCodUsuarioFormControl,
    idEsF2500Infocontrato: this.idFormControl,


    infocontrTpcontr: this.tipoContratoFormControl,
    infocontrIndcontr: this.possuiInformacaoContribuicaoFormControl,
    infocontrDtadmorig: this.dataAdmissaoOriginalFormControl,
    infocontrIndreint: this.reintegracaoEmpregadoFormControl,
    infocontrIndcateg: this.categoriaDiferenteCadastradaFormControl,
    infocontrIndnatativ: this.naturezaDiferenteCadastradaFormControl,
    infocontrIndmotdeslig: this.motivoDesligamentoDiferenteContratoFormControl,
    infocontrIndunic: this.unicidadeContratualFormControl,
    infocontrMatricula: this.matriculaFormControl,
    infocontrCodcateg: this.codigoCategoriaFormControl,
    infocontrDtinicio: this.dataInicioContratoFormControl,
    infocomplCodcbo: this.codigoCboFormControl,
    infotermDtterm: this.dataTerminoContratoFormControl,
    infocomplNatatividade: this.naturezaAtividadeFormControl,
    infotermMtvdesligtsv: this.motivoDesligamentoFormControl
  });

  constructor(
    private service: ContratoService,
    private dialogService: DialogService,
    private modal: NgbActiveModal,
    private configLocalizacao: BsLocaleService,
    private listaService: ESocialListaFormularioService,
    private esocialCustonValidator: EsocialFormcontrolCustomValidators
  ) {
    this.configLocalizacao.use('pt-BR');
  }
  ngAfterViewInit(): void {
    this.obterListaCombo();
    this.iniciarForm();
    this.ajustaValidators();
  }

  iniciarForm() {
    if (this.contrato) {
      this.IdFormulario.setValue(this.contrato.idF2500);
      this.logDataOperacaoFormControl.setValue(this.contrato.logDataOperacao);
      this.lodCodUsuarioFormControl.setValue(this.contrato.logCodUsuario);
      this.tipoContratoFormControl.setValue(this.contrato.infocontrTpcontr);

      this.possuiInformacaoContribuicaoFormControl.setValue(
        this.contrato.infocontrIndcontr
      );
      this.dataAdmissaoOriginalFormControl.setValue(
        this.contrato.infocontrDtadmorig
          ? new Date(this.contrato.infocontrDtadmorig)
          : null
      );
      this.reintegracaoEmpregadoFormControl.setValue(
        this.contrato.infocontrIndreint
      );
      this.categoriaDiferenteCadastradaFormControl.setValue(
        this.contrato.infocontrIndcateg
      );
      this.naturezaDiferenteCadastradaFormControl.setValue(
        this.contrato.infocontrIndnatativ
      );
      this.motivoDesligamentoDiferenteContratoFormControl.setValue(
        this.contrato.infocontrIndmotdeslig
      );
      this.unicidadeContratualFormControl.setValue(
        this.contrato.infocontrIndunic
      );

      this.matriculaFormControl.setValue(this.contrato.infocontrMatricula);
      this.codigoCategoriaFormControl.setValue(this.contrato.infocontrCodcateg);
      // this.codigoCategoriaFormControl.setValue(this.contrato.infocontrCodcateg);
      this.dataInicioContratoFormControl.setValue(
        this.contrato.infocontrDtinicio
          ? new Date(this.contrato.infocontrDtinicio)
          : null
      );
      this.codigoCboFormControl.setValue(this.contrato.infocomplCodcbo ? Number(this.contrato.infocomplCodcbo) : null);
      this.naturezaAtividadeFormControl.setValue(this.contrato.infocomplNatatividade);
      this.dataTerminoContratoFormControl.setValue(this.contrato.infotermDtterm
        ? new Date(this.contrato.infotermDtterm)
        : null);
      this.motivoDesligamentoFormControl.setValue(Number(this.contrato.infotermMtvdesligtsv));

      this.idFormControl.setValue(this.contrato.idEsF2500Infocontrato);

      this.exibirGerarFicticia = this.contrato.indProprioTerceiro == 'T';

    }

    if (this.dataInicioContratoFormControl.value == null) {
      this.dataTerminoContratoFormControl.setValue(null);
      this.dataTerminoContratoFormControl.disable();
    }
  }

  async salvar() {
    this.ajustaValidators();
    this.formGroup.markAllAsTouched();
    if (this.formGroup.invalid) {
      return;
    }

    let Operacao = this.contrato ? 'Alteração' : 'Inclusão';
    try {
      let obj = this.formGroup.value;
      if (this.contrato) {
        await this.service.alterar(obj);
      } else {
        let obj = this.formGroup.value;
        obj.idF2500 = this.idF2500;
        await this.service.incluir(obj);
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

  static exibeModalAlterar(contrato: Contrato): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      ContratoModalComponent,
      {
        windowClass: 'modal-contrato',
        centered: true,
        size: 'md',
        backdrop: 'static'
      }
    );
    modalRef.componentInstance.contrato = contrato;
    modalRef.componentInstance.titulo = 'Alterar Contrato';
    return modalRef.result;
  }

  static exibeModalConsultar(contrato: Contrato): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      ContratoModalComponent,
      {
        windowClass: 'modal-contrato',
        centered: true,
        size: 'md',
        backdrop: 'static'
      }
      );
      modalRef.componentInstance.contrato = contrato;
      modalRef.componentInstance.titulo = 'Consultar Contrato';
      modalRef.componentInstance.consulta = true;
      modalRef.componentInstance.disableAll();
      return modalRef.result;
  }

  static exibeModalIncluir(f2500: number, contratoTerceiro: boolean): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      ContratoModalComponent,
      {
        windowClass: 'modal-contrato',
        centered: true,
        size: 'md',
        backdrop: 'static'
      }
      );
      modalRef.componentInstance.idF2500 = f2500;
      modalRef.componentInstance.exibirGerarFicticia = contratoTerceiro;
      modalRef.componentInstance.titulo = 'Incluir Contrato';
      return modalRef.result;
  }

  close(): void {
    this.modal.close(false);
  }

  async obterListaCombo() {
    const respostaTipoContrato =
      await this.listaService.obterTipoContratoTSVEAsync();
    if (respostaTipoContrato) {
      this.tipoContratoLista = respostaTipoContrato.map(tipoContrato => {
        return {
          id: tipoContrato.id,
          descricao: tipoContrato.descricao,
          descricaoConcatenada: `${tipoContrato.id} - ${tipoContrato.descricao}`
        };
      });
    }

    const respostaMotivoDeslig =
      await this.listaService.obterMotivoDesligamentoAsync();
    if (respostaMotivoDeslig) {
      this.listaMotivoDesligamento = respostaMotivoDeslig.map(motivo => {
        return {
          id: motivo.id,
          descricao: motivo.descricao,
          descricaoConcatenada: `${motivo.id} - ${motivo.descricao}`
        };
      });
    }

    const respostaCategoria =
      await this.listaService.obterCodigoCategoriaAsync();
    if (respostaCategoria) {
      this.listaCategoria = respostaCategoria.map(categoria => {
        return {
          id: categoria.id,
          descricao: categoria.descricao,
          descricaoConcatenada: `${categoria.id} - ${categoria.descricao}`
        };
      });
    }

    const respostaCBO = await this.listaService.obterCodigoCBOAsync();
    if (respostaCBO) {
      this.listaCBO = respostaCBO.map(cbo => {
        return {
          id: cbo.id,
          descricao: cbo.descricao,
          descricaoConcatenada: `${cbo.id.toString().padStart(6,'0')} - ${cbo.descricao}`
        };
      });
    }

    const respostaNatureza =
      await this.listaService.obterNaturezaAtividadeAsync();
    if (respostaNatureza) {
      this.listaNaturezaAtividade = respostaNatureza.map(motivo => {
        return {
          id: motivo.id,
          descricao: motivo.descricao,
          descricaoConcatenada: `${motivo.id} - ${motivo.descricao}`
        };
      });
    }
  }

  ajustaDataTermindoTSVE(evento: any) {
    if (this.possuiInformacaoContribuicaoFormControl.value == 'S') {
      this.dataTerminoContratoFormControl.setValue(null);
      this.dataTerminoContratoFormControl.disable();
    }else{
      if (
        evento != null &&
        this.tipoContratoFormControl.value != null &&
        this.tipoContratoFormControl.value === 6
      ) {
        this.dataTerminoContratoFormControl.enable();
      } else {
        this.dataTerminoContratoFormControl.setValue(null);
        this.dataTerminoContratoFormControl.disable();
      }
    }    
  }

  ajustaMotivoTerminidoTSVE(evento: any) {
    if (this.possuiInformacaoContribuicaoFormControl.value == 'S') {
      this.motivoDesligamentoFormControl.setValue(null)
      this.motivoDesligamentoFormControl.disable();
    }else{
      if (evento != null
        && (this.codigoCategoriaFormControl.value != null
          && this.codigoCategoriaFormControl.value === 721)
      ) {
        this.motivoDesligamentoFormControl.enable();
      } else {
        this.motivoDesligamentoFormControl.setValue(null)
        this.motivoDesligamentoFormControl.disable();
      }
    }
  }

  ajustaValidators() {   
    if (
      this.tipoContratoFormControl.value != null &&
      (this.tipoContratoFormControl.value == 2 ||
        this.tipoContratoFormControl.value == 4)
    ) {
      this.validaDataAdmissaoOriginal = true;
    } else {
      this.validaDataAdmissaoOriginal = false;
    }

    if (this.tipoContratoFormControl.value != null &&
      (this.tipoContratoFormControl.value != 2 &&
      this.tipoContratoFormControl.value != 4)) {
        this.dataAdmissaoOriginalFormControl.setValue(null);
        this.dataAdmissaoOriginalFormControl.disable();
    }else{
      this.dataAdmissaoOriginalFormControl.enable();
    }

    if (
      (this.tipoContratoFormControl.value != null &&
      this.tipoContratoFormControl.value == 6) ||
      (this.possuiInformacaoContribuicaoFormControl.value != null &&
        this.possuiInformacaoContribuicaoFormControl.value == 'N'))
    {
      this.reintegracaoEmpregadoFormControl.setValue(null);
      this.reintegracaoEmpregadoFormControl.disable();

    } else {
      this.reintegracaoEmpregadoFormControl.enable();
    }

    if (
      this.tipoContratoFormControl.value != null &&
      this.tipoContratoFormControl.value != 6 &&
      this.possuiInformacaoContribuicaoFormControl.value != null &&
      this.possuiInformacaoContribuicaoFormControl.value == 'S'
    ) {
      this.reintegracaoEmpregadoFormControl.setValidators([
        Validators.required
      ]);
    } else {
      this.reintegracaoEmpregadoFormControl.clearValidators();
    }
    this.reintegracaoEmpregadoFormControl.updateValueAndValidity();
    this.reintegracaoEmpregadoFormControl.markAsTouched();

    if (
      this.matriculaFormControl.value == null ||
      (this.matriculaFormControl.value != null &&
        this.matriculaFormControl.value == '') ||
      (this.possuiInformacaoContribuicaoFormControl.value != null &&
        this.possuiInformacaoContribuicaoFormControl.value == 'N')
    ) {
      this.codigoCategoriaFormControl.setValidators([Validators.required]);
    } else {
      this.codigoCategoriaFormControl.clearValidators();
    }
    this.codigoCategoriaFormControl.updateValueAndValidity();
    this.codigoCategoriaFormControl.markAsTouched();

    // if (this.tipoContratoFormControl.value != null &&
    //   this.tipoContratoFormControl.value != 6 ||
    //   (this.possuiInformacaoContribuicaoFormControl.value != null &&
    //     this.possuiInformacaoContribuicaoFormControl.value == 'S' && (this.matriculaFormControl != null && this.matriculaFormControl.value != ''))) {
    //     this.dataInicioContratoFormControl.setValue(null);
    //     this.dataInicioContratoFormControl.disable();
    // }else{
    //   this.dataInicioContratoFormControl.enable();
    // }

    if (
      ((this.tipoContratoFormControl.value != null &&
        this.tipoContratoFormControl.value === 6) &&
        (this.possuiInformacaoContribuicaoFormControl.value != null &&
          this.possuiInformacaoContribuicaoFormControl.value == 'N')) ||
      (this.matriculaFormControl.value == null ||
        (this.matriculaFormControl.value != null &&
          this.matriculaFormControl.value == ''))
    ) {
      this.dataInicioContratoFormControl.enable();
      this.validaDataInicioTSVE = true;
    } else {
      this.dataInicioContratoFormControl.setValue(null);
      this.dataInicioContratoFormControl.disable();
      this.validaDataInicioTSVE = false;
    }
    this.dataInicioContratoFormControl.updateValueAndValidity();
    this.dataInicioContratoFormControl.markAsTouched();

    if (this.possuiInformacaoContribuicaoFormControl.value == 'S') {
      this.dataTerminoContratoFormControl.setValue(null);
      this.dataTerminoContratoFormControl.disable();
    }else{
      if (
        this.dataInicioContratoFormControl.value != null &&
        this.tipoContratoFormControl.value != null &&
        this.tipoContratoFormControl.value === 6
      ) {
        this.dataTerminoContratoFormControl.enable();
      } else {
        this.dataTerminoContratoFormControl.setValue(null);
        this.dataTerminoContratoFormControl.disable();
      }
    }
    if (this.possuiInformacaoContribuicaoFormControl.value == 'S') {
      this.codigoCboFormControl.clearValidators();
      this.codigoCboFormControl.setValue(null);
      this.codigoCboFormControl.disable();
    }else{
      this.codigoCboFormControl.enable();
      if (
        this.possuiInformacaoContribuicaoFormControl.value != null
        && this.possuiInformacaoContribuicaoFormControl.value == 'N'
        && (this.codigoCategoriaFormControl.value != null
          && this.codigoCategoriaFormControl.value != 901
          && this.codigoCategoriaFormControl.value != 903
          && this.codigoCategoriaFormControl.value != 904)
      ) {
        this.codigoCboFormControl.setValidators([Validators.required]);
      } else {
        this.codigoCboFormControl.clearValidators();
      }
    }
    this.codigoCboFormControl.updateValueAndValidity();
    this.codigoCboFormControl.markAsTouched();

    if (this.possuiInformacaoContribuicaoFormControl.value == 'S') {
      this.motivoDesligamentoFormControl.setValue(null)
        this.motivoDesligamentoFormControl.disable();
    }else{
      if (this.dataTerminoContratoFormControl.value != null
        && (this.codigoCategoriaFormControl.value != null
          && this.codigoCategoriaFormControl.value === 721)
      ) {
        this.motivoDesligamentoFormControl.enable();
        this.motivoDesligamentoFormControl.setValidators([Validators.required]);
      } else {
        this.motivoDesligamentoFormControl.setValue(null)
        this.motivoDesligamentoFormControl.disable();
        this.motivoDesligamentoFormControl.clearValidators();
      }
    }
    this.motivoDesligamentoFormControl.updateValueAndValidity();
    this.motivoDesligamentoFormControl.markAsTouched();

    if (this.possuiInformacaoContribuicaoFormControl.value == 'S') {
      this.naturezaAtividadeFormControl.clearValidators();
      this.naturezaAtividadeFormControl.setValue(null);
      this.naturezaAtividadeFormControl.disable();
    }else{
      this.naturezaAtividadeFormControl.enable();

      if (
        this.naturezaAtividadeFormControl.value != null &&
        (this.codigoCategoriaFormControl.value == 721
          || this.codigoCategoriaFormControl.value == 722
          || this.codigoCategoriaFormControl.value == 771
          || this.codigoCategoriaFormControl.value == 901)) {
        this.naturezaAtividadeFormControl.setValidators([this.esocialCustonValidator.preenchimentoNaturezaProibido(this.codigoCategoriaFormControl.value)]);
      } else if (this.naturezaAtividadeFormControl.value == 2 && this.codigoCategoriaFormControl.value == 104) {
        this.naturezaAtividadeFormControl.setValidators([this.esocialCustonValidator.preenchimentoNaturezaProibido(this.codigoCategoriaFormControl.value)]);
      } else if (this.naturezaAtividadeFormControl.value == 1 && this.codigoCategoriaFormControl.value == 102) {
        this.naturezaAtividadeFormControl.setValidators([this.esocialCustonValidator.preenchimentoNaturezaProibido(this.codigoCategoriaFormControl.value)]);
      } else if (
        this.possuiInformacaoContribuicaoFormControl.value != null
        && this.possuiInformacaoContribuicaoFormControl.value == 'N'
        && (this.codigoCategoriaFormControl.value != null
          && this.codigoCategoriaFormControl.value == 401
          || this.codigoCategoriaFormControl.value == 731
          || this.codigoCategoriaFormControl.value == 734
          || this.codigoCategoriaFormControl.value == 738
          || this.listaCategoria.some((categoria) => {
            if (categoria.id == this.codigoCategoriaFormControl.value
              && (categoria.descricao.includes('EMPREGADO')
                || categoria.descricao.includes('AGENTE PÚBLICO')
                || categoria.descricao.includes('AVULSO'))) {
              return true;
            } else {
              return false;
            }
          })) && (this.codigoCategoriaFormControl.value != null
            && this.codigoCategoriaFormControl.value != 721
            && this.codigoCategoriaFormControl.value != 722
            && this.codigoCategoriaFormControl.value != 771
            && this.codigoCategoriaFormControl.value != 901)
      ) {
        this.naturezaAtividadeFormControl.setValidators([Validators.required]);
      } else if (this.possuiInformacaoContribuicaoFormControl.value != null
        && this.possuiInformacaoContribuicaoFormControl.value == 'N'
        && (this.codigoCategoriaFormControl.value != null
          && this.codigoCategoriaFormControl.value == 721
          || this.codigoCategoriaFormControl.value == 722
          && this.codigoCategoriaFormControl.value == 771
          && this.codigoCategoriaFormControl.value == 901)
      ) {
        this.naturezaAtividadeFormControl.setValidators(this.esocialCustonValidator.preenchimentoNaturezaProibido(this.codigoCategoriaFormControl.value));
      } else {
        this.naturezaAtividadeFormControl.clearValidators();
      }
      
    }
    this.naturezaAtividadeFormControl.updateValueAndValidity();
    this.naturezaAtividadeFormControl.markAsTouched();


    if (this.possuiInformacaoContribuicaoFormControl.value != null &&
      this.possuiInformacaoContribuicaoFormControl.value == 'N') {
      this.matriculaFormControl.setValidators([Validators.required]);
    } else {
      this.matriculaFormControl.clearValidators();
    }
    this.matriculaFormControl.updateValueAndValidity();
    this.matriculaFormControl.markAsTouched();

    this.formGroup.markAllAsTouched();
  }

  async gerarMatriculaTerceiro(){

    try {
        var novaMatricula = await this.service.gerarMatricula();
        if (novaMatricula) {
          novaMatricula = novaMatricula.toString().padStart(12,"0");
          this.matriculaFormControl.setValue(novaMatricula);
        }else{
          await this.dialogService.info(
            'Matricula não gerada',
            `Desculpe, não foi possivel gerar a matricula!`
          );
        }

    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      await this.dialogService.err(
        `Desculpe, não foi possivel gerar a matricula!`,
        mensagem
      );
    }
  }

  disableAll(): void {
    this.tipoContratoFormControl.disable();
    this.possuiInformacaoContribuicaoFormControl.disable();
    this.dataAdmissaoOriginalFormControl.disable();
    this.reintegracaoEmpregadoFormControl.disable();
    this.categoriaDiferenteCadastradaFormControl.disable();
    this.naturezaDiferenteCadastradaFormControl.disable();
    this.motivoDesligamentoDiferenteContratoFormControl.disable();
    this.unicidadeContratualFormControl.disable();
    this.matriculaFormControl.disable();
    this.codigoCategoriaFormControl.disable();
    this.dataInicioContratoFormControl.disable();
    this.codigoCboFormControl.disable();
    this.dataTerminoContratoFormControl.disable();
    this.naturezaAtividadeFormControl.disable();
    this.motivoDesligamentoFormControl.disable();
  }

  desabilitaTooltip(control: FormControl):boolean{
    return control.valid || control.untouched || control.disabled
  }

}
