import { Injectable } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { CentroCustoService } from 'src/app/core/services/sap/centroCusto.service';
import { ManutencaoCentroCustoService } from './manutencao-centro-custo.service';
import { take } from 'rxjs/operators';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CentroCustoCrudService {

  constructor(private fb: FormBuilder, private centroCustoService: CentroCustoService,
    private manutencaoCentroCustoService: ManutencaoCentroCustoService,
    private messageService: HelperAngular) { }
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
      descricaoCentroCusto: ['', [Validators.maxLength(100), Validators.required
      ]],
      centroCustoSAP: ['', [Validators.maxLength(10), Validators.required
      ]],
      indicaAtivo: true
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
            codigo: this.valor.codigo
          },
          registerForm.value
        );
      }
      this.manutencaoCentroCustoService.cadastrar(this.valor).subscribe(
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



  // excluirCentroCustoSap(centroCusto) {
  //   this.helperAngular.MsgBox2('Deseja excluir o centro de custo?',
  //     'Excluir Centro de Custo', 'question', 'Sim', 'Não').then(
  //       item => {
  //         if (item.value) {
  //           this.centroCustoService.excluirCentroCusto(centroCusto.codigo)
  //             .subscribe(resposta => {
  //               if (!resposta.sucesso && resposta.exibeNotificacao) {
  //                 this.helperAngular.MsgBox2(resposta.mensagem, 'Exclusão não permitida', 'warning', 'Ok');
  //                 this.fecharModal.next(false);
  //               } else if(!resposta.sucesso) {
  //                 throw Error(resposta.mensagem);
  //               } else {
  //                 if(this.manutencaoCentroCustoService.centroCustoSubject.value.length <= 1){
  //                   this.manutencaoCentroCustoService.paginaSubject.next(this.manutencaoCentroCustoService.paginaSubject.value -1)
  //                 }

  //                 this.manutencaoCentroCustoService.getCentroCusto();
  //                 this.fecharModal.next(true);
  //                 this.sucesso.next(true);
  //               }
  //             });
  //         }
  //       }
  //     );

  // }


  // editarCentroCusto(dados) {
  //   this.modoSalvar = 'Editar';
  //   this.valor = Object.assign({}, dados);
  //   this.valoresEdicaoFormulario.next(this.valor);
  // }

  // addCategoria() {
  //   this.modoSalvar = 'Cadastrar';
  //   this.valoresEdicaoFormulario.next(null);
  // }

  // salvarCentroCusto(registerForm) {
  //   if (registerForm.valid) {
  //     if (this.modoSalvar === 'Cadastrar') {
  //       this.valor = Object.assign({}, registerForm.value);


  //       this.centroCustoService.salvarCentroCusto(this.valor)
  //         .pipe(take(1)).subscribe(response => {
  //           if (!response.sucesso) {
  //               this.helperAngular.MsgBox2(response.mensagem, 'Ops!', 'warning', 'Ok');
  //               this.fecharModal.next(false);
  //           } else {
  //             this.fecharModal.next(true);
  //             this.sucesso.next(true);

  //             setTimeout(() => this.manutencaoCentroCustoService.getCentroCusto(), 500);
  //           }
  //         });

  //     } else {
  //       this.valor = Object.assign(
  //         {
  //           codigo: this.valor.codigo
  //         },
  //         registerForm.value
  //       );


  //       this.centroCustoService.salvarCentroCusto(this.valor).subscribe(
  //         response => {
  //           if (response.sucesso) {
  //             setTimeout(() => this.manutencaoCentroCustoService.getCentroCusto(), 500);
  //             this.fecharModal.next(true);
  //             this.sucesso.next(true);

  //           } else {
  //             this.helperAngular.MsgBox2(response.mensagem, 'Atenção!', 'warning', 'Ok');
  //             this.fecharModal.next(false);

  //           }
  //         });
  //     }
  //   }

  // }


}
