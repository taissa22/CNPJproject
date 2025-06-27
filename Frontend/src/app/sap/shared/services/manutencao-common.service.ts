/** O criador deste código não está mais entre nós
 * Cuide deste código com carinho
 *
 *         _.---,._,'
       /' _.--.<
         /'     `'
       /' _.---._____
       \.'   ___, .-'`
           /'    \\             .
         /'       `-.          -|-
        |                       |
        |                   .-'~~~`-.
        |                 .'         `.
        |                 |  R  I  P  |
  ADS   |                 |           |
        |                 |           |
         \              \\|           |//
   ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

   04/09/2020
 */

import { IBaseService } from '@shared/interfaces/ibase-service';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { APIResponse } from '../interfaces/apiresponse';
import { IManutencaoCommonService } from '../interfaces/imanutencao-common-service';
import { IOrderingService } from '../interfaces/iordering-service';
import { IPaginatorService } from '../interfaces/ipaginator-service';
import { IQuantityService } from '../interfaces/iquantity-service';
import { CommonService } from './common.service';
import { DefaultOrderingService } from './default-ordering.service';
import { DefaultPaginatorService } from './default-paginator.service';
import { DefaultQuantityService } from './default-quantity.service';

/**
 * @description Service genérico para as telas de manutenção.
 * Realiza a implementação de todo o CRUD (Classe CommonService)
 * e toda a lógica de ordenação/paginação da página.
 *
 * O service deve ser injetado e protected para ser acessível em todas as classes.
 *
 * @param TModel: Modelo que irá ser a referência do service.
 * @param TType: Tipo de dado que representa a chave primária do modelo do service.
 */
export class ManutencaoCommonService<TModel, TType> extends CommonService<TModel, TType> implements IManutencaoCommonService {

  public filtrosSubject = new BehaviorSubject<object>({});
  public totalSubject = new BehaviorSubject<number>(0);
  public consultaSubject = new BehaviorSubject<APIResponse>({
    mensagem: '',
    sucesso: false,
    data: [],
    exibeNotificacao: false
  });

  public get quantidadePorPagina(): number {
    return this.quantityService.quantidadeSubject.value;
  }
  public set quantidadePorPagina(v: number) {
    this.quantityService.setQuantidade(v);
    this.consultarPorFiltros(this.filtrosSubject.value);
  }

  public get paginaAtual(): number {
    return this.paginatorService.pageSubject.value;
  }
  public set paginaAtual(v: number) {
    this.paginatorService.setPage(v);
    this.consultarPorFiltros(this.filtrosSubject.value);
  }


  constructor(protected service: IBaseService<TModel, TType>,
    protected paginatorService: IPaginatorService = new DefaultPaginatorService(),
    protected quantityService: IQuantityService = new DefaultQuantityService(),
    protected orderingService: IOrderingService = new DefaultOrderingService()) {
    super(service);
  }
  // refreshFiltroSemQuantidade(args: object): void {
  //   throw new Error('Method not implemented.');
  // }

  protected trataResposta(response) {
    if (response.hasOwnProperty('sucesso') &&
      response['sucesso'] &&
      response.hasOwnProperty('total'))
      this.setTotal(response['total']);
    else {
      this.setTotal(0);
    }
    this.consultaSubject.next({ ...response });
    return response;
  }

  protected setTotal(total: number) {
    this.paginatorService.setMaxPage(Math.ceil(total / this.quantityService.quantidadeSubject.value));
    this.totalSubject.next(total);
  }

  public refreshFiltros(args: object) {
    this.paginatorService.setPage(1);
    this.quantityService.setQuantidade(8);
    if (args.hasOwnProperty('ascendente'))
      this.orderingService.setAscencao(args['ascendente']);
    if (args.hasOwnProperty('ordenacao'))
      this.orderingService.setCampo(args['ordenacao']);
  }


  public refreshFiltroSemQuantidade(args: object) {
    this.paginatorService.setPage(1);
    if (args.hasOwnProperty('ascendente'))
      this.orderingService.setAscencao(args['ascendente']);
    if (args.hasOwnProperty('ordenacao'))
      this.orderingService.setCampo(args['ordenacao']);
  }

  public consultarPorFiltros(filtros?: any): Observable<any> {
    let filtros$ = {
      ...filtros,
      pagina: this.paginatorService.pageSubject.value,
      quantidade: this.quantityService.quantidadeSubject.value,
      ascendente: this.orderingService.ascendenteSubject.value,
      ordenacao: this.orderingService.campoSubject.value,
      total: this.paginatorService.pageSubject.value == 1 ? 0 : this.totalSubject.value,
    }

    this.filtrosSubject.next({ ...filtros });

    return this.service.consultarPorFiltros(filtros$)
      .pipe(map(response => this.trataResposta(response)));
  }

  public excluir(id: TType) {
    return this.service.excluir(id)
      .pipe(map(excluirResponse => {
        return this.consultarPorFiltros(this.filtrosSubject.value)
          .toPromise()
          .then(consultaResponse => {
            return {
              consultar: consultaResponse,
              excluir: excluirResponse
            }
          })
          .then(response => {
            const consultar = response.consultar;
            const excluir = response.excluir;
            if (consultar.data.length == 0 && excluir['sucesso']) {
              this.paginatorService.setPage(this.paginatorService.pageSubject.value - 1);
              return this.consultarPorFiltros(this.filtrosSubject.value)
                .toPromise()
                .then(consultaResponse => {
                  return {
                    consultar: consultaResponse,
                    excluir: excluir
                  }
                });
            }
            return response;
          });
      }))
  }


  public cadastrar(model: TModel) {
    return this.service.cadastrar(model)
      .pipe(map(cadastrarResponse => {
        return this.consultarPorFiltros(this.filtrosSubject.value)
          .toPromise()
          .then(consultaResponse => {
            return {
              consultar: consultaResponse,
              cadastrar: cadastrarResponse
            }
          });;
      }));
  }

  public editar(model: TModel) {
    return this.service.editar(model).pipe(map(editarResponse => {
      return this.consultarPorFiltros(this.filtrosSubject.value)
        .toPromise()
        .then(consultaResponse => {
          return {
            consultar: consultaResponse,
            editar: editarResponse
          }
        });;
    }));
  }

  public ordenar(campo: string, ascendente: boolean): Observable<any> {
    this.orderingService.setCampo(campo);
    this.orderingService.setAscencao(ascendente);
    return this.consultarPorFiltros(this.filtrosSubject.value);
  }

  public trocarPagina(pagina: number) {
    this.paginatorService.setPage(pagina);
    return this.consultarPorFiltros(this.filtrosSubject.value);
  }

  public trocarQuantidade(quantidade: number) {
    this.quantityService.setQuantidade(quantidade);


  return this.trocarPagina( 1);   // return this.consultarPorFiltros(this.filtrosSubject.value);


  }
}
