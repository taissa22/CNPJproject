import { Injectable } from '@angular/core';
import { ApiService } from 'src/app/core';
import { Observable } from 'rxjs';
import { HistoricoModel } from 'src/app/core/models/historico.model';
import { pluck } from 'rxjs/operators';


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
