import { Injectable } from '@angular/core';
import { ApiService } from '..';
import { Observable, BehaviorSubject } from 'rxjs';
import { StatusPagamentos } from '@shared/utils';
import { take } from 'rxjs/operators';
import { LoteCriacaoGeracaoLoteDto } from '@shared/interfaces/lote-criacao-geracao-lote-dto';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { IFiltroCriacaoLote } from 'src/app/sap/criacaoLote/interfaces/IFiltroCriacaoLote';
import { money } from '@shared/utils';
import * as $ from 'jquery';


@Injectable({
  providedIn: 'root'
})
export class LoteService {


  constructor(private http: ApiService, private helperAngular: HelperAngular
            ) { }

  public ERROMSG = new BehaviorSubject<string>(null);
  public lancamentosTableErro = new BehaviorSubject([]);
  public sucessoCriarLote = new BehaviorSubject<boolean>(false);
  // Busca pelo Endpoint

  buscaLotes(busca: string, tipoProcesso: number): Observable<any> {
    return this.http
      .get('/Lotes/RecuperarPorNumeroSAPCodigoTipoProcesso?NumeroSAP=' + busca + '&CodigoTipoProcesso=' + tipoProcesso);
  }

  getAlterarNumeroBB(numeroLote: number) {
    return this.http
      .get('/Lotes/AtualizarNumeroLoteBB?CodigoLote=' + numeroLote);
  }


  criarLote(lote: LoteCriacaoGeracaoLoteDto) {

    const URL = '/Lotes/CriarLote';
    lote.ValorLote = parseFloat(lote.ValorLote.toString());
    this.http.post(URL, JSON.stringify(lote))
      .pipe(
        take(1)
      )
      .subscribe(response => {

        if (response.sucesso) {
          {
            this.helperAngular.MsgBox2(
              '',
              'Lote gerado com sucesso!',
              'success',
              'OK'
            );
            this.sucessoCriarLote.next(response.sucesso);

            return this.obterLancamentoDoCriacao;
          }
        } else {
          this.helperAngular.MsgBox2(
            response.mensagem,
            'Desculpe! O lote não pode ser gerado',
            'warning',
            'OK'
          );
          this.lancamentosTableErro.next(response.data.dadosLancamentoDTOs);
          this.sucessoCriarLote.next(response.sucesso);
          // this.ERROMSG.next(response.mensagem);
        }
      });
  }

