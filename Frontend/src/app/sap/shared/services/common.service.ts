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
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { ICommonService } from '../interfaces/icommon-service';

/**
 * Service com os métodos padrões de CRUD e exportação.
 * @param TModel: Modelo que irá ser a referência do service.
 * @param TType: Tipo de dado que representa a chave primária do modelo do service.
 */
export class CommonService<TModel, TType> implements ICommonService<TModel, TType> {

  /**
   * @description Método construtor padrão do serviço. Deve ser construído com o service da API que se deseja utilizar
   * @param service Service de API que será utilizado.
   */
  constructor(protected service: IBaseService<TModel, TType>) { }

  //#region CRUD
  /**
   * @description Realiza a exclusão de um item enviando para o back através de seu id.
   * @param id id do item.
   * @returns Behavior Subject com o notifier da exclusão.
   */
  excluir(id: TType): Observable<any> {
    return this.service.excluir(id).pipe(take(1));
  }

  /**
   * @description Realiza o cadastro de um item enviando para o back o seu modelo declarado na classe.
   * @param model O item à ser cadastrado
   * @returns Behavior Subject com o notifier do cadastro.
   */
  cadastrar(model: TModel): Observable<any> {
    return this.service.cadastrar(model).pipe(take(1));
  }

  /**
   * @description Realiza a edição de um item enviando para o back o seu modelo declarado na classe.
   * @param model O item à ser cadastrado
   * @returns Behavior Subject com o notifier da edição.
   */
  editar(model: TModel): Observable<any> {
    return this.service.editar(model).pipe(take(1));
  }

  /**
   * @description Realiza a consulta do item pelo id dele.
   * @param id: id do item para consulta.
   * @returns Behavior Subject com o notifier da consulta.
   */
  consultar(id: TType): Observable<any> {
    return this.service.consultar(id)
      .pipe(take(1));
  }

  /**
   * @description Realiza a consulta de todos itens da consulta.
   * @returns Behavior Subject com o notifier da consulta.
   */
  consultarTodos(): Observable<any> {
    return this.service.consultarTodos()
      .pipe(take(1));
  }

  /**
   * @description Realiza a consulta de todos itens da consulta.
   * @param filtros: Filtros à serem utilizados na consulta.
   * @returns Behavior Subject com o notifier da consulta.
   */
  consultarPorFiltros(filtros: any): Observable<any> {
    return this.service.consultarPorFiltros(filtros)
      .pipe(take(1));
  }

  //#endregion CRUD

  exportar(nomeArquivo: string): Promise<any> {
    return this.service.exportar(nomeArquivo);
  }

  exportarPorFiltro(nomeArquivo: string, json: any): Promise<any> {
    return this.service.exportarPorFiltro(nomeArquivo, json);
  }
}
