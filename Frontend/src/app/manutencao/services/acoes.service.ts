import { NumberComponent } from './../../componentes/number/number.component';
import { TransferenciaArquivos } from '../../shared/services/transferencia-arquivos.service';
import { Acao } from '@manutencao/models/acao.model';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from '@angular/common/http';
import { HttpErrorResult } from '@core/http/http-error-result';
import { map } from 'rxjs/operators';
import { StringMap } from '@angular/compiler/src/compiler_facade_interface';

@Injectable({
  providedIn: 'root'
})
export class AcoesService {

  private endPoint: string = 'acoes';

  constructor(private api: HttpClient, private exportarService: TransferenciaArquivos) { }

  protected url(parametros: string): string {
    return `${environment.api_url}/manutencao/${this.endPoint}/${parametros}`;
  }

  async obterPaginado(
    pesquisa : string,
    tipoProcesso: string,
    pagina: number,
    quantidade: number,
    coluna: string,
    direcao: 'asc' | 'desc'
  ): Promise<{ data: Array<Acao>, total: number }> {
    try {
      let url;
      if (pesquisa){
         url = this.url(`${tipoProcesso}/?pesquisa=${pesquisa}&pagina=${pagina}&quantidade=${quantidade}&coluna=${coluna}&direcao=${direcao}`);
      }
      else{
        url = this.url(`${tipoProcesso}/?pagina=${pagina}&quantidade=${quantidade}&coluna=${coluna}&direcao=${direcao}`);
      }
      return await this.api.get<{ data: Array<Acao>, total: number }>(url)
      .pipe(map(x => {
        return {
          total: x.total,
          data: x.data.map(a => Acao.fromJson(a))
        };
      })).toPromise();

    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }

  }

  async criar(tipoProcesso: string, command: {
    idEstrategico?: Number,
    idConsumidor?: Number,
    descricao: string,
    naturezaAcaoBB?: number,
    ativo?: boolean,
    enviarAppPreposto? : boolean
  }): Promise<void> {
    try {
      await this.api.post<void>(this.url(tipoProcesso), {
        idEstrategico : command.idEstrategico,
        idConsumidor: command.idConsumidor,
        descricao: command.descricao,
        naturezaAcaoBBId : command.naturezaAcaoBB,
        ativo: command.ativo,
        enviarAppPreposto : command.enviarAppPreposto
      }).toPromise();
    } catch (error) {
      const httpError: HttpErrorResponse = error;
      if (Array.isArray(httpError.error)) {
        throw new HttpErrorResult(httpError.error);
      } else {
        throw new HttpErrorResult(['Erro desconhecido']);
      }
    }
  }

  async atualizar(tipoProcesso: string, command: {
    id: number, descricao: string,
    idEstrategico?: Number,
    idConsumidor?: number,
    naturezaAcaoBB?: number,
    ativo?: boolean,
    enviarAppPreposto? : boolean
  }): Promise<void> {
    try {
      await this.api.put<void>(this.url(tipoProcesso), {
        id: command.id,
        idEstrategico : command.idEstrategico,
        idConsumidor: command.idConsumidor,
        descricao: command.descricao,
        naturezaAcaoBBId: command.naturezaAcaoBB,
        ativo: command.ativo,
        enviarAppPreposto: command.enviarAppPreposto
      }).toPromise();
    } catch (error) {
      const httpError: HttpErrorResponse = error;
      if (Array.isArray(httpError.error)) {
        throw new HttpErrorResult(httpError.error);
      } else {
        throw new HttpErrorResult(['Erro desconhecido']);
      }
    }
  }

  async remover(tipoProcesso: string, codigo: number): Promise<void> {
    try {
      await this.api.delete<void>(this.url(`${tipoProcesso}/${codigo}`)).toPromise();
    } catch (error) {
      const httpError: HttpErrorResponse = error;
      if (Array.isArray(httpError.error)) {
        throw new HttpErrorResult(httpError.error);
      } else {
        throw new HttpErrorResult(['Erro desconhecido']);
      }
    }
  }

  async exportar(pesquisa : string,tipoProcesso: string, coluna: string, direcao: 'asc' | 'desc'): Promise<void> {
    try {
      let url ="";
      if (pesquisa){
        url = `${tipoProcesso}/exportar/?pesquisa=${pesquisa}&coluna=${coluna}&direcao=${direcao}`;
      }
      else
      {
        url = `${tipoProcesso}/exportar/?coluna=${coluna}&direcao=${direcao}`;
      }
      const response: any = await this.api.get(this.url(url), this.exportarService.downloadOptions).toPromise();
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

  async ObterDescricaoDeParaCivelEstrategico(): Promise<Array<Acao>> {
    try {
      return this.api
        .get<Array<Acao>>(`${this.url("civel-estrategico/ObterDescricaoDeParaCivelEstrategico")}` )
        .toPromise();
    } catch (error) {
      const httpError: HttpErrorResponse = error;
      if (Array.isArray(httpError.error)) {
        throw new HttpErrorResult(httpError.error);
      } else {
        throw new HttpErrorResult(['Erro desconhecido']);
      }
    }
  }

  async ObterDescricaoDeParaConsumidor(): Promise<Array<Acao>> {
    try {
      return this.api
        .get<Array<Acao>>(`${this.url("civel-consumidor/ObterDescricaoDeParaConsumidor")}` )
        .toPromise();
    } catch (error) {
      const httpError: HttpErrorResponse = error;
      if (Array.isArray(httpError.error)) {
        throw new HttpErrorResult(httpError.error);
      } else {
        throw new HttpErrorResult(['Erro desconhecido']);
      }
    }
  }

}
