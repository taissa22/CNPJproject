import { HelperAngular } from '@shared/helpers/helper-angular';
import { Injectable } from '@angular/core';
import { CorStatusPagamento } from '@shared/enums/cor-status-pagamento.enum';
import { environment } from 'src/environments/environment';
import { ItemExpansivel } from '@shared/interfaces/item-expansivel';
import { HttpClient } from '@angular/common/http';
import { map, pluck } from 'rxjs/operators';
import { IResultado } from '@shared/interfaces/resultado';
import { ApiService } from '..';
import { BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';
import { IColoredMessage } from '@shared/interfaces/colored-message';
import { InfoTooltip } from '../../models/info-tooltip.model';
import { FilterService } from 'src/app/sap/consultaLote/services/filter.service';
import { LoteService } from './lote.service';
import { StatusPagamentoEnum } from '@shared/enums/status-pagamento.enum';

const RESULTADO_ENDPOINT = environment.api_url + '/Lotes/RecuperarPorFiltroPaginado';

@Injectable({
  providedIn: 'root'
})
export class ResultadoService {
  public listaResultados = new BehaviorSubject<ItemExpansivel[]>({} as ItemExpansivel[]);
  public filtros;
  public infoTooltip: InfoTooltip = null;
  constructor(private client: HttpClient, private router: Router, private loteService: LoteService,
   private messageService: HelperAngular
 ) { }


  updateData(listaResultados: ItemExpansivel[]) {
    this.listaResultados.next(listaResultados);
  }

  redirectToResultado(listaResultados: ItemExpansivel[]) {
    this.updateData(listaResultados);
    this.router.navigate(['/sap/lote/consulta/resultado']);
  }

  filtroMap(filtro: string): string {
    const mappedFiltro = {
      dataCriacaoPedidoMenor: 'Data Criação Pedido Inicial',
      dataCriacaoPedidoMaior: 'Data Criação Pedido Final',
      dataCriacaoMenor: 'Data Criação Lote Inicial',
      dataCriacaoMaior: 'Data Criação Lote Final',
      listaPedidoSAP: 'Nº Pedido Sap',
      listaNumeroGuia: 'Nº Guia',
      tipoProcesso: 'Tipo de Processo'
    };

    if (mappedFiltro.hasOwnProperty(filtro)) { return mappedFiltro[filtro]; }
  }



  getListaResultados(json) {
    const StatusPagamentos = [{
        idStatus: StatusPagamentoEnum.novo,
        nomeStatus: 'Novo - Aguardando Geração de Lote',
        detalhe: '',
        cor: ''
      },
      {
        idStatus: StatusPagamentoEnum.loteGerado,
        nomeStatus: 'Lote Gerado - Aguardando Envio para o SAP',
        detalhe: '',
        cor: '#6DAFE9'
      },
      {
        idStatus: StatusPagamentoEnum.loteCancelado,
        nomeStatus: 'Lote Cancelado',
        detalhe: '',
        cor: '#F80000'
      },
      {
        idStatus: StatusPagamentoEnum.loteEnviado,
        nomeStatus: 'Lote Enviado - Aguardando Criação do Pedido SAP',
        detalhe: '',
        cor: '#3C89CC'
      },
      {
        idStatus: StatusPagamentoEnum.erroCriacaoSap,
        nomeStatus: 'Erro na Criação do Pedido SAP - Aguardando Criação de Lote',
        detalhe: '',
        cor: '#FF8C00'
      },
      {
        idStatus: StatusPagamentoEnum.pedidoSapCriado,
        nomeStatus: 'Pedido SAP Criado - Aguardando Recebimento Fiscal',
        detalhe: '',
        cor: '#3270A7'
      },
      {
        idStatus: StatusPagamentoEnum.aguardandoEnvioCancelamentoSap,
        nomeStatus: 'Aguardando Envio para Cancelamento do Pedido SAP',
        detalhe: '',
        cor: '#FF8C00'
      },
      {
        idStatus: StatusPagamentoEnum.pedidoSapEnviado,
        nomeStatus: 'Pedido SAP Enviado - Aguardando Cancelamento',
        detalhe: '',
        cor: '#FC4646'
      },
      {
        idStatus: StatusPagamentoEnum.erroCancelamentoSap,
        nomeStatus: 'Erro no Cancelamento do Pedido SAP',
        detalhe: '',
        cor: '#FF8C00'
      },
      {
        idStatus: StatusPagamentoEnum.pedidoSapCancelado,
        nomeStatus: 'Pedido SAP Cancelado - Aguardando Geração de Lote',
        detalhe: '',
        cor: '#FF8C00'
      },
      {
        idStatus: StatusPagamentoEnum.pedidoSapPago,
        nomeStatus: 'Pedido SAP Pago',
        detalhe: '',
        cor: '#19A519'
      },
      {
        idStatus: StatusPagamentoEnum.pedidoSapPagoManualmente,
        nomeStatus: 'Pedido SAP Pago Manualmente',
        detalhe: '',
        cor: '#19A519'
      },
      {
        idStatus: StatusPagamentoEnum.estorno,
        nomeStatus: 'Estorno',
        detalhe: '',
        cor: ''
      },
      {
        idStatus: StatusPagamentoEnum.semLancamento,
        nomeStatus: 'Sem Lançamento para o SAP',
        detalhe: '',
        cor: ''
      },
      {
        idStatus: StatusPagamentoEnum.aguardandoConfirmacaoPagamento,
        nomeStatus: 'Pedido SAP Recebido Fiscal - Aguardando Confirmação de Pagamento',
        detalhe: '',
        cor: '#19CE19'
      },
      {
        idStatus: StatusPagamentoEnum.semLancamentoHistorico,
        nomeStatus: 'Sem Lançamento para o SAP (Histórico)',
        detalhe: '',
        cor: ''
      },
      {
        idStatus: StatusPagamentoEnum.lancamentoControle,
        nomeStatus: 'Lançamento de Controle',
        detalhe: '',
        cor: ''
      },
      {
        idStatus: StatusPagamentoEnum.pedidoSapRetido,
        nomeStatus: 'Pedido SAP Retido - RJ',
        detalhe: '',
        cor: '#19A519'
      },
      {
        idStatus: StatusPagamentoEnum.loteAutomaticoCancelado,
        nomeStatus: 'Lote Automático Cancelado',
        detalhe: '',
        cor: '#FF0000'
      },
      {
        idStatus: StatusPagamentoEnum.lancamentoAutomaticoCancelado,
        nomeStatus: 'Lançamento Automático Cancelado',
        detalhe: '',
        cor: ''
      }];
    this.filtros = json;
    console.log(json);

    // tslint:disable-next-line: max-line-length
    return this.client.post<{data: IResultado[], total: number, totalLotes: number, totalValorLotes: number, quantidadesLancamentos: number}>(`${RESULTADO_ENDPOINT}`, json)
      .pipe(map(v => {
        this.filtros.total = 0;
        this.filtros.total = v.total;
        this.filtros.pagina++;

        this.infoTooltip = {
          quantidadesLancamentos: v.quantidadesLancamentos,
          totalLotes: v.total,
          totalValorLotes: v.totalValorLotes
        };

        return v.data.map(resultado => {
          const indexStatusPagamento = StatusPagamentos.findIndex(e => e.idStatus === resultado.codigoStatusPagamento);

          const valorCorStatusPagamento = StatusPagamentos[indexStatusPagamento].cor;
          const valorDetalheStatusPagamento = StatusPagamentos[indexStatusPagamento].detalhe;
          return {
            id: resultado.id,
            titulo: `Lote: ${resultado.id} - ${resultado.descricaoLote} - ${resultado.formaPagamento}`,
            subtitulo: resultado.nomeEmpresaGrupo,
            numeroLoteBB: resultado.numeroLoteBB,
            descricao: `Lote criado por ${resultado.nomeUsuario} em ${resultado.dataCriacao}`,
            estado: {
              id: resultado.codigoStatusPagamento,
              mensagem: resultado.statusPagamento,
              cor: valorCorStatusPagamento
            } as IColoredMessage,
            estadoDetalhado: resultado.numeroPedidoSAP ? `Realizado em ${valorDetalheStatusPagamento} ${resultado.dataCriacaoPedido}` : '',
            helperLabel: resultado.numeroPedidoSAP ? `Nº do Pedido SAP: ${resultado.numeroPedidoSAP}` : '',
            observacao: resultado.existeBordero ? 'Lançamento com borderô' : null,
            iconeObservacao: 'fa-exclamation-circle',
            isOpen: false
          } as ItemExpansivel;
        }) as ItemExpansivel[];
      }
      ));
  }
}
