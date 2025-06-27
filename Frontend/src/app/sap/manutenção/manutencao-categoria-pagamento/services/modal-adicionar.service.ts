import { Injectable } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { CategoriaPagamentoService } from 'src/app/core/services/sap/categoria-pagamento.service';
import { take, pluck } from 'rxjs/operators';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { BehaviorSubject, Observable } from 'rxjs';
import { ManutencaoCategoriaPagamentoService } from './manutencao-categoria-pagamento.service';
import { TipoLancamentoCategoriaPagamento } from '@shared/enums/tipo-lancamento-categoria-pagamento.enum';
import { ICategoriaPagamento } from '../../interface/ICategoriaPagamento';

@Injectable({
  providedIn: 'root'
})
export class ModalAdicionarService {

  constructor(private fb: FormBuilder,
    private categoriaPagamentoService: CategoriaPagamentoService,
    private helperAngular: HelperAngular,
    private manutencaoCategoriaPagamentoService: ManutencaoCategoriaPagamentoService,
  ) { }


  get tooltip() {
    if (this.manutencaoCategoriaPagamentoService.tipoLancamentoSelecionado
      == TipoLancamentoCategoriaPagamento.garantias) {
      if (this.codigoClasseGarantia == 1) {
        return '';
      } else {
        return `Os campos Envia para o SAP e Código do Material SAP não podem ser preenchidos porque o tipo de lançamento é garantia,
       e a classe de garantia é Bloqueio, Desbloqueio ou Outros.`;
      }
    } else if (this.manutencaoCategoriaPagamentoService.tipoLancamentoSelecionado
      == TipoLancamentoCategoriaPagamento.pagamentos) {
      if (this.codigoClasseGarantia != null && this.codigoClasseGarantia) {
        return `Os campos Envia para o SAP e Código do Material SAP não podem ser preenchidos porque o tipo de lançamento é pagamento,
        e a classe de garantia é Depósito ou Bloqueio.`;
      } else {
        return '';
      }
    }
  }

  get fornecedorPermitido() {
    return this.registerForm.get('fornecedoresPermitidos').value;
  }


  set fornecedorPermitido(value) {
    this.registerForm.get('fornecedoresPermitidos').setValue(value);
  }

  get codigoClasseGarantia() {
    return this.registerForm.get('codigoClasseGarantia').value;
  }

  set codigoClasseGarantia(value) {
    this.registerForm.get('codigoClasseGarantia').setValue(value);
    this.registerForm.get('codigoClasseGarantia').updateValueAndValidity();
  }

  get pagamentoA() {
    return this.registerForm.get('pagamentoA').value;
  }

  set pagamentoA(value) {
    this.registerForm.get('pagamentoA').setValue(value);
  }

  get codigoGrupoCorrecao() {
    return this.registerForm.get('codigoGrupoCorrecao').value;
  }

  set codigoGrupoCorrecao(value) {
    this.registerForm.get('codigoGrupoCorrecao').setValue(value);
  }

  get responsabilidadeOi() {
    
    if (this.registerForm.get('responsabilidadeOi').value != null
    && this.registerForm.get('responsabilidadeOi').value != '' )
      return this.registerForm.get('responsabilidadeOi').value.toString().replace(",", ".");
    else
      return this.registerForm.get('responsabilidadeOi').value;
  }

  set responsabilidadeOi(value) {
    this.registerForm.get('responsabilidadeOi').setValue(value);
  }

  registerForm: FormGroup;
  valoresEdicaoFormulario = new BehaviorSubject<any>(null);

  modoSalvar = 'Cadastrar';
  valor;
  fecharModal = new BehaviorSubject<boolean>(false);
  comboCivelEstrategico: any;


  /**
  * Inicializa o formulário da tela de modal-adicionar-despesa
  *
  */
  inicializarForm(): FormGroup {
    this.registerForm = this.fb.group({
      descricao: ['', [Validators.required, Validators.maxLength(100)]],
      codigoMaterial: ['', Validators.maxLength(18)],
      fornecedoresPermitidos: null,
      indicaAtivo: [true],
      indicaEnvioSAP: [false],
      indicaNumeroGuia: [false],
      registrarProcessosFinalizadoContabil: [false],
      escritorioPodeSolicitar: [false],
      codigoClasseGarantia: null,
      codigoGrupoCorrecao: [{ value: null, disabled: true }],
      responsabilidadeOi: '',
      justificativa: ['', [Validators.maxLength(400)]],
      influenciaContingenciaMedia: true,
      encerrarProcesso: [false],
      requerDataVencimentoDocumento: [false],
      requerComprovanteSolicitacao: [false],
      indicaHistorica: [false],
      indicaInfluenciarContingenciaMedia: [false],
      indicaInfluenciarContingencia: [false],
      pagamentoA: 1,
      idMigracaoEstrategico: null,
      comboCivelEstrategico: null,
      idMigracaoConsumidor: null,
      comboCivelConsumidor: null,
    });
    return this.registerForm;
  }

