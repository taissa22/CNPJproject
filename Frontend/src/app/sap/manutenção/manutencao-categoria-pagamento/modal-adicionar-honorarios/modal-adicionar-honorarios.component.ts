import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap';
import { ManutencaoCategoriaPagamentoService } from '../services/manutencao-categoria-pagamento.service';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';
import { textosValidacaoFormulario } from '@shared/utils';
import { ModalAdicionarGarantiasService } from '../services/modal-adicionar-garantias.service';
import { ModalAdicionarService } from '../services/modal-adicionar.service';
import { distinctUntilChanged } from 'rxjs/operators';
import { TipoLancamentoCategoriaPagamento } from '@shared/enums/tipo-lancamento-categoria-pagamento.enum';

@Component({
  selector: 'app-modal-adicionar-honorarios',
  templateUrl: './modal-adicionar-honorarios.component.html',
  styleUrls: ['./modal-adicionar-honorarios.component.scss']
})
export class ModalAdicionarHonorariosComponent implements OnInit {

  constructor(public bsModalRef: BsModalRef,
    private manutencaoService: ManutencaoCategoriaPagamentoService,
    private service: ModalAdicionarGarantiasService,
    public modalAdicionarService: ModalAdicionarService) { }

  registerForm: FormGroup;

  tipoProcessoSelecionado;

  tipoProcessoEnum = TipoProcessoEnum;
  listCivelEstrategico;
  listCivelConsumidor;
  titulo;

  ngOnInit() {
    this.tipoProcessoSelecionado = this.manutencaoService.tipoProcessoSelecionado;
    this.validation();
    this.registerForm.get('codigoMaterial').valueChanges.pipe(distinctUntilChanged()).subscribe(
      item => this.limpar(item)
    );


    this.modalAdicionarService.preencherCombo().subscribe(
      item => {
        
        this.listCivelEstrategico = item.categoriaPagamento.sort((a,b) => a.descricao.localeCompare(b.descricao)).filter(r => r.tipoLancamento === TipoLancamentoCategoriaPagamento.honorários).map(r => ({ id: r.id, descricao: r.descricao + (r.ativo ? '' : ' [INATIVO]') }));
      }
    );

    this.modalAdicionarService.preencherConsumidor().subscribe(
      item => {
        
        this.listCivelConsumidor = item.categoriaPagamento.sort((a,b) => a.descricao.localeCompare(b.descricao)).filter(r => r.tipoLancamento === TipoLancamentoCategoriaPagamento.honorários).map(r => ({ id: r.id, descricao: r.descricao + (r.ativo ? '' : ' [INATIVO]') }));
      }
    );

    this.modalAdicionarService.fecharModal.subscribe(
      item => {
        if (item) {
          this.bsModalRef.hide();
        }
      }
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
