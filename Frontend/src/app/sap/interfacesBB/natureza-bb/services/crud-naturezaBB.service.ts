import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { BehaviorSubject } from 'rxjs';
import { NaturezaBBService } from './naturezaBB.service';

@Injectable({
  providedIn: 'root'
})
export class CrudNaturezaBBService {

  constructor(private fb: FormBuilder, private naturezaService: NaturezaBBService,
    private messageService: HelperAngular) { }

  valoresEdicaoFormulario = new BehaviorSubject<any>(null);

  private modoSalvar = 'Cadastrar';

  private valor;

  fecharModal = new BehaviorSubject<boolean>(false);
  sucesso = new BehaviorSubject<boolean>(false);

  naturezaBB = new BehaviorSubject([]);

  form: FormGroup;

  inicializarForm() {
    this.form = this.fb.group({
      codigoBB: ['', [Validators.required]],
      descricao: ['', [Validators.required]]
    })

    return this.form;
  }

  editarNatureza(dados) {
    this.modoSalvar = 'Editar';
    this.valor = Object.assign({}, dados);
    this.valoresEdicaoFormulario.next(this.valor);
  }

  addNatureza() {
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
      this.naturezaService.cadastrar(this.valor).subscribe(
        resposta => {
          resposta.then(response => {
            if (!response.cadastrar['sucesso']) {
              this.messageService.MsgBox2(response.cadastrar['mensagem'], 'Ops!',
              'warning', 'Ok');
              this.fecharModal.next(false);
            } else {
              this.naturezaBB.next(response.consultar['data']);
              this.fecharModal.next(true);
            }
        });
        }
      );
    }

  }


}
