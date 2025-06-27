import { SapService } from './../../../core/services/sap/sap.service';
import { FilterService } from './filter.service';
import { LoteService } from 'src/app/core/services/sap/lote.service';
import { TipoProcessoEnum } from './../../../shared/enums/tipo-processoEnum.enum';
import { ProcessoService } from './../../../core/services/sap/processo.service';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';



export const headerTabela = ['Numero do Lote'];

export const headerTabelaObj = {
    numeroLote: 'Numero do Lote'
};

@Injectable({
    providedIn: 'root'
})

export class ConsultaNumeroLoteService {


    contagemSubject$ = new BehaviorSubject<number>(0);
    public listaLotes$ = new BehaviorSubject<any[]>([]);

    // valorComboSelecionado$ = new BehaviorSubject<number>(1);

    // get isCodigoInterno() {
    //     if (this.valorComboSelecionado$.value == 2) return true
    //     return false
    // }


    constructor(private endpoint: LoteService, private service: FilterService, private sapService : SapService) { }

    get listaHeaderTabela() {
        return headerTabela;
    }
    get listaHeaderTabelaObj() {
        return headerTabelaObj;
    }

    limpar() {
        this.listaLotes$.next([]);
        this.contagemSubject$.next(0);
        // this.valorComboSelecionado$.next(1);
    }

    /**
     *
     *@description verififica qual o endpoint deve ser chamado para fazer a busca
     * @param {*} busca - valor digitado pelo usuário
     * @param {*} isCodigoInterno - verifica se na combo está selecionado para codigo interno
     * @memberof ProcessosTrabalhistaService
     */
    buscarProcesso(busca, processo) {

        return this.recuperarLote(busca, processo);

    }

    private recuperarLote(busca, processo) {

        return this.endpoint.validarLote(busca, processo).pipe(
            map(res => res)
        ).toPromise();
    }


    public updateProcesoss(array: any[]) {
        this.service.filtro.idsNumerosLote = []
        array.map(e => this.service.filtro.idsNumerosLote.push( e["Numero do Lote"]))
        this.listaLotes$.next(array);
        this.sapService.atualizaCount(array.length, 13);
    }

}
