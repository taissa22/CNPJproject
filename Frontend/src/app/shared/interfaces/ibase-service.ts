import { Observable } from 'rxjs';

export interface IBaseService<TModel, TType> {
    excluir(id: TType): Observable<any>;
    cadastrar(model: TModel): Observable<any>;
    editar(model: TModel): Observable<any>;
    consultar(id: TType): Observable<any>;
    consultarTodos(): Observable<any>;
    consultarPorFiltros(filtros: any): Observable<any>
  exportar(nomeArquivo: string): Promise<any>;
  exportarPorFiltro(nomeArquivo: string, json: any): Promise<any>;
}
