import { HelperAngular } from './../../../../shared/helpers/helper-angular';
import { BehaviorSubject } from 'rxjs';
import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { FornecedoresContingenciaSapService } from './fornecedores-contingencia-sap.service';

@Injectable({
  providedIn: 'root'
})
export class CrudFornecedoresContingenciaSapService {

  constructor(private fb: FormBuilder,
    private fornecedoresContingenciaService: FornecedoresContingenciaSapService,
  private messageService: HelperAngular) { }

  private form: FormGroup;

  get fornecedoresSelecionado(){
    return this.fornecedoresContingenciaService.fornecedoresSelecionado.value
  }
  modoSalvar = 'Cadastrar';
  valor;
  fecharModal = new BehaviorSubject<boolean>(false);
  fornecedorSubject = new BehaviorSubject<boolean>(false);




  inicializarForm() {
    this.form = this.fb.group({
      codigo: {value: '', disabled: true},
      nome: {value: '', disabled: true},
      cnpj: {value: '', disabled: true},
      valorCartaFianca: '',
      dataVencimentoCartaFianca: null,
      statusFornecedor: "2"
    });
    return this.form;
  }
  salvarFornecedor(registerForm) {
    if (registerForm.valid) {
      this.valor = Object.assign(
          {
            id: this.fornecedoresSelecionado.id
          },
          registerForm.value
        );
      this.fornecedoresContingenciaService.cadastrar(this.valor).subscribe(response => {
          response.then(responseObject => {
            if (responseObject.cadastrar.sucesso) {
              this.fecharModal.next(true);
              this.fornecedorSubject.next(responseObject.consultar);

            } else {
              this.messageService.MsgBox2(responseObject.cadastrar.mensagem, 'Ops!', 'warning', 'Ok');
              this.fecharModal.next(false);
            }
          });
        });
      }
    }
  }
