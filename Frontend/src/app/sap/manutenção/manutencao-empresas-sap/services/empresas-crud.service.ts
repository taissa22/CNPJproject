import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { EmpresasSapDTO } from '@shared/interfaces/empresas-sap-dto';
import Swal from 'sweetalert2';
import { LancamentoService } from 'src/app/core/services/sap/lancamento.service';
import { EmpresasSAPService } from 'src/app/core/services/sap/empresas-sap.service';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { ManutencaoEmpresasSAPService } from './manutencao-empresas-sap.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class EmpresasCrudService {

  constructor(
    private messageService: HelperAngular,
    private manutencaoEmpresaService: ManutencaoEmpresasSAPService,
    private fb: FormBuilder) { }

  valoresEdicaoFormulario = new BehaviorSubject<any>(null);

  private modoSalvar = 'Cadastrar';

  private valor;

  fecharModal = new BehaviorSubject<boolean>(false);
  sucesso = new BehaviorSubject<boolean>(false);

  valores = new BehaviorSubject([]);

  form: FormGroup;

  inicializarForm() {
    this.form =  this.fb.group({
          sigla: ['', [Validators.required, Validators.maxLength(4)
           ]],
          nome: ['', [Validators.required, Validators.maxLength(100)
            ]],
          indicaAtivo: [true],
          codigoOrganizacaocompra: ['', [Validators.required, Validators.minLength(4),
          Validators.maxLength(4),]],
          indicaEnvioArquivoSolicitacao: [false]});
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

  salvar(registerForm , alterarSiglaIgual = false) {
    if (registerForm.valid) {
      if (this.modoSalvar === 'Cadastrar') {
        this.valor = Object.assign({confirmaSiglaRepetidaNaAlteracao: alterarSiglaIgual}, registerForm.value);
      } else {
        this.valor = Object.assign(
          {
            id: this.valor.id,
            confirmaSiglaRepetidaNaAlteracao: alterarSiglaIgual
          },
          registerForm.value
        );
      }
      this.manutencaoEmpresaService.cadastrar(this.valor).subscribe(
        resposta => {
          resposta.then(response => {
            if(response.cadastrar['data'] != null && response.cadastrar['data'].confirmacaoEnvio == true ){
              this.messageService.MsgBox2(response.cadastrar['mensagem'], 'Atenção!',
                'warning', 'Sim', 'Não').then(resposta => {
                  if (resposta.value) {
                    
                    this.salvar(registerForm, true)

                  }
                });
               
              
              this.fecharModal.next(false);

            }
            else if (!response.cadastrar['sucesso'] && response.cadastrar['exibeNotificacao']) {
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
