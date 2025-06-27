import { Injectable } from '@angular/core';
import { ApiService } from '..';
import { TipoProcessoService } from './tipo-processo.service';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';
import { Observable } from 'rxjs';
import { LancamentoModel } from '../../models/lancamento.model';
import { pluck } from 'rxjs/operators';
import { FilterService } from 'src/app/sap/consultaLote/services/filter.service';


@Injectable({
  providedIn: 'root'
})
export class LancamentoService {

  public colunasGrid: any[];
  public tituloDetalhamento: string;
  public colunaPadrao: any[];

  constructor(private apiService: ApiService,
    private tipoProcesso: TipoProcessoService,
              private filterService: FilterService

  ) {
    this.tituloDetalhamento = 'LANÇAMENTOS';

    this.colunaPadrao = [
      'Nº Processo',
      'Comarca',
      'Juizado',
      'Data Envio Escritório',
      'Escritório',
      'Tipo de Lançamento',
      'Categoria de Pagamento',
      'Status de Pagamento',
      'Data lançamento',
      'Nº Guia',
      'Nº Pedido SAP',
      'Data Recebimento Fiscal',
      'Data Pagto Pedido',
      'Valor Liquido',
      'Texto SAP',
      'Comentário',
      'Autor',
      'Nº Conta Judicial',
      'Nº Parcela Judicial',
      'Autenticação Eletrônica',
      'Status Parcela BB',
      'Data Efetivação Parcela BB'
    ];
  }

  getColulasLancamento() {

    this.colunasGrid = [];

    if (this.filterService.tipoProcessoTracker.value === TipoProcessoEnum.civelConsumidor) {
      this.colunasGrid = [
        'Nº Processo',
        'Comarca',
        'Vara',
        'Data Envio Escritório',
        'Escritório',
        'Tipo de Lançamento',
        'Categoria de Pagamento',
        'Status Pagamento',
        'Data Lançamento',
        'Nº Pedido SAP',
        'Data Recebimento Fisical',
        'Data Pagto Pedido',
        'Valor Líquido',
        'Texto SAP',
        'Comentário',
        'Autor',
        'Nº Conta Judicial',
        'Nº Parcela Judicial',
        'Autenticação Eletrônica',
        'Status Parcela BB',
        'Data Efetivação Parcela BB'
      ];
    } else if (this.filterService.tipoProcessoTracker.value === TipoProcessoEnum.trabalhista) {
      this.colunasGrid = [
        'Nº Processo',
        'Comarca',
        'Vara',
        'Data Envio Escritório',
        'Escritório',
        'Tipo de Lançamento',
        'Categoria de Pagamento',
        'Status Pagamento',
        'Data Lançamento',
        'Nº guia',
        'Nº do Pedido SAP',
        'Data Recebimento Fisical',
        'Data Pagto Pedido',
        'Valor Líquido',
        'Texto SAP',
        'Comentário'
      ];
    } else if (this.filterService.tipoProcessoTracker.value === TipoProcessoEnum.juizadoEspecial) {
      this.colunasGrid = [
        'Nº Processo',
        'Comarca',
        'Juizado',
        'Data Envio Escritório',
        'Escritório',
        'Tipo de Lançamento',
        'Categoria de Pagamento',
        'Status de Pagamento',
        'Data lançamento',
        'Nº Guia',
        'Nº Pedido SAP',
        'Data Recebimento Fiscal',
        'Data Pagto Pedido',
        'Valor Liquido',
        'Texto SAP',
        'Comentário',
        'Autor',
        'Nº Conta Judicial',
        'Nº Parcela Judicial',
        'Autenticação Eletrônica',
        'Status Parcela BB',
        'Data Efetivação Parcela BB'
      ];
    } else if (this.filterService.tipoProcessoTracker.value === TipoProcessoEnum.civelEstrategico) {
      this.colunasGrid = [
        'Nº Processo',
        'Comarca',
        'Vara',
        'Data Envio Escritório',
        'Escritório',
        'Tipo de Lançamento',
        'Categoria de Pagamento',
        'Status de Pagamento',
        'Data lançamento',
        'Nº Pedido SAP',
        'Data Recebimento Fiscal',
        'Data Pagto Pedido',
        'Valor Liquido',
        'Texto SAP',
        'Comentário'
      ];
    } else if (this.filterService.tipoProcessoTracker.value === TipoProcessoEnum.PEX) {
      this.colunasGrid = [
        'Nº Processo',
        'Comarca',
        'Vara/Juizado',
        'Data Envio Escritório',
        'Escritório',
        'Tipo de Lançamento',
        'Categoria de Pagamento',
        'Status de Pagamento',
        'Data lançamento',
        'Nº Guia',
        'Nº Pedido SAP',
        'Data Recebimento Fiscal',
        'Data Pagto Pedido',
        'Valor Liquido',
        'Texto SAP',
        'Comentário',
        'Autor',
        'Nº Conta Judicial',
        'Nº Parcela Judicial',
        'Autenticação Eletrônica',
        'Status Parcela BB',
        'Data Efetivação Parcela BB'
      ];
    }

    return this.colunasGrid;
  }


  getTipoProcesso(): number {
    this.filterService.tipoProcessoTracker.next(1);
    return this.filterService.tipoProcessoTracker.value;
  }

  getLancamento(numeroLote: any, tipoProcesso: any): Observable<LancamentoModel[]> {
    return this.apiService.get('/LancamentoProcesso/ObterLancamentoDoLote?CodigoLote=' + numeroLote
    + '&CodigoTipoProcesso=' + tipoProcesso ).pipe(pluck('data'));
  }

  public alterarDataEnvioEscritorio(json): Observable<any> {
    return this.apiService
      .post(`/LancamentoProcesso/AlterarDataEnvioEscritorio`, json);
  }



}
