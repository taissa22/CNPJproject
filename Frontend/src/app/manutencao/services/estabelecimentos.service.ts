import { queryStringGenerator } from '@shared/helpers/url-helper';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Estabelecimento } from '@manutencao/models/estabelecimento.model';
import { HttpErrorResult } from '@core/http/http-error-result';
import { map } from 'rxjs/operators';
import { TransferenciaArquivos } from '../../shared/services/transferencia-arquivos.service';

@Injectable({
  providedIn: 'root'
})
export class EstabelecimentosService {

  protected endPoint: string = 'estabelecimentos';

  constructor(private api: HttpClient, private exportarService: TransferenciaArquivos) { }

  private url(paramentros: string = ''): string {
    return `${environment.api_url}/manutencao/${this.endPoint}/${paramentros}`;
  }

  async obterPaginado(
    pagina: number,
    quantidade: number,
    coluna: string,
    direcao: 'asc' | 'desc',
    parteNome: string = null
  ): Promise<{ data: Array<Estabelecimento>, total: number }> {
    try {
      let url = "";
    if (parteNome)
    {
       url = this.url(`?pagina=${pagina}&quantidade=${quantidade}&coluna=${coluna}&direcao=${direcao}&nome=${parteNome}`);      
    }
    else{
      url = this.url(`?pagina=${pagina}&quantidade=${quantidade}&coluna=${coluna}&direcao=${direcao}`);      
    }
      
      return await this.api
        .get<{ data: Array<Estabelecimento>, total: number }>(url)
        .pipe(map(x => {
          return {
            total: x.total,
            data: x.data.map(e => Estabelecimento.fromJson(e))
          };
        })).toPromise();

    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }
  }


  async criar(command: {
    cnpj: string,
    nome: string,
    endereco: string,
    bairro: string,
    cidade: string,
    estado: number,
    cep: string,
    telefone?: string,
    celular?: string
  }): Promise<void> {
    try {
      await this.api.post<void>(this.url(), {
        cnpj: command.cnpj,
        nome: command.nome,
        endereco: command.endereco,
        bairro: command.bairro,
        cidade: command.cidade,
        estado: command.estado,
        cep: command.cep,
        telefone: command.telefone,
        celular: command.celular
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

  async atualizar(command: {
    id: number,
    cnpj: string,
    nome: string,
    endereco: string,
    bairro: string,
    cidade: string,
    estado: number,
    cep: string,
    telefone?: string,
    celular?: string
  }): Promise<void> {
    try {
      await this.api.put<void>(this.url(), {
        id: command.id,
        cnpj: command.cnpj,
        nome: command.nome,
        endereco: command.endereco,
        bairro: command.bairro,
        cidade: command.cidade,
        estado: command.estado,
        cep: command.cep,
        telefone: command.telefone,
        celular: command.celular
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

  async remover(codigo: number): Promise<void>  {
    try {
      await this.api.delete<void>(this.url(`${codigo}`)).toPromise();
    } catch (error) {
      const httpError: HttpErrorResponse = error;
      if (Array.isArray(httpError.error)) {
        throw new HttpErrorResult(httpError.error);
      } else {
        throw new HttpErrorResult(['Erro desconhecido']);
      }
    }
  }

  async exportar(
    nome : string,
    coluna: string, 
    direcao: string): Promise<void> {
    try {
      
      let url = "";
      if (nome){
        url = `exportar?nome=${nome}&coluna=${coluna}&direcao=${direcao}`;
      }
      else{
        url = `exportar?coluna=${coluna}&direcao=${direcao}`
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

}
