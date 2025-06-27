import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpErrorResult } from '@core/http';
import { environment } from '@environment';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { map } from 'rxjs/operators';
import { DistribuicaoModel } from '../models/parametrizar-distribuicao-processos/distribuicao.model';
import { EscritorioDistribuicaoModel } from '../models/parametrizar-distribuicao-processos/escritorio-distribuicao.model';
import { ListarAnexosResponse } from '../models/parametrizar-distribuicao-processos/listar-anexos-response';

@Injectable({
  providedIn: 'root'
})
export class ParametrizarDistribuicaoProcessosService {

  constructor(
    private http: HttpClient,
    private exportarService: TransferenciaArquivos
  ) { }

  private baseUrl = environment.api_v2_url + '/parametrizacao-distribuicao-escritorio'

  async obterDistribuicao(CodEstado: string, CodComarca: number, CodigosVara: string, CodTipoProcesso: number, CodEmpresaCentralizadora: number, CodProfissional: number, ordem?: string, asc?: boolean, page?: number, size?: number): Promise<{ lista: DistribuicaoModel[], total: number }> {
    try {
      let newUrl = `${this.baseUrl}/obter-parametrizacao?CodEstado=${CodEstado}&CodComarca=${CodComarca}&Codigos=${CodigosVara}&CodTipoProcesso=${CodTipoProcesso}&CodEmpresaCentralizadora=${CodEmpresaCentralizadora}&CodProfissional=${CodProfissional}`

      if (ordem) newUrl += `&ordem=${ordem}&asc=${asc}`

      newUrl += `&page=${page}&size=${size}`

      return await this.http.get<{ lista: DistribuicaoModel[], total: number }>(newUrl)
        .pipe(map(x => {
          return {
            total: x.total,
            lista: x.lista
          };
        }))
        .toPromise();
    }
    catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async obterEscritorioDistribuicao(CodParametrizacao: string, NomeEscritorio?: string, ordem?: string, asc?: boolean, page?: number, size?: number): Promise<{ lista: EscritorioDistribuicaoModel[], total: number }> {
    try {
      let newUrl = `${this.baseUrl}/obter-parametrizacao-escritorio?CodParametrizacao=${CodParametrizacao}&NomeEscritorio=${NomeEscritorio}`

      if (ordem) newUrl += `&ordem=${ordem}&asc=${asc}`

      if (page) newUrl += `&page=${page}&size=${size}`

      return await this.http.get<{ lista: EscritorioDistribuicaoModel[], total: number }>(newUrl)
        .pipe(map(x => {
          return {
            total: x.total,
            lista: x.lista
          };
        }))
        .toPromise();
    }
    catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async excluirDistribuicao(id: number): Promise<any> {
    let newUrl = this.baseUrl + `/excluir-parametrizacao?CodParamDistribuicao=${id}`
    return await this.http.delete<any>(newUrl).toPromise().catch(error => {
      return Promise.reject(error);
    })
  }

  async validarParametrizacao(parametrizacao: any): Promise<boolean> {
    let url = `${this.baseUrl}/validar-parametrizacao`
    return await this.http.post<boolean>(url, parametrizacao).toPromise();
  }

  async validarEscritorio(escritorio: any): Promise<boolean> {
    let url = `${this.baseUrl}/validar-escritorio`
    return await this.http.post<boolean>(url, escritorio).toPromise();
  }

  async salvarDistribuicao(distribuicao: any): Promise<any> {
    let url = `${this.baseUrl}/salvar-parametrizacao`
    return await this.http.post<any>(url, distribuicao).toPromise()
      .then(x => {
        return x
      });
  }

  async SalvarParametrizacaoEmLoteAsync(parametrizacao: any): Promise<any> {
    let url = `${this.baseUrl}/salvar-parametrizacao-em-lote`
    return await this.http.post<any>(url, parametrizacao).toPromise()
      .then(x => {
        return x
      });
  }

  async downloadLog(): Promise<void> {
    try {
      let newUrl = `${this.baseUrl}/download-arquivos-log`
      this.exportarService.baixarArquivo(newUrl)
    } catch (error) {
      return Promise.reject(error);
    }
  }

  async downloadLista(CodEstado: string, CodComarca: number, CodigosVara: string, CodTipoProcesso: number, CodEmpresaCentralizadora: number, CodProfissional: number, ordem: string, asc: boolean): Promise<void> {
    try {
      let newUrl = `${this.baseUrl}/download-lista-parametrizacao?CodEstado=${CodEstado}&CodComarca=${CodComarca}&Codigos=${CodigosVara}&CodTipoProcesso=${CodTipoProcesso}&CodEmpresaCentralizadora=${CodEmpresaCentralizadora}&CodProfissional=${CodProfissional}&ordem=${ordem}&asc=${asc}`
      this.exportarService.baixarArquivo(newUrl)
    } catch (error) {
      return Promise.reject(error);
    }
  }
  //#endregion

  //#region COMBOS

  async obterEmpresasCentralizadora(): Promise<any[]> {
    let newUrl = `${this.baseUrl}/obter-empresas-centralizadoras`
    return await this.http.get<any[]>(newUrl).toPromise().then(r => {
      return r;
    }).catch(error => {
      return Promise.reject(error);
    });
  }

  async obterComarca(uf: string): Promise<any[]> {
    let newUrl = `${this.baseUrl}/obter-comarcas?uf=${uf}`
    return await this.http.get<any[]>(newUrl).toPromise().then(r => {
      return r;
    }).catch(error => {
      return Promise.reject(error);
    });
  }

  async obterVara(codComarca?: number, natureza?: number): Promise<any[]> {
    let newUrl = `${this.baseUrl}/obter-varas`
    if (codComarca && natureza)
      newUrl += `?codComarca=${codComarca}&codTipoProcesso=${natureza}`
    return await this.http.get<any[]>(newUrl).toPromise().then(r => {
      return r;
    }).catch(error => {
      return Promise.reject(error);
    });
  }

  async obterSolicitantes(): Promise<any[]> {
    let newUrl = `${this.baseUrl}/obter-solicitantes`
    try {
      return await this.http.get<any[]>(newUrl).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async obterEscritorios(naturezaList: number[]): Promise<any[]> {
    let newUrl = `${this.baseUrl}/obter-escritorios`
    naturezaList.forEach((natureza, index) => {
      if (index === 0) {
        newUrl += `?codTipoProcesso=${natureza}`
      } else {
        newUrl += `&codTipoProcesso=${natureza}`
      }
    })
    try {
      return await this.http.get<any[]>(newUrl).toPromise()
    } catch (error) {
      throw error;
    }
  }

  async obterUf(): Promise<any[]> {
    let newUrl = `${this.baseUrl}/obter-uf`
    return await this.http.get<any[]>(newUrl).toPromise().then(r => {
      return r;
    }).catch(error => {
      return Promise.reject(error);
    });
  }

  async obterNatureza(): Promise<any[]> {
    let newUrl = `${this.baseUrl}/obter-natureza`
    return await this.http.get<any[]>(newUrl).toPromise().then(r => {
      return r;
    }).catch(error => {
      return Promise.reject(error);
    });
  }

  //#endregion

  //#region ANEXO

  async obterListaAnexoAsync(codParamDistribuicao: number, listaCodAnexos: Array<number>): Promise<Array<ListarAnexosResponse>> {
    let newUrl = `${this.baseUrl}/listar-anexos/${codParamDistribuicao}`
    return await this.http.post<Array<ListarAnexosResponse>>(newUrl, listaCodAnexos).toPromise();
  }

  async incluirAnexoAsync(codParamDistribuicao: number, comentario: string, arquivo: File): Promise<any> {
    let newUrl = this.baseUrl + `/upload/incluir-anexo/${codParamDistribuicao}`
    const form = new FormData();
    form.append('comentario', comentario);
    form.append('arquivo', arquivo, arquivo.name);

    // IMPORTANTE TER O UPLOAD NA ROTA PRA PODER FAZER O TRATAMENTO CORRETO PARA TRABALHAR COM ENVIO DE .ZIP PARA O BACKEND
    return await this.http.post<any>(newUrl, form).toPromise();
  }

  async excluirAnexoAsync(codParamDistribuicao: number, idAnexoDistEscritorio: number): Promise<any> {
    let newUrl = this.baseUrl + `/excluir-anexo/${codParamDistribuicao}/${idAnexoDistEscritorio}`
    return await this.http.delete<any>(newUrl).toPromise();
  }

  async downloadArquivoZip(id: number): Promise<void> {
    try {
      let newUrl = `${this.baseUrl}/download-anexo/${id}`
      this.exportarService.baixarArquivo(newUrl)
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async downloadArquivosZip(listaId: number[]): Promise<void> {
    try {
      let newUrl = `${this.baseUrl}/download-todos-anexos?`
      listaId.forEach((id, index) => {
        newUrl += `${index === 0 ? '' : '&'}idAnexos=${id}`;
      })
      this.exportarService.baixarArquivo(newUrl)
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  //#endregion

}