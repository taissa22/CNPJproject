import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpErrorResult } from '@core/http';
import { environment } from '@environment';
import { DeduçoesDependente } from '@esocial/models/subgrupos/v1_2/deducoes-dependente';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class DeducoesDependenteService {
  constructor(
    private http: HttpClient
  ) {}

  private readonly urlBase = `${environment.api_v2_url}/api/esocial/formulario/ESocialF2501/`;

  private readonly href: string = `${this.urlBase}lista/deddepen/`;
  private readonly hrefAlteracao: string = `${this.urlBase}alteracao/deddepen/`;
  private readonly hrefInclusao: string = `${this.urlBase}inclusao/deddepen/`;

  private readonly hrefExclusao: string = `${this.urlBase}exclusao/deddepen/`;

  public obterPaginado(
    codigoInfocrirrf: number,
    pagina: number,
    coluna: string,
    ascendente: boolean
  ): Promise<{ total: number; lista: Array<DeduçoesDependente> }> {
    let url: string = `${this.href}${codigoInfocrirrf}?pagina=${pagina}&coluna=${coluna}&ascendente=${ascendente}`;

    return new Promise<{ total: number; lista: Array<DeduçoesDependente> }>(
      (resolve, reject) => {
        this.http
          .get<{ lista: DeduçoesDependente[]; total: number }>(url)
          .pipe(
            map(x => {
              return {
                total: x.total,
                lista: x.lista.map(e => DeduçoesDependente.fromObj(e))
              };
            })
          )
          .subscribe(
            data => {
              resolve(data);
            },
            error => {
              reject(error);
            }
          );
      }
    );
  }

  async incluir(formularioId: number, idInfocrirrf: number, obj: any): Promise<void> {
    await this.http.post(`${this.hrefInclusao}${formularioId}/${idInfocrirrf}`, obj).toPromise();
  }

  async alterar(formularioId: number, iddeddepen: number, idInfocrirrf: number, obj: any): Promise<any> {
    const url = `${this.hrefAlteracao}${formularioId}/${idInfocrirrf}/${iddeddepen}`;
    return await this.http.put<any>(url, obj).toPromise();
  }

  async excluir(id: number, formularioId: number, idInfocrirrf: number): Promise<void> {
    try {
      await this.http
        .delete(`${this.hrefExclusao}${formularioId}/${idInfocrirrf}/${id}`)
        .toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }
}
