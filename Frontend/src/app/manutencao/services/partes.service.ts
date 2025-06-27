import { TransferenciaArquivos } from '../../shared/services/transferencia-arquivos.service';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { map } from 'rxjs/operators';

import { environment } from '@environment';
import { HttpErrorResult } from '@core/http';

import { Parte, TipoParte } from '@manutencao/models';

@Injectable({
  providedIn: 'root'
})
export class PartesService {
  constructor(private http: HttpClient, private exportarService: TransferenciaArquivos) { }

  private urlBase: string = environment.api_url + '/manutencao/partes';

  public async obterPaginado(
    pagina: number, quantidade: number,
    coluna: 'nome' | 'tipoParte' | 'documento' | 'carteiraTrabalho' | '', direcao: 'asc' | 'desc',
    nome: string, documento: string, carteiraTrabalho: string, tipoParte: TipoParte,
  ): Promise<{ data: Parte[], total: number }> {
    try {
      // tslint:disable-next-line: max-line-length
      const url: string = `${this.urlBase}/?pagina=${pagina}&quantidade=${quantidade}&coluna=${coluna}&direcao=${direcao}&nome=${nome}&documento=${documento}&carteiraDeTrabalho=${carteiraTrabalho}&tipoParte=${tipoParte.valor}`;
      return await this.http.get<{ data: Parte[], total: number }>(url)
        .pipe(map(x => {
          return {
            total: x.total,
            data: x.data.map(e => Parte.fromJson(e))
          };
        }))
        .toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async criar(command: {
    nome: string,
    documento: string,
    pessoaJuridica: boolean,
    carteiraTrabalho: string,
    tipoParteId: string,
    estadoId: string,
    endereco: string,
    cep: number,
    cidade: string,
    bairro: string,
    enderecosAdicionais: string,
    telefone: string,
    celular: string,
    telefonesAdicionais: string,
    valorCartaFianca: number,
    dataCartaFianca: Date
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
    carteiraTrabalho: string,
    tipoParteId: string,
    estadoId: string,
    endereco: string,
    cep: number,
    cidade: string,
    bairro: string,
    enderecosAdicionais: string,
    telefone: string,
    celular: string,
    telefonesAdicionais: string,
    valorCartaFianca: number,
    dataCartaFianca: Date
  }): Promise<void> {
    try {
      await this.http.put<void>(`${this.urlBase}/`, command).toPromise();
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
    coluna: 'nome' | 'tipoParte' | 'documento' | 'carteiraTrabalho' | '', direcao: 'asc' | 'desc',
    nome: string, documento: string, carteiraTrabalho: string, tipoParte: TipoParte
  ): Promise<void> {
    try {
      const response: any = await this.http.get(
        `${this.urlBase}/exportar?coluna=${coluna}&direcao=${direcao}&nome=${nome}&documento=${documento}&carteiraDeTrabalho=${carteiraTrabalho}&tipoParte=${tipoParte.valor}`,
        this.exportarService.downloadOptions
      ).toPromise();
      const file = response.body;
      const filename = this.exportarService.obterNomeArquivo(response);
      this.exportarService.download(file, filename);
    } catch (error) {

    }
  }
}
