import { isNullOrUndefined } from 'util';
import { HttpErrorResult } from '@core/http/http-error-result';
import { EmpresaDoGrupoService } from '@manutencao/services/empresa-do-grupo.service';
import { Fornecedor } from '@shared/models/fornecedor.model';
import { Estados } from 'src/app/core/models/estados.model';
import { Estado } from './../../../../../core/models/estado.model';
import { FormControl, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { NgbActiveModal, NgbTabChangeEvent, NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Component, OnInit, Input } from '@angular/core';
import { Regional } from '@shared/models/regional.model';
import { DialogService } from '@shared/services/dialog.service';
import { ObterService } from '@manutencao/services/obter.service';
import { EmpresaDoGrupo } from '@manutencao/models/empresa-do-grupo.model';
import { EmpresaCentralizadora } from '@manutencao/models';
import { StaticInjector } from '@manutencao/static-injector';

@Component({
  selector: 'app-manter',
  templateUrl: './manter.component.html',
  styleUrls: ['./manter.component.scss']
})
export class ManterComponent implements OnInit {
  @Input() titulo: string;
  @Input() entidade: EmpresaDoGrupo;

  activeId: any = '';
  initialized = false;

  // ABA: DADOS DA EMPRESA
  cnpjFormControl: FormControl = new FormControl('', [Validators.required, this.cnpjValido()]);
  razaoSocialFormControl: FormControl = new FormControl('', [Validators.maxLength(400), Validators.required]);
  enderecoFormControl: FormControl = new FormControl('', [Validators.maxLength(400)]);
  bairroFormControl: FormControl = new FormControl('', [Validators.maxLength(30)]);
  estadoFormControl: FormControl = new FormControl(null);
  cidadeFormControl: FormControl = new FormControl('', [Validators.maxLength(30)]);
  cepFormControl: FormControl = new FormControl('');
  telefoneFormControl: FormControl = new FormControl('');
  faxFormControl: FormControl = new FormControl('');
  regionalFormControl: FormControl = new FormControl(null, [Validators.required]);
  empresaCentralizadoraFormControl: FormControl = new FormControl(null, [Validators.required]);

  estados: Array<Estado> = [];
  regionais: Array<Regional> = [];
  empresasCentralizadoras: Array<{ id: string | number, nome: string }> = [];

  // ABA: SAP
  empresaSapFormControl: FormControl = new FormControl(null, [Validators.required]);
  fornecedorDefaultFormControl: FormControl = new FormControl(null);
  // tslint:disable-next-line: max-line-length
  centroSapFormControl: FormControl = new FormControl(null, [Validators.required, Validators.maxLength(4)]);
  centroDeCustoFormControl: FormControl = new FormControl(null);
  geraArquivoBBFormControl: FormControl = new FormControl(false);
  interfaceBBFormControl: FormControl = new FormControl(null);
  empRecuperandaFormControl: FormControl = new FormControl(false);
  empTrioFormControl: FormControl = new FormControl(false);

  empresasSap: any = [];
  fornecedores: Array<Fornecedor> = [];
  centrosDeCusto: Array<object> = [];
  interfacesBB: Array<object> = [];

  formulario: FormGroup = new FormGroup({
    cnpj: this.cnpjFormControl,
    razaoSocial: this.razaoSocialFormControl,
    endereco: this.enderecoFormControl,
    bairro: this.bairroFormControl,
    estado: this.estadoFormControl,
    cidade: this.cidadeFormControl,
    cep: this.cepFormControl,
    telefone: this.telefoneFormControl,
    fax: this.faxFormControl,
    regional: this.regionalFormControl,
    empresaSap: this.empresaSapFormControl,
    empresaCentralizadora: this.empresaCentralizadoraFormControl,
    fornecedor: this.fornecedorDefaultFormControl,
    centro: this.centroSapFormControl,
    centroDeCusto: this.centroDeCustoFormControl,
    geraArquivoBB: this.geraArquivoBBFormControl,
    interfaceBB: this.interfaceBBFormControl,
    empRecuperanda: this.empRecuperandaFormControl,
    empTrio: this.empTrioFormControl
  });

  // tslint:disable-next-line: max-line-length
  mascaraCnpj = [/[0-9]/, /[0-9]/, '.', /[0-9]/, /[0-9]/, /[0-9]/, '.', /[0-9]/, /[0-9]/, /[0-9]/, '/', /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, '-', /[0-9]/, /[0-9]/];
  mascaraTelefone = ['(', /[1-9]/, /\d/, ')', /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/];
  mascaraFax = ['(', /[1-9]/, /\d/, ')', /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/];
  mascaraCep = [/[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, /[0-9]/, '-', /[0-9]/, /[0-9]/, /[0-9]/];

  constructor(
              public activeModal: NgbActiveModal,
              private service: EmpresaDoGrupoService,
              private obterService: ObterService,
              private dialog: DialogService) { }

  async ngOnInit(): Promise<void> {
    try {
      this.estados = Estados.obterUfs();
      this.regionais = await this.obterService.obterRegionais();
      const empresasCentralizadoras = await this.obterService.obterEmpresasCentralizadoras();
      this.empresasCentralizadoras = empresasCentralizadoras.map(x => ({ id: x.codigo, nome: x.nome }));

      if (this.entidade) {
        this.popularFormulario();
      }

      this.aoMarcarGeraArquivoBB();

    } catch (error) {
      await this.dialog.showErr('Não foi possível carregar as informações', (error as HttpErrorResult).messages.join('\n'));
      this.activeModal.dismiss();
    }
  }

  // tslint:disable-next-line: member-ordering
  private static exibeModal(): NgbModalRef {
    const modalService: NgbModal = StaticInjector.Instance.get(NgbModal);
    const modalRef = modalService.open(ManterComponent, { centered: true, size: 'lg', backdrop: 'static' });
    return modalRef;
  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeCriar(): any {
    const modalRef = this.exibeModal();
    modalRef.componentInstance.titulo = 'Inclusão de Empresa do Grupo';

    return modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeAlterar(entidade: EmpresaDoGrupo): any {
    const modalRef = this.exibeModal();
    modalRef.componentInstance.titulo = 'Edição de Empresa do Grupo';
    modalRef.componentInstance.entidade = entidade;
    return modalRef.result;
  }

  async verificaCarregamentoDeListas($event: NgbTabChangeEvent) {
    if ($event.nextId === 'tab-sap' && !this.initialized) {
      this.carregarListas();
      this.initialized = true;
    }
  }

  async carregarListas() {
    if (!this.initialized) {
      const empresasSap = await this.obterService.obterEmpresasSap();
      this.empresasSap = empresasSap.map(e => ({ id: e.codigo, nome: e.nome }));

      this.fornecedores = await this.obterService.obterFornecedores();

      const centrosDeCusto = await this.obterService.obterCentrosDeCusto();
      this.centrosDeCusto = centrosDeCusto.map(c => ({ id: c.id, nome: c.descricao }));

      const interfacesBB = await this.obterService.obterInterfacesBB();
      this.interfacesBB = interfacesBB.map(i => ({ id: i.codigoDiretorio, nome: i.descricao }));
    }
  }

  async confirmar(): Promise<void> {
    this.formulario.markAllAsTouched();
    if (this.formulario.invalid) {
      if (!(this.cnpjFormControl.invalid || this.razaoSocialFormControl.invalid
        || this.enderecoFormControl.invalid || this.bairroFormControl.invalid
        || this.regionalFormControl.invalid || this.empresaCentralizadoraFormControl.invalid)) {
        this.carregarListas();
        this.activeId = 'tab-sap';
        return;
      }
      return;
    }

    try {

      if (this.entidade) {
        await this.atualizar();
      } else {
        const mesmoCnpjValido = await this.validarCadastroMesmoCnpj();
        if (!mesmoCnpjValido) {
          return;
        }

        await this.criar();
      }

      this.activeModal.close();
    } catch (error) {
      console.log(error);
    }
  }

  private async validarCadastroMesmoCnpj(): Promise<boolean> {

    const empresasEncontradas = await this.service.nomesPorCNPJ(this.cnpjFormControl.value.toString().replace(/[^0-9]+/g, ''));

    if (empresasEncontradas.length === 0) {
      return true;
    }

    // tslint:disable-next-line: max-line-length
    const confirmarHomonimo: boolean = await this.dialog.showConfirm('Confirmação de cadastro', `Foram encontradas Empresas do Grupo com o mesmo CNPJ ${empresasEncontradas.join(', ')}. Deseja continuar?`);

    if (confirmarHomonimo) {
      return true;
    }

    return false;
  }

  private async criar(): Promise<void> {
    try {

      await this.service.criar({
        // PROPRIEDADE EMRPESA DO GRUPO
        cnpj: this.cnpjFormControl.value.toString().replace(/[^0-9]+/g, ''),
        nome: this.razaoSocialFormControl.value,
        endereco: this.enderecoFormControl.value,
        bairro: this.bairroFormControl.value,
        estado: this.estadoFormControl.value,
        cidade: this.cidadeFormControl.value,
        cep: !isNullOrUndefined(this.cepFormControl.value) ? this.cepFormControl.value.toString().replace(/[^0-9]+/g, '') : null,
        // tslint:disable-next-line: max-line-length
        telefoneDDD: !isNullOrUndefined(this.telefoneFormControl.value) ? this.telefoneFormControl.value.toString().replace(/[^0-9]+/g, '').substr(0, 2) : null,
        // tslint:disable-next-line: max-line-length
        telefone: !isNullOrUndefined(this.telefoneFormControl.value) ? this.telefoneFormControl.value.toString().replace(/[^0-9]+/g, '').substr(2, 10) : null,
        // tslint:disable-next-line: max-line-length
        faxDDD: !isNullOrUndefined(this.faxFormControl.value) ? this.faxFormControl.value.toString().replace(/[^0-9]+/g, '').substr(0, 2) : null,
        // tslint:disable-next-line: max-line-length
        fax: !isNullOrUndefined(this.faxFormControl.value) ? this.faxFormControl.value.toString().replace(/[^0-9]+/g, '').substr(2, 10) : null,
        regional: this.regionalFormControl.value,
        empresaCentralizadora: this.empresaCentralizadoraFormControl.value,

        // PROPRIEDADE SAP
        empresaSap: this.empresaSapFormControl.value,
        fornecedor: this.fornecedorDefaultFormControl.value,
        centroSap: this.centroSapFormControl.value,
        centroCusto: this.centroDeCustoFormControl.value,
        geraArquivoBB: this.geraArquivoBBFormControl.value,
        interfaceBB: this.interfaceBBFormControl.value,
        empRecuperanda: this.empRecuperandaFormControl.value,
        empTrio: this.empTrioFormControl.value
      });

      await this.dialog.showAlert('Cadastro realizado com sucesso', 'A Empresa do Grupo foi registrada no sistema.');

    } catch (error) {
      const messages = (error as HttpErrorResult).messages.join('\n');
      const title = messages === 'Já existe uma empresa do grupo cadastrada com o mesmo nome' ?
        'A inclusão não poderá ser realizada' : 'Cadastro não realizado';

      await this.dialog.showErr(title, (error as HttpErrorResult).messages.join('\n'));
      throw error;
    }
  }

  private async atualizar(): Promise<void> {
    try {
      await this.service.atualizar({
        // PROPRIEDADE EMRPESA DO GRUPO
        id: this.entidade.id,
        cnpj: this.cnpjFormControl.value.toString().replace(/[^0-9]+/g, ''),
        nome: this.razaoSocialFormControl.value,
        endereco: this.enderecoFormControl.value,
        bairro: this.bairroFormControl.value,
        estado: this.estadoFormControl.value,
        cidade: this.cidadeFormControl.value,
        cep: !isNullOrUndefined(this.cepFormControl.value) ? this.cepFormControl.value.toString().replace(/[^0-9]+/g, '') : null,
        // tslint:disable-next-line: max-line-length
        telefoneDDD: !isNullOrUndefined(this.telefoneFormControl.value) ? this.telefoneFormControl.value.toString().replace(/[^0-9]+/g, '').substr(0, 2) : null,
        // tslint:disable-next-line: max-line-length
        telefone: !isNullOrUndefined(this.telefoneFormControl.value) ? this.telefoneFormControl.value.toString().replace(/[^0-9]+/g, '').substr(2, 10) : null,
        // tslint:disable-next-line: max-line-length
        faxDDD: !isNullOrUndefined(this.faxFormControl.value) ? this.faxFormControl.value.toString().replace(/[^0-9]+/g, '').substr(0, 2) : null,
        // tslint:disable-next-line: max-line-length
        fax: !isNullOrUndefined(this.faxFormControl.value) ? this.faxFormControl.value.toString().replace(/[^0-9]+/g, '').substr(2, 10) : null,
        regional: this.regionalFormControl.value,
        empresaCentralizadora: this.empresaCentralizadoraFormControl.value,

        // PROPRIEDADE SAP
        empresaSap: this.empresaSapFormControl.value,
        fornecedor: this.fornecedorDefaultFormControl.value,
        centroSap: this.centroSapFormControl.value,
        centroCusto: this.centroDeCustoFormControl.value,
        geraArquivoBB: this.geraArquivoBBFormControl.value,
        interfaceBB: this.interfaceBBFormControl.value,
        empRecuperanda: this.empRecuperandaFormControl.value,
        empTrio: this.empTrioFormControl.value
      });
      await this.dialog.showAlert('Alteração realizada com sucesso', 'A Empresa do Grupo foi alterada no sistema.');
    } catch (error) {
      await this.dialog.showErr('Alteração não realizada', (error as HttpErrorResult).messages.join('\n'));
      throw error;
    }
  }

  private popularFormulario(): void {
    this.cnpjFormControl.setValue(this.entidade.cnpj);
    this.razaoSocialFormControl.setValue(this.entidade.nome);
    this.enderecoFormControl.setValue(this.entidade.endereco);
    this.bairroFormControl.setValue(this.entidade.bairro);
    this.estadoFormControl.setValue(isNullOrUndefined(this.entidade.estado) ? '' : this.entidade.estado.id);
    this.cidadeFormControl.setValue(this.entidade.cidade);
    this.cepFormControl.setValue(this.entidade.cep);
    this.telefoneFormControl.setValue(this.entidade.telefoneDDD + this.entidade.telefone);
    this.faxFormControl.setValue(this.entidade.faxDDD + this.entidade.fax);
    this.regionalFormControl.setValue(!isNullOrUndefined(this.entidade.regional) ? this.entidade.regional.id : '');
    this.empresaCentralizadoraFormControl.setValue(!isNullOrUndefined(this.entidade.empresaCentralizadora) ? this.entidade.empresaCentralizadora.codigo : '');
    this.empresaSapFormControl.setValue(isNullOrUndefined(this.entidade.empresaSap) ? '' : this.entidade.empresaSap.codigo);
    this.fornecedorDefaultFormControl.setValue(isNullOrUndefined(this.entidade.fornecedor) ? '' : this.entidade.fornecedor.id);
    this.centroSapFormControl.setValue(this.entidade.codCentroSap);
    this.centroDeCustoFormControl.setValue(isNullOrUndefined(this.entidade.centroCusto) ? '' : this.entidade.centroCusto.id);
    this.geraArquivoBBFormControl.setValue(this.entidade.geraArquivoBB);
    this.interfaceBBFormControl.setValue(this.entidade.diretorioBB);
    this.empRecuperandaFormControl.setValue(isNullOrUndefined(this.entidade.empRecuperanda) || !this.entidade.empRecuperanda ? false : true );
    this.empTrioFormControl.setValue(isNullOrUndefined(this.entidade.empTrio) || !this.entidade.empTrio ? false : true );
  }

  desabilitaTooltip(formControl: FormControl) {
    return formControl.untouched || formControl.valid;
  }

  cnpjValido() {
    return (control: AbstractControl): Validators => {
      const re = /\./gi;
      let cnpj = control.value;
      if (cnpj) {
        cnpj = cnpj.replace(re, '');
        cnpj = cnpj.replace('-', '');
        cnpj = cnpj.replace('/', '');
        if (cnpj.length !== 14) {
          return null;
        }

        if (cnpj === '11111111111111' || cnpj === '99999999999999') {
          return null;
        }

        // Elimina CNPJs invalidos conhecidos
        if (cnpj === '00000000000000' ||
          cnpj === '22222222222222' ||
          cnpj === '33333333333333' ||
          cnpj === '44444444444444' ||
          cnpj === '55555555555555' ||
          cnpj === '66666666666666' ||
          cnpj === '77777777777777' ||
          cnpj === '88888888888888') {
          return { cnpjNotValid: true };
        }

        // Valida DVs
        let tamanho;
        let numeros;
        let digitos;
        let soma;
        let pos;
        let i;
        let resultado;

        tamanho = cnpj.length - 2;
        numeros = cnpj.substring(0, tamanho);
        digitos = cnpj.substring(tamanho);
        soma = 0;
        pos = tamanho - 7;

        for (i = tamanho; i >= 1; i--) {
          soma += numeros.charAt(tamanho - i) * pos--;
          if (pos < 2) {
            pos = 9;
          }
        }
        resultado = soma % 11 < 2 ? 0 : 11 - (soma % 11);
        // tslint:disable-next-line: triple-equals
        if (resultado != digitos.charAt(0)) {
          return { cnpjNotValid: true };
        }

        tamanho = tamanho + 1;
        numeros = cnpj.substring(0, tamanho);
        soma = 0;
        pos = tamanho - 7;
        for (i = tamanho; i >= 1; i--) {
          soma += numeros.charAt(tamanho - i) * pos--;
          if (pos < 2) {
            pos = 9;
          }
        }
        resultado = soma % 11 < 2 ? 0 : 11 - (soma % 11);
        // tslint:disable-next-line: triple-equals
        if (resultado != digitos.charAt(1)) {
          return { cnpjNotValid: true };
        }
        return null;
      }
      return null;
    };
  }

  aoMarcarGeraArquivoBB() {
    const geraArquivoBB = this.geraArquivoBBFormControl.value === true;
    if (geraArquivoBB) {
      this.interfaceBBFormControl.setValidators([Validators.required]);
    } else {
      this.interfaceBBFormControl.setValidators([]);
    }
    this.interfaceBBFormControl.updateValueAndValidity();
  }
}
