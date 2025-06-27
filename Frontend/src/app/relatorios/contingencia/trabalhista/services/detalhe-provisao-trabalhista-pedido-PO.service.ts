import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpErrorResult } from '@core/http';
import { environment } from '@environment';
import { DetalhesEmpresaPo } from '@relatorios/models/detalhe-provisao-trabalhista-empresa-po';
import { DetalheProvisaoTrabalhistaPedidoPo } from '@relatorios/models/detalhe-provisao-trabalhista-pedido-po';
import { TotalDetalhesPo } from '@relatorios/models/detalhe-provisao-trabalhista-total-po';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DetalheProvisaoTrabalhistaPedidoPOService {

  private readonly href: string = `${environment.api_url}/relatorio-contigencia-provisao-trabalhista-possivel`;

  constructor(private http: HttpClient, private exportarService: TransferenciaArquivos) { }

  dadosDetalhes(id: string, pagina?: number): Observable<DetalheProvisaoTrabalhistaPedidoPo[]> {
    return this.http.get<DetalheProvisaoTrabalhistaPedidoPo[]>(`${this.href}/obter-detalhe?idItemProvisaoPO=${id}&pagina=${pagina}`);
  }

  dadosTotais(id: string): Promise<TotalDetalhesPo> {
    return this.http.get<TotalDetalhesPo>(`${this.href}/obter-totais?idItemProvisaoPO=${id}`).toPromise();
  }

  dadosCabecalho(id: string): Promise<DetalhesEmpresaPo> {
    return this.http.get<DetalhesEmpresaPo>(`${this.href}/obter-cabecalho?idItemProvisaoPO=${id}`).toPromise();
  }

  possuiHibrido(id: string): Promise<string> {
    return this.http.get<string>(`${this.href}/possui-hibrido?idItemProvisaoPO=${id}`).toPromise();
  }

  async exportar(id: string): Promise<void> {
    try {
      const response: any = await this.http.get(`${this.href}/exportar?idItemProvisaoPO=${id}`, this.exportarService.downloadOptions).toPromise();
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
