import { TransferenciaArquivos } from '../../shared/services/transferencia-arquivos.service';
import { Competencia } from '../models/competencias.model';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { map } from 'rxjs/operators';

import { environment } from '@environment';
import { HttpErrorResult } from '@core/http';
import { OrientacaoJuridicaTrabalhista } from '@manutencao/models/orientacao-juridica-trabalhista';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrientacaoJuridicaService {
  private endPoint: string = 'orientacao-juridica';

  constructor(private http: HttpClient, private exportarService: TransferenciaArquivos) { }

  protected url(parametros: string = ''): string {
    return `${environment.api_url}/manutencao/${this.endPoint}/${parametros}`;
  }

  public obterPaginado(    
    pagina: number, quantidade: number, obterTrabalhista: boolean,
    coluna: 'nome' | 'descricao' | 'ativo' | 'codigo' | '', direcao: 'asc' | 'desc',
    pesquisa?: string 
  ):Observable<{total:number,data:Array<OrientacaoJuridicaTrabalhista>}>{
      let link : string = this.url(`?pagina=${pagina}&quantidade=${quantidade}&obterTrabalhista=${true}&coluna=${coluna}&direcao=${direcao}`);      
      
      if (pesquisa) {
        link = `${link}&pesquisa=${pesquisa}`
      }
      console.log('pesquisa' + pesquisa);
      return this.http.get<{ data: OrientacaoJuridicaTrabalhista[], total: number }>(link)
        .pipe(
          map(x => {
            return {
              total: x.total,
              data: x.data.map(e => OrientacaoJuridicaTrabalhista.fromObj(e))
            };
          })
        );
  }

  async incluir(   
    codTipoOrientacaoJuridica: number,
    nome: string,
    descricao: string,   
    palavraChave: string,
    ehTrabalhista : boolean,
    ativo :boolean
  ): Promise<void> {
    const obj = {      
      codTipoOrientacaoJuridica: codTipoOrientacaoJuridica,
      nome: nome,
      descricao: descricao,
      palavraChave: palavraChave,
      ehTrabalhista: ehTrabalhista,
      ativo: ativo
    };
    try {
      console.log(obj);
      await this.http.post(this.url(), obj).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async alterar(
    codOrientacaoJuridica: number,
    codTipoOrientacaoJuridica: number,
    nome: string,
    descricao: string,   
    palavraChave: string,
    ehTrabalhista : boolean,
    ativo :boolean
  ): Promise<void> {
    const obj = {
      codOrientacaoJuridica: codOrientacaoJuridica,
      codTipoOrientacaoJuridica: codTipoOrientacaoJuridica,
      nome: nome,
      descricao: descricao,
      palavraChave: palavraChave,
      ehTrabalhista: ehTrabalhista,
      ativo: ativo
    };
    try {
      console.log(obj);
      
      await this.http.put(this.url(), obj).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }
 

  public async excluir(id: number): Promise<void> {
    try {
      await this.http.delete<void>(this.url(`${id}`)).toPromise();
    } catch (error) {
      console.log(error);
      throw HttpErrorResult.fromError(error);
    }
  }


  async exportar(
          obterTrabalhista: boolean, coluna: 'nome' | 'descricao' | 'ativo' | 'codigo' | '',
          direcao: 'asc' | 'desc',
          pesquisa? : string): Promise<void> {
    try {
      let url = this.url(`exportar/?obterTrabalhista=${obterTrabalhista}&coluna=${coluna}&direcao=${direcao}`);
      if (pesquisa != null) {
        url += `&pesquisa=${pesquisa}`;
      }
      const response: any = await this.http.get(url, this.exportarService.downloadOptions).toPromise();
      const file = response.body;
      const filename = this.exportarService.obterNomeArquivo(response);
      this.exportarService.download(file, filename);
    } catch (error) {
      console.log(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  
}

