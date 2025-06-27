import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpErrorResult } from '@core/http';
import { environment } from '@environment';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AgendamentoLogProcessoModel } from '../models/AgendamentoLogProcesso.model';

@Injectable({
  providedIn: 'root'
})
export class AgendamentoLogProcessoService {
  private endPoint: string = 'AgendamentoLogProcesso';

  constructor(
    private http: HttpClient,
    private exportarService: TransferenciaArquivos
  ) { }

  protected url(parametros: string = ''): string {
    return `${environment.api_v2_url}/${this.endPoint}/${parametros}`;
  }

  public obterPaginado(
    pagina: number,
    quantidade: number,
    dataIni: string,
    dataFim: string
  ): Observable<{ data: Array<AgendamentoLogProcessoModel>, total: number }> {
    return this.http.get<{ data: Array<AgendamentoLogProcessoModel>, total: number }>(this.url(`Listar?pagina=${pagina}&quantidade=${quantidade}&dataIni=${dataIni}&dataFim=${dataFim}`))
      .pipe(
        map(x => {
          return {
            total: x.total,
            data: x.data.map(obj => AgendamentoLogProcessoModel.fromObj(obj))
          };
        })
      );
  }

  public async criar(obj): Promise<any> {
      await this.http
        .post<any>(this.url("criar"), obj )
        .toPromise();
  }

  public async deletar(id): Promise<any> {
    try {
      await this.http
        .get<any>(this.url("Deletar?id="+id))
        .toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error)
    }
  }

  async BaixarArquivo(id : number): Promise<void> {
    try {
      const response: any = await this.http.get(this.url("BaixarArquivo?id="+id), this.exportarService.downloadOptions).toPromise();
      const file = response.body;
      const filename = this.exportarService.obterNomeArquivo(response);
      this.exportarService.download(file, filename);
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async BaixarArquivoModelo(): Promise<void> {
    try {
      const response: any = await this.http.get(this.url("BaixarArquivoModelo"), this.exportarService.downloadOptions).toPromise();
      const file = response.body;
      const filename = this.exportarService.obterNomeArquivo(response);
      this.exportarService.download(file, filename);
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async BaixarArquivoZip(id: number): Promise<void> {
    try {
      const response: any = await this.http.get(this.url("BaixarArquivoZip?id="+id), this.exportarService.downloadOptions).toPromise();
      const file = response.body;
      const filename = this.exportarService.obterNomeArquivo(response);
      this.exportarService.download(file, filename);
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async obterTamanhoMaximoArquivoAnexo(): Promise<any> {
    try {
      return await this.http
      .get<number>(this.url("ParametroJuridico"))
      .toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error)
    }
  }
}
