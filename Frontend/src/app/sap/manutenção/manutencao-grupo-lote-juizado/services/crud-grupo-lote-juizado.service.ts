import { GrupoLoteJuizadoService } from './grupo-lote-juizado.service';
import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NaturezaBBService } from 'src/app/sap/interfacesBB/natureza-bb/services/naturezaBB.service';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CrudGrupoLoteJuizadoService {


  constructor(private fb: FormBuilder, private grupoLoteJuizadoServide:GrupoLoteJuizadoService,
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
      descricao: ['', [Validators.required]]
    })

    return this.form;
  }

  editarGrupoLoteJuizado(dados) {
    this.modoSalvar = 'Editar';
    this.valor = Object.assign({}, dados);
    this.valoresEdicaoFormulario.next(this.valor);
  }

  addGrupoLoteJuizado() {
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
      this.grupoLoteJuizadoServide.cadastrar(this.valor).subscribe(
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
