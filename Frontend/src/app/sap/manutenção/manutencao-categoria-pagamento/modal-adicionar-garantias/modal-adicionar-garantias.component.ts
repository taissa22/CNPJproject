import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap';
import { TipoProcessoEnum } from './../../../../shared/enums/tipo-processoEnum.enum';
import { ManutencaoCategoriaPagamentoService } from '../services/manutencao-categoria-pagamento.service';
import { ModalAdicionarService } from '../services/modal-adicionar.service';
import { ModalAdicionarGarantiasService } from '../services/modal-adicionar-garantias.service';
import { take, distinctUntilChanged } from 'rxjs/operators';
import { textosValidacaoFormulario } from '@shared/utils';
import { TipoLancamentoCategoriaPagamento } from '@shared/enums/tipo-lancamento-categoria-pagamento.enum';


@Component({
  selector: 'app-modal-adicionar-garantias',
  templateUrl: './modal-adicionar-garantias.component.html',
  styleUrls: ['./modal-adicionar-garantias.component.scss']
})
export class ModalAdicionarGarantiasComponent implements OnInit {

  constructor(public bsModalRef: BsModalRef,
    private manutencaoService: ManutencaoCategoriaPagamentoService,
    private service: ModalAdicionarGarantiasService,
    public modalAdicionarService: ModalAdicionarService) { }

  registerForm: FormGroup;

  tipoProcessoSelecionado;

  tipoProcessoEnum = TipoProcessoEnum;

  garantias;
  grupoCorrecao;
  listCivelEstrategico;
  listCivelConsumidor;

  fornecedor;
  titulo;
  ngOnInit() {


    this.modalAdicionarService.preencherCombos().subscribe(
      item => {
        this.garantias = item.classesGarantias;
        this.grupoCorrecao = item.grupoCorrecao;
      }
    );

    
    this.modalAdicionarService.preencherCombo().subscribe(
      item => {
        
        this.listCivelEstrategico = item.categoriaPagamento.sort((a,b) => a.descricao.localeCompare(b.descricao)).filter(r => r.tipoLancamento === TipoLancamentoCategoriaPagamento.garantias).map(r => ({ id: r.id, descricao: r.descricao + (r.ativo ? '' : ' [INATIVO]') }));
      }
    );

    this.modalAdicionarService.preencherConsumidor().subscribe(
      item => {
        
        this.listCivelConsumidor = item.categoriaPagamento.sort((a,b) => a.descricao.localeCompare(b.descricao)).filter(r => r.tipoLancamento === TipoLancamentoCategoriaPagamento.garantias).map(r => ({ id: r.id, descricao: r.descricao + (r.ativo ? '' : ' [INATIVO]') }));
      }
    );

    this.modalAdicionarService.fecharModal.subscribe(
      item => {
        if (item) {
          this.bsModalRef.hide();
        }
      }
    );

    this.tipoProcessoSelecionado = this.manutencaoService.tipoProcessoSelecionado;

    this.fornecedor = this.manutencaoService.listaFornecedoresPermitidos;

    this.inicializacaoForm();


    this.service.setDisable(this.registerForm);
    this.service.setObrigatoriedadeClasseGarantia(this.registerForm);


    this.registerForm.get('codigoClasseGarantia').valueChanges.pipe(distinctUntilChanged()).
      subscribe(item => {
        this.verificarForm()
      });

      this.modalAdicionarService.valoresEdicaoFormulario.pipe(distinctUntilChanged()).subscribe(
        item => {
          if (item) {
            this.titulo = 'Edição';
            this.registerForm.patchValue(item);
            this.verificarForm();
          } else {
            this.titulo = 'Inclusão';
            this.verificarForm();
          }
        }
      );

    this.registerForm.get('indicaEnvioSAP').valueChanges.pipe(distinctUntilChanged())
      .subscribe(item => this.verificarForm());



    this.registerForm.get('codigoMaterial').valueChanges.pipe(distinctUntilChanged()).subscribe(
      item => this.limpar(item)
    );

    this.modalAdicionarService.valoresEdicaoFormulario.pipe(distinctUntilChanged()).subscribe(
      item => {
        if (item) {
          this.titulo = 'Edição';
          this.registerForm.patchValue(item);
        } else {
          this.titulo = 'Inclusão';
        }
      }
    );




  }

  limpar(item) {
    item = item + '';
    item = item.replace(/^0|^((?!\d).)/g, '')
    this.registerForm.get('codigoMaterial').patchValue(item, { emitEvent: false });
    this.registerForm.get('codigoMaterial').updateValueAndValidity();

  }
  verificarForm() {
    this.service.verificarForm(this.registerForm, this.tipoProcessoSelecionado);
  }

  inicializacaoForm() {
    this.registerForm = this.modalAdicionarService.inicializarForm();
  }

  validacaoTextos(nomeControl: string, nomeCampo: string, isFeminino: boolean) {
    return textosValidacaoFormulario(this.registerForm, nomeControl, nomeCampo, isFeminino);
  }

  onConfirmar() {
    this.modalAdicionarService.salvarAlteracao(this.registerForm);
  }
}
