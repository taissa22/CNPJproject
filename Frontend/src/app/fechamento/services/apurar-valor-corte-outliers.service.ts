import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { environment } from 'src/environments/environment';
import { BehaviorSubject, Observable } from 'rxjs';
import { AgendarApuracaoOutliersModel } from '../models/contigencia/apurar-valor-corte-outliers/agendar-apuracao-outliers';
import { OrdenacaoPaginacaoDTO } from '@shared/interfaces/ordenacao-paginacao-dto';
import { map, take, tap } from 'rxjs/operators';
import { DownloadService } from '@core/services/sap/download.service';

@Injectable({
  providedIn: 'root'
})
export class ApurarValorCorteOutliersService {

  backEndURL: any;

  constructor(
    protected http: HttpClient,
    private downloadService: DownloadService,
  ) { }

  paginacaoVerMais = new BehaviorSubject<OrdenacaoPaginacaoDTO>({
    pagina: 1,
    quantidade: 5,
    total: 0
  });

  consultarMais() {
    this.paginacaoVerMais.next({
      ...this.paginacaoVerMais.value,
      pagina: this.paginacaoVerMais.value.pagina + 1,
      total: 0
    });
    return this.consultar().pipe(take(1));
  }

  consultar() {
    return this.listarApuracoesAgendadasExecutadas(
      this.paginacaoVerMais.value['pagina'], 5
    ).pipe(
      tap(
        res => this.paginacaoVerMais.next({
          ...this.paginacaoVerMais.value,
          total: res['total']
        })
      ),
      map(res => res['data'])
    );
  }

  limpar() {
    this.paginacaoVerMais.next({
      pagina: 1,
      quantidade: 5,
      total: 0,
    });
  }

  listarApuracoesAgendadasExecutadas(pag, qtd): Observable<any> {
    const backEndURL = environment.api_url + '/ApuracaoOutliers/CarregarAgendamento';

    let params = new HttpParams().set('pagina', pag);
    params = params.set('qtd', qtd);
    return this.http.get<any>(backEndURL, { params: params });
  }

  listarBaseFechamento(): Observable<any> {
    let backEndURL = environment.api_url + '/ApuracaoOutliers/CarregarFechamentosDisponiveisParaAgendamento';
    return this.http.get<any>(backEndURL);
  }

  agendarApuracaoOutliers(model: AgendarApuracaoOutliersModel): Observable<any> {
    let backEndURL = environment.api_url + '/ApuracaoOutliers/AgendarApuracaoOutliers';
    return this.http.post<any>(backEndURL, model);
  }

  removerApuracoesAgendadas(id): Observable<any> {
    const backEndURL = environment.api_url + '/ApuracaoOutliers/Delete';

    let params = new HttpParams().set('id', id);
    return this.http.delete(backEndURL, { params: params });
  }

  download(nomeArquivo): Observable<any> {
    let params = new HttpParams().set('nomeArquivo', nomeArquivo);
    return this.http.get(environment.api_url + '/ApuracaoOutliers/DownloadApuracaoOutliers', { params: params, responseType: 'blob' });
  }

  downloadArquivos(nomeArquivo) {
    return this.download(nomeArquivo)
      .pipe(take(1))
      .subscribe(res => {
        this.downloadService.prepararDownload(res, nomeArquivo);
      });
  }
}
