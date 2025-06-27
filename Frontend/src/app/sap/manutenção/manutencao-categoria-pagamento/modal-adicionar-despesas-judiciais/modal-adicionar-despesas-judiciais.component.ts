import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap';
import { ManutencaoCategoriaPagamentoService } from '../services/manutencao-categoria-pagamento.service';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';
import {  ModalAdicionarDespesaService } from '../services/modal-adicionar-despesa.service';
import { textosValidacaoFormulario } from '@shared/utils';
import { ModalAdicionarService } from '../services/modal-adicionar.service';
import { distinctUntilChanged } from 'rxjs/operators';
import { CategoriaPagamentoService } from 'src/app/core/services/sap/categoria-pagamento.service';
import { Observable } from 'rxjs';
import { TipoLancamentoCategoriaPagamento } from '@shared/enums/tipo-lancamento-categoria-pagamento.enum';
@Component({
  selector: 'app-modal-adicionar-despesas-judiciais',
  templateUrl: './modal-adicionar-despesas-judiciais.component.html',
  styleUrls: ['./modal-adicionar-despesas-judiciais.component.scss']
})
export class ModalAdicionarDespesasJudiciaisComponent implements OnInit {

  constructor(public bsModalRef: BsModalRef,
              private manutencaoService: ManutencaoCategoriaPagamentoService,
              private service: ModalAdicionarDespesaService,
    public modalAdicionarService: ModalAdicionarService,
    private categoriaPagamento: CategoriaPagamentoService
  ) { }

  registerForm: FormGroup;
  tipoProcessoSelecionado;
  civelEstrategico: any;
  tipoProcessoEnum = TipoProcessoEnum;

  txtForncedorObrigatorio = '';
  temValorVazio = true;
  titulo = 'Inclusão';
  listCivelEstrategico;
  listCivelConsumidor;

  fornecedor;


  ngOnInit() {
    this.tipoProcessoSelecionado = this.manutencaoService.tipoProcessoSelecionado;
    this.validation();

    this.modalAdicionarService.preencherCombo().subscribe(
      item => {
        this.listCivelEstrategico = item.categoriaPagamento.sort((a,b) => a.descricao.localeCompare(b.descricao)).filter(r => r.tipoLancamento === TipoLancamentoCategoriaPagamento.despesasJudiciais).map(r => ({ id: r.id, descricao: r.descricao + (r.ativo ? '' : ' [INATIVO]') }));
      }
    );

    this.modalAdicionarService.preencherConsumidor().subscribe(
      item => {
        this.listCivelConsumidor = item.categoriaPagamento.sort((a,b) => a.descricao.localeCompare(b.descricao)).filter(r => r.tipoLancamento === TipoLancamentoCategoriaPagamento.despesasJudiciais).map(r => ({ id: r.id, descricao: r.descricao + (r.ativo ? '' : ' [INATIVO]') }));
      }
    );
    

    this.modalAdicionarService.valoresEdicaoFormulario.pipe(distinctUntilChanged()).subscribe(
      item => {
        if (item) {
          this.titulo = 'Edição';
          this.registerForm.patchValue(item);
          this.service.verificarForm(this.registerForm, this.tipoProcessoSelecionado, item.indicaEnvioSAP);
        } else {
          this.service.verificarForm(this.registerForm, this.tipoProcessoSelecionado, false);
          this.titulo = 'Inclusão';
          this.civelEstrategico = this.registerForm.get('comboCivelEstrategico').value;
        }
      }
    );

    this.validacoesTela();
    this.fornecedor = this.manutencaoService.listaFornecedoresPermitidos;


    this.modalAdicionarService.fecharModal.subscribe(
      item => {
        if (item) {
          this.bsModalRef.hide();
        }
      }
    );
  }




  validacoesTela() {
    this.registerForm.get('indicaEnvioSAP').valueChanges.subscribe(
      item => { this.service.verificarForm(this.registerForm, this.tipoProcessoSelecionado, item); });

    this.service.txtForncedorObrigatorio.subscribe(
      item => this.txtForncedorObrigatorio = item
    );

    this.service.temValorVazio.subscribe(item =>
      this.temValorVazio = item);

    this.registerForm.get('codigoMaterial').valueChanges.pipe(distinctUntilChanged()).subscribe(
      item => this.limpar(item)
    );
  }

  limpar(item) {
    item = item + '';
    item = item.replace(/^0|^((?!\d).)/g, '');
    this.registerForm.get('codigoMaterial').patchValue(item, { emitEvent: false });
    this.registerForm.get('codigoMaterial').updateValueAndValidity();
  }


  validation() {
    this.registerForm = this.modalAdicionarService.inicializarForm();

  }

  validacaoTextos(nomeControl: string, nomeCampo: string, isFeminino: boolean) {
   return textosValidacaoFormulario(this.registerForm, nomeControl, nomeCampo, isFeminino);
  }



  salvarAlteracoes() {
    this.modalAdicionarService.salvarAlteracao(this.registerForm);
  }

}
