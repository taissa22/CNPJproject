import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "@environment";
import { OrgaoBB } from "@manutencao/models/orgao_bb.model";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";

@Injectable({
    providedIn : "root"
})
export class OrgaoBBService{
    
    private urlBase: string = environment.api_url + '/manutencao/orgao-bb';
    
    constructor(private http: HttpClient) {

    }

    public obterPorTribunal(tribunalId: number,comarcaBBId : number): Observable<Array<OrgaoBB>> {
        return this.http.get<OrgaoBB[]>(this.urlBase+"/?TribunalBBId="+tribunalId+"&ComarcaBBId="+comarcaBBId)
            .pipe(
                map(x => {
                    return x.map(e => OrgaoBB.fromObj(e));
                })
            );
    }


}