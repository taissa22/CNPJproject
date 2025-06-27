import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {  validarObrigatoriedadeCombo } from '@shared/utils';
import { BehaviorSubject } from 'rxjs';
import { TribunalBBService } from './tribunalBBService.service';
import { HelperAngular } from '@shared/helpers/helper-angular';

@Injectable({
  providedIn: 'root'
})
export class CrudTribunalBbService {

  constructor(private fb: FormBuilder, private tribunalService: TribunalBBService,
    private messageService: HelperAngular) { }

  valoresEdicaoFormulario = new BehaviorSubject<any>({});

  private modoSalvar = 'Cadastrar';

  private valor;

  fecharModal = new BehaviorSubject<boolean>(false);
  sucesso = new BehaviorSubject<boolean>(false);
  dadosConsulta = new BehaviorSubject([]);


  form: FormGroup;

  inicializarForm() {
    this.form = this.fb.group({
      codigoBB: ['', [Validators.required]],
      indicadorInstancia: ['', [validarObrigatoriedadeCombo]],
      descricao: ['', [Validators.required]]
    })

    return this.form;
  }

  editarTribunal(dados) {
    this.modoSalvar = 'Editar';
    this.valor = Object.assign({}, dados);
    this.valoresEdicaoFormulario.next(this.valor);
  }

  addTribunal() {
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
      this.tribunalService.cadastrar(this.valor).subscribe(
        resposta => {
          resposta.then(obj => {
            if (!obj.cadastrar['sucesso']) {
              this.fecharModal.next(false);
              this.messageService.MsgBox2(obj.cadastrar['mensagem'], 'Ops!',
              'warning', 'Ok');
            } else {
              this.fecharModal.next(true);
              this.dadosConsulta.next(obj.consultar['data']);
            }
          });
        }
      );
    }

  }

}
