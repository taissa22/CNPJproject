import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpErrorResult } from '@core/http';
import { environment } from '@environment';
import { TelaPerfil } from '@manutencao/models/tela-perfil.model';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class TelaPerfilService {

  constructor(
    private http: HttpClient,
    private exportarService: TransferenciaArquivos
  ) { }

  protected baseUrl: string = `${environment.api_v2_url}/api/Perfil`;
  downloadOptions = { observe: 'response' as 'body', responseType: 'blob' as 'json' };

  obter(pagina: number, qtd: number, filtro: string, status: string, modulo: string, coluna: string, direcao: string): Observable<{ data: Array<TelaPerfil>, quantidade: number }> {
    try {
      let url = this.baseUrl + `/Obter?pagina=${pagina}&quantidade=${qtd}`;
      url = filtro != null ? url + `&filtro=${filtro}&status=${status}&modulo=${modulo}&coluna=${coluna}&direcao=${direcao}` : url + `&status=${status}&modulo=${modulo}&coluna=${coluna}&direcao=${direcao}`

      return this.http.get<{ perfil: Array<TelaPerfil>, total: number }>(url)
        .pipe(map(x => {
          return {
            data: x.perfil,
            quantidade: x.total
          };
        }));
    }
    catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  excluir(codigo: number) {
    try {
      let url = this.baseUrl + `/Excluir?cod=${codigo}`
      return this.http.delete(url);
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  exportar(filtro: string, status: string, modulo: string, coluna: string, direcao: string) {
    try {
      let url = this.baseUrl + `/Exportar?`;
      url = filtro != null ? url + `filtro=${filtro}&status=${status}&modulo=${modulo}&coluna=${coluna}&direcao=${direcao}` : url + `status=${status}&modulo=${modulo}&coluna=${coluna}&direcao=${direcao}`
      this.exportarService.baixarArquivo(url);
    }
    catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }
}
