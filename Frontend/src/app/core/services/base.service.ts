import { Injectable } from '@angular/core';
import { ApiService } from '.';
import { IBaseService } from '@shared/interfaces/ibase-service';
import { Observable, of } from 'rxjs';
import { DownloadService } from './sap/download.service';
import { concatenarNomeExportacao } from '@shared/utils';

@Injectable({
  providedIn: 'root'
})
export class BaseService<TModel, TType> implements IBaseService<TModel, TType> {

  public endpoint: string;

  constructor(protected http: ApiService,
              protected downloadService: DownloadService) { }

  excluir(id: TType): Observable<any> {
    return this.http.get(`/${this.endpoint}/Excluir${this.endpoint}?codigo=${id}`);
  }

  cadastrar(model: TModel): Observable<any> {
    return this.http.post(`/${this.endpoint}/Salvar${this.endpoint}`, model);
  }

  editar(model: TModel): Observable<any> {
    return this.http.post(`/${this.endpoint}/Editar${this.endpoint}`, model);
  }

  exportar(nomeArquivo: string): Promise<any> {
    return this.http.get(`/${this.endpoint}/Exportar${this.endpoint}`)
                    .toPromise()
                    .then(resultado => {
                      const buffer = this.downloadService.converterBase64ParaBuffer(resultado['data']);
                      this.downloadService.prepararDownload(buffer, concatenarNomeExportacao(nomeArquivo));
                    });
  }

  exportarPorFiltro(nomeArquivo: string, json): Promise<any> {
    return this.http.post(`/${this.endpoint}/Exportar${this.endpoint}`, json)
                    .toPromise()
                    .then(resultado => {
                      const buffer = this.downloadService.converterBase64ParaBuffer(resultado.data);
                      this.downloadService.prepararDownload(buffer, concatenarNomeExportacao(nomeArquivo));
                    });
  }

  consultar(id: TType): Observable<any> {
    return this.http.get(`/${this.endpoint}/Consultar${this.endpoint}?codigo=${id}`);
  }

  consultarPorFiltros(filtros: any): Observable<any> {
    return this.http.post(`/${this.endpoint}/Consultar${this.endpoint}`, filtros);

  }

  consultarTodos(): Observable<any> {
    return this.http.get(`/${this.endpoint}/ConsultarTodos`);
  }
}