  private selecionarVinculadoParcelaCompromisso(lote,tipoProcesso, compromisso) {
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
          <input class=" cancelar-popup-radio" type="radio" name="opcaoEstornar" value="1" onclick="javascript: jQuery('.btn-popup-confirm').attr('disabled',null);">
          </div>
          <div class="col-sm-11">

            Cancelar a parcela e reduzir R$${money(compromisso.valorParcela)} do valor pago
            ao credor (o valor a ser pago ao credor ficaria com o valor de R$${money(compromisso.valorCompromisso - compromisso.valorParcela)})
          </div>
        </div><br />
        <div class="row">
        <div class="col-sm-1 d-flex align-items-center p-0 justify-content-center">
          <input class="cancelar-popup-radio" type="radio" name="opcaoEstornar" value="2" onclick="javascript: jQuery('.btn-popup-confirm').attr('disabled',null);">
          </div>

          <div class="col-sm-11">
            Cancelar a parcela e criar uma nova parcela no valor de R$${money(compromisso.valorParcela)}
            e com data de vencimento para daqui a 30 dias.
          </div>
        </div>
      </form>
    `
    const tituloPopup = 'Este lote foi criado a partir de um compromisso agendado da RJ';
    this.helperAngular.promptHtmlBox(htmlPopup, tituloPopup, null,
      'Confirmar', 'Cancelar')
      .then(e => e.value && this.cancelarLote(lote,tipoProcesso, compromisso,
        document.querySelector('.cancelar-popup-radio:checked')['value']));

      window.setTimeout(function(){ $('.btn-popup-confirm').attr("disabled","disabled"); },100);
  }
  cancelarLote(lote,tipoProcesso,compromisso,opcao){
    
    const URL = '/Lotes/CancelamentoLote';
    const body = { codigoLote: lote.id, codigoTipoProcesso:tipoProcesso, codigoCompromisso: compromisso!=null? compromisso.codigoCompromisso:null, opcaoCancelamento:opcao };
    this.http.post(URL, body)
      .pipe(
        take(1)
      )
      .subscribe(response => {
        if (response.sucesso) {
          this.setLoteAsStatus(lote,response.data.codigoStatusPagamento);
        } else { this.helperAngular.MsgBox2(
          response.mensagem,
          'Atenção!',
          'warning',
          'OK'
        );}
      });
  }
  avisoNovaParcela(lote,tipoProcesso,compromisso){

    this.helperAngular.promptHtmlBox(`
            <div class="row">
              <div class="col-12" >
                Este lote possui um lançamento associado a uma parcela de compromisso RJ e o compromisso possui diversos credores.
                Será criada para o compromisso uma nova parcela para daqui a 30 dias no valor de R$${money(compromisso.valorParcela)}, mantendo assim o valor total de R$${money(compromisso.valorCompromisso)} a ser pago ao credor. <br/>
                <br/>
                Confirma o cancelamento do lote?
              </div>
            </div>
            `,
              null, 'info',
              'Confirmar', 'Cancelar' ).then( result =>{
                if (result.value) { 
                  this.cancelarLote(lote,tipoProcesso,compromisso,"2");
                }
              });

  }
  avisoReducao(lote,tipoProcesso,compromisso){

    this.helperAngular.promptHtmlBox(`
            <div class="row">
              <div class="col-12" >
                Este lote possui um lançamento associado a uma parcela de compromisso RJ.
                O valor a ser pago ao credor será reduzido de R$${money(compromisso.valorParcela)}, quando o cancelamento for confirmado pelo SAP. <br/>
                Se for o caso, crie manualmente uma nova parcela no comprimisso (aba de lançamentos do processo), para substituir o valor cancelado.<br/>
                <br/>
                Confirma o cancelamento do lote?
              </div>
            </div>
            `,
              null, 'info',
              'Confirmar', 'Cancelar' ).then( result =>{
                if (result.value) { 
                  this.cancelarLote(lote,tipoProcesso,compromisso,"1");
                }
              });

  }

  verificarCompromisso(lote,tipoProcesso): void{
    const URL = '/Lotes/VerificarCompromisso?codigoLote=' + lote.id;

    this.http.get(URL)
      .pipe(
        take(1)
      )
      .subscribe(response => {
        
        
        
        if (response.sucesso) {
          
          if(response.data==null)
          {
            this.cancelarLote(lote,tipoProcesso,null,null);
          }
          else if(response.data.quantidadeCredoresAssociados<=1)
          {
            switch(lote.estado.id){
              //“Lote Gerado - Aguardando Envio para o SAP” 
              case 2:  this.cancelarLote(lote,tipoProcesso,response.data,"1"); break;
              //“Pedido SAP Criado - Aguardando Recebimento Fiscal” 
              case 6:  this.avisoReducao(lote,tipoProcesso,response.data); break;
              //“Pedido SAP Recebido Fiscal - Aguardando Confirmação de Pagamento” 
              case 15: this.selecionarVinculadoParcelaCompromisso(lote,tipoProcesso,response.data); break;
            }
          }
          else // mais de um credor:
          {
            switch(lote.estado.id){
              //“Lote Gerado - Aguardando Envio para o SAP” 
              case 2:  this.cancelarLote(lote,tipoProcesso,response.data,"1"); break;
              //“Pedido SAP Criado - Aguardando Recebimento Fiscal” 
              case 6:  
              //“Pedido SAP Recebido Fiscal - Aguardando Confirmação de Pagamento” 
              case 15: 
              this.avisoNovaParcela(lote,tipoProcesso,response.data); 
              break;
            }
          }

        } else { this.helperAngular.MsgBox2(
          response.mensagem,
          'Atenção!',
          'warning',
          'OK'
        );}
      });
  }

  private setLoteAsStatus(lote, statusId): void {
    const indexStatusPagamento = StatusPagamentos.findIndex(e => e.idStatus === statusId);
    lote.estado.id = StatusPagamentos[indexStatusPagamento].idStatus;
    lote.estado.cor = StatusPagamentos[indexStatusPagamento].cor;
    lote.estado.mensagem = StatusPagamentos[indexStatusPagamento].nomeStatus;
  }

  /**
   * Realiza a chamada dos lotes para a tela de Criação de Lotes
   */
  public obterLotesAgrupadoPorEmpresaDoGrupo(json: IFiltroCriacaoLote): Observable<any> {
    return this.http
      .post(`/Lotes/ObterLotesAgrupadoPorEmpresaDoGrupo`, json);
  }

  public obterLancamentoDoCriacao(json): Observable<any> {
    return this.http
      .post(`/Lotes/ObterLancamentosLoteCriacao`, json);
  }

  public obterLotesAgrupadoPorEmpresaCentralizadora(json: IFiltroCriacaoLote): Observable<any> {
    return this.http.post(`/Lotes/ObterLotesAgrupadoPorEmpresaCentralizadora`,
      json);
  }

  public regerarArquivoBB(formulario): Observable<any>  {
    return this.http.post('/Lotes/RegerarArquivoBB', formulario);
  }

  public validarLote(lote, tipoProcesso): Observable<any>{
    return this.http.get(`/Lotes/ValidarNumeroLote?numeroLote=`+ lote + '&codigoTipoProcesso=' + tipoProcesso);
  }

}



