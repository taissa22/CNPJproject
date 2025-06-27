import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { ParteProcesso } from '../../models/parte-processo';
import { PesquisaProcesso } from '../../models/pesquisa-processo';

@Injectable({
  providedIn: 'root'
})
export class ESocialParteProcessoService {

  constructor(
    private http: HttpClient,
    private exportarService: TransferenciaArquivos) { }

  private readonly href: string = `${environment.api_v2_url}/api/esocial/v1/ESocialPartesProcesso`;

  async obterPaginadoAsync(
    codigoInterno: number,
    pagina: number,
    quantidade: number,
    statusReclamente: number,
    statusFormulario: number,
    filtro: any
  ): Promise<{ total: number, lista: Array<ParteProcesso> }> {
        let url: string = `${this.href}/lista/${codigoInterno}?pagina=${pagina}&quantidade=${quantidade}`;

    if (statusFormulario != null) url += `&statusReclamente=${statusFormulario}`
    if (statusReclamente != null) url += `&statusFormulario=${statusReclamente}`
    if (filtro != null) url += `&filtro=${filtro}`

    const resposta = await this.http.get<{ lista: ParteProcesso[], total: number }>(url).toPromise();
    if (resposta) {
      return {
        total: resposta.total,
        lista: resposta.lista.map(parte => ParteProcesso.fromObj(parte))
      }
    }
  }

  async obterProcessoAsync(codigoInterno: string, campoProcesso: string, criterioBuscaProcesso: string): Promise<Array<PesquisaProcesso>> {
    let url: string = `${this.href}/consulta-processo/${codigoInterno}/${campoProcesso}/${criterioBuscaProcesso}`;
    const resposta = await this.http.get<Array<PesquisaProcesso>>(url).toPromise();
    if (resposta.length > 0) {
      return resposta;
    }
  }

  async alterarStatusAsync(codigoInterno: number, codigoProcesso: number, codigoStatus: number): Promise<any> {
    let url: string = `${this.href}/altera-status/${codigoInterno}/${codigoProcesso}/${codigoStatus}`;
    return this.http.post<any>(url, null).toPromise();
  }

  async retificarF2500Async(codigoInterno: number): Promise<any> {
    try {
      let url: string = `${this.href}/retificar/f2500/${codigoInterno}`;
      return this.http.post<any>(url, null).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async removerF2500Async(codigoInterno: number): Promise<void> {
    try {
      let url: string = `${this.href}/remover/f2500/${codigoInterno}`;
      return this.http.delete<void>(url).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async excluirF2500Async(idF2500: number) {
    try {
      let url: string = `${this.href}/excluir/f2500/${idF2500}`;
      return this.http.delete<void>(url).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async desfazerExclusaoF2500Async(idF2500: number) {
    try {
      let url: string = `${this.href}/desfazer-exclusao/f2500/${idF2500}`;
      return this.http.patch(url, null).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async criarNovoF2500Async(codigoInterno: number, codigoParte: number) {
    try {
      let url: string = `${this.href}/novo/f2500/${codigoInterno}/${codigoParte}`;
      return this.http.post<void>(url, null).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async adicionarF2501Async(codigoInterno: number, codigoParte: number) {
    try {
      let url: string = `${this.href}/incluir/f2501/${codigoInterno}/${codigoParte}`;
      return this.http.post<void>(url, null).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async removerF2501Async(codigoInterno: number): Promise<void> {
    try {
      let url: string = `${this.href}/remover/f2501/${codigoInterno}`;
      return this.http.delete<void>(url).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async retificarF2501Async(codigoInterno: number): Promise<any> {
    try {
      let url: string = `${this.href}/retificar/f2501/${codigoInterno}`;
      return this.http.post<any>(url, null).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async excluirF2501Async(idF2501: number) {
    try {
      let url: string = `${this.href}/excluir/f2501/${idF2501}`;
      return this.http.delete<void>(url).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async desfazerExclusaoF2501Async(idF2501: number) {
    try {
      let url: string = `${this.href}/desfazer-exclusao/f2501/${idF2501}`;
      return this.http.patch<void>(url,null).toPromise();
    } catch (error) {
      throw error;
    }
  }
  
  async exportarRetorno2500Async(codFormulario: number): Promise<void> {
    try {
      const newUrl = `${this.href}/exportar-retorno/f2500?codigoFormulario=${codFormulario}`;
      return await this.exportarService.baixarArquivo(newUrl);
    } catch (error) {
      throw error;
    }
  }

  async exportarRetorno2501Async(codFormulario: number): Promise<void> {
    try {
      const newUrl = `${this.href}/exportar-retorno/f2501?codigoFormulario=${codFormulario}`;
      return await this.exportarService.baixarArquivo(newUrl);
    } catch (error) {
      throw error;
    }
  }

}
