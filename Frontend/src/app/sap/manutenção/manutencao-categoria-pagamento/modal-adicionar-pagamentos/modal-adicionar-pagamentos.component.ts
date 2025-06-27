import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap';
import { ManutencaoCategoriaPagamentoService } from '../services/manutencao-categoria-pagamento.service';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';
import { ModalAdicionarService } from '../services/modal-adicionar.service';
import { textosValidacaoFormulario } from '@shared/utils';
import { distinctUntilChanged } from 'rxjs/operators';
import { ModalAdicionarPagamentoService } from '../services/modal-adicionar-pagamento.service';
import { TipoLancamentoCategoriaPagamento } from '@shared/enums/tipo-lancamento-categoria-pagamento.enum';

@Component({
  selector: 'app-modal-adicionar-pagamentos',
  templateUrl: './modal-adicionar-pagamentos.component.html',
  styleUrls: ['./modal-adicionar-pagamentos.component.scss']
})
export class ModalAdicionarPagamentosComponent implements OnInit {

  constructor(public bsModalRef: BsModalRef,
              private manutencaoService: ManutencaoCategoriaPagamentoService,
              private service: ModalAdicionarPagamentoService,
              public modalAdicionarService: ModalAdicionarService) { }

  registerForm: FormGroup;

  tipoProcessoSelecionado;

  tipoProcessoEnum = TipoProcessoEnum;
  txtForncedorObrigatorio;
  temValorVazio;

  fornecedor;

  titulo;

  garantias;
  grupoCorrecao;
  pagamentoA;
  listCivelEstrategico;
  listCivelConsumidor;

  ngOnInit() {
    this.tipoProcessoSelecionado = this.manutencaoService.tipoProcessoSelecionado;
    this.fornecedor = this.manutencaoService.listaFornecedoresPermitidos;
    this.validation();
    this.validacaoTela();
    this.registerForm.get('codigoMaterial').valueChanges.pipe(distinctUntilChanged()).subscribe(
      item => this.limpar(item)
    );

    this.modalAdicionarService.valoresEdicaoFormulario.pipe(distinctUntilChanged()).subscribe(
      item => {
        if (item) {

          this.titulo = 'Edição';
          this.registerForm.patchValue(item);
          this.verificarForm()
        } else {
          this.titulo = 'Inclusão';
          this.verificarForm();
        }
      }
    );

    this.modalAdicionarService.fecharModal.subscribe(
      item => {
        if (item) {
          this.bsModalRef.hide();
        }
      }
    );

    this.modalAdicionarService.preencherCombos().subscribe(
      item => {
        this.garantias = item.classesGarantias;
        this.grupoCorrecao = item.grupoCorrecao;
        this.pagamentoA = item.pagamentoA;
      }
    );   

    this.modalAdicionarService.preencherCombo().subscribe(
      item => {
        this.listCivelEstrategico = item.categoriaPagamento.sort((a,b) => a.descricao.localeCompare(b.descricao)).filter(r => r.tipoLancamento === TipoLancamentoCategoriaPagamento.pagamentos).map(r => ({ id: r.id, descricao: r.descricao + (r.ativo ? '' : ' [INATIVO]') }));
      }
    );

    this.modalAdicionarService.preencherConsumidor().subscribe(
      item => {
        this.listCivelConsumidor = item.categoriaPagamento.sort((a,b) => a.descricao.localeCompare(b.descricao)).filter(r => r.tipoLancamento === TipoLancamentoCategoriaPagamento.pagamentos).map(r => ({ id: r.id, descricao: r.descricao + (r.ativo ? '' : ' [INATIVO]') }));
      }
    );
    
  }

  

  limpar(item) {
    item = item + '';
    item = item.replace(/^0|^((?!\d).)/g, '')
    this.registerForm.get('codigoMaterial').patchValue(item, { emitEvent: false });
    this.registerForm.get('codigoMaterial').updateValueAndValidity();
  }
  validation() {
    this.registerForm = this.modalAdicionarService.inicializarForm();
  }

  validacaoTela() {
    this.registerForm.get('codigoClasseGarantia').valueChanges.pipe(distinctUntilChanged()).
      subscribe(item =>
        this.verificarForm());

    this.registerForm.get('pagamentoA').valueChanges.pipe(distinctUntilChanged()).
      subscribe(item =>
        this.verificarForm());

    this.registerForm.get('indicaEnvioSAP').valueChanges.pipe(distinctUntilChanged())
      .subscribe(item => this.verificarForm());

    this.service.txtForncedorObrigatorio.subscribe(
      item => this.txtForncedorObrigatorio = item
    );

    this.service.temValorVazio.subscribe(item =>
      this.temValorVazio = item);

    this.registerForm.get('influenciaContingenciaMedia').valueChanges.pipe(distinctUntilChanged())
    .subscribe(item => this.verificarForm());
  }

  validacaoTextos(nomeControl: string, nomeCampo: string, isFeminino: boolean) {
    return textosValidacaoFormulario(this.registerForm, nomeControl, nomeCampo, isFeminino);
  }

  numerosValidacaoFormulario(nomeControl: string) {
    if (this.registerForm.get(nomeControl).value != null &&
      this.registerForm.get(nomeControl).value != '' &&
      this.registerForm.get(nomeControl).touched &&
      isNaN(this.registerForm.get(nomeControl).value.toString().replace(",", ".")) ||
      (this.registerForm.get(nomeControl).value != null && this.registerForm.get(nomeControl).value != ''
      && (this.registerForm.get(nomeControl).value.toString().replace(",", ".") > 99.99
      || this.registerForm.get(nomeControl).value.toString().replace(",", ".") <= 0)))
    {
      this.registerForm.controls['responsabilidadeOi'].setErrors({'incorrect': true});
      return "O % de Responsabilidade Oi deve ser maior que 0 e menor que 100.";
    }
  }

  verificarForm() {
    this.service.verificarForm(this.registerForm, this.tipoProcessoSelecionado);
  }

  adicionarPagamento() {
    this.service.salvar(this.registerForm, this.tipoProcessoSelecionado);
  }

  isNumberKey(evt)
  {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode != 46 && charCode > 31
      && (charCode < 48 || charCode > 57) && charCode != 44)
        return false;

    return true;
  }
}
