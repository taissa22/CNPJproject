import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ManutencaoCategoriaPagamentoService } from '../services/manutencao-categoria-pagamento.service';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';
import { textosValidacaoFormulario } from '@shared/utils';
import { ModalAdicionarPagamentoService } from '../services/modal-adicionar-pagamento.service';
import { ModalAdicionarService } from '../services/modal-adicionar.service';
import { ModalAdicionarRecuperacaoPagamentoService } from '../services/modal-adicionar-recuperacao-pagamento.service';
import { distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-modal-adicionar-recuperacao-pagamento',
  templateUrl: './modal-adicionar-recuperacao-pagamento.component.html',
  styleUrls: ['./modal-adicionar-recuperacao-pagamento.component.scss']
})
export class ModalAdicionarRecuperacaoPagamentoComponent implements OnInit {

  constructor(public bsModalRef: BsModalRef,
    private manutencaoService: ManutencaoCategoriaPagamentoService,
    private service: ModalAdicionarRecuperacaoPagamentoService,
    public modalAdicionarService: ModalAdicionarService) { }

  registerForm: FormGroup;

  tipoProcessoSelecionado;

  tipoProcessoEnum = TipoProcessoEnum;

  titulo;

  ngOnInit() {
    this.tipoProcessoSelecionado = this.manutencaoService.tipoProcessoSelecionado;
    this.validation();
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

    this.modalAdicionarService.fecharModal.subscribe(
      item => {
        if (item) {
          this.bsModalRef.hide();
        }
      }
    );

  }

  limpar(item) {
    this.registerForm.get('codigoMaterial').patchValue(item, { emitEvent: false });
    this.registerForm.get('codigoMaterial').updateValueAndValidity();
  }
  validation() {
    this.registerForm = this.modalAdicionarService.inicializarForm();
  }

  validacaoTextos(nomeControl: string, nomeCampo: string, isFeminino: boolean) {
   return textosValidacaoFormulario(this.registerForm, nomeControl, nomeCampo, isFeminino);
  }

  onConfirmar() {
    this.modalAdicionarService.salvarAlteracao(this.registerForm);
  }
}
