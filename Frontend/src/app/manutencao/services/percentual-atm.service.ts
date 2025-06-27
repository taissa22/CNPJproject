import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { HttpErrorResult } from '@core/http/http-error-result';
import { map } from 'rxjs/operators';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { PercentualAtmDTO } from '@shared/interfaces/percentual-atm-dto';

@Injectable({
  providedIn: 'root'
})
export class PercentualAtmService {
  private endPoint: string = 'percentual-atm';
  dataInicio: Date;

  constructor(
    private http: HttpClient,
    private exportarService: TransferenciaArquivos
  ) {}

  protected url(parametros: string = ''): string {
    return `${environment.api_url}/manutencao/${this.endPoint}/${parametros}`;
  }

  public async obterPaginado(
    pagina: number, quantidade: number, dataVigencia: string, codTipoProcesso: number,
    coluna: 'uf' | 'percentual' | '' = 'uf', direcao: 'asc' | 'desc' = 'asc'
  ): Promise<{ data: Array<PercentualAtmDTO>, total: number }> {
    try {
      const url: string = this.url(`?pagina=${pagina}&quantidade=${quantidade}&dataVigencia=${dataVigencia}&codTipoProcesso=${codTipoProcesso}&coluna=${coluna}&direcao=${direcao}`);
      return await this.http.get<{ data: Array<PercentualAtmDTO>, total: number }>(url)
        .pipe(map(x => {
          return {
            total: x.total,
            data: x.data.map(o => PercentualAtmDTO.fromJson(o))
          };
        }))
        .toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async exportar(coluna: 'uf' | 'percentual' | '' = 'uf', dataVigencia: string, codTipoProcesso: number, nomeTipoProcesso: string, direcao: 'asc' | 'desc'): Promise<void> {

    try {
      const response: any = await this.http.get(this.url(`exportar/?dataVigencia=${dataVigencia}&codTipoProcesso=${codTipoProcesso}&nomeTipoProcesso=${nomeTipoProcesso}&coluna=${coluna}&direcao=${direcao}`), this.exportarService.downloadOptions).toPromise();
      const file = response.body;
      const filename = this.exportarService.obterNomeArquivo(response);
      this.exportarService.download(file, filename);
    } catch (error) {
      const httpError: HttpErrorResponse = error;
      if (Array.isArray(httpError.error)) {
        throw new HttpErrorResult(httpError.error);
      } else {
        throw new HttpErrorResult(['Erro desconhecido']);
      }
    }
  }


  async criar(csv: File, vigencia: string, codTipoProcesso: number): Promise<void> {

    const form = new FormData();
    form.append('arquivo', csv);
    form.append(
      'dataVigencia',
      new Date(vigencia + 'T00:00:00-05:00').toISOString()
    );
    try {
      await this.http.post<void>(this.url(`upload/?codTipoProcesso=${codTipoProcesso}`), form, {}).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async ObterComboVigencias(codTipoProcesso: number): Promise<Array<Date>> {
    try {
      const url: string = this.url(`ObterComboVigencias?codTipoProcesso=${codTipoProcesso}`);
      return await this.http.get<Array<Date>>(url).pipe(map(x => x.map(y => y = new Date(y))))
        .toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }
}
