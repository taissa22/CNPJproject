import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "@environment";
import { ComarcaBB } from "@manutencao/models/comarca-bb.model";

@Injectable({
    providedIn : 'root'
})
export class ComarcaBBService{
    private urlBase: string = environment.api_url + '/manutencao/comarcabb';
   
    constructor(private http: HttpClient) { }

    public ObterPorEstado(estadoId:string){
        return this.http.get<ComarcaBB[]>(this.urlBase+"?estadoId="+estadoId);
    }

}