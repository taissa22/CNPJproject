import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpErrorResult } from '@core/http';
import { environment } from '@environment';
import { PenAlim } from '@esocial/models/subgrupos/v1_2/pen-alim';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PenAlimService {
  constructor(
    private http: HttpClient
  ) {}

  private readonly urlBase = `${environment.api_v2_url}/api/esocial/formulario/ESocialF2501/`;

  private readonly href: string = `${this.urlBase}lista/penalim/`;
  private readonly hrefConsultar: string = `${this.urlBase}consulta/penalim/`;
  private readonly hrefAlteracao: string = `${this.urlBase}alteracao/penalim/`;
  private readonly hrefInclusao: string = `${this.urlBase}inclusao/penalim/`;

  private readonly hrefExclusao: string = `${this.urlBase}exclusao/penalim/`;

  async obterPenAlim(codigoInfocrirrf: number, codigoPenAlim: number): Promise<PenAlim> {
    let url: string = `${this.hrefConsultar}${codigoInfocrirrf}/${codigoPenAlim}`;
    return await this.http.get<PenAlim>(url).toPromise();
  }

  public obterPaginado(
    codigoInfocrirrf: number,
    pagina: number,
    coluna: string,
    ascendente: boolean
  ): Promise<{ total: number; lista: Array<PenAlim> }> {
    let url: string = `${this.href}${codigoInfocrirrf}?pagina=${pagina}&coluna=${coluna}&ascendente=${ascendente}`;

    return new Promise<{ total: number; lista: Array<PenAlim> }>(
      (resolve, reject) => {
        this.http
          .get<{ lista: PenAlim[]; total: number }>(url)
          .pipe(
            map(x => {
              return {
                total: x.total,
                lista: x.lista.map(e => PenAlim.fromObj(e))
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
