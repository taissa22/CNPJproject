import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { map } from 'rxjs/operators';

import { environment } from '@environment';
import { HttpErrorResult } from '@core/http';

import { Profissional } from '../models/profissional.model';

@Injectable({
  providedIn: 'root'
})
export class ProfissionaisService {
  constructor(private http: HttpClient, private exportarService: TransferenciaArquivos) { }

  private urlBase: string = environment.api_url + '/manutencao/profissionais';

  public async obterPaginado(
    pagina: number, quantidade: number,
    coluna: 'nome' | 'documento' | 'estado' | 'advogadoDoAutor' | 'registroOAB' | 'estadoOAB', direcao: 'asc' | 'desc',
    nome: string, documento: string, tipoPessoa: 'F' | 'J' | '', advogadoDoAutor: true | false | ''
  ): Promise<{ data: Profissional[], total: number }> {
    try {
      // tslint:disable-next-line: max-line-length
      const url: string = `${this.urlBase}/?pagina=${pagina}&quantidade=${quantidade}&coluna=${coluna}&direcao=${direcao}&nome=${nome}&documento=${documento}&tipoPessoa=${tipoPessoa}&advogadoDoAutor=${advogadoDoAutor}`;
      return await this.http.get<{ data: Profissional[], total: number }>(url)
        .pipe(map(x => {
          return {
            total: x.total,
            data: x.data.map(p => Profissional.fromJson(p))
          };
        }))
        .toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async existe(command: {nome: string, id?: number}): Promise<boolean> {
    try {
      const url: string = `${this.urlBase}/existe`;
      // if (id) {
      //   url += `/?id=${id}`;
      // }
      return await this.http.post<boolean>(url, command).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async criar(command: {
    nome: string,
    documento: string,
    pessoaJuridica: boolean,
    email: string,
    estado: string,
    endereco: string,
    cep: number,
    cidade: string,
    bairro: string,
    enderecosAdicionais: string,
    telefone: string,
    fax: string,
    celular: string,
    telefonesAdicionais: string,
    advogado: boolean,
    numeroOab: string,
    estadoOab: string,
    contador: boolean,
    contadorPex: boolean
  }): Promise<void> {
    try {
      await this.http.post<void>(this.urlBase, command).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async atualizar(command: {
    id: number,
    nome: string,
    documento: string,
    pessoaJuridica: boolean,
    email: string,
    estado: string,
    endereco: string,
    cep: number,
    cidade: string,
    bairro: string,
    enderecosAdicionais: string,
    telefone: string,
    fax: string,
    celular: string,
    telefonesAdicionais: string,
    advogado: boolean,
    numeroOab: string,
    estadoOab: string,
    contador: boolean,
    contadorPex: boolean
  }): Promise<void> {
    try {
      await this.http.put<void>(this.urlBase, command).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async remover(id: number): Promise<void> {
    try {
      await this.http.delete<void>(`${this.urlBase}/${id}`).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async exportar(
    coluna: 'nome' | 'documento' | 'estado' | 'advogadoDoAutor' | 'registroOAB' | 'estadoOAB', direcao: 'asc' | 'desc',
    nome: string, documento: string, tipoPessoa: 'F' | 'J' | '', advogadoDoAutor: true | false | ''
  ): Promise<void> {
    try {
      const response: any = await this.http.get(`${this.urlBase}/exportar/?coluna=${coluna}&direcao=${direcao}&nome=${nome}&documento=${documento}&tipoPessoa=${tipoPessoa}&advogadoDoAutor=${advogadoDoAutor}`, this.exportarService.downloadOptions).toPromise();
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
