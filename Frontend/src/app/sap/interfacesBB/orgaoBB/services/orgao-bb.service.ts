import { Injectable } from '@angular/core';
import { ManutencaoCommonService } from 'src/app/sap/shared/services/manutencao-common.service';
import { OrgaoBB } from '@shared/interfaces/orgao-bb';
import { BBOrgaoService } from 'src/app/core/services/sap/bborgao.service';
import { BehaviorSubject } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { validarObrigatoriedadeCombo } from '@shared/utils';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class OrgaoBBService extends ManutencaoCommonService<OrgaoBB, number> {
  public orgaoSelecionado: any;
  registerForm: FormGroup;

  modoSalvar = 'Cadastrar'
  private valor: any;
  helperAngular: any;

  orgaosBB = new BehaviorSubject({});

  fecharModal = new BehaviorSubject<boolean>(false);

  constructor(protected service: BBOrgaoService,
    private fb: FormBuilder, private messageService: HelperAngular) {
    super(service);
  }

  comboValueTribunalSubject = new BehaviorSubject<any[]>(null);
  comboValueComarcaSubject = new BehaviorSubject<any[]>(null);

  preencherValoresCombo(route) {
    route.data.subscribe(listas => {
      this.comboValueTribunalSubject.next(listas.combo.bbTribunais);
      this.comboValueComarcaSubject.next(listas.combo.bbComarcas);
    });
  }


  inicializarForm(): FormGroup {
    this.registerForm = this.fb.group({
      codigo: ['', [Validators.maxLength(9), Validators.required]],
      nome: ['', [Validators.maxLength(100), Validators.required]],
      codigoBBComarca: [null, [validarObrigatoriedadeCombo]],
      codigoBBTribunal: [null, [validarObrigatoriedadeCombo]],
    });

    return this.registerForm;
  }

  salvarOrgaoBB(registerForm) {

    if (registerForm.valid) {
      if (this.modoSalvar === 'Cadastrar') {
        this.valor = Object.assign({}, registerForm.value);


        this.cadastrar(this.valor).subscribe(response => {
          response.then(responseObject => {
            if (responseObject.cadastrar['sucesso']) {
              this.fecharModal.next(true);
              this.orgaosBB.next(responseObject.consultar);
            } else {
              this.messageService.MsgBox2(responseObject.cadastrar['mensagem'], 'Ops!', 'warning', 'Ok');
              this.fecharModal.next(false);

            }
          })
        });

      } else {
        this.valor = Object.assign(
          {
            id: this.orgaoSelecionado.id
          },
          registerForm.value
        );


        this.cadastrar(this.valor).subscribe(response => {
          response.then(responseObject => {
            if (responseObject.cadastrar['sucesso']) {
              // setTimeout(() => this.comarcaBb.getComarcaBB(), 500);
              this.fecharModal.next(true);
              // this.sucesso.next(true);
              this.orgaosBB.next(responseObject.consultar);
            } else {
              this.messageService.MsgBox2(responseObject.cadastrar['mensagem'], 'Ops!', 'warning', 'Ok');
              this.fecharModal.next(false);

            }
          })
        });
      }
    }
  }


  excluirOrgao(orgao) {
    this.messageService.MsgBox2('Deseja excluir o Órgão?',
      'Excluir Órgão', 'question', 'Sim', 'Não').then(
        item => {
          if (item.value) {
            this.excluir(orgao.id).pipe(take(1)).subscribe(
              response => {
                response.then(responseObject => {
                  if (responseObject.excluir['sucesso'])
                    this.orgaosBB.next(responseObject.consultar);
                  else {
                    this.messageService.MsgBox(responseObject.excluir['mensagem'], 'Ops!');
                  }
                })
              }
            );
          }
        });

  }
}
