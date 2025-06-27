import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { BehaviorSubject } from 'rxjs';
import { StatusParcelaBBService } from './status-parcela-bb.service';

@Injectable({
  providedIn: 'root'
})
export class CrudStatusParcelaService {

  constructor(private fb: FormBuilder, private statusParcelaService: StatusParcelaBBService,
    private messageService: HelperAngular) { }

  valoresEdicaoFormulario = new BehaviorSubject<any>(null);

  private modoSalvar = 'Cadastrar';

  private valor;

  statusParcela = new BehaviorSubject([]);
  fecharModal = new BehaviorSubject<boolean>(false);
  sucesso = new BehaviorSubject<boolean>(false);



  form: FormGroup;

  inicializarForm() {
    this.form = this.fb.group({
      codigoBB: ['', [Validators.required]],
      descricao: ['', [Validators.required]]
    });

    return this.form;
  }

  editarParcela(dados) {
    this.modoSalvar = 'Editar';
    this.valor = Object.assign({}, dados);
    this.valoresEdicaoFormulario.next(this.valor);
  }

  addParcela() {
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
      this.statusParcelaService.cadastrar(this.valor).subscribe(
        resposta => {
          resposta.then(
            respostaObject => {
              if (!respostaObject.cadastrar['sucesso']) {
                this.fecharModal.next(false);
                this.messageService.MsgBox2(respostaObject.cadastrar['mensagem'], 'Ops!',
                'warning', 'Ok');
              } else {
                this.fecharModal.next(true);
                this.statusParcela.next(respostaObject.consultar['data'])
              }
            })
        }
      );
    }
  }


}
