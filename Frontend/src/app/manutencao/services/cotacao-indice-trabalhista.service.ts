import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpErrorResult } from '@core/http';
import { environment } from '@environment';
import { CotacaoIndiceTrabalhista } from '@manutencao/models/cotacao-indice-trabalhista.model';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { Page } from '@shared/types/page';
import { Sort } from '@shared/types/sort';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CotacaoIndiceTrabalhistaService {


  private readonly href: string = `${environment.api_url}/manutencao/cotacao-indice-trabalhista`;
  constructor(private http: HttpClient,private exportarService: TransferenciaArquivos) { }

  public async obterPaginado(
    dataCorrecao: string,
    dataBaseInicial: string,
    dataBaseFinal: string,
    pagina: number,
    quantidade: number,
    coluna: 'DataCorrecao' | 'DataBase' | 'ValorCotacao' | '',
    direcao: 'asc' | 'desc'
  ): Promise<{ data: Array<CotacaoIndiceTrabalhista>, total: number }> {
    try {
      const url: string = `${this.href}/?dataCorrecao=${dataCorrecao}&dataBaseInicial=${dataBaseInicial}&dataBaseFinal=${dataBaseFinal}&pagina=${pagina}&quantidade=${quantidade}&coluna=${coluna}&direcao=${direcao}`;
      return await this.http.get<{ data: Array<CotacaoIndiceTrabalhista>, total: number }>(url)
        .pipe(map(x => {
          return {
            total: x.total,
            data: x.data.map(e => CotacaoIndiceTrabalhista.fromObj(e))
          };
        }))
        .toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async obterPaginadoTemCotacaoTrab(
    
    pagina: number,
    quantidade: number,
    coluna: 'DataCorrecao' | 'DataBase' | 'ValorCotacao' | '',
    direcao: 'asc' | 'desc'
  ): Promise<{ data: CotacaoIndiceTrabalhista[], total: number }> {
    try {
      const url: string = `${this.href}/ObterPaginadoTemp/?pagina=${pagina}&quantidade=${quantidade}&coluna=${coluna}&direcao=${direcao}`;
      return await this.http.get<{ data: CotacaoIndiceTrabalhista[], total: number }>(url)
        .pipe(map(x => {
          return {
            total: x.total,
            data: x.data.map(e => CotacaoIndiceTrabalhista.fromObj(e))
          };
        }))
        .toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }
  
 
  public async excluir(dataCorrecao: Date, dataBase: Date) {
    try {

      const url: string = `${this.href}/${dataCorrecao.getFullYear() + "-" + (dataCorrecao.getMonth() + 1)}/${dataBase.getFullYear() + "-" + (dataBase.getMonth() + 1)}`;
      await this.http.delete(url).toPromise();
    } catch (error) {
      console.log(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  async AplicarImportacao(){
    try {
      let url = `${this.href}/AplicarImportacao`;      

      return await this.http.get(url).toPromise();
    } catch(error) {
      throw HttpErrorResult.fromError(error);
      
    } 
  }
   async criarImportacao(csv: File,dataCotacao: string): Promise<String>  {

    const form = new FormData();
    form.append('arquivo', csv);
     form.append(
      'dataCotacao',
      new Date(dataCotacao).toISOString()
    ); 
    try {
      let url = `${this.href}/upload`;  
      return await this.http.post<string>(url, form).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }
  async exportar(
    dataCorrecao: string,
    dataBaseInicial: string,
    dataBaseFinal :string,
    coluna: 'DataCorrecao' | 'DataBase' | 'ValorCotacao' | '',
    direcao:'asc' | 'desc'): Promise<void> {
    try {
      let url = `${this.href}/exportar/?dataCorrecao=${dataCorrecao}&dataBaseInicial=${dataBaseInicial}&dataBaseFinal=${dataBaseFinal}&coluna=${coluna}&direcao=${direcao}`;      
      const response: any = await this.http.get(url, this.exportarService.downloadOptions).toPromise();
      const file = response.body;
      const filename = this.exportarService.obterNomeArquivo(response);
      this.exportarService.download(file, filename);

    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }
  }
}
