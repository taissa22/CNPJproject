import { Injectable } from '@angular/core';
import { HistoricoModel } from '../../models/historico.model';
import { pluck } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { ApiService } from '..';

@Injectable({
  providedIn: 'root'
})
export class HistoricoService {
  public colunasGrid: any[];
  public tituloDetalhamento: string;


  constructor(private apiService: ApiService) {
    this.tituloDetalhamento = 'HISTÓRICO DO STATUS DE PAGAMENTO';
    this.colunasGrid = [
      'Data',
      'Status de Pagamento',
      'Gerado Por...'


    ];

  }


  getHistorico(numeroLote: any): Observable<HistoricoModel[]> {
    return this.apiService.get('/LogLoteProcesso/Obterhistorico?CodigoLote=' + numeroLote).pipe(pluck('data'));
  }

}
