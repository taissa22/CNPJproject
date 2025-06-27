import { isNullOrUndefined } from 'util';
import { BuscarService } from '@shared/services/buscar.service';
import { EmpresaDoGrupo } from '@shared/models/empresa-do-grupo.model';
import { HttpErrorResult } from './../../../../../core/http/http-error-result';
import { environment } from './../../../../../../environments/environment';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class EmpresaDoGrupoService {
  constructor(private api: HttpClient) { }

  private url(endpoint: string): string {
    return `${environment.api_url}/${endpoint}`;
  }

  async obter(): Promise<Array<EmpresaDoGrupo>> {
    try {
      const url = this.url(`agenda-de-audiencias-do-civel-estrategico/empresas-do-grupo`);
      return await this.api.get<Array<EmpresaDoGrupo>>(url).toPromise();
    } catch (error) {
      const httpError: HttpErrorResponse = error;
      if (Array.isArray(httpError.error)) {
        throw new HttpErrorResult(httpError.error);
      } else {
        throw new HttpErrorResult(['Erro desconhecido']);
      }
    }
  }

  // tslint:disable-next-line: max-line-length
  async obterPaginado(pagina: number, quantidade: number, empresaDoGrupoId?: number): Promise<{ total: number, data: Array<{ id: number, nome: string }> }> {
    try {
      // tslint:disable-next-line: max-line-length
      let url = this.url(`agenda-de-audiencias-do-civel-estrategico/empresas-do-grupo/dropdown?pagina=${pagina}&quantidade=${quantidade}`);
      if (!isNullOrUndefined(empresaDoGrupoId)) {
        url += `&empresaDoGrupoId=${empresaDoGrupoId}`;
      } else {
        url += `&empresaDoGrupoId=0`;
      }
      return await this.api.get<{ total: number, data: Array<{ id: number, nome: string }> }>(url)
        .pipe(map(x => {
          return {
            total: x.total,
            data: x.data.map(r => ({ id: r.id, nome: r.nome }))
          };
        })).toPromise();
    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }
  }
}
