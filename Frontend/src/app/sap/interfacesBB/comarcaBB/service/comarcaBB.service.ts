import { ManutencaoCommonService } from 'src/app/sap/shared/services/manutencao-common.service';
import { ComarcaBBDTO } from './../../../../shared/interfaces/comarca-BB-DTO';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { OrdenacaoStatus } from '@shared/interfaces/ordenacao-status';

import { InterfaceBBComarcaBBendpointService } from '../../../../core/services/sap/interfaceBBComarcaBBendpoint.service';

@Injectable({
  providedIn: 'root'
})
export class ComarcaBBService  extends ManutencaoCommonService<ComarcaBBDTO, number>{

  constructor(protected  service: InterfaceBBComarcaBBendpointService) {
    super(service);
  }

  

  
  // public comarcaBBSubject = new BehaviorSubject<[]>([]);
  // public filtroSubject = new BehaviorSubject({
  //   pagina: 1,
  //   quantidade: 8,
  //   total: 0,
  //   ordenacao: '',
  //   ascendente: true
  // });
  // public paginaSubject = new BehaviorSubject<number>(1);
  // public quantidadeSubject = new BehaviorSubject<number>(8);
  // public totalSubject = new BehaviorSubject<number>(0);
  // public ordenacaoSubject = new BehaviorSubject<string>('');
  // public ascendenteSubject = new BehaviorSubject<boolean>(true);
  // public minimoItensSubject = new BehaviorSubject<number>(0);
  // public maximoItensSubject = new BehaviorSubject<number>(0);
  // public headerSubject = new BehaviorSubject<string>('');
  // public ordenacaoActivitySubject = new BehaviorSubject<Array<OrdenacaoStatus>>([]);
  // public selectedComarcaBBSubject = new BehaviorSubject(null);

  // constructor(private service: InterfacesBBComarcaBBService) { }

  // updateComarcaBB(comarcaBB: []) {
  //   this.comarcaBBSubject.next(comarcaBB);
  // }

  // /**
  //  * Atualiza a quantidade de itens da página atual.
  //  */
  // public updateItemCount() {
  //   const currentPage = this.paginaSubject.value;

  //   // Valor máximo de itens na página
  //   const currentItemMax = currentPage * this.quantidadeSubject.value;

  //   // Valor mínimo de itens na página
  //   const currentItemMin = currentItemMax - this.quantidadeSubject.value;

  //   this.minimoItensSubject.next(currentItemMin);
  //   this.maximoItensSubject.next(currentItemMax);
  // }

  // /**
  //  * Adiciona um novo item no subject "ordenacaoActivity"
  //  * @param obj Item à ser adicionado
  //  */
  // public pushOrdenacaoActivity(obj: OrdenacaoStatus) {
  //   const currentOrdenacaoActivity = this.ordenacaoActivitySubject.value;
  //   currentOrdenacaoActivity.push(obj);
  //   this.ordenacaoActivitySubject.next(currentOrdenacaoActivity);
  // }

  // /**
  //  * Atualiza o total de itens da grid
  //  * @param total Quantidade total vinda da API
  //  */
  // public updateTotal(total: number) {
  //   this.totalSubject.next(total);
  // }

  // /**
  //  * Atualiza a página atual
  //  * @param pagina página atual
  //  */
  // public updatePagina(pagina: number) {
  //   this.paginaSubject.next(pagina);
  // }

  // /**
  //  * Atualiza a quantidade de itens por página atual.
  //  * @param quantidade Quantidade de itens por página
  //  */
  // public updateQuantidade(quantidade: number) {
  //   this.quantidadeSubject.next(quantidade);
  // }

  // /**
  //  * Atualiza uma atividade do botão de ordenação
  //  * @param header Coluna do botão de ordenação
  //  * @param active Se deve estar ativo ou não
  //  */
  // public updateOrdenacaoActivity(header: string, active: boolean) {
  //   const index = this.ordenacaoActivitySubject.value.findIndex(e => e.key == header);
  //   if (index == -1) {
  //     throw new Error('Header não encontrado na instância de ordenacaoActivitySubject');
  //   }
  //   const ordenacaoActivity = this.ordenacaoActivitySubject.value;
  //   ordenacaoActivity[index].isActive = active;
  //   this.ordenacaoActivitySubject.next(ordenacaoActivity);
  // }

  // public updateHeader(header: string) {
  //   this.headerSubject.next(header);
  // }

  // public updateOrdenacao(campo, isAscendente) {
  //   this.ordenacaoSubject.next(campo);
  //   this.ascendenteSubject.next(isAscendente);
  // }

  // public updateFiltro(filtro) {
  //   this.filtroSubject.next(filtro);
  // }

  // public isOrdenacaoActive(header) {
  //   const activityObjectIndex = this.ordenacaoActivitySubject.value.findIndex(e => e.key == header);

  //   if (activityObjectIndex == -1) {
  //     return false;
  //   }
  //   const activityObject = this.ordenacaoActivitySubject.value[activityObjectIndex];
  //   if (activityObject) {
  //     return activityObject.isActive;
  //   }
  //   return false;
  // }

  // getComarcaBB() : Observable<[]> {
  //   this.updateFiltro({
  //     pagina: this.paginaSubject.value,
  //     quantidade: this.quantidadeSubject.value,
  //     total: this.totalSubject.value,
  //     ordenacao: this.ordenacaoSubject.value,
  //     ascendente: this.ascendenteSubject.value
  //   });
  //   const comarcaBB = this.service.getComarcaBB(this.filtroSubject.value);
  //   comarcaBB.subscribe(response => {
  //     this.updateTotal(response['total']);
  //     this.updateComarcaBB(response['data']);
  //   });

  //   return this.comarcaBBSubject.asObservable();
  // }





  // private updatecomarcaBB(comarcaBB: []) {
  //   this.comarcaBBSubject.next(comarcaBB);
  // }



}
