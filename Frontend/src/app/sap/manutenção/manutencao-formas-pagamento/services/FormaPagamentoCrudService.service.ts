import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { BehaviorSubject } from 'rxjs';
import { ManutencaoFormasPagamentoService } from './manutencao-formas-pagamento.service';
import { FormaPagamento } from './../../../../shared/interfaces/sap/FormaPagamento';

@Injectable({
  providedIn: 'root'
})
export class FormaPagamentoCrudService {

constructor(private messageService: HelperAngular,
  private manutencaoService: ManutencaoFormasPagamentoService,
  private fb: FormBuilder) { }

valoresEdicaoFormulario = new BehaviorSubject<FormaPagamento>(null);

private modoSalvar = 'Cadastrar';

private valor;

fecharModal = new BehaviorSubject<boolean>(false);
sucesso = new BehaviorSubject<boolean>(false);

valores = new BehaviorSubject([]);

form: FormGroup;

inicializarForm() {
  this.form = this.fb.group({
    descricaoFormaPagamento: ['', [Validators.required, Validators.maxLength(100),
    ]],
    restrita: false,
    requerBordero: true
  });
  return this.form;
}

editar(dados) {
  this.modoSalvar = 'Editar';
  this.valor = Object.assign({}, dados);
  this.valoresEdicaoFormulario.next(this.valor);
}

adicionar() {
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
          codigo: this.valor.codigo
        },
        registerForm.value
      );
    }
    this.manutencaoService.cadastrar(this.valor).subscribe(
      resposta => {
        resposta.then(response => {
          if (!response.cadastrar['sucesso'] && response.cadastrar['exibeNotificacao']) {
            this.messageService.MsgBox2(response.cadastrar['mensagem'],
              'Exclusão não permitida', "warning", 'Ok')
          }
          else if (!response.cadastrar['sucesso']) {
            this.messageService.MsgBox2(response.cadastrar['mensagem'], 'Ops!',
              'warning', 'Ok');
            this.fecharModal.next(false);
          } else {
            this.valores.next(response.consultar['data']);
            this.fecharModal.next(true);
          }
        });
      }
    );
  }

}


}
