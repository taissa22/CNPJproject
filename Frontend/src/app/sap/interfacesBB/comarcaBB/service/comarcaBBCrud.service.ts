

import { Injectable, ɵclearResolutionOfComponentResourcesQueue } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { ComarcaBBService } from './comarcaBB.service';


@Injectable({
  providedIn: 'root'
})
export class ComarcaBBCrudService {

  constructor(private messageService: HelperAngular,
              private fb: FormBuilder, private interfaceBbComarcaBB: ComarcaBBService) { }
registerForm: FormGroup;



private modoSalvar = 'Cadastrar';

private valor;

fecharModal = new BehaviorSubject<boolean>(false);
sucesso = new BehaviorSubject<boolean>(false);

valoresEdicaoFormulario = new BehaviorSubject<any>(null);

valores = new BehaviorSubject([]);

form: FormGroup;


  inicializarForm(): FormGroup {
    this.registerForm = this.fb.group({
      descricao: ['', [Validators.maxLength(100), Validators.required]],
      codigoBB: ['', [Validators.maxLength(9),  Validators.required]],
      codigoEstado: ['', [Validators.maxLength(2),  Validators.required]]
    });

    return this.registerForm;
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
          id: this.valor.id
        },
        registerForm.value
      );
    }
    this.interfaceBbComarcaBB.cadastrar(this.valor).subscribe(
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

}F


  // private modoSalvar = 'Cadastrar';

  // private valor;

  // fecharModal = new BehaviorSubject<boolean>(false);
  // sucesso = new BehaviorSubject<boolean>(false);

  // valoresEdicaoFormulario = new BehaviorSubject<any>(null);

  // excluirComarcaBB(ComarcaBB) {
  //   this.helperAngular.MsgBox2('Deseja excluir a comarca BB?',
  //     'Excluir Comarca BB', 'question', 'Sim', 'Não').then(
  //       item => {
  //         if (item.value) {
  //           this.interfaceBbComarcaBB.excluirComarcaBB(ComarcaBB.id)
  //             .subscribe(resposta => {
  //               if (!resposta.sucesso) {
  //                 this.helperAngular.MsgBox2(resposta.mensagem, 'Exclusão não permitida!', 'warning', 'Ok');
  //                 this.fecharModal.next(false);
  //               } else {
  //                 this.comarcaBb.getComarcaBB();
  //                 this.fecharModal.next(true);
  //                 this.sucesso.next(true);
  //               }
  //             });
  //         }
  //       }
  //     );

  // }

  // inicializarForm(): FormGroup {
  //   this.registerForm = this.fb.group({
  //     descricao: ['', [Validators.maxLength(100), Validators.required]],
  //     codigoBB: ['', [Validators.maxLength(9),  Validators.required]],
  //     codigoEstado: ['', [Validators.maxLength(2),  Validators.required]]
  //   });

  //   return this.registerForm;
  // }

  // editarComarcaBB(dados) {
  //   this.modoSalvar = 'Editar';
  //   this.valor = Object.assign({}, dados);
  //   this.valoresEdicaoFormulario.next(this.valor);
  // }

  // addComarca() {
  //   this.modoSalvar = 'Cadastrar';
  //   this.valoresEdicaoFormulario.next(null);
  // }

  // salvarComarcaBB(registerForm) {
  //   if (registerForm.valid) {
  //     if (this.modoSalvar === 'Cadastrar') {
  //       this.valor = Object.assign({}, registerForm.value);


  //       this.interfaceBbComarcaBB.salvarComarcaBB(this.valor)
  //         .pipe(take(1)).subscribe(response => {
  //           if (!response.sucesso &&
  //             response.exibeNotificacao) {
  //             this.helperAngular.MsgBox2(response.mensagem, 'Código da UF inválido!',
  //               'warning', 'Sim', 'Não').then(result => {
  //                 if (result.value) {
  //                   registerForm.value.confirmaCardastro = true;
  //                   this.salvarComarcaBB(registerForm);
  //                 }
  //               })
  //             }
  //           else if (!response.sucesso) {
  //               this.helperAngular.MsgBox2(response.mensagem, 'Ops!', 'warning', 'Ok');
  //               this.fecharModal.next(false);
  //           } else {
  //             this.fecharModal.next(true);
  //             this.sucesso.next(true);

  //             setTimeout(() => this.comarcaBb.getComarcaBB(), 500);
  //           }
  //         });

  //     } else {
  //       this.valor = Object.assign(
  //         {
  //           id: this.valor.id
  //         },
  //         registerForm.value
  //       );


  //       this.interfaceBbComarcaBB.salvarComarcaBB(this.valor).subscribe(
  //         response => {

  //           if (!response.sucesso &&
  //             response.exibeNotificacao) {
  //             this.helperAngular.MsgBox2(response.mensagem, 'Código da UF inválido!',
  //               'warning', 'Sim', 'Não').then(result => {
  //                 if (result.value) {
  //                   registerForm.value.confirmaCardastro = true;
  //                   this.salvarComarcaBB(registerForm);
  //                 }
  //               })
  //             }
  //           else if(response.sucesso){
  //             setTimeout(() => this.comarcaBb.getComarcaBB(), 500);
  //             this.fecharModal.next(true);
  //             this.sucesso.next(true);

  //           } else {

  //             this.helperAngular.MsgBox2(response.mensagem, 'Ops!', 'warning', 'Ok');
  //             this.fecharModal.next(false);

  //           }
  //         });
  //     }
  //   }

  // }


}
