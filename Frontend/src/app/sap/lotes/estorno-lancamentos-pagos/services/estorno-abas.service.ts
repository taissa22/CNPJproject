import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { DadosLancamentoEstornoDTO } from '@shared/interfaces/dados-lancamento-estorno-dto';
import { money } from '@shared/utils';
import { BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';
import { LancamentoProcessoService } from 'src/app/core/services/sap/lancamento-processo.service';
import { DespesasTabService } from './despesas-tab.service';
import { EstornoLancamentosPagosService } from './estorno-lancamentos-pagos.service';
import { GarantiasTabService } from './garantias-tab.service';
import { PagamentosTabService } from './pagamentos-tab.service';

@Injectable({
  providedIn: 'root'
})
export class EstornoAbasService {

  constructor(private messageService: HelperAngular,
    private estornoLancamentosPagosService: EstornoLancamentosPagosService,
    private lancamentoProcessoService: LancamentoProcessoService,
    private despesasTabService: DespesasTabService,
    private pagamentosTabService: PagamentosTabService,
    private garantiasTabService: GarantiasTabService,
    private router: Router) { }

  public estornoAPIResponseSubject = new BehaviorSubject({});

  garantias = new BehaviorSubject<any[]>([]);
  despesas =  new BehaviorSubject< any[]>([]);
  pagamentos =  new BehaviorSubject<any[]>([]);

  estornou : boolean = false;

  set valoresAbas(dadosLancamentos){
  this.garantias.next(
       dadosLancamentos.filter(e => e.codigoTipoLancamento == 1)
     );

     this.despesas.next(
      dadosLancamentos.filter(e => e.codigoTipoLancamento == 2)
     );

     this.pagamentos.next(
      dadosLancamentos.filter(e => e.codigoTipoLancamento == 3)
     );
  }


  limparSelecao() {
    this.garantiasTabService.limparSelecao();
    this.despesasTabService.limparSelecao();
    this.pagamentosTabService.limparSelecao();
  }

  //#region API callers
  private enviaEstornoAPI(objSelecionado: DadosLancamentoEstornoDTO) {
    objSelecionado.codigoTipoProcesso = this.estornoLancamentosPagosService.currentItemComboboxTipoProcesso.value
    this.lancamentoProcessoService
      .estornarLancamento(objSelecionado,
        this.estornoLancamentosPagosService.currentItemComboboxTipoProcesso.value)
      .pipe(take(1))
      .subscribe(response => {
        if (response['sucesso']) {
          this.estornoAPIResponseSubject.next(response['data']);
          this.messageService.MsgBox2('Lançamento estornado com sucesso.', 'Sucesso!',
            'success', 'Ok').then(item => {
              if (item.value) {
              this.estornou = true;
              }
            });
        } else {
          this.messageService.MsgBox2(response.mensagem, 'Atenção!', "warning", 'OK')
        }
      });
    return this.estornoAPIResponseSubject;
  }
  //#endregion

  estornar(objSelecionado) {
  return   this.messageService
      .MsgBox2(`Deseja estornar o lançamento no valor de <br>
      <b>${money(objSelecionado.valor)}</b>?`,
        'Estornar Lançamento Pago',
        'question',
        'Sim',
        'Não')
      // .then(e => e.value && this.realizaEstorno(objSelecionado));
  }

   realizaEstorno(objSelecionado) {
    // Trabalhista
    if (this.estornoLancamentosPagosService.currentItemComboboxTipoProcesso.value == 2) {
      const quantidadeCredoresAssociados = objSelecionado['quantidadeCredoresAssociados'];
      const codigoCompromisso = objSelecionado['codigoCompromisso'];


      const facadeFuncao = (quantidadeCredoresAssociados, obj) => {

        if(quantidadeCredoresAssociados == 0 ){
          if(codigoCompromisso > 0){
            this.selecionarVinculadoParcelaCompromisso(obj)
          }else{
            this.enviaEstornoAPI(obj);
          }
        }else if(quantidadeCredoresAssociados == 1){
          return this.selecionarVinculadoParcelaCompromisso(obj)
        }else{
          return this.selecionarMultiplosCredores(obj)
        }


        // switch (quantidadeCredoresAssociados) {
        //   case 0: return this.enviaEstornoAPI(obj);
        //   case 1: return this.selecionarVinculadoParcelaCompromisso(obj);
        //   default: return this.selecionarMultiplosCredores(obj);
        // }
      }

      return facadeFuncao(quantidadeCredoresAssociados, objSelecionado);
    } else {
      return this.enviaEstornoAPI(objSelecionado);
    }
  }

  private selecionarVinculadoParcelaCompromisso(objSelecionado) {
    // TODO: Refactor
    const htmlPopup = `
      <form>
        <div class="row">
          <div class="col-12 paddingEstorno" >
          O que deseja fazer?
            </div>
            </div><br />
        <div class="row">
           <div class="col-sm-1 d-flex align-items-center p-0 justify-content-center">
          <input class=" estorno-popup-radio" type="radio" name="opcaoEstornar" checked value=1>
          </div>
          <div class="col-sm-11">

            Estornar o lançamento e reduzir R$${money(objSelecionado.valor)} do valor pago
            ao credor (o valor a ser pago ao credor ficaria com o valor de R$${money(objSelecionado.valorCompromisso - objSelecionado.valor)})
          </div>
        </div><br />
        <div class="row">
        <div class="col-sm-1 d-flex align-items-center p-0 justify-content-center">
          <input class="estorno-popup-radio" type="radio" name="opcaoEstornar" value=2>
          </div>

          <div class="col-sm-11">
            Estornar o lançamento e criar uma nova parcela no valor de R$${money(objSelecionado.valor)}
            e com data de vencimento para daqui a 30 dias.
          </div>
        </div>
      </form>
    `
    const tituloPopup = 'Este lançamento está vinculado a uma parcela de compromisso da RJ';
    this.messageService.promptHtmlBox(htmlPopup, tituloPopup, null,
      'Confirmar', 'Cancelar')
      .then(e => e.value && this.estornarVinculadoParcelaCompromisso(objSelecionado,
        document.querySelector('.estorno-popup-radio:checked')['value']))
  }

  private estornarVinculadoParcelaCompromisso(objSelecionado, opcao) {
    if (opcao == 1) {
      objSelecionado['reduzirPagamentoCredor'] = true;
    } else if (opcao == 2) {
      objSelecionado['criarNovaParcelaFutura'] = true;
    }
    return this.enviaEstornoAPI(objSelecionado);
  }

  private selecionarMultiplosCredores(objSelecionado) {
    const htmlPopup = `
      <div>
        <div class="title-atencao pb-4">Atenção!</div>
        Este lançamento está associado a uma parcela de compromisso da RJ
        e o compromisso possui diversos credores. Será criada para o compromisso uma
        nova parcela para daqui a 30 dias no valor de R$${money(objSelecionado.valor)}, mantendo assim o valor
        total de R$${money(objSelecionado.valorCompromisso)} a ser pago ao credor.
        <div class='pt-2'>Confirma o estorno do lançamento?<div>
      </div>
    `
    this.messageService.promptHtmlBox(htmlPopup, null, null,
      'Confirmar', 'Cancelar')
      .then(e => e.value && this.chamarEstornoLancamento(objSelecionado));
  }

  private chamarEstornoLancamento(objSelecionado) {
    objSelecionado['criarNovaParcelaFutura'] = true;
    return this.enviaEstornoAPI(objSelecionado);
  }
}