  preencherCombos(): Observable<any> {
    
    let valor = this.categoriaPagamentoService.comboBoxCategoriaPagamento(
      this.manutencaoCategoriaPagamentoService.currentValueComboTipoProcessoSubject.value,
      this.manutencaoCategoriaPagamentoService.currentValueComboLancamentoSubject.value)
      .pipe(pluck('data'));

    return valor;
  }
  preencherCombo():Observable<any> {
    let valor = this.categoriaPagamentoService.comboBoxMigracaoEstrategico().pipe(pluck('data'));
    return valor;
  }

  preencherConsumidor():Observable<any> {
    let valor = this.categoriaPagamentoService.comboBoxConsumidor().pipe(pluck('data'));
    return valor;
  }

  sucesso = new BehaviorSubject<boolean>(false);

  salvarAlteracao(registerForm) {

    if (registerForm.valid) {

      if (this.modoSalvar === 'Cadastrar') {
        this.valor = Object.assign({}, registerForm.value);
        let valorCompleto = {
          ...this.valor,
          codigoTipoProcesso: this.manutencaoCategoriaPagamentoService.currentValueComboTipoProcessoSubject.value,
          codigoTipoLancamento: this.manutencaoCategoriaPagamentoService.currentValueComboLancamentoSubject.value,
          pagamentoA: this.pagamentoA,
          responsabilidadeOi: this.responsabilidadeOi != "" &&  this.responsabilidadeOi != null   ? this.responsabilidadeOi.replace(",", ".") : null,
          idMigracaoEstrategico: this.valor.comboCivelEstrategico != null ?  this.valor.comboCivelEstrategico : null,
          idMigracaoConsumidor: this.valor.comboCivelConsumidor != null ?  this.valor.comboCivelConsumidor : null
        }

        valorCompleto.codigoMaterial == null ? valorCompleto.codigoMaterial = null : valorCompleto.codigoMaterialSAP;

        this.categoriaPagamentoService.salvarCategoriaPagamento(valorCompleto)
          .pipe(take(1)).subscribe(response => {
            if (!response.sucesso) {
              this.helperAngular
                .MsgBox2(response.mensagem, 'Ops!', 'warning', 'Ok');
              this.fecharModal.next(false);
            }
            else {
              if (response.exibeNotificacao) {
                this.helperAngular.MsgBox2(`Os lançamentos cadastrados anteriormente para esta categoria
                 NÃO serão enviados para o SAP.` , 'Atenção', 'info', 'Ok').then(result => {
                  if (result.value) {
                    this.fecharModal.next(true);
                    this.sucesso.next(true);
                    setTimeout(() => this.manutencaoCategoriaPagamentoService.getCategorias(), 500);
                  }
                }
                )
              } else {
                this.fecharModal.next(true);
                this.sucesso.next(true);
                setTimeout(() => this.manutencaoCategoriaPagamentoService.getCategorias(), 500);
              }

            }

          }
          );


      } else {
        this.valor = Object.assign(
          {
            codigo: this.valor.codigo,
            pagamentoA: this.pagamentoA
          },
          Object.assign({}, registerForm.value)
        );

        let valorCompleto = {
          ...this.valor,
          codigoTipoProcesso: this.manutencaoCategoriaPagamentoService.currentValueComboTipoProcessoSubject.value,
          codigoTipoLancamento: this.manutencaoCategoriaPagamentoService.currentValueComboLancamentoSubject.value,
          responsabilidadeOi:  this.responsabilidadeOi != "" &&  this.responsabilidadeOi != null   ? this.responsabilidadeOi.replace(",", ".") : null,
          idMigracaoEstrategico:  this.valor.comboCivelEstrategico != null ?  this.valor.comboCivelEstrategico : null,
          idMigracaoConsumidor:  this.valor.comboCivelConsumidor != null ?  this.valor.comboCivelConsumidor : null,
        };
        valorCompleto.codigoMaterial == null ? valorCompleto.codigoMaterial = null : valorCompleto.codigoMaterialSAP;



        this.categoriaPagamentoService.salvarCategoriaPagamento(valorCompleto).subscribe(
          response => {
            if (response.sucesso) {
              if (response.exibeNotificacao) {
                this.helperAngular.MsgBox2(`Os lançamentos cadastrados anteriormente para esta categoria
                 NÃO serão enviados para o SAP.` , 'Atenção', 'info', 'Ok');
              }
              setTimeout(() => this.manutencaoCategoriaPagamentoService.getCategorias(), 500);
              this.fecharModal.next(true);
              this.sucesso.next(true);

            }
            else {

              this.helperAngular.MsgBox2(response.mensagem, 'Ops!', 'warning', 'Ok');
              this.fecharModal.next(false);

            }
          });

      }
      // this.service.selectedEmpresasSapSubject.next(null);
    }

  }
  editarCategoria(valor: ICategoriaPagamento) {
    //Existem valores booleanos no banco que são nullable e precisam passar por verificação
    //para virar false
    let dados = {
      codigo: valor.codigo,
      descricao: valor.descricao,
      codigoMaterial: valor.codigoMaterialSAP == 0 ? null : valor.codigoMaterialSAP,
      fornecedoresPermitidos: valor.tipoFornecedorPermitido ? valor.tipoFornecedorPermitido : null,
      indicaAtivo: !valor.indAtivo ? false : valor.indAtivo,
      indicaEnvioSAP: !valor.indEnvioSap ? false : valor.indEnvioSap,
      indicaNumeroGuia: !valor.indicadorNumeroGuia ? false : valor.indicadorNumeroGuia,
      registrarProcessosFinalizadoContabil: !valor.indicadorFinalizacaoContabil ? false : valor.indicadorFinalizacaoContabil,
      escritorioPodeSolicitar: !valor.indEscritorioSolicitaLan ? false : valor.indEscritorioSolicitaLan,
      codigoClasseGarantia: valor.clgarCodigoClasseGarantia ? valor.clgarCodigoClasseGarantia : null,
      pagamentoA: valor.pagamentoA,
      codigoGrupoCorrecao: valor.grpcgIdGrupoCorrecaoGar ? valor.grpcgIdGrupoCorrecaoGar : null,
      justificativa: valor.descricaoJustificativa,
      justificativainfuenciacontigencia: valor.descricaoJustificativainfuenciaacontigencia,
      influenciaContingenciaMedia: !valor.indicadorContingencia ? false : valor.indicadorContingencia,
      encerrarProcesso: !valor.indEncerraProcessoContabil ? false : valor.indEncerraProcessoContabil,
      requerDataVencimentoDocumento: !valor.indicadorRequerDataVencimento ? false : valor.indicadorRequerDataVencimento,
      requerComprovanteSolicitacao: !valor.indComprovanteSolicitacao ? false : valor.indicadorRequerDataVencimento,
      indicaHistorica: valor.indicadorHistorico == null ? false : valor.indicadorHistorico,
      responsabilidadeOi: valor.responsabilidadeOi,
      idMigracaoEstrategico: valor.codMigEstrategico,
      comboCivelEstrategico: valor.codMigEstrategico,
      idMigracaoConsumidor: valor.codMigConsumidor,
      comboCivelConsumidor: valor.codMigConsumidor,
    }
    this.modoSalvar = 'Editar';
    this.valor = Object.assign({}, dados);
    this.valoresEdicaoFormulario.next(this.valor);
  }
  addCategoria() {
    this.modoSalvar = 'Cadastrar';
    this.valoresEdicaoFormulario.next(null);
  }

  excluirCategoria(categoria: ICategoriaPagamento) {
    this.helperAngular.MsgBox2(`Deseja excluir a Categoria de Pagamento
    <br> <b>${categoria.descricao}</b>?`,
      'Excluir Categoria de Pagamento', 'question', 'Sim', 'Não').then(
        item => {
          if (item.value) {
            this.categoriaPagamentoService.excluiCategoriaPagamento(categoria.codigo)
              .subscribe(resposta => {
                if (!resposta.sucesso && resposta.exibeNotificacao) {
                  this.helperAngular.MsgBox2(resposta.mensagem, 'Exclusão não permitida', 'warning', 'Ok');
                } else if (!resposta.sucesso) {
                  throw Error(resposta.mensagem);
                } else {
                  this.manutencaoCategoriaPagamentoService.getCategorias();
                  this.sucesso.next(true);
                }
              });
          }
        }
      );
  }
}
