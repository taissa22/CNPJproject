import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpErrorResult } from '@core/http';
import { environment } from '@environment';
import { DetalhesEmpresa } from '@relatorios/models/detalhe-provisao-trabalhista-empresa';
import { DetalheProvisaoTrabalhistaPedidoModel } from '@relatorios/models/detalhe-provisao-trabalhista-pedido';
import { totalDetalhes } from '@relatorios/models/detalhe-provisao-trabalhista-total';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DetalheProvisaoTrabalhistaPedidoPRService {

  private readonly href: string = `${environment.api_url}/relatorio-contigencia-provisao-trabalhista-provavel`;

  constructor(private http: HttpClient, private exportarService: TransferenciaArquivos) { }

  dadosDetalhes(id: string, pagina?: number): Observable<DetalheProvisaoTrabalhistaPedidoModel[]> {
    return this.http.get<DetalheProvisaoTrabalhistaPedidoModel[]>(`${this.href}/obter-detalhe?idItemProvisaoPR=${id}&pagina=${pagina}`);
  }

  dadosTotais(id: string): Promise<totalDetalhes> {
    return this.http.get<totalDetalhes>(`${this.href}/obter-totais?idItemProvisaoPR=${id}`).toPromise();
  }

  dadosCabecalho(id: string): Promise<DetalhesEmpresa> {
    return this.http.get<DetalhesEmpresa>(`${this.href}/obter-cabecalho?idItemProvisaoPR=${id}`).toPromise();
  }

  possuiHibrido(id: string): Promise<string> {
    return this.http.get<string>(`${this.href}/possui-hibrido?idItemProvisaoPR=${id}`).toPromise();
  }

  async exportar(id: string): Promise<void> {
    try {
      const response: any = await this.http.get(`${this.href}/exportar?idItemProvisaoPR=${id}`, this.exportarService.downloadOptions).toPromise();
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
