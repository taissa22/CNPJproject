import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "@environment";
import { TribunalBB } from "@manutencao/models/tribunal-bb.model";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";

@Injectable({
    providedIn: "root"
})
export class TribunalBBService {

    private urlBase: string = environment.api_url + '/manutencao/tribunal-bb';
    constructor(private http: HttpClient) {

    }

    public obterTodos(): Observable<Array<TribunalBB>> {
        return this.http.get<TribunalBB[]>(this.urlBase)
            .pipe(
                map(x => {
                    return x.map(e => TribunalBB.fromObj(e));
                })
            );
    }


}