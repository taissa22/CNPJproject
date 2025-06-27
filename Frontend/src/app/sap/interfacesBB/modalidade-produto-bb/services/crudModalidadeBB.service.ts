import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TribunalBBService } from '../../tribunaisBB/services/tribunalBBService.service';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { ModalidadeProdutoBbService } from './modalidade-produto-bb.service';

@Injectable({
  providedIn: 'root'
})
export class CrudModalidadeBBService {

  constructor(private fb: FormBuilder, private modalidadeService: ModalidadeProdutoBbService,
              private messageService: HelperAngular) { }

  valoresEdicaoFormulario = new BehaviorSubject<any>(null);

  private modoSalvar = 'Cadastrar';

  private valor;

  fecharModal = new BehaviorSubject<boolean>(false);
  sucesso = new BehaviorSubject<boolean>(false);

  modalidadeBB = new BehaviorSubject([]);

  form: FormGroup;

  inicializarForm() {
    this.form = this.fb.group({
      codigoBB: ['', [Validators.required]],
      descricao: ['', [Validators.required]]
    });

    return this.form;
  }

  editarModalidade(dados) {
    this.modoSalvar = 'Editar';
    this.valor = Object.assign({}, dados);
    this.valoresEdicaoFormulario.next(this.valor);
  }

  addModalidade() {
    this.modoSalvar = 'Cadastrar';
    this.valoresEdicaoFormulario.next(null);
  }

  salvar(registerForm) {
    if (registerForm.valid) {
      if (this.modoSalvar === 'Cadastrar') {
        this.valor = Object.assign({}, registerForm.value);
      } else {
        this.valor = Object.assign(
          {
            id: this.valor.id
          },
          registerForm.value
        );
      }
      this.modalidadeService.cadastrar(this.valor).subscribe(
        resposta => {
          resposta.then(
            response => {
              if (!response.cadastrar['sucesso']) {
                this.messageService.MsgBox2(response.cadastrar['mensagem'], 'Ops!',
                'warning', 'Ok');
                this.fecharModal.next(false);
              } else {
                this.fecharModal.next(true);
              }
              this.modalidadeBB.next(response.consultar.data);
            }
          );
        }
      );
    }

  }

}
