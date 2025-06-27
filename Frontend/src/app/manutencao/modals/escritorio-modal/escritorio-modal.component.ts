import { AfterViewInit, Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

// 3rd party
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

// core & shared
import { StaticInjector } from '@manutencao/static-injector';
import { DialogService } from '@shared/services/dialog.service';

// local
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';
import { HttpErrorResult } from '@core/http';
import { EscritorioService } from '@manutencao/services/escritorio.service';
import { Escritorio } from '@manutencao/models/escritorio.model';
import { Estados } from '@core/models';
import { EstadosSelecaoComponent } from 'src/app/componentes/estados-selecao/estados-selecao.component';
import { Estado } from '@manutencao/models';
import { TipoProcesso } from '@core/models/tipo-processo';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';
import { element } from 'protractor';
import { setTime } from 'ngx-bootstrap/chronos/utils/date-setters';

@Component({
  selector: 'app-escritorio',
  templateUrl: './escritorio-modal.component.html',
  styleUrls: ['./escritorio-modal.component.scss']
})
export class EscritorioModalComponent implements AfterViewInit {
  escritorio: Escritorio = null;
  parametrizado: boolean = false;
  naturezas: Array<number>;
  exibirAdvogados: boolean = false;
  ativo: boolean = true;
  abaSelecionada: string = 'escritorio';
  mask: string = "00.000.0000-00";
  desmarcouIndicador: boolean = false;

  constructor(
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private service: EscritorioService
  ) { }

  estados: Array<Estados> = Estados.obterUfs();
  estadosSelecionadosJec: Array<{ id: string, selecionado: boolean }> = [];
  estadosSelecionadosCivelConsumidor: Array<{ id: string, selecionado: boolean }> = [];

  nomeFormControl: FormControl = new FormControl('', [
    Validators.required,
    Validators.maxLength(50)
  ]);

  CPFFormControl : FormControl = new FormControl('',[]);
  cidadeFormControl : FormControl = new FormControl('',[Validators.required]);
  estadoFormControl : FormControl = new FormControl('',[Validators.required]);
  cepFormControl : FormControl = new FormControl('',[]);
  emailFormControl  : FormControl = new FormControl('',[]);
  siteFormControl  : FormControl = new FormControl('',[]);
  areaCivelConsumidorFormControl  : FormControl = new FormControl(false,[]);
  areaCivelAdministrativoFormControl  : FormControl = new FormControl(false,[]);
  areaCivelEstrategicoFormControl :  FormControl = new FormControl(false,[]);
  areaCriminalAdministrativoFormControl :  FormControl = new FormControl(false,[]);
  areaCriminalJudicialFormControl : FormControl = new FormControl(false,[]);
  areaJuizadoEspecialFormControl: FormControl = new FormControl(false,[]);
  areaPexFormControl: FormControl = new FormControl(false,[]);
  areaProconFormControl: FormControl = new FormControl(false,[]);
  areaTrabalhistaFormControl: FormControl = new FormControl(false,[]);
  areaTributariaFormControl: FormControl = new FormControl(false,[]);
  alertaemFormControl : FormControl = new FormControl('',[]);
  codProfissionalSAPFormControl : FormControl = new FormControl('',[]);
  ativoFormControl: FormControl = new FormControl(true,[]);
  tipoPessoaFormControl : FormControl = new FormControl('F',[Validators.required]);
  areaRegulatoriaFormControl : FormControl = new FormControl(false,[]);
  enderecoFormControl :  FormControl = new FormControl('',[Validators.required]);
  bairroFormControl :  FormControl = new FormControl('',[]);
  ehContadorPexFormControl :  FormControl = new FormControl(false,[]);
  CNPJFormControl : FormControl = new FormControl('',[]);
  telefoneDDDFormControl : FormControl = new FormControl('',[Validators.required, Validators.maxLength(3),Validators.minLength(2)]);
  telefoneFormControl : FormControl = new FormControl('',[Validators.required, Validators.maxLength(9),Validators.minLength(8)]);
  FaxDDDFormControl : FormControl = new FormControl('',[]);
  FaxFormControl : FormControl = new FormControl('',[]);
  CelularDDDFormControl : FormControl = new FormControl('',[]);
  CelularFormControl : FormControl = new FormControl('',[]);
  enviarAppPrepostoFormControl : FormControl = new FormControl(true,[]);


  formGroup: FormGroup = new FormGroup({
    Nome: this.nomeFormControl,
    CPF: this.CPFFormControl,
    cidade: this.cidadeFormControl,
    estadoId: this.estadoFormControl,
    cep: this.cepFormControl,
    endereco: this.enderecoFormControl,
    email: this.emailFormControl,
    site: this.siteFormControl,
    Ativo: this.ativoFormControl,
    bairro: this.bairroFormControl,
    indareaCivel: this.areaCivelConsumidorFormControl,
    indareaCivelAdministrativo: this.areaCivelAdministrativoFormControl,
    Civelestrategico: this.areaCivelEstrategicoFormControl,
    indareaCriminalAdministrativo: this.areaCriminalAdministrativoFormControl,
    indareaCriminalJudicial: this.areaCriminalJudicialFormControl,
    indareaJuizado: this.areaJuizadoEspecialFormControl,
    indareaRegulatoria: this.areaRegulatoriaFormControl,
    indareaPEX: this.areaPexFormControl,
    indareaProcon: this.areaProconFormControl,
    indareaTrabalhista: this.areaTrabalhistaFormControl,
    indareaTributaria: this.areaTributariaFormControl,
    alertaem: this.alertaemFormControl,
    codProfissionalSAP: this.codProfissionalSAPFormControl,
    tipoPessoaValor : this.tipoPessoaFormControl,
    ehContadorPex : this.ehContadorPexFormControl,
    CNPJ : this.CNPJFormControl,
    telefoneDDD : this.telefoneDDDFormControl,
    telefone : this.telefoneFormControl,
    FaxDDD : this.FaxDDDFormControl,
    Fax : this.FaxFormControl,
    celularDDD : this.CelularDDDFormControl,
    celular : this.CelularFormControl,
    enviarAppPreposto : this.enviarAppPrepostoFormControl
  });

  async carregarEstadoAtuacao() {
    this.estadosSelecionadosJec = await this.service.obterEstadoAtuacao(this.escritorio ? this.escritorio.id : 0, 7);
    this.estadosSelecionadosCivelConsumidor = await this.service.obterEstadoAtuacao(this.escritorio ? this.escritorio.id : 0, 1);
  }

  ngAfterViewInit(): void {

    if (this.escritorio) {

      

      this.exibirAdvogados = true;
      this.carregarEstadoAtuacao();
      this.nomeFormControl.setValue(this.escritorio.nome);
      this.bairroFormControl.setValue(this.escritorio.bairro);
      this.CPFFormControl.setValue(this.escritorio.CPF);
      this.CNPJFormControl.setValue(this.escritorio.CNPJ);
      this.cidadeFormControl.setValue(this.escritorio.cidade);
      this.estadoFormControl.setValue(this.escritorio.estadoId);
      this.cepFormControl.setValue(this.escritorio.cep);
      this.emailFormControl.setValue(this.escritorio.email);
      this.siteFormControl.setValue(this.escritorio.site);
      this.areaCivelConsumidorFormControl.setValue(this.escritorio.indAreaCivel);
      this.areaCivelAdministrativoFormControl.setValue(this.escritorio.indAreaCivelAdministrativo);
      this.areaCivelEstrategicoFormControl.setValue(this.escritorio.civelEstrategico);
      this.areaCriminalAdministrativoFormControl.setValue(this.escritorio.indAreaCriminalAdministrativo);
      this.areaCriminalJudicialFormControl.setValue(this.escritorio.indAreaCriminalJudicial);
      this.areaJuizadoEspecialFormControl.setValue(this.escritorio.indAreaJuizado);
      this.areaPexFormControl.setValue(this.escritorio.indAreaPEX);
      this.areaProconFormControl.setValue(this.escritorio.indAreaProcon);
      this.areaTrabalhistaFormControl.setValue(this.escritorio.indAreaTrabalhista);
      this.areaTributariaFormControl.setValue(this.escritorio.indAreaTributaria);
      this.alertaemFormControl.setValue(this.escritorio.alertaEm);
      this.codProfissionalSAPFormControl.setValue(this.escritorio.codProfissionalSAP);
      this.ativoFormControl.setValue(this.escritorio.ativo);
      this.tipoPessoaFormControl.setValue(this.escritorio.tipoPessoaId);
      this.areaRegulatoriaFormControl.setValue(this.escritorio.indAreaRegulatoria);
      this.enderecoFormControl.setValue(this.escritorio.endereco);
      this.telefoneDDDFormControl.setValue(this.escritorio.TelefoneDDD.trim());
      this.telefoneFormControl.setValue(this.escritorio.Telefone.trim());
      this.FaxDDDFormControl.setValue(this.escritorio.FaxDDD);
      this.FaxFormControl.setValue(this.escritorio.Fax);
      this.CelularDDDFormControl.setValue(this.escritorio.celularDDD);
      this.CelularFormControl.setValue(this.escritorio.celular);
      this.ativo = this.escritorio.ativo;
      this.changepessoa(this.escritorio.tipoPessoaId);
      this.enviarAppPrepostoFormControl.setValue(this.escritorio.enviarAppPreposto)

      this.formGroup.markAllAsTouched();
    }
    else {
      this.changepessoa("F");
    }

    this.changeProcesso();
  }

  close(): void {
    this.modal.close(false);
  }

  temAreadeAtuacao(): boolean {
    return (this.areaCivelAdministrativoFormControl.value || this.areaCivelConsumidorFormControl.value || this.areaCivelEstrategicoFormControl.value ||
      this.areaCriminalAdministrativoFormControl.value || this.areaCriminalJudicialFormControl.value || this.areaJuizadoEspecialFormControl.value ||
      this.areaPexFormControl.value || this.areaProconFormControl.value || this.areaRegulatoriaFormControl.value || this.areaTrabalhistaFormControl.value ||
      this.areaTributariaFormControl.value);
  }

  async save(): Promise<void> {
    const operacao = this.escritorio ? 'Alteração' : 'Inclusão';

    if (operacao === 'Alteração') {
      if ((!this.areaCivelConsumidorFormControl.value) && this.escritorio.indAreaCivel) {
        if (this.estadosSelecionadosCivelConsumidor.find(x => x.selecionado)) {
          const removerEstados = await this.dialogService.confirm(
            `Existem estados associados ao Escritório.`,
            `Deseja realmente desmarcar a área de atuação?`
          );

          if (removerEstados) {
            this.estadosSelecionadosCivelConsumidor.forEach((element) => { if (element.selecionado) { element.selecionado = false } });
          }
          else {
            this.areaCivelConsumidorFormControl.setValue(true);
            return
          }
        }
      }

      if ((!this.areaJuizadoEspecialFormControl.value) && this.escritorio.indAreaJuizado) {
        if (this.estadosSelecionadosJec.find(x => x.selecionado)) {
          const removerEstados = await this.dialogService.confirm(
            `Existem estados associados ao Escritório.`,
            `Deseja realmente desmarcar a área de atuação?`
          );

          if (removerEstados) {
            this.estadosSelecionadosJec.forEach((element) => { if (element.selecionado) { element.selecionado = false } });
          }
          else {
            this.areaJuizadoEspecialFormControl.setValue(true);
            return;
          }
        }
      }
    }

    if (!this.emailValido(this.emailFormControl.value)) {
      await this.dialogService.err(
        `${operacao} não realizada`,
        `Email informado está inválido.`
      );
      return;
    }

    if (!this.temAreadeAtuacao()) {
      await this.dialogService.err(
        `${operacao} não realizada`,
        `É preciso selecionar ao menos uma Área de Atuação.`
      );
      return;
    }

    if (!this.nomeFormControl.value.replace(/\s/g, '').length) {
      await this.dialogService.err(
        `${operacao} não realizada`,
        `O campo nome não pode conter apenas espaços.`
      );
      return;
    }

    if ((this.tipoPessoaFormControl.value == "F")) {
      if (!this.ValidarCPF(this.CPFFormControl.value)) {
        await this.dialogService.err(
          `${operacao} não realizada`,
          `CPF Informado está Inválido.`
        );
        return;
      }

    }
    else {
      if (!this.validarCNPJ(this.CNPJFormControl.value)) {
        await this.dialogService.err(
          `${operacao} não realizada`,
          `CNPJ Informado está Inválido.`
        );
        return;
      }
    }

    try {
      let obj: any = this.formGroup.value;

      if (this.areaJuizadoEspecialFormControl.value && (!this.estadosSelecionadosJec.find(x => x.selecionado))) {
        this.estadosSelecionadosJec = Estados.obterUfs().map((estado) => {
          return (
            new Estado(estado.id, estado.nome)
          )
        });

        this.estadosSelecionadosJec.forEach(estado => estado.selecionado = true);
      }

      if (this.areaCivelConsumidorFormControl.value && (!this.estadosSelecionadosCivelConsumidor.find(x => x.selecionado))) {
        this.estadosSelecionadosCivelConsumidor = Estados.obterUfs().map((estado) => {
          return (
            new Estado(estado.id, estado.nome)
          )
        });
        this.estadosSelecionadosCivelConsumidor.forEach(estado => estado.selecionado = true);
      }

      obj.selecionadosJec = this.estadosSelecionadosJec;
      obj.selecionadosCivelConsumidor = this.estadosSelecionadosCivelConsumidor;
      obj.enviarAppPreposto = this.enviarAppPrepostoFormControl.value;

      let retorno;
      if (this.escritorio) {

        obj.id = this.escritorio.id;
        await this.service.alterar(obj);
      }
      else {
        retorno = await this.service.incluir(obj);
      }


      if ((retorno) && (retorno.id > 0)) {
        await this.dialogService.alert(`${operacao} realizada com sucesso`);
        if (await this.dialogService.confirm(`Deseja incluir advogados do escritório?`)) {

          this.escritorio = retorno;
          this.exibirAdvogados = true;
          this.abaSelecionada = 'advogado';
          return;
        }
        else {
          this.modal.close(true);
        }

      }

      await this.dialogService.alert(`${operacao} realizada com sucesso`);
      this.modal.close(true);

    } catch (error) {
      await this.dialogService.err(`${operacao} não realizada`, (error as HttpErrorResult).messages.join('\n'));
    };
  }
  // tslint:disable-next-line: member-ordering
  public static exibeModalDeIncluir(): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(EscritorioModalComponent, { windowClass: 'escritorio-modal', centered: true, size: 'lg', backdrop: 'static' });
    return modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeAlterar(escritorio: Escritorio): Promise<boolean> {
    
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(EscritorioModalComponent, { windowClass: 'escritorio-modal', centered: true, size: 'lg', backdrop: 'static' });
    modalRef.componentInstance.escritorio = escritorio;
    console.log(escritorio);
    modalRef.componentInstance.verificaEscritorioParametrizado(escritorio.id);
    return modalRef.result;
  }

  async verificaEscritorioParametrizado(id: number): Promise<any> {
    this.service.validaEscritorioParametrizado(id).then(x => {
      this.parametrizado = x.parametrizado;
      this.naturezas = x.naturezas;
    });
  }

  async changeStatus(status: boolean) {
    if (this.parametrizado) {
      const checkboxAtivo = document.getElementById("ativo") as HTMLInputElement;
      checkboxAtivo.checked = true;
      await this.dialogService.info('A inativação não poderá ser realizada', 'O escritório não poderá ser inativado, pois está parametrizado em uma chave de distribuição automática de processos. <br> <br> Menu > Workflow > Cadastro > Parametrizar Distribuição de Processos aos Escritórios');
      this.ativoFormControl.setValue(true);
      this.ativo = true;
      return;
    } else {
      this.ativoFormControl.setValue(status);
      this.ativo = status;
    }
  }

  changepessoa(pessoa: string) {

    this.tipoPessoaFormControl.setValue(pessoa);
    this.CPFFormControl.clearValidators();
    this.CNPJFormControl.clearValidators();

    if (pessoa == "F") {
      this.CPFFormControl.setValidators([Validators.maxLength(11), Validators.minLength(10), Validators.required]);
    }
    else {
      this.CNPJFormControl.setValidators([Validators.maxLength(14), Validators.minLength(13), Validators.required]);
    }
  }

  async changeAreaAtuacao(id: string, codigo: number) {
    const checkboxAtivo = document.getElementById(id) as HTMLInputElement;
    let exibirMsg = false;

    if (this.escritorio) {
      if (this.naturezas.find(x => x == codigo && x == 1) && this.escritorio.indAreaCivel) {
        checkboxAtivo.checked = true;
        this.areaCivelConsumidorFormControl.setValue(true);
        exibirMsg = true;
      }
      
      if (this.naturezas.find(x => x == codigo && x == 7) && this.escritorio.indAreaJuizado) {
        checkboxAtivo.checked = true;
        this.areaJuizadoEspecialFormControl.setValue(true);
        exibirMsg = true;
      }

      if (this.naturezas.find(x => x == codigo && x == 17) && this.escritorio.indAreaProcon) {
        checkboxAtivo.checked = true;
        this.areaProconFormControl.setValue(true);
        exibirMsg = true;
      }
    }

    if (exibirMsg) {
      return await this.dialogService.info('A área de atuação não poderá ser alterada', 'A área de atuação não poderá ser alterada, pois o escritório  está parametrizado em uma chave de distribuição automática de processos. <br> <br> Menu > Workflow > Cadastro > Parametrizar Distribuição de Processos aos Escritórios');
    }
  }


  async ExibirEstadosSelecionado(tipoProcessoId: number): Promise<void> {
    if (tipoProcessoId == 7) {
      this.estadosSelecionadosJec = await EstadosSelecaoComponent.exibir(7, this.estadosSelecionadosJec);
    }
    else {
      this.estadosSelecionadosCivelConsumidor = await EstadosSelecaoComponent.exibir(1, this.estadosSelecionadosCivelConsumidor);
    }

  }

  ValidarCPF(strCPF) {
    var Soma;
    var Resto;
    var i = 0;
    Soma = 0;

    if (strCPF == "00000000000") {
      return false;
    }


    for (i = 1; i <= 9; i++) {
      Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (11 - i);
    }

    Resto = (Soma * 10) % 11;

    if ((Resto == 10) || (Resto == 11)) {
      Resto = 0;
    }


    if (Resto != parseInt(strCPF.substring(9, 10))) {
      return false;
    }

    Soma = 0;

    for (i = 1; i <= 10; i++) {
      Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (12 - i);
    }

    Resto = (Soma * 10) % 11;

    if ((Resto == 10) || (Resto == 11))
      Resto = 0;

    if (Resto != parseInt(strCPF.substring(10, 11))) {
      return false;
    }

    return true;
  }


  validarCNPJ(cnpj) {
    var tamanho;
    var numeros;
    var digitos;
    var soma;
    var pos;
    var resultado;
    var i;

    cnpj = cnpj.replace(/[^\d]+/g, '');

    if (cnpj == '') return false;

    if (cnpj.length != 14)
      return false;

    // Elimina CNPJs invalidos conhecidos
    if (cnpj == "00000000000000" ||
      cnpj == "11111111111111" ||
      cnpj == "22222222222222" ||
      cnpj == "33333333333333" ||
      cnpj == "44444444444444" ||
      cnpj == "55555555555555" ||
      cnpj == "66666666666666" ||
      cnpj == "77777777777777" ||
      cnpj == "88888888888888" ||
      cnpj == "99999999999999")
      return false;

    // Valida DVs
    tamanho = cnpj.length - 2
    numeros = cnpj.substring(0, tamanho);
    digitos = cnpj.substring(tamanho);
    soma = 0;
    pos = tamanho - 7;

    for (i = tamanho; i >= 1; i--) {
      soma += numeros.charAt(tamanho - i) * pos--;
      if (pos < 2)
        pos = 9;
    }

    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;

    if (resultado != digitos.charAt(0))
      return false;

    tamanho = tamanho + 1;
    numeros = cnpj.substring(0, tamanho);
    soma = 0;
    pos = tamanho - 7;

    for (i = tamanho; i >= 1; i--) {
      soma += numeros.charAt(tamanho - i) * pos--;
      if (pos < 2)
        pos = 9;
    }

    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;

    if (resultado != digitos.charAt(1))
      return false;

    return true;
  }


  emailValido(email) {
    var reg = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/
    if (reg.test(email)) {
      return true;
    }
    else {
      return false;
    }
  }

  mascara() {
    if (this.tipoPessoaFormControl.value == "J") {
      this.mask = '00.000.000/0000-00'
    }
    else {
      this.mask = '00.000.0000-00'
    }
  }

changeProcesso(){
  const habilita =  this.areaCivelConsumidorFormControl.value || this.areaProconFormControl.value || this.areaJuizadoEspecialFormControl.value;

  habilita ? this.enviarAppPrepostoFormControl.enable() :  this.enviarAppPrepostoFormControl.disable();

  if (habilita && (!this.escritorio) && !this.desmarcouIndicador ){
    this.enviarAppPrepostoFormControl.setValue(true);
  }

  if (!habilita){
    this.enviarAppPrepostoFormControl.setValue(false)
  }


}

changeAppPreposto(){
  
  if (!this.desmarcouIndicador){
    this.desmarcouIndicador = true;
  }

}

}
