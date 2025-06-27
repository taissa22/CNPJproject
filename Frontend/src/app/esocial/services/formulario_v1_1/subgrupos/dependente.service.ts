import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpErrorResult } from '@core/http';
import { environment } from '@environment';
import { map } from 'rxjs/operators';
import { Dependente } from '../../../models/subgrupos/dependente';

@Injectable({
  providedIn: 'root'
})
export class DependenteService {
  constructor(
    private http: HttpClient
  ) {}

  private readonly urlBase = `${environment.api_v2_url}/api/esocial/v1_1/ESocialF2500/`;

  private readonly href: string = `${this.urlBase}lista/dependente/`;
  private readonly hrefAlteracao: string = `${this.urlBase}alteracao/dependente/`;
  private readonly hrefInclusao: string = `${this.urlBase}inclusao/dependente/`;

  private readonly hrefExclusao: string = `${this.urlBase}exclusao/dependente/`;

  public obterPaginado(
    codigoFormulario: number,
    pagina: number,
    coluna: string,
    ascendente: boolean
  ): Promise<{ total: number; lista: Array<Dependente> }> {
    let url: string = `${this.href}${codigoFormulario}?pagina=${pagina}&coluna=${coluna}&ascendente=${ascendente}`;

    return new Promise<{ total: number; lista: Array<Dependente> }>(
      (resolve, reject) => {
        this.http
          .get<{ lista: Dependente[]; total: number }>(url)
          .pipe(
            map(x => {
              return {
                total: x.total,
                lista: x.lista.map(e => Dependente.fromObj(e))
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

  async incluir(obj: any): Promise<void> {
    await this.http.post(`${this.hrefInclusao}${obj.idF2500}`, obj).toPromise();
  }

  async alterar(categora: Dependente): Promise<any> {
    const url = `${this.hrefAlteracao}${categora.idF2500}/${categora.idEsF2500Dependente}`;
    return await this.http.put<any>(url, categora).toPromise();
  }

  async excluir(id: number, formularioId: number): Promise<void> {
    try {
      await this.http
        .delete(`${this.hrefExclusao}${formularioId}/${id}`)
        .toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }
}
