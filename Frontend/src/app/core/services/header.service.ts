import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { CardMenu } from '@shared/models/card-menu.model';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HeaderService {
  constructor(
    private http: HttpClient
  ) { }

  private headerVisible$: BehaviorSubject<boolean> = new BehaviorSubject(true);
  get headerVisible(): boolean {
    return this.headerVisible$.value;
  }

  headerVisibilityChange(): Observable<boolean> {
    return this.headerVisible$;
  }

  setHeaderVisibility(visibility: boolean): void {
    this.headerVisible$.next(visibility);
  }

  getMenu(): Observable<any>{
    return this.http.get(`${environment.api_v2_url}/Menu/ItemsMenu`);
  }

  getMenuPesquisar(body: any){
    return this.http.post(`${environment.api_v2_url}/Menu/pesquisar`, `'${body}'`);
  }
}
